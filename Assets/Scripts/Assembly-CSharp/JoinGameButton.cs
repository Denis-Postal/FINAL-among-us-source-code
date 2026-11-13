using System.Collections;
using AmongUs.Data;
using InnerNet;
using PowerTools;
using TMPro;
using UnityEngine;

public class JoinGameButton : MonoBehaviour, IConnectButton
{
	public AudioClip IntroMusic;

	public TextBoxTMP GameIdText;

	public TextMeshPro gameNameText;

	public float timeRecieved;

	public SpriteRenderer FillScreen;

	public SpriteAnim connectIcon;

	public AnimationClip connectClip;

	public NetworkModes NetworkMode;

	public string netAddress;

	private void Start()
	{
		if (!(GameIdText == null))
		{
			GameIdText.Hidden = DataManager.Settings.Gameplay.StreamerMode;
		}
	}

	public void OnClick()
	{
		if (!string.IsNullOrWhiteSpace(netAddress) && (!NameTextBehaviour.Instance || !NameTextBehaviour.Instance.ShakeIfInvalid()))
		{
			AmongUsClient.Instance.CheckOnlinePermissions(ContinueOnClick, NotOnlinePermissions, StartIcon, checkOnline: false);
		}
	}

	private void NotOnlinePermissions()
	{
	}

	private void ContinueOnClick()
	{
		DataManager.Settings.Save();
		if (!DestroyableSingleton<MatchMaker>.Instance.Connecting(this))
		{
			return;
		}
		AmongUsClient.Instance.NetworkMode = NetworkMode;
		if (NetworkMode == NetworkModes.OnlineGame)
		{
			int num = GameCode.GameNameToInt(GameIdText.text);
			if (num == -1)
			{
				if (string.IsNullOrWhiteSpace(GameIdText.text))
				{
					TextTranslatorTMP component = gameNameText.GetComponent<TextTranslatorTMP>();
					if ((bool)component)
					{
						component.ResetText();
					}
				}
				StartCoroutine(Effects.SwayX(GameIdText.transform));
				DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
			}
			else
			{
				StartCoroutine(AmongUsClient.Instance.CoJoinOnlineGameFromCode(num, fromEnterCode: true));
			}
		}
		else
		{
			AmongUsClient.Instance.SetEndpoint(netAddress, 22023, dtls: false);
			AmongUsClient.Instance.GameId = InnerNet.InnerNetServer.LegacyLocalGameId; // 32: what the original/official local games use (our server also accepts it and then hands out "LOCL")
			AmongUsClient.Instance.NetworkMode = NetworkModes.LocalGame;
			AmongUsClient.Instance.MainMenuScene = "MatchMaking";
			StartCoroutine(JoinLocalGame());
		}
	}

	private IEnumerator JoinLocalGame()
	{
		if ((bool)FillScreen)
		{
			SoundManager.Instance.CrossFadeSound("MainBG", null, 0.5f);
			FillScreen.gameObject.SetActive(value: true);
			for (float time = 0f; time < 0.25f; time += Time.deltaTime)
			{
				FillScreen.color = Color.Lerp(Color.clear, Color.black, time / 0.25f);
				yield return null;
			}
			FillScreen.color = Color.black;
		}
		AmongUsClient.Instance.OnlineScene = "OnlineGame";
		AmongUsClient.Instance.Connect(MatchMakerModes.Client, null);
		yield return AmongUsClient.Instance.WaitForConnectionOrFail();
		Logger.GlobalInstance.Info($"Attempted to join local game and got mode {AmongUsClient.Instance.mode}");
		if (AmongUsClient.Instance.mode != MatchMakerModes.None)
		{
			yield break;
		}
		if ((bool)FillScreen)
		{
			SoundManager.Instance.CrossFadeSound("MainBG", IntroMusic, 0.5f);
			for (float time = 0f; time < 0.25f; time += Time.deltaTime)
			{
				FillScreen.color = Color.Lerp(Color.black, Color.clear, time / 0.25f);
				yield return null;
			}
			FillScreen.color = Color.clear;
		}
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
	}

	public void SetGameName(string[] gameNameParts)
	{
		int num = 15;
		gameNameText.text = $"{gameNameParts[0]} ({gameNameParts[2]}/{num})";
	}

	public void StartIcon()
	{
		if ((bool)connectIcon)
		{
			connectIcon.Play(connectClip);
		}
	}

	public void StopIcon()
	{
		if ((bool)connectIcon)
		{
			connectIcon.Stop();
			connectIcon.GetComponent<SpriteRenderer>().sprite = null;
		}
	}
}
