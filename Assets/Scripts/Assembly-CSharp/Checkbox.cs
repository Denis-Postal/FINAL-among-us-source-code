using System;
using UnityEngine;
using UnityEngine.Events;

public class Checkbox : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer checkmark;

	[SerializeField]
	private PassiveButton button;

	private bool oldValue;

	public bool IsChecked
	{
		get
		{
			return checkmark.enabled;
		}
		set
		{
			checkmark.enabled = value;
		}
	}

	public void Toggle()
	{
		IsChecked = !IsChecked;
	}

	private void Awake()
	{
		button.OnClick.AddListener(Toggle);
		IsChecked = false;
	}
}
