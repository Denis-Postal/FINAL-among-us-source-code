using AmongUs.Data;
using AmongUs.GameOptions;
using InnerNet;
using TMPro;
using UnityEngine;

public class ConfirmCreatePopUp : MonoBehaviour
{
	[SerializeField]
	private CreateGameOptions createGameOptions;

	[SerializeField]
	private TextMeshPro modeText;

	[SerializeField]
	private TextMeshPro serverText;

	[SerializeField]
	private TextMeshPro capacityText;

	[SerializeField]
	private TextMeshPro chatText;

	[SerializeField]
	private TextMeshPro tagText;

	[SerializeField]
	private TextMeshPro langText;

	[SerializeField]
	private Sprite[] mapBanners;

	[SerializeField]
	private Sprite[] mapLogos;

	[SerializeField]
	private SpriteRenderer mapBanner;

	[SerializeField]
	private SpriteRenderer mapLogo;

	private void OnEnable()
	{
		SetupInfo();
	}

	private void SetupInfo()
	{
		IRegionInfo currentRegion = DestroyableSingleton<ServerManager>.Instance.CurrentRegion;
		serverText.text = DestroyableSingleton<TranslationController>.Instance.GetStringWithDefault(currentRegion.TranslateName, currentRegion.Name);
		if (DataManager.Settings.Multiplayer.LastPlayedGameMode == GameModes.HideNSeek)
		{
			modeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeHideAndSeek);
		}
		else
		{
			modeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameTypeClassic);
		}
		capacityText.text = createGameOptions.GetCapacity().ToString();
		if (DataManager.Settings.Multiplayer.ChatMode == QuickChatModes.FreeChatOrQuickChat)
		{
			chatText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeOrQuickChat);
		}
		else
		{
			chatText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QuickChat);
		}
		uint keywords = (uint)GameOptionsManager.Instance.GameHostOptions.Keywords;
		langText.text = ChatLanguageSet.Instance.GetString(keywords);
		tagText.text = createGameOptions.GetLevelString();
		int mapId = GameOptionsManager.Instance.GameHostOptions.MapId;
		mapBanner.sprite = mapBanners[mapId];
		mapLogo.sprite = mapLogos[mapId];
		if (mapId == 0 && AprilFoolsMode.ShouldFlipSkeld())
		{
			mapLogo.flipX = true;
		}
		else
		{
			mapLogo.flipX = false;
		}
	}
}
