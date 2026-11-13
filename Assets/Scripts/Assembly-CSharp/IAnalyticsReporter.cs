using System.Collections.Generic;
using AmongUs.GameOptions;

public interface IAnalyticsReporter
{
	void StartGame(NetworkedPlayerInfo localPlayer, int playerCount, int impostorCount, NetworkModes networkMode, MapNames map, GameModes gameMode, string roomCode, string serverName, IGameOptions options, List<NetworkedPlayerInfo> players);

	void EndGame(float durationInSeconds, GameOverReason endReason, List<NetworkedPlayerInfo> players);

	void VentUsed(NetworkedPlayerInfo player);

	void Kill(NetworkedPlayerInfo victim, NetworkedPlayerInfo killer);

	void SabotageStart(SystemTypes type);

	void SabotageFixed(SystemTypes type);

	void TaskCompleted(NetworkedPlayerInfo player, PlayerTask task);

	void MeetingStarted(NetworkedPlayerInfo callingPlayer, bool bodyReported);

	void MeetingEnded(float durationInSeconds, NetworkedPlayerInfo playerEjected);

	void MinigameOpened(NetworkedPlayerInfo callingPlayer, TaskTypes task);

	void MinigameClosed(NetworkedPlayerInfo callingPlayer, TaskTypes task, float durationInSeconds, bool isCompleted);
}
