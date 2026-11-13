using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine;

public class TouchpadBehavior : MonoBehaviour
{
	private float aspect;

	public bool touching;

	public bool firstTouch;

	private Vector2 toCenter;

	private Vector2 firstTouchPos;

	private Vector2 delta;

	private Vector2 fromCenter;

	private IDualShock4Extension ds4;

	public float touchpadSensitivity;

	private void Start()
	{
		GetExtension();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(GetExtension));
	}

	private void OnDestroy()
	{
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(GetExtension));
	}

	private void GetExtension()
	{
		ReInput.PlayerHelper players = ReInput.players;
		Player player = players.GetPlayer(0);
		ds4 = null;
		if (player != null)
		{
			int joystickCount = player.controllers.joystickCount;
			if (joystickCount > 0)
			{
				IList<Joystick> joysticks = player.controllers.Joysticks;
				Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1800036B0");
				Rewired.Controller controller = default;
				IDualShock4Extension extension = controller.GetExtension<IDualShock4Extension>();
				ds4 = extension;
			}
		}
	}

	// TODO(verify): DualShock4 touchpad tracking not recovered; without a DS4 there is never a touch
	private void Update()
	{
		if (ds4 == null)
		{
			touching = false;
			firstTouch = false;
		}
	}

	public bool IsTouching()
	{
		return touching;
	}

	public bool IsFirstTouch()
	{
		return firstTouch;
	}

	public void ResetTouch()
	{
		touching = false;
	}

	public Vector2 GetTouchVector()
	{
		Vector2 result = default;
		if (touching)
		{
			return result;
		}
		return result;
	}

	public Vector2 GetCenterToTouch()
	{
		Vector2 result = default;
		if (touching)
		{
			return result;
		}
		return result;
	}

	public TouchpadBehavior()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		int width = Screen.width;
		int height = Screen.height;
		// TODO(verify): lost struct initializer (decomp: toCenter = (Vector2)1056964608;)
		touchpadSensitivity = 3f;
		aspect = width / height;
	}
}
