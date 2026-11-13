using System;
using TMPro;
using UnityEngine;

public class DropdownOption : PoolableBehavior
{
	[SerializeField]
	private TextMeshPro optionText;

	[SerializeField]
	private PassiveButton button;

	public UiElement ButtonUiElement => button;

	public void Initialize(string text, Action onClick, Collider2D clickMask, Vector3 position)
	{
		optionText.text = text;
		button.OnClick.RemoveAllListeners();
		button.OnClick.AddListener(() =>
		{
			onClick();
		});
		button.ClickMask = clickMask;
		transform.localPosition = position;
	}

	public override void Reset()
	{
		optionText.text = string.Empty;
		button.OnClick.RemoveAllListeners();
		button.ClickMask = null;
	}
}
