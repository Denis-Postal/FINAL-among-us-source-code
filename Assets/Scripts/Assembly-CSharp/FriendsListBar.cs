using System.Collections.Generic;
using Epic.OnlineServices.Connect;
using InnerNet;
using Steamworks;
using TMPro;
using UnityEngine;

public class FriendsListBar : MonoBehaviour
{
	public TextMeshPro SenderName;

	public List<PassiveButton> ControllerSelectable;

	public ClientData ClientData;

	public List<PassiveButton> Buttons;

	public SpriteRenderer InactiveSprite;

	public PlatformIdentifier PlatformIdentifier;

	public FriendsListManager.PlatformFriend PlatformFriendData;

	public PassiveButton PlatformProfileButton;

	protected float actionCooldown;

	protected string puid;

	protected string friendCode;

	protected FriendsListUI parentUI;

	private void Start()
	{
		if ((bool)PlatformIdentifier)
		{
			PlatformIdentifier.iconRenderer.enabled = true;
		}
		if (PlatformFriendData == null && (bool)PlatformProfileButton)
		{
			PlatformProfileButton.enabled = false;
		}
	}

	public virtual void SetUp(string puid, FriendsListUI parentUI, string friendCode, string playerInGameName)
	{
		this.parentUI = parentUI;
		this.puid = puid;
		this.friendCode = friendCode;
		SenderName.text = ((friendCode == string.Empty) ? playerInGameName : friendCode);
		SenderName.fontMaterial.SetFloat("_StencilComp", 3f);
		SenderName.fontMaterial.SetFloat("_Stencil", 51f);
	}

	public void GetAndSetPlatform()
	{
		DestroyableSingleton<EOSManager>.Instance.GetExternalAccountInfo(puid, Constants.GetPlatformType(), (ExternalAccountInfo? info) =>
		{
			if ((bool)PlatformIdentifier)
			{
				PlatformIdentifier.SetInfo(Constants.GetPlatformType(), "");
				PlatformIdentifier.iconRenderer.enabled = true;
				if (!info.HasValue)
				{
					PlatformIdentifier.iconRenderer.color = Color.white;
					PlatformIdentifier.iconRenderer.transform.localScale /= 2f;
					PlatformIdentifier.iconRenderer.sprite = DestroyableSingleton<FriendsListManager>.Instance.GlobalPlayerIcon;
					if (PlatformIdentifier.iconRenderer.transform.childCount > 0)
					{
						PlatformIdentifier.iconRenderer.transform.GetChild(0).gameObject.SetActive(value: false);
					}
				}
			}
		});
	}

	public void SetPlatform(Platforms p, string name)
	{
		PlatformIdentifier.SetInfo(p, name);
		PlatformIdentifier.iconRenderer.enabled = true;
	}

	private void Update()
	{
		if (actionCooldown > 0f)
		{
			actionCooldown -= Time.unscaledDeltaTime;
		}
	}

	public bool CanUseApi()
	{
		return actionCooldown <= 0f;
	}

	protected void GoInactive()
	{
		InactiveSprite.enabled = true;
		Buttons.ForEach((PassiveButton b) =>
		{
			b.enabled = false;
		});
	}

	protected void GoActive()
	{
		InactiveSprite.enabled = false;
		Buttons.ForEach((PassiveButton b) =>
		{
			b.enabled = true;
		});
	}

	public void PlatformViewProfile()
	{
		if (PlatformFriendData != null)
		{
			Debug.Log("Viewing profile " + PlatformFriendData.GUID);
			if (CanUseApi())
			{
				actionCooldown = 1f;
				SteamFriends.ActivateGameOverlayToUser("steamid", PlatformFriendData.CSteamID);
			}
		}
	}
}
