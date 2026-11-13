using System;
using System.Collections;
using System.Linq;
using AmongUs.Data;
using Epic.OnlineServices.KWS;
using InnerNet;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AccountManager : DestroyableSingleton<AccountManager>
{
	private static readonly string[] ACCOUNT_TAB_SCENES = new string[2] { "MainMenu", "MatchMaking" };

	[SerializeField]
	private AccountTab accountTab;

	[SerializeField]
	private GameObject accountWindow;

	[SerializeField]
	private PermissionsRequest enterGuardianEmailWindow;

	[SerializeField]
	private UpdateGuardianEmail updateGuardianEmailWindow;

	[SerializeField]
	private InfoTextBox guardianEmailConfirmWindow;

	[SerializeField]
	private InfoTextBox genericInfoDisplayBox;

	[SerializeField]
	private AgeGateScreen enterDateOfBirthScreen;

	public GameObject waitingText;

	public GameObject postLoadWaiting;

	public GameObject privacyPolicyBg;

	public SignInScreen signInScreen;

	public PrivacyPolicyScreen PrivacyPolicy;

	private IChatModeDisplay chatModeMenuScreen;

	public KWSPermissionStatus freeChatAllowed = KWSPermissionStatus.Rejected;

	public KWSPermissionStatus customDisplayName = KWSPermissionStatus.Rejected;

	public KWSPermissionStatus friendsListAllowed = KWSPermissionStatus.Rejected;

	public KWSPermissionStatus accountLinkingAllowed = KWSPermissionStatus.Rejected;

	public Action OnLoggedInStatusChange;

	private EOSManager.AccountLoginStatus prevLoggedInStatus;

	[SerializeField]
	private TextMeshPro accountDeletionText;

	private bool isAccountWaitingForDeletion;

	public bool IsAccountWaitingForDeletion => isAccountWaitingForDeletion;

	public bool AccountTabActive => accountTab.isActiveAndEnabled;

	public void UpdateKidAccountDisplay()
	{
		if (!AmongUsClient.Instance || !AmongUsClient.Instance.AmClient)
		{
			accountTab.UpdateKidAccountCanChangeName();
			if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
			{
				DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.QuickChatOnly;
			}
			if (chatModeMenuScreen != null)
			{
				chatModeMenuScreen.UpdateDisplay();
			}
		}
	}

	public void SetChatModeButtonForUpdates(IChatModeDisplay modeCycle)
	{
		chatModeMenuScreen = modeCycle;
	}

	public void UpdateAccountInfoDisplays()
	{
		accountTab.UpdateNameDisplay();
	}

	public void ShowGuardianEmailSentConfirm()
	{
		guardianEmailConfirmWindow.gameObject.SetActive(value: true);
	}

	public void ShowPermissionsRequestForm()
	{
		enterGuardianEmailWindow.Show();
	}

	public void CreateAccount()
	{
		DestroyableSingleton<EOSManager>.Instance.CreateAccountWithPlatformAuth();
	}

	public IEnumerator EditGuardianEmail()
	{
		yield return updateGuardianEmailWindow.Show();
		UpdateGuardianEmailDisplay();
	}

	public IEnumerator UpdateMissingGuardianEmail()
	{
		yield return updateGuardianEmailWindow.ShowMissingEmail();
		UpdateGuardianEmailDisplay();
	}

	public void UpdateGuardianEmailDisplay()
	{
		accountTab.UpdateGuardianEmailText();
	}

	public IEnumerator ShowAgeGate()
	{
		yield return enterDateOfBirthScreen.Show();
	}

	public void CheckAndRegenerateName()
	{
		if (!ValidateRandomName())
		{
			RandomizeName();
		}
	}

	public bool CanMinorSetCustomDisplayName()
	{
		if (PhotonOnlineTransport.IsConfigured)
		{
			return true;
		}
		if (customDisplayName == KWSPermissionStatus.Granted)
		{
			return DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn;
		}
		return false;
	}

	public bool HasMinorsGuardianEverUpdatedAnything()
	{
		if (freeChatAllowed == KWSPermissionStatus.Pending && customDisplayName == KWSPermissionStatus.Pending && friendsListAllowed == KWSPermissionStatus.Pending)
		{
			return accountLinkingAllowed != KWSPermissionStatus.Pending;
		}
		return true;
	}

	public void RandomizeName()
	{
		DataManager.Player.Customization.Name = GetRandomName();
		DataManager.Player.Save();
		accountTab.UpdateNameDisplay();
	}

	public string GetRandomName()
	{
		string text;
		do
		{
			text = GetComponent<RandomNameGenerator>().GetName();
		}
		while (text.Length > 32 || BlockedWords.ContainsWord(text));
		return text;
	}

	private bool ValidateRandomName()
	{
		return GetComponent<RandomNameGenerator>().ValidateName(DataManager.Player.Customization.Name);
	}

	public override void Awake()
	{
		base.Awake();
		// Offline remake: these covers (loading / waiting / privacy-policy backdrop) are shown and hidden by the EOS login flow, which does not
		// run here; the exported prefab has them active, so hide them or they black out the account window and the main menu.
		if (waitingText != null)
		{
			waitingText.SetActive(value: false);
		}
		if (postLoadWaiting != null)
		{
			postLoadWaiting.SetActive(value: false);
		}
		if (privacyPolicyBg != null)
		{
			privacyPolicyBg.SetActive(value: false);
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		accountWindow.SetActive(value: false);
		accountTab.gameObject.SetActive(ACCOUNT_TAB_SCENES.Contains(scene.name));
		if (accountTab.gameObject.activeSelf)
		{
			UpdateAccountDeletionStatus(isAccountWaitingForDeletion);
		}
		else
		{
			accountDeletionText.gameObject.SetActive(value: false);
		}
	}

	public void SignInSuccess(Action callback)
	{
		genericInfoDisplayBox.SetOneButton();
		genericInfoDisplayBox.titleTexxt.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Success);
		genericInfoDisplayBox.bodyText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SuccessLogIn);
		genericInfoDisplayBox.button1Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Close);
		genericInfoDisplayBox.button1.OnClick.RemoveAllListeners();
		genericInfoDisplayBox.button1.OnClick.AddListener(() =>
		{
			callback();
		});
		genericInfoDisplayBox.gameObject.SetActive(value: true);
		if (signInScreen.IsOpen())
		{
			signInScreen.OnClose();
		}
	}

	public void SignInFail(EOSManager.EOS_ERRORS error, Action callback)
	{
		DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Sign in fail {error}");
		if (error == EOSManager.EOS_ERRORS.FriendCodeFail)
		{
			Debug.LogWarning("SignInFail FriendCodeFail suppressed to preserve logged in status.");
			return;
		}
		genericInfoDisplayBox.SetOneButton();
		genericInfoDisplayBox.titleTexxt.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Failed);
		genericInfoDisplayBox.bodyText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ErrorLogIn) + $"\n(Error {error})";
		genericInfoDisplayBox.button1Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Close);
		genericInfoDisplayBox.gameObject.SetActive(value: true);
		genericInfoDisplayBox.button1.OnClick.RemoveAllListeners();
		genericInfoDisplayBox.button1.OnClick.AddListener(() =>
		{
			callback();
		});
	}

	public void PlatformSignInFail()
	{
		if (DataManager.Player.Account.LoginStatus != EOSManager.AccountLoginStatus.TempAccount)
		{
			string text = "";
			genericInfoDisplayBox.SetOneButton();
			genericInfoDisplayBox.titleTexxt.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SignInIssueTitle, Constants.GetCurrentPlatformName());
			genericInfoDisplayBox.bodyText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SignInIssueText, text);
			genericInfoDisplayBox.button1Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Continue);
			genericInfoDisplayBox.button1.OnClick.RemoveAllListeners();
			genericInfoDisplayBox.button1.OnClick.AddListener(() =>
			{
				DestroyableSingleton<EOSManager>.Instance.StartTempAccountFlow();
			});
			genericInfoDisplayBox.gameObject.SetActive(value: true);
			if (signInScreen.IsOpen())
			{
				signInScreen.OnClose();
			}
		}
		else
		{
			DestroyableSingleton<EOSManager>.Instance.StartTempAccountFlow();
		}
	}

	public void SetDLLErrorMode()
	{
		DataManager.Player.Account.LoginStatus = EOSManager.AccountLoginStatus.Offline;
		DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.QuickChatOnly;
		accountTab.SetDLLErrorMode();
	}

	public void UpdateVisuals()
	{
		if (prevLoggedInStatus != DataManager.Player.Account.LoginStatus)
		{
			OnLoggedInStatusChange?.Invoke();
		}
		prevLoggedInStatus = DataManager.Player.Account.LoginStatus;
		if (DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed() && SceneManager.GetActiveScene().name.Equals("MainMenu"))
		{
			accountTab.TurnAllSectionsOff();
			StartCoroutine(CheckFriendCodeAndUpdateVisuals());
		}
		else
		{
			accountTab.UpdateVisuals();
		}
	}

	private IEnumerator CheckFriendCodeAndUpdateVisuals()
	{
		yield return StartCoroutine(DestroyableSingleton<FriendsListManager>.Instance.CheckFriendCodeOnLogin());
		accountTab.UpdateVisuals();
	}

	public void UpdatePlayerCustomizationVisuals()
	{
		accountTab.UpdatePlayerCosmetics();
	}

	public bool CanPlayOnline()
	{
		return PhotonOnlineTransport.IsConfigured;
	}

	private bool CanPlayOnlineOriginal()
	{
		if (DestroyableSingleton<EOSManager>.Instance == null)
		{
			return false;
		}
		if (DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.Offline)
		{
			return false;
		}
		if (DestroyableSingleton<EOSManager>.Instance.PlatformInterface == null)
		{
			return false;
		}
		return true;
	}

	// Offline remake: the original asks the live account backend (BackendEndpoints.AccountDeletion, Bearer EOS token) whether the account is
	// scheduled for deletion. A local profile can never be, and no request is made.
	public void GetDeletionInfo()
	{
		UpdateAccountDeletionStatus(shouldDelete: false);
	}

	public void UpdateAccountDeletionStatus(bool shouldDelete)
	{
		isAccountWaitingForDeletion = shouldDelete;
		accountDeletionText.gameObject.SetActive(IsAccountWaitingForDeletion);
	}

	public void OpenAccountWindow()
	{
		accountWindow.SetActive(value: true);
		accountTab.Open();
	}

	public void CloseAccountWindow()
	{
		accountTab.Close();
		accountWindow.SetActive(value: false);
	}
}
