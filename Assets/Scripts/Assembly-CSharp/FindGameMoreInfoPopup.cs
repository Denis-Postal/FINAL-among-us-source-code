using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.GameOptions;
using InnerNet;
using TMPro;
using UnityEngine;

public class FindGameMoreInfoPopup : MonoBehaviour, IConnectButton
{
	[SerializeField]
	private TextMeshPro modeText;

	[SerializeField]
	private SpriteRenderer mapBackground;

	[SerializeField]
	private SpriteRenderer mapLogo;

	[SerializeField]
	private Sprite[] mapBackgroundSprites;

	[SerializeField]
	private Sprite[] mapLogoSprites;

	[SerializeField]
	private TextMeshPro capacity;

	[SerializeField]
	private GameObject classicFilters;

	[SerializeField]
	private GameObject hnsFilters;

	[SerializeField]
	private TextMeshPro impostorsText;

	[SerializeField]
	private TextMeshPro rolesText;

	[SerializeField]
	private TextMeshPro confirmEjectsText;

	[SerializeField]
	private TextMeshPro visualTaskText;

	[SerializeField]
	private TextMeshPro meetingTimeText;

	[SerializeField]
	private TextMeshPro anonVotesText;

	[SerializeField]
	private TextMeshPro killcooldownText;

	[SerializeField]
	private TextMeshPro playerSpeedText;

	[SerializeField]
	private TextMeshPro tagText;

	[SerializeField]
	private TextMeshPro flashlightText;

	[SerializeField]
	private TextMeshPro hidingTimeText;

	[SerializeField]
	private TextMeshPro finalHideText;

	[SerializeField]
	private TextMeshPro playerSpeedHnSText;

	[SerializeField]
	private TextMeshPro maxVentUsesText;

	[SerializeField]
	private TextMeshPro chatTypeText;

	[SerializeField]
	private TextMeshPro regionText;

	[SerializeField]
	private TextMeshPro languageText;

	private bool animating = true;

	private GameListing gameListing;

	private const float WAIT_SLIDE_INTRO = 0.3f;

	private const float Y_POS_OFFSET = -9f;

	[Header("Console Controller Navigation")]
	[SerializeField]
	private UiElement BackButton;

	[SerializeField]
	private UiElement defaultButtonSelected;

	private List<UiElement> controllerSelectable;

	public void SetupInfo(GameListing gameL)
	{
		gameListing = gameL;
		int mapId = gameListing.MapId;
		mapBackground.sprite = mapBackgroundSprites[0];
		mapLogo.sprite = mapLogoSprites[mapId];
		if (mapId == 0 && AprilFoolsMode.ShouldFlipSkeld())
		{
			mapLogo.flipX = true;
		}
		else
		{
			mapLogo.flipX = false;
		}
		capacity.text = $"{gameListing.PlayerCount}/{gameListing.MaxPlayers}";
		switch (gameListing.Options.GameMode)
		{
		case GameModes.Normal:
		case GameModes.NormalFools:
		{
			modeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeClassic);
			classicFilters.SetActive(value: true);
			hnsFilters.SetActive(value: false);
			impostorsText.text = gameL.NumImpostors.ToString();
			gameL.Options.TryGetBool(BoolOptionNames.Roles, out var value5);
			rolesText.text = GetStringSetting(value5);
			gameL.Options.TryGetBool(BoolOptionNames.ConfirmImpostor, out var value6);
			confirmEjectsText.text = GetStringSetting(value6);
			gameL.Options.TryGetBool(BoolOptionNames.VisualTasks, out var value7);
			visualTaskText.text = GetStringSetting(value7);
			gameL.Options.TryGetInt(Int32OptionNames.VotingTime, out var value8);
			meetingTimeText.text = value8 + "s";
			gameL.Options.TryGetBool(BoolOptionNames.AnonymousVotes, out var value9);
			anonVotesText.text = GetStringSetting(value9);
			gameL.Options.TryGetFloat(FloatOptionNames.KillCooldown, out var value10);
			killcooldownText.text = (int)value10 + "s";
			break;
		}
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
		{
			modeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeHideAndSeek);
			classicFilters.SetActive(value: false);
			hnsFilters.SetActive(value: true);
			gameL.Options.TryGetBool(BoolOptionNames.UseFlashlight, out var value);
			flashlightText.text = GetStringSetting(value);
			gameL.Options.TryGetFloat(FloatOptionNames.EscapeTime, out var value2);
			hidingTimeText.text = value2 + "s";
			gameL.Options.TryGetFloat(FloatOptionNames.FinalEscapeTime, out var value3);
			finalHideText.text = value3 + "s";
			gameL.Options.TryGetInt(Int32OptionNames.CrewmateVentUses, out var value4);
			maxVentUsesText.text = value4.ToString();
			break;
		}
		}
		switch (gameListing.QuickChat)
		{
		case QuickChatModes.FreeChatOrQuickChat:
			chatTypeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeOrQuickChat);
			break;
		case QuickChatModes.QuickChatOnly:
			chatTypeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QuickChat);
			break;
		}
		bool flag = false;
		foreach (KeyValuePair<string, uint> language in ChatLanguageSet.Instance.Languages)
		{
			if (language.Value == gameListing.Language)
			{
				languageText.text = language.Key;
				flag = true;
			}
		}
		if (!flag)
		{
			languageText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OtherLanguage);
		}
		IRegionInfo currentRegion = DestroyableSingleton<ServerManager>.Instance.CurrentRegion;
		regionText.text = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(currentRegion.TranslateName, currentRegion.Name);
		gameL.Options.TryGetFloat(FloatOptionNames.PlayerSpeedMod, out var value11);
		playerSpeedText.text = value11 + "x";
		playerSpeedHnSText.text = value11 + "x";
		gameL.Options.TryGetInt(Int32OptionNames.Tag, out var value12);
		tagText.text = GetLevelString(value12);
	}

	public string GetLevelString(int tag)
	{
		return tag switch
		{
			1 => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterBeginner), 
			2 => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterIntermediate), 
			3 => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FilterExpert), 
			_ => DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NoneLabel), 
		};
	}

	private string GetStringSetting(bool value)
	{
		if (value)
		{
			return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOn);
		}
		return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOff);
	}

	private void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, defaultButtonSelected, controllerSelectable);
		StartCoroutine(CoShow());
	}

	private IEnumerator CoShow()
	{
		animating = true;
		Vector2 source = new Vector2(transform.localPosition.x, -9f);
		Vector2 dest = new Vector2(transform.localPosition.x, 0f);
		transform.localPosition = new Vector3(source.x, source.y, transform.localPosition.z);
		yield return Effects.Slide2D(transform, source, dest, 0.3f);
		animating = false;
	}

	public void Close()
	{
		if (!animating)
		{
			ControllerManager.Instance.CloseOverlayMenu(name);
			gameObject.SetActive(value: false);
		}
	}

	public void OnClick()
	{
		if (!DestroyableSingleton<MatchMaker>.Instance.Connecting(this) || gameListing.IP == 0)
		{
			return;
		}
		if (gameListing.QuickChat == QuickChatModes.FreeChatOrQuickChat)
		{
			if (DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
			{
				DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
			}
		}
		else
		{
			DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.QuickChatOnly;
		}
		Logger.GlobalInstance.Info("[FindGameMoreInfoPopup Connecting to: " + InnerNetClient.AddressToString(gameListing.IP) + " for " + GameCode.IntToGameName(gameListing.GameId));
		StartCoroutine(AmongUsClient.Instance.CoJoinOnlinePublicGame(gameListing.GameId, gameListing.IPString, gameListing.Port, AmongUsClient.MainMenuTarget.FindGameMenu));
	}

	public void StartIcon()
	{
	}

	public void StopIcon()
	{
	}
}
