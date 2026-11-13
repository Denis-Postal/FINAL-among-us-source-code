using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.GameOptions;
using InnerNet;
using TMPro;
using UnityEngine;

public class GameStartManager : DestroyableSingleton<GameStartManager>, IDisconnectHandler
{
	private enum StartingStates
	{
		NotStarting,
		Countdown,
		Starting
	}

	[SerializeField]
	private List<MapIconByName> AllMapIcons;

	private IGameOptions cachedData;

	private const float CountdownDuration = 5.0001f;

	public int MinPlayers = 4;

	public TextMeshPro PlayerCounter;

	private int LastPlayerCount = -1;

	public GameObject GameSizePopup;

	public GameObject GameRoomButton;

	public GameObject RoomCodeHeader;

	public GameObject LocalLabel;

	public TextMeshPro GameRoomNameCode;

	public SpriteRenderer MapImage;

	public LobbyBehaviour LobbyPrefab;

	public GameObject GameStartTextParent;

	public TextMeshPro GameStartText;

	public SpriteRenderer StartButtonClient;

	public TextMeshPro GameStartTextClient;

	public ActionMapGlyphDisplay StartButtonGlyph;

	public GameObject StartButtonGlyphContainer;

	public TextMeshPro RulesPresetText;

	public GameObject HostInfoPanelButtons;

	public GameObject ClientInfoPanelButtons;

	public TextMeshPro privatePublicPanelText;

	public SpriteRenderer ShareOnDiscordButton;

	public GameObject InviteFriendsButton;

	private StartingStates startState;

	private float countDownTimer;

	[SerializeField]
	private PassiveButton StartButton;

	[SerializeField]
	private GameObject RulesViewPanel;

	[SerializeField]
	private GameObject RulesEditPanel;

	[SerializeField]
	private HostInfoPanel HostInfoPanel;

	[SerializeField]
	private PassiveButton EditButton;

	[SerializeField]
	private Vector3 GameOptionsPosition;

	[SerializeField]
	private GameObject PlayerOptionsMenu;

	[SerializeField]
	private GameObject HostPrivacyButtons;

	[SerializeField]
	private PassiveButton HostPublicButton;

	[SerializeField]
	private PassiveButton HostPrivateButton;

	[SerializeField]
	private GameObject ClientPrivacyValue;

	[SerializeField]
	private LobbyInfoPane LobbyInfoPane;

	[SerializeField]
	private AudioClip lobbyViewPanelOpenSound;

	[SerializeField]
	private AudioClip lobbyViewPanelCloseSound;

	[SerializeField]
	private AudioClip gameStartSound;

	[SerializeField]
	private PassiveButton HostViewButton;

	[SerializeField]
	private PassiveButton ClientViewButton;

	public bool IsPersistent => false;

	public void Start()
	{
		if (DestroyableSingleton<TutorialManager>.InstanceExists)
		{
			Object.Destroy(gameObject);
			return;
		}
		LobbyInfoPane.gameObject.SetActive(value: true);
		DataManager.Settings.Gameplay.OnStreamerModeChanged += UpdateStreamerModeUI;
		UpdateStreamerModeUI();
		if (GameCode.IntToGameName(AmongUsClient.Instance.GameId) == null)
		{
			RoomCodeHeader.gameObject.SetActive(value: false);
			GameRoomButton.gameObject.SetActive(value: false);
			LocalLabel.gameObject.SetActive(value: true);
			HostPrivacyButtons.gameObject.SetActive(value: false);
			ClientPrivacyValue.gameObject.SetActive(value: true);
		}
		AmongUsClient.Instance.DisconnectHandlers.AddUnique(this);
		DoHostSetup();
		if (AmongUsClient.Instance.AmHost)
		{
			LobbyBehaviour.Instance = Object.Instantiate(LobbyPrefab);
			AmongUsClient.Instance.Spawn(LobbyBehaviour.Instance);
		}
		ShareOnDiscordButton.gameObject.SetActive(value: false);
	}

	private void DoHostSetup()
	{
		string text = GameCode.IntToGameName(AmongUsClient.Instance.GameId);
		if (!AmongUsClient.Instance.AmHost)
		{
			HostPrivacyButtons.gameObject.SetActive(value: false);
			ClientPrivacyValue.gameObject.SetActive(value: true);
			StartButton.gameObject.SetActive(value: false);
			StartButtonClient.gameObject.SetActive(value: true);
			GameStartTextParent.SetActive(value: false);
			HostInfoPanelButtons.gameObject.SetActive(value: false);
			ClientInfoPanelButtons.gameObject.SetActive(value: true);
			return;
		}
		if (text != null)
		{
			HostPrivacyButtons.gameObject.SetActive(value: true);
			ClientPrivacyValue.gameObject.SetActive(value: false);
		}
		HostInfoPanelButtons.gameObject.SetActive(value: true);
		ClientInfoPanelButtons.gameObject.SetActive(value: false);
		StartButton.gameObject.SetActive(value: true);
		StartButtonClient.gameObject.SetActive(value: false);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		DataManager.Settings.Gameplay.OnStreamerModeChanged -= UpdateStreamerModeUI;
	}

	public void ClickEdit()
	{
		if (RulesEditPanel != null)
		{
			return;
		}
		PlayerControl localPlayer = PlayerControl.LocalPlayer;
		if (AmongUsClient.Instance.AmHost)
		{
			localPlayer.NetTransform.Halt();
			RulesEditPanel = Object.Instantiate(PlayerOptionsMenu);
			RulesEditPanel.transform.SetParent(Camera.main.transform, worldPositionStays: false);
			RulesEditPanel.transform.localPosition = GameOptionsPosition;
			DestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(null, RulesEditPanel.gameObject, null);
			RulesViewPanel.SetActive(value: false);
			if ((bool)PlayerCustomizationMenu.Instance)
			{
				PlayerCustomizationMenu.Instance.Close(canMove: false);
			}
			SelectViewButton(selected: false);
			LobbyInfoPane.DeactivatePane();
		}
	}

	private void UpdateStreamerModeUI()
	{
		string text = GameCode.IntToGameName(AmongUsClient.Instance.GameId);
		if (text == null)
		{
			text = string.Empty;
		}
		GameRoomNameCode.text = ((DataManager.Settings.Gameplay.StreamerMode && text != string.Empty) ? "******" : text);
	}

	public void MakePublic()
	{
		if (!AmongUsClient.Instance.AmHost)
		{
			return;
		}
		AmongUsClient.Instance.ChangeGamePublic(!AmongUsClient.Instance.IsGamePublic);
		HostPrivateButton.gameObject.SetActive(!AmongUsClient.Instance.IsGamePublic);
		HostPublicButton.gameObject.SetActive(AmongUsClient.Instance.IsGamePublic);
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			if (AmongUsClient.Instance.IsGamePublic)
			{
				ControllerManager.Instance.SetCurrentSelected(HostPublicButton);
			}
			else
			{
				ControllerManager.Instance.SetCurrentSelected(HostPrivateButton);
			}
		}
	}

	public void ShareGameInvite()
	{
		if (DestroyableSingleton<DiscordManager>.InstanceExists)
		{
			DestroyableSingleton<DiscordManager>.Instance.ShareGameOnDiscord();
		}
	}

	private int GetRoleRateChange(IGameOptions cachedOpts, out int roleStringName, out int chance, out int count, out RoleTeamTypes team)
	{
		roleStringName = 0;
		chance = 0;
		count = 0;
		team = RoleTeamTypes.Crewmate;
		foreach (RoleBehaviour allRole in DestroyableSingleton<RoleManager>.Instance.AllRoles)
		{
			if (allRole.Role != RoleTypes.Crewmate && allRole.Role != RoleTypes.Impostor && allRole.Role != RoleTypes.CrewmateGhost && allRole.Role != RoleTypes.ImpostorGhost)
			{
				int chancePerGame = cachedOpts.RoleOptions.GetChancePerGame(allRole.Role);
				int chancePerGame2 = GameOptionsManager.Instance.CurrentGameOptions.RoleOptions.GetChancePerGame(allRole.Role);
				int numPerGame = cachedOpts.RoleOptions.GetNumPerGame(allRole.Role);
				int numPerGame2 = GameOptionsManager.Instance.CurrentGameOptions.RoleOptions.GetNumPerGame(allRole.Role);
				if (chancePerGame != chancePerGame2 || numPerGame != numPerGame2)
				{
					roleStringName = (int)allRole.StringName;
					chance = chancePerGame2;
					count = numPerGame2;
					team = allRole.TeamType;
					return (int)allRole.Role;
				}
			}
		}
		return -1;
	}

	private void CheckSettingsDiffs()
	{
		if (cachedData == GameOptionsManager.Instance.CurrentGameOptions)
		{
			return;
		}
		if (cachedData != null)
		{
			if (cachedData.RulesPreset != GameOptionsManager.Instance.CurrentGameOptions.RulesPreset && GameOptionsManager.Instance.CurrentGameOptions.RulesPreset != RulesPresets.Custom)
			{
				DestroyableSingleton<HudManager>.Instance.Notifier.AddSettingsChangeMessage(StringNames.ModeLabel, DestroyableSingleton<TranslationController>.Instance.GetString(GameOptionsManager.Instance.CurrentGameOptions.GetRulesPresetTitle()));
			}
			else
			{
				if (GetRoleRateChange(cachedData, out var roleStringName, out var chance, out var count, out var team) != -1)
				{
					DestroyableSingleton<HudManager>.Instance.Notifier.AddRoleSettingsChangeMessage((StringNames)roleStringName, count, chance, team);
				}
				foreach (KeyValuePair<StringNames, BaseGameSetting> allGameSettingDatum in GameManager.Instance.AllGameSettingData)
				{
					float value = GameOptionsManager.Instance.CurrentGameOptions.GetValue(allGameSettingDatum.Value);
					if (value != cachedData.GetValue(allGameSettingDatum.Value))
					{
						DestroyableSingleton<HudManager>.Instance.Notifier.AddSettingsChangeMessage(allGameSettingDatum.Key, allGameSettingDatum.Value.GetValueString(value));
						break;
					}
				}
			}
		}
		else
		{
			foreach (BaseGameSetting overviewSetting in GameManager.Instance.GameSettingsList.OverviewSettings)
			{
				float value2 = GameOptionsManager.Instance.CurrentGameOptions.GetValue(overviewSetting);
				DestroyableSingleton<HudManager>.Instance.Notifier.AddSettingsChangeMessage(overviewSetting.Title, overviewSetting.GetValueString(value2), playSound: false);
			}
		}
		cachedData = GameOptionsManager.Instance.CurrentGameOptions;
		LobbyInfoPane.RefreshPane();
	}

	public void Update()
	{
		if (!GameData.Instance || !GameManager.Instance)
		{
			return;
		}
		UpdateMapImage((MapNames)GameManager.Instance.LogicOptions.MapId);
		CheckSettingsDiffs();
		RulesPresetText.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameOptionsManager.Instance.CurrentGameOptions.GetRulesPresetTitle());
		if (GameCode.IntToGameName(AmongUsClient.Instance.GameId) == null)
		{
			privatePublicPanelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LocalButton);
		}
		else if (AmongUsClient.Instance.IsGamePublic)
		{
			privatePublicPanelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PublicHeader);
		}
		else
		{
			privatePublicPanelText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PrivateHeader);
		}
		HostPrivateButton.gameObject.SetActive(!AmongUsClient.Instance.IsGamePublic);
		HostPublicButton.gameObject.SetActive(AmongUsClient.Instance.IsGamePublic);
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.C))
		{
			ClipboardHelper.PutClipboardString(GameCode.IntToGameName(AmongUsClient.Instance.GameId));
		}
		if (DestroyableSingleton<DiscordManager>.InstanceExists)
		{
			bool active = AmongUsClient.Instance.AmHost && AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame && DestroyableSingleton<DiscordManager>.Instance.CanShareGameOnDiscord() && DestroyableSingleton<DiscordManager>.Instance.HasValidPartyID();
			ShareOnDiscordButton.gameObject.SetActive(active);
		}
		if (GameData.Instance.PlayerCount != LastPlayerCount)
		{
			LastPlayerCount = GameData.Instance.PlayerCount;
			string arg = "<color=#FF0000FF>";
			if (LastPlayerCount > MinPlayers)
			{
				arg = "<color=#00FF00FF>";
			}
			if (LastPlayerCount == MinPlayers)
			{
				arg = "<color=#FFFF00FF>";
			}
			if (AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame)
			{
				PlayerCounter.text = $"{arg}{LastPlayerCount}/{15}";
			}
			else
			{
				PlayerCounter.text = $"{arg}{LastPlayerCount}/{GameManager.Instance.LogicOptions.MaxPlayers}";
			}
			StartButton.SetButtonEnableState(LastPlayerCount >= MinPlayers);
			StartButtonGlyph?.SetColor((LastPlayerCount >= MinPlayers) ? Palette.EnabledColor : Palette.DisabledClear);
			if (LastPlayerCount >= MinPlayers)
			{
				StartButton.ChangeButtonText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.StartLabel));
				GameStartTextClient.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WaitingForHost);
			}
			else
			{
				StartButton.ChangeButtonText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WaitingForPlayers));
				GameStartTextClient.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WaitingForPlayers);
			}
			if (DestroyableSingleton<DiscordManager>.InstanceExists)
			{
				if (AmongUsClient.Instance.AmHost && AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame)
				{
					DestroyableSingleton<DiscordManager>.Instance.SetInLobbyHost(LastPlayerCount, GameManager.Instance.LogicOptions.MaxPlayers, AmongUsClient.Instance.GameId);
				}
				else
				{
					DestroyableSingleton<DiscordManager>.Instance.SetInLobbyClient(LastPlayerCount, GameManager.Instance.LogicOptions.MaxPlayers, AmongUsClient.Instance.GameId);
				}
			}
		}
		if (AmongUsClient.Instance.AmHost)
		{
			if (startState == StartingStates.Countdown)
			{
				int num = Mathf.CeilToInt(countDownTimer);
				countDownTimer -= Time.deltaTime;
				int num2 = Mathf.CeilToInt(countDownTimer);
				if (!GameStartTextParent.activeSelf)
				{
					SoundManager.Instance.PlaySound(gameStartSound, loop: false);
				}
				GameStartTextParent.SetActive(value: true);
				GameStartText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameStarting, num2);
				if (num != num2)
				{
					PlayerControl.LocalPlayer.RpcSetStartCounter(num2);
				}
				if (num2 <= 0)
				{
					FinallyBegin();
				}
			}
			else
			{
				GameStartTextParent.SetActive(value: false);
				GameStartText.text = string.Empty;
			}
		}
		if (LobbyInfoPane.gameObject.activeSelf && DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening)
		{
			LobbyInfoPane.DeactivatePane();
		}
		LobbyInfoPane.gameObject.SetActive(!DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening);
	}

	public void ResetStartState()
	{
		startState = StartingStates.NotStarting;
		if ((bool)StartButton && (bool)StartButton.gameObject)
		{
			StartButton.gameObject.SetActive(AmongUsClient.Instance.AmHost);
			StartButtonClient.gameObject.SetActive(!AmongUsClient.Instance.AmHost);
		}
		if ((bool)PlayerControl.LocalPlayer)
		{
			PlayerControl.LocalPlayer.RpcSetStartCounter(-1);
		}
	}

	public void SetStartCounter(sbyte sec)
	{
		if (sec == -1)
		{
			GameStartTextParent.SetActive(value: false);
			GameStartText.text = string.Empty;
			if (!AmongUsClient.Instance.AmHost)
			{
				StartButtonClient.gameObject.SetActive(value: true);
			}
			return;
		}
		if (!GameStartTextParent.activeSelf)
		{
			SoundManager.Instance.PlaySound(gameStartSound, loop: false);
		}
		StartButtonClient.gameObject.SetActive(value: false);
		GameStartTextParent.SetActive(value: true);
		GameStartText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameStarting, sec);
	}

	public void BeginGame()
	{
		if (startState == StartingStates.NotStarting)
		{
			if ((DataManager.Player.Onboarding.AlwaysShowMinPlayerWarning || !DataManager.Player.Onboarding.ViewedMinPlayerWarning) && GameData.Instance.PlayerCount == MinPlayers)
			{
				GameSizePopup.SetActive(value: true);
			}
			else if (GameData.Instance.PlayerCount < MinPlayers)
			{
				StartCoroutine(Effects.SwayX(PlayerCounter.transform));
			}
			else
			{
				ReallyBegin(neverShow: false);
			}
		}
	}

	public void ReallyBegin(bool neverShow)
	{
		startState = StartingStates.Countdown;
		GameSizePopup.SetActive(value: false);
		if (neverShow)
		{
			DataManager.Player.Onboarding.AlwaysShowMinPlayerWarning = false;
		}
		DataManager.Player.Onboarding.ViewedMinPlayerWarning = true;
		DataManager.Player.Save();
		StartButton.gameObject.SetActive(value: false);
		StartButtonClient.gameObject.SetActive(value: false);
		GameStartTextParent.SetActive(value: false);
		countDownTimer = 5.0001f;
		startState = StartingStates.Countdown;
		AmongUsClient.Instance.KickNotJoinedPlayers();
	}

	public void FinallyBegin()
	{
		if (startState == StartingStates.Countdown)
		{
			startState = StartingStates.Starting;
			if (DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen)
			{
				DestroyableSingleton<HudManager>.Instance.GameMenu.Close();
			}
			AmongUsClient.Instance.StartGame();
			AmongUsClient.Instance.DisconnectHandlers.Remove(this);
			Object.Destroy(gameObject);
		}
	}

	public void HandleDisconnect(PlayerControl pc, DisconnectReasons reason)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			LastPlayerCount = -1;
			if ((bool)StartButton)
			{
				StartButton.gameObject.SetActive(value: true);
				StartButton.ChangeButtonText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WaitingForPlayers));
				GameStartTextClient.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WaitingForPlayers);
			}
			DoHostSetup();
		}
		DestroyableSingleton<GameStartManager>.Instance.UpdateHostPanelImage(GameData.Instance.GetHost());
	}

	public void HandleDisconnect()
	{
		HandleDisconnect(null, DisconnectReasons.ExitGame);
	}

	public void ShowInviteMenu()
	{
	}

	public void ToggleButtonGlyphs(bool enabled)
	{
		StartButtonGlyphContainer.SetActive(enabled);
	}

	public void UpdateMapImage(MapNames map)
	{
		MapImage.sprite = AllMapIcons.Find((MapIconByName m) => m.Name == map).MapIcon;
		if (map == MapNames.Skeld && AprilFoolsMode.ShouldFlipSkeld())
		{
			MapImage.flipX = true;
		}
		else
		{
			MapImage.flipX = false;
		}
	}

	public void ToggleViewPane()
	{
		PlayerControl.LocalPlayer.NetTransform.Halt();
		RulesViewPanel.SetActive(!RulesViewPanel.activeSelf);
		if (RulesViewPanel.activeSelf)
		{
			SelectViewButton(selected: true);
			ClientViewButton.ChangeButtonText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ViewingLabel));
			if ((bool)PlayerCustomizationMenu.Instance)
			{
				PlayerCustomizationMenu.Instance.Close(canMove: false);
			}
			SoundManager.Instance.PlaySound(lobbyViewPanelOpenSound, loop: false);
		}
		else
		{
			SelectViewButton(selected: false);
			ClientViewButton.ChangeButtonText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ViewLabel));
			SoundManager.Instance.PlaySoundImmediate(lobbyViewPanelOpenSound, loop: false);
		}
	}

	public void SelectViewButton(bool selected)
	{
		HostViewButton.SelectButton(selected);
		ClientViewButton.SelectButton(selected);
	}

	public void UpdateHostPanelImage(NetworkedPlayerInfo player)
	{
		StartCoroutine(HostInfoPanel.SetCosmetics(player));
	}

	public void CloseGameOptionsMenus()
	{
		if (RulesViewPanel.activeSelf)
		{
			ToggleViewPane();
		}
		if (RulesEditPanel != null)
		{
			RulesEditPanel.GetComponent<GameSettingMenu>().Close();
		}
	}
}
