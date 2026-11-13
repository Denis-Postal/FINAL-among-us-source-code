using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class SignInStatusComponent : MonoBehaviour
{
	private const string FRIEND_CODE_HIDDEN_TEXT = "XXXXXXXXXX";

	[SerializeField]
	private SpriteRenderer statusSprite;

	[SerializeField]
	private SpriteRenderer glowSprite;

	[SerializeField]
	private TextMeshPro friendCodeText;

	[SerializeField]
	private SpriteRenderer FriendCodeHiddenIcon;

	[SerializeField]
	private GameObject friendCodeShowToggle;

	[SerializeField]
	private GameObject friendsButton;

	[SerializeField]
	private Sprite onlineSprite;

	[SerializeField]
	private Sprite pendingSprite;

	[SerializeField]
	private Sprite guestSprite;

	[SerializeField]
	private Sprite offlineSprite;

	[SerializeField]
	private Sprite onlineGlow;

	[SerializeField]
	private Sprite pendingGlow;

	[SerializeField]
	private Sprite guestGlow;

	public void SetOnline()
	{
		statusSprite.sprite = onlineSprite;
		glowSprite.sprite = onlineGlow;
		friendsButton.SetActive(value: true);
	}

	public void SetPending()
	{
		statusSprite.sprite = pendingSprite;
		glowSprite.sprite = pendingGlow;
		friendsButton.SetActive(value: true);
	}

	public void SetGuest()
	{
		statusSprite.sprite = guestSprite;
		glowSprite.sprite = guestGlow;
		friendsButton.SetActive(value: true);
	}

	public void SetOffline()
	{
		statusSprite.sprite = offlineSprite;
		glowSprite.sprite = null;
		friendsButton.SetActive(value: false);
	}

	public void ToggleFriendCode()
	{
		PlayerData player = DataManager.Player;
		PlayerAccountData account = player.account;
		if (account.loginStatus == EOSManager.AccountLoginStatus.LoggedIn)
		{
			FriendsListManager instance = DestroyableSingleton<FriendsListManager>.Instance;
			FriendsListManager instance2 = DestroyableSingleton<FriendsListManager>.Instance;
			bool friendCodeHidden = !instance2.FriendCodeHidden;
			instance.FriendCodeHidden = friendCodeHidden;
		}
	}

	private void Update()
	{
		EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
		if (instance.loginFlowFinished)
		{
			UpdateFriendCodeUI();
		}
	}

	private void UpdateFriendCodeUI()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}
}
