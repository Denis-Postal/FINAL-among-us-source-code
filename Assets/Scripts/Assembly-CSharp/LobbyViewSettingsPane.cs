using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LobbyViewSettingsPane : MonoBehaviour
{
	[SerializeField]
	private PassiveButton rolesTabButton;

	[SerializeField]
	private PassiveButton taskTabButton;

	[SerializeField]
	private ViewSettingsInfoPanel infoPanelOrigin;

	[SerializeField]
	private Scroller scrollBar;

	[SerializeField]
	private CategoryHeaderMasked categoryHeaderOrigin;

	[SerializeField]
	private CategoryHeaderRoleVariant categoryHeaderRoleOrigin;

	[SerializeField]
	private ViewSettingsInfoPanelRoleVariant infoPanelRoleOrigin;

	[SerializeField]
	private AdvancedRoleViewPanel advancedRolePanelOrigin;

	[SerializeField]
	private PassiveButton backButton;

	[SerializeField]
	private Transform settingsContainer;

	[SerializeField]
	private SpriteRenderer backgroundMask;

	[SerializeField]
	private SpriteRenderer areaMask;

	[SerializeField]
	private TextMeshPro gameModeText;

	private const float START_POS_Y = 1.44f;

	private const float START_POS_X = -8.95f;

	private const float START_POS_X_ROLES = -6.53f;

	private const float START_POS_Y_ROLES = 0.95f;

	private const float START_POS_X_ROLES_ADVANCED = -5.8f;

	private const float SPACING_X = 5.95f;

	private const float SPACING_Y = 0.85f;

	private const float SPACING_Y_ROLES = 0.664f;

	private const float HEADER_SPACING_Y = 1.05f;

	private const float HEADER_START_X = -9.77f;

	private const float HEADER_SPACING_Y_ROLES = 0.696f;

	private const int MASK_LAYER = 61;

	private List<GameObject> settingsInfo;

	private StringNames currentTab;

	[SerializeField]
	private UiElement BackButton;

	[SerializeField]
	private UiElement DefaultButtonSelected;

	[SerializeField]
	private List<UiElement> ControllerSelectable;

	private void Awake()
	{
		settingsInfo = new List<GameObject>();
		rolesTabButton.gameObject.SetActive(!GameManager.Instance.IsHideAndSeek());
		taskTabButton.gameObject.SetActive(!GameManager.Instance.IsHideAndSeek());
		rolesTabButton.OnClick.AddListener(() =>
		{
			ChangeTab(StringNames.RolesCategory);
		});
		taskTabButton.OnClick.AddListener(() =>
		{
			ChangeTab(StringNames.OverviewCategory);
		});
		backgroundMask.material.SetInt(PlayerMaterial.MaskLayer, 61);
		areaMask.material.SetInt(PlayerMaterial.MaskLayer, 61);
		gameModeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameManager.Instance.GameSettingsList.GameModeName);
	}

	private void OnEnable()
	{
		ChangeTab(StringNames.OverviewCategory);
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable);
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			backButton.OnClick.Invoke();
		}
	}

	public void ChangeTab(StringNames category)
	{
		currentTab = category;
		for (int i = 0; i < settingsInfo.Count; i++)
		{
			Object.Destroy(settingsInfo[i].gameObject);
		}
		settingsInfo.Clear();
		SetTab();
		scrollBar.ScrollToTop();
	}

	public void SetTab()
	{
		if (currentTab == StringNames.RolesCategory)
		{
			rolesTabButton.SelectButton(isSelected: true);
			taskTabButton.SelectButton(isSelected: false);
			DrawRolesTab();
		}
		else
		{
			taskTabButton.SelectButton(isSelected: true);
			rolesTabButton.SelectButton(isSelected: false);
			DrawNormalTab();
		}
	}

	public void RefreshTab()
	{
		for (int i = 0; i < settingsInfo.Count; i++)
		{
			Object.Destroy(settingsInfo[i].gameObject);
		}
		settingsInfo.Clear();
		SetTab();
	}

	private unsafe void DrawRolesTab()
	{
		float num = 0.95f;
		float x = -6.53f;
		CategoryHeaderMasked categoryHeaderMasked = Object.Instantiate(categoryHeaderOrigin);
		categoryHeaderMasked.SetHeader(StringNames.RoleQuotaLabel, 61);
		categoryHeaderMasked.transform.SetParent(settingsContainer);
		categoryHeaderMasked.transform.localScale = Vector3.one;
		categoryHeaderMasked.transform.localPosition = new Vector3(-9.77f, 1.26f, -2f);
		settingsInfo.Add(categoryHeaderMasked.gameObject);
		List<RoleBehaviour> list = new List<RoleBehaviour>();
		for (int i = 0; i < 2; i++)
		{
			CategoryHeaderRoleVariant categoryHeaderRoleVariant = Object.Instantiate(categoryHeaderRoleOrigin);
			categoryHeaderRoleVariant.SetHeader((i == 0) ? StringNames.CrewmateRolesHeader : StringNames.ImpostorRolesHeader, 61);
			categoryHeaderRoleVariant.transform.SetParent(settingsContainer);
			categoryHeaderRoleVariant.transform.localScale = Vector3.one;
			categoryHeaderRoleVariant.transform.localPosition = new Vector3(0.09f, num, -2f);
			settingsInfo.Add(categoryHeaderRoleVariant.gameObject);
			num -= 0.696f;
			for (int j = 0; j < DestroyableSingleton<RoleManager>.Instance.AllRoles.Count; j++)
			{
				RoleBehaviour roleBehaviour = DestroyableSingleton<RoleManager>.Instance.AllRoles[j];
				if ((i != 0 || roleBehaviour.TeamType != RoleTeamTypes.Impostor) && (i != 1 || roleBehaviour.TeamType != RoleTeamTypes.Crewmate) && roleBehaviour.Role != RoleTypes.ImpostorGhost && roleBehaviour.Role != RoleTypes.CrewmateGhost && roleBehaviour.Role != RoleTypes.Crewmate && roleBehaviour.Role != RoleTypes.Impostor)
				{
					int chancePerGame = GameOptionsManager.Instance.CurrentGameOptions.RoleOptions.GetChancePerGame(roleBehaviour.Role);
					int numPerGame = GameOptionsManager.Instance.CurrentGameOptions.RoleOptions.GetNumPerGame(roleBehaviour.Role);
					bool flag = numPerGame == 0;
					ViewSettingsInfoPanelRoleVariant viewSettingsInfoPanelRoleVariant = Object.Instantiate(infoPanelRoleOrigin);
					viewSettingsInfoPanelRoleVariant.transform.SetParent(settingsContainer);
					viewSettingsInfoPanelRoleVariant.transform.localScale = Vector3.one;
					viewSettingsInfoPanelRoleVariant.transform.localPosition = new Vector3(x, num, -2f);
					if (!flag)
					{
						list.Add(roleBehaviour);
					}
					viewSettingsInfoPanelRoleVariant.SetInfo(roleBehaviour.NiceName, numPerGame, chancePerGame, 61, (i == 0) ? Palette.CrewmateRoleBlue : Palette.ImpostorRoleRed, roleBehaviour.RoleIconSolid, i == 0, flag);
					settingsInfo.Add(viewSettingsInfoPanelRoleVariant.gameObject);
					num -= 0.664f;
				}
			}
		}
		if (list.Count > 0)
		{
			CategoryHeaderMasked categoryHeaderMasked2 = Object.Instantiate(categoryHeaderOrigin);
			categoryHeaderMasked2.SetHeader(StringNames.RoleSettingsLabel, 61);
			categoryHeaderMasked2.transform.SetParent(settingsContainer);
			categoryHeaderMasked2.transform.localScale = Vector3.one;
			categoryHeaderMasked2.transform.localPosition = new Vector3(-9.77f, num, -2f);
			settingsInfo.Add(categoryHeaderMasked2.gameObject);
			num -= 2.1f;
			float num2 = 0f;
			float num3 = 0f;
			for (int k = 0; k < list.Count; k++)
			{
				if (k % 2 == 0)
				{
					num2 = -5.8f;
					if (k > 0)
					{
						num -= num3 + 0.85f;
						num3 = 0f;
					}
				}
				else
				{
					num2 = 0.14999962f;
				}
				AdvancedRoleViewPanel advancedRoleViewPanel = Object.Instantiate(advancedRolePanelOrigin);
				advancedRoleViewPanel.transform.SetParent(settingsContainer);
				advancedRoleViewPanel.transform.localScale = Vector3.one;
				advancedRoleViewPanel.transform.localPosition = new Vector3(num2, num, -2f);
				float num4 = advancedRoleViewPanel.SetUp(list[k], 0.85f, 61);
				if (num4 > num3)
				{
					num3 = num4;
				}
				settingsInfo.Add(advancedRoleViewPanel.gameObject);
			}
		}
		scrollBar.SetYBoundsMax(0f - num);
	}

	private unsafe void DrawNormalTab()
	{
		float num = 1.44f;
		float num2 = -8.95f;
		foreach (RulesCategory allCategory in GameManager.Instance.GameSettingsList.AllCategories)
		{
			CategoryHeaderMasked categoryHeaderMasked = Object.Instantiate(categoryHeaderOrigin);
			categoryHeaderMasked.SetHeader(allCategory.CategoryName, 61);
			categoryHeaderMasked.transform.SetParent(settingsContainer);
			categoryHeaderMasked.transform.localScale = Vector3.one;
			categoryHeaderMasked.transform.localPosition = new Vector3(-9.77f, num, -2f);
			settingsInfo.Add(categoryHeaderMasked.gameObject);
			num -= 1.05f;
			for (int i = 0; i < allCategory.AllGameSettings.Count; i++)
			{
				ViewSettingsInfoPanel viewSettingsInfoPanel = Object.Instantiate(infoPanelOrigin);
				viewSettingsInfoPanel.transform.SetParent(settingsContainer);
				viewSettingsInfoPanel.transform.localScale = Vector3.one;
				if (i % 2 == 0)
				{
					num2 = -8.95f;
					if (i > 0)
					{
						num -= 0.85f;
					}
				}
				else
				{
					num2 = -3f;
				}
				viewSettingsInfoPanel.transform.localPosition = new Vector3(num2, num, -2f);
				float value = GameOptionsManager.Instance.CurrentGameOptions.GetValue(allCategory.AllGameSettings[i]);
				if (allCategory.AllGameSettings[i].Type == OptionTypes.Checkbox)
				{
					viewSettingsInfoPanel.SetInfoCheckbox(allCategory.AllGameSettings[i].Title, 61, value > 0f);
				}
				else
				{
					viewSettingsInfoPanel.SetInfo(allCategory.AllGameSettings[i].Title, allCategory.AllGameSettings[i].GetValueString(value), 61);
				}
				settingsInfo.Add(viewSettingsInfoPanel.gameObject);
			}
			num -= 0.85f;
		}
		scrollBar.CalculateAndSetYBounds(settingsInfo.Count + 10, 2f, 6f, 0.85f);
	}

	private void _003CAwake_003Eb__30_0()
	{
		ChangeTab(StringNames.RolesCategory);
	}

	private void _003CAwake_003Eb__30_1()
	{
		ChangeTab(StringNames.OverviewCategory);
	}
}
