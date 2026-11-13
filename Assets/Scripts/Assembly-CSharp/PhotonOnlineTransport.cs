using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using ExitGames.Client.Photon;
using InnerNet;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public sealed class PhotonOnlineTransport : MonoBehaviourPunCallbacks, IOnEventCallback
{
	private const byte ReliablePacketEvent = 41;
	private const byte UnreliablePacketEvent = 42;
	private const string GuestIdPreferenceKey = "PhotonGuestUserId";
	private const string GuestChatMigrationKey = "PhotonGuestChatModeMigrated_";

	private static PhotonOnlineTransport instance;

	private Action<bool, string> roomResult;
	private Action pendingRoomOperation;
	private bool pendingLobbyJoin;
	private bool hasReceivedRoomList;
	private string guestUserId;
	private readonly Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

	public event Action<int, int, byte[], bool> PacketReceived;
	public event Action<Player> PlayerJoined;
	public event Action<Player> PlayerLeft;
	public event Action<Player> MasterClientChanged;
	public event Action<List<RoomInfo>> RoomListUpdated;

	public static bool IsInRoom => instance != null && PhotonNetwork.InRoom;
	public static bool IsInLobby => instance != null && PhotonNetwork.InLobby;
	public static bool IsConfigured => PhotonNetwork.PhotonServerSettings?.AppSettings != null && !string.IsNullOrWhiteSpace(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime);
	public int LocalActorNumber => PhotonNetwork.LocalPlayer?.ActorNumber ?? -1;
	public int MasterActorNumber => PhotonNetwork.MasterClient?.ActorNumber ?? -1;
	public Player[] Players => PhotonNetwork.PlayerList;
	public List<RoomInfo> Rooms => roomCache.Values.ToList();
	public bool HasReceivedRoomList => hasReceivedRoomList;
	public byte[] CurrentRoomSettings => PhotonNetwork.CurrentRoom?.CustomProperties["settings"] as byte[];
	public QuickChatModes? CurrentRoomChatMode
	{
		get
		{
			object value = PhotonNetwork.CurrentRoom?.CustomProperties["chat"];
			return value is byte chatMode ? (QuickChatModes?)chatMode : null;
		}
	}

	public static PhotonOnlineTransport Instance
	{
		get
		{
			EnsureInstance();
			return instance;
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void EnsureInstance()
	{
		if (instance != null)
		{
			return;
		}
		GameObject gameObject = new GameObject(nameof(PhotonOnlineTransport));
		DontDestroyOnLoad(gameObject);
		instance = gameObject.AddComponent<PhotonOnlineTransport>();
	}

	public bool CreateRoom(string roomName, Action<bool, string> callback)
	{
		return CreateRoom(roomName, null, callback);
	}

	public bool CreateRoom(string roomName, Hashtable roomProperties, Action<bool, string> callback)
	{
		if (string.IsNullOrWhiteSpace(roomName))
		{
			callback?.Invoke(false, "Room name is required.");
			return false;
		}
		return BeginRoomOperation(callback, () => PhotonNetwork.CreateRoom(roomName, new RoomOptions
		{
			MaxPlayers = 15,
			IsVisible = true,
			IsOpen = true,
			CustomRoomProperties = roomProperties,
			CustomRoomPropertiesForLobby = roomProperties?.Keys.Cast<string>().ToArray()
		}));
	}

	public bool JoinRoom(string roomName, Action<bool, string> callback)
	{
		if (string.IsNullOrWhiteSpace(roomName))
		{
			callback?.Invoke(false, "Room name is required.");
			return false;
		}
		return BeginRoomOperation(callback, () => PhotonNetwork.JoinRoom(roomName));
	}

	public bool JoinRandomRoom(Action<bool, string> callback)
	{
		return BeginRoomOperation(callback, () => PhotonNetwork.JoinRandomRoom());
	}

	public bool ConnectToLobby(Action<bool, string> callback)
	{
		if (PhotonNetwork.InLobby)
		{
			callback?.Invoke(true, string.Empty);
			return true;
		}
		pendingLobbyJoin = true;
		return BeginRoomOperation(callback, () => PhotonNetwork.JoinLobby());
	}

	public void SetPlayerName(string playerName)
	{
		PhotonNetwork.NickName = playerName;
	}

	public void PrepareGuestProfile()
	{
		EnsureGuestIdentity();
		string migrationKey = GuestChatMigrationKey + guestUserId;
		if (PlayerPrefs.GetInt(migrationKey, 0) != 0)
		{
			return;
		}
		if (DataManager.Settings.Multiplayer.ChatMode == QuickChatModes.QuickChatOnly)
		{
			DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
			DataManager.Settings.Save();
		}
		PlayerPrefs.SetInt(migrationKey, 1);
		PlayerPrefs.Save();
	}

	private void EnsureGuestIdentity()
	{
		if (string.IsNullOrEmpty(guestUserId))
		{
			guestUserId = PlayerPrefs.GetString(GuestIdPreferenceKey, string.Empty);
			if (string.IsNullOrEmpty(guestUserId))
			{
				guestUserId = Guid.NewGuid().ToString("N");
				PlayerPrefs.SetString(GuestIdPreferenceKey, guestUserId);
				PlayerPrefs.Save();
			}
		}
		PhotonNetwork.AuthValues = new AuthenticationValues(guestUserId);
	}

	public void LeaveRoom()
	{
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
	}

	public void CancelPendingRoomOperation(string reason)
	{
		if (roomResult == null)
		{
			return;
		}
		CompleteRoomOperation(false, reason);
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
	}

	private bool BeginRoomOperation(Action<bool, string> callback, Action operation)
	{
		if (roomResult != null || PhotonNetwork.InRoom)
		{
			callback?.Invoke(false, "A Photon room operation is already active.");
			return false;
		}

		roomResult = callback;
		pendingRoomOperation = operation;
		if (PhotonNetwork.IsConnectedAndReady)
		{
			RunPendingRoomOperation();
		}
		else if (!TryConnectUsingSettings())
		{
			CompleteRoomOperation(false, "Photon connection could not be started. Check PhotonServerSettings and its Realtime App ID.");
		}
		return true;
	}

	private bool TryConnectUsingSettings()
	{
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			PhotonNetwork.LoadOrCreateSettings();
		}
		if (PhotonNetwork.PhotonServerSettings?.AppSettings == null || string.IsNullOrWhiteSpace(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime))
		{
			return false;
		}
		EnsureGuestIdentity();
		return PhotonNetwork.ConnectUsingSettings();
	}

	private void RunPendingRoomOperation()
	{
		Action operation = pendingRoomOperation;
		pendingRoomOperation = null;
		operation?.Invoke();
	}

	private void CompleteRoomOperation(bool succeeded, string message)
	{
		Action<bool, string> callback = roomResult;
		roomResult = null;
		pendingRoomOperation = null;
		callback?.Invoke(succeeded, message);
	}

	public override void OnConnectedToMaster()
	{
		RunPendingRoomOperation();
	}

	public override void OnJoinedRoom()
	{
		pendingLobbyJoin = false;
		CompleteRoomOperation(true, PhotonNetwork.CurrentRoom.Name);
	}

	public override void OnJoinedLobby()
	{
		roomCache.Clear();
		hasReceivedRoomList = false;
		if (pendingLobbyJoin)
		{
			pendingLobbyJoin = false;
			CompleteRoomOperation(true, string.Empty);
		}
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		hasReceivedRoomList = true;
		foreach (RoomInfo room in roomList)
		{
			if (room.RemovedFromList)
			{
				roomCache.Remove(room.Name);
			}
			else
			{
				roomCache[room.Name] = room;
			}
		}
		RoomListUpdated?.Invoke(Rooms);
	}

	public RoomInfo FindRoom(string roomName)
	{
		roomCache.TryGetValue(roomName, out RoomInfo room);
		return room;
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		CompleteRoomOperation(false, message);
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		CompleteRoomOperation(false, message);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		CompleteRoomOperation(false, message);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		CompleteRoomOperation(false, cause.ToString());
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		PlayerJoined?.Invoke(newPlayer);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		PlayerLeft?.Invoke(otherPlayer);
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		MasterClientChanged?.Invoke(newMasterClient);
	}

	public bool SendPacket(byte[] packet, bool reliable)
	{
		if (packet == null || !PhotonNetwork.InRoom)
		{
			return false;
		}

		byte eventCode = reliable ? ReliablePacketEvent : UnreliablePacketEvent;
		object[] payload = { PhotonNetwork.LocalPlayer.ActorNumber, packet };
		RaiseEventOptions options = new RaiseEventOptions
		{
			Receivers = PhotonNetwork.IsMasterClient ? ReceiverGroup.Others : ReceiverGroup.MasterClient
		};
		SendOptions sendOptions = reliable ? SendOptions.SendReliable : SendOptions.SendUnreliable;
		return PhotonNetwork.RaiseEvent(eventCode, payload, options, sendOptions);
	}

	public bool BroadcastPacket(byte[] packet, bool reliable, int excludedActorNumber)
	{
		if (packet == null || !PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient)
		{
			return false;
		}

		int[] targetActors = PhotonNetwork.PlayerList
			.Where(player => player.ActorNumber != excludedActorNumber && player.ActorNumber != LocalActorNumber)
			.Select(player => player.ActorNumber)
			.ToArray();
		if (targetActors.Length == 0)
		{
			return true;
		}

		byte eventCode = reliable ? ReliablePacketEvent : UnreliablePacketEvent;
		object[] payload = { LocalActorNumber, packet };
		return PhotonNetwork.RaiseEvent(eventCode, payload, new RaiseEventOptions
		{
			TargetActors = targetActors
		}, reliable ? SendOptions.SendReliable : SendOptions.SendUnreliable);
	}

	public void OnEvent(EventData photonEvent)
	{
		bool reliable = photonEvent.Code == ReliablePacketEvent;
		if (!reliable && photonEvent.Code != UnreliablePacketEvent)
		{
			return;
		}
		if (!(photonEvent.CustomData is object[] payload) || payload.Length != 2 || !(payload[1] is byte[] packet))
		{
			return;
		}

		int senderActorNumber = PhotonNetwork.IsMasterClient && photonEvent.Sender != PhotonNetwork.LocalPlayer.ActorNumber
			? photonEvent.Sender
			: (int)payload[0];
		PacketReceived?.Invoke(photonEvent.Sender, senderActorNumber, packet, reliable);
	}
}