using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using AmongUs.Data;
using AmongUs.GameOptions;
using AmongUs.InnerNet.GameDataMessages;
using Hazel;
using Hazel.Udp;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace InnerNet
{
	public abstract class InnerNetClient : MonoBehaviour
	{
		public enum GameStates
		{
			NotJoined,
			Joined,
			Started,
			Ended
		}

		public class TotalGameData
		{
			public int[] PerMapTotals;
		}

		private class RecentPlayerComparer : IComparer<ClientData>
		{
			public static readonly RecentPlayerComparer Instance = new RecentPlayerComparer();

			public int Compare(ClientData x, ClientData y)
			{
				if (!AmongUsClient.Instance)
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				int num = ((AmongUsClient.Instance.GetClient(x.Id) != null) ? 1 : 0);
				int num2 = ((AmongUsClient.Instance.GetClient(y.Id) != null) ? 1 : 0);
				if (num != num2)
				{
					return num - num2;
				}
				return y.PlayerName.CompareTo(x.PlayerName);
			}
		}

		private static readonly HashSet<DisconnectReasons> DontBotherLoggingTheseDisconnectReasons = new HashSet<DisconnectReasons>
		{
			DisconnectReasons.ExitGame,
			DisconnectReasons.GameFull,
			DisconnectReasons.GameStarted,
			DisconnectReasons.GameNotFound,
			DisconnectReasons.ConnectionLimit,
			DisconnectReasons.QuickchatLock
		};

		public const int MaxRecentClients = 20;

		private const int DefaultSecondsSuspendedBeforeDisconnect = 30;

		public static int SecondsSuspendedBeforeDisconnect = 30;

		public const int NoClientId = -1;

		private readonly Logger serverLogger = new Logger(Logger.Category.Server);

		private string networkAddress = "127.0.0.1";

		private int networkPort;

		private bool useDtls;

		private UnityUdpClientConnection connection;

		public MatchMakerModes mode;

		public NetworkModes NetworkMode;

		public int GameId = 32;

		public int HostId;

		public int ClientId = -1;

		protected List<ClientData> allClients = new List<ClientData>();

		protected CircleBuffer<ClientData> recentClients = new CircleBuffer<ClientData>(20);

		public DisconnectReasons LastDisconnectReason;

		public string LastCustomDisconnect;

		public byte LastServerChatMode;

		public HttpMatchmakerManager.MatchmakerError LastMatchmakerError;

		private readonly List<Action> PreSpawnDispatcher = new List<Action>();

		private readonly List<Action> Dispatcher = new List<Action>();

		private readonly List<SpawnGameDataMessage> sendInitialDataSpawnGameDataMessages = new List<SpawnGameDataMessage>();

		private readonly GameOptionsFactory gameOptionsFactory = new GameOptionsFactory(new UnityLogger());

		public GameStates GameState;

		private bool isConnecting;

		private bool platformSpecificsChecked;

		private List<Action> TempQueue = new List<Action>();

		private volatile bool appPaused;

		public const int BroadcastTargetClientId = -1;

		public const int CurrentClient = -3;

		public const int HostInherit = -2;

		public const int ServerOwned = -4;

		public const int MinServerID = 100000;

		private const int MaxMessageSizeBytes = 500;

		public float MinSendInterval = 0.1f;

		private uint NetIdCnt = 1u;

		private float timer;

		public AssetReference[] SpawnableObjects;

		public InnerNetObject[] NonAddressableSpawnableObjects;

		protected InnerNetObjectCollection allObjects = new InnerNetObjectCollection();

		private bool InOnlineScene;

		private HashSet<uint> DestroyedObjects = new HashSet<uint>();

		private Queue<IGameDataMessage> reliableMessageQueue = new Queue<IGameDataMessage>();

		private Queue<IGameDataMessage> unreliableMessageQueue = new Queue<IGameDataMessage>();

		private int numberOfConsecutiveFramesWithReliableMessages;

		private int numberOfConsecutiveFramesWithUnreliableMessages;

		private int indexForReliableStreamedUpdates;

		private int indexForUnreliableStreamedUpdates;

		private int msgNum;

		public bool AmConnected => connection != null || (NetworkMode == NetworkModes.OnlineGame && PhotonOnlineTransport.IsInRoom);

		public int Ping
		{
			get
			{
				if (connection == null)
				{
					return 0;
				}
				return (int)connection.AveragePingMs;
			}
		}

		public int BytesSent
		{
			get
			{
				if (connection == null)
				{
					return 0;
				}
				return (int)connection.Statistics.TotalBytesSent;
			}
		}

		public int BytesGot
		{
			get
			{
				if (connection == null)
				{
					return 0;
				}
				return (int)connection.Statistics.TotalBytesReceived;
			}
		}

		public int Resends
		{
			get
			{
				if (connection == null)
				{
					return 0;
				}
				return connection.Statistics.MessagesResent;
			}
		}

		public bool AmHost => HostId == ClientId;

		public bool AmLocalHost
		{
			get
			{
				if (!AmHost || AmongUsClient.Instance.NetworkMode != NetworkModes.LocalGame)
				{
					return DestroyableSingleton<TutorialManager>.InstanceExists;
				}
				return true;
			}
		}

		public bool AmModdedHost
		{
			get
			{
				if (AmHost)
				{
					return Constants.IsVersionModded();
				}
				return false;
			}
		}

		public bool AmClient => ClientId > 0;

		public bool IsGamePublic { get; private set; }

		public bool IsGameStarted => GameState == GameStates.Started;

		public bool IsGameOver => GameState == GameStates.Ended;

		public bool IsInGame => GameState != GameStates.NotJoined;

		public void SetEndpoint(string addr, ushort port, bool dtls)
		{
			networkAddress = addr;
			networkPort = port;
			useDtls = dtls;
		}

		public string GetNetworkAddress()
		{
			return networkAddress;
		}

		public int GetNetworkPort()
		{
			return networkPort;
		}

		public virtual void Start()
		{
			SceneManager.activeSceneChanged += (Scene oldScene, Scene scene) =>
			{
				SendSceneChange(scene.name);
			};
			ClientId = -1;
			GameId = 32;
		}

		private void SendOrDisconnect(MessageWriter msg)
		{
			if (NetworkMode == NetworkModes.OnlineGame)
			{
				if (!PhotonOnlineTransport.Instance.SendPacket(msg.ToByteArray(includeHeader: true), msg.SendOption == SendOption.Reliable))
				{
					EnqueueDisconnect(DisconnectReasons.Error, "Failed to send Photon packet.");
				}
				return;
			}
			SendErrors sendErrors = connection.Send(msg);
			if (sendErrors != SendErrors.None)
			{
				Debug.Log("Failed to send message: " + sendErrors);
				EnqueueDisconnect(DisconnectReasons.Error, "Failed to send message: " + sendErrors);
			}
		}

		public ClientData GetHost()
		{
			lock (allClients)
			{
				for (int i = 0; i < allClients.Count; i++)
				{
					ClientData clientData = allClients[i];
					if (clientData.Id == HostId)
					{
						return clientData;
					}
				}
			}
			return null;
		}

		public ClientData GetClientFromCharacter(PlayerControl character)
		{
			if (!character)
			{
				return null;
			}
			lock (allClients)
			{
				for (int i = 0; i < allClients.Count; i++)
				{
					ClientData clientData = allClients[i];
					if (clientData.Character == character)
					{
						return clientData;
					}
				}
			}
			return null;
		}

		public ClientData GetClientFromPlayerInfo(NetworkedPlayerInfo playerInfo)
		{
			if (playerInfo == null)
			{
				return null;
			}
			lock (allClients)
			{
				for (int i = 0; i < allClients.Count; i++)
				{
					ClientData clientData = allClients[i];
					if (clientData.Id == playerInfo.ClientId)
					{
						return clientData;
					}
				}
			}
			return null;
		}

		public int GetClientIdFromCharacter(PlayerControl character)
		{
			return GetClientFromCharacter(character)?.Id ?? (-1);
		}

		public virtual void OnDestroy()
		{
			if (AmConnected)
			{
				DisconnectInternal(DisconnectReasons.Destroy);
			}
		}

		public IEnumerator CoConnect(string matchmakerToken)
		{
			if (AmConnected)
			{
				yield break;
			}
			while (true)
			{
				string ipAddr = networkAddress;
				DestroyableSingleton<DisconnectPopup>.Instance.Close();
				LastDisconnectReason = DisconnectReasons.Unknown;
				NetIdCnt = 1u;
				DestroyedObjects.Clear();
				if (DestroyableSingleton<EOSManager>.Instance.ProductUserId == null)
				{
					EnqueueDisconnect(DisconnectReasons.NotAuthorized);
					break;
				}
				if (useDtls && NetworkMode == NetworkModes.OnlineGame)
				{
					connection = AuthManager.CreateDtlsConnection(ipAddr, (ushort)(networkPort + 3));
				}
				else
				{
					if (NetworkMode == NetworkModes.OnlineGame)
					{
						yield return DestroyableSingleton<AuthManager>.Instance.CoConnect(ipAddr, (ushort)(networkPort + 2), matchmakerToken);
						yield return DestroyableSingleton<AuthManager>.Instance.CoWaitForNonce();
					}
					IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddr), networkPort);
					connection = new UnityUdpClientConnection(new UnityLogger(), remoteEndPoint);
				}
				connection.KeepAliveInterval = 1000;
				connection.DisconnectTimeoutMs = 7500;
				connection.ResendPingMultiplier = 1.2f;
				connection.DataReceived += OnMessageReceived;
				connection.Disconnected += OnDisconnect;
				lock (Dispatcher)
				{
					Dispatcher.Clear();
				}
				connection.ConnectAsync(GetConnectionData(useDtls, matchmakerToken));
				while (connection != null && connection.State == ConnectionState.Connecting)
				{
					yield return null;
				}
				if ((connection == null || connection.State != ConnectionState.Connected) && LastDisconnectReason != DisconnectReasons.IncorrectVersion && LastDisconnectReason != DisconnectReasons.InvalidName)
				{
					Debug.Log($"Failed to connected to: {ipAddr}:{networkPort}");
					if (DestroyableSingleton<ServerManager>.Instance.TrackServerFailure(ipAddr))
					{
						DisconnectInternal(DisconnectReasons.NewConnection);
						continue;
					}
					break;
				}
				break;
			}
		}

		private void Connection_DataReceivedRaw(byte[] data)
		{
			Debug.Log("Client Got: " + string.Join(" ", data.Select((byte b) => b.ToString())));
		}

		private void Connection_DataSentRaw(byte[] data, int length)
		{
			Debug.Log("Client Sent: " + string.Join(" ", data.Select((byte b) => b.ToString()).ToArray(), 0, length));
		}

		public void Connect(MatchMakerModes mode, string matchmakerToken)
		{
			StartCoroutine(CoConnect(mode, matchmakerToken));
		}

		private IEnumerator CoConnect(MatchMakerModes mode, string matchmakerToken)
		{
			if (this.mode != MatchMakerModes.None)
			{
				DisconnectInternal(DisconnectReasons.NewConnection);
			}
			this.mode = mode;
			isConnecting = true;
			yield return CoConnect(matchmakerToken);
			if (connection == null || connection.State != ConnectionState.Connected)
			{
				HandleDisconnect(DisconnectReasons.Error);
				isConnecting = false;
				yield break;
			}
			switch (this.mode)
			{
			case MatchMakerModes.Client:
				JoinGame();
				yield return WaitWithTimeout(() => ClientId >= 0, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PSNErrorSessionJoinFailed));
				isConnecting = false;
				_ = AmConnected;
				break;
			case MatchMakerModes.HostAndClient:
				GameId = 0;
				GameOptionsManager.Instance.CurrentGameOptions = GameOptionsManager.Instance.GameHostOptions;
				HostGame(GameOptionsManager.Instance.CurrentGameOptions, DataManager.Settings.Multiplayer.HostGameFilterOptions);
				yield return WaitWithTimeout(() => GameId != 0, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorFailedToCreateGame));
				if (!AmConnected)
				{
					isConnecting = false;
					break;
				}
				JoinGame();
				yield return WaitWithTimeout(() => ClientId >= 0, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorFailedToJoinCreatedGame));
				isConnecting = false;
				_ = AmConnected;
				break;
			}
		}

		public IEnumerator WaitForConnectionOrFail()
		{
			while (true)
			{
				if (!AmConnected)
				{
					if (!isConnecting)
					{
						break;
					}
					yield return null;
					continue;
				}
				switch (mode)
				{
				default:
					yield break;
				case MatchMakerModes.Client:
					if (ClientId >= 0)
					{
						yield break;
					}
					break;
				case MatchMakerModes.HostAndClient:
					if (GameId != 0 && ClientId >= 0)
					{
						yield break;
					}
					break;
				}
				yield return null;
			}
		}

		private IEnumerator WaitWithTimeout(Func<bool> success, string errorMessage, int durationSeconds = 15)
		{
			bool failed = true;
			for (float timer = 0f; timer < (float)durationSeconds; timer += Time.deltaTime)
			{
				if (success())
				{
					failed = false;
					break;
				}
				if (!AmConnected)
				{
					yield break;
				}
				yield return null;
			}
			if (failed && errorMessage != null)
			{
				LastCustomDisconnect = errorMessage;
				EnqueueDisconnect(DisconnectReasons.Custom, "Couldn't connect");
			}
		}

		public virtual void Update()
		{
			if (Input.GetKeyDown(KeyCode.Return) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
			{
				ResolutionManager.ToggleFullscreen();
			}
			TempQueue.Clear();
			lock (Dispatcher)
			{
				TempQueue.AddAll(Dispatcher);
				Dispatcher.Clear();
			}
			for (int i = 0; i < TempQueue.Count; i++)
			{
				Action action = TempQueue[i];
				try
				{
					action();
				}
				catch (Exception exception)
				{
					Debug.LogError("InnerNetClient::Update Exception 1");
					Debug.LogException(exception, this);
				}
			}
			if (!InOnlineScene)
			{
				return;
			}
			TempQueue.Clear();
			lock (PreSpawnDispatcher)
			{
				TempQueue.AddAll(PreSpawnDispatcher);
				PreSpawnDispatcher.Clear();
			}
			for (int j = 0; j < TempQueue.Count; j++)
			{
				Action action2 = TempQueue[j];
				try
				{
					action2();
				}
				catch (Exception exception2)
				{
					Debug.LogError("InnerNetClient::Update Exception 2");
					Debug.LogException(exception2, this);
				}
			}
		}

		private void OnDisconnect(object sender, DisconnectedEventArgs e)
		{
			MessageReader message = e.Message;
			if (message != null && message.Position < message.Length)
			{
				if (message.ReadByte() == 1)
				{
					MessageReader messageReader = message.ReadMessage();
					DisconnectReasons disconnectReasons = (DisconnectReasons)messageReader.ReadByte();
					switch (disconnectReasons)
					{
					case DisconnectReasons.Custom:
					case DisconnectReasons.MismatchedVersion:
						LastCustomDisconnect = messageReader.ReadString();
						if (string.IsNullOrWhiteSpace(LastCustomDisconnect))
						{
							LastCustomDisconnect = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorDisconnectPacket);
						}
						break;
					case DisconnectReasons.QuickchatLock:
						LastServerChatMode = messageReader.ReadByte();
						break;
					}
					EnqueueDisconnect(disconnectReasons, LastCustomDisconnect);
				}
				else
				{
					string text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorDisconnectPacket);
					LastCustomDisconnect = text + "\r\n\r\n" + (e.Reason ?? "Null");
					EnqueueDisconnect(DisconnectReasons.Custom, LastCustomDisconnect);
				}
			}
			else if (e.Reason == null)
			{
				string text2 = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorDisconnectPacket);
				LastCustomDisconnect = text2 + "\r\n\r\nNull";
				EnqueueDisconnect(DisconnectReasons.Custom, LastCustomDisconnect);
			}
			else if (!e.Reason.Contains("The remote sent a"))
			{
				string text3 = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorDisconnectPacket);
				LastCustomDisconnect = text3 + "\r\n\r\n" + e.Reason;
				EnqueueDisconnect(DisconnectReasons.Custom, LastCustomDisconnect);
			}
			else
			{
				EnqueueDisconnect(DisconnectReasons.Error, e.Reason);
			}
		}

		public void HandleDisconnect(DisconnectReasons reason, string stringReason = null)
		{
			if (AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay)
			{
				isConnecting = false;
				StopAllCoroutines();
				PreDisconnectInternal();
				DisconnectInternal(reason, stringReason);
				OnDisconnected();
				GameId = -1;
			}
		}

		public void EnqueueDisconnect(DisconnectReasons reason, string stringReason = null)
		{
			_ = connection;
			lock (Dispatcher)
			{
				Dispatcher.Add(() =>
				{
					HandleDisconnect(reason, stringReason);
				});
			}
		}

		protected void DisconnectInternal(DisconnectReasons reason, string stringReason = null)
		{
			bool leavePhotonRoom = NetworkMode == NetworkModes.OnlineGame && PhotonOnlineTransport.IsInRoom;
			if (!DontBotherLoggingTheseDisconnectReasons.Contains(reason))
			{
				serverLogger.Error(string.Format("Client DC because {0}: {1}", reason, stringReason ?? "null"));
			}
			if (reason != DisconnectReasons.NewConnection && reason != DisconnectReasons.FocusLostBackground)
			{
				LastDisconnectReason = reason;
				if (reason != DisconnectReasons.ExitGame && reason != DisconnectReasons.Unknown && DestroyableSingleton<DisconnectPopup>.InstanceExists)
				{
					DestroyableSingleton<DisconnectPopup>.Instance.Show();
				}
			}
			if (reason != DisconnectReasons.NewConnection)
			{
				if (mode == MatchMakerModes.HostAndClient)
				{
					GameId = 0;
				}
				if (mode == MatchMakerModes.Client || mode == MatchMakerModes.HostAndClient)
				{
					ClientId = -1;
				}
			}
			mode = MatchMakerModes.None;
			GameState = GameStates.NotJoined;
			if (leavePhotonRoom)
			{
				PhotonOnlineTransport.Instance.LeaveRoom();
			}
			UnityUdpClientConnection unityUdpClientConnection = Interlocked.Exchange(ref connection, null);
			if (unityUdpClientConnection != null)
			{
				try
				{
					unityUdpClientConnection.Dispose();
				}
				catch (Exception exception)
				{
					Debug.LogError("InnerNetClient::DisconnectInternal Exception");
					Debug.LogException(exception, this);
				}
			}
			if (DestroyableSingleton<InnerNetServer>.InstanceExists)
			{
				DestroyableSingleton<InnerNetServer>.Instance.StopServer();
			}
			lock (Dispatcher)
			{
				Dispatcher.Clear();
			}
			lock (PreSpawnDispatcher)
			{
				PreSpawnDispatcher.Clear();
			}
			if (reason != DisconnectReasons.Error)
			{
				TempQueue.Clear();
			}
			lock (allObjects)
			{
				allObjects.Clear();
			}
			allClients.Clear();
			recentClients.Clear();
			platformSpecificsChecked = false;
			GameManager.DestroyInstance();
			GameData.OnDisconnected();
		}

		public void HostGame(IGameOptions settings, GameFilterOptions filterOpts)
		{
			bool flag = CurrentModRegistration.TryGetModRegistrationGuid(out var guid);
			byte typeFlag = (byte)(flag ? 25 : 0);
			IsGamePublic = false;
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(typeFlag);
			messageWriter.WriteBytesAndSize(gameOptionsFactory.ToBytes(settings, AprilFoolsMode.IsAprilFoolsModeToggledOn));
			messageWriter.Write(CrossplayMode.GetCrossplayFlags());
			filterOpts.Serialize(messageWriter);
			if (flag)
			{
				messageWriter.Write(guid.ToByteArray());
			}
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
			Debug.Log("Client requesting new game.");
		}

		public void ReportPlayer(int clientId, ReportReasons reason)
		{
			ClientData recentClient = GetRecentClient(clientId);
			if (recentClient != null)
			{
				recentClient.HasBeenReported = true;
			}
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(17);
			messageWriter.Write(GameId);
			messageWriter.WritePacked(clientId);
			messageWriter.Write((byte)reason);
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		public void JoinGame()
		{
			ClientId = -1;
			if (!AmConnected)
			{
				LastCustomDisconnect = "Disconnected before joining game.";
				HandleDisconnect(DisconnectReasons.Custom);
				return;
			}
			reliableMessageQueue.Clear();
			unreliableMessageQueue.Clear();
			Debug.Log($"Client joining game: {GameId}/{GameCode.IntToGameName(GameId)} useDtls: {useDtls}");
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(1);
			messageWriter.Write(GameId);
			messageWriter.Write(!DataManager.Settings.Multiplayer.CrossPlay);
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		public bool CanBan()
		{
			if (AmHost)
			{
				return !IsGameStarted;
			}
			return false;
		}

		public bool CanKick()
		{
			if (!IsGameStarted)
			{
				return AmHost;
			}
			return true;
		}

		public void KickPlayer(int clientId, bool ban)
		{
			if (AmHost)
			{
				MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
				messageWriter.StartMessage(11);
				messageWriter.Write(GameId);
				messageWriter.WritePacked(clientId);
				messageWriter.Write(ban);
				messageWriter.EndMessage();
				SendOrDisconnect(messageWriter);
				messageWriter.Recycle();
			}
		}

		public MessageWriter StartEndGame()
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(8);
			messageWriter.Write(GameId);
			return messageWriter;
		}

		public void FinishEndGame(MessageWriter msg)
		{
			msg.EndMessage();
			SendOrDisconnect(msg);
			msg.Recycle();
		}

		protected void SendLateRejection(int targetId, DisconnectReasons reason)
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(4);
			messageWriter.Write(GameId);
			messageWriter.WritePacked(targetId);
			messageWriter.Write((byte)reason);
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		protected void SendClientReady()
		{
			ClientData clientData = FindClientById(ClientId);
			if (clientData == null)
			{
				HandleDisconnect(DisconnectReasons.Error, "Couldn't find self");
				return;
			}
			clientData.IsReady = true;
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(5);
			messageWriter.Write(GameId);
			messageWriter.StartMessage(7);
			messageWriter.WritePacked(ClientId);
			messageWriter.EndMessage();
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		protected void SendStartGame()
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(2);
			messageWriter.Write(GameId);
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		public void RequestGameList(IGameOptions settings, GameFilterOptions filterOpts)
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(16);
			messageWriter.WritePacked(2);
			messageWriter.WriteBytesAndSize(gameOptionsFactory.ToBytes(settings, AprilFoolsMode.IsAprilFoolsModeToggledOn));
			messageWriter.Write((byte)DataManager.Settings.Multiplayer.ChatMode);
			messageWriter.Write(CrossplayMode.GetCrossplayFlags());
			filterOpts.Serialize(messageWriter);
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		public void ChangeGamePublic(bool isPublic)
		{
			if (AmHost)
			{
				MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
				messageWriter.StartMessage(10);
				messageWriter.Write(GameId);
				messageWriter.Write((byte)1);
				messageWriter.Write(isPublic);
				messageWriter.EndMessage();
				SendOrDisconnect(messageWriter);
				messageWriter.Recycle();
				IsGamePublic = isPublic;
			}
		}

		public void SetGameSession(string session)
		{
		}

		public void SetActivePodType(string podType)
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(21);
			messageWriter.Write(podType ?? "");
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		public void QueryPlatformIds()
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(22);
			messageWriter.Write(GameId);
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		private void OnQueryPlatformData(List<PlatformSpecificData> playerData)
		{
			platformSpecificsChecked = true;
			JoinGame();
		}

		private void OnMessageReceived(DataReceivedEventArgs e)
		{
			MessageReader message = e.Message;
			try
			{
				while (message.Position < message.Length)
				{
					HandleMessage(message.ReadMessage(), e.SendOption);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
			finally
			{
				message.Recycle();
			}
		}

		protected void OnPhotonPacketReceived(int transportSenderId, int sourceActorId, byte[] packet, bool reliable)
		{
			if (NetworkMode != NetworkModes.OnlineGame || packet == null)
			{
				return;
			}
			bool isMaster = PhotonOnlineTransport.Instance.MasterActorNumber == ClientId;
			if (isMaster && transportSenderId == ClientId)
			{
				return;
			}
			if (!isMaster && transportSenderId != HostId)
			{
				return;
			}

			MessageReader message = MessageReader.Get(packet);
			bool shouldRelay = false;
			try
			{
				while (message.Position < message.Length)
				{
					MessageReader child = message.ReadMessage();
					if (isMaster && child.Tag != 5 && child.Tag != 6)
					{
						continue;
					}
					if (isMaster)
					{
						int packetGameId = child.ReadInt32();
						child.Position = 0;
						if (packetGameId != GameId)
						{
							continue;
						}
						shouldRelay = true;
					}
					HandleMessage(child, reliable ? SendOption.Reliable : SendOption.None);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception, this);
			}
			finally
			{
				message.Recycle();
			}

			if (shouldRelay)
			{
				PhotonOnlineTransport.Instance.BroadcastPacket(packet, reliable, sourceActorId);
			}
		}

		private void HandleMessage(MessageReader reader, SendOption sendOption)
		{
			switch (reader.Tag)
			{
			case 17:
			{
				int clientId = reader.ReadPackedInt32();
				ReportReasons reason = (ReportReasons)reader.ReadInt32();
				ReportOutcome outcome = (ReportOutcome)reader.ReadByte();
				string playerName = reader.ReadString();
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						OnReportedPlayer(outcome, clientId, playerName, reason);
					});
					break;
				}
			}
			case 0:
				GameId = reader.ReadInt32();
				Debug.Log("Client hosting game: " + GameCode.IntToGameName(GameId));
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						OnGameCreated(GameCode.IntToGameName(GameId));
					});
					break;
				}
			case 4:
			{
				int num12 = reader.ReadInt32();
				if (GameId != num12)
				{
					break;
				}
				int playerIdThatLeft = reader.ReadInt32();
				int hostId2 = reader.ReadInt32();
				DisconnectReasons reason2 = (DisconnectReasons)reader.ReadByte();
				if (!AmHost)
				{
					HostId = hostId2;
					if (AmHost)
					{
						lock (Dispatcher)
						{
							Dispatcher.Add(() =>
							{
								OnBecomeHost();
							});
						}
					}
				}
				RemovePlayer(playerIdThatLeft, reason2);
				break;
			}
			case 8:
			{
				int num15 = reader.ReadInt32();
				if (GameId != num15 || GameState == GameStates.Ended)
				{
					break;
				}
				GameState = GameStates.Ended;
				lock (allClients)
				{
					allClients.Clear();
				}
				EndGameResult endGameResult = EndGameResult.Create(reader);
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						OnGameEnd(endGameResult);
					});
					break;
				}
			}
			case 22:
			{
				int num9 = reader.ReadInt32();
				if (GameId != num9)
				{
					break;
				}
				List<PlatformSpecificData> received = new List<PlatformSpecificData>();
				while (reader.Position < reader.Length)
				{
					PlatformSpecificData platformSpecificData2 = new PlatformSpecificData();
					platformSpecificData2.Deserialize(reader.ReadMessage());
					received.Add(platformSpecificData2);
				}
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						OnQueryPlatformData(received);
					});
					break;
				}
			}
			case 12:
			{
				int num16 = reader.ReadInt32();
				if (GameId != num16)
				{
					break;
				}
				ClientId = reader.ReadInt32();
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						OnWaitForHost(GameCode.IntToGameName(GameId));
					});
					break;
				}
			}
			case 7:
			{
				int num5 = reader.ReadInt32();
				if (GameId != num5 || GameState == GameStates.Joined)
				{
					break;
				}
				GameState = GameStates.Joined;
				ClientId = reader.ReadInt32();
				GetOrCreateClient(new ClientData(ClientId, DataManager.Player.Customization.Name, Constants.GetPlatformData(), DataManager.Player.Stats.Level, DestroyableSingleton<EOSManager>.Instance.ProductUserId, DestroyableSingleton<EOSManager>.Instance.FriendCode));
				HostId = reader.ReadInt32();
				int num6 = reader.ReadPackedInt32();
				for (int num7 = 0; num7 < num6; num7++)
				{
					int id = reader.ReadPackedInt32();
					string playerName2 = reader.ReadString();
					PlatformSpecificData platformSpecificData = new PlatformSpecificData();
					platformSpecificData.Deserialize(reader.ReadMessage());
					uint playerLevel = reader.ReadPackedUInt32();
					string productUserId = reader.ReadString();
					string friendCode = reader.ReadString();
					GetOrCreateClient(new ClientData(id, playerName2, platformSpecificData, playerLevel, productUserId, friendCode));
				}
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						OnGameJoined(GameCode.IntToGameName(GameId));
					});
					break;
				}
			}
			case 1:
			{
				int num13 = reader.ReadInt32();
				if (GameId == num13)
				{
					int num14 = reader.ReadInt32();
					bool amHost = AmHost;
					HostId = reader.ReadInt32();
					string playerName3 = reader.ReadString();
					PlatformSpecificData platformSpecificData3 = new PlatformSpecificData();
					platformSpecificData3.Deserialize(reader.ReadMessage());
					uint playerLevel2 = reader.ReadPackedUInt32();
					string productUserId2 = reader.ReadString();
					string friendCode2 = reader.ReadString();
					ClientData client = GetOrCreateClient(new ClientData(num14, playerName3, platformSpecificData3, playerLevel2, productUserId2, friendCode2));
					Debug.Log($"Player {num14} joined");
					lock (Dispatcher)
					{
						Dispatcher.Add(() =>
						{
							OnPlayerJoined(client);
						});
					}
					if (!AmHost || amHost)
					{
						break;
					}
					lock (Dispatcher)
					{
						Dispatcher.Add(() =>
						{
							OnBecomeHost();
						});
						break;
					}
				}
				EnqueueDisconnect(DisconnectReasons.IncorrectGame);
				break;
			}
			case 5:
			case 6:
			{
				int num10 = reader.ReadInt32();
				if (GameId != num10)
				{
					break;
				}
				if (reader.Tag == 6)
				{
					int num11 = reader.ReadPackedInt32();
					if (ClientId != num11)
					{
						Debug.LogError($"Got data meant for {num11}");
						break;
					}
				}
				if (InOnlineScene)
				{
					MessageReader subReader = MessageReader.Get(reader);
					lock (Dispatcher)
					{
						Dispatcher.Add(() =>
						{
							HandleGameData(subReader);
						});
						break;
					}
				}
				if (sendOption == SendOption.None)
				{
					break;
				}
				MessageReader subReader2 = MessageReader.Get(reader);
				lock (PreSpawnDispatcher)
				{
					PreSpawnDispatcher.Add(() =>
					{
						HandleGameData(subReader2);
					});
					break;
				}
			}
			case 16:
			{
				TotalGameData totals = new TotalGameData();
				List<GameListing> list = new List<GameListing>();
				while (reader.Position < reader.Length)
				{
					MessageReader messageReader = reader.ReadMessage();
					switch (messageReader.Tag)
					{
					case 0:
						while (messageReader.Position < messageReader.Length)
						{
							GameListing item = GameListing.DeserializeV2(messageReader.ReadMessage());
							list.Add(item);
						}
						break;
					case 1:
					{
						totals.PerMapTotals = new int[3];
						for (int num8 = 0; num8 < totals.PerMapTotals.Length; num8++)
						{
							totals.PerMapTotals[num8] = messageReader.ReadInt32();
						}
						break;
					}
					}
				}
				HttpMatchmakerManager.FindGamesListFilteredResponse response = new HttpMatchmakerManager.FindGamesListFilteredResponse
				{
					Games = list,
					Metadata = new HttpMatchmakerManager.GamesListMetadata
					{
						AllGamesCount = list.Count,
						MatchingGamesCount = list.Count
					}
				};
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						OnGetGameList(totals, response);
					});
					break;
				}
			}
			case 10:
			{
				int num2 = reader.ReadInt32();
				if (GameId == num2)
				{
					if (reader.ReadByte() == 1)
					{
						IsGamePublic = reader.ReadBoolean();
						Debug.Log("Alter Public = " + IsGamePublic);
					}
					else
					{
						Debug.Log("Alter unknown");
					}
				}
				break;
			}
			case 3:
			{
				DisconnectReasons reason3 = DisconnectReasons.ServerRequest;
				if (reader.Position < reader.Length)
				{
					reason3 = (DisconnectReasons)reader.ReadByte();
				}
				EnqueueDisconnect(reason3);
				break;
			}
			case 2:
				GameState = GameStates.Started;
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						StartCoroutine(CoStartGame());
					});
					break;
				}
			case 11:
			{
				int num4 = reader.ReadInt32();
				if (GameId == num4 && reader.ReadPackedInt32() == ClientId)
				{
					bool flag = reader.ReadBoolean();
					if (reader.Position < reader.Length)
					{
						EnqueueDisconnect((DisconnectReasons)reader.ReadByte());
					}
					else
					{
						EnqueueDisconnect(flag ? DisconnectReasons.Banned : DisconnectReasons.Kicked);
					}
				}
				break;
			}
			case 13:
			{
				uint address = reader.ReadUInt32();
				ushort port = reader.ReadUInt16();
				lock (Dispatcher)
				{
					Dispatcher.Add(() =>
					{
						AmongUsClient.Instance.SetEndpoint(AddressToString(address), port, DestroyableSingleton<ServerManager>.Instance.UdpUseDtls);
						Debug.Log($"Redirected to: {networkAddress}:{networkPort} {DestroyableSingleton<ServerManager>.Instance.UdpUseDtls}");
						StopAllCoroutines();
						Connect(mode, null);
					});
					break;
				}
			}
			case byte.MaxValue:
			{
				int num3 = reader.ReadInt32();
				string arg = reader.ReadString();
				serverLogger.Debug($"v{num3}: {arg}");
				break;
			}
			case 24:
			{
				int num = reader.ReadInt32();
				if (GameId == num)
				{
					int hostId = reader.ReadInt32();
					HostId = hostId;
					if (AmHost)
					{
						JoinGame();
					}
				}
				break;
			}
			default:
				Debug.Log($"Bad tag {reader.Tag} at {reader.Offset}+{reader.Position}={reader.Length}:  " + string.Join(" ", reader.Buffer.Skip(reader.Offset).Take(reader.Length)));
				break;
			case 14:
			case 20:
				break;
			}
		}

		public static string AddressToString(uint address)
		{
			return $"{(byte)address}.{(byte)(address >> 8)}.{(byte)(address >> 16)}.{(byte)(address >> 24)}";
		}

		private ClientData GetOrCreateClient(ClientData client)
		{
			ClientData clientData;
			lock (allClients)
			{
				clientData = allClients.FirstOrDefault((ClientData c) => c.Id == client.Id);
				if (clientData == null)
				{
					clientData = client;
					allClients.Add(clientData);
				}
			}
			lock (recentClients)
			{
				if (recentClients.FirstOrDefault((ClientData c) => c.Id == client.Id) == null)
				{
					recentClients.Add(clientData);
					recentClients.Sort(RecentPlayerComparer.Instance);
				}
			}
			return clientData;
		}

		// Host-created fake client (debug "Create character"): registers a ClientData for an id that has no connection.
		protected ClientData RegisterFakeClient(ClientData fake)
		{
			return GetOrCreateClient(fake);
		}

		public ClientData GetClient(int clientId)
		{
			lock (allClients)
			{
				return allClients.FirstOrDefault((ClientData c) => c.Id == clientId);
			}
		}

		public void GetAllClients(List<ClientData> buffer)
		{
			lock (allClients)
			{
				buffer.Clear();
				buffer.AddRange(allClients);
			}
		}

		public void GetRecentClients(List<ClientData> buffer)
		{
			lock (recentClients)
			{
				buffer.Clear();
				buffer.AddRange(recentClients);
				buffer.Reverse();
			}
		}

		public ClientData GetRecentClient(int clientId)
		{
			lock (recentClients)
			{
				return recentClients.FirstOrDefault((ClientData c) => c.Id == clientId);
			}
		}

		private void RemovePlayer(int playerIdThatLeft, DisconnectReasons reason)
		{
			ClientData client = null;
			lock (allClients)
			{
				for (int i = 0; i < allClients.Count; i++)
				{
					ClientData clientData = allClients[i];
					if (clientData.Id == playerIdThatLeft)
					{
						client = clientData;
						allClients.RemoveAt(i);
						break;
					}
				}
			}
			if (client == null)
			{
				return;
			}
			lock (Dispatcher)
			{
				Dispatcher.Add(() =>
				{
					OnPlayerLeft(client, reason);
				});
			}
		}

		protected virtual void OnApplicationPause(bool pause)
		{
			appPaused = pause;
			if (!pause)
			{
				SecondsSuspendedBeforeDisconnect = 30;
				Debug.Log("Resumed Game");
				if (AmHost)
				{
					RemoveUnownedObjects();
				}
			}
			else if (GameState != GameStates.Ended && AmConnected)
			{
				Debug.Log("Lost focus during game");
				ThreadPool.QueueUserWorkItem(WaitToDisconnect);
			}
		}

		private void WaitToDisconnect(object state)
		{
			for (int i = 0; i < SecondsSuspendedBeforeDisconnect; i++)
			{
				if (!appPaused)
				{
					break;
				}
				Thread.Sleep(1000);
			}
			if (appPaused && GameState != GameStates.Ended && AmConnected)
			{
				DisconnectInternal(DisconnectReasons.FocusLostBackground);
				EnqueueDisconnect(DisconnectReasons.FocusLost);
			}
		}

		protected void SendInitialData(int clientId)
		{
			sendInitialDataSpawnGameDataMessages.Clear();
			lock (allObjects)
			{
				HashSet<GameObject> hashSet = new HashSet<GameObject>();
				IReadOnlyList<InnerNetObject> readOnlyList = allObjects.AllObjects;
				for (int i = 0; i < readOnlyList.Count; i++)
				{
					InnerNetObject innerNetObject = readOnlyList[i];
					if ((bool)innerNetObject && (innerNetObject.OwnerId != -4 || AmModdedHost) && hashSet.Add(innerNetObject.gameObject))
					{
						if (innerNetObject is GameManager gameManager)
						{
							SendGameManager(clientId, gameManager);
						}
						else
						{
							sendInitialDataSpawnGameDataMessages.Add(CreateSpawnMessage(innerNetObject, innerNetObject.OwnerId, innerNetObject.SpawnFlags));
						}
					}
				}
			}
			MessageWriter messageWriter = null;
			int count = sendInitialDataSpawnGameDataMessages.Count;
			int maxMessagePackingLimit = GetMaxMessagePackingLimit();
			int num = 0;
			for (int j = 0; j < count; j++)
			{
				if (messageWriter == null)
				{
					num = 0;
					messageWriter = MessageWriter.Get(SendOption.Reliable);
					messageWriter.StartMessage(6);
					messageWriter.Write(GameId);
					messageWriter.WritePacked(clientId);
				}
				SpawnGameDataMessage spawnGameDataMessage = sendInitialDataSpawnGameDataMessages[j];
				int length = messageWriter.Length;
				int position = messageWriter.Position;
				spawnGameDataMessage.Serialize(messageWriter);
				if (messageWriter.Length > 500)
				{
					if (num == 0)
					{
						serverLogger.Warning(string.Format("MTU Warning: Spawning {0} during {1} is {2} bytes!", spawnGameDataMessage.NetObjectType, "SendInitialData", messageWriter.Length));
						spawnGameDataMessage.ClearOrDecrementChildObjectDirt();
					}
					else
					{
						messageWriter.Length = length;
						messageWriter.Position = position;
						j--;
					}
					messageWriter.EndMessage();
					SendOrDisconnect(messageWriter);
					messageWriter.Recycle();
					messageWriter = null;
				}
				else
				{
					spawnGameDataMessage.ClearOrDecrementChildObjectDirt();
					num++;
					if (j + 1 >= count || num >= maxMessagePackingLimit)
					{
						messageWriter.EndMessage();
						SendOrDisconnect(messageWriter);
						messageWriter.Recycle();
						messageWriter = null;
					}
				}
			}
		}

		private void SendGameManager(int clientId, GameManager gameManager)
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(6);
			messageWriter.Write(GameId);
			messageWriter.WritePacked(clientId);
			SpawnGameDataMessage spawnGameDataMessage = CreateSpawnMessage(gameManager, gameManager.OwnerId, gameManager.SpawnFlags);
			if (messageWriter.Length > 500)
			{
				serverLogger.Warning(string.Format("MTU Warning: Spawning {0} is {1} bytes!", "GameManager", messageWriter.Length));
			}
			spawnGameDataMessage.Serialize(messageWriter);
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		protected abstract void OnGameCreated(string gameIdString);

		protected abstract void OnGameJoined(string gameIdString);

		protected abstract void OnWaitForHost(string gameIdString);

		protected abstract IEnumerator CoStartGame();

		protected abstract void OnGameEnd(EndGameResult endGameResult);

		protected abstract void OnBecomeHost();

		protected abstract void OnPlayerJoined(ClientData client);

		protected abstract IEnumerator CoOnPlayerChangedScene(ClientData client, string targetScene);

		protected abstract void OnPlayerLeft(ClientData client, DisconnectReasons reason);

		protected abstract void OnReportedPlayer(ReportOutcome outcome, int clientId, string playerName, ReportReasons reason);

		protected abstract void OnDisconnected();

		protected abstract void PreDisconnectInternal();

		private void OnSetPSNSessionId(string sessionId)
		{
		}

		protected abstract void OnGetGameList(TotalGameData totalGames, HttpMatchmakerManager.FindGamesListFilteredResponse gamesResponse);

		private byte[] GetConnectionData(bool useDtlsLayout, string matchmakerToken)
		{
			MessageWriter messageWriter = new MessageWriter(1000);
			messageWriter.Write(Constants.GetBroadcastVersion());
			messageWriter.Write(DataManager.Player.Customization.Name);
			if (useDtlsLayout)
			{
				messageWriter.Write(matchmakerToken ?? string.Empty);
			}
			else
			{
				messageWriter.Write(DestroyableSingleton<AuthManager>.Instance.LastNonceReceived.GetValueOrDefault());
			}
			messageWriter.Write((uint)DataManager.Settings.Language.CurrentLanguage);
			messageWriter.Write((byte)DataManager.Settings.Multiplayer.ChatMode);
			Constants.GetPlatformData().Serialize(messageWriter);
			if (useDtlsLayout)
			{
				messageWriter.Write(DestroyableSingleton<EOSManager>.Instance.FriendCode ?? string.Empty);
			}
			else
			{
				messageWriter.Write("");
				messageWriter.Write(0u);
			}
			return messageWriter.ToByteArray(includeHeader: true);
		}

		protected ClientData FindClientById(int id)
		{
			if (id < 0)
			{
				return null;
			}
			lock (allClients)
			{
				for (int i = 0; i < allClients.Count; i++)
				{
					ClientData clientData = allClients[i];
					if (clientData.Id == id)
					{
						return clientData;
					}
				}
				return null;
			}
		}

		private void FixedUpdate()
		{
			if (connection != null)
			{
				connection.FixedUpdate();
			}
			if (mode == MatchMakerModes.None)
			{
				timer = 0f;
				return;
			}
			timer += Time.fixedDeltaTime;
			if (!(timer < MinSendInterval))
			{
				timer = 0f;
				SendAllStreamedObjects();
			}
		}

		protected void UpdateCachedClients(ClientData clientData, PlayerControl character)
		{
			ClientData client = GetClient(clientData.Id);
			if (client != null)
			{
				client.Character = character;
			}
			ClientData recentClient = GetRecentClient(clientData.Id);
			if (recentClient != null)
			{
				recentClient.Character = character;
			}
		}

		private void SendAllStreamedObjects()
		{
			PackAndSendQueuedMessages(reliableMessageQueue, SendOption.Reliable, out var thereAreQueuedMessagesLeft);
			if (thereAreQueuedMessagesLeft)
			{
				numberOfConsecutiveFramesWithReliableMessages++;
				if (numberOfConsecutiveFramesWithReliableMessages > 5)
				{
					serverLogger.Warning(string.Format("{0} - sending queued reliable messages for {1} frames in a row", "SendAllStreamedObjects", numberOfConsecutiveFramesWithReliableMessages));
				}
			}
			else
			{
				numberOfConsecutiveFramesWithReliableMessages = 0;
			}
			PackAndSendQueuedMessages(unreliableMessageQueue, SendOption.None, out var thereAreQueuedMessagesLeft2);
			if (thereAreQueuedMessagesLeft2)
			{
				numberOfConsecutiveFramesWithUnreliableMessages++;
				if (numberOfConsecutiveFramesWithUnreliableMessages > 5)
				{
					serverLogger.Warning(string.Format("{0} - sending queued unreliable messages for {1} frames in a row", "SendAllStreamedObjects", numberOfConsecutiveFramesWithUnreliableMessages));
				}
			}
			else
			{
				numberOfConsecutiveFramesWithUnreliableMessages = 0;
			}
			lock (allObjects)
			{
				PackAndSendDirtyStreamedObjects(allObjects.AllReliableObjects, SendOption.Reliable, ref indexForReliableStreamedUpdates);
				PackAndSendDirtyStreamedObjects(allObjects.AllUnreliableObjects, SendOption.None, ref indexForUnreliableStreamedUpdates);
			}
		}

		public T FindObjectByNetId<T>(uint netId) where T : InnerNetObject
		{
			lock (allObjects)
			{
				if (allObjects.AllObjectsFast.TryGetValue(netId, out var value))
				{
					return (T)value;
				}
			}
			return null;
		}

		public MessageWriter StartRpcImmediately(uint targetNetId, byte callId, SendOption option, int targetClientId = -1)
		{
			MessageWriter messageWriter = MessageWriter.Get(option);
			if (targetClientId < 0)
			{
				messageWriter.StartMessage(5);
				messageWriter.Write(GameId);
			}
			else
			{
				messageWriter.StartMessage(6);
				messageWriter.Write(GameId);
				messageWriter.WritePacked(targetClientId);
			}
			messageWriter.StartMessage(2);
			messageWriter.WritePacked(targetNetId);
			messageWriter.Write(callId);
			return messageWriter;
		}

		public void FinishRpcImmediately(MessageWriter msg)
		{
			msg.EndMessage();
			msg.EndMessage();
			SendOrDisconnect(msg);
			msg.Recycle();
		}

		public void SendXuidToAll()
		{
			Debug.Log("InnerNetClient.During::SendXuidToAll");
		}

		public void LateBroadcastReliableMessage(IGameDataMessage rpcMessage)
		{
			reliableMessageQueue.Enqueue(rpcMessage);
		}

		public void LateBroadcastUnreliableMessage(IGameDataMessage rpcMessage)
		{
			unreliableMessageQueue.Enqueue(rpcMessage);
		}

		private void SendSceneChange(string sceneName)
		{
			if (!(sceneName == "FindAGame"))
			{
				InOnlineScene = string.Equals(sceneName, "OnlineGame");
				if (AmConnected)
				{
					Debug.Log("Changed To " + sceneName);
					StartCoroutine(CoSendSceneChange(sceneName));
				}
			}
		}

		private IEnumerator CoSendSceneChange(string sceneName)
		{
			FriendsListManager friendsListManager = DestroyableSingleton<FriendsListManager>.Instance;
			if (friendsListManager)
			{
				friendsListManager.ReparentUI();
			}
			lock (allObjects)
			{
				allObjects.RemoveNullObjects();
			}
			while (AmConnected && ClientId < 0)
			{
				yield return null;
			}
			if (!AmConnected)
			{
				yield break;
			}
			ClientData clientData = FindClientById(ClientId);
			if (clientData != null)
			{
				Debug.Log($"Self changed scene: {ClientId} {sceneName}");
				if (NetworkMode == NetworkModes.OnlineGame || (connection != null && connection.State == ConnectionState.Connected))
				{
					MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
					messageWriter.StartMessage(5);
					messageWriter.Write(GameId);
					messageWriter.StartMessage(6);
					messageWriter.WritePacked(ClientId);
					messageWriter.Write(sceneName);
					messageWriter.EndMessage();
					messageWriter.EndMessage();
					SendOrDisconnect(messageWriter);
					messageWriter.Recycle();
				}
				yield return CoOnPlayerChangedScene(clientData, sceneName);
			}
			else
			{
				Debug.Log($"Couldn't find self in clients: {ClientId}: " + sceneName);
			}
		}

		public void Spawn(InnerNetObject netObjParent, int ownerId = -2, SpawnFlags flags = SpawnFlags.None)
		{
			if (!AmHost)
			{
				if (AmClient)
				{
					Debug.LogError("Tried to spawn while not host:" + netObjParent);
				}
			}
			else
			{
				ownerId = ((ownerId == -3) ? ClientId : ownerId);
				LateBroadcastReliableMessage(CreateSpawnMessage(netObjParent, ownerId, flags));
			}
		}

		private SpawnGameDataMessage CreateSpawnMessage(InnerNetObject netObjParent, int ownerId, SpawnFlags flags)
		{
			InnerNetObject[] componentsInChildren = netObjParent.GetComponentsInChildren<InnerNetObject>();
			foreach (InnerNetObject innerNetObject in componentsInChildren)
			{
				innerNetObject.OwnerId = ownerId;
				innerNetObject.SpawnFlags = flags;
				if (innerNetObject.NetId == 0)
				{
					innerNetObject.NetId = NetIdCnt++;
					lock (allObjects)
					{
						allObjects.TryAddNetObject(innerNetObject);
					}
				}
			}
			return new SpawnGameDataMessage(netObjParent, ownerId, flags, componentsInChildren);
		}

		public void Despawn(InnerNetObject objToDespawn)
		{
			if (objToDespawn.NetId < 1)
			{
				Debug.LogError("Tried to net destroy: " + objToDespawn);
				return;
			}
			DespawnGameDataMessage item = new DespawnGameDataMessage(objToDespawn.NetId);
			reliableMessageQueue.Enqueue(item);
			RemoveNetObject(objToDespawn);
		}

		private bool AddNetObject(InnerNetObject obj)
		{
			uint num = obj.NetId + 1;
			if (num > NetIdCnt && num < 100000)
			{
				NetIdCnt = num;
			}
			lock (allObjects)
			{
				return allObjects.TryAddNetObject(obj);
			}
		}

		public void RemoveNetObject(InnerNetObject obj)
		{
			lock (allObjects)
			{
				allObjects.RemoveNetObject(obj);
			}
		}

		public void RemoveUnownedObjects()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(-2);
			hashSet.Add(-4);
			lock (allClients)
			{
				for (int i = 0; i < allClients.Count; i++)
				{
					ClientData clientData = allClients[i];
					if ((bool)clientData.Character)
					{
						hashSet.Add(clientData.Id);
					}
				}
			}
			lock (allObjects)
			{
				allObjects.RemoveUnownedObjects(hashSet, ClientId, (GameObject gameObject) =>
				{
					UnityEngine.Object.Destroy(gameObject);
				});
			}
		}

		private void HandleGameData(MessageReader parentReader)
		{
			try
			{
				while (parentReader.Position < parentReader.Length)
				{
					MessageReader reader = parentReader.ReadMessageAsNewBuffer();
					StartCoroutine(HandleGameDataInner(reader, msgNum++));
				}
			}
			finally
			{
				parentReader.Recycle();
			}
		}

		private IEnumerator HandleGameDataInner(MessageReader reader, int msgNum)
		{
			int cnt = 0;
			reader.Position = 0;
			switch ((GameDataTypes)reader.Tag)
			{
			case GameDataTypes.DataFlag:
				try
				{
					while (true)
					{
						uint num3 = reader.ReadPackedUInt32();
						lock (allObjects)
						{
							if (allObjects.AllObjectsFast.TryGetValue(num3, out var value2))
							{
								value2.Deserialize(reader, initialState: false);
							}
							else if (!DestroyedObjects.Contains(num3))
							{
								Debug.LogWarning("Stored data for " + num3);
								if (cnt++ <= 10)
								{
									reader.Position = 0;
									yield return Effects.Wait(0.1f);
									continue;
								}
								break;
							}
						}
						break;
					}
					break;
				}
				finally
				{
					reader.Recycle();
				}
			case GameDataTypes.RpcFlag:
				try
				{
					while (true)
					{
						uint num2;
						try
						{
							num2 = reader.ReadPackedUInt32();
						}
						catch (Exception ex)
						{
							Debug.LogError($"Error in {msgNum} try {cnt}, Pos:{reader.Position}/{reader.Length}: {ex}");
							Debug.LogException(ex, this);
							throw;
						}
						byte b = reader.ReadByte();
						lock (allObjects)
						{
							if (allObjects.AllObjectsFast.TryGetValue(num2, out var value))
							{
								value.HandleRpc(b, reader);
							}
							else if (num2 != uint.MaxValue && !DestroyedObjects.Contains(num2))
							{
								Debug.LogWarning($"Stored Msg {msgNum} RPC {(RpcCalls)b} for " + num2);
								if (cnt++ <= 10)
								{
									reader.Position = 0;
									yield return Effects.Wait(0.1f);
									continue;
								}
								break;
							}
						}
						break;
					}
					break;
				}
				finally
				{
					reader.Recycle();
				}
			case GameDataTypes.SpawnFlag:
				StartCoroutine(CoHandleSpawn(reader));
				break;
			case GameDataTypes.DespawnFlag:
				try
				{
					uint num4 = reader.ReadPackedUInt32();
					DestroyedObjects.Add(num4);
					InnerNetObject innerNetObject = FindObjectByNetId<InnerNetObject>(num4);
					if ((bool)innerNetObject && !innerNetObject.AmOwner)
					{
						RemoveNetObject(innerNetObject);
						UnityEngine.Object.Destroy(innerNetObject.gameObject);
					}
					break;
				}
				finally
				{
					reader.Recycle();
				}
			case GameDataTypes.SceneChangeFlag:
			{
				int num = reader.ReadPackedInt32();
				ClientData clientData2 = FindClientById(num);
				string text = reader.ReadString();
				if (clientData2 != null && !string.IsNullOrWhiteSpace(text))
				{
					StartCoroutine(CoOnPlayerChangedScene(clientData2, text));
					break;
				}
				Debug.Log($"Couldn't find client {num} to change scene to {text}");
				reader.Recycle();
				break;
			}
			case GameDataTypes.ReadyFlag:
				try
				{
					ClientData clientData = FindClientById(reader.ReadPackedInt32());
					if (clientData != null)
					{
						clientData.IsReady = true;
					}
					break;
				}
				finally
				{
					reader.Recycle();
				}
			case GameDataTypes.XboxDeclareXuid:
				try
				{
					ulong.TryParse(reader.ReadString(), out var _);
					break;
				}
				finally
				{
					reader.Recycle();
				}
			default:
				Debug.Log($"Bad tag {reader.Tag} at {reader.Offset}+{reader.Position}={reader.Length}:  " + string.Join(" ", reader.Buffer.Take(128)));
				reader.Recycle();
				break;
			}
		}

		private IEnumerator CoHandleSpawn(MessageReader reader)
		{
			uint spawnId = reader.ReadPackedUInt32();
			if (spawnId < SpawnableObjects.Length)
			{
				int ownerId = reader.ReadPackedInt32();
				ClientData ownerClient = FindClientById(ownerId);
				int frames = 0;
				while (frames < 1000 && ownerId > 0 && ownerClient == null)
				{
					Debug.LogWarning("Delay spawn for unowned " + spawnId);
					yield return null;
					ownerClient = FindClientById(ownerId);
					int num = frames + 1;
					frames = num;
				}
				if (ownerId > 0 && ownerClient == null)
				{
					yield break;
				}
				InnerNetObject innerNetObject = NonAddressableSpawnableObjects.FirstOrDefault((InnerNetObject f) => f.SpawnId == spawnId);
				InnerNetObject innerNetObject2;
				if ((bool)innerNetObject)
				{
					innerNetObject2 = UnityEngine.Object.Instantiate(innerNetObject);
				}
				else
				{
					AsyncOperationHandle<GameObject> spawnPrefab = SpawnableObjects[spawnId].InstantiateAsync((Transform)null, false);
					yield return spawnPrefab;
					innerNetObject2 = spawnPrefab.Result.GetComponent<InnerNetObject>();
				}
				innerNetObject2.SpawnFlags = (SpawnFlags)reader.ReadByte();
				if (innerNetObject2.SpawnFlags.HasFlag(SpawnFlags.IsClientCharacter) && ownerClient != null)
				{
					if (!ownerClient.Character)
					{
						ownerClient.InScene = true;
						ownerClient.Character = innerNetObject2 as PlayerControl;
						UpdateCachedClients(ownerClient, ownerClient.Character);
					}
					else if ((bool)innerNetObject2)
					{
						Debug.LogWarning($"Double spawn character: {ownerClient.Id} already has {ownerClient.Character.NetId}");
						UnityEngine.Object.Destroy(innerNetObject2.gameObject);
						yield break;
					}
				}
				int num2 = reader.ReadPackedInt32();
				InnerNetObject[] componentsInChildren = innerNetObject2.GetComponentsInChildren<InnerNetObject>();
				if (num2 != componentsInChildren.Length)
				{
					Debug.LogError($"Children didn't match for spawnable {spawnId} ({innerNetObject2}): {num2} != {componentsInChildren.Length}");
					UnityEngine.Object.Destroy(innerNetObject2.gameObject);
					yield break;
				}
				for (int num3 = 0; num3 < num2; num3++)
				{
					InnerNetObject innerNetObject3 = componentsInChildren[num3];
					innerNetObject3.NetId = reader.ReadPackedUInt32();
					innerNetObject3.OwnerId = ownerId;
					if (DestroyedObjects.Contains(innerNetObject3.NetId))
					{
						innerNetObject2.NetId = uint.MaxValue;
						UnityEngine.Object.Destroy(innerNetObject2.gameObject);
						break;
					}
					if (!AddNetObject(innerNetObject3))
					{
						innerNetObject2.NetId = uint.MaxValue;
						UnityEngine.Object.Destroy(innerNetObject2.gameObject);
						break;
					}
					MessageReader messageReader = reader.ReadMessage();
					if (messageReader.Length > 0)
					{
						try
						{
							innerNetObject3.Deserialize(messageReader, initialState: true);
						}
						catch (Exception arg)
						{
							Debug.LogError($"Failed to deserialize initial info: {innerNetObject3}: {arg}");
						}
					}
				}
			}
			else
			{
				Debug.LogError("Couldn't find spawnable prefab: " + spawnId);
			}
		}

		private void PackAndSendQueuedMessages(Queue<IGameDataMessage> queue, SendOption sendOption, out bool thereAreQueuedMessagesLeft)
		{
			thereAreQueuedMessagesLeft = false;
			if (queue.Count <= 0)
			{
				return;
			}
			MessageWriter messageWriter = MessageWriter.Get(sendOption);
			messageWriter.StartMessage(5);
			messageWriter.Write(GameId);
			int maxMessagePackingLimit = GetMaxMessagePackingLimit();
			for (int i = 0; i < maxMessagePackingLimit; i++)
			{
				if (queue.Count <= 0)
				{
					break;
				}
				IGameDataMessage gameDataMessage = queue.Peek();
				int length = messageWriter.Length;
				int position = messageWriter.Position;
				gameDataMessage.Serialize(messageWriter);
				if (messageWriter.Length > 500)
				{
					if (i == 0)
					{
						queue.Dequeue();
						if (gameDataMessage is SpawnGameDataMessage spawnGameDataMessage)
						{
							spawnGameDataMessage.ClearOrDecrementChildObjectDirt();
						}
						serverLogger.Warning(string.Format("MTU Warning: Packing {0} during {1} is {2} bytes!", gameDataMessage.GameDataType, "PackAndSendQueuedMessages", messageWriter.Length));
					}
					else
					{
						messageWriter.Length = length;
						messageWriter.Position = position;
					}
					break;
				}
				queue.Dequeue();
				if (gameDataMessage is SpawnGameDataMessage spawnGameDataMessage2)
				{
					spawnGameDataMessage2.ClearOrDecrementChildObjectDirt();
				}
			}
			messageWriter.EndMessage();
			SendOrDisconnect(messageWriter);
			messageWriter.Recycle();
		}

		private void PackAndSendDirtyStreamedObjects(IReadOnlyList<InnerNetObject> streamCollection, SendOption sendOption, ref int trackingIndex)
		{
			MessageWriter writer = null;
			int maxMessagePackingLimit = GetMaxMessagePackingLimit();
			int count = streamCollection.Count;
			if (count == 0)
			{
				return;
			}
			if (trackingIndex >= count)
			{
				trackingIndex = 0;
			}
			int num = 0;
			int num2 = 0;
			while (num < maxMessagePackingLimit && num2 < count)
			{
				InnerNetObject innerNetObject = streamCollection[trackingIndex];
				trackingIndex++;
				if (trackingIndex >= count)
				{
					trackingIndex = 0;
				}
				if ((bool)innerNetObject && innerNetObject.IsDirty && (innerNetObject.AmOwner || (innerNetObject.OwnerId == -2 && AmHost)))
				{
					if (writer == null)
					{
						StartStreamedObjectMessage(ref writer, sendOption);
					}
					writer.StartMessage(1);
					writer.WritePacked(innerNetObject.NetId);
					try
					{
						if (innerNetObject.Serialize(writer, initialState: false))
						{
							if (writer.Length > 500)
							{
								if (num > 0)
								{
									writer.CancelMessage();
									trackingIndex--;
									if (trackingIndex < 0)
									{
										trackingIndex = count - 1;
									}
									break;
								}
								serverLogger.Warning(string.Format("MTU Warning: Packing {0} during {1} is {2} bytes!", innerNetObject.GetType(), "PackAndSendDirtyStreamedObjects", writer.Length));
							}
							writer.EndMessage();
							innerNetObject.ClearOrDecrementDirt();
							num++;
						}
						else
						{
							writer.CancelMessage();
						}
					}
					catch (Exception arg)
					{
						serverLogger.Error(string.Format("{0} failed to pack message of Type = {1} due to exception = {2}", "PackAndSendDirtyStreamedObjects", innerNetObject.GetType(), arg));
						writer.CancelMessage();
					}
				}
				num2++;
			}
			if (writer != null)
			{
				if (writer.HasBytes(7))
				{
					writer.EndMessage();
					SendOrDisconnect(writer);
				}
				writer.Recycle();
			}
		}

		private void StartStreamedObjectMessage(ref MessageWriter writer, SendOption option)
		{
			writer = MessageWriter.Get(option);
			writer.StartMessage(5);
			writer.Write(GameId);
		}

		private int GetMaxMessagePackingLimit()
		{
			int num = 0;
			if (AmHost)
			{
				num = allClients.Count * 2;
			}
			return 10 + num;
		}
	}
}
