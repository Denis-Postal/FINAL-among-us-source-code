using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponsMinigame : Minigame
{
	public FloatRange XSpan = new FloatRange(-1.15f, 1.15f);

	public FloatRange YSpan = new FloatRange(-1.15f, 1.15f);

	public FloatRange TimeToSpawn;

	public ObjectPoolBehavior asteroidPool;

	public TextMeshPro ScoreText;

	public SpriteRenderer TargetReticle;

	public LineRenderer TargetLines;

	private Vector3 TargetCenter;

	public Collider2D BackgroundCol;

	public SpriteRenderer Background;

	public Controller myController = new Controller();

	private float Timer;

	public AudioClip ShootSound;

	public AudioClip[] ExplodeSounds;

	public override void Begin(PlayerTask task)
	{
		SetupInput(disableCursor: false);
		base.Begin(task);
		ScoreText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AstDestroyed, MyNormTask.taskStep);
		TimeToSpawn.Next();
	}

	protected override IEnumerator CoAnimateOpen()
	{
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			float num = timer / 0.1f;
			transform.localScale = new Vector3(num, 0.1f, num);
			yield return null;
		}
		for (float timer = 0.010000001f; timer < 0.1f; timer += Time.deltaTime)
		{
			float y = timer / 0.1f;
			transform.localScale = new Vector3(1f, y, 1f);
			yield return null;
		}
		transform.localScale = new Vector3(1f, 1f, 1f);
	}

	protected override IEnumerator CoDestroySelf()
	{
		for (float timer = 0.010000001f; timer < 0.1f; timer += Time.deltaTime)
		{
			float y = 1f - timer / 0.1f;
			transform.localScale = new Vector3(1f, y, 1f);
			yield return null;
		}
		for (float timer = 0f; timer < 0.1f; timer += Time.deltaTime)
		{
			float num = 1f - timer / 0.1f;
			transform.localScale = new Vector3(num, 0.1f, num);
			yield return null;
		}
		Object.Destroy(gameObject);
	}

	public void FixedUpdate()
	{
		Background.color = Color.Lerp(Palette.ClearWhite, Color.white, Mathf.Sin(Time.time * 3f) * 0.1f + 0.79999995f);
		if ((bool)MyNormTask && MyNormTask.IsComplete)
		{
			return;
		}
		Timer += Time.fixedDeltaTime;
		if (Timer >= TimeToSpawn.Last)
		{
			Timer = 0f;
			TimeToSpawn.Next();
			if (asteroidPool.InUse < MyNormTask.MaxStep - MyNormTask.TaskStep)
			{
				Asteroid ast = asteroidPool.Get<Asteroid>();
				ast.transform.localPosition = new Vector3(XSpan.max, YSpan.Next(), -1f);
				ast.TargetPosition = new Vector3(XSpan.min, YSpan.Next(), -1f);
				ast.GetComponent<ButtonBehavior>().OnClick.AddListener(() =>
				{
					BreakApart(ast);
				});
			}
		}
		myController.Update();
		bool flag = myController.CheckHover(BackgroundCol);
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			if (flag)
			{
				Vector3 vector = (Vector3)myController.HoverPosition - transform.position;
				vector.z = -2f;
				TargetReticle.transform.localPosition = vector;
				vector.z = 0f;
				TargetLines.SetPosition(1, vector);
			}
			else
			{
				Bounds bounds = BackgroundCol.bounds;
				Vector3 vector2 = myController.HoverPosition;
				vector2.x = Mathf.Clamp(vector2.x, bounds.min.x, bounds.max.x);
				vector2.y = Mathf.Clamp(vector2.y, bounds.min.y, bounds.max.y);
				VirtualCursor.instance.SetWorldPosition(vector2);
				vector2 -= transform.position;
				vector2.z = -2f;
				TargetReticle.transform.localPosition = vector2;
				vector2.z = 0f;
				TargetLines.SetPosition(1, vector2);
			}
		}
		if (myController.CheckDrag(BackgroundCol) == DragState.TouchStart)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ShootSound, loop: false);
			}
			Vector3 vector3 = (Vector3)myController.DragPosition - transform.position;
			vector3.z = -2f;
			TargetReticle.transform.localPosition = vector3;
			vector3.z = 0f;
			TargetLines.SetPosition(1, vector3);
			if ((bool)ShipStatus.Instance.WeaponsImage && !ShipStatus.Instance.WeaponsImage.IsPlaying())
			{
				PlayerControl.LocalPlayer.RpcPlayAnimation(6);
			}
		}
	}

	public void BreakApart(Asteroid ast)
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(ExplodeSounds.Random(), loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
		}
		if (MyNormTask.IsComplete)
		{
			return;
		}
		StartCoroutine(ast.CoBreakApart());
		if ((bool)MyNormTask)
		{
			MyNormTask.NextStep();
			ScoreText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AstDestroyed, MyNormTask.taskStep);
		}
		if (!MyNormTask || !MyNormTask.IsComplete)
		{
			return;
		}
		StartCoroutine(CoStartClose());
		foreach (Asteroid activeChild in asteroidPool.activeChildren)
		{
			if (!(activeChild == ast))
			{
				StartCoroutine(activeChild.CoBreakApart());
			}
		}
	}
}
