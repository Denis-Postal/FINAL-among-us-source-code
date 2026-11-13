using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

public class GameOptionsMenu : MonoBehaviour
{
	private IGameOptions cachedData;

	private List<OptionBehaviour> Children;

	[SerializeField]
	private GameOptionsMapPicker MapPicker;

	[SerializeField]
	private CategoryHeaderMasked categoryHeaderOrigin;

	[SerializeField]
	private ToggleOption checkboxOrigin;

	[SerializeField]
	private NumberOption numberOptionOrigin;

	[SerializeField]
	private PlayerOption playerOptionOrigin;

	[SerializeField]
	private StringOption stringOptionOrigin;

	[SerializeField]
	private Transform settingsContainer;

	[SerializeField]
	private Scroller scrollBar;

	[SerializeField]
	private Transform[] HideForOnline;

	[SerializeField]
	private RolesSettingsMenu RolesMenu;

	[SerializeField]
	private Collider2D ButtonClickMask;

	[SerializeField]
	private SpriteRenderer MaskBg;

	[SerializeField]
	private SpriteRenderer MaskArea;

	private const float START_POS_Y = 0.713f;

	private const float START_POS_X = 0.952f;

	private const float HEADER_HEIGHT = 0.63f;

	private const float MAP_PICKER_HEIGHT = 1.65f;

	private const float SPACING_Y = 0.45f;

	private const float HEADER_X = -0.903f;

	private const int MASK_LAYER = 20;

	private const float HEADER_SCALE = 0.63f;

	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private void Awake()
	{
		MaskBg.material.SetInt(PlayerMaterial.MaskLayer, 20);
		MaskArea.material.SetInt(PlayerMaterial.MaskLayer, 20);
	}

	private unsafe void CreateSettings()
	{
		float num = 0.713f;
		bool isHnS = GameManager.Instance.IsHideAndSeek();
		bool addedHnSImpostors = false;
		foreach (RulesCategory allCategory in GameManager.Instance.GameSettingsList.AllCategories)
		{
			CategoryHeaderMasked categoryHeaderMasked = Object.Instantiate(categoryHeaderOrigin, Vector3.zero, Quaternion.identity, settingsContainer);
			categoryHeaderMasked.SetHeader(allCategory.CategoryName, 20);
			categoryHeaderMasked.transform.localScale = Vector3.one * 0.63f;
			categoryHeaderMasked.transform.localPosition = new Vector3(-0.903f, num, -2f);
			num -= 0.63f;
			if (isHnS && !addedHnSImpostors)
			{
				BaseGameSetting impSetting = null;
				if (GameManager.Instance.AllGameSettingData != null)
				{
					GameManager.Instance.AllGameSettingData.TryGetValue(StringNames.GameNumImpostors, out impSetting);
				}
				if (impSetting == null)
				{
					IntGameSetting[] allIntSettings = Resources.FindObjectsOfTypeAll<IntGameSetting>();
					foreach (IntGameSetting s in allIntSettings)
					{
						if (s != null && s.Title == StringNames.GameNumImpostors)
						{
							impSetting = s;
							break;
						}
					}
				}
				if (impSetting != null)
				{
					OptionBehaviour optionBehaviour = Object.Instantiate(numberOptionOrigin, Vector3.zero, Quaternion.identity, settingsContainer);
					optionBehaviour.transform.localPosition = new Vector3(0.952f, num, -2f);
					optionBehaviour.SetClickMask(ButtonClickMask);
					optionBehaviour.SetUpFromData(impSetting, 20);
					Children.Add(optionBehaviour);
					num -= 0.45f;
					addedHnSImpostors = true;
				}
			}
			foreach (BaseGameSetting allGameSetting in allCategory.AllGameSettings)
			{
				switch (allGameSetting.Type)
				{
				case OptionTypes.Float:
				case OptionTypes.Int:
				{
					OptionBehaviour optionBehaviour = Object.Instantiate(numberOptionOrigin, Vector3.zero, Quaternion.identity, settingsContainer);
					optionBehaviour.transform.localPosition = new Vector3(0.952f, num, -2f);
					optionBehaviour.SetClickMask(ButtonClickMask);
					optionBehaviour.SetUpFromData(allGameSetting, 20);
					Children.Add(optionBehaviour);
					break;
				}
				case OptionTypes.Checkbox:
				{
					OptionBehaviour optionBehaviour = Object.Instantiate(checkboxOrigin, Vector3.zero, Quaternion.identity, settingsContainer);
					optionBehaviour.transform.localPosition = new Vector3(0.952f, num, -2f);
					optionBehaviour.SetClickMask(ButtonClickMask);
					optionBehaviour.SetUpFromData(allGameSetting, 20);
					Children.Add(optionBehaviour);
					break;
				}
				case OptionTypes.String:
				{
					OptionBehaviour optionBehaviour = Object.Instantiate(stringOptionOrigin, Vector3.zero, Quaternion.identity, settingsContainer);
					optionBehaviour.transform.localPosition = new Vector3(0.952f, num, -2f);
					optionBehaviour.SetClickMask(ButtonClickMask);
					optionBehaviour.SetUpFromData(allGameSetting, 20);
					Children.Add(optionBehaviour);
					break;
				}
				case OptionTypes.Player:
				{
					OptionBehaviour optionBehaviour = Object.Instantiate(playerOptionOrigin, Vector3.zero, Quaternion.identity, settingsContainer);
					optionBehaviour.transform.localPosition = new Vector3(0.952f, num, -2f);
					optionBehaviour.SetClickMask(ButtonClickMask);
					optionBehaviour.SetUpFromData(allGameSetting, 20);
					Children.Add(optionBehaviour);
					break;
				}
				}
				num -= 0.45f;
			}
		}
		ControllerSelectable.AddRange(scrollBar.GetComponentsInChildren<UiElement>());
		scrollBar.SetYBoundsMax(0f - num - 1.65f);
	}

	private void Initialize()
	{
		if (Children != null && Children.Count != 0)
		{
			return;
		}
		MapPicker.Initialize(20);
		BaseGameSetting mapNameSetting = GameManager.Instance.GameSettingsList.MapNameSetting;
		MapPicker.SetUpFromData(mapNameSetting, 20);
		Children = new List<OptionBehaviour>();
		Children.Add(MapPicker);
		CreateSettings();
		cachedData = GameOptionsManager.Instance.CurrentGameOptions;
		for (int i = 0; i < Children.Count; i++)
		{
			OptionBehaviour optionBehaviour = Children[i];
			optionBehaviour.OnValueChanged = ValueChanged;
			if ((bool)AmongUsClient.Instance && !AmongUsClient.Instance.AmHost)
			{
				optionBehaviour.SetAsPlayer();
			}
		}
		InitializeControllerNavigation();
	}

	private void Update()
	{
		if (cachedData != GameOptionsManager.Instance.CurrentGameOptions)
		{
			cachedData = GameOptionsManager.Instance.CurrentGameOptions;
			RefreshChildren();
		}
	}

	private void OnEnable()
	{
		Initialize();
	}

	private void OnDisable()
	{
		CloseMenu();
	}

	public void OpenMenu()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, MapPicker.MapButtons[0].Button, ControllerSelectable);
	}

	public void CloseMenu()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void RefreshChildren()
	{
		if (Children != null && Children.Count != 0)
		{
			for (int i = 0; i < Children.Count; i++)
			{
				Children[i].Initialize();
			}
		}
	}

	private void ValueChanged(OptionBehaviour option)
	{
		if ((bool)AmongUsClient.Instance && AmongUsClient.Instance.AmHost)
		{
			_ = GameOptionsManager.Instance.CurrentGameOptions;
			if (option.Title != StringNames.GameMapName)
			{
				GameOptionsManager.Instance.CurrentGameOptions.SetInt(Int32OptionNames.RulePreset, 100);
			}
			GameOptionsManager.Instance.CurrentGameOptions.SetBool(BoolOptionNames.IsDefaults, value: false);
			GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
			GameManager.Instance.LogicOptions.SyncOptions();
			float value = GameOptionsManager.Instance.CurrentGameOptions.GetValue(option.Data);
			DestroyableSingleton<HudManager>.Instance.Notifier.AddSettingsChangeMessage(option.Title, option.GetValueString(value), playSound: false);
		}
	}

	private void InitializeControllerNavigation()
	{
		for (int i = 0; i < Children.Count; i++)
		{
			OptionBehaviour optionBehaviour = Children[i];
			if (!optionBehaviour.gameObject.activeSelf)
			{
				continue;
			}
			UiElement[] array = null;
			UiElement[] array2 = null;
			if (i - 1 >= 0)
			{
				array = Children[i - 1].GetComponentsInChildren<UiElement>(includeInactive: true);
			}
			if (i + 1 < Children.Count)
			{
				array2 = Children[i + 1].GetComponentsInChildren<UiElement>(includeInactive: true);
			}
			UiElement[] componentsInChildren = optionBehaviour.GetComponentsInChildren<UiElement>(includeInactive: true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				UiElement uiElement = componentsInChildren[j];
				uiElement.ControllerNav.mode = ControllerNavigation.Mode.Explicit;
				if (array != null && array.Length != 0)
				{
					if (i == 1)
					{
						uiElement.ControllerNav.selectOnUp = array[0];
					}
					else if (j < array.Length)
					{
						uiElement.ControllerNav.selectOnUp = array[j];
					}
					else
					{
						uiElement.ControllerNav.selectOnUp = array[0];
					}
				}
				else
				{
					uiElement.ControllerNav.selectOnUp = null;
				}
				if (array2 != null && array2.Length != 0)
				{
					if (i == 0)
					{
						uiElement.ControllerNav.selectOnDown = array2[0];
					}
					else if (j < array2.Length)
					{
						uiElement.ControllerNav.selectOnDown = array2[j];
					}
					else
					{
						uiElement.ControllerNav.selectOnDown = array2[0];
					}
				}
				else
				{
					uiElement.ControllerNav.selectOnDown = null;
				}
			}
		}
	}

	public void ClickPresetButton(RulesPresets preset)
	{
		GameOptionsManager.Instance.CurrentGameOptions.SetRecommendations(GameData.Instance.PlayerCount, AmongUsClient.Instance.NetworkMode, preset);
		RefreshChildren();
		RolesMenu.RefreshChildren();
		GameOptionsManager.Instance.CurrentGameOptions.SetBool(BoolOptionNames.IsDefaults, value: false);
		GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
		GameManager.Instance.LogicOptions.SyncOptions();
		DestroyableSingleton<HudManager>.Instance.Notifier.AddSettingsChangeMessage(StringNames.ModeLabel, DestroyableSingleton<TranslationController>.Instance.GetString(GameOptionsManager.Instance.CurrentGameOptions.GetRulesPresetTitle()), playSound: false);
	}
}
