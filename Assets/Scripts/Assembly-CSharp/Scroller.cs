using UnityEngine.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class Scroller : PassiveUiElement
{
	public delegate void ScrollHandler(float value);

	public Transform Inner;

	public bool allowY;

	public bool showY = true;

	[FormerlySerializedAs("YBounds")]
	[SerializeField]
	public FloatRange ContentYBounds;

	[FormerlySerializedAs("ScrollerYRange")]
	public FloatRange ScrollbarYBounds;

	[FormerlySerializedAs("ScrollerY")]
	public Scrollbar ScrollbarY;

	public bool allowX;

	public bool showX;

	[SerializeField]
	[FormerlySerializedAs("XBounds")]
	public FloatRange ContentXBounds;

	[FormerlySerializedAs("ScrollerXRange")]
	public FloatRange ScrollbarXBounds;

	[FormerlySerializedAs("ScrollerX")]
	public Scrollbar ScrollbarX;

	public float DragScrollSpeed;

	public float ScrollWheelSpeed;

	public bool MouseMustBeOverToScroll;

	private Vector2 velocity;

	private bool active;

	private bool mouseOver;

	public ScrollHandler OnScrollXEvent;

	public ScrollHandler OnScrollYEvent;

	public override bool HandleUp => true;

	public override bool HandleDown => true;

	public override bool HandleDrag => true;

	public override bool HandleOverOut => MouseMustBeOverToScroll;

	public bool AtTop => Inner == null || Inner.localPosition.y <= ContentYBounds.min + 0.001f;

	public bool AtBottom => Inner == null || Inner.localPosition.y >= ContentYBounds.max - 0.001f;

	public bool AtLeft => Inner == null || Inner.localPosition.x <= ContentXBounds.min + 0.001f;

	public bool AtRight => Inner == null || Inner.localPosition.x >= ContentXBounds.max - 0.001f;

	public Collider2D Hitbox
	{
		get
		{
			Collider2D[] colliders = Colliders;
			if (colliders.Length > 0)
			{
				return colliders[0];
			}
			return (Collider2D)(object)new IndexOutOfRangeException();
		}
	}

	public void SetBounds(FloatRange yBounds, FloatRange xBounds)
	{
		if (yBounds != null)
		{
			ContentYBounds = yBounds;
		}
		if (xBounds != null)
		{
			ContentXBounds = xBounds;
		}
		UpdateScrollBars();
	}

	public void CalculateAndSetYBounds(float amount, float numPerRow, float numRowsVisible, float spacing)
	{
		// TODO(verify): Scroller logic rebuilt from the 2019 baseline (DoScroll/UpdateScrollBars) and the field set; original body was a no-op in the decompile.
		float rows = Mathf.Ceil(amount / Mathf.Max(1f, numPerRow));
		ContentYBounds.max = Mathf.Max(0f, (rows - numRowsVisible) * spacing);
		UpdateScrollBars();
	}

	public void SetBoundsMax(float yMax, float xMax)
	{
		FloatRange contentYBounds = ContentYBounds;
		contentYBounds.max = yMax;
		FloatRange contentXBounds = ContentXBounds;
		contentXBounds.max = xMax;
		UpdateScrollBars();
	}

	public void SetYBoundsMax(float yMax)
	{
		FloatRange contentYBounds = ContentYBounds;
		contentYBounds.max = yMax;
		UpdateScrollBars();
	}

	public void SetBoundsMin(float yMin, float xMin)
	{
		FloatRange contentYBounds = ContentYBounds;
		contentYBounds.min = yMin;
		FloatRange contentXBounds = ContentXBounds;
		contentXBounds.min = xMin;
		UpdateScrollBars();
	}

	public void SetYBoundsMin(float yMin)
	{
		ContentYBounds.min = yMin;
		UpdateScrollBars();
	}

	public FloatRange GetYBounds()
	{
		return ContentYBounds;
	}

	public FloatRange GetXBounds()
	{
		return ContentXBounds;
	}

	protected override void Update()
	{
		// TODO(verify): Scroller logic rebuilt from the 2019 baseline (DoScroll/UpdateScrollBars) and the field set; original body was a no-op in the decompile.
		if (Inner == null)
		{
			return;
		}
		Vector2 wheel = Input.mouseScrollDelta;
		if (wheel.sqrMagnitude > 0f && (!MouseMustBeOverToScroll || IsMouseOverHitbox()))
		{
			float notch = Mathf.Max(ScrollWheelSpeed, 0.05f) * 4f;
			ScrollRelative(new Vector2(-wheel.x * notch, -wheel.y * notch));
		}
		if (!active && velocity.sqrMagnitude > 0.0001f)
		{
			ScrollRelative(velocity);
			velocity *= 0.9f;
		}
		else if (!active)
		{
			velocity = Vector2.zero;
		}
	}

	private bool IsMouseOverHitbox()
	{
		Collider2D hitbox = Colliders != null && Colliders.Length > 0 ? Colliders[0] : null;
		if (hitbox == null)
		{
			return true;
		}
		Camera cam = Camera.main;
		foreach (Camera c in Camera.allCameras)
		{
			if (c.enabled && (c.cullingMask & (1 << hitbox.gameObject.layer)) != 0)
			{
				cam = c;
				break;
			}
		}
		if (cam == null)
		{
			return true;
		}
		Vector3 world = cam.ScreenToWorldPoint(Input.mousePosition);
		return hitbox.OverlapPoint(world);
	}

	public void ScrollDown()
	{
		Collider2D hitbox = Colliders != null && Colliders.Length > 0 ? Colliders[0] : null;
		float height = hitbox != null ? hitbox.bounds.max.y - hitbox.bounds.min.y : 1f;
		ScrollRelative(new Vector2(0f, height * 0.75f));
	}

	public void ScrollUp()
	{
		Collider2D hitbox = Colliders != null && Colliders.Length > 0 ? Colliders[0] : null;
		float height = hitbox != null ? hitbox.bounds.max.y - hitbox.bounds.min.y : 1f;
		ScrollRelative(new Vector2(0f, height * -0.75f));
	}

	public float GetScrollPercY()
	{
		if (ContentYBounds == null || ContentYBounds.Width < 0.0001f || Inner == null)
		{
			return 1f;
		}
		return ContentYBounds.ReverseLerp(Inner.localPosition.y);
	}

	public float GetScrollPercX()
	{
		if (ContentXBounds != null)
		{
			float width = ContentXBounds.Width;
			if (0.0001f > width)
			{
				return 1f;
			}
			if ((object)Inner != null)
			{
				Transform transform = Inner.transform;
				if ((object)transform != null)
				{
					Vector3 localPosition = transform.localPosition;
					width = localPosition.x;
					if (ContentXBounds != null)
					{
						return ContentXBounds.ReverseLerp(localPosition.x);
					}
				}
			}
		}
		throw new NullReferenceException();
	}

	public unsafe void ScrollPercentY(float p)
	{
		if (Inner == null) return;
		Vector3 pos = Inner.localPosition;
		pos.y = ContentYBounds.Lerp(p);
		Inner.localPosition = pos;
		UpdateScrollBars();
	}

	public unsafe void ScrollPercentX(float p)
	{
		if (Inner == null) return;
		Vector3 pos = Inner.localPosition;
		pos.x = ContentXBounds.Lerp(p);
		Inner.localPosition = pos;
		UpdateScrollBars();
	}

	public override void ReceiveClickDown()
	{
		active = true;
	}

	public override void ReceiveClickUp()
	{
		active = false;
	}

	public override void ReceiveClickDrag(Vector2 dragDelta)
	{
		// TODO(verify): Scroller logic rebuilt from the 2019 baseline (DoScroll/UpdateScrollBars) and the field set; original body was a no-op in the decompile.
		dragDelta *= DragScrollSpeed;
		velocity = dragDelta;
		ScrollRelative(dragDelta);
	}

	public unsafe void ScrollToScrollbarPositionY(Vector3 newScrollbarPosition)
	{
		float p = ScrollbarYBounds.ReverseLerp(newScrollbarPosition.y);
		ScrollPercentY(1f - p);
	}

	public unsafe void ScrollToScrollbarPositionX(Vector3 newScrollbarPosition)
	{
		float p = ScrollbarXBounds.ReverseLerp(newScrollbarPosition.x);
		ScrollPercentX(p);
	}

	public unsafe void ScrollRelative(Vector2 dragDelta)
	{
		if (Inner == null) return;
		if (!allowX) dragDelta.x = 0f;
		if (!allowY) dragDelta.y = 0f;
		if (dragDelta == Vector2.zero) return;
		Vector3 pos = Inner.localPosition + (Vector3)dragDelta;
		pos.x = Mathf.Clamp(pos.x, ContentXBounds.min, Mathf.Max(ContentXBounds.min, ContentXBounds.max));
		pos.y = Mathf.Clamp(pos.y, ContentYBounds.min, Mathf.Max(ContentYBounds.min, ContentYBounds.max));
		Inner.localPosition = pos;
		UpdateScrollBars();
		if (dragDelta.y != 0f && OnScrollYEvent != null) OnScrollYEvent(GetScrollPercY());
		if (dragDelta.x != 0f && OnScrollXEvent != null) OnScrollXEvent(GetScrollPercX());
	}

	public void UpdateScrollBars()
	{
		if (Inner == null) return;
		if (ScrollbarY != null)
		{
			bool on = showY && allowY && ContentYBounds.Width > 0.0001f;
			ScrollbarY.Toggle(on);
			if (on)
			{
				float perc = ContentYBounds.ReverseLerp(Inner.localPosition.y);
				Vector3 sp = ScrollbarY.graphic.transform.localPosition;
				sp.y = ScrollbarYBounds.Lerp(1f - perc);
				ScrollbarY.UpdatePosition(sp);
			}
		}
		if (ScrollbarX != null)
		{
			bool on = showX && allowX && ContentXBounds.Width > 0.0001f;
			ScrollbarX.Toggle(on);
			if (on)
			{
				float perc = ContentXBounds.ReverseLerp(Inner.localPosition.x);
				Vector3 sp = ScrollbarX.graphic.transform.localPosition;
				sp.x = ScrollbarXBounds.Lerp(perc);
				ScrollbarX.UpdatePosition(sp);
			}
		}
	}

	public unsafe void ScrollToTop()
	{
		if (Inner == null) return;
		Vector3 pos = Inner.localPosition;
		pos.y = ContentYBounds.min;
		if (allowX) pos.x = ContentXBounds.min;
		Inner.localPosition = pos;
		UpdateScrollBars();
	}

	public Scroller()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		FloatRange contentYBounds = new FloatRange(-10f, 10f);
		ContentYBounds = contentYBounds;
		showX = true;
		ContentXBounds = new FloatRange(-10f, 10f);
		DragScrollSpeed = 1f;
		// TODO(verify): lost struct initializer (decomp: ControllerNav = (ControllerNavigation)0;)
		ScrollWheelSpeed = 0.25f;
	}
}
