using System.Collections;
using System.Linq;
using Rewired;
using UnityEngine;

public class EmptyGarbageMinigame : Minigame
{
	private const float GrinderVolume = 0.8f;

	public FloatRange HandleRange = new FloatRange(-0.65f, 0.65f);

	public Vector2Range SpawnRange;

	public Collider2D Blocker;

	public AreaEffector2D Popper;

	public Collider2D Handle;

	public SpriteRenderer Bars;

	public Transform VibratePivot;

	private Controller controller = new Controller();

	private bool finished;

	public int NumObjects = 15;

	private SpriteRenderer[] Objects;

	public SpriteRenderer[] GarbagePrefabs;

	public SpriteRenderer[] LeafPrefabs;

	public SpriteRenderer[] SpecialObjectPrefabs;

	public AudioClip LeverDown;

	public AudioClip LeverUp;

	public AudioClip GrinderStart;

	public AudioClip GrinderLoop;

	public AudioClip GrinderEnd;

	private TouchpadBehavior touchpad;

	private bool hadInput;

	private float leverInput;

	private Coroutine popCoroutine;

	private Coroutine animateCoroutine;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		int i = 0;
		Objects = new SpriteRenderer[NumObjects];
		RandomFill<SpriteRenderer> randomFill = new RandomFill<SpriteRenderer>();
		if (MyNormTask.StartAt == SystemTypes.LifeSupp)
		{
			randomFill.Set(GarbagePrefabs.Union(LeafPrefabs));
		}
		else
		{
			NormalPlayerTask myNormTask = MyNormTask;
			if ((object)myNormTask != null && myNormTask.taskStep == 0)
			{
				if (MyNormTask.TaskType == TaskTypes.EmptyChute)
				{
					randomFill.Set(GarbagePrefabs);
				}
				else
				{
					randomFill.Set(LeafPrefabs);
				}
			}
			else
			{
				randomFill.Set(GarbagePrefabs.Union(LeafPrefabs));
				for (; i < SpecialObjectPrefabs.Length; i++)
				{
					SpriteRenderer spriteRenderer = (Objects[i] = Object.Instantiate(SpecialObjectPrefabs[i]));
					spriteRenderer.transform.SetParent(transform);
					spriteRenderer.transform.localPosition = SpawnRange.Next();
				}
			}
		}
		for (; i < Objects.Length; i++)
		{
			SpriteRenderer spriteRenderer2 = (Objects[i] = Object.Instantiate(randomFill.Get()));
			spriteRenderer2.transform.SetParent(transform);
			Vector3 localPosition = SpawnRange.Next();
			localPosition.z = FloatRange.Next(-0.5f, 0.5f);
			spriteRenderer2.transform.localPosition = localPosition;
			spriteRenderer2.color = Color.Lerp(Color.white, Color.black, (localPosition.z + 0.5f) * 0.7f);
		}
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	private void StopCoroutines()
	{
		if (popCoroutine != null)
		{
			StopCoroutine(popCoroutine);
			popCoroutine = null;
		}
		if (animateCoroutine != null)
		{
			StopCoroutine(animateCoroutine);
			animateCoroutine = null;
		}
	}

	public void Update()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		controller.Update();
		Vector3 localPosition = Handle.transform.localPosition;
		float num = HandleRange.ReverseLerp(localPosition.y);
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			if (!finished)
			{
				Player player = ReInput.players.GetPlayer(0);
				if (touchpad.IsTouching())
				{
					leverInput = 0f - touchpad.GetTouchVector().y;
				}
				else
				{
					leverInput = Mathf.Clamp01(0f - player.GetAxis(17));
				}
				localPosition.y = HandleRange.Lerp(1f - leverInput);
				num = HandleRange.ReverseLerp(localPosition.y);
				if (leverInput >= 0.01f)
				{
					hadInput = true;
					if (num <= 0.5f && Blocker.enabled)
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(LeverDown, loop: false);
							SoundManager.Instance.PlaySound(GrinderStart, loop: false, 0.8f);
							SoundManager.Instance.StopSound(GrinderEnd);
							SoundManager.Instance.StopSound(GrinderLoop);
						}
						Blocker.enabled = false;
						StopCoroutines();
						popCoroutine = StartCoroutine(PopObjects());
						animateCoroutine = StartCoroutine(AnimateObjects());
					}
				}
				else
				{
					if (hadInput)
					{
						if (!Blocker.enabled)
						{
							Blocker.enabled = true;
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(LeverUp, loop: false);
								SoundManager.Instance.StopSound(GrinderStart);
								SoundManager.Instance.StopSound(GrinderLoop);
								SoundManager.Instance.PlaySound(GrinderEnd, loop: false, 0.8f);
							}
						}
						if (!finished && Objects.All((SpriteRenderer o) => !o))
						{
							finished = true;
							MyNormTask.NextStep();
							StartCoroutine(CoStartClose());
						}
					}
					hadInput = false;
				}
			}
		}
		else
		{
			switch (controller.CheckDrag(Handle))
			{
			case DragState.Dragging:
				if (finished)
				{
					break;
				}
				if (num > 0.5f)
				{
					Vector2 vector = controller.DragPosition - (Vector2)transform.position;
					float num2 = HandleRange.ReverseLerp(HandleRange.Clamp(vector.y));
					localPosition.y = HandleRange.Lerp(num2 / 2f + 0.5f);
					break;
				}
				localPosition.y = Mathf.Lerp(localPosition.y, HandleRange.min, num + Time.deltaTime * 15f);
				if (Blocker.enabled)
				{
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(LeverDown, loop: false);
						SoundManager.Instance.PlaySound(GrinderStart, loop: false, 0.8f);
						SoundManager.Instance.StopSound(GrinderEnd);
						SoundManager.Instance.StopSound(GrinderLoop);
					}
					Blocker.enabled = false;
					StopCoroutines();
					popCoroutine = StartCoroutine(PopObjects());
					animateCoroutine = StartCoroutine(AnimateObjects());
				}
				break;
			case DragState.NoTouch:
				localPosition.y = Mathf.Lerp(localPosition.y, HandleRange.max, num + Time.deltaTime * 15f);
				break;
			case DragState.Released:
				if (!Blocker.enabled)
				{
					Blocker.enabled = true;
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(LeverUp, loop: false);
						SoundManager.Instance.StopSound(GrinderStart);
						SoundManager.Instance.StopSound(GrinderLoop);
						SoundManager.Instance.PlaySound(GrinderEnd, loop: false, 0.8f);
					}
				}
				if (!finished && Objects.All((SpriteRenderer o) => !o))
				{
					finished = true;
					MyNormTask.NextStep();
					StartCoroutine(CoStartClose());
				}
				break;
			}
		}
		if (Constants.ShouldPlaySfx() && !Blocker.enabled && !SoundManager.Instance.SoundIsPlaying(GrinderStart))
		{
			SoundManager.Instance.PlaySound(GrinderLoop, loop: true, 0.8f, SoundManager.Instance.SfxChannel);
		}
		Handle.transform.localPosition = localPosition;
		Vector3 localScale = Bars.transform.localScale;
		localScale.y = HandleRange.ChangeRange(localPosition.y, -1f, 1f);
		Bars.transform.localScale = localScale;
	}

	private IEnumerator PopObjects()
	{
		Popper.enabled = true;
		yield return new WaitForSeconds(0.05f);
		Popper.enabled = false;
		popCoroutine = null;
	}

	private IEnumerator AnimateObjects()
	{
		Vector3 pos = VibratePivot.localPosition;
		for (float t = 3f; t > 0f; t -= Time.deltaTime)
		{
			float num = t / 3f;
			float num2 = num * 0.1f * 3f;
			VibrationManager.Vibrate(num2, num2, 0.01f);
			VibratePivot.localPosition = pos + (Vector3)Vector2Range.NextEdge() * num * 0.1f;
			yield return null;
		}
		animateCoroutine = null;
	}

	public override void Close()
	{
		SoundManager.Instance.StopSound(GrinderStart);
		SoundManager.Instance.StopSound(GrinderLoop);
		SoundManager.Instance.StopSound(GrinderEnd);
		if ((bool)MyNormTask && MyNormTask.IsComplete)
		{
			ShipStatus.Instance.OpenHatch();
			PlayerControl.LocalPlayer.RpcPlayAnimation((byte)MyTask.TaskType);
		}
		base.Close();
	}
}
