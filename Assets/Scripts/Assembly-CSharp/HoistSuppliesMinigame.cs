using System;
using Rewired;
using UnityEngine;

public class HoistSuppliesMinigame : Minigame
{
	private const string PULL_LOOP_NAME = "hoist_supplies_pull_loop_sfx";

	[SerializeField]
	private Collider2D draggable;

	[Header("Input")]
	[SerializeField]
	[Tooltip("The difficulty of the hoist minigame, increasing as the target gets closer to completion.")]
	private AnimationCurve liftMultiplierCurve;

	[SerializeField]
	[Range(0.01f, 4f)]
	[Tooltip("The forced delay (in seconds) between each hoist controller button press. Players cannot press the hoist button faster than this.")]
	private float controllerButtonDelay = 0.2f;

	[Tooltip("The amount added every time a controller button is pressed (Multiplied by the liftMultiplierCurve)")]
	[SerializeField]
	[Range(0.01f, 2f)]
	private float controllerRaiseAmount = 0.4f;

	[SerializeField]
	[Range(0.01f, 2f)]
	[Tooltip("The amount of offset to add to the pull-rope every time the controller pull button is pressed")]
	private float controllerPullAmount = 0.4f;

	[Header("Animation Speeds")]
	[SerializeField]
	[Tooltip("The Speed at which the hoisted sprite animates to it's intended offset each time the controller pull button is pressed. (Recommended between the 3-15 range)")]
	private float raiseAnimSpeed = 5f;

	[SerializeField]
	[Tooltip("The Speed at which the pull-rope animates to it's intended offset each time the controller pull button is pressed. (Recommended between the 3-15 range)")]
	private float pullAnimSpeed = 5f;

	[SerializeField]
	[Header("Rope")]
	private SpriteRenderer rope;

	[SerializeField]
	private Vector3 ropeTopPos;

	[SerializeField]
	private Vector3 ropeBottomPos;

	[SerializeField]
	private FloatRange ropeLength;

	[Header("Basket")]
	[SerializeField]
	private Transform basket;

	[SerializeField]
	private FloatRange basketSize;

	[SerializeField]
	private Vector3 basketTopPos;

	[SerializeField]
	private Vector3 basketBottomPos;

	[Header("Draggable Rope")]
	[SerializeField]
	private Vector3 draggableRopeTopPos;

	[SerializeField]
	private Vector3 draggableRopeBottomPos;

	[SerializeField]
	private Transform ropeTexture;

	[Header("Lights")]
	[SerializeField]
	private GameObject redCircle;

	[SerializeField]
	private GameObject greenCircle;

	[SerializeField]
	[Header("Controller Hint")]
	private GameObject controllerHintContainer;

	[SerializeField]
	[Header("Audio")]
	private AudioClip[] pullOneShotSfx;

	[SerializeField]
	private AudioClip pullLoopSfx;

	[SerializeField]
	[Space(10f)]
	private AnimationCurve pullLoopSfxVolumeCurve;

	[SerializeField]
	private float pullLoopSfxVolumeSpeed = 20f;

	private readonly Controller controller = new Controller();

	private Player player;

	private bool completed;

	private Vector2 lastDragPosition;

	private float targetPullLoopVolume;

	private float truePullLoopVolume;

	private float targetRaisedValue;

	private float trueRaisedValue;

	private float targetPullValue;

	private float truePullValue;

	private float raisedValueOnDragStart;

	private float pullValueOnDragStart;

	private float lastControllerInputTime;

	private void Update()
	{
		truePullLoopVolume = Mathf.Lerp(truePullLoopVolume, targetPullLoopVolume, Time.deltaTime * pullLoopSfxVolumeSpeed);
		targetPullLoopVolume = 0f;
		if (completed || amClosing != CloseState.None)
		{
			return;
		}
		if (draggable.enabled)
		{
			controller.Update();
			switch (controller.CheckDrag(draggable))
			{
			case DragState.TouchStart:
				raisedValueOnDragStart = trueRaisedValue;
				pullValueOnDragStart = truePullValue;
				break;
			case DragState.Dragging:
			{
				Vector2 vector = controller.DragPosition - controller.DragStartPosition;
				float num = (lastDragPosition - controller.DragPosition).magnitude / Time.deltaTime;
				lastDragPosition = controller.DragPosition;
				if (!(vector.y > 0f))
				{
					float num2 = Mathf.Clamp01(vector.magnitude / Vector3.Distance(draggableRopeBottomPos, draggableRopeTopPos));
					targetPullValue = (truePullValue = pullValueOnDragStart + num2);
					targetRaisedValue = (trueRaisedValue = raisedValueOnDragStart + num2 * liftMultiplierCurve.Evaluate(targetRaisedValue));
					targetPullLoopVolume = Mathf.Clamp01((num - 0.3f) / 3.7f);
				}
				break;
			}
			case DragState.Released:
				draggable.transform.localPosition = draggableRopeTopPos;
				break;
			}
		}
		if (Time.time > lastControllerInputTime + controllerButtonDelay && (player.GetButtonDown(21) || player.GetButtonDown(24)))
		{
			lastControllerInputTime = Time.time;
			targetRaisedValue += liftMultiplierCurve.Evaluate(targetRaisedValue) * controllerRaiseAmount;
			targetPullValue += controllerPullAmount;
			VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySoundImmediate(pullOneShotSfx.Random(), loop: false);
			}
		}
		truePullValue = Mathf.Lerp(truePullValue, targetPullValue, pullAnimSpeed * Time.deltaTime);
		SetPullPosition(truePullValue);
		trueRaisedValue = Mathf.Lerp(trueRaisedValue, targetRaisedValue, raiseAnimSpeed * Time.deltaTime);
		SetRopePosition(1f - trueRaisedValue);
		if (trueRaisedValue >= 1f)
		{
			completed = true;
			draggable.enabled = false;
			MyNormTask.NextStep();
			greenCircle.SetActive(value: true);
			redCircle.SetActive(value: false);
			StartCoroutine(Effects.ActionAfterDelay(0.5f, () =>
			{
				transform.parent.GetComponent<MultistageMinigame>().Close();
			}));
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: true);
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
		OnInputMethodChanged();
		player = ReInput.players.GetPlayer(0);
		redCircle.SetActive(value: true);
		greenCircle.SetActive(value: false);
		truePullLoopVolume = 0f;
		targetPullLoopVolume = 0f;
		SoundManager.Instance.PlayDynamicSound("hoist_supplies_pull_loop_sfx", pullLoopSfx, loop: true, PullLoopDynamics, SoundManager.Instance.SfxChannel);
	}

	public override void Close()
	{
		base.Close();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
		SoundManager.Instance.StopNamedSound("hoist_supplies_pull_loop_sfx");
	}

	[ContextMenu("ResetRope")]
	private void ResetRope()
	{
		SetRopePosition(1f);
	}

	[ContextMenu("Complete Hoist")]
	private void CompleteHoist()
	{
		SetRopePosition(0f);
	}

	private void SetRopePosition(float distanceFromTop)
	{
		distanceFromTop = Mathf.Clamp(distanceFromTop, -0.2f, 1f);
		rope.transform.localPosition = Vector3.Lerp(ropeTopPos, ropeBottomPos, distanceFromTop);
		rope.size = new Vector2(0.14f, ropeLength.Lerp(distanceFromTop));
		float num = basketSize.Lerp(distanceFromTop);
		basket.transform.localScale = new Vector3(num, num, 1f);
		basket.transform.localPosition = Vector3.Lerp(basketTopPos, basketBottomPos, distanceFromTop);
	}

	private void SetPullPosition(float amount)
	{
		ropeTexture.transform.localPosition = Vector3.Lerp(draggableRopeTopPos, draggableRopeBottomPos, Mathf.Repeat(amount, 1f));
	}

	private void OnInputMethodChanged()
	{
		controllerHintContainer.SetActive(ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
	}

	private void PullLoopDynamics(AudioSource source, float dt)
	{
		source.volume = pullLoopSfxVolumeCurve.Evaluate(truePullLoopVolume);
	}
}
