using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSettingMenu : MonoBehaviour
{
	public static GameSettingMenu Instance;

	[SerializeField]
	private PassiveButton GamePresetsButton;

	[SerializeField]
	private PassiveButton GameSettingsButton;

	[SerializeField]
	private PassiveButton RoleSettingsButton;

	[SerializeField]
	private GamePresetsTab PresetsTab;

	[SerializeField]
	private GameOptionsMenu GameSettingsTab;

	[SerializeField]
	private RolesSettingsMenu RoleSettingsTab;

	[SerializeField]
	private TextMeshPro MenuDescriptionText;

	[SerializeField]
	private GameObject LeftSideDarkener;

	[SerializeField]
	private GameObject RightSideDarkener;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public void Close()
	{
		Instance = null;
		PresetsTab.CloseMenu();
		GameSettingsTab.CloseMenu();
		RoleSettingsTab.CloseMenu();
		ControllerManager.Instance.CloseOverlayMenu(name);
		Object.Destroy(gameObject);
	}

	private void Start()
	{
		if ((bool)Instance && Instance != this)
		{
			Object.Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
		GamePresetsButton.OnClick.AddListener(() =>
		{
			ChangeTab(0, previewOnly: false);
		});
		GamePresetsButton.OnMouseOver.AddListener(() =>
		{
			ChangeTab(0, previewOnly: true);
		});
		GameSettingsButton.OnClick.AddListener(() =>
		{
			ChangeTab(1, previewOnly: false);
		});
		GameSettingsButton.OnMouseOver.AddListener(() =>
		{
			ChangeTab(1, previewOnly: true);
		});
		RoleSettingsButton.OnClick.AddListener(() =>
		{
			ChangeTab(2, previewOnly: false);
		});
		RoleSettingsButton.OnMouseOver.AddListener(() =>
		{
			ChangeTab(2, previewOnly: true);
		});
		if (GameManager.Instance.IsHideAndSeek())
		{
			RoleSettingsButton.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if ((bool)ShipStatus.Instance)
		{
			Close();
		}
		else if (Controller.currentTouchType != Controller.TouchType.Joystick)
		{
			ToggleLeftSideDarkener(on: false);
			ToggleRightSideDarkener(on: false);
		}
	}

	private void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
		if (Controller.currentTouchType != Controller.TouchType.Joystick)
		{
			ChangeTab(0, Controller.currentTouchType == Controller.TouchType.Joystick);
		}
		StartCoroutine(CoSelectDefault());
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private IEnumerator CoSelectDefault()
	{
		yield return new WaitForEndOfFrame();
		ControllerManager.Instance.SetCurrentSelected(DefaultButtonSelected);
	}

	public void ChangeTab(int tabNum, bool previewOnly)
	{
		if ((previewOnly && Controller.currentTouchType == Controller.TouchType.Joystick) || !previewOnly)
		{
			PresetsTab.gameObject.SetActive(value: false);
			GameSettingsTab.gameObject.SetActive(value: false);
			RoleSettingsTab.gameObject.SetActive(value: false);
			GamePresetsButton.SelectButton(isSelected: false);
			GameSettingsButton.SelectButton(isSelected: false);
			RoleSettingsButton.SelectButton(isSelected: false);
			switch (tabNum)
			{
			case 0:
				PresetsTab.gameObject.SetActive(value: true);
				MenuDescriptionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GamePresetsDescription);
				break;
			case 1:
				GameSettingsTab.gameObject.SetActive(value: true);
				MenuDescriptionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameSettingsDescription);
				break;
			case 2:
				RoleSettingsTab.gameObject.SetActive(value: true);
				RoleSettingsTab.OpenMenu(controllerSelected: false);
				MenuDescriptionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoleSettingsDescription);
				break;
			}
		}
		if (!previewOnly)
		{
			ToggleLeftSideDarkener(on: true);
			ToggleRightSideDarkener(on: false);
			switch (tabNum)
			{
			case 0:
				PresetsTab.OpenMenu();
				GamePresetsButton.SelectButton(isSelected: true);
				break;
			case 1:
				GameSettingsTab.OpenMenu();
				GameSettingsButton.SelectButton(isSelected: true);
				break;
			case 2:
				RoleSettingsTab.OpenMenu();
				RoleSettingsButton.SelectButton(isSelected: true);
				break;
			}
		}
		else
		{
			ToggleLeftSideDarkener(on: false);
			ToggleRightSideDarkener(on: true);
		}
	}

	private void ToggleLeftSideDarkener(bool on)
	{
		LeftSideDarkener.SetActive((Controller.currentTouchType == Controller.TouchType.Joystick) & on);
	}

	private void ToggleRightSideDarkener(bool on)
	{
		RightSideDarkener.SetActive((Controller.currentTouchType == Controller.TouchType.Joystick) & on);
	}

	public void CloseTab()
	{
		ToggleLeftSideDarkener(on: false);
		ToggleRightSideDarkener(on: true);
	}
}
