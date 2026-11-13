using System.Collections.Generic;
using AmongUs.GameOptions;
using Rewired;
using TMPro;
using UnityEngine;

public class RolesSettingsMenu : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer MaskBg;

	[SerializeField]
	private SpriteRenderer MaskArea;

	[SerializeField]
	private CategoryHeaderMasked quotaHeader;

	[SerializeField]
	private CategoryHeaderMasked advHeader;

	[SerializeField]
	private RoleSettingsTabButton roleSettingsTabButtonOrigin;

	[SerializeField]
	private RoleSettingsTabButton roleSettingsTabButtonOriginImpostor;

	[SerializeField]
	private Transform roleSettingsTabParent;

	[SerializeField]
	private Scroller roleSettingsTabScroller;

	[SerializeField]
	private RoleOptionSetting roleOptionSettingOrigin;

	[SerializeField]
	private GameObject RoleChancesSettings;

	[SerializeField]
	private CategoryHeaderEditRole categoryHeaderEditRoleOrigin;

	[SerializeField]
	private ToggleOption checkboxOrigin;

	[SerializeField]
	private NumberOption numberOptionOrigin;

	[SerializeField]
	private StringOption stringOptionOrigin;

	[SerializeField]
	private TextMeshPro roleTitleText;

	[SerializeField]
	private TextMeshPro roleDescriptionText;

	[SerializeField]
	private GameObject AdvancedRolesSettings;

	[SerializeField]
	private SpriteRenderer roleScreenshot;

	[SerializeField]
	private SpriteRenderer roleHeaderSprite;

	[SerializeField]
	private TextMeshPro roleHeaderText;

	[SerializeField]
	private Scroller scrollBar;

	[SerializeField]
	private Collider2D ButtonClickMask;

	[SerializeField]
	private Scroller roleTabsScroller;

	[SerializeField]
	private GameObject roleTabsGradient;

	[SerializeField]
	private Transform roleTabsMinPos;

	[SerializeField]
	private Transform roleTabsMaxPos;

	private List<OptionBehaviour> advancedSettingChildren;

	private List<RoleOptionSetting> roleChances;

	private List<PassiveButton> roleTabs;

	private int selectedRoleTab;

	private readonly Color glyphUnavailableColor = new Color(0.28f, 0.28f, 0.28f, 0.95f);

	private const float ROLE_HEADER_OFFSET = 0.522f;

	private const float X_START_ROLE_HEADER = 4.986f;

	private const float X_START_CHANCE = -0.15f;

	private const float Y_START = 0.662f;

	private const float Y_OFFSET = -0.43f;

	private const float X_START = -1.928f;

	private const float X_OFFSET = 0.762f;

	private const float TAB_Y_START = 2.275f;

	private const float Y_ADVANCED_START = -0.872f;

	private const float Y_ADVANCED_OFFSET = -0.45f;

	private const float X_ADVANCED_START = 2.17f;

	private const int MASK_LAYER = 20;

	private IRoleOptionsCollection cachedData;

	[SerializeField]
	[Header("Console Controller Navigation")]
	private UiElement BackButton;

	[SerializeField]
	private UiElement DefaultButtonSelected;

	[SerializeField]
	private PassiveButton AllButton;

	[SerializeField]
	private List<UiElement> ControllerSelectable;

	[SerializeField]
	private SpriteRenderer glyphL;

	[SerializeField]
	private SpriteRenderer glyphR;

	private List<UiElement> QuotaTabSelectables;

	private PassiveButton currentTabButton;

	private void Awake()
	{
		MaskBg.material.SetInt(PlayerMaterial.MaskLayer, 20);
		MaskArea.material.SetInt(PlayerMaterial.MaskLayer, 20);
		quotaHeader.SetHeader(StringNames.RoleQuotaLabel, 20);
		advHeader.SetHeader(StringNames.RoleSettingsLabel, 20);
	}

	private void InitialSetup()
	{
		QuotaTabSelectables = new List<UiElement>();
		advancedSettingChildren = new List<OptionBehaviour>();
		roleChances = new List<RoleOptionSetting>();
		SetQuotaTab();
	}

	private void OnEnable()
	{
		RoleChancesSettings.SetActive(value: true);
		AdvancedRolesSettings.SetActive(value: false);
		RefreshChildren();
		SpriteRenderer[] componentsInChildren = AdvancedRolesSettings.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, 20);
		}
		TextMeshPro[] componentsInChildren2 = AdvancedRolesSettings.GetComponentsInChildren<TextMeshPro>(includeInactive: true);
		foreach (TextMeshPro obj in componentsInChildren2)
		{
			obj.fontMaterial.SetFloat("_StencilComp", 3f);
			obj.fontMaterial.SetFloat("_Stencil", 20f);
		}
	}

	private void OnDisable()
	{
		CloseMenu();
	}

	public void OpenMenu(bool controllerSelected = true)
	{
		if (controllerSelected)
		{
			ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
			EnableTabControllerGlyphs(_enabled: true);
		}
		OpenChancesTab(controllerSelected);
	}

	public void CloseMenu()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		roleSettingsTabParent.localPosition = Vector3.zero;
		EnableTabControllerGlyphs(_enabled: false);
	}

	private void Update()
	{
		if (cachedData != GameOptionsManager.Instance.CurrentGameOptions.RoleOptions)
		{
			cachedData = GameOptionsManager.Instance.CurrentGameOptions.RoleOptions;
			RefreshChildren();
		}
		if (ControllerManager.Instance.CurrentUiState.MenuName == name)
		{
			Player player = ReInput.players.GetPlayer(0);
			bool flag = false;
			if (selectedRoleTab > 0 && player.GetButtonDown(35))
			{
				selectedRoleTab--;
				flag = true;
				if (roleTabs[selectedRoleTab].transform.position.x < roleTabsMinPos.position.x)
				{
					roleTabsScroller.ScrollRelative(new Vector2(0.762f, 0f));
				}
			}
			if (selectedRoleTab < roleTabs.Count - 1 && player.GetButtonDown(34))
			{
				selectedRoleTab++;
				flag = true;
				if (roleTabs[selectedRoleTab].transform.position.x > roleTabsMaxPos.position.x)
				{
					roleTabsScroller.ScrollRelative(new Vector2(-0.762f, 0f));
				}
			}
			if (flag)
			{
				if (selectedRoleTab == 0)
				{
					OpenChancesTab();
				}
				else
				{
					roleTabs[selectedRoleTab].OnClick.Invoke();
				}
			}
			if (selectedRoleTab > 0)
			{
				glyphL.color = Color.white;
			}
			else
			{
				glyphL.color = glyphUnavailableColor;
			}
			if (selectedRoleTab < roleTabs.Count - 1)
			{
				glyphR.color = Color.white;
			}
			else
			{
				glyphR.color = glyphUnavailableColor;
			}
		}
		if (roleTabsScroller.AtLeft)
		{
			roleTabsGradient.SetActive(value: false);
		}
		else
		{
			roleTabsGradient.SetActive(value: true);
		}
	}

	private void AddRoleTab(RoleBehaviour role, ref float tabXPos)
	{
		RoleSettingsTabButton tab;
		if (role.TeamType == RoleTeamTypes.Crewmate)
		{
			tab = Object.Instantiate(roleSettingsTabButtonOrigin, Vector3.zero, Quaternion.identity, roleSettingsTabParent);
		}
		else
		{
			tab = Object.Instantiate(roleSettingsTabButtonOriginImpostor, Vector3.zero, Quaternion.identity, roleSettingsTabParent);
		}
		tab.transform.localPosition = new Vector3(tabXPos, 2.275f, -2f);
		tab.SetButton(role, () =>
		{
			ChangeTab(role, tab.Button);
		});
		tabXPos += 0.762f;
		roleTabs.Add(tab.Button);
		if (roleTabs.Count > 8)
		{
			roleSettingsTabScroller.SetBounds(new FloatRange(0f, 0f), new FloatRange(0f - (float)(roleTabs.Count - 8) * 0.762f, 0f));
		}
	}

	public void SetQuotaTab()
	{
		float num = 0.662f;
		float tabXPos = -1.928f;
		roleTabs = new List<PassiveButton>();
		roleTabs.Add(AllButton);
		List<RoleBehaviour> list = DestroyableSingleton<RoleManager>.Instance.AllRoles.FindAll((RoleBehaviour r) => r.TeamType == RoleTeamTypes.Crewmate && r.Role != RoleTypes.Crewmate && r.Role != RoleTypes.CrewmateGhost);
		List<RoleBehaviour> list2 = DestroyableSingleton<RoleManager>.Instance.AllRoles.FindAll((RoleBehaviour r) => r.TeamType == RoleTeamTypes.Impostor && r.Role != RoleTypes.Impostor && r.Role != RoleTypes.ImpostorGhost);
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			AddRoleTab(list[num2], ref tabXPos);
		}
		for (int num3 = 0; num3 < list2.Count; num3++)
		{
			AddRoleTab(list2[num3], ref tabXPos);
		}
		CategoryHeaderEditRole categoryHeaderEditRole = Object.Instantiate(categoryHeaderEditRoleOrigin, Vector3.zero, Quaternion.identity, RoleChancesSettings.transform);
		categoryHeaderEditRole.SetHeader(StringNames.CrewmateRolesHeader, 20);
		categoryHeaderEditRole.transform.localPosition = new Vector3(4.986f, num, -2f);
		num -= 0.522f;
		int num4 = 0;
		for (int num5 = 0; num5 < list.Count; num5++)
		{
			CreateQuotaOption(list[num5], ref num, num4);
			num4++;
		}
		num -= 0.22f;
		CategoryHeaderEditRole categoryHeaderEditRole2 = Object.Instantiate(categoryHeaderEditRoleOrigin, Vector3.zero, Quaternion.identity, RoleChancesSettings.transform);
		categoryHeaderEditRole2.SetHeader(StringNames.ImpostorRolesHeader, 20);
		categoryHeaderEditRole2.transform.localPosition = new Vector3(4.986f, num, -2f);
		num -= 0.522f;
		for (int num6 = 0; num6 < list2.Count; num6++)
		{
			CreateQuotaOption(list2[num6], ref num, num4);
			num4++;
		}
	}

	private void CreateQuotaOption(RoleBehaviour role, ref float yPos, int index)
	{
		RoleOptionSetting roleOptionSetting = Object.Instantiate(roleOptionSettingOrigin, Vector3.zero, Quaternion.identity, RoleChancesSettings.transform);
		roleOptionSetting.transform.localPosition = new Vector3(-0.15f, yPos, -2f);
		roleOptionSetting.SetRole(GameOptionsManager.Instance.CurrentGameOptions.RoleOptions, role, 20);
		roleOptionSetting.OnValueChanged = ValueChanged;
		roleOptionSetting.SetClickMask(ButtonClickMask);
		roleChances.Add(roleOptionSetting);
		for (int i = 0; i < roleOptionSetting.ControllerSelectable.Count; i++)
		{
			if (index != 0)
			{
				roleOptionSetting.ControllerSelectable[i].ControllerNav.selectOnUp = roleChances[index - 1].ControllerSelectable[i];
				roleChances[index - 1].ControllerSelectable[i].ControllerNav.selectOnDown = roleOptionSetting.ControllerSelectable[i];
			}
		}
		QuotaTabSelectables.AddRange(roleOptionSetting.ControllerSelectable);
		if (index < DestroyableSingleton<RoleManager>.Instance.AllRoles.Count - 1)
		{
			yPos += -0.43f;
		}
	}

	private void ChangeTab(RoleBehaviour role, PassiveButton button)
	{
		currentTabButton?.SelectButton(isSelected: false);
		button.SelectButton(isSelected: true);
		currentTabButton = button;
		CreateAdvancedSettings(role);
		roleDescriptionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(role.BlurbNameMed);
		roleTitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(role.StringName);
		roleScreenshot.sprite = role.RoleScreenshot;
		if (role.TeamType == RoleTeamTypes.Crewmate)
		{
			roleHeaderSprite.color = Palette.CrewmateRoleHeaderBlue;
			roleHeaderText.color = Palette.CrewmateRoleHeaderTextBlue;
		}
		else
		{
			roleHeaderSprite.color = Palette.ImpostorRoleHeaderRed;
			roleHeaderText.color = Palette.ImpostorRoleHeaderTextRed;
		}
		for (int i = 0; i < advancedSettingChildren.Count; i++)
		{
			OptionBehaviour optionBehaviour = advancedSettingChildren[i];
			optionBehaviour.OnValueChanged = ValueChanged;
			if ((bool)AmongUsClient.Instance && !AmongUsClient.Instance.AmHost)
			{
				optionBehaviour.SetAsPlayer();
			}
		}
		RoleChancesSettings.SetActive(value: false);
		AdvancedRolesSettings.SetActive(value: true);
		RefreshChildren();
		ControllerManager.Instance.CurrentUiState.SelectableUiElements = ControllerSelectable;
		ControllerManager.Instance.SetDefaultSelection(ControllerSelectable[0]);
	}

	public void OpenChancesTab(bool controllerSelected = true)
	{
		selectedRoleTab = 0;
		RoleChancesSettings.SetActive(value: true);
		AdvancedRolesSettings.SetActive(value: false);
		ControllerSelectable.Clear();
		if (QuotaTabSelectables == null)
		{
			InitialSetup();
		}
		if (controllerSelected)
		{
			ControllerSelectable.AddRange(QuotaTabSelectables);
		}
		scrollBar.CalculateAndSetYBounds(roleChances.Count + 3, 1f, 6f, 0.43f);
		scrollBar.ScrollToTop();
		if (controllerSelected)
		{
			ControllerManager.Instance.CurrentUiState.SelectableUiElements = ControllerSelectable;
			ControllerManager.Instance.SetDefaultSelection(roleChances[0].ControllerSelectable[0]);
		}
		currentTabButton?.SelectButton(isSelected: false);
		AllButton.SelectButton(isSelected: true);
		currentTabButton = AllButton;
	}

	public void RefreshChildren()
	{
		if (advancedSettingChildren != null && advancedSettingChildren.Count > 0)
		{
			for (int i = 0; i < advancedSettingChildren.Count; i++)
			{
				OptionBehaviour optionBehaviour = advancedSettingChildren[i];
				optionBehaviour.enabled = false;
				optionBehaviour.enabled = true;
			}
		}
		if (roleChances != null && roleChances.Count > 0)
		{
			for (int j = 0; j < roleChances.Count; j++)
			{
				roleChances[j].UpdateValuesAndText(GameOptionsManager.Instance.CurrentGameOptions.RoleOptions);
			}
		}
	}

	private void ValueChanged(OptionBehaviour obj)
	{
		if (obj is RoleOptionSetting)
		{
			RoleOptionSetting roleOptionSetting = obj as RoleOptionSetting;
			GameOptionsManager.Instance.CurrentGameOptions.RoleOptions.SetRoleRate(roleOptionSetting.Role.Role, roleOptionSetting.RoleMaxCount, roleOptionSetting.RoleChance);
			roleOptionSetting.UpdateValuesAndText(GameOptionsManager.Instance.CurrentGameOptions.RoleOptions);
			DestroyableSingleton<HudManager>.Instance.Notifier.AddRoleSettingsChangeMessage(roleOptionSetting.Role.StringName, roleOptionSetting.RoleMaxCount, roleOptionSetting.RoleChance, roleOptionSetting.Role.TeamType, playSound: false);
		}
		else
		{
			float value = GameOptionsManager.Instance.CurrentGameOptions.GetValue(obj.Data);
			DestroyableSingleton<HudManager>.Instance.Notifier.AddSettingsChangeMessage(obj.Title, obj.GetValueString(value), playSound: false, obj.AssociatedRole);
		}
		GameOptionsManager.Instance.CurrentGameOptions.SetInt(Int32OptionNames.RulePreset, 100);
		GameOptionsManager.Instance.CurrentGameOptions.SetBool(BoolOptionNames.IsDefaults, value: false);
		RefreshChildren();
		GameOptionsManager.Instance.GameHostOptions = GameOptionsManager.Instance.CurrentGameOptions;
		GameManager.Instance.LogicOptions.SyncOptions();
	}

	private void CreateAdvancedSettings(RoleBehaviour role)
	{
		for (int i = 0; i < advancedSettingChildren.Count; i++)
		{
			Object.Destroy(advancedSettingChildren[i].gameObject);
		}
		ControllerSelectable.Clear();
		advancedSettingChildren.Clear();
		float num = -0.872f;
		foreach (BaseGameSetting allGameSetting in role.AllGameSettings)
		{
			switch (allGameSetting.Type)
			{
			case OptionTypes.Float:
			case OptionTypes.Int:
			{
				OptionBehaviour optionBehaviour = Object.Instantiate(numberOptionOrigin, Vector3.zero, Quaternion.identity, AdvancedRolesSettings.transform);
				optionBehaviour.transform.localPosition = new Vector3(2.17f, num, -2f);
				optionBehaviour.SetUpFromData(allGameSetting, 20);
				optionBehaviour.SetClickMask(ButtonClickMask);
				optionBehaviour.LabelBackground.enabled = false;
				optionBehaviour.AssociatedRole = role.Role;
				advancedSettingChildren.Add(optionBehaviour);
				ControllerSelectable.AddRange(optionBehaviour.GetComponentsInChildren<UiElement>(includeInactive: false));
				break;
			}
			case OptionTypes.Checkbox:
			{
				OptionBehaviour optionBehaviour = Object.Instantiate(checkboxOrigin, Vector3.zero, Quaternion.identity, AdvancedRolesSettings.transform);
				optionBehaviour.transform.localPosition = new Vector3(2.17f, num, -2f);
				optionBehaviour.SetUpFromData(allGameSetting, 20);
				optionBehaviour.SetClickMask(ButtonClickMask);
				optionBehaviour.LabelBackground.enabled = false;
				optionBehaviour.AssociatedRole = role.Role;
				advancedSettingChildren.Add(optionBehaviour);
				ControllerSelectable.AddRange(optionBehaviour.GetComponentsInChildren<UiElement>(includeInactive: false));
				break;
			}
			case OptionTypes.String:
			{
				OptionBehaviour optionBehaviour = Object.Instantiate(stringOptionOrigin, Vector3.zero, Quaternion.identity, AdvancedRolesSettings.transform);
				optionBehaviour.transform.localPosition = new Vector3(2.17f, num, -2f);
				optionBehaviour.SetUpFromData(allGameSetting, 20);
				optionBehaviour.SetClickMask(ButtonClickMask);
				optionBehaviour.LabelBackground.enabled = false;
				optionBehaviour.AssociatedRole = role.Role;
				advancedSettingChildren.Add(optionBehaviour);
				ControllerSelectable.AddRange(optionBehaviour.GetComponentsInChildren<UiElement>(includeInactive: false));
				break;
			}
			}
			num += -0.45f;
		}
		scrollBar.CalculateAndSetYBounds(role.AllGameSettings.Count + 3, 1f, 6f, 0.45f);
		InitializeControllerNavigation();
		scrollBar.ScrollToTop();
	}

	private void InitializeControllerNavigation()
	{
		for (int i = 0; i < advancedSettingChildren.Count; i++)
		{
			OptionBehaviour optionBehaviour = advancedSettingChildren[i];
			if (!optionBehaviour.gameObject.activeSelf)
			{
				continue;
			}
			UiElement[] array = null;
			UiElement[] array2 = null;
			if (i - 1 >= 0)
			{
				array = advancedSettingChildren[i - 1].GetComponentsInChildren<UiElement>(includeInactive: true);
			}
			if (i + 1 < advancedSettingChildren.Count)
			{
				array2 = advancedSettingChildren[i + 1].GetComponentsInChildren<UiElement>(includeInactive: true);
			}
			UiElement[] componentsInChildren = optionBehaviour.GetComponentsInChildren<UiElement>(includeInactive: true);
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				UiElement uiElement = componentsInChildren[j];
				uiElement.ControllerNav.mode = ControllerNavigation.Mode.Explicit;
				if (array != null && array.Length != 0)
				{
					if (j < array.Length)
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
					if (j < array2.Length)
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

	private void EnableTabControllerGlyphs(bool _enabled)
	{
		glyphL.enabled = _enabled;
		glyphR.enabled = _enabled;
	}
}
