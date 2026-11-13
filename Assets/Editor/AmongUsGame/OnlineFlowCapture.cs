#if UNITY_EDITOR
// Loopback regression for the native HostGame -> JoinGame -> lobby transition.
// Exercises a signed six-letter ID without HTTP, EOS, or community-server traffic.
using System;
using System.Collections;
using System.Net;
using System.Reflection;
using Hazel;
using Hazel.Udp;
using InnerNet;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AmongUsGameTools
{
	public static class OnlineFlowCapture
	{
		static UdpConnectionListener listener;
		static Connection peer;
		static AmongUsClient testedClient;
		static int gameId;
		static bool hostRequested, joinRequested;
		static bool allocatedInfo;
		static double started;
		static double nextState;
		static readonly BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;

		public static bool Run(string action)
		{
			if (action == "online42popup")
			{
				EnsureGameOptions();
				UnityEngine.Object.FindFirstObjectByType<MainMenuManager>().OpenCreateGame();
				return true;
			}
			if (action == "online42confirm")
			{
				UnityEngine.Object.FindFirstObjectByType<CreateGameOptions>().OpenConfirmPopup();
				Debug.Log("[OnlineFlowCapture] confirmation opened locally");
				return true;
			}
			if (action != "online42loopback") return false;
			EnsureGameOptions();
			if (listener != null) throw new InvalidOperationException("Loopback fixture already running");
			gameId = GameCode.GameNameToInt("PRNFBK");
			if (gameId >= 0) throw new InvalidOperationException("Expected a signed six-letter game ID");
			listener = new UdpConnectionListener(new IPEndPoint(IPAddress.Loopback, 22142), IPMode.IPv4, new UnityLogger());
			listener.NewConnection += evt =>
			{
				evt.HandshakeData.Recycle();
				peer = evt.Connection;
				peer.DataReceived += Receive;
			};
			listener.Start();
			EditorApplication.playModeStateChanged += Cleanup;
			EditorApplication.quitting += Dispose;
			var client = AmongUsClient.Instance;
			testedClient = client;
			var eos = DestroyableSingleton<EOSManager>.Instance;
			string puid = eos.ProductUserId;
			System.Threading.Tasks.Task.Run(() =>
			{
				try
				{
					if (string.IsNullOrEmpty(puid) || puid.Length != 32 || eos.ProductUserId != puid) throw new InvalidOperationException("Local ID changed on the receive thread");
					Debug.Log("[OnlineFlowCapture] PASS local ID worker read");
				}
				catch (Exception ex) { Debug.LogException(ex); }
			});
			client.NetworkMode = NetworkModes.LocalGame; // no online nonce/auth; fixture accepts the local handshake
			client.OnlineScene = ""; // proves CoConnectToGameServer restores the lobby scene name
			client.MainMenuScene = "";
			started = EditorApplication.timeSinceStartup;
			nextState = started + 5;
			var method = typeof(AmongUsClient).GetMethod("CoConnectToGameServer", Private);
			client.StartCoroutine((IEnumerator)method.Invoke(client, new object[] { MatchMakerModes.HostAndClient, "127.0.0.1", (ushort)22142, null }));
			EditorApplication.update += Check;
			Debug.Log("[OnlineFlowCapture] loopback start gameId=" + gameId + " enabled=" + client.enabled + " active=" + client.gameObject.activeInHierarchy + " mode=" + client.mode + " state=" + client.GameState);
			return true;
		}

		static void EnsureGameOptions()
		{
			// Direct MainMenu editor captures bypass the creator in SplashIntro.
			if (GameOptionsManager.Instance == null) new GameOptionsManager(new UnityLogger());
			if (GameOptionsManager.Instance.GameHostOptions == null) GameOptionsManager.Instance.Initialize();
		}

		static void Receive(DataReceivedEventArgs evt)
		{
			try
			{
				while (evt.Message.Position < evt.Message.Length)
				{
					var msg = evt.Message.ReadMessage();
					if (msg.Tag == Tags.HostGame || msg.Tag == Tags.HostModdedGame)
					{
						hostRequested = true;
						// The loopback handshake has completed. Use the online lobby arm so it never starts LAN discovery.
						// This only writes a managed enum; Unity APIs are left to the main thread.
						testedClient.NetworkMode = NetworkModes.OnlineGame;
						Reply(Tags.HostGame, writer => writer.Write(gameId));
					}
					else if (msg.Tag == Tags.JoinGame)
					{
						int requested = msg.ReadInt32();
						if (requested != gameId) throw new InvalidOperationException("Join used a different game ID");
						joinRequested = true;
						Reply(Tags.JoinedGame, writer =>
						{
							writer.Write(gameId);
							writer.Write(2); // client id
							writer.Write(2); // host id
							writer.WritePacked(0); // no other players
						});
						Debug.Log("[OnlineFlowCapture] sent JoinedGame");
					}
				}
			}
			finally { evt.Message.Recycle(); }
		}

		static void Reply(byte tag, Action<MessageWriter> body)
		{
			var writer = MessageWriter.Get(SendOption.Reliable);
			try
			{
				writer.StartMessage(tag);
				body(writer);
				writer.EndMessage();
				peer.Send(writer);
			}
			finally { writer.Recycle(); }
		}

		static void Check()
		{
			var client = AmongUsClient.Instance;
			if (!allocatedInfo && client && client.AmHost && GameData.Instance && SceneManager.GetActiveScene().name == "OnlineGame")
			{
				// A modern online backend allocates NetworkedPlayerInfo before the host creates its character.
				// This minimal loopback fixture supplies that allocation locally; it does not validate live backend packets.
				var data = client.GetClient(client.ClientId);
				var info = UnityEngine.Object.Instantiate(GameData.Instance.PlayerInfoPrefab);
				info.PlayerId = 0;
				info.ClientId = client.ClientId;
				info.Puid = data.ProductUserId;
				info.FriendCode = data.FriendCode;
				info.PlayerName = data.PlayerName;
				info.Role = UnityEngine.Object.Instantiate(GameData.Instance.DefaultRole);
				GameData.Instance.AddPlayerInfo(info);
				allocatedInfo = true;
				Debug.Log("[OnlineFlowCapture] fixture allocated server player info");
			}
			if (client && EditorApplication.timeSinceStartup >= nextState)
			{
				Debug.Log("[OnlineFlowCapture] state id=" + client.ClientId + " host=" + client.HostId + " game=" + client.GameId + " state=" + client.GameState + " enabled=" + client.enabled + " sceneTarget=" + client.OnlineScene + " scene=" + SceneManager.GetActiveScene().name);
				nextState += 5;
			}
			if (client && SceneManager.GetActiveScene().name == "OnlineGame" && PlayerControl.LocalPlayer && LobbyBehaviour.Instance)
			{
				Debug.Log("[OnlineFlowCapture] PASS host=" + hostRequested + " join=" + joinRequested + " gameId=" + client.GameId + " client=" + client.ClientId + " scene=OnlineGame player=" + PlayerControl.LocalPlayer.name + " elapsed=" + (EditorApplication.timeSinceStartup - started).ToString("F2"));
				EditorApplication.update -= Check;
			}
			else if (EditorApplication.timeSinceStartup - started > 30)
			{
				Debug.LogError("[OnlineFlowCapture] FAIL host=" + hostRequested + " join=" + joinRequested + " scene=" + SceneManager.GetActiveScene().name);
				EditorApplication.update -= Check;
			}
		}

		static void Cleanup(PlayModeStateChange state) { if (state == PlayModeStateChange.ExitingPlayMode) Dispose(); }
		static void Dispose()
		{
			EditorApplication.update -= Check;
			EditorApplication.playModeStateChanged -= Cleanup;
			EditorApplication.quitting -= Dispose;
			listener?.Dispose();
			listener = null;
			peer = null;
		}
	}
}
#endif
