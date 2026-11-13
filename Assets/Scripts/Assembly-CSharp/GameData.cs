using System.Collections.Generic;
using System.Linq;
using InnerNet;
using UnityEngine;

public class GameData : MonoBehaviour, IDisconnectHandler
{
	public static GameData Instance;

	public static float TimeGameStarted;

	public static float TimeLastMeetingStarted;

	public static int MeetingCount;

	public static int RoundsPlayedInSession;

	public static DeathReason LastDeathReason;

	public List<NetworkedPlayerInfo> AllPlayers = new List<NetworkedPlayerInfo>();

	private List<NetworkedPlayerInfo> PlayerQueue = new List<NetworkedPlayerInfo>();

	public int TotalTasks;

	public int CompletedTasks;

	public RoleBehaviour DefaultRole;

	public NetworkedPlayerInfo PlayerInfoPrefab;

	public const byte InvalidPlayerId = byte.MaxValue;

	public const byte DisconnectedPlayerId = 254;

	public int PlayerCount => AllPlayers.Count;

	public bool IsPersistent => true;

	public static void DestroyInstance()
	{
		if (Instance != null)
		{
			Object.Destroy(Instance.gameObject);
		}
	}

	public static void OnMeetingStart()
	{
		MeetingCount++;
	}

	public static void OnGameEnd()
	{
		RoundsPlayedInSession++;
		MeetingCount = 0;
		EndGameResult.ResetCachedValues();
	}

	public static void OnDisconnected()
	{
		RoundsPlayedInSession = 0;
	}

	public void Awake()
	{
		if ((bool)Instance && Instance != this)
		{
			Debug.Log("Destroying dupe GameData");
			Object.Destroy(gameObject);
			return;
		}
		Instance = this;
		if ((bool)AmongUsClient.Instance)
		{
			AmongUsClient.Instance.DisconnectHandlers.AddUnique(this);
		}
	}

	public NetworkedPlayerInfo GetHost()
	{
		ClientData host = AmongUsClient.Instance.GetHost();
		if (host != null && (bool)host.Character)
		{
			return host.Character.Data;
		}
		return null;
	}

	public bool HasPlayer(ClientData client)
	{
		return GetPlayerByClient(client) != null;
	}

	public sbyte GetPlayerIdFromClient(ClientData client)
	{
		NetworkedPlayerInfo playerByClient = GetPlayerByClient(client);
		if (playerByClient == null)
		{
			return -1;
		}
		return (sbyte)playerByClient.PlayerId;
	}

	public sbyte GetAvailableId()
	{
		sbyte i;
		for (i = 0; i < sbyte.MaxValue; i++)
		{
			if (!AllPlayers.Any((NetworkedPlayerInfo p) => p.PlayerId == i) && !PlayerQueue.Any((NetworkedPlayerInfo p) => p.PlayerId == i))
			{
				return i;
			}
		}
		return -1;
	}

	public NetworkedPlayerInfo GetPlayerByClient(ClientData client)
	{
		for (int i = 0; i < AllPlayers.Count; i++)
		{
			if (AllPlayers[i].ClientId == client.Id)
			{
				return AllPlayers[i];
			}
		}
		return null;
	}

	public NetworkedPlayerInfo GetPlayerById(byte id)
	{
		if (id == byte.MaxValue)
		{
			return null;
		}
		for (int i = 0; i < AllPlayers.Count; i++)
		{
			if (AllPlayers[i].PlayerId == id)
			{
				return AllPlayers[i];
			}
		}
		return null;
	}

	public NetworkedPlayerInfo AddDummy(PlayerControl pc)
	{
		NetworkedPlayerInfo networkedPlayerInfo = Object.Instantiate(PlayerInfoPrefab);
		networkedPlayerInfo.Init(pc, -1);
		networkedPlayerInfo.Role = Object.Instantiate(DefaultRole);
		networkedPlayerInfo.Role.Initialize(pc);
		AllPlayers.Add(networkedPlayerInfo);
		return networkedPlayerInfo;
	}

	public NetworkedPlayerInfo AddPlayer(PlayerControl pc, ClientData client)
	{
		NetworkedPlayerInfo networkedPlayerInfo = Object.Instantiate(PlayerInfoPrefab);
		networkedPlayerInfo.Init(pc, client.Id);
		networkedPlayerInfo.Role = Object.Instantiate(DefaultRole);
		networkedPlayerInfo.Role.Initialize(pc);
		AddPlayerInfo(networkedPlayerInfo);
		return networkedPlayerInfo;
	}

	public bool IsProcessingInfo(NetworkedPlayerInfo info)
	{
		return PlayerQueue.Contains(info);
	}

	public void DirtyAllData()
	{
		foreach (NetworkedPlayerInfo allPlayer in AllPlayers)
		{
			allPlayer.MarkDirty();
		}
	}

	public void AddPlayerInfo(NetworkedPlayerInfo info)
	{
		PlayerQueue.Add(info);
		info.CensorNameAsync(() =>
		{
			PlayerQueue.Remove(info);
			AllPlayers.Add(info);
			DestroyableSingleton<FriendsListManager>.Instance.AddRecentlyPlayed(info);
		});
	}

	public bool RemovePlayer(byte playerId)
	{
		for (int i = 0; i < AllPlayers.Count; i++)
		{
			NetworkedPlayerInfo networkedPlayerInfo = AllPlayers[i];
			if (networkedPlayerInfo.PlayerId == playerId)
			{
				AllPlayers.RemoveAt(i);
				Object.Destroy(networkedPlayerInfo.gameObject);
				return true;
			}
		}
		return false;
	}

	public void RecomputeTaskCounts()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		TotalTasks = 0;
		CompletedTasks = 0;
		for (int i = 0; i < AllPlayers.Count; i++)
		{
			NetworkedPlayerInfo networkedPlayerInfo = AllPlayers[i];
			if (networkedPlayerInfo.Disconnected || networkedPlayerInfo.Tasks == null || !networkedPlayerInfo.Object || (!GameManager.Instance.LogicOptions.GetGhostsDoTasks() && networkedPlayerInfo.IsDead) || !networkedPlayerInfo.Role || !networkedPlayerInfo.Role.TasksCountTowardProgress)
			{
				continue;
			}
			for (int j = 0; j < networkedPlayerInfo.Tasks.Count; j++)
			{
				TotalTasks++;
				if (networkedPlayerInfo.Tasks[j].Complete)
				{
					CompletedTasks++;
				}
			}
		}
	}

	public void TutOnlyRemoveTask(byte playerId, uint taskId)
	{
		NetworkedPlayerInfo playerById = GetPlayerById(playerId);
		NetworkedPlayerInfo.TaskInfo item = playerById.FindTaskById(taskId);
		playerById.Tasks.Remove(item);
		RecomputeTaskCounts();
	}

	public uint TutOnlyAddTask(byte playerId)
	{
		NetworkedPlayerInfo playerById = GetPlayerById(playerId);
		uint num = playerById.Tasks.Select((NetworkedPlayerInfo.TaskInfo d) => d.Id).Max() + 1;
		playerById.Tasks.Add(new NetworkedPlayerInfo.TaskInfo
		{
			Id = num
		});
		TotalTasks++;
		return num;
	}

	public void CompleteTask(PlayerControl pc, uint taskId)
	{
		NetworkedPlayerInfo.TaskInfo taskInfo = pc.Data.FindTaskById(taskId);
		if (taskInfo != null)
		{
			if (!taskInfo.Complete)
			{
				taskInfo.Complete = true;
				CompletedTasks++;
			}
			else
			{
				Debug.LogWarning("Double complete task: " + taskId);
			}
		}
		else
		{
			Debug.LogWarning("Couldn't find task: " + taskId);
		}
	}

	public void RemoveDisconnectedPlayers()
	{
		for (int num = AllPlayers.Count - 1; num >= 0; num--)
		{
			if (AllPlayers[num] == null)
			{
				AllPlayers.RemoveAt(num);
			}
			else if (AllPlayers[num].Disconnected)
			{
				NetworkedPlayerInfo networkedPlayerInfo = AllPlayers[num];
				AllPlayers.RemoveAt(num);
				Object.Destroy(networkedPlayerInfo.gameObject);
			}
		}
	}

	public void HandleDisconnect(PlayerControl player, DisconnectReasons reason)
	{
		if (!player)
		{
			return;
		}
		NetworkedPlayerInfo playerById = GetPlayerById(player.PlayerId);
		if (playerById == null)
		{
			return;
		}
		if (AmongUsClient.Instance.IsGameStarted)
		{
			if (!playerById.Disconnected)
			{
				playerById.Disconnected = true;
				if (playerById.Role == null)
				{
					playerById.Role = Object.Instantiate(Instance.DefaultRole, playerById.gameObject.transform);
				}
				LastDeathReason = DeathReason.Disconnect;
				ShowNotification(playerById.PlayerName, reason);
			}
		}
		else if (RemovePlayer(player.PlayerId))
		{
			ShowNotification(playerById.PlayerName, reason);
		}
		RecomputeTaskCounts();
	}

	private void ShowNotification(string playerName, DisconnectReasons reason)
	{
		if (string.IsNullOrEmpty(playerName))
		{
			return;
		}
		if (reason <= DisconnectReasons.Banned)
		{
			switch (reason)
			{
			case DisconnectReasons.ExitGame:
				DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.UserLeftGame, playerName));
				return;
			case DisconnectReasons.Banned:
			{
				NetworkedPlayerInfo data = AmongUsClient.Instance.GetHost().Character.Data;
				DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PlayerWasBannedBy, playerName, data.PlayerName));
				return;
			}
			}
		}
		else
		{
			if (reason == DisconnectReasons.Kicked)
			{
				NetworkedPlayerInfo data2 = AmongUsClient.Instance.GetHost().Character.Data;
				DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PlayerWasKickedBy, playerName, data2.PlayerName));
				return;
			}
			_ = 17;
		}
		DestroyableSingleton<HudManager>.Instance.Notifier.AddDisconnectMessage(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LeftGameError, playerName));
	}

	public void HandleDisconnect()
	{
		if (AmongUsClient.Instance.IsGameStarted)
		{
			return;
		}
		for (int num = AllPlayers.Count - 1; num >= 0; num--)
		{
			NetworkedPlayerInfo networkedPlayerInfo = AllPlayers[num];
			if (AmongUsClient.Instance.GetClient(networkedPlayerInfo.ClientId) == null)
			{
				AllPlayers.RemoveAt(num);
				Object.Destroy(networkedPlayerInfo.gameObject);
			}
		}
	}
}
