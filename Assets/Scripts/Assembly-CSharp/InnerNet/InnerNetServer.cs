using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Cpp2ILInjected;
using Hazel;
using Hazel.Udp;
using UnityEngine;

namespace InnerNet
{
	public class InnerNetServer : DestroyableSingleton<InnerNetServer>
	{
		protected class Player
		{
			// Host-created fake clients carry their own identity (no connection).
			public uint FakeLevel = 1u;

			public string FakePuid = string.Empty;

			public string FakeFriendCode = string.Empty;

			private static int IdCount = 1;

			public int Id;

			public Connection Connection;

			public string PlayerName;

			public PlatformSpecificData PlatformData;

			public LimboStates LimboState;

			public unsafe Player(Connection connection, string playerName, PlatformSpecificData platformData)
			{
				Id = Interlocked.Increment(ref IdCount);
				Connection = connection;
				PlayerName = playerName;
				PlatformData = platformData;
			}
		}

		[Serializable]
		private sealed class _003C_003Ec
		{
			public static readonly _003C_003Ec _003C_003E9;

			public static Func<byte, string> _003C_003E9__19_0;

			static _003C_003Ec()
			{
				_003C_003Ec obj = new _003C_003Ec();
				_003C_003E9 = obj;
			}

			internal string _003CConnection_DataRecievedRaw_003Eb__19_0(byte b)
			{
				byte b2 = default;
				return b2.ToString();
			}
		}

		private sealed class _003C_003Ec__DisplayClass17_0
		{
			public InnerNetServer _003C_003E4__this;

			public Player client;

			internal unsafe void _003COnServerConnect_003Eb__0(DataReceivedEventArgs e)
			{
				_003C_003E4__this.OnDataReceived(client, e);
			}

			internal void _003COnServerConnect_003Eb__1(object o, DisconnectedEventArgs e)
			{
				_003C_003E4__this.ClientDisconnect(client);
			}
		}

		// valid 4-letter game code "LOCL" (the old value 32 does not map to a code string)
		public const int LegacyLocalGameId = 32;

		public const int LocalGameId = 32; // original value: not a valid room code, so the lobby shows the LOCAL label and hides code/privacy (GameStartManager)

		private const int InvalidHost = -1;

		private int HostId = -1;

		public PlatformSpecificData PlatformData;

		public HashSet<string> ipBans;

		public int Port;

		[SerializeField]
		private GameStates GameState;

		private UdpConnectionListener listener;

		private List<Player> Clients;

		public override void OnDestroy()
		{
			StopServer();
			base.OnDestroy();
		}

		public void StartAsServer()
		{
			if (listener != null)
			{
				StopServer();
			}
			GameState = GameStates.NotStarted;
			HostId = -1;
			listener = new UdpConnectionListener(new IPEndPoint(IPAddress.Any, Port), IPMode.IPv4, new UnityLogger());
			listener.NewConnection += OnServerConnect;
			listener.OnInternalError += OnInternalError;
			// TODO(verify): the decompile loses the tail (Start call and extra listener fields); Start() assumed as in the 2019 baseline
			listener.Start();
		}

		private unsafe void OnInternalError(HazelInternalErrors errorType)
		{
			if (errorType == HazelInternalErrors.ConnectionDisconnected)
			{
				AmongUsClient.Instance.ExitGame(DisconnectReasons.FocusLost);
				return;
			}
			Debug.LogError("Internal error in InnerNetServer:" + errorType);
			AmongUsClient.Instance.ExitGame(DisconnectReasons.Error);
		}

		public void StartAsLocalServer()
		{
			if (listener != null)
			{
				StopServer();
			}
			GameState = GameStates.NotStarted;
			HostId = -1;
			listener = new UdpConnectionListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port), IPMode.IPv4, new UnityLogger());
			listener.NewConnection += OnServerConnect;
			// TODO(verify): the decompile loses the tail (Start call and extra listener fields); Start() assumed as in the 2019 baseline
			listener.Start();
		}

		private void DebugString(string obj)
		{
			if (!string.IsNullOrWhiteSpace(obj))
			{
				Debug.LogError(obj);
			}
		}

		public void StopServer()
		{
			HostId = -1;
			GameState = GameStates.Destroyed;
			if (listener != null)
			{
				listener.Dispose();
				listener = null;
			}
			lock (Clients)
			{
				Clients.Clear();
				FakePlayers.Clear();
			}
		}

		public static bool IsCompatibleVersion(int version)
		{
			return Constants.CompatVersions.Contains(version);
		}

		private unsafe void OnServerConnect(NewConnectionEventArgs evt)
		{
			MessageReader handshakeData = evt.HandshakeData;
			Player client;
			try
			{
				if (handshakeData.Length < 5)
				{
					SendIncorrectVersion(evt.Connection);
					return;
				}
				int clientVersion = handshakeData.ReadInt32();
				Debug.Log("Server: handshake from " + evt.Connection.EndPoint + " version=" + clientVersion + " ("
					+ Constants.GetVersionComponents(clientVersion) + ") ours=" + Constants.GetBroadcastVersion() + " length=" + handshakeData.Length);
				if (!IsCompatibleVersion(clientVersion))
				{
					Debug.Log("Server: rejecting client, incompatible version " + clientVersion);
					SendIncorrectVersion(evt.Connection);
					return;
				}
				string playerName = handshakeData.ReadString();
				handshakeData.ReadInt32();
				handshakeData.ReadInt32();
				handshakeData.ReadByte();
				PlatformSpecificData platformSpecificData = new PlatformSpecificData();
				platformSpecificData.Deserialize(handshakeData.ReadMessage());
				client = new Player(evt.Connection, playerName, platformSpecificData);
			}
			finally
			{
				handshakeData.Recycle();
			}
			Debug.Log($"Client {client.Id} added: {evt.Connection.EndPoint}");
			UdpConnection udpConnection = (UdpConnection)evt.Connection;
			udpConnection.KeepAliveInterval = 1500;
			udpConnection.DisconnectTimeoutMs = 6000;
			udpConnection.ResendPingMultiplier = 1.5f;
			udpConnection.DataReceived += delegate(DataReceivedEventArgs e)
			{
				OnDataReceived(client, e);
			};
			udpConnection.Disconnected += delegate
			{
				ClientDisconnect(client);
			};
		}

		private static void SendIncorrectVersion(Connection connection)
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(1);
			messageWriter.Write(5);
			messageWriter.EndMessage();
			connection.Send(messageWriter);
			messageWriter.Recycle();
		}

		private void Connection_DataRecievedRaw(byte[] data, int length)
		{
			Debug.Log("Server Got: " + string.Join(" ", data.Select((byte b) => b.ToString()).Take(length)));
		}

		private void OnDataReceived(Player client, DataReceivedEventArgs evt)
		{
			MessageReader message = evt.Message;
			try
			{
				while (message.Position < message.Length)
				{
					HandleMessage(client, message.ReadMessage(), evt.SendOption);
				}
			}
			catch (Exception exception)
			{
				Debug.LogError("InnerNetServer::OnDataReceived Exception");
				Debug.LogException(exception, this);
			}
			finally
			{
				message.Recycle();
			}
		}

		// Rewritten against the CURRENT client protocol (InnerNetClient.HandleMessage is the spec; layouts in scratch/reports/net.md,
		// server behaviour follows Impostor: relay GameData, reply HostGame/JoinedGame/JoinGame). No limbo handling (single local game).
		private void HandleMessage(Player client, MessageReader reader, SendOption sendOption)
		{
			lock (Clients)
			{
				switch (reader.Tag)
				{
				case 0:
				case 25:
					// HostGame / HostModdedGame: options are ignored, reply with the (fixed) local game id.
					Debug.Log("Server: HostGame from client " + client.Id);
					SendHostGameReply(client);
					break;
				case 1:
				{
					int gameId = reader.ReadInt32();
					Debug.Log("Server: JoinGame from client " + client.Id + " game " + gameId);
					// The original game (and the official clients' LAN discovery, PC and mobile) joins local games with the fixed id 32; the room
					// code we hand out afterwards in JoinedGame is LocalGameId ("LOCL"), which the client then uses for every later message.
					if (gameId != LocalGameId && gameId != LegacyLocalGameId)
					{
						SendDisconnectReason(client.Connection, DisconnectReasons.GameNotFound);
						break;
					}
					JoinGame(client);
					break;
				}
				case 2:
					if (reader.ReadInt32() == LocalGameId && client.Id == HostId)
					{
						StartGame(reader, client);
					}
					break;
				case 4:
				{
					int gameId = reader.ReadInt32();
					int targetId = reader.ReadPackedInt32();
					byte reason = reader.ReadByte();
					if (gameId == LocalGameId && (client.Id == HostId || client.Id == targetId))
					{
						RemovePlayerRequest(targetId, reason);
					}
					break;
				}
				case 5:
				case 6:
				{
					int gameId = reader.ReadInt32();
					if (gameId != LocalGameId)
					{
						break;
					}
					int targetId = -1;
					if (reader.Tag == 6)
					{
						targetId = reader.ReadPackedInt32();
					}
					MessageWriter messageWriter = MessageWriter.Get(sendOption);
					messageWriter.StartMessage(reader.Tag);
					messageWriter.Write(gameId);
					if (reader.Tag == 6)
					{
						messageWriter.WritePacked(targetId);
					}
					int remaining = reader.Length - reader.Position;
					if (remaining > 0)
					{
						messageWriter.Write(reader.ReadBytes(remaining));
					}
					messageWriter.EndMessage();
					if (reader.Tag == 6)
					{
						SendTo(messageWriter, targetId);
					}
					else
					{
						Broadcast(messageWriter, client);
					}
					messageWriter.Recycle();
					break;
				}
				case 8:
					EndGame(reader, client);
					break;
				case 10:
				{
					int gameId = reader.ReadInt32();
					if (gameId != LocalGameId || client.Id != HostId)
					{
						break;
					}
					byte alterTag = reader.ReadByte();
					bool value = reader.ReadBoolean();
					MessageWriter messageWriter2 = MessageWriter.Get(SendOption.Reliable);
					messageWriter2.StartMessage(10);
					messageWriter2.Write(gameId);
					messageWriter2.Write(alterTag);
					messageWriter2.Write(value);
					messageWriter2.EndMessage();
					Broadcast(messageWriter2, null);
					messageWriter2.Recycle();
					break;
				}
				case 11:
				{
					int gameId = reader.ReadInt32();
					int targetId = reader.ReadPackedInt32();
					bool ban = reader.ReadBoolean();
					if (gameId == LocalGameId && client.Id == HostId)
					{
						KickPlayer(targetId, ban);
					}
					break;
				}
				case 22:
				{
					int gameId = reader.ReadInt32();
					MessageWriter messageWriter3 = MessageWriter.Get(SendOption.Reliable);
					messageWriter3.StartMessage(22);
					messageWriter3.Write(gameId);
					for (int i = 0; i < Clients.Count; i++)
					{
						Clients[i].PlatformData.Serialize(messageWriter3);
					}
					messageWriter3.EndMessage();
					SendTo(messageWriter3, client.Id);
					messageWriter3.Recycle();
					break;
				}
				default:
					Debug.Log("Server: unhandled tag " + reader.Tag + " from client " + client.Id);
					break;
				}
			}
		}

		private void SendHostGameReply(Player client)
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(0);
			messageWriter.Write(LocalGameId);
			messageWriter.EndMessage();
			SendTo(messageWriter, client.Id, client);
			messageWriter.Recycle();
		}

		// Disconnect payload: bool hasReason, message(0){u8 reason} (see InnerNetClient.OnDisconnect).
		private static void SendDisconnectReason(Connection connection, DisconnectReasons reason)
		{
			MessageWriter messageWriter = MessageWriter.Get(SendOption.None);
			try
			{
				messageWriter.Write(true);
				messageWriter.StartMessage(0);
				messageWriter.Write((byte)reason);
				messageWriter.EndMessage();
				connection.Disconnect(reason.ToString(), messageWriter);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			finally
			{
				messageWriter.Recycle();
			}
		}

		private void RemovePlayerRequest(int targetId, byte reason)
		{
			Player player = null;
			for (int i = 0; i < Clients.Count; i++)
			{
				if (Clients[i].Id == targetId)
				{
					player = Clients[i];
					break;
				}
			}
			if (player != null)
			{
				RemoveClient(player, reason);
			}
		}

		private unsafe void KickPlayer(int targetId, bool ban)
		{
			lock (Clients)
			{
				Player player = null;
				for (int i = 0; i < Clients.Count; i++)
				{
					if (Clients[i].Id == targetId)
					{
						player = Clients[i];
						break;
					}
				}
				if (player == null)
				{
					return;
				}
				if (ban)
				{
					lock (ipBans)
					{
						IPEndPoint endPoint = player.Connection.EndPoint;
						ipBans.Add(endPoint.Address.ToString());
					}
				}
				MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
				messageWriter.StartMessage(11);
				messageWriter.Write(LocalGameId);
				messageWriter.WritePacked(targetId);
				messageWriter.Write(ban);
				messageWriter.EndMessage();
				Broadcast(messageWriter, null);
				messageWriter.Recycle();
			}
		}

		protected void JoinGame(Player client)
		{
			lock (Clients)
			{
				if (client == null || client.Connection == null)
				{
					return;
				}
				IPEndPoint endPoint = client.Connection.EndPoint;
				if (endPoint != null)
				{
					string key = endPoint.Address.ToString();
					bool banned;
					lock (ipBans)
					{
						banned = ipBans.Contains(key);
					}
					if (banned)
					{
						SendDisconnectReason(client.Connection, DisconnectReasons.Banned);
						return;
					}
				}
				if (Clients.Contains(client))
				{
					// duplicate JoinGame from the same connection: re-send the joined message only
					MessageWriter again = MessageWriter.Get(SendOption.Reliable);
					WriteJoinedMessage(client, again, clear: false);
					client.Connection.Send(again);
					again.Recycle();
					return;
				}
				switch (GameState)
				{
				case GameStates.NotStarted:
					HandleNewGameJoin(client);
					break;
				case GameStates.Ended:
					HandleRejoin(client);
					break;
				default:
					SendDisconnectReason(client.Connection, DisconnectReasons.GameStarted);
					break;
				}
			}
		}

		private void HandleRejoin(Player client)
		{
			if (client.Id == HostId || Clients.Count == 0)
			{
				GameState = GameStates.NotStarted;
				HandleNewGameJoin(client);
				return;
			}
			if (Clients.Count >= 15)
			{
				SendDisconnectReason(client.Connection, DisconnectReasons.GameFull);
				return;
			}
			Clients.Add(client);
			client.LimboState = LimboStates.WaitingForHost;
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			try
			{
				messageWriter.StartMessage(12);
				messageWriter.Write(LocalGameId);
				messageWriter.Write(client.Id);
				messageWriter.EndMessage();
				client.Connection.Send(messageWriter);
				BroadcastJoinMessage(client, messageWriter);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			finally
			{
				messageWriter.Recycle();
			}
		}

		private void HandleNewGameJoin(Player client)
		{
			if (Clients.Count >= 15)
			{
				SendDisconnectReason(client.Connection, DisconnectReasons.GameFull);
				return;
			}
			Clients.Add(client);
			client.LimboState = LimboStates.PreSpawn;
			if (HostId == -1 || Clients.Count == 1)
			{
				HostId = Clients[0].Id;
			}
			if (HostId == client.Id)
			{
				client.LimboState = LimboStates.NotLimbo;
			}
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			try
			{
				WriteJoinedMessage(client, messageWriter, clear: false);
				client.Connection.Send(messageWriter);
				Debug.Log("Server: sent JoinedGame to client " + client.Id + " host=" + HostId);
				BroadcastJoinMessage(client, messageWriter);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			finally
			{
				messageWriter.Recycle();
			}
		}

		private void EndGame(MessageReader message, Player source)
		{
			int gameId = message.ReadInt32();
			if (gameId != LocalGameId)
			{
				return;
			}
			if (source.Id != HostId)
			{
				Debug.LogWarning("Reset request rejected from: " + source.Id);
				return;
			}
			byte reason = message.ReadByte();
			bool showAd = message.Position < message.Length && message.ReadBoolean();
			GameState = GameStates.Ended;
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(8);
			messageWriter.Write(gameId);
			messageWriter.Write(reason);
			messageWriter.Write(showAd);
			messageWriter.EndMessage();
			Broadcast(messageWriter, null);
			messageWriter.Recycle();
			lock (Clients)
			{
				Clients.Clear();
			}
		}

		private void StartGame(MessageReader message, Player source)
		{
			GameState = GameStates.Started;
			Debug.Log("Server: StartGame");
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.StartMessage(2);
			messageWriter.Write(LocalGameId);
			messageWriter.EndMessage();
			Broadcast(messageWriter, null);
			messageWriter.Recycle();
			// Fake (connectionless) test characters can never send their own "client ready" (GameData msg 7), and every client waits for all
			// clients to be ready after scene load, so announce it for them. TODO(verify): not part of the original server (no fake clients there).
			lock (Clients)
			{
				foreach (Player fake in FakePlayers)
				{
					MessageWriter ready = MessageWriter.Get(SendOption.Reliable);
					ready.StartMessage(5);
					ready.Write(LocalGameId);
					ready.StartMessage(7);
					ready.WritePacked(fake.Id);
					ready.EndMessage();
					ready.EndMessage();
					Broadcast(ready, null);
					ready.Recycle();
				}
			}
		}

		private void ClientDisconnect(Player client)
		{
			Debug.Log("Server DC client " + client.Id);
			RemoveClient(client, 0);
		}

		private void RemoveClient(Player client, byte reason)
		{
			lock (Clients)
			{
				if (!Clients.Remove(client))
				{
					return;
				}
				if (Clients.Count > 0)
				{
					if (client.Id == HostId)
					{
						HostId = Clients[0].Id;
					}
				}
				else
				{
					HostId = -1;
					if (GameState != GameStates.Destroyed)
					{
						GameState = GameStates.NotStarted;
					}
				}
				MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
				messageWriter.StartMessage(4);
				messageWriter.Write(LocalGameId);
				messageWriter.Write(client.Id);
				messageWriter.Write(HostId);
				messageWriter.Write(reason);
				messageWriter.EndMessage();
				Broadcast(messageWriter, null);
				messageWriter.Recycle();
			}
			try
			{
				client.Connection.Dispose();
			}
			catch (Exception)
			{
			}
		}

		protected void SendTo(MessageWriter msg, int targetId)
		{
			SendTo(msg, targetId, null);
		}

		// The extra overload lets a client that is not yet in Clients (HostGame precedes JoinGame) receive a reply.
		protected void SendTo(MessageWriter msg, int targetId, Player fallback)
		{
			lock (Clients)
			{
				Player player = fallback;
				for (int i = 0; i < Clients.Count; i++)
				{
					if (Clients[i].Id == targetId)
					{
						player = Clients[i];
						break;
					}
				}
				if (player == null)
				{
					return;
				}
				try
				{
					player.Connection.Send(msg);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		protected void Broadcast(MessageWriter msg, Player source)
		{
			lock (Clients)
			{
				for (int i = 0; i < Clients.Count; i++)
				{
					Player player = Clients[i];
					if (player != source)
					{
						try
						{
							player.Connection.Send(msg);
						}
						catch (Exception)
						{
						}
					}
				}
			}
		}

		// S2C JoinGame(1): i32 gameId, i32 clientId, i32 hostId, string name, platform msg, packed level, string productUserId, string friendCode.
		private void BroadcastJoinMessage(Player client, MessageWriter msg)
		{
			msg.Clear(SendOption.Reliable);
			msg.StartMessage(1);
			msg.Write(LocalGameId);
			msg.Write(client.Id);
			msg.Write(HostId);
			msg.Write(client.PlayerName);
			client.PlatformData.Serialize(msg);
			msg.WritePacked(1u);
			msg.Write(string.Empty);
			msg.Write(string.Empty);
			msg.EndMessage();
			Broadcast(msg, client);
		}

		// S2C JoinedGame(7): i32 gameId, i32 clientId, i32 hostId, packed n, n x {packed id, name, platform msg, packed level, puid, friendCode}.
		private void WriteJoinedMessage(Player client, MessageWriter msg, bool clear)
		{
			if (clear)
			{
				msg.Clear(SendOption.Reliable);
			}
			msg.StartMessage(7);
			msg.Write(LocalGameId);
			msg.Write(client.Id);
			msg.Write(HostId);
			msg.WritePacked(Clients.Count - 1 + FakePlayers.Count);
			for (int i = 0; i < Clients.Count; i++)
			{
				Player player = Clients[i];
				if (player != client)
				{
					msg.WritePacked(player.Id);
					msg.Write(player.PlayerName);
					player.PlatformData.Serialize(msg);
					msg.WritePacked(1u);
					msg.Write(string.Empty);
					msg.Write(string.Empty);
				}
			}
			for (int i = 0; i < FakePlayers.Count; i++)
			{
				Player fake = FakePlayers[i];
				msg.WritePacked(fake.Id);
				msg.Write(fake.PlayerName);
				fake.PlatformData.Serialize(msg);
				msg.WritePacked(fake.FakeLevel);
				msg.Write(fake.FakePuid);
				msg.Write(fake.FakeFriendCode);
			}
			msg.EndMessage();
		}

		// Fake (connectionless) clients created by the host's debug "Create character". They are announced to every real
		// client with a JoinGame message and listed in JoinedGame for late joiners so spawn messages owned by them resolve.
		protected readonly List<Player> FakePlayers = new List<Player>();

		public int AddFakeClient(string playerName, uint level, string productUserId, string friendCode)
		{
			Player fake = new Player(null, playerName, new PlatformSpecificData());
			fake.FakeLevel = level;
			fake.FakePuid = productUserId ?? string.Empty;
			fake.FakeFriendCode = friendCode ?? string.Empty;
			lock (Clients)
			{
				FakePlayers.Add(fake);
				MessageWriter msg = MessageWriter.Get(SendOption.Reliable);
				msg.StartMessage(1);
				msg.Write(LocalGameId);
				msg.Write(fake.Id);
				msg.Write(HostId);
				msg.Write(fake.PlayerName);
				fake.PlatformData.Serialize(msg);
				msg.WritePacked(fake.FakeLevel);
				msg.Write(fake.FakePuid);
				msg.Write(fake.FakeFriendCode);
				msg.EndMessage();
				Broadcast(msg, null);
				msg.Recycle();
			}
			return fake.Id;
		}

		public InnerNetServer()
		{
			ipBans = new HashSet<string>();
			Port = 22023;
			Clients = new List<Player>();
		}
	}
}
