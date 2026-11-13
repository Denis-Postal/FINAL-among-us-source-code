using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using InnerNet;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class MainMenuManager : MonoBehaviour
{
	public AdDataCollectScreen AdsPolicy;

	public PassiveButton PlayOnlineButton;

	public HatManager HatManagerRef;

	public CosmicubeManager CosmicubeManagerRef;

	public PlayerCustomizationMenu playerCustomizationPrefab;

	[SerializeField]
	private AnnouncementPopUp announcementPopUp;

	[SerializeField]
	private GameObject gameModeButtons;

	[SerializeField]
	private GameObject accountButtons;

	[SerializeField]
	private PassiveButton accountStatsButton;

	[SerializeField]
	private GameObject onlineButtons;

	[SerializeField]
	private PassiveButton backButtonOnline;

	[SerializeField]
	private GameObject enterCodeButtons;

	[SerializeField]
	private SpriteMask screenMask;

	[SerializeField]
	private GameObject mainMenuUI;

	[SerializeField]
	private GameObject rightPanelMask;

	[SerializeField]
	private SpriteRenderer screenTint;

	[SerializeField]
	private PassiveButton freePlayButton;

	[SerializeField]
	private PassiveButton entercodeField;

	[SerializeField]
	private PassiveButton redeemcodeField;

	[SerializeField]
	private PassiveButton redeemRewardButton;

	[SerializeField]
	[Header("Left panel buttons")]
	private PassiveButton playButton;

	[SerializeField]
	private PassiveButton inventoryButton;

	[SerializeField]
	private PassiveButton shopButton;

	[SerializeField]
	private PassiveButton myAccountButton;

	[SerializeField]
	private PassiveButton newsButton;

	[SerializeField]
	private PassiveButton settingsButton;

	[SerializeField]
	private PassiveButton creditsButton;

	[SerializeField]
	private PassiveButton quitButton;

	[SerializeField]
	private FindGameButton findGameButton;

	[SerializeField]
	[Header("Default right panel buttons")]
	private PassiveButton playLocalButton;

	[SerializeField]
	private PassiveButton howToPlayButton;

	[SerializeField]
	private PassiveButton accountCTAButton;

	[SerializeField]
	private PassiveButton createGameButton;

	private List<PassiveButton> mainButtons;

	[SerializeField]
	private GameObject creditsScreen;

	[SerializeField]
	private CreateGameOptions createGameScreen;

	[SerializeField]
	private GameObject adsMenu;

	[SerializeField]
	private EjectMainMenu ejectMenu;

	[SerializeField]
	private SpriteRenderer fullScreenSprite;

	[Header("Animations")]
	[SerializeField]
	private Transform enterCodeContainer;

	[SerializeField]
	private GameObject enterCodeHeader;

	[SerializeField]
	private Transform onlineButtonsContainer;

	[SerializeField]
	private GameObject onlineHeader;

	[SerializeField]
	private Transform redeemCodeContainer;

	private const float TIME_SLIDE = 0.3f;

	private const float X_OFFSCREEN_LEFT = -8f;

	private const float X_OFFSCREEN_RIGHT = 7f;

	[Header("Console Controller Navigation")]
	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public List<PassiveButton> disableOnStartup = new List<PassiveButton>();

	[SerializeField]
	private GameObject NewStoreItemsIcon;

	private CosmicubeManager cosmicubeManager;

	private bool animating;

	private bool finishStartup;

	private void Awake()
	{
		ReferenceDataManager.EnsureBooted();
		mainButtons = new List<PassiveButton> { playButton, inventoryButton, shopButton, newsButton, myAccountButton, settingsButton };
		SetUpControllerNav();
		CheckNewItems();
		GameOptionsManager optionsManager = GameOptionsManager.Instance;
		if (optionsManager == null)
		{
			optionsManager = new GameOptionsManager(new UnityLogger());
		}
		optionsManager.Initialize();
		rightPanelMask.gameObject.SetActive(value: false);
		ResetScreen();
		ConnectMainMenuScreenButtonEvents();
		ejectMenu.gameObject.SetActive(value: true);
	}

	public void Start()
	{
		ChatLanguageSet.Instance.Load();
		StartCoroutine(RunStartUp());
		QualitySettings.vSyncCount = (DataManager.Settings.Video.VSync ? 1 : 0);
		announcementPopUp.ActionOnDisable += () =>
		{
			screenMask.enabled = true;
		};
		announcementPopUp.ActionOnEnable += () =>
		{
			screenMask.enabled = false;
		};
		announcementPopUp.SetMainMenuManager(this);
	}

	public void ResetScreen()
	{
		if (DestroyableSingleton<AccountManager>.InstanceExists)
		{
			DestroyableSingleton<AccountManager>.Instance.CloseAccountWindow();
		}
		for (int i = 0; i < mainButtons.Count; i++)
		{
			mainButtons[i].ControllerNav.selectOnRight = null;
		}
		creditsScreen.SetActive(value: false);
		gameModeButtons.SetActive(value: false);
		accountButtons.SetActive(value: false);
		onlineButtons.SetActive(value: false);
		enterCodeButtons.SetActive(value: false);
		createGameScreen.gameObject.SetActive(value: false);
		screenTint.enabled = false;
		accountStatsButton.SetButtonEnableState(DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn);
		redeemRewardButton.SetButtonEnableState(DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn);
		fullScreenSprite.gameObject.SetActive(value: false);
		adsMenu.gameObject.SetActive(value: false);
		ejectMenu.gameObject.SetActive(value: false);
		redeemCodeContainer.gameObject.SetActive(value: false);
		ControllerManager.Instance.SetBackButton(null);
	}

	public void StartEjectCountdown()
	{
		ejectMenu.StartEjectButton();
	}

	public void OpenGameModeMenu()
	{
		if (PlayOnlineButton != null)
		{
			PlayOnlineButton.enabled = true;
			PlayOnlineButton.SetButtonEnableState(true);
		}
		ResetScreen();
		gameModeButtons.SetActive(value: true);
		screenTint.enabled = true;
		for (int i = 0; i < mainButtons.Count; i++)
		{
			if (playLocalButton.gameObject.activeSelf)
			{
				mainButtons[i].ControllerNav.selectOnRight = playLocalButton;
			}
			else if (PlayOnlineButton.enabled)
			{
				mainButtons[i].ControllerNav.selectOnRight = PlayOnlineButton;
			}
			else
			{
				mainButtons[i].ControllerNav.selectOnRight = howToPlayButton;
			}
		}
	}

	public void OpenCreateGame()
	{
		if (!animating)
		{
			if (!DestroyableSingleton<AccountManager>.Instance.CanPlayOnline())
			{
				AmongUsClient.Instance.LastDisconnectReason = DisconnectReasons.NotAuthorized;
				DestroyableSingleton<DisconnectPopup>.Instance.Show();
			}
			else if (DataManager.Player.Ban.IsBanned)
			{
				AmongUsClient.Instance.LastDisconnectReason = DisconnectReasons.IntentionalLeaving;
				DestroyableSingleton<DisconnectPopup>.Instance.Show();
			}
			else
			{
				ControllerManager.Instance.SetCurrentSelected(null);
				StartCoroutine(ShowCreateGameCo());
			}
		}
	}

	private IEnumerator ShowCreateGameCo()
	{
		animating = true;
		fullScreenSprite.gameObject.SetActive(value: true);
		fullScreenSprite.color = Color.clear;
		yield return Effects.ColorFade(fullScreenSprite, Color.clear, Color.black, 0.1f);
		onlineButtons.SetActive(value: false);
		createGameScreen.Show();
		animating = false;
	}

	public void GoBackCreateGame()
	{
		if (!animating)
		{
			StartCoroutine(GoBackCreateGameCo());
		}
	}

	private IEnumerator GoBackCreateGameCo()
	{
		yield return Effects.ColorFade(fullScreenSprite, Color.black, Color.clear, 0.1f);
		fullScreenSprite.gameObject.SetActive(value: false);
		OpenOnlineMenu();
	}

	public void OpenOnlineMenu()
	{
		if (!animating)
		{
			ResetScreen();
			screenTint.enabled = true;
			onlineButtons.SetActive(value: true);
			for (int i = 0; i < mainButtons.Count; i++)
			{
				mainButtons[i].ControllerNav.selectOnRight = createGameButton;
			}
			ControllerManager.Instance.SetBackButton(backButtonOnline);
			ControllerManager.Instance.SetCurrentSelected(createGameButton);
			onlineButtonsContainer.localPosition = new Vector3(0f, onlineButtonsContainer.localPosition.y, onlineButtonsContainer.position.z);
		}
	}

	public void OpenEnterCodeMenu(bool animate)
	{
		if (!animating)
		{
			ResetScreen();
			screenTint.enabled = true;
			enterCodeButtons.SetActive(value: true);
			for (int i = 0; i < mainButtons.Count; i++)
			{
				mainButtons[i].ControllerNav.selectOnRight = entercodeField;
			}
			ControllerManager.Instance.SetCurrentSelected(entercodeField);
			if (animate)
			{
				StartCoroutine(AnimateEnterCodeSlideCo());
				return;
			}
			enterCodeHeader.SetActive(value: true);
			enterCodeContainer.localPosition = new Vector3(0f, enterCodeContainer.localPosition.y, enterCodeContainer.position.z);
			onlineButtonsContainer.localPosition = new Vector3(0f, onlineButtonsContainer.localPosition.y, onlineButtonsContainer.position.z);
		}
	}

	public void OpenRedeemCodeMenu()
	{
		ResetScreen();
		screenTint.enabled = true;
		for (int i = 0; i < mainButtons.Count; i++)
		{
			mainButtons[i].ControllerNav.selectOnRight = redeemcodeField;
		}
		ControllerManager.Instance.SetCurrentSelected(redeemcodeField);
		redeemCodeContainer.gameObject.SetActive(value: true);
	}

	private IEnumerator AnimateEnterCodeSlideCo()
	{
		onlineButtons.SetActive(value: true);
		onlineButtonsContainer.localPosition = new Vector3(0f, onlineButtonsContainer.localPosition.y, onlineButtonsContainer.localPosition.z);
		enterCodeContainer.localPosition = new Vector3(7f, enterCodeContainer.localPosition.y, enterCodeContainer.localPosition.z);
		animating = true;
		onlineHeader.SetActive(value: true);
		enterCodeHeader.SetActive(value: false);
		Vector2 startPosEnterCode = new Vector2(7f, enterCodeContainer.localPosition.y);
		Vector2 endPosEnterCode = new Vector2(0f, enterCodeContainer.localPosition.y);
		yield return Effects.Slide2D(source: new Vector2(0f, onlineButtonsContainer.localPosition.y), dest: new Vector2(-8f, onlineButtonsContainer.localPosition.y), target: onlineButtonsContainer, duration: 0.3f);
		yield return Effects.Slide2D(enterCodeContainer, startPosEnterCode, endPosEnterCode, 0.3f);
		onlineButtons.SetActive(value: false);
		enterCodeHeader.SetActive(value: true);
		animating = false;
	}

	public void ClickBackEnterCode()
	{
		if (!animating)
		{
			StartCoroutine(AnimateEnterCodeExitSlideCo());
		}
	}

	private IEnumerator AnimateEnterCodeExitSlideCo()
	{
		onlineButtons.SetActive(value: true);
		onlineButtonsContainer.localPosition = new Vector3(-8f, onlineButtonsContainer.localPosition.y, onlineButtonsContainer.localPosition.z);
		enterCodeContainer.localPosition = new Vector3(0f, enterCodeContainer.localPosition.y, enterCodeContainer.localPosition.z);
		animating = true;
		onlineHeader.SetActive(value: false);
		enterCodeHeader.SetActive(value: true);
		Vector2 source = new Vector2(0f, enterCodeContainer.localPosition.y);
		Vector2 dest = new Vector2(7f, enterCodeContainer.localPosition.y);
		Vector2 startPosOnline = new Vector2(-8f, onlineButtonsContainer.localPosition.y);
		Vector2 endPosOnline = new Vector2(0f, onlineButtonsContainer.localPosition.y);
		yield return Effects.Slide2D(enterCodeContainer, source, dest, 0.3f);
		yield return Effects.Slide2D(onlineButtonsContainer, startPosOnline, endPosOnline, 0.3f);
		enterCodeHeader.SetActive(value: false);
		onlineHeader.SetActive(value: true);
		animating = false;
		OpenOnlineMenu();
	}

	public void ClickBackOnline()
	{
		if (!animating)
		{
			OpenGameModeMenu();
		}
	}

	public void OpenFindGame()
	{
		if (!animating)
		{
			findGameButton.OnClick();
		}
	}

	public void OpenAccountMenu()
	{
		ResetScreen();
		accountButtons.SetActive(value: true);
		screenTint.enabled = true;
		for (int i = 0; i < mainButtons.Count; i++)
		{
			mainButtons[i].ControllerNav.selectOnRight = accountCTAButton;
		}
	}

	public void OpenCredits()
	{
		ResetScreen();
		creditsScreen.SetActive(value: true);
		screenTint.enabled = true;
	}

	public void CheckNewItems()
	{
		NewStoreItemsIcon.SetActive(DestroyableSingleton<StoreMenu>.Instance.Tabs.Any((StoreTabButton t) => DestroyableSingleton<ReferenceDataManager>.Instance.StoreTabVersions.ContainsKey(t.storeTab) && DataManager.Player.Store.GetStoreTabViewDate(t.storeTab) < Constants.ParseDate(DestroyableSingleton<ReferenceDataManager>.Instance.StoreTabVersions[t.storeTab])));
	}

	private IEnumerator RunStartUp()
	{
		finishStartup = false;
		Constants.SetMainThread();
		// TODO(verify): offline service boundary; skip EOS assets/login and announcement downloads.
		// Native startup activates this display mask after EOS.RunLogin. In this offline build,
		// apply that display state before unavailable cosmetic initialization can abort the iterator.
		// This does not assert login, purchase, or finishStartup readiness.
		rightPanelMask.gameObject.SetActive(true);
		// These local initializers must complete before finishStartup can be set. Existing stubs
		// intentionally block completion; no synthetic login/account/store success is reported.
		UnityEngine.Object.Instantiate(HatManagerRef).Initialize();
		cosmicubeManager = UnityEngine.Object.Instantiate(CosmicubeManagerRef);
		cosmicubeManager.Initialize();
		StoreManager store = DestroyableSingleton<StoreManager>.Instance;
		if (!store.FinishedInitializationFlow) store.Initialize();
		StoreMenu storeMenu = DestroyableSingleton<StoreMenu>.Instance;
		if (!storeMenu.Initialized) storeMenu.Initialize();
		storeMenu.OnSaveDataChanged();
		if (!DataManager.Settings.DoesFileExist()) DataManager.Settings.ForceSave();
		ControllerManager.Instance.NewScene(name, null, DefaultButtonSelected, ControllerSelectable, false);
		AmongUsClient client = DestroyableSingleton<AmongUsClient>.Instance;
		if (client.MenuTarget == AmongUsClient.MainMenuTarget.OnlineMenu) OpenOnlineMenu();
		else if (client.MenuTarget == AmongUsClient.MainMenuTarget.EnterCodeMenu) OpenEnterCodeMenu(false);
		client.MenuTarget = AmongUsClient.MainMenuTarget.None;
		finishStartup = true;
		if (DestroyableSingleton<DisconnectPopup>.Instance.gameObject.activeSelf)
			DestroyableSingleton<DisconnectPopup>.Instance.RegainUIControl();
		yield return null;
	}

	private void CheckAddOns()
	{
	}

	private void LateUpdate()
	{
		if (!ControllerManager.Instance.IsUiControllerActive)
		{
			EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
			if (instance.loginFlowFinished && finishStartup)
			{
				Debug.Log("No current valid menu active, opening main menu scene");
				string menuName = name;
				ControllerManager.Instance.NewScene(menuName, null, DefaultButtonSelected, ControllerSelectable, false);
				ControllerManager instance2 = ControllerManager.Instance;
				ControllerUiElementsState currentUiState = instance2.CurrentUiState;
				currentUiState.CurrentSelection = DefaultButtonSelected;
			}
		}
	}

	public void TransitionToInventory()
	{
		if (finishStartup)
		{
			ResetScreen();
			PlayerCustomizationMenu playerCustomizationMenu = UnityEngine.Object.Instantiate(playerCustomizationPrefab);
			playerCustomizationMenu.Open();
			playerCustomizationMenu.OnClose = (Action)Delegate.Combine(playerCustomizationMenu.OnClose, new Action(ActivateMainMenuUI));
			playerCustomizationMenu.OnOpen = (Action)Delegate.Combine(playerCustomizationMenu.OnOpen, new Action(DeactivateMainMenuUI));
			playerCustomizationMenu.transform.SetParent(Camera.main.transform, worldPositionStays: false);
			playerCustomizationMenu.transform.localPosition = new Vector3(0f, 0f, -20f);
		}
	}

	public void TransitionToShop()
	{
		// Recovered from decomp9; callbacks belong to the persistent StoreMenu instance.
		if (!finishStartup) return;
		// Offline remake: there is no store (StoreManager has no products and nothing can be purchased; every item is unlocked, see
		// PlayerPurchasesData.UnlockEverythingForTesting). Opening the shop only hid the main menu and left a blank screen, so do nothing.
		Debug.Log("[MainMenu] Shop is unavailable in the offline build (all items are unlocked)");
		return;
		ResetScreen();
		StoreMenu menu = DestroyableSingleton<StoreMenu>.Instance;
		menu.OnClose += ActivateMainMenuUI;
		menu.OnOpen += DeactivateMainMenuUI;
		menu.Open();
	}

	public bool IsMainMenuActive()
	{
		return mainMenuUI.activeSelf;
	}

	private void ActivateMainMenuUI()
	{
		mainMenuUI.SetActive(value: true);
		ejectMenu.gameObject.SetActive(value: true);
		StartEjectCountdown();
	}

	private void DeactivateMainMenuUI()
	{
		mainMenuUI.SetActive(value: false);
		if (announcementPopUp.gameObject.activeSelf)
		{
			announcementPopUp.Close();
		}
	}

	private void SetUpControllerNav()
	{
		for (int i = 0; i < mainButtons.Count; i++)
		{
			mainButtons[i].ControllerNav.mode = ControllerNavigation.Mode.Explicit;
			if (i < mainButtons.Count - 1)
			{
				mainButtons[i].ControllerNav.selectOnDown = mainButtons[i + 1];
			}
			else
			{
				mainButtons[i].ControllerNav.selectOnDown = creditsButton;
			}
			if (i > 0)
			{
				mainButtons[i].ControllerNav.selectOnUp = mainButtons[i - 1];
			}
		}
		creditsButton.ControllerNav.selectOnRight = quitButton;
		creditsButton.ControllerNav.selectOnUp = settingsButton;
		quitButton.ControllerNav.selectOnUp = settingsButton;
		quitButton.ControllerNav.selectOnLeft = creditsButton;
	}

	private void ConnectMainMenuScreenButtonEvents()
	{
		accountCTAButton.OnClick.RemoveAllListeners();
		accountCTAButton.OnClick.AddListener(() =>
		{
			DestroyableSingleton<AccountManager>.Instance.OpenAccountWindow();
		});
	}
}
