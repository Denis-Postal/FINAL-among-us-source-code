using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class AdjustSteeringGame : Minigame
{
	private const float ArrowOffset = -3.25f;

	private const float ThrustOffset = -2.15f;

	private static readonly FloatRange ThrustRange= new FloatRange(-1.84f, 1.84f);

	private static readonly FloatRange SteeringRange= new FloatRange(-30f, 30f);

	public Collider2D Thrust;

	public Collider2D Steering;

	public SpriteRenderer ThrustTarget;

	public SpriteRenderer SteeringTarget;

	private Controller controller = new Controller();

	private float TargetThrustY;

	private float TargetSteeringRot;

	private bool thrustLocked;

	private bool steeringLocked;

	private float startAngle;

	public AudioClip HornSound;

	private bool prevHadLeftInput;

	private bool prevHadRightInput;

	private Vector2 prevRightStickInput= Vector2.zero;

	private bool prevThrustWasGood;

	private bool prevSteeringWasGood;

	private bool playerHasDualshock;

	private const float rotationSensitivity = 0.035f;

	private const float hintVibrationIntensity = 0.5f;

	public void HonkHorn()
	{
		SoundManager.Instance.PlaySoundImmediate(HornSound, loop: false);
	}

	public unsafe override void Begin(PlayerTask task)
{
		base.Begin(task);
		TargetThrustY = ThrustRange.Next();
		ThrustTarget.transform.localPosition = new Vector3(-3.25f, TargetThrustY);
		float y = ThrustRange.NextMinDistance(TargetThrustY, 0.5f);
		Thrust.transform.localPosition = new Vector3(-2.15f, y, -1f);
		TargetSteeringRot = SteeringRange.Next();
		SteeringTarget.transform.localEulerAngles = new Vector3(0f, 0f, TargetSteeringRot);
		float z = SteeringRange.NextMinDistance(TargetSteeringRot, 15f);
		Steering.transform.localEulerAngles = new Vector3(0f, 0f, z);
		playerHasDualshock = false;
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(UpdateControllerIsDualshock));
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			UpdateControllerIsDualshock();
		}
		SetupInput(disableCursor: true, playerHasDualshock);
	}

	private void OnDisable()
{
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(UpdateControllerIsDualshock));
	}

	private void UpdateControllerIsDualshock()
{
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			Rewired.Controller currentController = DestroyableSingleton<ActiveInputManager>.Instance.CurrentController;
			playerHasDualshock = false;
			for (int i = 0; i < Constants.PlayStationControllers.Length; i++)
			{
				if (currentController.name == Constants.PlayStationControllers[i])
				{
					playerHasDualshock = true;
					break;
				}
			}
		}
		else
		{
			playerHasDualshock = false;
		}
		if ((bool)inputHandler)
		{
			inputHandler.suppressInGamePlayerList = playerHasDualshock;
		}
	}

	public unsafe void Update()
{
		if (amClosing != CloseState.None)
		{
			return;
		}
		controller.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			if ((playerHasDualshock && player.GetButtonDown(38)) || player.GetButtonDown(51))
			{
				HonkHorn();
			}
			if (!thrustLocked)
			{
				float axisRaw = player.GetAxisRaw(14);
				Vector3 localPosition = Thrust.transform.localPosition;
				if (Mathf.Abs(axisRaw) > 0.1f)
				{
					localPosition.y += axisRaw * Time.deltaTime * 4f;
					localPosition.y = ThrustRange.Clamp(localPosition.y);
					Thrust.transform.localPosition = localPosition;
					prevHadLeftInput = true;
				}
				else
				{
					if (prevHadLeftInput && Mathf.Abs(localPosition.y - TargetThrustY) < 0.075f)
					{
						thrustLocked = true;
						localPosition.y = TargetThrustY;
						Thrust.transform.localPosition = localPosition;
					}
					prevHadLeftInput = false;
				}
				if (!thrustLocked)
				{
					bool flag = Mathf.Abs(localPosition.y - TargetThrustY) < 0.075f;
					if (flag && !prevThrustWasGood)
					{
						VibrationManager.Vibrate(0.5f, 0f, 0.1f);
					}
					prevThrustWasGood = flag;
				}
			}
			if (!steeringLocked)
			{
				Vector3 localEulerAngles = Steering.transform.localEulerAngles;
				if (localEulerAngles.z > 180f)
				{
					localEulerAngles.z -= 360f;
				}
				Vector2 axis2DRaw = player.GetAxis2DRaw(16, 17);
				_ = Thrust.transform.localPosition;
				float magnitude = axis2DRaw.magnitude;
				if (magnitude > 0.9f)
				{
					if (prevHadRightInput)
					{
						axis2DRaw /= magnitude;
						float num = Vector2.SignedAngle(prevRightStickInput, axis2DRaw);
						localEulerAngles.z += num * 0.035f;
						localEulerAngles.z = SteeringRange.Clamp(localEulerAngles.z);
						Steering.transform.localEulerAngles = localEulerAngles;
					}
					prevRightStickInput = axis2DRaw;
					prevHadRightInput = true;
				}
				else
				{
					if (prevHadRightInput && Mathf.Abs(localEulerAngles.z - TargetSteeringRot) < 5f)
					{
						steeringLocked = true;
						localEulerAngles.z = TargetSteeringRot;
						Steering.transform.localEulerAngles = localEulerAngles;
					}
					prevHadRightInput = false;
				}
				if (!steeringLocked)
				{
					bool flag2 = Mathf.Abs(localEulerAngles.z - TargetSteeringRot) < 5f;
					if (flag2 && !prevSteeringWasGood)
					{
						VibrationManager.Vibrate(0f, 0.5f, 0.1f);
					}
					prevSteeringWasGood = flag2;
				}
			}
		}
		else
		{
			if (!thrustLocked)
			{
				Vector3 localPosition2 = Thrust.transform.localPosition;
				switch (controller.CheckDrag(Thrust))
				{
				case DragState.Dragging:
				{
					Vector2 vector = controller.DragPosition - (Vector2)transform.position;
					vector.y = ThrustRange.Clamp(vector.y);
					localPosition2.y = vector.y;
					Thrust.transform.localPosition = localPosition2;
					break;
				}
				case DragState.Released:
					if (Mathf.Abs(localPosition2.y - TargetThrustY) < 0.075f)
					{
						thrustLocked = true;
						localPosition2.y = TargetThrustY;
						Thrust.transform.localPosition = localPosition2;
					}
					break;
				}
			}
			if (!steeringLocked)
			{
				Vector3 localEulerAngles2 = Steering.transform.localEulerAngles;
				if (localEulerAngles2.z > 180f)
				{
					localEulerAngles2.z -= 360f;
				}
				switch (controller.CheckDrag(Steering))
				{
				case DragState.TouchStart:
					startAngle = localEulerAngles2.z;
					break;
				case DragState.Dragging:
				{
					Vector2 vector2 = controller.DragPosition - controller.DragStartPosition;
					localEulerAngles2.z = SteeringRange.Clamp(vector2.x * -15f + startAngle);
					Steering.transform.localEulerAngles = localEulerAngles2;
					break;
				}
				case DragState.Released:
					if (Mathf.Abs(localEulerAngles2.z - TargetSteeringRot) < 5f)
					{
						steeringLocked = true;
						localEulerAngles2.z = TargetSteeringRot;
						Steering.transform.localEulerAngles = localEulerAngles2;
					}
					break;
				}
			}
		}
		if (thrustLocked && steeringLocked)
		{
			MyNormTask.NextStep();
			Close();
		}
	}

	public AdjustSteeringGame()
	{ }

	static AdjustSteeringGame()
	{ }
}
