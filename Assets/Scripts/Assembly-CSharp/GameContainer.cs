using System;
using AmongUs.Data;
using AmongUs.GameOptions;
using InnerNet;
using TMPro;
using UnityEngine;

public class GameContainer : MonoBehaviour, IConnectButton
{
	[SerializeField]
	private TextMeshPro tag1;

	[SerializeField]
	private TextMeshPro tag2;

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

	public Action<GameListing> OnMoreAction;

	private GameListing gameListing;

	public void SetGameListing(GameListing gameL)
	{
		gameListing = gameL;
	}

	public void SetupGameInfo()
	{
		capacity.text = $"{gameListing.PlayerCount}/{gameListing.MaxPlayers}";
		int mapId = gameListing.MapId;
		mapBackground.sprite = mapBackgroundSprites[mapId];
		mapLogo.sprite = mapLogoSprites[mapId];
		if (mapId == 0 && AprilFoolsMode.ShouldFlipSkeld())
		{
			mapLogo.flipX = true;
		}
		else
		{
			mapLogo.flipX = false;
		}
		if (gameListing.Options != null)
		{
			float num = (gameListing.Options.TryGetFloat(FloatOptionNames.PlayerSpeedMod, out var value) ? value : 1f);
			tag1.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GamePlayerSpeed) + " " + num + "x";
			gameListing.Options.TryGetBool(BoolOptionNames.Roles, out var value2);
			if (value2)
			{
				tag2.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Roles) + " " + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOn);
			}
			else
			{
				tag2.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Roles) + " " + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SettingsOff);
			}
		}
		else
		{
			tag1.text = string.Empty;
			tag2.text = string.Empty;
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
		Logger.GlobalInstance.Debug("Connecting to: " + InnerNetClient.AddressToString(gameListing.IP) + " for " + GameCode.IntToGameName(gameListing.GameId));
		StartCoroutine(AmongUsClient.Instance.CoJoinOnlinePublicGame(gameListing.GameId, gameListing.IPString, gameListing.Port, AmongUsClient.MainMenuTarget.FindGameMenu));
	}

	public void ClickMore()
	{
		OnMoreAction?.Invoke(gameListing);
	}

	public void StartIcon()
	{
	}

	public void StopIcon()
	{
	}
}
