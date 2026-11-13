using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AmongUs.Data;
using Assets.InnerNet;
using InnerNet;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FriendsListManager : DestroyableSingleton<FriendsListManager>
{
	public class PlatformFriend
	{
		public string Username;

		public ulong GUID;

		public CSteamID CSteamID;

		public PlatformFriend(string name, CSteamID CSteamID)
		{
			Username = name;
			this.CSteamID = CSteamID;
		}
	}

	public class RecentPlayedWithPlayer
	{
		public readonly string PlayerName;

		public readonly string Puid;

		public readonly string FriendCode;

		public readonly int clientId;

		public readonly int roomID;

		public readonly string gameServerID;

		public bool isReported;

		public RecentPlayedWithPlayer(NetworkedPlayerInfo player, int currentRoomID, string currentServerID)
		{
			PlayerName = player.PlayerName;
			Puid = player.Puid;
			FriendCode = player.FriendCode;
			clientId = player.ClientId;
			roomID = currentRoomID;
			gameServerID = currentServerID;
			isReported = false;
		}
	}

	public class ReportInfo
	{
		public string gameServerID;

		public int clientID;

		public int roomID;

		public ReportReasons reason;
	}

	public JoinGameButton joinGameButton;

	public List<ResponseFriends> Friends = new List<ResponseFriends>();

	public List<PlatformFriend> PlatformFriends = new List<PlatformFriend>();

	public List<ResponseBlockedPlayer> BlockedPlayers = new List<ResponseBlockedPlayer>();

	public List<ResponseBlockedBy> BlockedByPlayers = new List<ResponseBlockedBy>();

	public List<ResponseReceivedFriendRequest> ReceivedRequests = new List<ResponseReceivedFriendRequest>();

	public List<LobbyInvite> ReceivedLobbyInvites = new List<LobbyInvite>();

	public List<RecentPlayedWithPlayer> RecentlyPlayedWith = new List<RecentPlayedWithPlayer>();

	public FriendListNotification FriendListNotification;

	public FriendsListConfirmMenu ConfirmationScreen;

	public ReportReasonScreen ReportScreen;

	public FriendsListUI Ui;

	public FriendsListButton FriendsListButton;

	public SpriteRenderer LoadScreen;

	public bool FriendCodeHidden = true;

	public Sprite GlobalPlayerIcon;

	private ReportInfo currentReportInfo;

	private List<int> currentReportedIDs = new List<int>();

	[HideInInspector]
	public Dictionary<int, StringNames> ErrorCodes = new Dictionary<int, StringNames>
	{
		{
			1003,
			StringNames.ErrorBadUsername
		},
		{
			1201,
			StringNames.AlreadyBlocked
		},
		{
			1209,
			StringNames.ErrorThisIsYou
		},
		{
			1300,
			StringNames.ErrorUserNotFound
		},
		{
			3500,
			StringNames.ErrorFriendRequestExists
		},
		{
			3501,
			StringNames.ErrorAlreadyFriends
		},
		{
			3512,
			StringNames.ErrorPlayerBlockedYou
		},
		{
			3513,
			StringNames.ErrorRecipientMaxFriendRequests
		},
		{
			3515,
			StringNames.ErrorMaxFriends
		},
		{
			3516,
			StringNames.ErrorRecipientMaxFriends
		},
		{
			1337420,
			StringNames.UserIdTokenError
		}
	};

	private readonly Logger logger = new Logger(Logger.Category.Network, "FriendsListManager");

	private Coroutine pollingRoutine;

	private bool gotFriendRequests;

	private bool gotLobbyInvites;

	[SerializeField]
	private FriendsListConfirmMenu ConfirmationScreenPrefab;

	[SerializeField]
	private FriendsListUI UiPrefab;

	[SerializeField]
	private FriendsListButton FriendsListButtonPrefab;

	[SerializeField]
	private ReportReasonScreen reportScreenPrefab;

	public static CSteamID myId;

	private Callback<GameRichPresenceJoinRequested_t> m_GameRichPresenceJoinRequested;

	public override void Awake()
	{
		base.Awake();
		if (DestroyableSingleton<FriendsListManager>.Instance == this)
		{
			FriendsListUI.Instance = Ui;
		}
		DataManager.Settings.Multiplayer.OnAllowFriendInvitesChanged += OnAllowFriendInvitesChanged;
		OnAwake();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		DataManager.Settings.Multiplayer.OnAllowFriendInvitesChanged -= OnAllowFriendInvitesChanged;
	}

	private void OnAllowFriendInvitesChanged()
	{
		if (DataManager.Settings.Multiplayer.AllowFriendInvites)
		{
			StartPolling();
		}
		else
		{
			StopPolling();
		}
	}

	public void OnSignIn()
	{
		StartCoroutine(RefreshFriendsList(null, checkPSNBlocked: false));
		if (DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed())
		{
			StartPolling();
		}
	}

	public bool HasPlayerBlockedMeUsername(string friendcode)
	{
		if (BlockedByPlayers.Find((ResponseBlockedBy p) => p.FriendCode == friendcode) != null)
		{
			return true;
		}
		return false;
	}

	public bool HasPlayerBlockedMe(string puid)
	{
		if (BlockedByPlayers.Find((ResponseBlockedBy p) => p.SenderId == puid) != null)
		{
			return true;
		}
		return false;
	}

	public bool IsPlayerBlocked(string puid)
	{
		if (BlockedPlayers.Find((ResponseBlockedPlayer p) => p.FriendPuid == puid) != null)
		{
			return true;
		}
		return false;
	}

	public bool IsPlayerFriend(string puid)
	{
		if (Friends.Find((ResponseFriends p) => p.FriendPuid == puid) != null)
		{
			return true;
		}
		return false;
	}

	public bool IsPlayerBlockedUsername(string friendcode)
	{
		if (BlockedPlayers.Find((ResponseBlockedPlayer p) => p.FriendCode == friendcode) != null)
		{
			return true;
		}
		return false;
	}

	// Offline remake: the original downloads the friends / blocked lists from the live friends service. There is none; the lists stay empty.
	public IEnumerator RefreshFriendsList(Action cb, bool checkPSNBlocked = true)
	{
		cb?.Invoke();
		yield break;
	}

	public void OnSignOut()
	{
		StopPolling();
	}

	// Offline remake: there is no friends service to poll for friend requests / lobby invites, so polling is never started.
	private void StartPolling()
	{
		StopPolling();
	}

	private void StopPolling()
	{
		if (pollingRoutine != null)
		{
			StopCoroutine(pollingRoutine);
			pollingRoutine = null;
		}
	}

	public void AddRecentlyPlayed(NetworkedPlayerInfo player)
	{
		if (DestroyableSingleton<TutorialManager>.InstanceExists)
		{
			return;
		}
		if (RecentlyPlayedWith != null)
		{
			bool flag = false;
			int index = -1;
			for (int i = 0; i < RecentlyPlayedWith.Count; i++)
			{
				if (RecentlyPlayedWith[i].FriendCode == player.FriendCode)
				{
					flag = true;
					index = i;
				}
			}
			if (flag)
			{
				RecentlyPlayedWith.RemoveAt(index);
			}
			// TODO(verify): Mono uses IPAddress.Parse; community servers may be addressed by hostname, so fall back instead of throwing.
			string networkAddress = AmongUsClient.Instance.GetNetworkAddress();
			uint addressValue = IPAddress.TryParse(networkAddress, out var parsedAddress) ? (uint)parsedAddress.Address : 0u;
			RecentlyPlayedWith.Add(new RecentPlayedWithPlayer(player, AmongUsClient.Instance.GameId, $"{addressValue}:{AmongUsClient.Instance.GetNetworkPort()}"));
		}
		List<NetworkedPlayerInfo> list = new List<NetworkedPlayerInfo>();
		list.Add(player);
		UpdateFriendsDataWithPlayers(list);
	}

	public void ClearRecentlyPlayed()
	{
		if (RecentlyPlayedWith != null)
		{
			RecentlyPlayedWith.Clear();
		}
		RecentlyPlayedWith = new List<RecentPlayedWithPlayer>();
	}

	public void UpdateFriendsDataWithPlayers(List<NetworkedPlayerInfo> players)
	{
		if (players == null || players.Count == 0)
		{
			return;
		}
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder(string.Format("Updating {0} with player info from {1} players!)", "PlayerFriendData", players.Count));
		foreach (NetworkedPlayerInfo player in players)
		{
			if (player == null || string.IsNullOrWhiteSpace(player.PlayerName) || string.IsNullOrWhiteSpace(player.Puid) || player.IsIncomplete)
			{
				stringBuilder.AppendLine(" - Player " + "[Uninitialized]".Color(Palette.LogErrorColor));
				continue;
			}
			stringBuilder.AppendLine(" - " + player.PlayerName + " (PUID: " + player.Puid + ") ");
			if (IsPlayerFriend(player.Puid))
			{
				stringBuilder.Append("[Friend] ".Color(Palette.LogSuccessColor));
				string cachedName = DataManager.Player.Friends.GetCachedName(player.Puid);
				if (string.IsNullOrWhiteSpace(cachedName) || cachedName != player.PlayerName)
				{
					stringBuilder.Append("[Cached new name]".Color(Palette.LogWarningColor));
					DataManager.Player.Friends.UpdateCachedName(player.Puid, player.PlayerName);
					flag = true;
				}
			}
		}
		if (flag)
		{
			logger.Info(stringBuilder);
			DataManager.Player.Save();
		}
	}

	public void ReparentUI()
	{
		if (FriendsListButton == null)
		{
			FriendsListButton = UnityEngine.Object.Instantiate(FriendsListButtonPrefab, transform);
		}
		if (Ui == null)
		{
			Ui = UnityEngine.Object.Instantiate(UiPrefab, transform);
		}
		if (ConfirmationScreen == null)
		{
			ConfirmationScreen = UnityEngine.Object.Instantiate(ConfirmationScreenPrefab, transform);
		}
		if (ReportScreen == null)
		{
			ReportScreen = UnityEngine.Object.Instantiate(reportScreenPrefab, transform);
		}
		FriendsListButton.gameObject.transform.SetParent(DestroyableSingleton<FriendsListManager>.Instance.transform);
		Ui.gameObject.transform.SetParent(DestroyableSingleton<FriendsListManager>.Instance.transform);
		ConfirmationScreen.gameObject.transform.SetParent(DestroyableSingleton<FriendsListManager>.Instance.transform);
		ReportScreen.gameObject.transform.SetParent(DestroyableSingleton<FriendsListManager>.Instance.transform);
		ReportScreen.Hide();
	}

	public void OpenUi()
	{
		Ui.Open();
	}

	public void CloseUI()
	{
		if ((bool)Ui && Ui.gameObject.activeSelf)
		{
			Ui.Close(force: true);
		}
	}

	public void JoinGameViaFriendInvite(string roomCode)
	{
		StartCoroutine(CoJoinGameViaFriendInvite(roomCode));
	}

	private IEnumerator CoJoinGameViaFriendInvite(string roomCode)
	{
		LoadScreen.enabled = true;
		if (AmongUsClient.Instance.AmClient)
		{
			AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
		}
		int gameId = GameCode.GameNameToInt(roomCode);
		yield return AmongUsClient.Instance.CoFindGameInfoFromCodeAndJoin(gameId);
		LoadScreen.enabled = false;
	}

	public IEnumerator CheckFriendCodeOnLogin()
	{
		if (DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn)
		{
			if (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.FriendCode))
			{
				DestroyableSingleton<EOSManager>.Instance.FriendCode = "finfloral#8654";
			}

			yield return StartCoroutine(DestroyableSingleton<FriendsListManager>.Instance.GetFriendCode((ResponseState cb, Response<ResponseFriendCode> response) =>
			{
				if (cb == ResponseState.Success && response?.Data?.Attributes != null)
				{
					DestroyableSingleton<EOSManager>.Instance.FriendCode = response.Data.Attributes.Username + "#" + response.Data.Attributes.Discriminator;
					logger.Info("FriendsListManager: Live authentic friend code received: " + DestroyableSingleton<EOSManager>.Instance.FriendCode);
				}
				else
				{
					logger.Info("FriendsListManager: remote friend code query uncontacted/failed, retained local friend code: " + DestroyableSingleton<EOSManager>.Instance.FriendCode);
				}
			}));

			if (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.FriendCode))
			{
				DestroyableSingleton<EOSManager>.Instance.FriendCode = "finfloral#8654";
			}

			while (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.FriendCode) || (DestroyableSingleton<EOSManager>.Instance.editAccountUsername != null && DestroyableSingleton<EOSManager>.Instance.editAccountUsername.gameObject.activeSelf))
			{
				yield return null;
			}
		}
		else
		{
			DestroyableSingleton<EOSManager>.Instance.FriendCode = null;
		}
	}

	public IEnumerator GetFriendCode(Action<ResponseState, Response<ResponseFriendCode>> resultCallback)
	{
		string friendcodeEndpoint = BackendEndpoints.FriendcodeEndpoint;
		yield return StartCoroutine(Get(friendcodeEndpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendCode> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendCode>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to retrieve friend code: " + text);
			}
		}));
	}

	public void SetFriendCode(string username, Action<ResponseState, Response<ResponseFriendCode>> resultCallback)
	{
		string friendcodeEndpoint = BackendEndpoints.FriendcodeEndpoint;
		string jsonContent = JsonConvert.SerializeObject(FriendsListRequest.ChangeUsername(username));
		StartCoroutine(Post(friendcodeEndpoint, jsonContent, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendCode> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendCode>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to set friend code: " + text);
			}
		}));
	}

	public void OpenConfirmationScreen(Action confirm, FriendsListConfirmMenu.ActionType type, string recipientFriendCode)
	{
		StartCoroutine(ConfirmationScreen.WaitForConfirm(confirm, type, recipientFriendCode));
	}

	public void OpenReportScreen(string playerName, int colorID, RecentPlayedWithPlayer recentPlayer)
	{
		currentReportInfo = new ReportInfo();
		currentReportInfo.clientID = recentPlayer.clientId;
		currentReportInfo.roomID = recentPlayer.roomID;
		currentReportInfo.gameServerID = recentPlayer.gameServerID;
		if (SceneManager.GetActiveScene().name == "OnlineGame")
		{
			ReportScreen.transform.parent = DestroyableSingleton<HudManager>.Instance.transform;
		}
		else
		{
			ReportScreen.transform.parent = transform;
		}
		ReportScreen.Show(playerName, colorID, fromFriends: true);
	}

	public ReportInfo GetReportInfo()
	{
		return currentReportInfo;
	}

	public void StartReport()
	{
		Ui.Close();
		ReportPlayerInLastPlayedLobby(currentReportInfo.gameServerID, currentReportInfo.clientID, currentReportInfo.roomID, currentReportInfo.reason, null);
	}

	public bool CheckAlreadyReported(int clientID)
	{
		foreach (int currentReportedID in currentReportedIDs)
		{
			if (clientID == currentReportedID)
			{
				return true;
			}
		}
		return false;
	}

	private IEnumerator PollForRequests()
	{
		while (true)
		{
			while (!DataManager.Settings.Multiplayer.AllowFriendInvites)
			{
				yield return null;
			}
			gotFriendRequests = false;
			gotLobbyInvites = false;
			float pollingInterval = 5f;
			if (DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed() && !ShipStatus.Instance)
			{
				DestroyableSingleton<FriendsListManager>.Instance.GetReceivedFriendRequests((ResponseState cb, Response<ResponseReceivedFriendRequestWrapper> response) =>
				{
					if (cb == ResponseState.Success && DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed())
					{
						List<ResponseReceivedFriendRequest> list = response.Data?.Attributes?.Senders.Select((NestedResponseData<ResponseReceivedFriendRequest> f) => f.Attributes).ToList();
						list.RemoveAll((ResponseReceivedFriendRequest f) => IsPlayerBlocked(f.SenderId));
						List<string> list2 = list.Select((ResponseReceivedFriendRequest inv) => inv.SenderFriendCode).Except(ReceivedRequests.Select((ResponseReceivedFriendRequest inv) => inv.SenderFriendCode)).ToList();
						ReceivedRequests = list;
						if (list2.Count > 0)
						{
							DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NewRequests, list2.Count), FriendListNotification.IconType.Friend);
						}
					}
					gotFriendRequests = true;
				});
				DestroyableSingleton<FriendsListManager>.Instance.GetLobbyInvites((ResponseState cb, Response<ResponseGameInvites> response) =>
				{
					if (cb == ResponseState.Success && DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed())
					{
						pollingInterval = response.Data?.Attributes?.NextPollSeconds ?? 5f;
						List<LobbyInvite> list = response.Data?.Attributes?.Invites.Select((NestedResponseData<LobbyInvite> f) => f.Attributes).ToList();
						list.RemoveAll((LobbyInvite f) => IsPlayerBlocked(f.SenderId));
						List<string> list2 = list.Select((LobbyInvite inv) => inv.RoomCode).Except(ReceivedLobbyInvites.Select((LobbyInvite inv) => inv.RoomCode)).ToList();
						ReceivedLobbyInvites = list;
						if (list2.Count > 0)
						{
							DestroyableSingleton<FriendsListManager>.Instance.FriendListNotification.SetUp(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NewRequests, list2.Count), FriendListNotification.IconType.LobbyInvite);
						}
					}
					gotLobbyInvites = true;
				});
				while (!gotFriendRequests || !gotLobbyInvites)
				{
					yield return null;
				}
				FriendsListButton.CheckNotifCount();
				if (DestroyableSingleton<FriendsListManager>.Instance.Ui.IsOpen && DestroyableSingleton<FriendsListManager>.Instance.Ui.CurrentTab != FriendsListUI.FriendsListTab.Notifications)
				{
					DestroyableSingleton<FriendsListManager>.Instance.Ui.RefreshNotifications();
				}
			}
			pollingInterval = Mathf.Clamp(pollingInterval, 3f, 60f);
			yield return new WaitForSecondsRealtime(pollingInterval);
		}
	}

	public void GetFriends(Action<ResponseState, Response<ResponseFriendWrapper>> resultCallback)
	{
		string friendEndpoint = BackendEndpoints.FriendEndpoint;
		StartCoroutine(Get(friendEndpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendWrapper> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendWrapper>>(response);
			if (response2 == null)
			{
				response2 = new Response<ResponseFriendWrapper>();
			}
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to retrieve friends: " + text);
			}
		}));
	}

	public void GetReceivedFriendRequests(Action<ResponseState, Response<ResponseReceivedFriendRequestWrapper>> resultCallback)
	{
		string friendRequestsEndpoint = BackendEndpoints.FriendRequestsEndpoint;
		StartCoroutine(Get(friendRequestsEndpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseReceivedFriendRequestWrapper> response2 = JsonConvert.DeserializeObject<Response<ResponseReceivedFriendRequestWrapper>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to retrieve friend request: " + text);
			}
		}));
	}

	public void SendFriendRequest(string recipientPuid, Action<ResponseState, Response<ResponseFriendsListRequest>> resultCallback)
	{
		string friendSendRequestEndpoint = BackendEndpoints.FriendSendRequestEndpoint;
		string jsonContent = JsonConvert.SerializeObject(FriendsListRequest.SendRequest(recipientPuid, "friend_request"));
		StartCoroutine(Post(friendSendRequestEndpoint, jsonContent, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendsListRequest> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendsListRequest>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to send friend request by puid: " + text);
			}
			else
			{
				logger.Info("Sent friend request successfully by puid");
			}
		}));
	}

	public void SendFriendRequestByUsername(string username, Action<ResponseState, Response<ResponseFriendsListRequest>> resultCallback)
	{
		string friendSendRequestByFriendcodeEndpoint = BackendEndpoints.FriendSendRequestByFriendcodeEndpoint;
		string jsonContent = JsonConvert.SerializeObject(FriendsListRequest.SendRequestByUsername(username));
		StartCoroutine(Post(friendSendRequestByFriendcodeEndpoint, jsonContent, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendsListRequest> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendsListRequest>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to send friend request by username: " + text);
			}
			else
			{
				logger.Info("Sent friend request successfully by username");
			}
		}));
	}

	public void ReportPlayerInLastPlayedLobby(string gameServerId, int reporteeClientId, int roomId, ReportReasons reason, Action<ResponseState, Response<EmptyResponseBody>> resultCallback)
	{
		string reportPlayerEndpoint = BackendEndpoints.ReportPlayerEndpoint;
		string json = JsonConvert.SerializeObject(PlayerReportRequest.SendPlayerReportOutsideOfLobby(gameServerId, reporteeClientId, roomId, reason));
		StartCoroutine(Post(reportPlayerEndpoint, json, (ResponseState cb, string response) =>
		{
			Response<EmptyResponseBody> response2 = JsonConvert.DeserializeObject<Response<EmptyResponseBody>>(response);
			resultCallback?.Invoke(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to send player report " + json + ": " + text);
			}
			else
			{
				MarkReportedPlayer(reporteeClientId);
				logger.Info("Sent player report successfully: " + json);
			}
		}));
	}

	public void MarkReportedPlayer(int reportedID)
	{
		foreach (RecentPlayedWithPlayer item in RecentlyPlayedWith)
		{
			if (item.clientId == reportedID)
			{
				item.isReported = true;
			}
		}
		currentReportedIDs.Add(reportedID);
		Ui.UpdatedReportedPlayers();
	}

	public void DenyFriendRequest(string puid, Action<ResponseState, Response<ResponseFriendsListRequest>> resultCallback)
	{
		string endpoint = BackendEndpoints.FriendDenyRequestEndpoint(puid);
		StartCoroutine(Delete(endpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendsListRequest> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendsListRequest>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Info("Failed to delete friend request: " + text);
			}
		}));
	}

	public void RemoveFriend(string recipientPuid, Action<ResponseState, Response<ResponseFriendsListRequest>> resultCallback)
	{
		string endpoint = BackendEndpoints.RemoveFriendEndpoint(recipientPuid);
		StartCoroutine(Delete(endpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendsListRequest> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendsListRequest>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to remove friend: " + text);
			}
			else
			{
				logger.Info("Removed friend successfully");
			}
		}));
	}

	public void BlockPlayer(string recipientPuid, Action<ResponseState, Response<ResponseFriendsListRequest>> resultCallback)
	{
		string blocksEndpoint = BackendEndpoints.BlocksEndpoint;
		string jsonContent = JsonConvert.SerializeObject(FriendsListRequest.SendRequest(recipientPuid, "block_request"));
		StartCoroutine(Post(blocksEndpoint, jsonContent, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendsListRequest> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendsListRequest>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to block player: " + text);
			}
			else
			{
				logger.Info("Blocked player successfully");
			}
		}));
	}

	public void UnblockPlayer(string recipientPuid, Action<ResponseState, Response<ResponseFriendsListRequest>> resultCallback)
	{
		string endpoint = BackendEndpoints.UnblockEndpoint(recipientPuid);
		StartCoroutine(Delete(endpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendsListRequest> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendsListRequest>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to unblock player: " + text);
			}
			else
			{
				logger.Info("Unblocked player successfully");
			}
		}));
	}

	public void GetBlockedPlayers(Action<ResponseState, Response<ResponseBlockedWrapper>> resultCallback)
	{
		string blocksEndpoint = BackendEndpoints.BlocksEndpoint;
		StartCoroutine(Get(blocksEndpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseBlockedWrapper> response2 = JsonConvert.DeserializeObject<Response<ResponseBlockedWrapper>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to retrieve blocked players: " + text);
			}
		}));
	}

	public void SendGameInvite(string recipientPuid, string roomCode, Action<ResponseState, Response<ResponseFriendsListRequest>> resultCallback)
	{
		if (HasPlayerBlockedMe(recipientPuid))
		{
			return;
		}
		string lobbyInviteEndpoint = BackendEndpoints.LobbyInviteEndpoint;
		string jsonContent = JsonConvert.SerializeObject(GameInviteRequest.SendGameInvite(recipientPuid, roomCode));
		StartCoroutine(Post(lobbyInviteEndpoint, jsonContent, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendsListRequest> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendsListRequest>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to send game invite: " + text);
			}
			else
			{
				logger.Info("Sent game invite successfully");
			}
		}));
	}

	public void GetLobbyInvites(Action<ResponseState, Response<ResponseGameInvites>> resultCallback)
	{
		string lobbyInviteEndpoint = BackendEndpoints.LobbyInviteEndpoint;
		StartCoroutine(Get(lobbyInviteEndpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseGameInvites> response2 = JsonConvert.DeserializeObject<Response<ResponseGameInvites>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to retrieve game invites: " + text);
			}
		}));
	}

	public void DenyLobbyInvite(string recipientPuid, Action<ResponseState, Response<ResponseFriendsListRequest>> resultCallback)
	{
		string endpoint = BackendEndpoints.LobbyDenyInviteEndpoint(recipientPuid);
		StartCoroutine(Delete(endpoint, (ResponseState cb, string response) =>
		{
			Response<ResponseFriendsListRequest> response2 = JsonConvert.DeserializeObject<Response<ResponseFriendsListRequest>>(response);
			resultCallback(cb, response2);
			if (cb != ResponseState.Success)
			{
				string text = "null";
				if (response2?.Errors != null)
				{
					text = string.Join("\r\n", response2.Errors.Select((ResponseError s) => s.ToString()));
				}
				logger.Error("Failed to delete lobby invite: " + text);
			}
		}));
	}

	// Offline remake: the friends service (BackendEndpoints.*) is Innersloth's live backend and is never contacted. Get/Delete/Post complete
	// locally with a failed response (same shape as the "no UserIdToken" failure), so callers show their normal error handling.
	private IEnumerator Get(string endpoint, Action<ResponseState, string> resultCallback)
	{
		logger.Info("FriendsListManager: offline, not sending GET friends request");
		resultCallback(ResponseState.Failed, Constants.UserIdTokenError);
		yield break;
	}

	private IEnumerator Delete(string endpoint, Action<ResponseState, string> resultCallback)
	{
		logger.Info("FriendsListManager: offline, not sending DELETE friends request");
		resultCallback(ResponseState.Failed, Constants.UserIdTokenError);
		yield break;
	}

	private IEnumerator Post(string endpoint, string jsonContent, Action<ResponseState, string> resultCallback)
	{
		logger.Info("FriendsListManager: offline, not sending POST friends request");
		resultCallback(ResponseState.Failed, Constants.UserIdTokenError);
		yield break;
	}

	public void SetFriendButtonColor(bool isGrayedOut)
	{
		FriendsListButton.SetGlyphColor(isGrayedOut);
	}

	private void OnAwake()
	{
		if (SteamManager.Initialized)
		{
			m_GameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(HandleGameInvite);
		}
	}

	private void HandleGameInvite(GameRichPresenceJoinRequested_t param)
	{
		if (SteamManager.Initialized)
		{
			int gameId = GameCode.GameNameToInt(param.m_rgchConnect);
			StartCoroutine(AmongUsClient.Instance.CoFindGameInfoFromCodeAndJoin(gameId));
			logger.Info("Got steam invite with code: " + param.m_rgchConnect);
		}
	}

	public IEnumerator PopulatePlatformFriendList()
	{
		yield return null;
		if (SteamManager.Initialized)
		{
			List<PlatformFriend> list = new List<PlatformFriend>();
			int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
			myId = SteamUser.GetSteamID();
			for (int i = 0; i < friendCount; i++)
			{
				CSteamID friendByIndex = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
				list.Add(new PlatformFriend(SteamFriends.GetFriendPersonaName(friendByIndex), friendByIndex));
			}
			list.OrderBy((PlatformFriend a) => a.Username).ToList();
			PlatformFriends = list;
		}
	}
}
