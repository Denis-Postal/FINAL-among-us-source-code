using System;
using System.Collections.Generic;
using AmongUs.Data;
using InnerNet;
using TMPro;
using UnityEngine;

public class AccountTab : MonoBehaviour
{
	public TextMeshPro userName;

	public TextMeshPro friendCode;

	public TextMeshPro friendCodeTitle;

	public PoolablePlayer playerImage;

	public GameObject friendCodeObject;

	public SpriteRenderer FriendCodeHiddenIcon;

	public GameObject offlineMode;

	public GameObject guestMode;

	public FullAccount loggedInMode;

	public GameObject waitingForGuardian;

	public TextMeshPro guardianEmailText;

	public EditName editNameScreen;

	public GameObject idCard;

	public GameObject resendEmailButton;

	public TextMeshPro levelText;

	public ProgressBar xpProgressBar;

	public TextMeshPro veryBadErrorText;

	public Collider2D clickToCloseCollider;

	public TextMeshPro accountIDDisplayText;

	public GameObject showAccountIDButton;

	public SpriteRenderer SpaceBean;

	public SpriteRenderer SpaceHorse;

	public InfoTextBox InfoTextBoxDisplay;

	[SerializeField]
	private SignInStatusComponent signInStatusComponent;

	[SerializeField]
	private GameObject accountCopyButton;

	private bool showAccountID;

	private string friendCodeHiddenText = "XXXXXXXXXX";

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public List<UiElement> PotentialDefaultSelections;

	public List<UiElement> selectableObjects;

	private UiElement DefaultSelection
	{
		get
		{
			foreach (UiElement potentialDefaultSelection in PotentialDefaultSelections)
			{
				if (potentialDefaultSelection.isActiveAndEnabled)
				{
					return potentialDefaultSelection;
				}
			}
			return null;
		}
	}

	private void Awake()
	{
		SpaceHorse.enabled = AprilFoolsMode.ShouldHorseAround();
		SpaceBean.enabled = !AprilFoolsMode.ShouldHorseAround();
	}

	public void TurnAllSectionsOff()
	{
		loggedInMode.gameObject.SetActive(value: false);
		waitingForGuardian.SetActive(value: false);
		veryBadErrorText.gameObject.SetActive(value: false);
		offlineMode.gameObject.SetActive(value: false);
		guestMode.gameObject.SetActive(value: false);
	}

	public void UpdateKidAccountCanChangeName()
	{
		loggedInMode.CanSetCustomName(DestroyableSingleton<AccountManager>.Instance.CanMinorSetCustomDisplayName());
	}

	public void SignIn()
	{
		DestroyableSingleton<EOSManager>.Instance.LoginFromAccountTab();
	}

	public void RandomizeName()
	{
		DestroyableSingleton<AccountManager>.Instance.RandomizeName();
	}

	public void ManageAccount()
	{
		InfoTextBoxDisplay.titleTexxt.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ManageAccountTitle);
		InfoTextBoxDisplay.button1Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Okay);
		InfoTextBoxDisplay.SetOneButton();
		InfoTextBoxDisplay.gameObject.SetActive(value: true);
		if (!string.IsNullOrEmpty(DataManager.Player.Account.GuardianEmail))
		{
			InfoTextBoxDisplay.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ManageAccountText) + "<br><br>" + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GuardianEmailTitle) + ": " + DataManager.Player.Account.GuardianEmail);
		}
		else
		{
			InfoTextBoxDisplay.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ManageAccountText));
		}
	}

	public void UpdateNameDisplay()
	{
		userName.text = DataManager.Player.Customization.Name;
		if (!string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.FriendCode))
		{
			friendCodeTitle.enabled = true;
			friendCode.enabled = true;
		}
		else
		{
			friendCodeTitle.enabled = false;
			friendCode.enabled = false;
		}
	}

	public void UpdatePlayerCosmetics()
	{
		playerImage.UpdateFromDataManager(PlayerMaterial.MaskType.SimpleUI);
		playerImage.ToggleName(active: false);
	}

	public void ChangeName()
	{
		editNameScreen.gameObject.SetActive(value: true);
	}

	public void ResendEmail()
	{
		DestroyableSingleton<EOSManager>.Instance.ResendGuardianEmail();
		DestroyableSingleton<AccountManager>.Instance.ShowGuardianEmailSentConfirm();
	}

	public void EditGuardianEmail()
	{
		StartCoroutine(DestroyableSingleton<AccountManager>.Instance.EditGuardianEmail());
	}

	public void UpdateGuardianEmailText()
	{
		guardianEmailText.text = DataManager.Player.Account.GuardianEmail;
		resendEmailButton.gameObject.SetActive(!string.IsNullOrEmpty(DataManager.Player.Account.GuardianEmail));
	}

	public void SetDLLErrorMode()
	{
		TurnAllSectionsOff();
		DestroyableSingleton<AccountManager>.Instance.RandomizeName();
		idCard.SetActive(value: true);
		veryBadErrorText.gameObject.SetActive(value: true);
		veryBadErrorText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DLLNotFoundAccountError);
	}

	public void UpdateVisuals()
	{
		TurnAllSectionsOff();
		UpdateNameDisplay();
		UpdateLevelDisplay();
		if (string.IsNullOrEmpty(userName.text))
		{
			DestroyableSingleton<AccountManager>.Instance.RandomizeName();
		}
		showAccountID = false;
		UpdateAccountIDDisplay();
		UpdateFriendCodeUI();
		switch (DataManager.Player.Account.LoginStatus)
		{
		case EOSManager.AccountLoginStatus.LoggedIn:
			Debug.Log("*******************updating to LOGGED IN MODE");
			signInStatusComponent.SetOnline();
			if (DataManager.Settings.Multiplayer.ChatMode == QuickChatModes.QuickChatOnly)
			{
				resendEmailButton.gameObject.SetActive(!string.IsNullOrEmpty(DataManager.Player.Account.GuardianEmail));
			}
			idCard.SetActive(value: true);
			accountIDDisplayText.gameObject.SetActive(value: true);
			loggedInMode.gameObject.SetActive(value: true);
			loggedInMode.UpdateLoggedInAccountVisuals();
			DestroyableSingleton<AccountManager>.Instance.GetDeletionInfo();
			break;
		case EOSManager.AccountLoginStatus.Offline:
			Debug.Log("*******************updating to OFFLINE MODE");
			signInStatusComponent.SetOffline();
			idCard.SetActive(value: true);
			offlineMode.gameObject.SetActive(value: true);
			accountIDDisplayText.gameObject.SetActive(value: false);
			if (PhotonOnlineTransport.IsConfigured)
			{
				loggedInMode.gameObject.SetActive(value: true);
				loggedInMode.UpdateLoggedInAccountVisuals();
			}
			else if (DestroyableSingleton<EOSManager>.Instance.PlatformInterface == null)
			{
				SetDLLErrorMode();
			}
			break;
		case EOSManager.AccountLoginStatus.WaitingForParent:
			Debug.Log("*******************updating to WAITING FOR PARENT MODE");
			signInStatusComponent.SetPending();
			guardianEmailText.text = (string.IsNullOrEmpty(DataManager.Player.Account.GuardianEmail) ? DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GuardianEmail) : DataManager.Player.Account.GuardianEmail);
			idCard.SetActive(value: false);
			accountIDDisplayText.gameObject.SetActive(value: true);
			waitingForGuardian.gameObject.SetActive(value: true);
			DestroyableSingleton<AccountManager>.Instance.GetDeletionInfo();
			break;
		case EOSManager.AccountLoginStatus.TempAccount:
			Debug.Log("*******************updating to ERROR MODE");
			signInStatusComponent.SetGuest();
			idCard.SetActive(value: true);
			accountIDDisplayText.gameObject.SetActive(value: true);
			loggedInMode.gameObject.SetActive(value: false);
			guestMode.gameObject.SetActive(value: true);
			loggedInMode.UpdateLoggedInAccountVisuals();
			DestroyableSingleton<AccountManager>.Instance.GetDeletionInfo();
			break;
		}
	}

	private void UpdateAccountIDDisplay()
	{
		if (!string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.ProductUserId))
		{
			if (showAccountID)
			{
				accountIDDisplayText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AccountIDDisplay, DestroyableSingleton<EOSManager>.Instance.ProductUserId);
				showAccountIDButton.gameObject.SetActive(value: false);
				accountCopyButton.SetActive(value: true);
			}
			else
			{
				showAccountIDButton.gameObject.SetActive(value: true);
				accountIDDisplayText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AccountIDDisplay, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.HiddenAccountID));
				accountCopyButton.SetActive(value: false);
			}
		}
		else
		{
			showAccountID = false;
			accountCopyButton.SetActive(value: false);
			showAccountIDButton.gameObject.SetActive(value: false);
			accountIDDisplayText.text = "";
		}
	}

	public void HideAccountIDDisplay()
	{
		showAccountID = false;
		UpdateAccountIDDisplay();
	}

	public void ShowAccountIDDisplay()
	{
		showAccountID = true;
		UpdateAccountIDDisplay();
	}

	public void ToggleFriendCodeDisplay()
	{
		if (DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn)
		{
			DestroyableSingleton<FriendsListManager>.Instance.FriendCodeHidden = !DestroyableSingleton<FriendsListManager>.Instance.FriendCodeHidden;
			UpdateFriendCodeUI();
		}
	}

	public void UpdateFriendCodeUI()
	{
		if (DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed())
		{
			friendCodeObject.SetActive(value: true);
			FriendCodeHiddenIcon.enabled = DestroyableSingleton<FriendsListManager>.Instance.FriendCodeHidden;
			if (DestroyableSingleton<FriendsListManager>.Instance.FriendCodeHidden)
			{
				friendCode.text = friendCodeHiddenText;
			}
			else
			{
				friendCode.text = DestroyableSingleton<EOSManager>.Instance.FriendCode;
			}
		}
		else
		{
			friendCodeObject.SetActive(value: false);
		}
	}

	public void Toggle()
	{
		SlideOpen component = GetComponent<SlideOpen>();
		if (!(component == null))
		{
			if (component.isOpen)
			{
				Close();
			}
			else
			{
				Open();
			}
		}
	}

	public void Close()
	{
		clickToCloseCollider.enabled = false;
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void Open()
	{
		HideAccountIDDisplay();
		clickToCloseCollider.enabled = true;
		UpdateVisuals();
		UpdatePlayerCosmetics();
		UpdateFriendCodeUI();
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultSelection, selectableObjects);
	}

	private void UpdateLevelDisplay()
	{
		levelText.text = ProgressionManager.Instance.CurrentVisualLevel;
		xpProgressBar.MaxValue = Math.Max(1u, DataManager.Player.Stats.XpForNextLevel);
		xpProgressBar.Value = Mathf.Clamp(DataManager.Player.Stats.Xp, 0f, xpProgressBar.MaxValue);
		xpProgressBar.CapValue = xpProgressBar.Value;
		xpProgressBar.GlowAlpha = 0f;
	}
}
