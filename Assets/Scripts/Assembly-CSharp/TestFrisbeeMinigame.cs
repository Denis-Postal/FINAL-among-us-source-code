using System.Collections;
using PowerTools;
using Rewired;
using UnityEngine;

public class TestFrisbeeMinigame : Minigame
{
	[SerializeField]
	[Header("Design Values")]
	[Range(0.5f, 0.99f)]
	private float pickUpAnalogThresshold = 0.9f;

	[SerializeField]
	[Header("Components")]
	private PassiveButton stuckFrisbee;

	[SerializeField]
	private Collider2D throwableFrisbee;

	[SerializeField]
	private SpriteAnim sparkle;

	[SerializeField]
	private GameObject throwControllerGlyph;

	[SerializeField]
	[Tooltip("The number of seconds frisbee spends in the air after tossing, before it disappears")]
	private float totalSeconds = 0.5f;

	[SerializeField]
	private Transform finalFrisbeePosition;

	[Header("Sounds")]
	[SerializeField]
	private AudioClip grabSound;

	[SerializeField]
	private AudioClip throwSound;

	[SerializeField]
	private AudioClip twinkleSound;

	private readonly Controller controller = new Controller();

	private Vector3 originalFrisbeePosition;

	private Vector3 throwStartPosition;

	private bool pickedUp;

	private bool thrown;

	private float time;

	private bool isCompleting;

	private Player player;

	private void Update()
	{
		if (MyNormTask.IsComplete || amClosing != CloseState.None || isCompleting)
		{
			return;
		}
		if (!pickedUp)
		{
			if (player.GetAxis(17) >= pickUpAnalogThresshold)
			{
				PickUpFrisbee();
			}
			return;
		}
		if (thrown)
		{
			time += Time.deltaTime;
			float t = time / totalSeconds;
			throwableFrisbee.transform.localPosition = Vector3.Lerp(throwStartPosition, finalFrisbeePosition.localPosition, t);
			throwableFrisbee.transform.localScale = Vector3.one * Mathf.Lerp(1f, 0f, t);
			if (Mathf.Approximately(0f, throwableFrisbee.transform.localScale.x))
			{
				thrown = false;
				StartCoroutine(CoFinalAnimation());
			}
			return;
		}
		controller.Update();
		switch (controller.CheckDrag(throwableFrisbee))
		{
		case DragState.Dragging:
			if (!thrown && !MyNormTask.IsComplete)
			{
				Vector2 vector2 = controller.DragPosition - (Vector2)transform.position;
				Vector3 localPosition = throwableFrisbee.transform.localPosition;
				if (vector2.x < localPosition.x || vector2.y < localPosition.y)
				{
					return;
				}
				localPosition.x = vector2.x;
				localPosition.y = vector2.y;
				throwableFrisbee.transform.localPosition = localPosition;
				if (localPosition.x - originalFrisbeePosition.x > 0.2f && localPosition.y - originalFrisbeePosition.y > 0.2f)
				{
					ThrowFrisbee();
				}
			}
			break;
		case DragState.Released:
		{
			Vector2 vector = throwableFrisbee.transform.localPosition - originalFrisbeePosition;
			if (vector.y < 0f || vector.x < 0f || vector.magnitude < 0.2f)
			{
				throwableFrisbee.transform.localPosition = originalFrisbeePosition;
				return;
			}
			ThrowFrisbee();
			break;
		}
		}
		if (!thrown && !MyNormTask.IsComplete && player.GetAxis(17) > 0.6f && player.GetAxis(16) > 0.6f)
		{
			ThrowFrisbee();
		}
	}

	private IEnumerator CoFinalAnimation()
	{
		isCompleting = true;
		yield return Effects.Wait(0.3f);
		sparkle.gameObject.SetActive(value: true);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(twinkleSound, loop: false);
		}
		yield return Effects.Wait(0.5f);
		sparkle.gameObject.SetActive(value: false);
		yield return Effects.Wait(0.25f);
		MyNormTask.NextStep();
		yield return CoStartClose();
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: true);
		player = ReInput.players.GetPlayer(0);
		stuckFrisbee.gameObject.SetActive(value: true);
		throwableFrisbee.gameObject.SetActive(value: false);
		stuckFrisbee.OnClick.AddListener(PickUpFrisbee);
	}

	private void PickUpFrisbee()
	{
		pickedUp = true;
		stuckFrisbee.gameObject.SetActive(value: false);
		throwableFrisbee.gameObject.SetActive(value: true);
		originalFrisbeePosition = throwableFrisbee.transform.localPosition;
		VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(grabSound, loop: false);
		}
	}

	private void ThrowFrisbee()
	{
		throwControllerGlyph.SetActive(value: false);
		thrown = true;
		throwStartPosition = throwableFrisbee.transform.localPosition;
		VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(throwSound, loop: false);
		}
	}

	[ContextMenu("Reset Frisbee Position")]
	private void ResetFrisbeePosition()
	{
		throwableFrisbee.transform.localPosition = originalFrisbeePosition;
		throwableFrisbee.transform.localScale = Vector3.one;
	}
}
