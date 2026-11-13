using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SlideBar : PassiveUiElement
{
	public TextMeshPro Title;

	public SpriteRenderer Bar;

	public SpriteRenderer Dot;

	public FloatRange Range;

	public bool Vertical;

	public float Value;

	public UnityEvent OnValueChange;

	private float sliderSegmentIncrement = 0.1f;

	public override bool HandleDrag => true;

	protected unsafe override void OnEnable()
	{
		base.OnEnable();
		SetEnabledColors();
	}

	protected unsafe override void OnDisable()
	{
		base.OnDisable();
		SetDisabledColors();
	}

	public override void ReceiveClickDrag(Vector2 dragDelta)
	{
		Vector3 localPosition = Dot.transform.localPosition;
		Vector2 vector = DestroyableSingleton<PassiveButtonManager>.Instance.controller.DragPosition - (Vector2)Bar.transform.position;
		if (Vertical)
		{
			localPosition.y = Range.Clamp(vector.y);
			Value = Range.ReverseLerp(localPosition.y);
		}
		else
		{
			localPosition.x = Range.Clamp(vector.x);
			Value = Range.ReverseLerp(localPosition.x);
		}
		UpdateValue();
		OnValueChange.Invoke();
	}

	public unsafe void UpdateValue()
	{
		Vector3 localPosition = Dot.transform.localPosition;
		if (Vertical)
		{
			localPosition.y = Range.Lerp(Value);
		}
		else
		{
			localPosition.x = Range.Lerp(Value);
		}
		Dot.transform.localPosition = localPosition;
	}

	public void SetValue(float newValue)
	{
		Value = Mathf.Clamp01(newValue);
		UpdateValue();
	}

	public unsafe void ControllerIncrease()
	{
		float value = Value + sliderSegmentIncrement;
		Value = Mathf.Clamp(value, 0f, 1f);
		Debug.Log("ControllerIncrease: " + Value);
		UpdateValue();
		OnValueChange.Invoke();
	}

	public unsafe void ControllerDecrease()
	{
		float value = Value - sliderSegmentIncrement;
		Value = Mathf.Clamp(value, 0f, 1f);
		Debug.Log("ControllerDecrease: " + Value);
		UpdateValue();
		OnValueChange.Invoke();
	}

	private void OnValidate()
	{
		UpdateValue();
	}

	public unsafe void SetEnabledColors()
	{
		if ((bool)Title)
		{
			Title.color = Color.white;
		}
		if ((bool)Bar)
		{
			Bar.color = Color.white;
		}
		Dot.color = Color.white;
		UpdateValue();
	}

	public unsafe void SetDisabledColors()
	{
		if ((bool)Title)
		{
			Title.color = Color.gray;
		}
		if ((bool)Bar)
		{
			Bar.color = Color.gray;
		}
		Dot.color = Color.gray;
	}
}
