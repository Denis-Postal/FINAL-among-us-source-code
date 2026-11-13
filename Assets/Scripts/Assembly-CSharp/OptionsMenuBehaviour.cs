using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class OptionsMenuBehaviour : MonoBehaviour, ITranslatedText
{
	public SpriteRenderer Background;

	public SpriteRenderer JoystickButton;

	public SpriteRenderer TouchButton;

	public SlideBar JoystickSizeSlider;

	public FloatRange JoystickSizes= new FloatRange(0.5f, 1.5f);

	public SlideBar SoundSlider;

	public SlideBar MusicSlider;

	public ToggleButtonBehaviour CensorChatButton;

	public ToggleButtonBehaviour DisableMouseMovement;

	public ToggleButtonBehaviour EnableFriendInvitesButton;

	public ToggleButtonBehaviour ColorBlindButton;

	public GameObject KeyboardOptions;

	public GameObject MouseAndKeyboardOptions;

	public ToggleButtonBehaviour StreamerModeButton;

	public GameObject TermsOfUsePlayStationPopup;

	public TextMeshPro TermsOfUsePlayStationEULAText;

	public BugReportPopup bugReportPopup;

	public bool Toggle= true;

	public TabGroup[] Tabs;

	public PassiveButton MenuButton;

	private bool grabbedControllerButtons;

	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public List<UiElement> IgnoreControllerSelection;

	public bool IsOpen => isActiveAndEnabled;

	public void OpenTabGroup(int index)
	{
		for (int i = 0; i < Tabs.Length; i++)
		{
			TabGroup tabGroup = Tabs[i];
			if (i == index)
			{
				tabGroup.Open();
			}
			else
			{
				tabGroup.Close();
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape) && !KeyboardOptions.activeSelf)
		{
			Close();
		}
		int inputMode = (int)DataManager.Settings.Input.InputMode;
		inputMode--;
		KeyboardOptions.gameObject.SetActive(inputMode == 1 && ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick);
		MouseAndKeyboardOptions.SetActive(ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick);
	}

	public void Start()
	{ /* spliced-from-2019 */
		DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Add(this);
		if (!grabbedControllerButtons)
		{
			grabbedControllerButtons = true;
			GrabControllerButtons();
		}
	}

	private void GrabControllerButtons()
	{
		ControllerSelectable.Clear();
		UiElement[] componentsInChildren = GetComponentsInChildren<UiElement>(includeInactive: true);
		foreach (UiElement item in componentsInChildren)
		{
			if (!IgnoreControllerSelection.Contains(item))
			{
				ControllerSelectable.Add(item);
			}
		}
	}

	private void OnDisable()
	{
		MenuButton?.SelectButton(isSelected: false);
		DataManager.Settings.Save();
	}

	public void OnDestroy()
	{
		if (DestroyableSingleton<TranslationController>.InstanceExists)
		{
			DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Remove(this);
		}
	}

	public void ResetText()
	{
		JoystickButton.transform.parent.GetComponentInChildren<TextMeshPro>().text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsMouseMode);
		TouchButton.transform.parent.GetComponentInChildren<TextMeshPro>().text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsKeyboardMode);
		JoystickSizeSlider.gameObject.SetActive(value: false);
	}

	public void Open()
	{
		if ((DestroyableSingleton<HudManager>.InstanceExists && !DestroyableSingleton<HudManager>.Instance.SettingsButton.activeSelf) || ((bool)MatchInfoGuide.Instance && MatchInfoGuide.Instance.IsActive))
		{
			return;
		}
		ResetText();
		if (gameObject.activeSelf)
		{
			if (Toggle)
			{
				GetComponent<TransitionOpen>().Close();
			}
			return;
		}
		if (Minigame.Instance != null)
		{
			Minigame.Instance.Close();
		}
		OpenTabGroup(0);
		UpdateButtons();
		gameObject.SetActive(value: true);
		MenuButton?.SelectButton(isSelected: true);
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			ConsoleJoystick.SetMode_MenuAdditive();
		}
		if (!grabbedControllerButtons)
		{
			grabbedControllerButtons = true;
			GrabControllerButtons();
		}
		ControllerManager.Instance.OpenOverlayMenu("OptionsMenu", BackButton, DefaultButtonSelected, ControllerSelectable);
	}

	public void SetControlType(int i)
	{
		ControlTypes controlTypes = (ControlTypes)(i + 1);
		Logger.GlobalInstance.Info(string.Format("[{0}] SetControlType: {1}", "OptionsMenuBehaviour", controlTypes));
		DataManager.Settings.Input.InputMode = controlTypes;
		UpdateButtons();
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.SetTouchType(controlTypes);
		}
	}

	public void UpdateJoystickSize()
	{
		float touchJoystickSize = JoystickSizes.Lerp(JoystickSizeSlider.Value);
		DataManager.Settings.Input.TouchJoystickSize = touchJoystickSize;
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.SetJoystickSize(DataManager.Settings.Input.TouchJoystickSize);
		}
	}

	public void UpdateSfxVolume()
	{
		DataManager.Settings.Audio.SfxVolume = SoundSlider.Value;
	}

	public void UpdateMusicVolume()
	{
		DataManager.Settings.Audio.MusicVolume = MusicSlider.Value;
	}

	public void OpenPrivacyPolicy()
	{
		Constants.OpenURL(Constants.PrivacyPolicyURL);
	}

	public void OpenTermsOfUse()
	{
		ReallyOpenTermsOfUse();
	}

	public void ReallyOpenTermsOfUse()
	{
		Constants.OpenURL(Constants.TermsOfUseURL);
	}

	public void TogglePersonalizedAd()
	{
		Close();
		Object.FindObjectOfType<MainMenuManager>().AdsPolicy.ForceShow();
	}

	public void ToggleCensorChat()
	{
		DataManager.Settings.Multiplayer.CensorChat = !DataManager.Settings.Multiplayer.CensorChat;
		UpdateButtons();
	}

	public void ToggleFriendsListInvites()
	{
		DataManager.Settings.Multiplayer.AllowFriendInvites = !DataManager.Settings.Multiplayer.AllowFriendInvites;
		UpdateButtons();
	}

	public void ToggleMouseMovement()
	{
		DataManager.Settings.Input.MouseMovementEnabled = !DataManager.Settings.Input.MouseMovementEnabled;
		UpdateButtons();
	}

	public void ToggleStreamerMode()
	{
		DataManager.Settings.Gameplay.StreamerMode = !DataManager.Settings.Gameplay.StreamerMode;
		UpdateButtons();
	}

	public void ToggleColorBlind()
	{
		DataManager.Settings.Accessibility.ColorBlindMode = !DataManager.Settings.Accessibility.ColorBlindMode;
		UpdateButtons();
	}

	public void OpenBugReportPopup()
	{
		bugReportPopup.gameObject.SetActive(value: true);
	}

	public unsafe void UpdateButtons()
	{
		if (DataManager.Settings.Input.InputMode - 1 == ControlTypes.VirtualJoystick)
		{
			JoystickButton.color = Palette.AcceptedGreen;
			TouchButton.color = Color.white;
			JoystickSizeSlider.enabled = true;
			JoystickSizeSlider.SetEnabledColors();
		}
		else
		{
			JoystickButton.color = Color.white;
			TouchButton.color = Palette.AcceptedGreen;
			JoystickSizeSlider.enabled = false;
			JoystickSizeSlider.SetDisabledColors();
		}
		JoystickSizeSlider.Value = JoystickSizes.ReverseLerp(DataManager.Settings.Input.TouchJoystickSize);
		SoundSlider.Value = DataManager.Settings.Audio.SfxVolume;
		MusicSlider.Value = DataManager.Settings.Audio.MusicVolume;
		CensorChatButton.UpdateText(DataManager.Settings.Multiplayer.CensorChat);
		EnableFriendInvitesButton.UpdateText(DataManager.Settings.Multiplayer.AllowFriendInvites);
		DisableMouseMovement.UpdateText(DataManager.Settings.Input.MouseMovementEnabled);
		ColorBlindButton.UpdateText(DataManager.Settings.Accessibility.ColorBlindMode);
		StreamerModeButton.UpdateText(DataManager.Settings.Gameplay.StreamerMode);
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
		ControllerManager.Instance.CloseOverlayMenu("OptionsMenu");
	}

	public OptionsMenuBehaviour()
	{ }
}
