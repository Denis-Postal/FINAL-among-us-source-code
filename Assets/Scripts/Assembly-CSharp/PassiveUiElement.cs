using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveUiElement : UiElement
{
	public Collider2D ClickMask;

	public Collider2D[] Colliders;

	public virtual bool HandleUp => false;

	public virtual bool HandleDown => false;

	public virtual bool HandleRepeat => false;

	public virtual bool HandleUpClickGraphic => false;

	public virtual bool HandleDownClickGraphic => false;

	public virtual bool HandleDrag => false;

	public virtual bool HandleOverOut => true;

	public float CachedZ { get; set; }

	protected virtual void OnEnable()
	{
	}

	protected virtual void Start()
	{
		// TODO(verify): reimplemented (decomp stub)
		DestroyableSingleton<PassiveButtonManager>.Instance.RegisterOne(this);
		if (Colliders == null || Colliders.Length == 0)
		{
			Colliders = GetComponents<Collider2D>();
		}
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual void OnDestroy()
	{
		if (DestroyableSingleton<PassiveButtonManager>.InstanceExists)
		{
			DestroyableSingleton<PassiveButtonManager>.Instance.RemoveOne(this);
		}
	}

	public virtual void ReceiveClickDown()
	{
	}

	public virtual void ReceiveRepeatDown()
	{
	}

	public virtual void ReceiveClickUp()
	{
	}

	public virtual void ReleaseButton()
	{
	}

	public virtual void ReceiveClickDrag(Vector2 dragDelta)
	{
	}

	public virtual void ReceiveClickUpGraphic()
	{
	}

	public virtual void ReceiveClickDownGraphic()
	{
	}

	protected PassiveUiElement()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		// TODO(verify): lost struct initializer (decomp: ControllerNav = (ControllerNavigation)0;)
	}
}
