using System.Collections.Generic;
using AmongUs.GameOptions;

public class DummyAnalytics : IAnalyticsReporter
{
	public void StartGame(NetworkedPlayerInfo localPlayer, int playerCount, int impostorCount, NetworkModes networkMode, MapNames map, GameModes gameMode, string roomCode, string serverName, IGameOptions options, List<NetworkedPlayerInfo> players)
	{
	}

	public void EndGame(float durationInSeconds, GameOverReason endReason, List<NetworkedPlayerInfo> players)
	{
	}

	public void VentUsed(NetworkedPlayerInfo player)
	{
	}

	public void Kill(NetworkedPlayerInfo victim, NetworkedPlayerInfo killer)
	{
	}

	public void SabotageStart(SystemTypes type)
	{
	}

	public void SabotageFixed(SystemTypes type)
	{
	}

	public void TaskCompleted(NetworkedPlayerInfo player, PlayerTask task)
	{
	}

	public void MeetingStarted(NetworkedPlayerInfo callingPlayer, bool bodyReported)
	{
	}

	public void MeetingEnded(float durationInSeconds, NetworkedPlayerInfo playerEjected)
	{
	}

	public void MinigameOpened(NetworkedPlayerInfo callingPlayer, TaskTypes task)
	{
	}

	public void MinigameClosed(NetworkedPlayerInfo callingPlayer, TaskTypes task, float durationInSeconds, bool isCompleted)
	{
	}
}
