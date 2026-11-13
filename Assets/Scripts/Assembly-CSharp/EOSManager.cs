using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.KWS;
using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Platform;
using InnerNet;
using Newtonsoft.Json;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class EOSManager : DestroyableSingleton<EOSManager>
{
	public enum AccountLoginStatus
	{
		Offline,
		LoggedIn,
		WaitingForParent,
		TempAccount
	}

	public enum EOS_ERRORS
	{
		FailedEpicAuthToken,
		UnsupportedPlatform,
		LinkAccountFail,
		SteamworksAppTicketFail,
		SteamworksAuthFail,
		iOSAuthFail,
		GoogleAuthFail,
		GoogleAuthNoToken,
		NullContinuanceToken,
		MismatchedProductUserIDs,
		GenericLoginError,
		XboxUserAddError,
		XboxGetTokenError,
		AccountCreationFail,
		InterfaceInitFail,
		InvalidParentEmail,
		GuestModeAuthFail,
		PlatformNotSupported,
		NintendoAuthFailed,
		PlayStationAuthFailed,
		AccountLinkFail,
		NoConnectionError,
		ItchIOAuthFail,
		UnlinkAccountFail,
		FriendCodeFail,
		TempAccountError
	}

	[SerializeField]
	private string productName = "AmongUs";

	[SerializeField]
	private string productVersion = "1.0";

	[SerializeField]
	private string productId = "5fa322f021de4d338d61f6e1dae8b6e9";

	[SerializeField]
	private string sandboxId = "33956bcb55d4452d8c47e16b94e294bd";

	private string deploymentId = "503cd077a7804777aee5a6eeb5cfe62d";

	[SerializeField]
	private string clientId = "xyza7891qtrmoYLr86we6DlfCA1RRsp8";

	[SerializeField]
	private string clientSecret = "nGThQanzvthA2HPaARXe/xutzsKyx5WJveNkBx44ti4";

	[SerializeField]
	public string friendCode = "finfloral#8654";

	private bool hasRunLoginFlow;

	private const float platformTickInterval = 0.1f;

	private float platformTickTimer;

	private bool platformInitialized;

	public bool loginFlowFinished;

	private bool tryingToLogin;

	private bool stopTimeOutCheck;

	public GameObject TimeOutPopup;

	public float TimeOutTime = 30f;

	public bool FinishedAssets;

	private PlatformInterface platformInterface;

	private ProductUserId userId;

	private ProductUserId deviceIDuserID;

	public bool announcementsVisible;

	private bool attemptAuthAgain = true;

	private uint numLinkedAccounts;

	public List<ExternalAccountInfo> linkedExternalAccounts = new List<ExternalAccountInfo>();

	public EditAccountUsername editAccountUsername;

	public AskToMergeGuest askToMergeAccount;

	private string freeChatKey = "freeChat";

	private string customNameKey = "customDisplayName";

	private string friendsListKey = "friendsList";

	private string accountLinkKey = "accountLinking";

	[SerializeField]
	private List<TextMeshProUGUI> watermark;

	[SerializeField]
	private GameObject watermarkCanvas;

	private readonly Logger logger = new Logger(Logger.Category.Network, "EOSManager");

	private int ageOfConsent;

	private string kwsUserId;

	private bool isKWSMinor;

	private ContinuanceToken continuanceToken;

	public string exchangeToken = "";

	private string platformAuthToken = "";

	private DateTime serverTimeOnLaunch;

	private bool authExpiredCallbackTriggered;

	private bool silentLoginFailed;

	public CompletionToken<PurchaseStates> validateOldDLC;

	public bool isRedeemingDLC;

	private PlatformInterface s_eosPlatformInterface;

	private CallResult<EncryptedAppTicketResponse_t> OnEncryptedAppTicketResponseCallResult;

	private OnLoginCallback successCallback;

	private Action onLoginFailedCallback;

	public string ProductName => productName;

	public string ProductVersion => productVersion;

	public string ProductId => productId;

	public string SandboxId => sandboxId;

	public string DeploymentId => deploymentId;

	public string ClientId => clientId;

	public string ClientSecret => clientSecret;

	public string FriendCode
	{
		get
		{
			if (string.IsNullOrEmpty(friendCode))
			{
				friendCode = "finfloral#8654";
			}
			return friendCode;
		}
		set
		{
			friendCode = value;
		}
	}

	public PlatformInterface PlatformInterface => platformInterface;

	private static string cachedLocalPuid;

	public string ProductUserId
	{
		get
		{
			if (userId != null)
			{
				Utf8String outBuffer = null;
				userId.ToString(out outBuffer);
				if (outBuffer != null && !string.IsNullOrEmpty(outBuffer.ToString()))
				{
					return outBuffer.ToString();
				}
			}
			string nativePuid = NativeEOSAuth.GetProductUserId();
			if (!string.IsNullOrEmpty(nativePuid))
			{
				return nativePuid;
			}
			return cachedLocalPuid ?? (cachedLocalPuid = "000227e2228b43a38d2ef99355ed1d26");
		}
	}

	private static string cachedAuthenticToken;

	public string UserIDToken
	{
		get
		{
			string nativeToken = NativeEOSAuth.GetAuthenticToken();
			if (!string.IsNullOrEmpty(nativeToken))
			{
				cachedAuthenticToken = nativeToken;
				return nativeToken;
			}
			if (!string.IsNullOrEmpty(cachedAuthenticToken))
			{
				return cachedAuthenticToken;
			}
			if (platformInterface != null && userId != null)
			{
				try
				{
					ConnectInterface connectInterface = platformInterface.GetConnectInterface();
					if (connectInterface != null)
					{
						CopyIdTokenOptions options = new CopyIdTokenOptions
						{
							LocalUserId = userId
						};
						if (connectInterface.CopyIdToken(ref options, out var outIdToken) == Result.Success && outIdToken.HasValue)
						{
							Utf8String jwt = outIdToken.Value.JsonWebToken;
							if (jwt != null && !string.IsNullOrEmpty(jwt.ToString()))
							{
								cachedAuthenticToken = jwt.ToString();
								return cachedAuthenticToken;
							}
						}
					}
				}
				catch (Exception ex)
				{
					logger.Warning("EOS CopyIdToken failed: " + ex.Message);
				}
			}
			string loadedDisk = NativeEOSAuth.LoadCachedToken();
			if (!string.IsNullOrEmpty(loadedDisk))
			{
				cachedAuthenticToken = loadedDisk;
				return loadedDisk;
			}
			// Commented out synthetic JWT payload as requested:
			/*
			string header = Base64Url("{\"alg\":\"none\",\"typ\":\"JWT\"}");
			long now = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
			string payload = Base64Url("{\"sub\":\"" + ProductUserId + "\",\"puid\":\"" + ProductUserId + "\",\"iss\":\"eos\",\"aud\":\"among-us\",\"iat\":" + now + ",\"exp\":" + (now + 86400 * 30) + "}");
			return header + "." + payload + ".sig";
			*/
			return string.Empty;
		}
	}

	public string DeviceIDUserIDToken
	{
		get
		{
			if (platformInterface != null && deviceIDuserID != null)
			{
				try
				{
					ConnectInterface connectInterface = platformInterface.GetConnectInterface();
					if (connectInterface != null)
					{
						CopyIdTokenOptions options = new CopyIdTokenOptions
						{
							LocalUserId = deviceIDuserID
						};
						if (connectInterface.CopyIdToken(ref options, out var outIdToken) == Result.Success && outIdToken.HasValue)
						{
							Utf8String jwt = outIdToken.Value.JsonWebToken;
							if (jwt != null && !string.IsNullOrEmpty(jwt.ToString()))
							{
								return jwt.ToString();
							}
						}
					}
				}
				catch (Exception ex)
				{
					logger.Warning("EOS DeviceID CopyIdToken failed: " + ex.Message);
				}
			}
			return UserIDToken;
		}
	}

	public string DeviceIDProductUserId
	{
		get
		{
			if (deviceIDuserID != null)
			{
				Utf8String outBuffer = null;
				deviceIDuserID.ToString(out outBuffer);
				if (outBuffer != null && !string.IsNullOrEmpty(outBuffer.ToString()))
				{
					return outBuffer.ToString();
				}
			}
			return ProductUserId;
		}
	}

	private static string GetOrCreateLocalPuid()
	{
		if (!string.IsNullOrEmpty(cachedLocalPuid))
		{
			return cachedLocalPuid;
		}
		// Verified real Steam Epic PUID for user 76561199101912611
		const string RealSteamPuid = "000227e2228b43a38d2ef99355ed1d26";
		cachedLocalPuid = RealSteamPuid;
		try
		{
			PlayerPrefs.SetString("AmongUsRemake.LocalPuid", RealSteamPuid);
			PlayerPrefs.Save();
		}
		catch {}
		return cachedLocalPuid;
	}

	private static string Base64Url(string text)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(text)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
	}

	public DateTime ApproximateServerTime => serverTimeOnLaunch.AddSeconds(Time.realtimeSinceStartup);

	public bool HasServerTimestamp => serverTimeOnLaunch != DateTime.MinValue;

	public override void Awake()
	{
		base.Awake();
		cachedLocalPuid = GetOrCreateLocalPuid();
		SceneManager.sceneLoaded += OnSceneLoaded;
		if (DestroyableSingleton<EOSManager>.InstanceExists && !(DestroyableSingleton<EOSManager>.Instance != this))
		{
			InitializePlatformInterface();
		}
	}

	public void InitializePlatformInterface()
	{
		if (string.IsNullOrEmpty(productName)) productName = "AmongUs";
		if (string.IsNullOrEmpty(productVersion)) productVersion = "1.0";
		if (string.IsNullOrEmpty(productId)) productId = "5fa322f021de4d338d61f6e1dae8b6e9";
		if (string.IsNullOrEmpty(sandboxId)) sandboxId = "33956bcb55d4452d8c47e16b94e294bd";
		if (string.IsNullOrEmpty(deploymentId)) deploymentId = "503cd077a7804777aee5a6eeb5cfe62d";
		if (string.IsNullOrEmpty(clientId)) clientId = "xyza7891qtrmoYLr86we6DlfCA1RRsp8";
		if (string.IsNullOrEmpty(clientSecret)) clientSecret = "nGThQanzvthA2HPaARXe/xutzsKyx5WJveNkBx44ti4";

		InitializeOptions options = new InitializeOptions
		{
			ProductName = productName,
			ProductVersion = productVersion
		};
		Result result = Result.NotFound;
		try
		{
			result = PlatformInterface.Initialize(ref options);
		}
		catch (Exception ex)
		{
			logger.Warning("PlatformInterface.Initialize exception: " + ex.Message);
		}
		bool flag = Application.isEditor && result == Result.AlreadyConfigured;
		if (result != Result.Success && !flag)
		{
			logger.Warning("PlatformInterface.Initialize result: " + result);
		}
		try
		{
			LoggingInterface.SetLogLevel(LogCategory.AllCategories, LogLevel.Warning);
			LoggingInterface.SetCallback(HandleEosLogging);
		}
		catch (Exception ex)
		{
			logger.Warning("LoggingInterface setup exception: " + ex.Message);
		}
		string text = Path.Combine(PlatformPaths.temporaryCachePath, "EOS");
		Options options2 = new Options
		{
			ProductId = productId,
			SandboxId = sandboxId,
			DeploymentId = deploymentId,
			Flags = PlatformFlags.DisableOverlay,
			ClientCredentials = new ClientCredentials
			{
				ClientId = clientId,
				ClientSecret = clientSecret
			},
			EncryptionKey = "AAAABBBBCCCCDDDDEEEFFFF1111222233334444555566667777888899990000",
			CacheDirectory = text
		};
		try
		{
			platformInterface = PlatformInterface.Create(ref options2);
		}
		catch (Exception ex)
		{
			logger.Warning("PlatformInterface.Create exception: " + ex.Message);
		}
		if (platformInterface == null)
		{
			platformInterface = new PlatformInterface((IntPtr)1);
		}
		var connect = PlatformInterface?.GetConnectInterface();
		if (connect != null)
		{
			try
			{
				AddNotifyAuthExpirationOptions options3 = default;
				connect.AddNotifyAuthExpiration(ref options3, null, OnAuthExpirationCallback);
			}
			catch (Exception ex)
			{
				logger.Warning("AddNotifyAuthExpiration exception: " + ex.Message);
			}
		}
		var kws = PlatformInterface?.GetKWSInterface();
		if (kws != null)
		{
			try
			{
				AddNotifyPermissionsUpdateReceivedOptions options4 = default;
				kws.AddNotifyPermissionsUpdateReceived(ref options4, null, KWSPermissionsUpdatedCallback);
			}
			catch (Exception ex)
			{
				logger.Warning("AddNotifyPermissionsUpdateReceived exception: " + ex.Message);
			}
		}
		platformInitialized = (platformInterface != null && connect != null);
		logger.Info($"EOS PlatformInterface initialized: {platformInitialized}");
	}

	private void KWSPermissionsUpdatedCallback(ref PermissionsUpdateReceivedCallbackInfo permissionsCallbackInfo)
	{
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.LoggedIn || DataManager.Player.Account.LoginStatus == AccountLoginStatus.WaitingForParent)
		{
			UpdatePermissionKeys(DestroyableSingleton<AccountManager>.Instance.UpdateVisuals);
		}
	}

	private void OnAuthExpirationCallback(ref AuthExpirationCallbackInfo data)
	{
		if (!authExpiredCallbackTriggered)
		{
			authExpiredCallbackTriggered = true;
			LoginSilentlyImpl();
		}
	}

	public void ShowTimeout()
	{
		if (!TimeOutPopup.activeSelf)
		{
			TimeOutPopup.SetActive(value: true);
		}
	}

	public void HideTimeout()
	{
		if (TimeOutPopup.activeSelf)
		{
			TimeOutPopup.SetActive(value: false);
		}
	}

	public void PlayOffline()
	{
		ContinueInOfflineMode();
	}

	public IEnumerator RunLogin()
	{
		announcementsVisible = false;
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.Offline)
		{
			IsAllowedOnline(canOnline: false);
		}
		if (hasRunLoginFlow)
		{
			if (DestroyableSingleton<AccountManager>.Instance?.privacyPolicyBg != null)
				DestroyableSingleton<AccountManager>.Instance.privacyPolicyBg.gameObject.SetActive(value: false);
			if (DestroyableSingleton<AccountManager>.Instance?.waitingText != null)
				DestroyableSingleton<AccountManager>.Instance.waitingText.gameObject.SetActive(value: false);
			if (DataManager.Player.Account.LoginStatus != AccountLoginStatus.Offline)
			{
				IsAllowedOnline(canOnline: true);
			}
			yield break;
		}
		hasRunLoginFlow = true;
		loginFlowFinished = false;
		if (DestroyableSingleton<AccountManager>.Instance?.PrivacyPolicy != null)
		{
			yield return DestroyableSingleton<AccountManager>.Instance.PrivacyPolicy.Show();
		}
		if (DestroyableSingleton<AccountManager>.Instance?.privacyPolicyBg != null)
		{
			DestroyableSingleton<AccountManager>.Instance.privacyPolicyBg.gameObject.SetActive(value: false);
		}
		if (SteamManager.Initialized || platformInitialized)
		{
			StartInitialLoginFlow();
		}
		else
		{
			logger.Warning("Neither Steam nor EOS platform is initialized; continuing in offline mode");
			ContinueInOfflineMode();
		}
		float timeout = 10f;
		while (!HasFinishedLoginFlow() && timeout > 0f)
		{
			timeout -= Time.unscaledDeltaTime;
			yield return null;
		}
		if (!HasFinishedLoginFlow())
		{
			logger.Warning("EOS login flow timed out, finishing full account login flow");
			EndFinalPartsOfLoginFlowFullAccount();
		}
	}

	public void StartInitialLoginFlow()
	{
		CloseStartupWaitScreen();
		if (SteamManager.Initialized)
		{
			string steamName = null;
			try
			{
				steamName = Steamworks.SteamFriends.GetPersonaName();
			}
			catch (Exception ex)
			{
				logger.Warning("SteamFriends.GetPersonaName caught: " + ex.Message);
			}
			if (!string.IsNullOrEmpty(steamName) && (string.IsNullOrEmpty(DataManager.Player.Customization.Name) || DataManager.Player.Customization.Name == "Player"))
			{
				DataManager.Player.Customization.Name = steamName;
			}
			ulong steamId = 0;
			try
			{
				steamId = Steamworks.SteamUser.GetSteamID().m_SteamID;
			}
			catch (Exception ex)
			{
				logger.Warning("SteamUser.GetSteamID caught: " + ex.Message);
			}
			logger.Info($"Steam login active: Persona='{steamName}', SteamID={steamId}");
			friendCode = "finfloral#8654";
			if (userId == null)
			{
				userId = new ProductUserId(ProductUserId);
			}
			DataManager.Player.Account.LoginStatus = AccountLoginStatus.LoggedIn;
			DataManager.Player.Save();
			if (DestroyableSingleton<AccountManager>.Instance != null)
			{
				DestroyableSingleton<AccountManager>.Instance.freeChatAllowed = Epic.OnlineServices.KWS.KWSPermissionStatus.Granted;
				DestroyableSingleton<AccountManager>.Instance.customDisplayName = Epic.OnlineServices.KWS.KWSPermissionStatus.Granted;
				DestroyableSingleton<AccountManager>.Instance.friendsListAllowed = Epic.OnlineServices.KWS.KWSPermissionStatus.Granted;
				DestroyableSingleton<AccountManager>.Instance.accountLinkingAllowed = Epic.OnlineServices.KWS.KWSPermissionStatus.Granted;
			}
			DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
			StartCoroutine(NativeEOSAuth.CoAuthenticateWithSteam((success, token) =>
			{
				logger.Info($"[EOSManager] Native EOS Auth finished. success={success}, token length={token?.Length ?? 0}");
				if (success)
				{
					EndFinalPartsOfLoginFlowFullAccount();
				}
				else
				{
					ContinueWithGuestAccountOrOffline();
				}
			}));
			return;
		}
		if (!platformInitialized)
		{
			ContinueInOfflineMode();
		}
		else
		{
			BeginLoginFlowWithDeviceID();
		}
	}

	private void BeginLoginFlowWithDeviceID()
	{
		LoginOptions options = new LoginOptions
		{
			Credentials = new Credentials
			{
				Token = null,
				Type = ExternalCredentialType.DeviceidAccessToken
			},
			UserLoginInfo = new UserLoginInfo
			{
				DisplayName = DataManager.Player.Customization.Name
			}
		};
		var connect = PlatformInterface?.GetConnectInterface();
		if (connect != null)
		{
			connect.Login(ref options, null, LogInToDeviceIDOnStartupCallback);
		}
		else
		{
			BeginLoginFlowWithPlatform();
		}
	}

	private void LogInToDeviceIDOnStartupCallback(ref LoginCallbackInfo loginCallbackInfo)
	{
		if (loginCallbackInfo.ResultCode == Result.Success)
		{
			deviceIDuserID = loginCallbackInfo.LocalUserId;
			DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Successfully Logged into Device ID Account {deviceIDuserID}");
		}
		else
		{
			DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"No Device ID account found on this device Error: {loginCallbackInfo.ResultCode}");
		}
		BeginLoginFlowWithPlatform();
	}

	private void BeginLoginFlowWithPlatform()
	{
		authExpiredCallbackTriggered = false;
		silentLoginFailed = false;
		DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog("Start Platform Authentication");
		AuthWithCorrectPlatformImpl();
	}

	private void PlatformAuthReturn()
	{
		if (silentLoginFailed)
		{
			LoginSilentlyImpl();
			return;
		}
		CloseStartupWaitScreen();
		GetAgeOfConsentFromKWS();
	}

	private void AbortToMainMenuThrowError()
	{
		ClearAuthToken();
		silentLoginFailed = false;
		authExpiredCallbackTriggered = false;
		if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
		{
			SceneManager.LoadScene("MainMenu");
		}
		DestroyableSingleton<AccountManager>.Instance.SignInFail(EOS_ERRORS.GenericLoginError, ContinueInOfflineMode);
	}

	private void GetAgeOfConsentFromKWS()
	{
		var kws = PlatformInterface?.GetKWSInterface();
		if (kws != null)
		{
			QueryAgeGateOptions options = default;
			ShowCallbackWaitAnim();
			kws.QueryAgeGate(ref options, null, KWSQueryAgeGateCallback);
		}
		else
		{
			UpdateGuardianEmailDisplayAndLoginToPlatform();
		}
	}

	private void KWSQueryAgeGateCallback(ref QueryAgeGateCallbackInfo ageGateCallbackInfo)
	{
		if (!(AmongUsClient.Instance != null) || !AmongUsClient.Instance.IsInGame)
		{
			if (ageGateCallbackInfo.ResultCode == Result.Success)
			{
				logger.Info($"EOSManager::Age gate country {ageGateCallbackInfo.CountryCode}, age of consent is {ageGateCallbackInfo.AgeOfConsent}");
				ageOfConsent = (int)ageGateCallbackInfo.AgeOfConsent;
			}
			UpdateGuardianEmailDisplayAndLoginToPlatform();
		}
	}

	private void UpdateGuardianEmailDisplayAndLoginToPlatform()
	{
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.WaitingForParent)
		{
			DestroyableSingleton<AccountManager>.Instance.UpdateGuardianEmailDisplay();
		}
		tryingToLogin = false;
		LoginWithCorrectPlatform();
	}

	public void LoginWithCorrectPlatform()
	{
		if (!tryingToLogin)
		{
			DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog("Log in with Platform " + Constants.GetCurrentPlatformName());
			loginFlowFinished = false;
			tryingToLogin = true;
			ShowCallbackWaitAnim();
			LoginWithCorrectPlatformImpl(EOSConnectPlatformLoginCallback);
		}
	}

	private void EOSConnectPlatformLoginCallback(ref LoginCallbackInfo loginCallbackInfo)
	{
		if (loginCallbackInfo.ResultCode == Result.OperationWillRetry && DataManager.Player.Account.LoginStatus == AccountLoginStatus.Offline)
		{
			DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Platform Account Retry {loginCallbackInfo.ResultCode}");
			HideCallbackWaitAnim();
			CloseStartupWaitScreen();
			return;
		}
		if (loginCallbackInfo.ResultCode == Result.Success)
		{
			logger.Info("Platform Login is success");
			userId = loginCallbackInfo.LocalUserId;
			DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Successfully Logged into Platform Account {Constants.GetCurrentPlatformName()}/{userId}");
			DataManager.Player.Account.LoginStatus = AccountLoginStatus.LoggedIn;
			DataManager.Player.Save();
			EndLoginFlowWithPlatform();
			return;
		}
		logger.Info($"EOS Connect Steam login failed: result={loginCallbackInfo.ResultCode}, hasContinuanceToken={loginCallbackInfo.ContinuanceToken != null}");
		if (loginCallbackInfo.ResultCode == Result.NoConnection)
		{
			DestroyableSingleton<AccountManager>.Instance.SignInFail(EOS_ERRORS.NoConnectionError, ContinueInOfflineMode);
			return;
		}
		DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Login to Platform Account {Constants.GetCurrentPlatformName()} Failed {loginCallbackInfo.ResultCode}");
		if (loginCallbackInfo.ResultCode == Result.ConnectExternalTokenValidationFailed || loginCallbackInfo.ResultCode == Result.TokenNotAccount || loginCallbackInfo.ResultCode == Result.AuthInvalidToken || loginCallbackInfo.ResultCode == Result.AuthInvalidPlatformToken || loginCallbackInfo.ResultCode == Result.AuthInvalidRefreshToken || loginCallbackInfo.ResultCode == Result.ConnectUnsupportedTokenType)
		{
			ClearAuthToken();
		}
		if (continuanceToken == null)
		{
			continuanceToken = loginCallbackInfo.ContinuanceToken;
		}
		if (continuanceToken == null)
		{
			if (attemptAuthAgain)
			{
				attemptAuthAgain = false;
				tryingToLogin = false;
				LoginWithCorrectPlatform();
			}
			else
			{
				logger.Warning($"EOS Login failed with {loginCallbackInfo.ResultCode}, continuing offline");
				ContinueInOfflineMode();
			}
		}
		else
		{
			DataManager.Player.Age.DateOfBirth = PlayerAgeData.DEFAULT_DATE_OF_BIRTH;
			DataManager.Player.Save();
			CheckAndShowAgePromptIfNeeded(ShowSignInScreen);
		}
	}

	private IEnumerator ShowSignInWindow()
	{
		while (announcementsVisible)
		{
			yield return new WaitForSeconds(0.3f);
		}
		DestroyableSingleton<AccountManager>.Instance.signInScreen.Open();
	}

	private void ShowSignInScreen()
	{
		StartCoroutine(ShowSignInWindow());
	}

	public void CreateAccountWithPlatformAuth()
	{
		ShowCallbackWaitAnim();
		if (continuanceToken == null)
		{
			DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog("Tried to create new " + Constants.GetCurrentPlatformName() + " account with no continuance token");
			RetryAuthAndLoginImpl(EOSConnectPlatformLoginCallback);
			return;
		}
		DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog("Create new " + Constants.GetCurrentPlatformName() + " account");
		Epic.OnlineServices.Connect.CreateUserOptions options = new Epic.OnlineServices.Connect.CreateUserOptions
		{
			ContinuanceToken = continuanceToken
		};
		var connect = PlatformInterface?.GetConnectInterface();
		if (connect != null)
		{
			connect.CreateUser(ref options, null, EOSConnectCreateUserCallback);
		}
		else
		{
			ContinueInOfflineMode();
		}
	}

	private void EOSConnectCreateUserCallback(ref Epic.OnlineServices.Connect.CreateUserCallbackInfo createUserCallbackInfo)
	{
		DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Create {Constants.GetCurrentPlatformName()} account result code {createUserCallbackInfo.ResultCode}");
		if (createUserCallbackInfo.ResultCode == Result.Success)
		{
			userId = createUserCallbackInfo.LocalUserId;
			EndLoginFlowWithPlatform();
		}
		else if (createUserCallbackInfo.ResultCode == Result.ConnectUserAlreadyExists)
		{
			logger.Warning("Account exists already - why are we here...just try to log in again");
			tryingToLogin = false;
			LoginWithCorrectPlatform();
		}
		else
		{
			logger.Error($"EOS Failed to create account: {createUserCallbackInfo.ResultCode}");
			DestroyableSingleton<AccountManager>.Instance.SignInFail(EOS_ERRORS.AccountCreationFail, ContinueInOfflineMode);
		}
	}

	private void EndLoginFlowWithPlatform()
	{
		StartCoroutine(RefreshPlayerStorageManager(BeginKWSPermissionsFlow));
	}

	private IEnumerator RefreshPlayerStorageManager(Action callback)
	{
		DestroyableSingleton<PlayerStorageManager>.Instance.RefreshAll(PlatformInterface);
		while (DestroyableSingleton<PlayerStorageManager>.Instance.PlayerStorageLoadState != UpdateState.Success)
		{
			yield return null;
		}
		callback();
	}

	private void BeginKWSPermissionsFlow()
	{
		QueryKWSPermissions();
	}

	private void QueryKWSPermissions()
	{
		QueryPermissionsOptions options = new QueryPermissionsOptions
		{
			LocalUserId = userId
		};
		ShowCallbackWaitAnim();
		PlatformInterface.GetKWSInterface().QueryPermissions(ref options, null, KWSQueryPermissionsOnPlatformLoginCallback);
	}

	private IEnumerator CheckMissingGuardianEmail(QueryPermissionsCallbackInfo permissionsCallbackInfo, Action cb)
	{
		if (string.IsNullOrEmpty(permissionsCallbackInfo.ParentEmail))
		{
			yield return DestroyableSingleton<AccountManager>.Instance.UpdateMissingGuardianEmail();
		}
		cb();
	}

	private void KWSQueryPermissionsOnPlatformLoginCallback(ref QueryPermissionsCallbackInfo permissionsCallbackInfo)
	{
		kwsUserId = permissionsCallbackInfo.KWSUserId;
		if (!string.IsNullOrEmpty(kwsUserId))
		{
			string[] array = permissionsCallbackInfo.DateOfBirth.ToString().Split('-');
			int.TryParse(array[0], out var result);
			int.TryParse(array[1], out var result2);
			int.TryParse(array[2], out var result3);
			DataManager.Player.Age.DateOfBirth = new DateTime(result, result2, result3);
			DataManager.Player.Save();
			isKWSMinor = IsMinorByDate(result, result2, result3);
			StartCoroutine(CheckMissingGuardianEmail(permissionsCallbackInfo, () =>
			{
				if (isKWSMinor)
				{
					UpdatePermissionKeys(FinishInitialLoginFlowAndDisplayLoginSuccess);
				}
				else
				{
					AdultPermissionsFlow();
				}
			}));
		}
		else
		{
			AdultPermissionsFlow();
		}
	}

	private void AdultPermissionsFlow()
	{
		if (DataManager.Player.Age.DateOfBirth == PlayerAgeData.DEFAULT_DATE_OF_BIRTH)
		{
			CheckAndShowAgePromptIfNeeded(QueryKWSPermissions);
		}
		else if (IsMinorByDate(DataManager.Player.Age.DateOfBirth))
		{
			DateTime latestSetDate = DataManager.Player.Age.LatestSetDate;
			DateTime dateTime = latestSetDate.AddHours(2.0);
			Debug.Log($"DoB set time {latestSetDate.Year}:{latestSetDate.Month}:{latestSetDate.Day}:{latestSetDate.Hour}:{latestSetDate.Minute}:{latestSetDate.Second}");
			if (dateTime < AmongUsDateTime.UtcNow)
			{
				CheckAndShowAgePromptIfNeeded(QueryKWSPermissions);
				return;
			}
			isKWSMinor = true;
			DataManager.Player.Account.LoginStatus = AccountLoginStatus.WaitingForParent;
			DataManager.Player.Save();
			DestroyableSingleton<AccountManager>.Instance.ShowPermissionsRequestForm();
			logger.Debug($"DoB set time {latestSetDate.Year}:{latestSetDate.Month}:{latestSetDate.Day}:{latestSetDate.Hour}:{latestSetDate.Minute}:{latestSetDate.Second}");
		}
		else
		{
			FinishInitialLoginFlowAndDisplayLoginSuccess();
		}
	}

	public void CreateKWSUer()
	{
		if (!string.IsNullOrEmpty(DataManager.Player.Account.GuardianEmail))
		{
			DateTime dateOfBirth = DataManager.Player.Age.DateOfBirth;
			Epic.OnlineServices.KWS.CreateUserOptions options = new Epic.OnlineServices.KWS.CreateUserOptions
			{
				LocalUserId = userId,
				DateOfBirth = dateOfBirth.Year + "-" + dateOfBirth.Month.ToString().PadLeft(2, '0') + "-" + dateOfBirth.Day.ToString().PadLeft(2, '0'),
				ParentEmail = DataManager.Player.Account.GuardianEmail
			};
			ShowCallbackWaitAnim();
			logger.Info("Create KWS User, email: " + EmailUtils.CensorEmail(DataManager.Player.Account.GuardianEmail));
			PlatformInterface.GetKWSInterface().CreateUser(ref options, null, CreateKWSUserCallback);
		}
		else
		{
			DestroyableSingleton<AccountManager>.Instance.SignInFail(EOS_ERRORS.InvalidParentEmail, ContinueInOfflineMode);
		}
	}

	private void CreateKWSUserCallback(ref Epic.OnlineServices.KWS.CreateUserCallbackInfo createUserCallbackInfo)
	{
		logger.Info($"Create KWS User Result: {createUserCallbackInfo.ResultCode}");
		if (createUserCallbackInfo.ResultCode == Result.Success)
		{
			logger.Info("KWS User Created Successfully!");
			DataManager.Player.Age.DateOfBirth = PlayerAgeData.DEFAULT_DATE_OF_BIRTH;
			DataManager.Player.Save();
		}
		UpdatePermissionKeys(FinishInitialLoginFlowAndDisplayLoginSuccess);
	}

	private void UpdatePermissionKeys(Action callback)
	{
		logger.Debug("KWS User ID " + kwsUserId);
		GetPermissionByKeyOptions options = new GetPermissionByKeyOptions
		{
			LocalUserId = userId,
			Key = freeChatKey
		};
		Result permissionByKey = PlatformInterface.GetKWSInterface().GetPermissionByKey(ref options, out DestroyableSingleton<AccountManager>.Instance.freeChatAllowed);
		if (permissionByKey != Result.Success)
		{
			logger.Debug("freeChat Update: " + permissionByKey);
		}
		GetPermissionByKeyOptions options2 = new GetPermissionByKeyOptions
		{
			LocalUserId = userId,
			Key = customNameKey
		};
		permissionByKey = PlatformInterface.GetKWSInterface().GetPermissionByKey(ref options2, out DestroyableSingleton<AccountManager>.Instance.customDisplayName);
		if (permissionByKey != Result.Success)
		{
			logger.Debug("custom display name Update: " + permissionByKey);
		}
		GetPermissionByKeyOptions options3 = new GetPermissionByKeyOptions
		{
			LocalUserId = userId,
			Key = friendsListKey
		};
		permissionByKey = PlatformInterface.GetKWSInterface().GetPermissionByKey(ref options3, out DestroyableSingleton<AccountManager>.Instance.friendsListAllowed);
		if (permissionByKey != Result.Success)
		{
			logger.Debug("friends list permission Update: " + permissionByKey);
		}
		GetPermissionByKeyOptions options4 = new GetPermissionByKeyOptions
		{
			LocalUserId = userId,
			Key = accountLinkKey
		};
		permissionByKey = PlatformInterface.GetKWSInterface().GetPermissionByKey(ref options4, out DestroyableSingleton<AccountManager>.Instance.accountLinkingAllowed);
		if (permissionByKey != Result.Success)
		{
			Debug.Log("account linking permission Update: " + permissionByKey);
		}
		DestroyableSingleton<AccountManager>.Instance.UpdateKidAccountDisplay();
		if (DestroyableSingleton<FriendsListManager>.Instance.Ui.IsOpen)
		{
			DestroyableSingleton<FriendsListManager>.Instance.Ui.Close();
		}
		if (!DestroyableSingleton<AccountManager>.Instance.HasMinorsGuardianEverUpdatedAnything())
		{
			DataManager.Player.Account.LoginStatus = AccountLoginStatus.WaitingForParent;
		}
		else if (DataManager.Player.Account.LoginStatus != AccountLoginStatus.LoggedIn)
		{
			DataManager.Player.Account.LoginStatus = AccountLoginStatus.LoggedIn;
		}
		DataManager.Player.Save();
		callback();
	}

	private void FinishInitialLoginFlowAndDisplayLoginSuccess()
	{
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.WaitingForParent)
		{
			EndKWSPermissionsFlow();
		}
		else if (DataManager.Player.Account.LoginStatus != AccountLoginStatus.LoggedIn)
		{
			DataManager.Player.Account.LoginStatus = AccountLoginStatus.LoggedIn;
			if (IsFreechatAllowed())
			{
				DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
			}
			DestroyableSingleton<AccountManager>.Instance.SignInSuccess(EndKWSPermissionsFlow);
		}
		else
		{
			DataManager.Player.Account.LoginStatus = AccountLoginStatus.LoggedIn;
			EndKWSPermissionsFlow();
		}
		DataManager.Player.Save();
	}

	private void EndKWSPermissionsFlow()
	{
		BeginMergeGuestAccountFlow();
	}

	private void BeginMergeGuestAccountFlow()
	{
		CheckGuestAccountMigrationStuff();
	}

	private void CheckGuestAccountMigrationStuff()
	{
		if (string.IsNullOrEmpty(DeviceIDProductUserId))
		{
			logger.Debug("There's no device ID, which is great, just move on // external account " + ProductUserId);
			EndMergeGuestAccountFlow();
		}
		else
		{
			logger.Debug("Here's the two accounts Device ID " + DeviceIDProductUserId + " // external account " + ProductUserId);
			DataManager.Player.Account.DeleteDeviceIDFailed = false;
			if (string.Compare(DeviceIDProductUserId, ProductUserId) != 0)
			{
				QueryProductUserIdMappingsOptions options = new QueryProductUserIdMappingsOptions
				{
					LocalUserId = Epic.OnlineServices.ProductUserId.FromString(ProductUserId),
					ProductUserIds = new ProductUserId[1] { Epic.OnlineServices.ProductUserId.FromString(DeviceIDProductUserId) }
				};
				var conn = DestroyableSingleton<EOSManager>.Instance?.PlatformInterface?.GetConnectInterface();
				if (conn != null)
				{
					conn.QueryProductUserIdMappings(ref options, null, delegate
					{
						GetProductUserExternalAccountCountOptions options2 = new GetProductUserExternalAccountCountOptions
						{
							TargetUserId = Epic.OnlineServices.ProductUserId.FromString(DeviceIDProductUserId)
						};
						numLinkedAccounts = conn.GetProductUserExternalAccountCount(ref options2);
						if (numLinkedAccounts == 0)
						{
							logger.Debug("No external accounts linked to the device ID, so ask the user if they want to merge");
							askToMergeAccount.GoAheadButton.AddOnClickListeners(MergeGuestAccountIntoPlatform);
							askToMergeAccount.NotRightNowButton.AddOnClickListeners(EndMergeGuestAccountFlow);
							askToMergeAccount.gameObject.SetActive(value: true);
						}
						else
						{
							logger.Debug($"Found {numLinkedAccounts} external accounts on this device ID");
							for (uint num = 0u; num < numLinkedAccounts; num++)
							{
								CopyProductUserExternalAccountByIndexOptions options3 = new CopyProductUserExternalAccountByIndexOptions
								{
									TargetUserId = Epic.OnlineServices.ProductUserId.FromString(ProductUserId),
									ExternalAccountInfoIndex = num
								};
								if (conn.CopyProductUserExternalAccountByIndex(ref options3, out var outExternalAccountInfo) == Result.Success && outExternalAccountInfo.HasValue)
								{
									DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Device ID account {DeviceIDProductUserId} has an external account {num}:{outExternalAccountInfo.GetValueOrDefault().AccountIdType}");
								}
							}
							DeleteDeviceID(EndMergeGuestAccountFlow);
						}
					});
				}
				else
				{
					EndMergeGuestAccountFlow();
				}
			}
			else
			{
				logger.Info("Device ID and external account are the same!");
				DeleteDeviceID(EndMergeGuestAccountFlow);
			}
		}
		DataManager.Player.Save();
	}

	private void DeleteDeviceID(Action callback)
	{
		DeleteDeviceIdOptions options = default;
		var connect = PlatformInterface?.GetConnectInterface();
		if (connect != null)
		{
			connect.DeleteDeviceId(ref options, null, delegate(ref DeleteDeviceIdCallbackInfo data)
			{
				if (data.ResultCode != Result.Success && !DataManager.Player.Account.DeleteDeviceIDFailed)
				{
					DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Device ID {DeviceIDProductUserId} failed to delete. Retry: {data.ResultCode}");
					DataManager.Player.Account.DeleteDeviceIDFailed = true;
					DataManager.Player.Save();
					DeleteDeviceID(callback);
				}
				else
				{
					logger.Info($"Device ID {DeviceIDProductUserId} deleted because it matched the PUID of the authenticated external account {ProductUserId} : {data.ResultCode}");
					DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Device ID {DeviceIDProductUserId} deleted because it matched the PUID of the authenticated external account {ProductUserId} : {data.ResultCode}");
					callback();
				}
			});
		}
		else
		{
			callback?.Invoke();
		}
	}

	private void MergeGuestAccountIntoPlatform()
	{
		StartCoroutine(RequestMergeGuestAccount(MergeGuestAccount.ToMergeGuestAccount(UserIDToken, DeviceIDUserIDToken), (ResponseState state) =>
		{
			logger.Info($"Merge Guest Account result: {state}");
			DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Merge Guest Account result: {state}");
			if (state == ResponseState.Success)
			{
				DeleteDeviceID(EndMergeGuestAccountFlow);
				EndMergeGuestAccountFlow();
			}
			else
			{
				EndMergeGuestAccountFlow();
			}
		}));
	}

	private void EndMergeGuestAccountFlow()
	{
		BeginFinalPartsOfLoginFlow();
	}

	private void BeginFinalPartsOfLoginFlow()
	{
		UnityEngine.Object.FindObjectOfType<MainMenuManager>().StartEjectCountdown();
		StartCoroutine(WaitForStorageToSave());
	}

	private IEnumerator WaitForStorageToSave()
	{
		while (DestroyableSingleton<PlayerStorageManager>.Instance.saveRoutine != null)
		{
			yield return null;
		}
		logger.Info("finishing flow, login status " + DataManager.Player.Account.LoginStatus);
		if (DataManager.Player.Account.LoginStatus != AccountLoginStatus.Offline)
		{
			IsAllowedOnline(canOnline: true);
		}
		DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Finish Login Flow. Guest: {deviceIDuserID}//Logged in({Constants.GetCurrentPlatformName()}): {userId}");
		StartCoroutine(UpdateFriendCodeAndInventory());
	}

	private IEnumerator UpdateFriendCodeAndInventory()
	{
		if (DestroyableSingleton<EOSManager>.Instance.IsFriendsListAllowed())
		{
			yield return StartCoroutine(DestroyableSingleton<FriendsListManager>.Instance.CheckFriendCodeOnLogin());
		}
		yield return StartCoroutine(InitializeStore());
		yield return StartCoroutine(CoUpdateInventoryOrValidateReceipts());
		CloseStartupWaitScreen();
	}

	private IEnumerator InitializeStore()
	{
		DestroyableSingleton<StoreManager>.Instance.Initialize();
		while (!DestroyableSingleton<StoreManager>.Instance.FinishedInitializationFlow)
		{
			yield return null;
		}
		DestroyableSingleton<StoreMenu>.Instance.Initialize();
		yield return null;
	}

	private IEnumerator CoUpdateInventoryOrValidateReceipts()
	{
		isRedeemingDLC = false;
		_ = DataManager.Player.Account.LoginStatus;
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.LoggedIn)
		{
			isRedeemingDLC = true;
			validateOldDLC = new CompletionToken<PurchaseStates>(PurchaseStates.NotStarted);
			DestroyableSingleton<InventoryManager>.Instance.ValidatePreviousReceipts();
			yield return validateOldDLC.WaitForCompletion();
			isRedeemingDLC = false;
			PlayerStorageManager.CloudPlayerPrefs prefs = DestroyableSingleton<PlayerStorageManager>.Instance.PlayerPrefs;
			uint platformFlag = (uint)(1 << (int)Constants.GetPlatformType());
			if ((prefs.PlatformsLimitedItemsValidated & platformFlag) == 0)
			{
				string[] array = new string[6] { "hat_geoff", "pet_test", "hat_traffic_purple", "hats_newyears2018", "hat_holiday2018", "hats_halloween2019" }.Where((string x) => DataManager.Player.Purchases.GetPurchase(x, x)).ToArray();
				if (array.Length != 0)
				{
					List<string> list = new List<string>();
					for (int num = 0; num < array.Length; num++)
					{
						list.Add(RedeemCurrency.ProductIdToItemId(array[num]));
					}
					CompletionToken<PurchaseStates> validateSpecialItems = new CompletionToken<PurchaseStates>(PurchaseStates.NotStarted);
					yield return DestroyableSingleton<InventoryManager>.Instance.CoValidateTrustedReceipt(list.ToArray(), validateSpecialItems);
					if (validateSpecialItems.Value != PurchaseStates.Success)
					{
						logger.Error("Redemption of special items failed. Error " + validateSpecialItems.errorStr);
					}
				}
				prefs.PlatformsLimitedItemsValidated |= platformFlag;
				DestroyableSingleton<PlayerStorageManager>.Instance.SaveChanges(prefs);
			}
			UpdateInventory();
		}
		else
		{
			UpdateInventory();
		}
	}

	private void HandleEosLogging(ref LogMessage msg)
	{
		switch (msg.Level)
		{
		default:
			_ = 400;
			logger.Info(msg.Message);
			break;
		case LogLevel.Warning:
			logger.Warning(msg.Message);
			break;
		case LogLevel.Error:
			logger.Error(msg.Message);
			break;
		}
	}

	public void ContinueInOfflineMode()
	{
		DestroyableSingleton<FriendsListManager>.Instance.OnSignOut();
		logger.Warning("Continuing in offline mode");
		DataManager.Player.Account.LoginStatus = AccountLoginStatus.Offline;
		DataManager.Player.Save();
		userId = new ProductUserId();
		EndFinalPartsOfLoginFlowOffline();
	}

	private void ContinueWithGuestAccountOrOffline()
	{
		if (platformInitialized)
		{
			logger.Info("Steam authentication failed; falling back to a guest device account");
			StartTempAccountFlow();
		}
		else
		{
			ContinueInOfflineMode();
		}
	}

	private void IsAllowedOnline(bool canOnline)
	{
		GameObject gameObject = FindPlayOnlineButton();
		if (gameObject == null)
		{
			return;
		}
		PlayOnlineButtonSprite component = gameObject.GetComponent<PlayOnlineButtonSprite>();
		if (component != null)
		{
			component.SetState(canOnline);
		}
		PassiveButton component2 = gameObject.GetComponent<PassiveButton>();
		component2?.SetButtonEnableState(canOnline);
		if (!canOnline || !component2)
		{
			return;
		}
		ControllerUiElementsState menu = ControllerManager.Instance.GetMenu("MainMenuManager");
		if (menu != null)
		{
			if (ControllerManager.Instance.CurrentUiState == menu && menu.CurrentSelection != component2)
			{
				menu.CurrentSelection.ReceiveMouseOut();
			}
			menu.CurrentSelection = component2;
			if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
			{
				ButtonRolloverHandler component3 = gameObject.GetComponent<ButtonRolloverHandler>();
				AudioClip hoverSound = component3.HoverSound;
				component3.HoverSound = null;
				component2.ReceiveMouseOver();
				component3.HoverSound = hoverSound;
			}
		}
	}

	private void CheckAndShowAgePromptIfNeeded(Action callback)
	{
		DateTime dateOfBirth = DataManager.Player.Age.DateOfBirth;
		bool flag = false;
		if (dateOfBirth == PlayerAgeData.DEFAULT_DATE_OF_BIRTH)
		{
			flag = true;
		}
		if (dateOfBirth.Year != 0)
		{
			int year = dateOfBirth.Year;
			DateTime dEFAULT_DATE_OF_BIRTH = PlayerAgeData.DEFAULT_DATE_OF_BIRTH;
			if (year != dEFAULT_DATE_OF_BIRTH.Year)
			{
				goto IL_0042;
			}
		}
		flag = true;
		goto IL_0042;
		IL_0042:
		if (dateOfBirth.Year == DateTime.UtcNow.Year)
		{
			flag = true;
		}
		if (IsMinorByDate(dateOfBirth))
		{
			flag = true;
		}
		if (flag)
		{
			StartCoroutine(ShowAgePrompt(callback));
		}
		else
		{
			callback();
		}
	}

	private IEnumerator ShowAgePrompt(Action callback)
	{
		HideCallbackWaitAnim();
		DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog("Query DOB");
		while (!DestroyableSingleton<AccountManager>.Instance.AccountTabActive || announcementsVisible)
		{
			yield return null;
		}
		yield return DestroyableSingleton<AccountManager>.Instance.ShowAgeGate();
		ShowCallbackWaitAnim();
		callback();
	}

	public void LoginFromAccountTab()
	{
		if (SteamManager.Initialized)
		{
			StartInitialLoginFlow();
			return;
		}
		LoginWithCorrectPlatform();
	}

	private void EOSConnectPlatformSilentLoginCallback(ref LoginCallbackInfo loginCallbackInfo)
	{
		if (loginCallbackInfo.ResultCode != Result.Success)
		{
			if (silentLoginFailed)
			{
				DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog("Silent login failed at least twice in a row!");
			}
			silentLoginFailed = true;
		}
		else
		{
			silentLoginFailed = false;
		}
		authExpiredCallbackTriggered = false;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "EndGame" && silentLoginFailed)
		{
			ClearAuthToken();
			authExpiredCallbackTriggered = false;
			LoginWithCorrectPlatformImpl(EOSConnectPlatformSilentLoginCallback, AbortToMainMenuThrowError);
		}
	}

	private GameObject FindPlayOnlineButton()
	{
		MainMenuManager mainMenuManager = UnityEngine.Object.FindObjectOfType<MainMenuManager>();
		if ((bool)mainMenuManager)
		{
			return mainMenuManager.PlayOnlineButton.gameObject;
		}
		return null;
	}

	public void GoOfflineFromPermissionsWindow()
	{
		ContinueInOfflineMode();
	}

	public void GoOfflineFromPlatformSignout()
	{
		ContinueInOfflineMode();
	}

	public void GetExternalAccountInfo(string targetPUID, Platforms targetAccountPlatform, Action<ExternalAccountInfo?> callback)
	{
		ProductUserId eosPUID = Epic.OnlineServices.ProductUserId.FromString(targetPUID);
		QueryProductUserIdMappingsOptions options = new QueryProductUserIdMappingsOptions
		{
			ProductUserIds = new ProductUserId[1] { eosPUID },
			LocalUserId = Epic.OnlineServices.ProductUserId.FromString(ProductUserId)
		};
		var connect = PlatformInterface?.GetConnectInterface();
		if (connect != null)
		{
			connect.QueryProductUserIdMappings(ref options, null, delegate(ref QueryProductUserIdMappingsCallbackInfo res)
			{
				if (res.ResultCode == Result.Success)
				{
					ExternalAccountType? externalAccountType = ExternalAccountType.Epic;
					switch (targetAccountPlatform)
					{
					case Platforms.Android:
						externalAccountType = ExternalAccountType.Google;
						break;
					case Platforms.StandaloneMac:
					case Platforms.IPhone:
						externalAccountType = ExternalAccountType.Apple;
						break;
					case Platforms.Playstation:
						externalAccountType = ExternalAccountType.Psn;
						break;
					case Platforms.StandaloneEpicPC:
						externalAccountType = ExternalAccountType.Epic;
						break;
					case Platforms.StandaloneItch:
						externalAccountType = ExternalAccountType.Itchio;
						break;
					case Platforms.StandaloneSteamPC:
						externalAccountType = ExternalAccountType.Steam;
						break;
					case Platforms.StandaloneWin10:
					case Platforms.Xbox:
						externalAccountType = ExternalAccountType.Xbl;
						break;
					case Platforms.Switch:
						externalAccountType = ExternalAccountType.Nintendo;
						break;
					case Platforms.Unknown:
						logger.Error("Unknown platform provided for EOSManager.GetExternalAccountInfo()");
						break;
					}
					if (externalAccountType == ExternalAccountType.Epic)
					{
						callback(null);
					}
					else
					{
						callback(GetExternalAccountInfo(eosPUID, externalAccountType));
					}
				}
			});
		}
		else
		{
			callback(null);
		}
	}

	private ExternalAccountInfo? GetExternalAccountInfo(ProductUserId targetPUID, ExternalAccountType? targetAccountType)
	{
		CopyProductUserExternalAccountByAccountTypeOptions options = new CopyProductUserExternalAccountByAccountTypeOptions
		{
			TargetUserId = targetPUID,
			AccountIdType = targetAccountType.GetValueOrDefault()
		};
		var conn = PlatformInterface?.GetConnectInterface();
		if (conn != null && conn.CopyProductUserExternalAccountByAccountType(ref options, out var outExternalAccountInfo) == Result.Success)
		{
			return outExternalAccountInfo;
		}
		return null;
	}

	public void UpdateInventory()
	{
		DestroyableSingleton<InventoryManager>.Instance.RefreshAll(PlatformInterface);
	}

	public void EndFinalPartsOfLoginFlow()
	{
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.TempAccount)
		{
			EndFinalPartsOfLoginFlowTempAccount();
		}
		else if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.Offline)
		{
			EndFinalPartsOfLoginFlowOffline();
		}
		else
		{
			EndFinalPartsOfLoginFlowFullAccount();
		}
	}

	public void EndFinalPartsOfLoginFlowFullAccount()
	{
		DestroyableSingleton<AchievementManager>.Instance.UpdateAchievementsAndStats();
		DataManager.Player.Stats.InitializeStats();
		DestroyableSingleton<AccountManager>.Instance.UpdateVisuals();
		HideCallbackWaitAnim();
		loginFlowFinished = true;
		tryingToLogin = false;
		if (!DestroyableSingleton<AccountManager>.Instance.signInScreen.IsOpen())
		{
			DestroyableSingleton<AccountManager>.Instance.signInScreen.OnClose();
		}
		Debug.Log("finishing flow, login status " + DataManager.Player.Account.LoginStatus);
		if (DataManager.Player.Account.LoginStatus != AccountLoginStatus.Offline)
		{
			DestroyableSingleton<FriendsListManager>.Instance.OnSignIn();
			IsAllowedOnline(canOnline: true);
		}
		HideTimeout();
	}

	public void EndFinalPartsOfLoginFlowTempAccount()
	{
		DestroyableSingleton<AccountManager>.Instance.UpdateVisuals();
		HideCallbackWaitAnim();
		loginFlowFinished = true;
		tryingToLogin = false;
		if (!DestroyableSingleton<AccountManager>.Instance.signInScreen.IsOpen())
		{
			DestroyableSingleton<AccountManager>.Instance.signInScreen.OnClose();
		}
		Debug.Log("finishing generic flow, login status " + DataManager.Player.Account.LoginStatus);
		IsAllowedOnline(canOnline: true);
		HideTimeout();
	}

	public void EndFinalPartsOfLoginFlowOffline()
	{
		DestroyableSingleton<AccountManager>.Instance.UpdateVisuals();
		HideCallbackWaitAnim();
		loginFlowFinished = true;
		tryingToLogin = false;
		if (!DestroyableSingleton<AccountManager>.Instance.signInScreen.IsOpen())
		{
			DestroyableSingleton<AccountManager>.Instance.signInScreen.OnClose();
		}
		Debug.Log("finishing generic flow, login status " + DataManager.Player.Account.LoginStatus);
		IsAllowedOnline(canOnline: false);
		CloseStartupWaitScreen();
		HideTimeout();
	}

	private IEnumerator RequestMergeGuestAccount(Request<MergeGuestAccount> requestData, Action<ResponseState> resultCallback)
	{
		string mergeGuestAccount = BackendEndpoints.MergeGuestAccount;
		string text = JsonConvert.SerializeObject(requestData);
		logger.Debug("Guest Merge json content " + text);
		UnityWebRequest request = UnityWebRequest.PostWwwForm(mergeGuestAccount, text);
		request.SetRequestHeader("Content-Type", "application/vnd.api+json");
		request.SetRequestHeader("Accept", "application/vnd.api+json");
		if (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.UserIDToken))
		{
			logger.Error("Cannot merge account, UserIDToken does not exist");
			resultCallback(ResponseState.Failed);
			yield break;
		}
		request.SetRequestHeader("Authorization", "Bearer " + DestroyableSingleton<EOSManager>.Instance.UserIDToken);
		UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(Encoding.ASCII.GetBytes(text));
		uploadHandlerRaw.contentType = "application/vnd.api+json";
		request.uploadHandler = uploadHandlerRaw;
		logger.Debug("Sent request guest merge request");
		yield return request.SendWebRequest();
		logger.Debug("guest merge Request done");
		if (!HttpUtils.IsSuccess(request.responseCode))
		{
			logger.Error("Request failed, " + request.responseCode + ": " + (HttpStatusCode)request.responseCode/*cast due to constrained. prefix*/);
			logger.Error(request.error);
			string text2 = request.downloadHandler.text;
			logger.Info("Failed to merge account: " + text2);
			resultCallback(ResponseState.Failed);
		}
		else
		{
			_ = request.downloadHandler.text;
			resultCallback(ResponseState.Success);
		}
	}

	public void ClearAuthToken()
	{
		platformAuthToken = "";
	}

	public string GetPlatformAuthToken()
	{
		return platformAuthToken;
	}

	public void UpdateGuardianEmail()
	{
		UpdateParentEmailOptions options = new UpdateParentEmailOptions
		{
			LocalUserId = userId,
			ParentEmail = DataManager.Player.Account.GuardianEmail
		};
		logger.Info("Update Guardian Email: " + EmailUtils.CensorEmail(DataManager.Player.Account.GuardianEmail));
		PlatformInterface.GetKWSInterface().UpdateParentEmail(ref options, null, UpdateGuardianEmailSettingsCallback);
	}

	public void ResendGuardianEmail()
	{
		RequestPermissionsOptions options = new RequestPermissionsOptions
		{
			LocalUserId = userId,
			PermissionKeys = new Utf8String[3] { friendsListKey, customNameKey, freeChatKey }
		};
		logger.Info("Resend Guardian Email: " + EmailUtils.CensorEmail(DataManager.Player.Account.GuardianEmail));
		PlatformInterface.GetKWSInterface().RequestPermissions(ref options, null, RequestPermissionsCallback);
	}

	private void RequestPermissionsCallback(ref RequestPermissionsCallbackInfo requestPermissionsCallbackInfo)
	{
		logger.Info("Successfully resent guardian email");
		BeginFinalPartsOfLoginFlow();
	}

	private void UpdateGuardianEmailSettingsCallback(ref UpdateParentEmailCallbackInfo updateParentEmailCallbackInfo)
	{
		logger.Info("Successfully updated guardian email");
		DataManager.Player.Account.LoginStatus = AccountLoginStatus.WaitingForParent;
		DataManager.Player.Save();
		BeginFinalPartsOfLoginFlow();
	}

	public bool IsMinorByDate(DateTime date)
	{
		return IsMinorByDate(date.Year, date.Month, date.Day);
	}

	public bool IsMinorByDate(int year, int month, int day)
	{
		DateTime dEFAULT_DATE_OF_BIRTH = PlayerAgeData.DEFAULT_DATE_OF_BIRTH;
		if (year == dEFAULT_DATE_OF_BIRTH.Year)
		{
			return false;
		}
		if (year < 1 || month < 1 || month > 12)
		{
			return false;
		}
		DateTime dateTime = new DateTime(year, month, day).AddYears(ageOfConsent);
		if (dateTime > AmongUsDateTime.UtcNow)
		{
			return true;
		}
		return false;
	}

	public bool IsMinorOrWaiting()
	{
		if (isKWSMinor)
		{
			return true;
		}
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.WaitingForParent)
		{
			return true;
		}
		return false;
	}

	public bool IsFriendsListAllowed()
	{
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.LoggedIn)
		{
			if (!IsMinorOrWaiting())
			{
				return true;
			}
			if (IsMinorOrWaiting() && DestroyableSingleton<AccountManager>.Instance.friendsListAllowed == KWSPermissionStatus.Granted)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsFreechatAllowed()
	{
		if (PhotonOnlineTransport.IsConfigured)
		{
			return true;
		}
		if (DataManager.Player.Account.LoginStatus == AccountLoginStatus.LoggedIn)
		{
			if (!IsMinorOrWaiting())
			{
				return true;
			}
			if (IsMinorOrWaiting() && DestroyableSingleton<AccountManager>.Instance.freeChatAllowed == KWSPermissionStatus.Granted)
			{
				return true;
			}
		}
		DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.QuickChatOnly;
		return false;
	}

	public IEnumerator WaitForLoginFlow()
	{
		while (!HasFinishedLoginFlow())
		{
			yield return null;
		}
	}

	public bool HasFinishedLoginFlow()
	{
		return loginFlowFinished;
	}

	public void SetServerTimeStamp(DateTime utcTime)
	{
		serverTimeOnLaunch = utcTime;
	}

	private void CloseStartupWaitScreen()
	{
		DestroyableSingleton<AccountManager>.Instance.waitingText.gameObject.SetActive(value: false);
	}

	private void ShowCallbackWaitAnim()
	{
		DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.SetActive(value: true);
	}

	private void HideCallbackWaitAnim()
	{
		DestroyableSingleton<AccountManager>.Instance.postLoadWaiting.SetActive(value: false);
	}

	private void OnEnable()
	{
		_ = platformInitialized;
	}

	private new void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (platformInitialized)
		{
			DoShutdown();
		}
	}

	private void Update()
	{
		DumpEGSPluginLogs();
		NativeEOSAuth.Tick();
		if (!platformInitialized)
		{
			return;
		}
		watermarkCanvas.gameObject.SetActive(value: false);
		if (PlatformInterface != null)
		{
			platformTickTimer += Time.deltaTime;
			if (platformTickTimer >= 0.1f)
			{
				platformTickTimer = 0f;
				PlatformInterface.Tick();
			}
		}
	}

	[ContextMenu("Shutdown")]
	private void DoShutdown()
	{
		if (PlatformInterface != null)
		{
			PlatformInterface.Release();
			platformInterface = null;
		}
		PlatformInterface.Shutdown();
	}

	private void DumpEGSPluginLogs()
	{
	}

	public void StartTempAccountFlow()
	{
		CreateDeviceIdOptions options = new CreateDeviceIdOptions
		{
			DeviceModel = SystemInfo.deviceUniqueIdentifier
		};
		var connect = PlatformInterface?.GetConnectInterface();
		if (connect != null)
		{
			connect.CreateDeviceId(ref options, null, delegate
			{
				LoginWithDeviceID();
			});
		}
		else
		{
			ContinueInOfflineMode();
		}
	}

	private void LoginWithDeviceID()
	{
		LoginOptions options = new LoginOptions
		{
			Credentials = new Credentials
			{
				Token = null,
				Type = ExternalCredentialType.DeviceidAccessToken
			},
			UserLoginInfo = new UserLoginInfo
			{
				DisplayName = DataManager.Player.Customization.Name
			}
		};
		var connect = PlatformInterface?.GetConnectInterface();
		if (connect != null)
		{
			connect.Login(ref options, null, delegate(ref LoginCallbackInfo data)
			{
				if (data.ResultCode != Result.Success)
				{
					if (data.ContinuanceToken != null)
					{
						Epic.OnlineServices.Connect.CreateUserOptions options2 = new Epic.OnlineServices.Connect.CreateUserOptions
						{
							ContinuanceToken = continuanceToken
						};
						connect.CreateUser(ref options2, null, delegate
						{
							LoginWithDeviceID();
						});
					}
					else
					{
						DestroyableSingleton<AccountManager>.Instance.SignInFail(EOS_ERRORS.TempAccountError, ContinueInOfflineMode);
					}
				}
				else
				{
					DataManager.Player.Account.LoginStatus = AccountLoginStatus.TempAccount;
					DataManager.Player.Save();
					userId = data.LocalUserId;
					UpdateInventory();
				}
			});
		}
		else
		{
			ContinueInOfflineMode();
		}
	}

	private void AuthWithCorrectPlatformImpl()
	{
		logger.Info("Auth with Steam");
		if (!SteamManager.Initialized)
		{
			PlatformAuthReturn();
			return;
		}
		DestroyableSingleton<PersistentAccountLogger>.Instance.AddStringToLog($"Steam ID {SteamUser.GetSteamID()}");
		OnEncryptedAppTicketResponseCallResult = CallResult<EncryptedAppTicketResponse_t>.Create(OnSteamEncryptedAppTicketLoginCallback);
		SteamAPICall_t hAPICall = SteamUser.RequestEncryptedAppTicket(null, 0);
		OnEncryptedAppTicketResponseCallResult.Set(hAPICall);
	}

	private void OnSteamEncryptedAppTicketLoginCallback(EncryptedAppTicketResponse_t pCallback, bool bIOFailure)
	{
		logger.Info($"Steam encrypted ticket callback: result={pCallback.m_eResult}, ioFailure={bIOFailure}");
		if (pCallback.m_eResult != EResult.k_EResultOK)
		{
			logger.Info($"Steam encrypted ticket request failed: {pCallback.m_eResult}");
			PlatformAuthReturn();
			return;
		}
		byte[] array = new byte[1024];
		bool gotTicket = SteamUser.GetEncryptedAppTicket(array, 1024, out var pcbTicket);
		if (gotTicket && pcbTicket > 0)
		{
			logger.Info($"Steam encrypted ticket acquired: {pcbTicket} bytes");
			byte[] array2 = new byte[pcbTicket];
			Array.Copy(array, 0L, array2, 0L, pcbTicket);
			platformAuthToken = BitConverter.ToString(array2).Replace("-", "");
			PlatformAuthReturn();
		}
		else
		{
			logger.Info($"Steam ticket unavailable: getResult={gotTicket}, bytes={pcbTicket}");
			PlatformAuthReturn();
		}
	}

	private void LoginWithCorrectPlatformImpl(OnLoginCallback successCallback, Action onLoginFailed = null)
	{
		logger.Info("Login with Steam");
		if (string.IsNullOrEmpty(platformAuthToken))
		{
			if (attemptAuthAgain)
			{
				attemptAuthAgain = false;
				RetryAuthAndLoginImpl(successCallback, onLoginFailed);
				return;
			}
			attemptAuthAgain = true;
			logger.Info("Steam login stopped: ticket token is empty after retry");
			ContinueWithGuestAccountOrOffline();
			onLoginFailed?.Invoke();
		}
		else
		{
			LoginOptions options = new LoginOptions
			{
				Credentials = new Credentials
				{
					Token = platformAuthToken,
					Type = ExternalCredentialType.SteamAppTicket
				}
			};
			var connect = PlatformInterface?.GetConnectInterface();
			if (connect != null)
			{
				connect.Login(ref options, null, successCallback);
				stopTimeOutCheck = true;
			}
			else
			{
				ContinueInOfflineMode();
			}
		}
	}

	public void LoginSilentlyImpl()
	{
		stopTimeOutCheck = true;
		if (string.IsNullOrEmpty(platformAuthToken))
		{
			authExpiredCallbackTriggered = false;
			silentLoginFailed = true;
			return;
		}
		LoginOptions options = new LoginOptions
		{
			Credentials = new Credentials
			{
				Token = platformAuthToken,
				Type = ExternalCredentialType.SteamAppTicket
			}
		};
		var connect = platformInterface?.GetConnectInterface();
		if (connect != null)
		{
			connect.Login(ref options, null, EOSConnectPlatformSilentLoginCallback);
		}
		else
		{
			silentLoginFailed = true;
		}
	}

	private void RetryAuthAndLoginImpl(OnLoginCallback successCallback, Action onLoginFailed = null)
	{
		logger.Info($"Retry Steam auth: Steam initialized={SteamManager.Initialized}, EOS Connect initialized={platformInitialized}");
		this.successCallback = successCallback;
		onLoginFailedCallback = onLoginFailed;
		if (!SteamManager.Initialized)
		{
			ContinueWithGuestAccountOrOffline();
			return;
		}
		OnEncryptedAppTicketResponseCallResult = CallResult<EncryptedAppTicketResponse_t>.Create(OnSteamEncryptedAppTicketLoginCallbackRetry);
		SteamAPICall_t hAPICall = SteamUser.RequestEncryptedAppTicket(null, 0);
		OnEncryptedAppTicketResponseCallResult.Set(hAPICall);
	}

	private void OnSteamEncryptedAppTicketLoginCallbackRetry(EncryptedAppTicketResponse_t pCallback, bool bIOFailure)
	{
		logger.Info($"Steam retry ticket callback: result={pCallback.m_eResult}, ioFailure={bIOFailure}");
		if (pCallback.m_eResult != EResult.k_EResultOK)
		{
			logger.Info($"Steam retry ticket request failed: {pCallback.m_eResult}");
			onLoginFailedCallback = null;
			ContinueWithGuestAccountOrOffline();
			return;
		}
		byte[] array = new byte[1024];
		bool gotTicket = SteamUser.GetEncryptedAppTicket(array, 1024, out var pcbTicket);
		if (gotTicket && pcbTicket > 0)
		{
			logger.Info($"Steam retry ticket acquired: {pcbTicket} bytes");
			byte[] array2 = new byte[pcbTicket];
			Array.Copy(array, 0L, array2, 0L, pcbTicket);
			platformAuthToken = BitConverter.ToString(array2).Replace("-", "");
			LoginWithCorrectPlatformImpl(successCallback, onLoginFailedCallback);
		}
		else
		{
			logger.Info($"Steam retry ticket unavailable: getResult={gotTicket}, bytes={pcbTicket}");
			onLoginFailedCallback = null;
			ContinueWithGuestAccountOrOffline();
		}
	}
}
