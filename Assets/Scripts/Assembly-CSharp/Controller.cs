using System;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;

public class Controller
{
#if UNITY_EDITOR
	public static Func<Controller, bool> CaptureUpdate;
#endif
	public class TouchState
	{
		public Vector2 ScreenDownAt;

		public Vector2 ScreenPosition;

		public Vector2 DownAt;

		public Vector2 Position;

		public bool WasDown;

		public bool IsDown;

		public bool TouchStart;

		public bool TouchEnd;

		public DragState dragState;

		public bool active;

		public int FingerId;
	}

	public enum TouchType
	{
		Joystick,
		Mouse,
		Touch
	}

	private const int maxTouchCount = 4;

	private const int mainTouchIndex = 0;

	public readonly TouchState[] Touches;

	public Collider2D amTouching;

	public int touchId;

	private static Vector3 oldMousePos;

	public static TouchType currentTouchType => (TouchType)ActiveInputManager.currentControlType;

	public Vector2 HoverPosition
	{
			get
		{
			// TODO(verify): rebuilt (decomp stub)
			return Touches[0].Position;
		}
	}

	public bool AnyTouch
	{
			get
		{
			// TODO(verify): rebuilt (decomp stub)
			return Touches[0].IsDown || Touches[1].IsDown;
		}
	}

	public bool AnyTouchDown
	{
			get
		{
			// TODO(verify): rebuilt (decomp stub)
			return Touches[0].TouchStart || Touches[1].TouchStart;
		}
	}

	public bool AnyTouchUp
	{
			get
		{
			// TODO(verify): rebuilt (decomp stub)
			return Touches[0].TouchEnd || Touches[1].TouchEnd;
		}
	}

	public bool FirstDown
	{
			get
		{
			// TODO(verify): rebuilt (decomp stub)
			return Touches[0].TouchStart;
		}
	}

	public Vector2 DragPosition
	{
			get
		{
			// TODO(verify): rebuilt (decomp stub)
			return touchId >= 0 ? Touches[touchId].Position : default(Vector2);
		}
	}

	public Vector2 DragStartPosition
	{
			get
		{
			// TODO(verify): rebuilt (decomp stub)
			return touchId >= 0 ? Touches[touchId].DownAt : default(Vector2);
		}
	}

	public Camera mainCam { get; set; }

	public Controller()
	{
		// TODO(verify): reimplemented (decomp stub)
		Touches = new TouchState[4];
		for (int i = 0; i < Touches.Length; i++) Touches[i] = new TouchState();
		touchId = -1;
	}

	public bool CheckHover(Collider2D coll)
	{
		// TODO(verify): reimplemented (decomp stub)
		return coll != null && coll.OverlapPoint(Touches[0].Position);
	}

	public DragState CheckDrag(Collider2D coll)
	{
		// tasks3: rebuilt from decomp9 Controller.CheckDrag. Released is reported as soon as the touch is no longer down (not only on the TouchEnd frame);
		// Holding is reported until the pointer moved more than 10 screen pixels from where it went down (decomp9 compares `vector <= 10f`, the Method not found @18060DB80 is the screen-distance call).
		// TODO(verify): the 10px threshold and that it is measured in screen space (ScreenDownAt/ScreenPosition) are inferred from decomp9 (distance call unresolved).
		if (!coll)
		{
			return DragState.NoTouch;
		}
		if (touchId > -1)
		{
			if (!amTouching || !amTouching.isActiveAndEnabled)
			{
				touchId = -1;
				amTouching = null;
			}
			if (touchId > -1)
			{
				if (coll != amTouching)
				{
					return DragState.NoTouch;
				}
				TouchState touchState = Touches[touchId];
				if (!touchState.IsDown)
				{
					amTouching = null;
					touchId = -1;
					touchState.dragState = DragState.Released;
					return DragState.Released;
				}
				if (Vector2.Distance(touchState.ScreenDownAt, touchState.ScreenPosition) <= 10f && touchState.dragState != DragState.Dragging)
				{
					touchState.dragState = DragState.Holding;
					return DragState.Holding;
				}
				touchState.dragState = DragState.Dragging;
				return DragState.Dragging;
			}
		}
		for (int i = 0; i < Touches.Length; i++)
		{
			TouchState touchState2 = Touches[i];
			if (touchState2.TouchStart && touchState2.active && coll.OverlapPoint(touchState2.Position))
			{
				amTouching = coll;
				touchId = i;
				touchState2.dragState = DragState.TouchStart;
				return DragState.TouchStart;
			}
		}
		return DragState.NoTouch;
	}

	public void ResetDragPosition()
	{
		TouchState[] touches = Touches;
		int num = touchId;
		TouchState touchState = touches[num];
		int num2 = touchId;
		TouchState[] touches2 = Touches;
		TouchState touchState2 = touches2[num2];
		touchState.DownAt = touchState2.Position;
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [v31 @ rcx_v3 (Controller+TouchState)+2C]");
		_ = 0;
	}

	public void ClearTouch()
	{
		//IL_0085: Expected I4, but got I8
		if (touchId >= 0)
		{
			TouchState[] touches = Touches;
			int num = touchId;
			TouchState touchState = touches[num];
			touchState.TouchStart = true;
			touchState.dragState = DragState.NoTouch;
			amTouching = null;
			touchId = -1;
		}
	}

	public unsafe void Update()
	{
#if UNITY_EDITOR
		// Opt-in capture input. Absent from the player build.
		if (CaptureUpdate != null && CaptureUpdate(this)) return;
		if (Rewired.KeyMap.IgnorePhysical)
		{
			var captureTouch = Touches[0];
			captureTouch.TouchStart = captureTouch.TouchEnd = captureTouch.IsDown = captureTouch.active = false;
			captureTouch.Position = new Vector2(100000f, 100000f);
			return;
		}
#endif
		// Current decomp9 caches the camera and records screen coordinates and active mouse state.
		if (!mainCam) mainCam = Camera.main;
		TouchState touchState = Touches[0];
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			if (!VirtualCursor.instance || !VirtualCursor.instance.isActiveAndEnabled || SpecialInputHandler.disableVirtualCursorCount != 0)
			{
				touchState.TouchStart = false;
				touchState.IsDown = false;
				touchState.active = false;
				return;
			}
			touchState.TouchStart = !touchState.IsDown && VirtualCursor.buttonDown;
			touchState.Position = VirtualCursor.currentPosition;
			touchState.ScreenPosition = mainCam.WorldToScreenPoint(touchState.Position);
			if (touchState.TouchStart)
			{
				touchState.DownAt = touchState.Position;
				touchState.ScreenDownAt = touchState.ScreenPosition;
			}
			touchState.TouchEnd = touchState.IsDown && !VirtualCursor.buttonDown;
			touchState.IsDown = VirtualCursor.buttonDown;
			touchState.active = true;
			return;
		}
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard && !ReInput.players.GetPlayer(0).controllers.hasMouse)
		{
			touchState.TouchStart = false;
			touchState.IsDown = false;
			touchState.active = false;
			return;
		}
		// TODO(verify): native multi-finger Touch-mode mapping remains unrecovered; this path retains baseline mouse input.
		bool mouseButton = Input.GetMouseButton(0);
		touchState.ScreenPosition = Input.mousePosition;
		touchState.Position = mainCam.ScreenToWorldPoint(Input.mousePosition);
		touchState.TouchStart = !touchState.IsDown && mouseButton;
		if (touchState.TouchStart)
		{
			touchState.DownAt = touchState.Position;
			touchState.ScreenDownAt = touchState.ScreenPosition;
		}
		touchState.TouchEnd = touchState.IsDown && !mouseButton;
		touchState.IsDown = mouseButton;
		touchState.active = true;
	}

	public void Reset()
	{ /* spliced-from-2019 */
		for (int i = 0; i < Touches.Length; i++)
		{
			Touches[i] = new TouchState();
		}
		touchId = -1;
		amTouching = null;
	}

	public TouchState GetTouch(int i)
	{
		TouchState[] touches = Touches;
		if (i < touches.Length)
		{
			return touches[i];
		}
		return (TouchState)(object)new IndexOutOfRangeException();
	}

	static Controller()
	{
		// TODO(verify): reimplemented (decomp stub)
		// TODO(verify): decomp static ctor unrecoverable
	}
}
