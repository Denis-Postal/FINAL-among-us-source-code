using InnerNet;

public interface IGameListHandler
{
	void HandleList(InnerNetClient.TotalGameData totalGames, HttpMatchmakerManager.FindGamesListFilteredResponse availableGames);
}
