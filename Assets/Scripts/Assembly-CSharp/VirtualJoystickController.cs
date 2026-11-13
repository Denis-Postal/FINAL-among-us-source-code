using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class VirtualJoystickController
{
	private Collider2D amTouching;

	private int fingerId = -1;

	private Controller.TouchState MyTouch;

	public Vector2 DragPosition
	{
		get
		{
			if (fingerId < 0)
			{
				return Vector2.zero;
			}
			return MyTouch.Position;
		}
	}

	public Vector2 DragStartPosition => MyTouch.DownAt;

	public Camera mainCam { get; set; }

	public VirtualJoystickController()
	{
		//IL_000f: Expected I4, but got I8
	}

	public void Update()
	{
		if (!mainCam)
		{
			mainCam = Camera.main;
		}
	}

	public unsafe DragState CheckDrag(Collider2D coll, Collider2D deadZone = null)
	{
		if (!coll)
		{
			return DragState.NoTouch;
		}
		if (fingerId <= -1)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if ((deadZone == null || !deadZone.OverlapPoint(touch.position)) && coll.OverlapPoint(mainCam.ScreenToWorldPoint(touch.position)))
				{
					fingerId = touch.fingerId;
					MyTouch = new Controller.TouchState();
					MyTouch.ScreenPosition = touch.position;
					MyTouch.Position = mainCam.ScreenToWorldPoint(touch.position);
					MyTouch.IsDown = true;
					MyTouch.active = true;
					MyTouch.FingerId = touch.fingerId;
					MyTouch.ScreenDownAt = MyTouch.ScreenPosition;
					MyTouch.DownAt = MyTouch.Position;
					amTouching = coll;
					MyTouch.dragState = DragState.TouchStart;
					return DragState.TouchStart;
				}
			}
		}
		if (fingerId > -1)
		{
			if (!amTouching || !amTouching.isActiveAndEnabled)
			{
				MyTouch = null;
				fingerId = -1;
				amTouching = null;
			}
			if (coll != amTouching)
			{
				return DragState.NoTouch;
			}
			Touch? touch2 = null;
			for (int j = 0; j < Input.touchCount; j++)
			{
				Touch touch3 = Input.GetTouch(j);
				if (touch3.fingerId == fingerId)
				{
					touch2 = touch3;
				}
			}
			if (!touch2.HasValue || (deadZone != null && deadZone.OverlapPoint(mainCam.ScreenToWorldPoint(touch2.Value.position))))
			{
				amTouching = null;
				fingerId = -1;
				MyTouch = null;
				return DragState.Released;
			}
			MyTouch.ScreenPosition = touch2.Value.position;
			MyTouch.Position = mainCam.ScreenToWorldPoint(touch2.Value.position);
			MyTouch.IsDown = true;
			MyTouch.active = true;
			if (Vector2.Distance(MyTouch.ScreenDownAt, MyTouch.ScreenPosition) > 10f || MyTouch.dragState == DragState.Dragging)
			{
				MyTouch.dragState = DragState.Dragging;
				return DragState.Dragging;
			}
			MyTouch.dragState = DragState.Holding;
			return DragState.Holding;
		}
		fingerId = -1;
		return DragState.NoTouch;
	}
}
