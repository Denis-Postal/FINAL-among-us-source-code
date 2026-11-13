using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Rewired;
using UnityEngine;

public class MatchInfoGuide : MonoBehaviour
{
	[SerializeField]
	private GameObject MatchInfoParent;

	[SerializeField]
	private GameObject MatchInfoSettingPrefab;

	[SerializeField]
	private MatchInfoRolePanel MatchInfoRolePanelPrefab;

	[SerializeField]
	private ObjectPoolBehavior PlayerPool;

	[SerializeField]
	private Scroller MatchInfoRoleScroller;

	[SerializeField]
	private SpriteRenderer MatchInfoRoleMaskArea;

	[SerializeField]
	private List<MatchInfoGuideTabButton> TabButtons;

	[SerializeField]
	private List<GameObject> settingsTabs;

	[SerializeField]
	private SpriteRenderer matchInfoSettingsMaskArea;

	[SerializeField]
	private Transform settingsScrollArea;

	[SerializeField]
	private SpriteRenderer matchInfoPlayersMaskArea;

	[SerializeField]
	private GameObject rolesEnabledMessage;

	[SerializeField]
	private TransitionOpen transitionOpen;

	[SerializeField]
	private UiElement closeButton;

	[SerializeField]
	private SpriteRenderer glyphL;

	[SerializeField]
	private SpriteRenderer glyphR;

	private List<GameObject> NormalModeSettings = new List<GameObject>();

	private List<GameObject> HnSModeSettings = new List<GameObject>();

	private readonly float RoleEntryBoundsModifier = -0.08f;

	private Player player;

	private int activeTabIndex;

	private int numOfTabs;

	[SerializeField]
	[Header("Console Controller Navigation")]
	private List<UiElement> ControllerSelectable;

	public static MatchInfoGuide Instance { get; private set; }

	public bool IsActive => MatchInfoParent.activeSelf;

	private void Awake()
	{
		Instance = this;
		player = ReInput.players.GetPlayer(0);
	}

	private void Update()
	{
		if (IsActive)
		{
			if (player.GetButtonDown(35))
			{
				activeTabIndex--;
				SetActiveTab(activeTabIndex);
			}
			else if (player.GetButtonDown(34))
			{
				activeTabIndex++;
				SetActiveTab(activeTabIndex);
			}
		}
	}

	public void Open()
	{
		if ((bool)LobbyBehaviour.Instance || DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen || DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening)
		{
			return;
		}
		if (Minigame.Instance != null)
		{
			Minigame.Instance.Close();
		}
		if ((bool)MapBehaviour.Instance)
		{
			MapBehaviour.Instance.Close();
		}
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			ConsoleJoystick.SetMode_MenuAdditive();
		}
		ControllerManager.Instance.OpenOverlayMenu("MatchInfoGuide", closeButton);
		bool flag = ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick;
		glyphL.enabled = flag;
		glyphR.enabled = flag;
		if (GameManager.Instance is NormalGameManager)
		{
			if (NormalModeSettings.Count == 0)
			{
				numOfTabs = 3;
				TabButtons[0].SelectButton(isSelected: true);
				CreateNormalModeSettings();
			}
		}
		else if (HnSModeSettings.Count == 0)
		{
			numOfTabs = 2;
			TabButtons[0].SelectButton(isSelected: true);
			CreateHnSModeSettings();
		}
		PlayerControl.LocalPlayer.NetTransform.Halt();
		MatchInfoParent.SetActive(value: true);
		ControllerManager instance = ControllerManager.Instance;
		ControllerUiElementsState currentUiState = ControllerManager.Instance.CurrentUiState;
		List<UiElement> controllerSelectable = ControllerSelectable;
		instance.SetUpSelectables(currentUiState, controllerSelectable[controllerSelectable.Count - 1], ControllerSelectable);
		ControllerManager instance2 = ControllerManager.Instance;
		List<UiElement> controllerSelectable2 = ControllerSelectable;
		instance2.SetCurrentSelected(controllerSelectable2[controllerSelectable2.Count - 1]);
		SetActiveTab(0);
	}

	public void Close()
	{
		MatchInfoParent.SetActive(value: false);
		ControllerManager.Instance.CloseOverlayMenu("MatchInfoGuide");
	}

	public void ToggleActive()
	{
		if (IsActive)
		{
			transitionOpen.Close();
		}
		else
		{
			Open();
		}
	}

	public void SetActiveTab(int tabIndex)
	{
		int index = Mathf.Clamp(tabIndex, 0, numOfTabs - 1);
		foreach (GameObject settingsTab in settingsTabs)
		{
			settingsTab.SetActive(value: false);
		}
		foreach (MatchInfoGuideTabButton tabButton in TabButtons)
		{
			tabButton.SelectButton(isSelected: false);
		}
		settingsTabs[index].SetActive(value: true);
		TabButtons[index].SelectButton(isSelected: true);
		activeTabIndex = index;
	}

	private void CreateNormalModeSettings()
	{
		CreateSettingsEntry(StringNames.GameNumImpostors, GameData.Instance.AllPlayers.Count((NetworkedPlayerInfo p) => p.Role.IsImpostor).ToString());
		CreateSettingsEntry(StringNames.GameKillCooldown, GameManager.Instance.AllGameSettingData[StringNames.GameKillCooldown].GetValueString(GameManager.Instance.LogicOptions.GetKillCooldown()));
		CreateSettingsEntry(StringNames.GameEmergencyCooldown, GameManager.Instance.AllGameSettingData[StringNames.GameEmergencyCooldown].GetValueString(GameManager.Instance.LogicOptions.GetEmergencyCooldown()));
		CreateSettingsEntry(StringNames.GameVisualTasks, GetBoolString(GameManager.Instance.LogicOptions.GetVisualTasks()));
		CreateSettingsEntry(StringNames.GameAnonymousVotes, GetBoolString(GameManager.Instance.LogicOptions.GetAnonymousVotes()));
		CreateSettingsEntry(StringNames.GameConfirmImpostor, GetBoolString(GameManager.Instance.LogicOptions.GetConfirmImpostor()));
		CreateSettingsEntry(StringNames.GameTaskBarMode, GameManager.Instance.LogicOptions.GetTaskBarMode().ToString());
		int num = 0;
		foreach (RoleBehaviour allRole in DestroyableSingleton<RoleManager>.Instance.AllRoles)
		{
			if (allRole.Role != RoleTypes.Crewmate && allRole.Role != RoleTypes.Impostor && allRole.Role != RoleTypes.CrewmateGhost && allRole.Role != RoleTypes.ImpostorGhost && GameOptionsManager.Instance.CurrentGameOptions.RoleOptions.GetChancePerGame(allRole.Role) > 0)
			{
				CreateRoleEntry(allRole);
				num++;
			}
		}
		if (num == 0)
		{
			rolesEnabledMessage.SetActive(value: true);
		}
		MatchInfoRoleScroller.SetYBoundsMax(Mathf.Clamp(Mathf.Ceil((float)num / 2f) + RoleEntryBoundsModifier, 0f, 999f));
		MatchInfoRoleMaskArea.material.SetInt(PlayerMaterial.MaskLayer, 50);
		matchInfoSettingsMaskArea.material.SetInt(PlayerMaterial.MaskLayer, 50);
		CreatePlayerEntries();
	}

	private void CreateHnSModeSettings()
	{
		CreatePlayerEntries();
		CreateHnSSettingsEntry(StringNames.MaxTimeInVent, GameManager.Instance.AllGameSettingData[StringNames.MaxTimeInVent].GetValueString(GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.CrewmateTimeInVent)));
		CreateHnSSettingsEntry(StringNames.FinalEscapeTime, GameManager.Instance.AllGameSettingData[StringNames.FinalEscapeTime].GetValueString(GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.FinalEscapeTime)));
		CreateHnSSettingsEntry(StringNames.SeekerFinalSpeed, GameManager.Instance.AllGameSettingData[StringNames.SeekerFinalSpeed].GetValueString(GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.SeekerFinalSpeed)));
		CreateHnSSettingsEntry(StringNames.SeekerFinalMap, GetBoolString(GameOptionsManager.Instance.CurrentGameOptions.GetBool(BoolOptionNames.SeekerFinalMap)));
		CreateHnSSettingsEntry(StringNames.SeekerPings, GetBoolString(GameOptionsManager.Instance.CurrentGameOptions.GetBool(BoolOptionNames.SeekerPings)));
		CreateHnSSettingsEntry(StringNames.MaxPingTime, GameManager.Instance.AllGameSettingData[StringNames.MaxPingTime].GetValueString(GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.MaxPingTime)));
		matchInfoSettingsMaskArea.material.SetInt(PlayerMaterial.MaskLayer, 50);
		TabButtons[2].gameObject.SetActive(value: false);
	}

	private void CreateSettingsEntry(StringNames settingName, string value)
	{
		GameObject gameObject = Object.Instantiate(MatchInfoSettingPrefab, settingsScrollArea);
		MatchInfoGuideSettingLabel component = gameObject.GetComponent<MatchInfoGuideSettingLabel>();
		if (component != null)
		{
			component.SetInfo(DestroyableSingleton<TranslationController>.Instance.GetString(settingName), value);
		}
		NormalModeSettings.Add(gameObject);
	}

	private void CreateHnSSettingsEntry(StringNames settingName, string value)
	{
		GameObject gameObject = Object.Instantiate(MatchInfoSettingPrefab, settingsScrollArea);
		MatchInfoGuideSettingLabel component = gameObject.GetComponent<MatchInfoGuideSettingLabel>();
		if (component != null)
		{
			component.SetInfo(DestroyableSingleton<TranslationController>.Instance.GetString(settingName), value);
		}
		HnSModeSettings.Add(gameObject);
	}

	private void CreateRoleEntry(RoleBehaviour roleBehaviour)
	{
		Object.Instantiate(MatchInfoRolePanelPrefab, settingsTabs[2].GetComponent<Scroller>().Inner).SetPanel(roleBehaviour, GameOptionsManager.Instance.CurrentGameOptions.RoleOptions.GetNumPerGame(roleBehaviour.Role), GameOptionsManager.Instance.CurrentGameOptions.RoleOptions.GetChancePerGame(roleBehaviour.Role));
	}

	private void CreatePlayerEntries()
	{
		PlayerPool.ReclaimAll();
		int num = 51;
		foreach (NetworkedPlayerInfo allPlayer in GameData.Instance.AllPlayers)
		{
			PlayerIdentifierButton component = PlayerPool.Get<PoolableBehavior>().GetComponent<PlayerIdentifierButton>();
			component.transform.localPosition = new Vector3(0f, 0f, -1f);
			component.Populate(allPlayer);
			ControllerSelectable.Add(component.Button);
			component.SetTextStencil(num++);
		}
	}

	private string GetBoolString(bool _bool)
	{
		if (!_bool)
		{
			return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOff);
		}
		return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOn);
	}
}
