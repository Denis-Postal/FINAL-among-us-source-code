using System;
using System.Collections;
using AmongUs.Data;
using AmongUs.GameOptions;
using InnerNet;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-20000)]
public class PSManager : DestroyableSingleton<PSManager>
{
	public Texture2D ps4SessionImage;

	[NonSerialized]
	public byte[] ps4SessionImageAsPNG;

	public JoinSessionSource joinSessionSource;

	public bool gameplayStarted;

	private bool keepBlockerActive;

	public string GetSessionName()
	{
		string text = ((GameOptionsManager.Instance.CurrentGameOptions.MapId == 0 && AprilFoolsMode.ShouldFlipSkeld()) ? "Dleks" : Constants.MapNames[GameOptionsManager.Instance.CurrentGameOptions.MapId]);
		string text2 = "";
		if ((bool)GameData.Instance)
		{
			text2 = ((AmongUsClient.Instance.NetworkMode != NetworkModes.LocalGame) ? $"{GameData.Instance.PlayerCount}/{GameOptionsManager.Instance.CurrentGameOptions.MaxPlayers}" : $"{GameData.Instance.PlayerCount}/{15}");
		}
		else
		{
			text2 = ((AmongUsClient.Instance.NetworkMode != NetworkModes.LocalGame) ? $"0/{GameOptionsManager.Instance.CurrentGameOptions.MaxPlayers}" : $"0/{15}");
		}
		string text3 = ((!LobbyBehaviour.Instance) ? "In-Game" : "Lobby");
		return text3 + " - " + text2 + " - " + text;
	}

	public void JoinGameFromCode(string gameCode)
	{
		int gameCode2 = GameCode.GameNameToInt(gameCode);
		StartCoroutine(JoinGame(gameCode2));
	}

	public void JoinGameFromCode(int gameCode)
	{
		StartCoroutine(JoinGame(gameCode));
	}

	public IEnumerator JoinGame(int gameCode)
	{
		while (!AmongUsClient.Instance)
		{
			yield return null;
		}
		while (!DestroyableSingleton<EOSManager>.Instance.HasFinishedLoginFlow())
		{
			yield return null;
		}
		if (AmongUsClient.Instance.mode != MatchMakerModes.None)
		{
			if (AmongUsClient.Instance.GameId == gameCode)
			{
				Debug.LogError("HandleEvents::OnHandleInvite: Already in the game we're trying to join");
				yield break;
			}
			Debug.LogError("HandleEvents::OnHandleInvite: Already Connected");
			AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
		}
		DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.SetActive(value: true);
		yield return AmongUsClient.Instance.CoFindGameInfoFromCodeAndJoin(gameCode);
		DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.SetActive(value: false);
		if (AmongUsClient.Instance.ClientId < 0)
		{
			Debug.LogError("HandleEvents::JoinGame: ClientId < 0");
			SceneManager.LoadScene("MainMenu");
		}
		Debug.LogError($"HandleEvents::JoinGame: ClientID = {AmongUsClient.Instance.ClientId}");
	}

	public void CreateGame(GameModes gameMode)
	{
		StartCoroutine(CreateGameCo(gameMode));
	}

	private IEnumerator CreateGameCo(GameModes gameMode)
	{
		while (!AmongUsClient.Instance)
		{
			yield return null;
		}
		while (!DestroyableSingleton<EOSManager>.Instance.HasFinishedLoginFlow())
		{
			yield return null;
		}
		DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.SetActive(value: true);
		keepBlockerActive = true;
		StartCoroutine(KeepLoadBlockerActive());
		if (AmongUsClient.Instance.mode != MatchMakerModes.None)
		{
			AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
		}
		yield return new WaitForSeconds(1f);
		DataManager.Settings.Multiplayer.LastPlayedGameMode = gameMode;
		yield return AmongUsClient.Instance.CoCreateOnlineGame();
		DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.SetActive(value: false);
		keepBlockerActive = false;
		if (AmongUsClient.Instance.ClientId < 0)
		{
			Debug.LogError("HandleEvents::CreateGame: ClientId < 0");
			SceneManager.LoadScene("MainMenu");
		}
		Debug.LogError($"HandleEvents::CreateGame: ClientID = {AmongUsClient.Instance.ClientId}");
	}

	private IEnumerator KeepLoadBlockerActive()
	{
		while (keepBlockerActive)
		{
			if (!DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.activeSelf)
			{
				DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.SetActive(value: true);
			}
			yield return null;
		}
		DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.SetActive(value: false);
	}
}
