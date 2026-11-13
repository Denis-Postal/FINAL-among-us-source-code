using System.Collections;
using AmongUs.Data;
using TMPro;
using UnityEngine;

public class HostInfoPanel : MonoBehaviour
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private PoolablePlayer player;

	[SerializeField]
	private GameObject playerHolder;

	[SerializeField]
	private TextMeshPro playerName;

	[SerializeField]
	private TextMeshPro hostLabel;

	[SerializeField]
	private Transform hostLabelCenter;

	private const float CREW_WIDTH = 0.43f;

	private const float CONTENT_CENTER_X = -0.43f;

	private float hostWidth;

	private bool firstUpdate;

	public void SetUp()
	{
		NetworkedPlayerInfo host = GameData.Instance.GetHost();
		hostWidth = hostLabel.GetRenderedValues().x;
		if (!(host == null) && !host.IsIncomplete)
		{
			content.SetActive(value: true);
			if (!firstUpdate)
			{
				firstUpdate = true;
				StartCoroutine(SetCosmetics(host));
			}
			string text = ColorUtility.ToHtmlStringRGB(Palette.PlayerColors[player.ColorId]);
			hostLabel.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.HostNounLabel);
			if (ShouldBoldenHostLabel(DataManager.Settings.Language.CurrentLanguage))
			{
				hostLabel.text = hostLabel.text.Insert(0, "<b>");
				hostLabel.text = hostLabel.text.Insert(hostLabel.text.Length, "</b>");
			}
			if (AmongUsClient.Instance.AmHost)
			{
				playerName.text = (string.IsNullOrEmpty(host.PlayerName) ? "..." : ("<color=#" + text + ">" + host.PlayerName + "</color>")) + "  <size=90%><b><font=\"Barlow-BoldItalic SDF\" material=\"Barlow-BoldItalic SDF Outline\">" + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.HostYouLabel);
			}
			else
			{
				playerName.text = (string.IsNullOrEmpty(host.PlayerName) ? "..." : ("<color=#" + text + ">" + host.PlayerName + "</color>")) + " (" + player.ColorBlindName + ")";
			}
			float x = playerName.GetRenderedValues().x;
			float num = hostWidth + 0.43f + 0.05f;
			content.transform.localPosition = new Vector3(-0.43f + num / 2f, content.transform.localPosition.y, content.transform.localPosition.z);
			playerHolder.transform.localPosition = new Vector3(playerName.transform.localPosition.x - x / 2f - 0.215f, playerHolder.transform.localPosition.y, playerHolder.transform.localPosition.z);
			hostLabel.transform.localPosition = new Vector3(playerHolder.transform.localPosition.x - 0.215f - 0.05f, hostLabel.transform.localPosition.y, hostLabel.transform.localPosition.z);
		}
	}

	public IEnumerator SetCosmetics(NetworkedPlayerInfo playerInfo)
	{
		yield return new WaitForSeconds(0.1f);
		if (gameObject.activeInHierarchy)
		{
			player.UpdateFromEitherPlayerDataOrCache(playerInfo, PlayerOutfitType.Default, PlayerMaterial.MaskType.ScrollingUI, includePet: false);
		}
		player.ToggleName(active: false);
	}

	private void Awake()
	{
		content.SetActive(value: false);
	}

	private void Update()
	{
		SetUp();
	}

	private bool ShouldBoldenHostLabel(SupportedLangs language)
	{
		if ((uint)(language - 4) <= 1u || language == SupportedLangs.Japanese || (uint)(language - 13) <= 1u)
		{
			return true;
		}
		return false;
	}
}
