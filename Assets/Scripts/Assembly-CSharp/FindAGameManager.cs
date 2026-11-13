using System;
using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.GameOptions;
using AmongUs.Matchmaking;
using InnerNet;
using TMPro;
using UnityEngine;

public class FindAGameManager : DestroyableSingleton<FindAGameManager>, IGameListHandler, IConnectButton
{
	private class GameSorter : IComparer<GameListing>
	{
		public static readonly GameSorter Instance = new GameSorter();

		public int Compare(GameListing x, GameListing y)
		{
			return -x.PlayerCount.CompareTo(y.PlayerCount);
		}
	}

	[SerializeField]
	private FilterPopUp filterPopUp;

	[SerializeField]
	private ServerDropdown serverDropdown;

	[SerializeField]
	private PassiveButton serverButton;

	[SerializeField]
	private TextMeshPro[] serverTexts;

	[SerializeField]
	private GameContainer[] gameContainers;

	[SerializeField]
	private PassiveButton refreshButton;

	[SerializeField]
	private GameObject animLoad;

	[SerializeField]
	private FindGameMoreInfoPopup findGameMoreInfoPopUp;

	[SerializeField]
	private GameObject clearFilterPop;

	[SerializeField]
	private TextMeshPro[] filtersFoundNums;

	[SerializeField]
	private TextMeshPro matchesFoundText;

	[SerializeField]
	private AudioClip findGameSFX;

	[SerializeField]
	private AudioClip foundGameSFX;

	[SerializeField]
	private Transform container;

	[SerializeField]
	private TextMeshPro modeText;

	[SerializeField]
	private PassiveButton clearFilterButton;

	public TextMeshPro TotalText;

	private const float RefreshTimeSeconds = 1f;

	private float timer;

	private Coroutine refreshFilterCoroutine;

	private bool animating = true;

	private GameFilterSet classicFilterSet;

	private GameFilterSet hnsFilterSet;

	private const string MAP_FILTER_NAME = "Map";

	private const string LANGUAGE_FILTER_NAME = "Language";

	private const string CHAT_FILTER_NAME = "Chat";

	private const string CROSSPLAY_FILTER_NAME = "Crossplay";

	private const float WAIT_FILTER_REFRESH = 1f;

	private const float WAIT_SLIDE_INTRO = 0.3f;

	private const float Y_POS_OFFSET = -9f;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	private readonly Logger logger = new Logger(Logger.Category.Network, "CreateGameOptions");

	public void Start()
	{
		AmongUsClient.Instance.GameListHandlers.Add(this);
		classicFilterSet = DataManager.Settings.Multiplayer.ClassicFilterSet;
		hnsFilterSet = DataManager.Settings.Multiplayer.HnSFilterSet;
		if (AprilFoolsMode.ShouldShowAprilFoolsToggle())
		{
			AprilFoolsMode.IsAprilFoolsModeToggledOn = true;
		}
		if (AprilFoolsMode.IsAprilFoolsModeToggledOn)
		{
			classicFilterSet.GameMode = GameModes.NormalFools;
			hnsFilterSet.GameMode = GameModes.SeekFools;
		}
		else
		{
			classicFilterSet.GameMode = GameModes.Normal;
			hnsFilterSet.GameMode = GameModes.HideNSeek;
		}
		SetChatRestriction();
		SetLanguage();
		UpdateFiltersTexts();
		ResetTimer();
		ResetContainers();
		GameContainer[] array = gameContainers;
		foreach (GameContainer obj in array)
		{
			obj.OnMoreAction = (Action<GameListing>)Delegate.Combine(obj.OnMoreAction, new Action<GameListing>(OpenMorePopUp));
		}
		ControllerManager.Instance.NewScene(name, BackButton, DefaultButtonSelected, ControllerSelectable);
		serverDropdown.Initialize(UpdateServerText, CloseServerDropdown);
		SetCurrentServer();
		UpdateGameModeText();
		animating = true;
		if (AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame && PhotonOnlineTransport.IsInLobby)
		{
			PhotonOnlineTransport.Instance.RoomListUpdated += HandlePhotonRoomList;
			HandlePhotonRoomList(PhotonOnlineTransport.Instance.Rooms);
			Vector2 startPos = new Vector2(container.localPosition.x, -9f);
			Vector2 endPos = new Vector2(container.localPosition.x, 0f);
			container.localPosition = new Vector3(startPos.x, startPos.y, container.localPosition.z);
			StartCoroutine(CoShow(startPos, endPos));
		}
		else if (DestroyableSingleton<ServerManager>.Instance.IsHttp)
		{
			Vector2 startPos = new Vector2(container.localPosition.x, -9f);
			Vector2 endPos = new Vector2(container.localPosition.x, 0f);
			container.localPosition = new Vector3(startPos.x, startPos.y, container.localPosition.z);
			StartIcon();
			StartCoroutine(DestroyableSingleton<HttpMatchmakerManager>.Instance.CoRefreshFilters((PermittedFilters filters) =>
			{
				RefreshList();
				filterPopUp.SetupFilters(filters);
				StartCoroutine(CoShow(startPos, endPos));
			}));
		}
		else
		{
			RefreshList();
			Vector2 vector = new Vector2(container.localPosition.x, -9f);
			Vector2 vector2 = new Vector2(container.localPosition.x, 0f);
			container.localPosition = new Vector3(vector.x, vector.y, container.localPosition.z);
			StartCoroutine(CoShow(vector, vector2));
		}
	}

	private IEnumerator CoShow(Vector3 startPos, Vector3 endPos)
	{
		yield return Effects.Slide2D(container, startPos, endPos, 0.3f);
		animating = false;
	}

	public MapGameFilter GetMapFilter()
	{
		MapGameFilter result = null;
		foreach (GameFilter filter in classicFilterSet.Filters)
		{
			if (filter.Key == "Map")
			{
				result = (MapGameFilter)filter.SubFilter;
			}
		}
		return result;
	}

	public void ResetTimer()
	{
		timer = 1f;
		SetRefresh(isActive: true);
	}

	private void ResetContainers()
	{
		GameContainer[] array = gameContainers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(value: false);
		}
	}

	private void SetLanguage()
	{
		uint langFilter = GetLangFilter();
		if (langFilter != 0)
		{
			AddLangFilterValue(langFilter, refreshList: false);
		}
		else
		{
			AddLangFilterValue((uint)GameOptionsManager.Instance.GameSearchOptions.Keywords, refreshList: false);
		}
	}

	private void SetChatRestriction()
	{
		if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
		{
			AddChatFilterValue(QuickChatModes.QuickChatOnly, refreshList: false);
		}
		else if (GetChatFilter() != 2)
		{
			AddChatFilterValue(QuickChatModes.FreeChatOrQuickChat, refreshList: false);
		}
		else
		{
			AddChatFilterValue(QuickChatModes.QuickChatOnly, refreshList: false);
		}
	}

	public void UpdateFiltersTexts()
	{
		int num = 0;
		TextMeshPro[] array = filtersFoundNums;
		foreach (TextMeshPro textMeshPro in array)
		{
			switch (GetGameMode())
			{
			case GameModes.Normal:
			case GameModes.NormalFools:
				num = GetFilterCount();
				textMeshPro.text = num.ToString();
				if (num > 0)
				{
					clearFilterButton.SetButtonEnableState(enabled: true);
				}
				else
				{
					clearFilterButton.SetButtonEnableState(enabled: false);
				}
				break;
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				num = GetFilterCount();
				textMeshPro.text = num.ToString();
				if (num > 0)
				{
					clearFilterButton.SetButtonEnableState(enabled: true);
				}
				else
				{
					clearFilterButton.SetButtonEnableState(enabled: false);
				}
				break;
			}
		}
	}

	public int GetRealFilterCount()
	{
		switch (GetGameMode())
		{
		default:
			return classicFilterSet.Filters.Count;
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
			return hnsFilterSet.Filters.Count;
		}
	}

	private int GetFilterCount()
	{
		int result = 0;
		switch (GetGameMode())
		{
		case GameModes.Normal:
		case GameModes.NormalFools:
			result = classicFilterSet.Filters.Count;
			if (result > 0)
			{
				foreach (GameFilter filter in classicFilterSet.Filters)
				{
					if (filter.Key == "Language")
					{
						result--;
					}
					if (filter.Key == "Chat")
					{
						result--;
					}
				}
			}
			return result;
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
			result = hnsFilterSet.Filters.Count;
			if (result > 0)
			{
				foreach (GameFilter filter2 in hnsFilterSet.Filters)
				{
					if (filter2.Key == "Language")
					{
						result--;
					}
					if (filter2.Key == "Chat")
					{
						result--;
					}
				}
			}
			return result;
		default:
			return result;
		}
	}

	public void UpdateGameModeText()
	{
		switch (GetGameMode())
		{
		case GameModes.Normal:
		case GameModes.NormalFools:
			modeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeClassic);
			break;
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
			modeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeHideAndSeek);
			break;
		}
	}

	public GameModes GetGameMode()
	{
		return DataManager.Settings.Multiplayer.LastPlayedGameMode;
	}

	private void SetCurrentServer()
	{
		IRegionInfo currentRegion = DestroyableSingleton<ServerManager>.Instance.CurrentRegion;
		UpdateServerText(DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(currentRegion.TranslateName, currentRegion.Name));
	}

	public void OpenServerDropdown()
	{
		serverButton.SetButtonEnableState(enabled: false);
		serverDropdown.gameObject.SetActive(value: true);
	}

	private void CloseServerDropdown()
	{
		serverButton.SetButtonEnableState(enabled: true);
	}

	private void UpdateServerText(string text)
	{
		RefreshList();
		TextMeshPro[] array = serverTexts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = text;
		}
	}

	public void Update()
	{
		timer += Time.deltaTime;
		if ((timer < 0f || timer > 1f) && !refreshButton.enabled)
		{
			SetRefresh(isActive: true);
		}
	}

	private void SetRefresh(bool isActive)
	{
		serverButton.SetButtonEnableState(isActive);
		refreshButton.SetButtonEnableState(isActive);
	}

	public void OpenMorePopUp(GameListing gameL)
	{
		if (!animating && !filterPopUp.gameObject.activeSelf)
		{
			findGameMoreInfoPopUp.SetupInfo(gameL);
			findGameMoreInfoPopUp.gameObject.SetActive(value: true);
		}
	}

	public void SetFilterPopUp(bool isOn)
	{
		if (!animating && !findGameMoreInfoPopUp.gameObject.activeSelf && (!isOn || !clearFilterPop.activeSelf))
		{
			filterPopUp.gameObject.SetActive(isOn);
		}
	}

	public void OpenFilterClear()
	{
		if (!filterPopUp.gameObject.activeSelf)
		{
			clearFilterPop.SetActive(value: true);
		}
	}

	public void ClearAllFilters()
	{
		if (AprilFoolsMode.IsAprilFoolsModeToggledOn)
		{
			classicFilterSet = new GameFilterSet(GameModes.NormalFools);
			hnsFilterSet = new GameFilterSet(GameModes.SeekFools);
		}
		else
		{
			classicFilterSet = new GameFilterSet(GameModes.Normal);
			hnsFilterSet = new GameFilterSet(GameModes.HideNSeek);
		}
		DataManager.Settings.Multiplayer.CrossPlay = true;
		DataManager.Settings.Multiplayer.ClassicFilterSet = classicFilterSet;
		DataManager.Settings.Multiplayer.HnSFilterSet = hnsFilterSet;
		DataManager.Settings.Save();
		SetLanguage();
		SetChatRestriction();
		UpdateFiltersTexts();
		RefreshList();
		filterPopUp.ResetFilterSelection();
	}

	public void RefreshList()
	{
		if (!(timer >= 1f))
		{
			return;
		}
		timer = 0f;
		SetRefresh(isActive: false);
		if (DestroyableSingleton<MatchMaker>.Instance.Connecting(this))
		{
			if (AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame && PhotonOnlineTransport.IsInLobby)
			{
				ResetContainers();
				SoundManager.Instance.PlaySound(findGameSFX, loop: false);
				HandlePhotonRoomList(PhotonOnlineTransport.Instance.Rooms);
			}
			else if (DestroyableSingleton<ServerManager>.Instance.IsHttp)
			{
				ResetContainers();
				SoundManager.Instance.PlaySound(findGameSFX, loop: false);
				StartCoroutine(DestroyableSingleton<HttpMatchmakerManager>.Instance.CoRequestGameListFiltered(GameOptionsManager.Instance.GameSearchOptions, DataManager.Settings.Multiplayer.SearchGameFilterOptions, HandleList));
			}
			else
			{
				AmongUsClient.Instance.RequestGameList(GameOptionsManager.Instance.GameSearchOptions, DataManager.Settings.Multiplayer.SearchGameFilterOptions);
			}
		}
	}

	public GameFilterSet GetFilterSet()
	{
		switch (GetGameMode())
		{
		case GameModes.Normal:
		case GameModes.NormalFools:
			return classicFilterSet;
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
			return hnsFilterSet;
		default:
			return classicFilterSet;
		}
	}

	public void UpdateFilterSets()
	{
		DataManager.Settings.Multiplayer.ClassicFilterSet = classicFilterSet;
		DataManager.Settings.Multiplayer.HnSFilterSet = hnsFilterSet;
	}

	public override void OnDestroy()
	{
		PhotonOnlineTransport.Instance.RoomListUpdated -= HandlePhotonRoomList;
		if ((bool)AmongUsClient.Instance)
		{
			AmongUsClient.Instance.GameListHandlers.Remove(this);
		}
		base.OnDestroy();
	}

	private void HandlePhotonRoomList(List<Photon.Realtime.RoomInfo> rooms)
	{
		List<GameListing> games = new List<GameListing>();
		GameOptionsFactory optionsFactory = new GameOptionsFactory(new UnityLogger());
		foreach (Photon.Realtime.RoomInfo room in rooms)
		{
			if (!room.IsOpen || room.PlayerCount == 0 || room.PlayerCount >= room.MaxPlayers)
			{
				continue;
			}
			if (!room.CustomProperties.ContainsKey("settings"))
			{
				continue;
			}
			object settingsValue = room.CustomProperties["settings"];
			if (!(settingsValue is byte[] settingsBytes))
			{
				continue;
			}
			IGameOptions options;
			try
			{
				options = optionsFactory.FromBytes(settingsBytes);
			}
			catch
			{
				continue;
			}
			games.Add(new GameListing
			{
				GameId = GameCode.GameNameToInt(room.Name),
				PlayerCount = (byte)room.PlayerCount,
				HostName = room.CustomProperties["host"] as string ?? string.Empty,
				HostPlatformName = "Photon",
				MaxPlayers = room.MaxPlayers,
				MapId = (byte)options.MapId,
				NumImpostors = options.NumImpostors,
				Options = options
			});
		}
		HandleList(null, new HttpMatchmakerManager.FindGamesListFilteredResponse
		{
			Games = games,
			Metadata = new HttpMatchmakerManager.GamesListMetadata
			{
				AllGamesCount = rooms.Count,
				MatchingGamesCount = games.Count
			}
		});
	}

	public void HandleList(InnerNetClient.TotalGameData totalGames, HttpMatchmakerManager.FindGamesListFilteredResponse response)
	{
		if (response == null) return;
		if (response.Metadata == null) response.Metadata = new HttpMatchmakerManager.GamesListMetadata();
		if (response.Games == null) response.Games = new List<GameListing>();
		List<GameListing> games = response.Games;
		logger.Info("HandleList invoked");
		logger.Info($"Games found: {games.Count}");
		logger.Info($"Matching Games found: {response.Metadata.MatchingGamesCount}");
		logger.Info($"All Games found: {response.Metadata.AllGamesCount}");
		SoundManager.Instance.StopSound(findGameSFX);
		SoundManager.Instance.StopSound(foundGameSFX);
		SoundManager.Instance.PlaySound(foundGameSFX, loop: false);
		int num = Mathf.RoundToInt(response.Metadata.AllGamesCount / 100) * 100;
		if (num < 100)
		{
			TotalText.text = "< 100";
		}
		else
		{
			TotalText.text = num + "+";
		}
		matchesFoundText.text = response.Metadata.MatchingGamesCount.ToString();
		try
		{
			DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
			timer = 0f;
			games.Shuffle();
			int num2 = 0;
			for (int i = 0; i < gameContainers.Length && i < games.Count; i++)
			{
				if (games[i].Options != null)
				{
					gameContainers[num2].gameObject.SetActive(value: true);
					gameContainers[num2].SetGameListing(games[i]);
					gameContainers[num2].SetupGameInfo();
					num2++;
				}
			}
		}
		catch (Exception e)
		{
			logger.Error("FindAGameManager::HandleList: Exception: ");
			logger.Exception(e, this);
		}
	}

	public void ExitGame()
	{
		if (!animating)
		{
			AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
		}
	}

	public void StartIcon()
	{
		if ((bool)animLoad)
		{
			animLoad.SetActive(value: true);
		}
	}

	public void StopIcon()
	{
		if ((bool)animLoad)
		{
			animLoad.SetActive(value: false);
		}
	}

	public void FilterModified()
	{
		if (refreshFilterCoroutine != null)
		{
			StopCoroutine(refreshFilterCoroutine);
		}
		refreshFilterCoroutine = StartCoroutine(RefreshListCo());
	}

	private IEnumerator RefreshListCo()
	{
		yield return new WaitForSeconds(1f);
		ResetTimer();
		RefreshList();
	}

	public void AddBoolFilterValue(bool value, string key, BoolOptionNames boolOptionNames)
	{
		bool flag = false;
		foreach (GameFilter filter in GetFilterSet().Filters)
		{
			if (filter.Key == key)
			{
				flag = true;
				if (filter.SubFilter is BoolGameFilter boolGameFilter && !boolGameFilter.AcceptedValues.Contains(value))
				{
					boolGameFilter.AcceptedValues.Add(value);
					filter.ModifySubFilter(boolGameFilter);
				}
				break;
			}
		}
		if (flag)
		{
			UpdateFiltersTexts();
			UpdateFilterSets();
			FilterModified();
			return;
		}
		GetFilterSet().Filters.Add(new GameFilter(key, new BoolGameFilter
		{
			AcceptedValues = new List<bool> { value },
			OptionEnum = boolOptionNames
		}));
		UpdateFiltersTexts();
		UpdateFilterSets();
		FilterModified();
	}

	public void RemoveBoolFilterValue(bool value, string key, BoolOptionNames boolOptionNames)
	{
		GameFilter gameFilter = null;
		foreach (GameFilter filter in GetFilterSet().Filters)
		{
			if (!(filter.Key == key))
			{
				continue;
			}
			if (filter.SubFilter is BoolGameFilter boolGameFilter && boolGameFilter.AcceptedValues.Contains(value))
			{
				boolGameFilter.AcceptedValues.Remove(value);
				if (boolGameFilter.AcceptedValues.Count <= 0)
				{
					gameFilter = filter;
				}
			}
			break;
		}
		if (gameFilter != null)
		{
			GetFilterSet().Filters.Remove(gameFilter);
		}
		UpdateFiltersTexts();
		UpdateFilterSets();
		FilterModified();
	}

	public void AddIntFilterValue(int value, string key, Int32OptionNames intOptionName)
	{
		bool flag = false;
		foreach (GameFilter filter in GetFilterSet().Filters)
		{
			if (filter.Key == key)
			{
				flag = true;
				if (filter.SubFilter is IntGameFilter intGameFilter && !intGameFilter.AcceptedValues.Contains(value))
				{
					intGameFilter.AcceptedValues.Add(value);
					filter.ModifySubFilter(intGameFilter);
				}
				break;
			}
		}
		if (flag)
		{
			UpdateFiltersTexts();
			UpdateFilterSets();
			FilterModified();
			return;
		}
		GetFilterSet().Filters.Add(new GameFilter(key, new IntGameFilter
		{
			AcceptedValues = new List<int> { value },
			OptionEnum = intOptionName
		}));
		UpdateFiltersTexts();
		UpdateFilterSets();
		FilterModified();
	}

	public void RemoveIntFilterValue(int value, string key, Int32OptionNames intOptionName)
	{
		GameFilter gameFilter = null;
		foreach (GameFilter filter in GetFilterSet().Filters)
		{
			if (!(filter.Key == key))
			{
				continue;
			}
			if (filter.SubFilter is IntGameFilter intGameFilter && intGameFilter.AcceptedValues.Contains(value))
			{
				intGameFilter.AcceptedValues.Remove(value);
				if (intGameFilter.AcceptedValues.Count <= 0)
				{
					gameFilter = filter;
				}
			}
			break;
		}
		if (gameFilter != null)
		{
			GetFilterSet().Filters.Remove(gameFilter);
		}
		UpdateFiltersTexts();
		UpdateFilterSets();
		FilterModified();
	}

	public void AddCatFilterValue(int value, string key, CategorizedOptionNames catFilterName)
	{
		bool flag = false;
		foreach (GameFilter filter in GetFilterSet().Filters)
		{
			if (filter.Key == key)
			{
				flag = true;
				if (filter.SubFilter is CategorizedGameFilter categorizedGameFilter && !categorizedGameFilter.AcceptedValues.Contains(value))
				{
					categorizedGameFilter.AcceptedValues.Add(value);
					filter.ModifySubFilter(categorizedGameFilter);
				}
				break;
			}
		}
		if (flag)
		{
			UpdateFiltersTexts();
			UpdateFilterSets();
			FilterModified();
			return;
		}
		GetFilterSet().Filters.Add(new GameFilter(key, new CategorizedGameFilter
		{
			AcceptedValues = new List<int> { value },
			OptionEnum = catFilterName
		}));
		UpdateFiltersTexts();
		UpdateFilterSets();
		FilterModified();
	}

	public void RemoveCatFilterValue(int value, string key, CategorizedOptionNames catFilterName)
	{
		GameFilter gameFilter = null;
		foreach (GameFilter filter in GetFilterSet().Filters)
		{
			if (!(filter.Key == key))
			{
				continue;
			}
			if (filter.SubFilter is CategorizedGameFilter categorizedGameFilter && categorizedGameFilter.AcceptedValues.Contains(value))
			{
				categorizedGameFilter.AcceptedValues.Remove(value);
				if (categorizedGameFilter.AcceptedValues.Count <= 0)
				{
					gameFilter = filter;
				}
			}
			break;
		}
		if (gameFilter != null)
		{
			GetFilterSet().Filters.Remove(gameFilter);
		}
		UpdateFiltersTexts();
		UpdateFilterSets();
		FilterModified();
	}

	public void SetMapFilter(byte mapByte)
	{
		GameFilter gameFilter = null;
		GameFilter gameFilter2 = null;
		foreach (GameFilter filter in classicFilterSet.Filters)
		{
			if (filter.Key == "Map")
			{
				gameFilter = filter;
				break;
			}
		}
		foreach (GameFilter filter2 in hnsFilterSet.Filters)
		{
			if (filter2.Key == "Map")
			{
				gameFilter2 = filter2;
				break;
			}
		}
		if (gameFilter != null)
		{
			classicFilterSet.Filters.Remove(gameFilter);
		}
		if (gameFilter2 != null)
		{
			hnsFilterSet.Filters.Remove(gameFilter2);
		}
		classicFilterSet.Filters.Add(new GameFilter("Map", new MapGameFilter
		{
			AcceptedValues = mapByte
		}));
		hnsFilterSet.Filters.Add(new GameFilter("Map", new MapGameFilter
		{
			AcceptedValues = mapByte
		}));
		UpdateFiltersTexts();
		UpdateFilterSets();
		FilterModified();
	}

	public void RemoveMapFilter()
	{
		GameFilter gameFilter = null;
		GameFilter gameFilter2 = null;
		foreach (GameFilter filter in classicFilterSet.Filters)
		{
			if (filter.Key == "Map")
			{
				gameFilter = filter;
				break;
			}
		}
		foreach (GameFilter filter2 in hnsFilterSet.Filters)
		{
			if (filter2.Key == "Map")
			{
				gameFilter2 = filter2;
				break;
			}
		}
		if (gameFilter != null)
		{
			classicFilterSet.Filters.Remove(gameFilter);
		}
		if (gameFilter2 != null)
		{
			hnsFilterSet.Filters.Remove(gameFilter2);
		}
		UpdateFiltersTexts();
		UpdateFilterSets();
		FilterModified();
	}

	public void AddLangFilterValue(uint value, bool refreshList = true)
	{
		GameFilter gameFilter = null;
		GameFilter gameFilter2 = null;
		foreach (GameFilter filter in classicFilterSet.Filters)
		{
			if (filter.Key == "Language")
			{
				gameFilter = filter;
				break;
			}
		}
		foreach (GameFilter filter2 in hnsFilterSet.Filters)
		{
			if (filter2.Key == "Language")
			{
				gameFilter2 = filter2;
				break;
			}
		}
		if (gameFilter != null)
		{
			classicFilterSet.Filters.Remove(gameFilter);
		}
		if (gameFilter2 != null)
		{
			hnsFilterSet.Filters.Remove(gameFilter2);
		}
		classicFilterSet.Filters.Add(new GameFilter("Language", new LanguageFilter
		{
			AcceptedValues = value
		}));
		hnsFilterSet.Filters.Add(new GameFilter("Language", new LanguageFilter
		{
			AcceptedValues = value
		}));
		UpdateFiltersTexts();
		UpdateFilterSets();
		if (refreshList)
		{
			FilterModified();
		}
	}

	public uint GetLangFilter()
	{
		GameFilter gameFilter = null;
		foreach (GameFilter filter in classicFilterSet.Filters)
		{
			if (filter.Key == "Language")
			{
				gameFilter = filter;
				break;
			}
		}
		if (gameFilter != null)
		{
			if (gameFilter.SubFilter is LanguageFilter languageFilter)
			{
				return languageFilter.AcceptedValues;
			}
			return 0u;
		}
		return 0u;
	}

	public void AddChatFilterValue(QuickChatModes mode, bool refreshList = true)
	{
		GameFilter gameFilter = null;
		GameFilter gameFilter2 = null;
		foreach (GameFilter filter in classicFilterSet.Filters)
		{
			if (filter.Key == "Chat")
			{
				gameFilter = filter;
				break;
			}
		}
		foreach (GameFilter filter2 in hnsFilterSet.Filters)
		{
			if (filter2.Key == "Chat")
			{
				gameFilter2 = filter2;
				break;
			}
		}
		if (gameFilter != null)
		{
			classicFilterSet.Filters.Remove(gameFilter);
		}
		if (gameFilter2 != null)
		{
			hnsFilterSet.Filters.Remove(gameFilter2);
		}
		classicFilterSet.Filters.Add(new GameFilter("Chat", new ChatModeGameFilter
		{
			AcceptedValues = (byte)mode
		}));
		hnsFilterSet.Filters.Add(new GameFilter("Chat", new ChatModeGameFilter
		{
			AcceptedValues = (byte)mode
		}));
		UpdateFiltersTexts();
		UpdateFilterSets();
		if (refreshList)
		{
			FilterModified();
		}
	}

	public void AddCrossplayFilterValue(uint mode, bool refreshList = true)
	{
		GameFilter gameFilter = null;
		GameFilter gameFilter2 = null;
		foreach (GameFilter filter in classicFilterSet.Filters)
		{
			if (filter.Key == "Crossplay")
			{
				gameFilter = filter;
				break;
			}
		}
		foreach (GameFilter filter2 in hnsFilterSet.Filters)
		{
			if (filter2.Key == "Crossplay")
			{
				gameFilter2 = filter2;
				break;
			}
		}
		if (gameFilter != null)
		{
			classicFilterSet.Filters.Remove(gameFilter);
		}
		if (gameFilter2 != null)
		{
			hnsFilterSet.Filters.Remove(gameFilter2);
		}
		classicFilterSet.Filters.Add(new GameFilter("Crossplay", new PlatformGameFilter
		{
			AcceptedValues = mode
		}));
		hnsFilterSet.Filters.Add(new GameFilter("Crossplay", new PlatformGameFilter
		{
			AcceptedValues = mode
		}));
		UpdateFiltersTexts();
		UpdateFilterSets();
		if (refreshList)
		{
			FilterModified();
		}
	}

	public int GetChatFilter()
	{
		GameFilter gameFilter = null;
		foreach (GameFilter filter in classicFilterSet.Filters)
		{
			if (filter.Key == "Chat")
			{
				gameFilter = filter;
				break;
			}
		}
		if (gameFilter != null)
		{
			if (gameFilter.SubFilter is ChatModeGameFilter chatModeGameFilter)
			{
				return chatModeGameFilter.AcceptedValues;
			}
			return -1;
		}
		return -1;
	}

	public void ToggleAprilFools(bool isOn)
	{
		AprilFoolsMode.IsAprilFoolsModeToggledOn = isOn;
		if (AprilFoolsMode.IsAprilFoolsModeToggledOn)
		{
			classicFilterSet.GameMode = GameModes.NormalFools;
			hnsFilterSet.GameMode = GameModes.SeekFools;
		}
		else
		{
			classicFilterSet.GameMode = GameModes.Normal;
			hnsFilterSet.GameMode = GameModes.HideNSeek;
		}
	}
}
