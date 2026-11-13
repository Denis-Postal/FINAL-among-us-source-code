using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownButton : MonoBehaviour
{
	[SerializeField]
	private float buttonStartY;

	[SerializeField]
	private float buttonHeight;

	[SerializeField]
	private GameObject optionsScreen;

	[SerializeField]
	private PassiveButton openOptionsButton;

	[SerializeField]
	private PassiveButton closeButton;

	[SerializeField]
	private TextMeshPro buttonText;

	[SerializeField]
	private Collider2D clickMask;

	[SerializeField]
	private ObjectPoolBehavior optionPool;

	private List<string> options = new List<string>();

	private Action<int> onOptionSelect;

	public int SelectedIndex { get; private set; }

	private string DropdownName => name + "DropdownOptionsMenu";

	public void AddOption(string optionName)
	{
		if (options.Count == 0)
		{
			buttonText.text = optionName;
		}
		options.Add(optionName);
	}

	public void SetSelectedIndex(int i)
	{
		if (i >= 0 && i < options.Count)
		{
			SelectedIndex = i;
			buttonText.text = options[i];
		}
	}

	public void SetOnOptionSelect(Action<int> onOptionSelect)
	{
		this.onOptionSelect = onOptionSelect;
	}

	private void Start()
	{
		openOptionsButton.OnClick.AddListener(OpenDropdownOptions);
		closeButton.OnClick.AddListener(Close);
	}

	private void OpenDropdownOptions()
	{
		if (options.Count == 0)
		{
			Debug.LogError("Dropdown button " + name + " had no options");
			return;
		}
		if (optionsScreen.activeInHierarchy)
		{
			Debug.LogError("Tried to open options screen when it was already open.");
			return;
		}
		Vector3 position = new Vector3(0f, buttonStartY, 0f);
		List<UiElement> list = new List<UiElement>();
		for (int i = 0; i < options.Count; i++)
		{
			int buttonIndex = i;
			string text = options[i];
			DropdownOption dropdownOption = optionPool.Get<DropdownOption>();
			dropdownOption.Initialize(text, () =>
			{
				SelectOption(buttonIndex);
			}, clickMask, position);
			list.Add(dropdownOption.ButtonUiElement);
			position.y -= buttonHeight;
		}
		optionsScreen.SetActive(value: true);
		Vector3 localPosition = transform.localPosition;
		optionsScreen.transform.localPosition = new Vector3(0f - localPosition.x, 0f - localPosition.y, optionsScreen.transform.localPosition.z);
		ControllerManager.Instance.OpenOverlayMenu(DropdownName, closeButton, list[SelectedIndex], list);
	}

	private void SelectOption(int buttonIndex)
	{
		Close();
		SetSelectedIndex(buttonIndex);
		onOptionSelect(buttonIndex);
	}

	private void Close()
	{
		ControllerManager.Instance.CloseOverlayMenu(DropdownName);
		optionsScreen.SetActive(value: false);
		optionPool.ReclaimAll();
	}
}
