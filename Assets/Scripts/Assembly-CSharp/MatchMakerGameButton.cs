using InnerNet;
using PowerTools;
using TMPro;
using UnityEngine;

public class MatchMakerGameButton : PoolableBehavior, IConnectButton
{
	public TextMeshPro NameText;

	public TextMeshPro SmallNameText;

	public TextMeshPro PlayerCountText;

	public TextMeshPro ImpostorCountText;

	public SpriteRenderer MapIcon;

	public PlatformIdentifier PlatformIdentifier;

	public TextMeshPro LanguageText;

	public Sprite[] MapIcons;

	public SpriteAnim connectIcon;

	public AnimationClip connectClip;

	public GameListing myListing;

	public void OnClick()
	{
		if (DestroyableSingleton<MatchMaker>.Instance.Connecting(this) && myListing.IP != 0)
		{
			Debug.Log("Connecting to: " + InnerNetClient.AddressToString(myListing.IP) + " for " + GameCode.IntToGameName(myListing.GameId));
			StartCoroutine(AmongUsClient.Instance.CoJoinOnlinePublicGame(myListing.GameId, myListing.IPString, myListing.Port));
		}
	}

	public void StartIcon()
	{
		connectIcon.Play(connectClip);
	}

	public void StopIcon()
	{
		connectIcon.Stop();
		connectIcon.GetComponent<SpriteRenderer>().sprite = null;
	}

	public void SetGame(GameListing gameListing)
	{
		myListing = gameListing;
		if (Constants.GetPlatformType() == gameListing.Platform && PlatformIdentifier.SupportsPlatformUsername(gameListing.Platform))
		{
			SmallNameText.gameObject.SetActive(value: true);
			NameText.gameObject.SetActive(value: false);
			SmallNameText.text = myListing.HostName;
		}
		else
		{
			SmallNameText.gameObject.SetActive(value: false);
			NameText.gameObject.SetActive(value: true);
			NameText.text = myListing.HostName;
		}
		if (myListing.Language == 1)
		{
			LanguageText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OtherLanguage);
		}
		else
		{
			LanguageText.text = ChatLanguageSet.Instance.GetString(myListing.Language);
		}
		PlatformIdentifier.gameObject.SetActive(value: false);
		ImpostorCountText.text = myListing.NumImpostors.ToString();
		PlayerCountText.text = $"{myListing.PlayerCount}/{myListing.MaxPlayers}";
		MapIcon.sprite = MapIcons[Mathf.Clamp(myListing.MapId, 0, MapIcons.Length - 1)];
	}
}
