using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.GameOptions;
using Assets.CoreScripts;
using ExitGames.Client.Photon;
using InnerNet;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class AmongUsClient : InnerNetClient
{
	public enum MainMenuTarget
	{
		None,
		OnlineMenu,
		EnterCodeMenu,
		FindGameMenu
	}

	private const int MAX_CLIENT_WAIT_SECONDS = 10;

	private const float SPAWN_TESTER_WAIT_SECONDS = 0.11f;

	public static AmongUsClient Instance;

	public string OnlineScene;

	public string MainMenuScene;

	public GameData GameDataPrefab;

	public VoteBanSystem VoteBanPrefab;

	public PlayerControl PlayerPrefab;

	public List<AssetReference> ShipPrefabs;

	public int TutorialMapId;

	public float SpawnRadius = 1.75f;

	public DiscoveryState discoverState;

	public List<IDisconnectHandler> DisconnectHandlers = new List<IDisconnectHandler>();

	public List<IGameListHandler> GameListHandlers = new List<IGameListHandler>();

	public CrossplayPrivilegeErrorType CrossplayPrivilegeError;

	public MainMenuTarget MenuTarget;

	private readonly Logger logger = new Logger(Logger.Category.Gameplay, "AmongUsClient");

	private AsyncOperationHandle<GameObject> ShipLoadingAsyncHandle;

	public void Awake()
	{
		if ((bool)Instance)
		{
			if (Instance != this)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		else
		{
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Application.targetFrameRate = 60;
			DebugToolInstantiator.EnsureOverlay();
			PhotonOnlineTransport.Instance.PlayerJoined += OnPhotonPlayerJoined;
			PhotonOnlineTransport.Instance.PlayerLeft += OnPhotonPlayerLeft;
			PhotonOnlineTransport.Instance.MasterClientChanged += OnPhotonMasterClientChanged;
			PhotonOnlineTransport.Instance.PacketReceived += OnPhotonPacketReceived;
		}
	}

	public void StartGame()
	{
		SendStartGame();
		discoverState = DiscoveryState.Off;
	}

	public override void Update()
	{
		base.Update();
	}

	public void ExitGame(DisconnectReasons reason)
	{
		DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: false);
		if (DestroyableSingleton<WaitForHostPopup>.InstanceExists)
		{
			DestroyableSingleton<WaitForHostPopup>.Instance.Hide();
		}
		StopAllCoroutines();
		AbortLoadingAssets();
		DestroyableSingleton<FriendsListManager>.Instance.ReparentUI();
		SoundManager.Instance.StopAllSound();
		discoverState = DiscoveryState.Off;
		DestroyPlayerInfoObjects();
		GameData.DestroyInstance();
		DisconnectHandlers.Clear();
		DisconnectInternal(reason);
		DestroyableSingleton<AchievementManager>.Instance.OnMatchExited();
		DataManager.Player.Stats.SaveStats();
		GameDebugCommands.RemoveCommands();
		Logger.GlobalInstance.Debug("Loading new scene " + MainMenuScene);
		if (MainMenuScene.Equals("MainMenu") && !OnlineScene.Equals("Tutorial"))
		{
			MenuTarget = MainMenuTarget.OnlineMenu;
		}
		SceneManager.LoadScene(MainMenuScene);
		Logger.GlobalInstance.Debug("Finished loading scene");
	}

	protected override void OnGetGameList(TotalGameData totalGames, HttpMatchmakerManager.FindGamesListFilteredResponse gamesResponse)
	{
		for (int i = 0; i < GameListHandlers.Count; i++)
		{
			try
			{
				GameListHandlers[i].HandleList(totalGames, gamesResponse);
			}
			catch (Exception exception)
			{
				logger.Error("AmongUsClient::OnGetGameList: Exception:");
				Debug.LogException(exception, this);
			}
		}
	}

	protected override void OnReportedPlayer(ReportOutcome outcome, int clientId, string playerName, ReportReasons reason)
	{
	}

	protected override void OnGameCreated(string gameIdString)
	{
	}

	protected override void OnWaitForHost(string gameIdString)
	{
		if (GameState != GameStates.Joined)
		{
			logger.Info("Waiting for host: " + gameIdString);
			if (DestroyableSingleton<WaitForHostPopup>.InstanceExists)
			{
				DestroyableSingleton<WaitForHostPopup>.Instance.Show();
			}
		}
	}

	protected void AbortLoadingAssets()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (ShipLoadingAsyncHandle.IsValid())
		{
			Addressables.Release<GameObject>(ShipLoadingAsyncHandle);
			ShipLoadingAsyncHandle = default;
		}
	}

	protected IEnumerator CoStartGameHost()
	{
		if ((bool)LobbyBehaviour.Instance)
		{
			LobbyBehaviour.Instance.Despawn();
		}
		if (!ShipStatus.Instance)
		{
			int num = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.MapId, 0, Constants.MapNames.Length - 1);
			try
			{
				if (num == 0 && AprilFoolsMode.ShouldFlipSkeld())
				{
					num = 3;
				}
				else if (num == 3 && !AprilFoolsMode.ShouldFlipSkeld())
				{
					num = 0;
				}
			}
			catch (Exception exception)
			{
				logger.Error("AmongUsClient::CoStartGame: Exception:");
				Debug.LogException(exception, this);
			}
			ShipLoadingAsyncHandle = ShipPrefabs[num].InstantiateAsync((Transform)null, false);
			yield return ShipLoadingAsyncHandle;
			GameObject result = ShipLoadingAsyncHandle.Result;
			ShipLoadingAsyncHandle = default;
			ShipStatus.Instance = result.GetComponent<ShipStatus>();
			Spawn(ShipStatus.Instance);
		}
		DateTime start = DateTime.Now;
		while (true)
		{
			bool flag = true;
			int num2 = 10;
			float num3 = (float)(DateTime.Now - start).TotalSeconds;
			if (GameOptionsManager.Instance.CurrentGameOptions.MapId == 5 || GameOptionsManager.Instance.CurrentGameOptions.MapId == 4)
			{
				num2 = 15;
			}
			lock (allClients)
			{
				for (int i = 0; i < allClients.Count; i++)
				{
					ClientData clientData = allClients[i];
					if (clientData.Id != ClientId && !clientData.IsReady)
					{
						if (num3 < (float)num2)
						{
							flag = false;
							continue;
						}
						SendLateRejection(clientData.Id, DisconnectReasons.ClientTimeout);
						clientData.IsReady = true;
						OnPlayerLeft(clientData, DisconnectReasons.ClientTimeout);
					}
				}
			}
			if (num3 > 1f && num3 < (float)num2)
			{
				DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: true);
				DestroyableSingleton<LoadingBarManager>.Instance.SetLoadingPercent(num3 / (float)num2 * 100f, StringNames.LoadingBarGameStartWaitingPlayers);
			}
			if (flag)
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
		DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: false);
		DestroyableSingleton<RoleManager>.Instance.SelectRoles();
		ShipStatus.Instance.Begin();
		SendClientReady();
	}

	public void KickNotJoinedPlayers()
	{
		if (!AmHost)
		{
			return;
		}
		lock (allClients)
		{
			for (int i = 0; i < allClients.Count; i++)
			{
				ClientData clientData = allClients[i];
				if (clientData.Character == null)
				{
					SendLateRejection(clientData.Id, DisconnectReasons.Error);
				}
			}
		}
	}

	protected IEnumerator CoStartGameClient()
	{
		while (PlayerControl.LocalPlayer.Data == null && !AmHost)
		{
			yield return null;
		}
		float timer = 0f;
		DateTime start = DateTime.Now;
		DateTime persistentStart = DateTime.Now;
		float timeOut = 15f;
		if (GameOptionsManager.Instance.CurrentGameOptions.MapId == 5 || GameOptionsManager.Instance.CurrentGameOptions.MapId == 4)
		{
			timeOut = 20f;
		}
		while (!ShipStatus.Instance || (bool)LobbyBehaviour.Instance)
		{
			if (AmHost)
			{
				yield break;
			}
			if (timer > timeOut)
			{
				StartCoroutine(CoWaitForDisconnect());
				yield break;
			}
			timer = (float)(DateTime.Now - start).TotalSeconds;
			if (timer > 1f && timer < timeOut)
			{
				DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: true);
				DestroyableSingleton<LoadingBarManager>.Instance.SetLoadingPercent(timer / timeOut * 100f, StringNames.LoadingBarGameStart);
			}
			yield return null;
		}
		SendClientReady();
		start = DateTime.Now;
		timer = 0f;
		while (true)
		{
			bool stopWaiting = true;
			lock (allClients)
			{
				for (int i = 0; i < allClients.Count; i++)
				{
					if (!allClients[i].IsReady)
					{
						stopWaiting = false;
					}
				}
			}
			yield return null;
			if (stopWaiting)
			{
				break;
			}
			if (timer > timeOut)
			{
				StartCoroutine(CoWaitForDisconnect());
				yield break;
			}
			timer = (float)(DateTime.Now - start).TotalSeconds;
			if (timer > 1f && timer < timeOut)
			{
				DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: true);
				DestroyableSingleton<LoadingBarManager>.Instance.SetLoadingPercent((float)(DateTime.Now - persistentStart).TotalSeconds / timeOut * 100f, StringNames.LoadingBarGameStartWaitingPlayers);
			}
		}
		DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: false);
		while ((PlayerControl.LocalPlayer.Data.Role == null || PlayerControl.LocalPlayer.myTasks.Count == 0) && !AmHost)
		{
			yield return null;
		}
	}

	private IEnumerator CoWaitForDisconnect()
	{
		Logger.GlobalInstance.Debug("Reached timeout while loading game client");
		yield return new WaitForEndOfFrame();
		DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: false);
		MainMenuScene = "MainMenu";
		ExitGame(DisconnectReasons.Error);
	}

	protected override IEnumerator CoStartGame()
	{
		if (DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen)
		{
			DestroyableSingleton<HudManager>.Instance.GameMenu.Close();
		}
		DestroyableSingleton<UnityTelemetry>.Instance.Init();
		logger.Info("Received game start: " + AmHost);
		yield return null;
		while (!DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return null;
		}
		while (!PlayerControl.LocalPlayer)
		{
			yield return null;
		}
		PlayerControl.LocalPlayer.moveable = false;
		PlayerControl.LocalPlayer.MyPhysics.inputHandler.enabled = true;
		PlayerCustomizationMenu playerCustomizationMenu = UnityEngine.Object.FindObjectOfType<PlayerCustomizationMenu>();
		if ((bool)playerCustomizationMenu)
		{
			playerCustomizationMenu.Close(canMove: false);
		}
		GameSettingMenu gameSettingMenu = UnityEngine.Object.FindObjectOfType<GameSettingMenu>();
		if ((bool)gameSettingMenu)
		{
			gameSettingMenu.Close();
		}
		if (DestroyableSingleton<GameStartManager>.InstanceExists)
		{
			DisconnectHandlers.Remove(DestroyableSingleton<GameStartManager>.Instance);
			UnityEngine.Object.Destroy(DestroyableSingleton<GameStartManager>.Instance.gameObject);
		}
		if (DestroyableSingleton<LobbyInfoPane>.InstanceExists)
		{
			UnityEngine.Object.Destroy(DestroyableSingleton<LobbyInfoPane>.Instance.gameObject);
		}
		if (DestroyableSingleton<DiscordManager>.InstanceExists)
		{
			DestroyableSingleton<DiscordManager>.Instance.SetPlayingGame();
		}
		if (!string.IsNullOrEmpty(DataManager.Player.Store.ActiveCosmicube))
		{
			Instance.SetActivePodType(DestroyableSingleton<CosmicubeManager>.Instance.GetCubeDataByID(DataManager.Player.Store.ActiveCosmicube).podId);
		}
		else
		{
			PlayerStorageManager.CloudPlayerPrefs playerPrefs = DestroyableSingleton<PlayerStorageManager>.Instance.PlayerPrefs;
			Instance.SetActivePodType(playerPrefs.ActivePodType);
		}
		DestroyableSingleton<FriendsListManager>.Instance.ConfirmationScreen.Cancel();
		DestroyableSingleton<FriendsListManager>.Instance.Ui.Close(force: true);
		DestroyableSingleton<FriendsListManager>.Instance.ReparentUI();
		CosmeticsCache.ClearUnusedCosmetics();
		yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.clear, Color.black);
		if (AmHost)
		{
			yield return CoStartGameHost();
		}
		else
		{
			yield return CoStartGameClient();
			if (AmHost)
			{
				yield return CoStartGameHost();
			}
		}
		for (int i = 0; i < GameData.Instance.PlayerCount; i++)
		{
			PlayerControl playerControl = GameData.Instance.AllPlayers[i].Object;
			if ((bool)playerControl)
			{
				playerControl.moveable = true;
				playerControl.NetTransform.enabled = true;
				playerControl.MyPhysics.enabled = true;
				playerControl.MyPhysics.Awake();
				playerControl.MyPhysics.ResetMoveState();
				playerControl.Collider.enabled = true;
				ShipStatus.Instance.SpawnPlayer(playerControl, GameData.Instance.PlayerCount, initialSpawn: true);
			}
		}
		GameData.TimeGameStarted = Time.realtimeSinceStartup;
		int map = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.MapId, 0, Constants.MapNames.Length - 1);
		string roomCode = GameCode.IntToGameName(Instance.GameId);
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.StartGame(PlayerControl.LocalPlayer.Data, GameData.Instance.PlayerCount, GameOptionsManager.Instance.CurrentGameOptions.NumImpostors, Instance.NetworkMode, (MapNames)map, GameOptionsManager.Instance.CurrentGameOptions.GameMode, roomCode, DestroyableSingleton<ServerManager>.Instance.CurrentRegion.Name, GameOptionsManager.Instance.CurrentGameOptions, GameData.Instance.AllPlayers);
		DataManager.Player.Ban.BanPoints++;
		DataManager.Player.Ban.PreviousGameStartDate = DateTime.UtcNow;
		logger.Info("SETTING BAN POINTS, CURRENT POINTS: " + DataManager.Player.Ban.BanPoints);
		DataManager.Player.Save();
		try
		{
			DestroyableSingleton<UnityTelemetry>.Instance.StartGame(Instance.AmHost, GameData.Instance.PlayerCount, GameOptionsManager.Instance.CurrentGameOptions.NumImpostors, Instance.NetworkMode, DataManager.Player.Stats.GetStat(StatID.GamesAsImpostor), DataManager.Player.Stats.GetStat(StatID.GamesStarted), DataManager.Player.Stats.GetStat(StatID.CrewmateStreak));
			NetworkedPlayerInfo.PlayerOutfit defaultOutfit = PlayerControl.LocalPlayer.Data.DefaultOutfit;
			DestroyableSingleton<UnityTelemetry>.Instance.StartGameCosmetics(defaultOutfit.ColorId, defaultOutfit.HatId, defaultOutfit.SkinId, defaultOutfit.PetId, defaultOutfit.VisorId, defaultOutfit.NamePlateId);
		}
		catch
		{
		}
		GameDebugCommands.AddCommands();
	}

	protected override void OnBecomeHost()
	{
		ClientData clientData = FindClientById(ClientId);
		if (!clientData.Character)
		{
			switch (GameState)
			{
			case GameStates.Joined:
			case GameStates.Started:
				StartCoroutine(CreatePlayer(clientData));
				break;
			case GameStates.Ended:
				OnGameJoined(null);
				break;
			}
		}
		logger.Info("Became Host");
		RemoveUnownedObjects();
		if ((bool)LobbyBehaviour.Instance && (bool)ShipStatus.Instance)
		{
			LobbyBehaviour.Instance.Despawn();
		}
	}

	protected override void OnGameEnd(EndGameResult endGameResult)
	{
		DataManager.Player.Ban.BanPoints -= 1.5f;
		DataManager.Player.Ban.PreviousGameStartDate = DateTime.MinValue;
		GameOverReason gameOverReason = endGameResult.GameOverReason;
		bool showAd = endGameResult.ShowAd;
		DisconnectHandlers.RemoveAll((IDisconnectHandler handler) => !handler.IsPersistent);
		if ((bool)Minigame.Instance)
		{
			try
			{
				Minigame.Instance.Close();
				Minigame.Instance.Close();
			}
			catch (Exception exception)
			{
				logger.Error("AmongUsClient::OnGameEnd Exception: 1");
				Debug.LogException(exception, this);
			}
		}
		float durationInSeconds = Time.realtimeSinceStartup - GameData.TimeGameStarted;
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.EndGame(durationInSeconds, gameOverReason, GameData.Instance.AllPlayers);
		try
		{
			DestroyableSingleton<UnityTelemetry>.Instance.EndGame(gameOverReason);
		}
		catch (Exception exception2)
		{
			logger.Error("AmongUsClient::OnGameEnd Exception: 2");
			Debug.LogException(exception2, this);
		}
		GameData.OnGameEnd();
		EndGameResult.CachedGameOverReason = gameOverReason;
		EndGameResult.CachedShowAd = showAd;
		GameManager.Instance.DidHumansWin(gameOverReason);
		ProgressionManager.XpGrantResult cachedXpGrantResult = endGameResult.XpGrantResult ?? ProgressionManager.XpGrantResult.Default();
		ProgressionManager.CurrencyGrantResult currencyGrantResult = endGameResult.BeansGrantResult ?? ProgressionManager.CurrencyGrantResult.Default();
		ProgressionManager.CurrencyGrantResult currencyGrantResult2 = endGameResult.PodsGrantResult ?? ProgressionManager.CurrencyGrantResult.Default();
		EndGameResult.CachedXpGrantResult = cachedXpGrantResult;
		EndGameResult.CachedBeansGrantResult = currencyGrantResult;
		EndGameResult.CachedPodsGrantResult = currencyGrantResult2;
		if (endGameResult.XpGrantResult != null)
		{
			DataManager.Player.Stats.Xp = endGameResult.XpGrantResult.OldXpAmount + endGameResult.XpGrantResult.GrantedXp;
			if (endGameResult.XpGrantResult.LevelledUp)
			{
				DataManager.Player.Stats.Xp = endGameResult.XpGrantResult.OldXpAmount + endGameResult.XpGrantResult.GrantedXp - endGameResult.XpGrantResult.XpRequiredToLevelUp;
				DataManager.Player.Stats.Level = endGameResult.XpGrantResult.NewLevel;
				DataManager.Player.Stats.XpForNextLevel = endGameResult.XpGrantResult.XpRequiredToLevelUpNextLevel;
			}
			else
			{
				DataManager.Player.Stats.Xp = endGameResult.XpGrantResult.OldXpAmount + endGameResult.XpGrantResult.GrantedXp;
				DataManager.Player.Stats.Level = endGameResult.XpGrantResult.OldLevel;
				DataManager.Player.Stats.XpForNextLevel = endGameResult.XpGrantResult.XpRequiredToLevelUp;
			}
		}
		DataManager.Player.Save();
		DestroyableSingleton<InventoryManager>.Instance.ChangePodCount(currencyGrantResult2.PodId, (int)currencyGrantResult2.GrantedPodsWithMultiplierApplied);
		DestroyableSingleton<InventoryManager>.Instance.UnusedBeans += (int)currencyGrantResult.GrantedPodsWithMultiplierApplied;
		for (int num = 0; num < GameData.Instance.PlayerCount; num++)
		{
			NetworkedPlayerInfo networkedPlayerInfo = GameData.Instance.AllPlayers[num];
			if (!(networkedPlayerInfo == null) && networkedPlayerInfo.Role.DidWin(gameOverReason))
			{
				EndGameResult.CachedWinners.Add(new CachedPlayerData(networkedPlayerInfo));
			}
		}
		EndGameResult.CachedLocalPlayer = new CachedPlayerData(PlayerControl.LocalPlayer.Data);
		GameDebugCommands.RemoveCommands();
		StartCoroutine(CoEndGame());
	}

	public IEnumerator CoEndGame()
	{
		yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.clear, Color.black, 0.5f);
		SceneManager.LoadScene("EndGame");
	}

	protected override void OnPlayerJoined(ClientData data)
	{
		if (DestroyableSingleton<GameStartManager>.InstanceExists)
		{
			DestroyableSingleton<GameStartManager>.Instance.ResetStartState();
		}
		if (AmHost && data.InScene)
		{
			StartCoroutine(CreatePlayer(data));
		}
	}

	protected override void OnGameJoined(string gameIdString)
	{
		if (DestroyableSingleton<WaitForHostPopup>.InstanceExists)
		{
			DestroyableSingleton<WaitForHostPopup>.Instance.Hide();
		}
		if (!string.IsNullOrWhiteSpace(OnlineScene))
		{
			SceneManager.LoadScene(OnlineScene);
		}
		if (NetworkMode != NetworkModes.FreePlay)
		{
			DestroyableSingleton<FriendsListManager>.Instance.ClearRecentlyPlayed();
			if (!string.IsNullOrEmpty(DataManager.Player.Store.ActiveCosmicube))
			{
				Instance.SetActivePodType(DestroyableSingleton<CosmicubeManager>.Instance.GetCubeDataByID(DataManager.Player.Store.ActiveCosmicube).podId);
				return;
			}
			PlayerStorageManager.CloudPlayerPrefs playerPrefs = DestroyableSingleton<PlayerStorageManager>.Instance.PlayerPrefs;
			Instance.SetActivePodType(playerPrefs.ActivePodType);
		}
	}

	protected override void OnPlayerLeft(ClientData data, DisconnectReasons reason)
	{
		if (DestroyableSingleton<GameStartManager>.InstanceExists)
		{
			DestroyableSingleton<GameStartManager>.Instance.ResetStartState();
		}
		PlayerControl character = data.Character;
		if ((bool)character)
		{
			for (int num = DisconnectHandlers.Count - 1; num > -1; num--)
			{
				IDisconnectHandler disconnectHandler = DisconnectHandlers[num];
				if (disconnectHandler is MonoBehaviour && !(MonoBehaviour)disconnectHandler)
				{
					DisconnectHandlers.RemoveAt(num);
				}
				else
				{
					try
					{
						disconnectHandler.HandleDisconnect(character, reason);
					}
					catch (Exception exception)
					{
						logger.Error("AmongUsClient::OnPlayerLeft: Exception: 1");
						Debug.LogException(exception, this);
						DisconnectHandlers.RemoveAt(num);
					}
				}
			}
			UnityEngine.Object.Destroy(character.gameObject);
		}
		else
		{
			logger.Warning($"A player without a character disconnected: {data.Id}: {data.InScene}");
			for (int num2 = DisconnectHandlers.Count - 1; num2 > -1; num2--)
			{
				IDisconnectHandler disconnectHandler2 = DisconnectHandlers[num2];
				if (disconnectHandler2 is MonoBehaviour && !(MonoBehaviour)disconnectHandler2)
				{
					DisconnectHandlers.RemoveAt(num2);
				}
				else
				{
					try
					{
						disconnectHandler2.HandleDisconnect();
					}
					catch (Exception exception2)
					{
						logger.Error("AmongUsClient::OnPlayerLeft: Exception: 2");
						Debug.LogException(exception2, this);
						DisconnectHandlers.RemoveAt(num2);
					}
				}
			}
		}
		if (AmHost && GameManager.Instance != null && GameManager.Instance.LogicOptions.IsDefaults)
		{
			GameManager.Instance.LogicOptions.SetRecommendations(GameData.Instance.PlayerCount, Instance.NetworkMode);
		}
		RemoveUnownedObjects();
	}

	protected override void PreDisconnectInternal()
	{
		DestroyPlayerInfoObjects();
		GameData.DestroyInstance();
	}

	protected override void OnDisconnected()
	{
		DestroyableSingleton<FriendsListManager>.Instance.ReparentUI();
		DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: false);
		SceneManager.LoadScene(MainMenuScene);
	}

	private void DestroyPlayerInfoObjects()
	{
		lock (allObjects)
		{
			allObjects.DestroyPlayerInfoObjects((GameObject gameObject) =>
			{
				UnityEngine.Object.Destroy(gameObject);
			});
		}
	}

	protected override IEnumerator CoOnPlayerChangedScene(ClientData client, string currentScene)
	{
		client.InScene = true;
		if (GameData.Instance == null)
		{
			GameData.Instance = UnityEngine.Object.Instantiate(GameDataPrefab);
		}
		GameData.Instance.RemoveDisconnectedPlayers();
		if (!AmHost)
		{
			yield break;
		}
		if (VoteBanSystem.Instance == null)
		{
			VoteBanSystem.Instance = UnityEngine.Object.Instantiate(VoteBanPrefab);
			Spawn(VoteBanSystem.Instance);
		}
		if (currentScene.Equals("Tutorial"))
		{
			GameManager.DestroyInstance();
			GameManager netObjParent = GameManagerCreator.CreateGameManager(GameOptionsManager.Instance.CurrentGameOptions.GameMode);
			Spawn(netObjParent);
			int index = ((TutorialMapId == 0 && AprilFoolsMode.ShouldFlipSkeld()) ? 3 : TutorialMapId);
			ShipLoadingAsyncHandle = ShipPrefabs[index].InstantiateAsync((Transform)null, false);
			yield return ShipLoadingAsyncHandle;
			GameObject result = ShipLoadingAsyncHandle.Result;
			ShipLoadingAsyncHandle = default;
			Spawn(result.GetComponent<ShipStatus>());
			yield return CreatePlayer(client);
		}
		else
		{
			if (!currentScene.Equals("OnlineGame"))
			{
				yield break;
			}
			if (client.Id != ClientId)
			{
				SendInitialData(client.Id);
			}
			else
			{
				if (NetworkMode == NetworkModes.LocalGame)
				{
					StartCoroutine(CoBroadcastManager());
				}
				GameManager.DestroyInstance();
				GameManager netObjParent2 = GameManagerCreator.CreateGameManager(GameOptionsManager.Instance.CurrentGameOptions.GameMode);
				Spawn(netObjParent2);
			}
			yield return CreatePlayer(client);
		}
	}

	private static int nextFreeplayFakeClientId = 100000;

	// Host-only: registers a connectionless fake client on every machine and spawns its PlayerControl through the normal
	// CreatePlayer path (GameData info + spawn messages), so all clients see it. Debug "Create character" uses this.
	public IEnumerator CoCreateFakeClientPlayer(string playerName, uint level, string productUserId, string friendCode, Action<PlayerControl> onCreated)
	{
		if (!AmHost)
		{
			throw new InvalidOperationException("Only the host can create fake players.");
		}
		int fakeId;
		if ((NetworkMode == NetworkModes.LocalGame || NetworkMode == NetworkModes.FreePlay) && DestroyableSingleton<InnerNetServer>.InstanceExists)
		{
			fakeId = DestroyableSingleton<InnerNetServer>.Instance.AddFakeClient(playerName, level, productUserId, friendCode);
		}
		else
		{
			fakeId = nextFreeplayFakeClientId++;
		}
		ClientData fake = RegisterFakeClient(new ClientData(fakeId, playerName, new PlatformSpecificData(), level, productUserId, friendCode));
		fake.InScene = true;
		yield return CreatePlayer(fake);
		onCreated?.Invoke(fake.Character);
	}

	private IEnumerator CreatePlayer(ClientData clientData)
	{
		if (clientData.IsBeingCreated || (bool)clientData.Character)
		{
			yield break;
		}
		if (!AmHost)
		{
			logger.Debug("Waiting for host to make my player");
			yield break;
		}
		clientData.IsBeingCreated = true;
		bool isOwnerOfPlayerData = NetworkMode == NetworkModes.LocalGame || (NetworkMode == NetworkModes.OnlineGame && AmHost) || AmModdedHost || NetworkMode == NetworkModes.FreePlay;
		sbyte b;
		if (isOwnerOfPlayerData)
		{
			b = (GameData.Instance.HasPlayer(clientData) ? GameData.Instance.GetPlayerIdFromClient(clientData) : GameData.Instance.GetAvailableId());
			if (b == -1)
			{
				SendLateRejection(clientData.Id, DisconnectReasons.GameFull);
				logger.Info("Overfilled room.");
				clientData.IsBeingCreated = false;
				yield break;
			}
		}
		else
		{
			yield return new WaitUntil(() => GameData.Instance.HasPlayer(clientData));
			b = GameData.Instance.GetPlayerIdFromClient(clientData);
		}
		Vector2 vector = Vector2.zero;
		if (DestroyableSingleton<TutorialManager>.InstanceExists)
		{
			vector = new Vector2(-1.9f, 3.25f);
		}
		PlayerControl pc = UnityEngine.Object.Instantiate(PlayerPrefab, vector, Quaternion.identity);
		pc.PlayerId = (byte)b;
		pc.FriendCode = clientData.FriendCode;
		pc.Puid = clientData.ProductUserId;
		clientData.Character = pc;
		UpdateCachedClients(clientData, clientData.Character);
		if ((bool)ShipStatus.Instance)
		{
			ShipStatus.Instance.SpawnPlayer(pc, Palette.PlayerColors.Length, initialSpawn: false);
		}
		if (isOwnerOfPlayerData)
		{
			NetworkedPlayerInfo netObjParent = GameData.Instance.AddPlayer(pc, clientData);
			Spawn(netObjParent);
		}
		else
		{
			while (GameData.Instance.GetPlayerByClient(clientData) == null)
			{
				yield return null;
			}
		}
		Spawn(pc, clientData.Id, SpawnFlags.IsClientCharacter);
		if (isOwnerOfPlayerData)
		{
			GameData.Instance.DirtyAllData();
		}
		if (GameManager.Instance.LogicOptions.IsDefaults)
		{
			GameManager.Instance.LogicOptions.SetRecommendations(GameData.Instance.PlayerCount, Instance.NetworkMode);
		}
		clientData.IsBeingCreated = false;
	}

	private IEnumerator CoBroadcastManager()
	{
		while (!GameData.Instance)
		{
			yield return null;
		}
		int lastPlayerCount = 0;
		discoverState = DiscoveryState.Broadcast;
		while (discoverState == DiscoveryState.Broadcast)
		{
			if (lastPlayerCount != GameData.Instance.PlayerCount)
			{
				lastPlayerCount = GameData.Instance.PlayerCount;
				string data = $"{DataManager.Player.Customization.Name}~Open~{GameData.Instance.PlayerCount}~";
				DestroyableSingleton<InnerDiscover>.Instance.Interval = 1f;
				DestroyableSingleton<InnerDiscover>.Instance.StartAsServer(data);
			}
			yield return null;
		}
		DestroyableSingleton<InnerDiscover>.Instance.StopServer();
	}

	public void CheckOnlinePermissions(Action success, Action failure, Action loadingCallback, bool checkOnline = true)
	{
		if (checkOnline && !DestroyableSingleton<AccountManager>.Instance.CanPlayOnline())
		{
			LastDisconnectReason = DisconnectReasons.NotAuthorized;
			DestroyableSingleton<DisconnectPopup>.Instance.Show();
		}
		else if (DataManager.Player.Ban.IsBanned)
		{
			LastDisconnectReason = DisconnectReasons.IntentionalLeaving;
			DestroyableSingleton<DisconnectPopup>.Instance.Show();
		}
		else
		{
			success?.Invoke();
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
	private static void InitSceneChangeListener()
	{
		Logger.GlobalInstance.Info("[AmongUsClient::InitSceneChangeListener()] Initializing OnActiveSceneChange detection");
		SceneManager.activeSceneChanged += OnActiveSceneChange;
	}

	private static void OnActiveSceneChange(Scene from, Scene to)
	{
		Logger.GlobalInstance.Info("[AmongUsClient::OnActiveSceneChange()] Scene change detected. Unloading unused assets and running garbage collector.");
		if (SceneManager.GetActiveScene().name != "SplashIntro" && SceneManager.GetActiveScene().name != "EarlyInitialization" && (bool)DestroyableSingleton<FriendsListManager>.Instance)
		{
			DestroyableSingleton<FriendsListManager>.Instance.CloseUI();
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public IEnumerator CoCreateOnlineGame()
	{
		NetworkMode = NetworkModes.OnlineGame;
		yield return CoConnectUsingPhoton(MatchMakerModes.HostAndClient, joinRandom: false);
	}

	public IEnumerator CoJoinOnlinePublicGame(int gameId, string ipAddress, ushort port, MainMenuTarget targetMenu = MainMenuTarget.OnlineMenu)
	{
		NetworkMode = NetworkModes.OnlineGame;
		GameId = gameId;
		MenuTarget = targetMenu;
		yield return CoConnectUsingPhoton(MatchMakerModes.Client, joinRandom: false);
	}

	public IEnumerator CoJoinOnlineGameFromListing(GameListing game, string matchmakerToken)
	{
		NetworkMode = NetworkModes.OnlineGame;
		GameId = game.GameId;
		yield return CoConnectUsingPhoton(MatchMakerModes.Client, joinRandom: false);
	}

	public IEnumerator CoJoinOnlineGameFromCode(int gameId, bool fromEnterCode = false)
	{
		NetworkMode = NetworkModes.OnlineGame;
		GameId = gameId;
		if (fromEnterCode)
		{
			MenuTarget = MainMenuTarget.EnterCodeMenu;
		}
		yield return CoConnectUsingPhoton(MatchMakerModes.Client, joinRandom: false);
	}

	public IEnumerator CoFindGameInfoFromCode(int gameId, Action<HttpMatchmakerManager.FindGameByCodeResponse, string> callback)
	{
		logger.Info("CoFindGameInfoFromCode Photon path");
		NetworkMode = NetworkModes.OnlineGame;
		GameId = gameId;
		LastCustomDisconnect = null;
		bool lobbyConnected = false;
		PhotonOnlineTransport.Instance.ConnectToLobby((success, error) =>
		{
			lobbyConnected = success;
			if (!success)
			{
				LastCustomDisconnect = error;
			}
		});
		float timeout = Time.realtimeSinceStartup + 8f;
		while (!lobbyConnected && string.IsNullOrEmpty(LastCustomDisconnect) && Time.realtimeSinceStartup < timeout)
		{
			yield return null;
		}
		while (lobbyConnected && !PhotonOnlineTransport.Instance.HasReceivedRoomList && Time.realtimeSinceStartup < timeout)
		{
			yield return null;
		}
		Photon.Realtime.RoomInfo room = PhotonOnlineTransport.Instance.FindRoom(InnerNet.GameCode.IntToGameName(gameId));
		callback?.Invoke(CreatePhotonGameInfo(room), null);
	}

	public IEnumerator CoFindGameInfoFromCodeAndJoin(int gameId)
	{
		logger.Info("CoFindGameInfoFromCodeAndJoin Photon path");
		yield return CoJoinOnlineGameFromCode(gameId);
	}

	private HttpMatchmakerManager.FindGameByCodeResponse CreatePhotonGameInfo(Photon.Realtime.RoomInfo room)
	{
		if (room == null || !room.IsOpen || !room.CustomProperties.ContainsKey("settings"))
		{
			return null;
		}
		if (!(room.CustomProperties["settings"] is byte[] settingsBytes))
		{
			return null;
		}
		IGameOptions options;
		try
		{
			options = GameOptionsManager.Instance.gameOptionsFactory.FromBytes(settingsBytes);
		}
		catch
		{
			return null;
		}
		return new HttpMatchmakerManager.FindGameByCodeResponse
		{
			Game = new GameListing
			{
				GameId = InnerNet.GameCode.GameNameToInt(room.Name),
				PlayerCount = (byte)room.PlayerCount,
				MaxPlayers = room.MaxPlayers,
				HostName = room.CustomProperties["host"] as string ?? string.Empty,
				TrueHostName = room.CustomProperties["host"] as string ?? string.Empty,
				HostPlatformName = "Photon",
				MapId = (byte)options.MapId,
				NumImpostors = options.NumImpostors,
				QuickChat = room.CustomProperties["chat"] is byte chatMode ? (QuickChatModes)chatMode : QuickChatModes.QuickChatOnly,
				Options = options
			},
			Region = StringNames.NoTranslation,
			UntranslatedRegion = "Photon"
		};
	}

	public IEnumerator CoFindGame()
	{
		NetworkMode = NetworkModes.OnlineGame;
		LastCustomDisconnect = null;
		bool connected = false;
		PhotonOnlineTransport.Instance.ConnectToLobby((success, error) =>
		{
			connected = success;
			if (!success)
			{
				LastCustomDisconnect = error;
			}
		});
		while (!connected && string.IsNullOrEmpty(LastCustomDisconnect))
		{
			yield return null;
		}
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
		if (connected)
		{
			SceneManager.LoadScene("FindAGame");
		}
		else
		{
			LastDisconnectReason = DisconnectReasons.Error;
			MenuTarget = MainMenuTarget.OnlineMenu;
			SceneManager.LoadScene("MainMenu");
		}
	}

	private IEnumerator CoConnectUsingPhoton(MatchMakerModes requestedMode, bool joinRandom)
	{
		if (mode != MatchMakerModes.None)
		{
			DisconnectInternal(DisconnectReasons.NewConnection);
		}
		mode = requestedMode;
		MainMenuScene = "MainMenu";
		OnlineScene = "OnlineGame";
		PhotonOnlineTransport transport = PhotonOnlineTransport.Instance;
		transport.PrepareGuestProfile();
		transport.SetPlayerName(DataManager.Player.Customization.Name);
		if (requestedMode == MatchMakerModes.HostAndClient)
		{
			GameOptionsManager.Instance.CurrentGameOptions = GameOptionsManager.Instance.GameHostOptions;
			GameId = InnerNet.GameCode.CreateGameId(UnityEngine.Random.Range(0, 1 << 20), UnityEngine.Random.Range(0, InnerNet.GameCode.MaxGameNumber));
		}
		string roomName = InnerNet.GameCode.IntToGameName(GameId);
		bool completed = false;
		bool succeeded = false;
		string resultRoomName = roomName;
		Action<bool, string> callback = (success, result) =>
		{
			succeeded = success;
			completed = true;
			if (success && !string.IsNullOrEmpty(result))
			{
				resultRoomName = result;
			}
			else if (!success)
			{
				LastCustomDisconnect = result;
			}
		};
		PhotonHashtable roomProperties = null;
		if (requestedMode == MatchMakerModes.HostAndClient)
		{
			IGameOptions options = GameOptionsManager.Instance.CurrentGameOptions;
			roomProperties = new PhotonHashtable
			{
				{ "host", DataManager.Player.Customization.Name },
				{ "settings", GameOptionsManager.Instance.gameOptionsFactory.ToBytes(options, AprilFoolsMode.IsAprilFoolsModeToggledOn) },
				{ "map", options.MapId },
				{ "impostors", options.NumImpostors },
				{ "mode", (byte)options.GameMode },
				{ "chat", (byte)DataManager.Settings.Multiplayer.ChatMode }
			};
		}
		bool operationStarted = joinRandom
			? transport.JoinRandomRoom(callback)
			: requestedMode == MatchMakerModes.HostAndClient
				? transport.CreateRoom(roomName, roomProperties, callback)
				: transport.JoinRoom(roomName, callback);
		if (!operationStarted && !completed)
		{
			completed = true;
		}
		float operationDeadline = Time.realtimeSinceStartup + 20f;
		while (!completed && Time.realtimeSinceStartup < operationDeadline)
		{
			yield return null;
		}
		if (!completed)
		{
			LastCustomDisconnect = "Photon room operation timed out.";
			transport.CancelPendingRoomOperation(LastCustomDisconnect);
			completed = true;
		}
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
		if (!succeeded)
		{
			LastDisconnectReason = DisconnectReasons.Error;
			DisconnectInternal(DisconnectReasons.NewConnection);
			if (MenuTarget == MainMenuTarget.FindGameMenu)
			{
				SceneManager.LoadScene("FindAGame");
			}
			else
			{
				MenuTarget = MainMenuTarget.OnlineMenu;
				SceneManager.LoadScene("MainMenu");
			}
			yield break;
		}
		GameId = InnerNet.GameCode.GameNameToInt(resultRoomName);
		ClientId = transport.LocalActorNumber;
		HostId = transport.MasterActorNumber;
		GameState = GameStates.Joined;
		if (transport.CurrentRoomChatMode.HasValue)
		{
			DataManager.Settings.Multiplayer.ChatMode = transport.CurrentRoomChatMode.Value;
			DataManager.Settings.Save();
		}
		byte[] roomSettings = transport.CurrentRoomSettings;
		if (roomSettings != null)
		{
			try
			{
				GameOptionsManager.Instance.CurrentGameOptions = GameOptionsManager.Instance.gameOptionsFactory.FromBytes(roomSettings);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
		}
		lock (allClients)
		{
			allClients.Clear();
			foreach (Photon.Realtime.Player player in transport.Players)
			{
				bool isLocalPlayer = player.ActorNumber == ClientId;
				ClientData data = new ClientData(player.ActorNumber, player.NickName, isLocalPlayer ? Constants.GetPlatformData() : new PlatformSpecificData(), isLocalPlayer ? DataManager.Player.Stats.Level : 1u, string.Empty, string.Empty);
				allClients.Add(data);
				if (!isLocalPlayer)
				{
					OnPlayerJoined(data);
				}
			}
		}
		OnGameJoined(resultRoomName);
	}

	private void OnPhotonPlayerJoined(Photon.Realtime.Player player)
	{
		if (NetworkMode != NetworkModes.OnlineGame || GetClient(player.ActorNumber) != null)
		{
			return;
		}
		ClientData data = new ClientData(player.ActorNumber, player.NickName, new PlatformSpecificData(), 1u, string.Empty, string.Empty);
		lock (allClients)
		{
			allClients.Add(data);
		}
		OnPlayerJoined(data);
	}

	private void OnPhotonPlayerLeft(Photon.Realtime.Player player)
	{
		if (NetworkMode != NetworkModes.OnlineGame)
		{
			return;
		}
		ClientData data;
		lock (allClients)
		{
			data = allClients.FirstOrDefault(client => client.Id == player.ActorNumber);
			if (data != null)
			{
				allClients.Remove(data);
			}
		}
		if (data != null)
		{
			OnPlayerLeft(data, DisconnectReasons.ExitGame);
		}
		HostId = PhotonOnlineTransport.Instance.MasterActorNumber;
	}

	private void OnPhotonMasterClientChanged(Photon.Realtime.Player newMasterClient)
	{
		int previousHostId = HostId;
		HostId = newMasterClient.ActorNumber;
		if (HostId == ClientId && previousHostId != ClientId)
		{
			OnBecomeHost();
		}
	}

	private IEnumerator CoConnectToGameServer(MatchMakerModes mode, string ipAddress, ushort port, string matchmakerToken)
	{
		SetEndpoint(ipAddress, port, DestroyableSingleton<ServerManager>.Instance.UdpUseDtls);
		MainMenuScene = "MainMenu";
		OnlineScene = "OnlineGame";
		Connect(mode, matchmakerToken);
		yield return WaitForConnectionOrFail();
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
		if (ClientId < 0)
		{
			if (MenuTarget == MainMenuTarget.FindGameMenu)
			{
				SceneManager.LoadScene("FindAGame");
				yield break;
			}
			MenuTarget = MainMenuTarget.OnlineMenu;
			SceneManager.LoadScene("MainMenu");
		}
	}

	private IEnumerator CoJoinOnlineGameDirect(int gameId, string ipAddress, ushort port, string matchmakerToken)
	{
		NetworkMode = NetworkModes.OnlineGame;
		GameId = gameId;
		yield return CoConnectToGameServer(MatchMakerModes.Client, ipAddress, port, matchmakerToken);
	}
}
