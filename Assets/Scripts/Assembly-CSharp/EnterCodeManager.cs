using AmongUs.Data;
using InnerNet;
using TMPro;
using UnityEngine;

public class EnterCodeManager : MonoBehaviour, IConnectButton
{
	[SerializeField]
	private TextBoxTMP enterCodeField;

	[SerializeField]
	private GameObject enterCodeCheckmark;

	[SerializeField]
	private TextMeshPro hostText;

	[SerializeField]
	private TextMeshPro capacityText;

	[SerializeField]
	private GameObject crewmateIcon;

	[SerializeField]
	private TextMeshPro serverText;

	[SerializeField]
	private TextMeshPro chatText;

	[SerializeField]
	private PassiveButton joinGamePassiveButton;

	[SerializeField]
	private JoinGameButton joinGameButton;

	[SerializeField]
	private GameObject loadingIcon;

	[SerializeField]
	private Color availableColor;

	[SerializeField]
	private Color fullCapacityColor;

	[SerializeField]
	private AudioClip finishLoadSFX;

	[SerializeField]
	private GameObject fieldsContainer;

	private string matchmakerToken;

	private GameListing gameFound;

	private const float DEFAULT_CONTAINER_Y = 0f;

	private const float PS_CONTAINER_Y = 0.28f;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	private void OnEnable()
	{
		if ((bool)enterCodeField)
		{
			enterCodeField.ResetField();
		}
		SetGameFields(active: false);
		joinGamePassiveButton.SetButtonEnableState(enabled: false);
		ControllerManager.Instance.SetBackButton(BackButton);
		fieldsContainer.transform.localPosition = new Vector3(fieldsContainer.transform.localPosition.x, 0f, fieldsContainer.transform.localPosition.z);
	}

	private void OnDisable()
	{
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
	}

	public void LookForGame()
	{
		CheckPermissions();
	}

	private void ContinueLookForGame()
	{
		AmongUsClient.Instance.NetworkMode = NetworkModes.OnlineGame;
		if (!DestroyableSingleton<MatchMaker>.Instance.Connecting(this))
		{
			return;
		}
		SetGameFields(active: false);
		joinGamePassiveButton.SetButtonEnableState(enabled: false);
		int num = GameCode.GameNameToInt(enterCodeField.text);
		if (num == -1)
		{
			if (string.IsNullOrWhiteSpace(enterCodeField.text) && (bool)enterCodeField)
			{
				enterCodeField.ResetField();
			}
			StartCoroutine(Effects.SwayX(enterCodeField.transform));
			DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
		}
		else
		{
			StartCoroutine(AmongUsClient.Instance.CoFindGameInfoFromCode(num, FindGameResult));
		}
	}

	private void CheckPermissions()
	{
		AmongUsClient.Instance.CheckOnlinePermissions(ContinueLookForGame, NotOnlinePermissions, StartIcon, checkOnline: false);
	}

	private void NotOnlinePermissions()
	{
	}

	private void FindGameResult(HttpMatchmakerManager.FindGameByCodeResponse response, string mmToken)
	{
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
		if (response != null)
		{
			SoundManager.Instance.PlaySound(finishLoadSFX, loop: false);
			gameFound = response.Game;
			SetGameFields(active: true);
			hostText.text = BlockedWords.CensorWords(gameFound.TrueHostName);
			int num = gameFound.PlayerCount;
			if (num <= 0)
			{
				num = 1;
			}
			capacityText.text = $"{num}/{gameFound.MaxPlayers}";
			if (gameFound.PlayerCount >= gameFound.MaxPlayers)
			{
				capacityText.color = fullCapacityColor;
				joinGamePassiveButton.SetButtonEnableState(enabled: false);
			}
			else
			{
				capacityText.color = availableColor;
				joinGamePassiveButton.SetButtonEnableState(enabled: true);
			}
			if (response.Region != StringNames.NoTranslation)
			{
				serverText.text = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(response.Region, response.Region.ToString());
			}
			else
			{
				serverText.text = response.UntranslatedRegion;
			}
			matchmakerToken = mmToken;
			if (gameFound.QuickChat == QuickChatModes.FreeChatOrQuickChat)
			{
				chatText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeOrQuickChat);
				if (DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
				{
					DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
				}
			}
			else
			{
				chatText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QuickChat);
				DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.QuickChatOnly;
			}
		}
		else
		{
			Logger.GlobalInstance.Info("[EnterCodeManager] FindGameResult null response");
			SetGameFields(active: false);
			joinGamePassiveButton.SetButtonEnableState(enabled: false);
		}
	}

	public void ClickJoin()
	{
		if (DestroyableSingleton<MatchMaker>.Instance.Connecting(this))
		{
			StartCoroutine(AmongUsClient.Instance.CoJoinOnlineGameFromListing(gameFound, matchmakerToken));
		}
	}

	public void SetGameFields(bool active)
	{
		hostText.gameObject.SetActive(active);
		capacityText.gameObject.SetActive(active);
		crewmateIcon.gameObject.SetActive(active);
		serverText.gameObject.SetActive(active);
		chatText.gameObject.SetActive(active);
		enterCodeCheckmark.SetActive(active);
	}

	public void StartIcon()
	{
		if ((bool)loadingIcon)
		{
			loadingIcon.SetActive(value: true);
		}
	}

	public void StopIcon()
	{
		if ((bool)loadingIcon)
		{
			loadingIcon.SetActive(value: false);
		}
	}
}
