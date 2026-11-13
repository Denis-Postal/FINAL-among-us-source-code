using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AmongUs;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using Cpp2ILInjected;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class StoreMenu : DestroyableSingleton<StoreMenu>
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Action<ColorChip> _003C_003E9__110_0;

		public static Func<CosmicubeData, bool> _003C_003E9__116_0;

		public static Func<CosmicubeData, DateTime> _003C_003E9__116_1;

		public static Func<StarBundle, float> _003C_003E9__116_2;

		public static Func<BundleData, bool> _003C_003E9__116_3;

		public static Func<CosmicubeData, bool> _003C_003E9__116_4;

		public static Func<BundleData, bool> _003C_003E9__116_5;

		public static Func<BundleData, bool> _003C_003E9__116_6;

		public static Func<RefreshTimer, DateTime> _003C_003E9__117_0;

		public static Action<TextMeshPro> _003C_003E9__128_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal void _003CDeselectAllItems_003Eb__110_0(ColorChip c)
		{
			c.Deselect();
		}

		internal bool _003CReSortCosmeticTabs_003Eb__116_0(CosmicubeData p)
		{
			return false; // TODO(verify): default, original logic unrecoverable
		}

		internal DateTime _003CReSortCosmeticTabs_003Eb__116_1(CosmicubeData p)
		{
			return default; // TODO(verify): default, original logic unrecoverable
		}

		internal float _003CReSortCosmeticTabs_003Eb__116_2(StarBundle p)
		{
			return p.price;
		}

		internal bool _003CReSortCosmeticTabs_003Eb__116_3(BundleData h)
		{
			return false; // TODO(verify): default, original logic unrecoverable
		}

		internal bool _003CReSortCosmeticTabs_003Eb__116_4(CosmicubeData x)
		{
			return false; // TODO(verify): default, original logic unrecoverable
		}

		internal bool _003CReSortCosmeticTabs_003Eb__116_5(BundleData x)
		{
			return false; // TODO(verify): default, original logic unrecoverable
		}

		internal bool _003CReSortCosmeticTabs_003Eb__116_6(BundleData x)
		{
			return false; // TODO(verify): default, original logic unrecoverable
		}

		internal DateTime _003CSetRefreshTimers_003Eb__117_0(RefreshTimer x)
		{
			return default; // TODO(verify): default, original logic unrecoverable
		}

		internal void _003CShowTab_003Eb__128_0(TextMeshPro t)
		{
			GameObject gameObject = t.gameObject;
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	private sealed class _003C_003Ec__DisplayClass109_0
	{
		public StoreChip chip;

		public CosmeticData prod;

		public StoreMenu _003C_003E4__this;

		internal void _003CSetProduct_003Eb__0()
		{
			_003C_003E4__this.PreviewItem(chip, prod);
		}
	}

	private sealed class _003C_003Ec__DisplayClass109_1
	{
		public StoreChip chip;

		public CosmeticData cosmeticData;

		public StoreMenu _003C_003E4__this;

		internal void _003CSetProduct_003Eb__1()
		{
			StoreMenu storeMenu = _003C_003E4__this;
			Action<ColorChip> action = _003C_003Ec._003C_003E9__110_0;
			if (_003C_003Ec._003C_003E9__110_0 == null)
			{
				action = (_003C_003Ec._003C_003E9__110_0 = (ColorChip c) =>
				{
					c.Deselect();
				});
			}
			storeMenu.ColorChips.ForEach(action);
			_003C_003E4__this.PreviewItem(chip, cosmeticData);
		}
	}

	public const string BoughtAdsProductId = "bought_ads";

	private const float MaxItemsPerRow = 4f;

	private const float NumRowsVisible = 2f;

	private const float DefaultAspectRatio = 1.7777778f;

	private HatData[] allHats;

	private SkinData[] allSkins;

	private VisorData[] allVisors;

	private NamePlateData[] allPlates;

	private BundleData[] allBundles;

	private CosmicubeData[] allCubes;

	private PetData[] allPets;

	private CosmeticData[] allFeaturedItems;

	private CosmicubeData[] allFeaturedCubes;

	private BundleData[] allLargeFeaturedBundles;

	private BundleData[] allFeaturedBundles;

	private StarBundle[] allStars;

	private Product[] allProducts;

	public StoreTabButton[] Tabs;

	public GameObject normalMenu;

	public GameObject featuredMenu;

	public TextMeshPro LoadingText;

	public TextMeshPro RestorePurchasesButton;

	public GameObject RestorePurchasesObj;

	public UiElement OpenAllInEShopButton;

	public PurchaseButton PurchasablePrefab;

	public PurchaseButton PurchasableNameplatePrefab;

	public BundlePurchaseButton BundlePurchasablePrefab;

	public BundlePurchaseButton LargeBundlePurchasablePrefab;

	public TextMeshPro beanAmount;

	public TextMeshPro starAmount;

	public GameObject comingSoonText;

	public TextMeshPro tabHeader;

	public PurchasePopUp plsWaitModal;

	public GameObject confirmMenu;

	public CubeCellPurchaseButton priceButton;

	public CubeCellPurchaseButton priceButtonCube;

	public GameObject starConfirmMenu;

	public TextMeshPro StarBundleName;

	public TextMeshPro StarPriceText;

	public PassiveButton starBuyButton;

	public SpriteRenderer starPreviewImage;

	public CosmicubeMenu cosmicubePreviewMenu;

	public PoolablePlayer dressMeUp;

	public PoolablePlayer dressMeUpCube;

	public PlayerVoteArea NameplatePreviewArea;

	public TextMeshPro ItemName;

	public TextMeshPro ItemNameCube;

	public TextMeshPro BundleName;

	public TextMeshPro CosmicubeName;

	public SpriteRenderer PurchaseBackground;

	public TextMeshPro PriceText;

	public TextMeshPro PriceTextCube;

	public StoreChip PreviewPrefab;

	public Scroller ConfirmMenuScroller;

	public UpdateProductTimerDisplay itemLimitedTimeText;

	public FloatRange BundleRange;

	public FloatRange XRange;

	public FloatRange CubesRange;

	public float YStart;

	public float YOffset;

	public int NumPerRow;

	public Vector3 ConfirmCosmeticPosition;

	public Sprite starIcon;

	public Sprite beanIcon;

	[HideInInspector]
	public List<ColorChip> ColorChips;

	private RefreshTimer[] refreshTimers;

	private StoreTab selectedTab;

	private StoreTabButton selectedTabObject;

	[SerializeField]
	private AudioClip swapTabSound;

	private const float NormalHeight = -1.95f;

	private const float BoxHeight = -1.95f;

	public TextMeshPro starDisclaimerText;

	private List<TextMeshPro> BundleTitles;

	private Logger logger;

	public Action OnOpen;

	public Action OnClose;

	private StoreTab[] controllerSelectableTabs;

	public Scroller Scroller;

	public Scroller FeatureTabScroller;

	private MainMenuManager manager;

	private PurchaseButton CurrentButton;

	private List<GameObject> AllObjects;

	private ControllerNavMenu controllerNavMenu;

	public GameObject glyphL;

	public GameObject glyphR;

	public bool initialized;

	public PurchaseStates PurchaseState { get; private set; }

	public bool Initialized => initialized;

	public void CloseConfirm()
	{
		confirmMenu.SetActive(value: false);
		starConfirmMenu.SetActive(value: false);
		GameObject gameObject = cosmicubePreviewMenu.gameObject;
		gameObject.SetActive(value: false);
		normalMenu.SetActive(value: true);
	}

	public void CloseWaitModal()
	{
		GameObject gameObject = plsWaitModal.gameObject;
		gameObject.SetActive(value: false);
	}

	public void DisplayFailed(string error)
	{
		plsWaitModal.SetResponse(ResponseState.Failed, error);
	}

	public void DisplaySuccess()
	{
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363CE12]");
		if ((nint)0 == 0)
		{
			_ = 1;
		}
		plsWaitModal.SetResponse(ResponseState.Success, "");
	}

	public void UpdateStarDisplay()
	{
		if (starAmount != null)
		{
			starAmount.text = DestroyableSingleton<InventoryManager>.Instance.UnusedStars.ToString();
		}
	}

	public void Start()
	{
		BundleTitles = new List<TextMeshPro>();
		base.gameObject.SetActive(value: false);
		if (OpenAllInEShopButton != null)
		{
			OpenAllInEShopButton.gameObject.SetActive(value: false);
		}
		var versions = DestroyableSingleton<ReferenceDataManager>.Instance.StoreTabVersions;
		foreach (StoreTabButton tab in Tabs)
		{
			if (versions.TryGetValue(tab.storeTab, out string date) &&
				Constants.ParseDate(date) <= DataManager.Player.store.GetStoreTabViewDate(tab.storeTab))
			{
				tab.NewItemIcon.SetActive(false);
			}
		}
	}

	protected virtual void OnDisable()
	{
		PlayerData player = DataManager.Player;
		player.Save();
	}

	public void OpenTab(StoreMenuTabButton Butt)
	{
		OpenTab(Butt.Tab);
	}

	public unsafe void OpenTab(StoreTab tab)
	{
		selectedTab = tab;
		foreach (StoreTabButton button in Tabs)
		{
			bool selected = button.storeTab == tab;
			button.Button.enabled = selected;
			if (selected)
			{
				selectedTabObject = button;
				var versions = DestroyableSingleton<ReferenceDataManager>.Instance.StoreTabVersions;
				if (versions.TryGetValue(button.storeTab, out string date))
				{
					button.NewItemIcon.SetActive(false);
					DataManager.Player.store.SetStoreTabViewDate(button.storeTab, Constants.ParseDate(date));
				}
			}
			if (button.tabText)
			{
				button.tabText.outlineColor = selected ? Palette.InventorySelectedTextColor : Palette.InventoryTextColor;
			}
			if (selected)
			{
				ShowTab();
			}
		}
		bool joystick = ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick;
		glyphL.SetActive(joystick && tab != controllerSelectableTabs[0]);
		glyphR.SetActive(joystick && tab != controllerSelectableTabs[controllerSelectableTabs.Length - 1]);
	}

	public void ShowConfirm(PurchaseButton butt)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	public void StarPurchaseFinished()
	{
		GameObject gameObject = plsWaitModal.gameObject;
		gameObject.SetActive(value: false);
	}

	public unsafe void Initialize()
	{
		if (initialized) return;
		initialized = true;
		controllerNavMenu = GetComponent<ControllerNavMenu>();
		gameObject.SetActive(false);
		dressMeUp.TogglePet(false);
		dressMeUp.ToggleName(false);
		dressMeUpCube.TogglePet(false);
		dressMeUpCube.ToggleName(false);
		PurchaseBackground.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		PriceText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
		PriceText.text = "";
		PriceTextCube.color = new Color(0.8f, 0.8f, 0.8f, 1f);
		PriceTextCube.text = "";
		LoadCosmeticData();
	}

	public unsafe void Update()
	{
		HandleRefreshTimers();
		if (!normalMenu.activeSelf && !featuredMenu.activeSelf) return;
		if (ControllerManager.Instance.CurrentUiState.MenuName != name) return;
		Player player = ReInput.players.GetPlayer(0);
		int index = Array.IndexOf(controllerSelectableTabs, selectedTab);
		bool changed = false;
		if (index > 0 && player.GetButtonDown(35))
		{
			index--;
			SoundManager.Instance.PlaySound(swapTabSound, false, 1f);
			changed = true;
		}
		if (index < controllerSelectableTabs.Length - 1 && player.GetButtonDown(34))
		{
			index++;
			SoundManager.Instance.PlaySound(swapTabSound, false, 1f);
			changed = true;
		}
		if (changed) OpenTab(controllerSelectableTabs[index]);
	}

	public unsafe void Open()
	{
		gameObject.SetActive(true);
		plsWaitModal.gameObject.SetActive(false);
		CloseConfirm();
		if (controllerNavMenu) controllerNavMenu.OpenMenu();
		transform.position = DestroyableSingleton<HudManager>.InstanceExists
			? DestroyableSingleton<HudManager>.Instance.transform.position - new Vector3(0f, 0f, 100f)
			: new Vector3(0f, 0f, -100f);
		InventoryManager inventory = DestroyableSingleton<InventoryManager>.Instance;
		beanAmount.text = inventory.UnusedBeans.ToString();
		starAmount.text = inventory.UnusedStars.ToString();
		OnOpen?.Invoke();
		OpenTab(StoreTab.Home);
	}

	public void OpenGeneralShop()
	{
	}

	private void DestroySliderObjects()
	{ /* spliced-from-2019 */
		for (int i = 0; i < AllObjects.Count; i++)
		{
			UnityEngine.Object.Destroy(AllObjects[i]);
		}
		AllObjects.Clear();
	}

	public void PreviewItem(StoreChip storeChip, CosmeticData product)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	public unsafe void SetProduct(PurchaseButton button)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void DeselectAllItems()
	{
		Action<ColorChip> action = _003C_003Ec._003C_003E9__110_0;
		if (_003C_003Ec._003C_003E9__110_0 == null)
		{
			action = (_003C_003Ec._003C_003E9__110_0 = (ColorChip c) =>
			{
				c.Deselect();
			});
		}
		ColorChips.ForEach(action);
	}

	public void BuyProduct()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	public void Close()
	{
		if (!plsWaitModal.gameObject.activeSelf && (normalMenu.activeSelf || featuredMenu.activeSelf))
		{
			HatsTab hatsTab = UnityEngine.Object.FindObjectOfType<HatsTab>();
			if (hatsTab)
			{
				hatsTab.OnDisable();
				hatsTab.OnEnable();
			}
			OnClose?.Invoke();
			DestroyableSingleton<TransitionFade>.Instance.DoTransitionFade(gameObject, null, null);
			if (manager == null) manager = UnityEngine.Object.FindObjectOfType<MainMenuManager>();
			if (manager) manager.CheckNewItems();
		}
		else
		{
			plsWaitModal.gameObject.SetActive(false);
			CloseConfirm();
		}
		logger.Info("Closing store");
	}

	public void CloseEntirely()
	{
		GameObject gameObject = plsWaitModal.gameObject;
		gameObject.SetActive(value: false);
		CloseConfirm();
		Close();
	}

	private void LoadCosmeticData()
	{
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363CE21]");
		if ((nint)0 == 0)
		{
			_ = 1;
		}
		SetRefreshTimers();
		ReSortCosmeticTabs();
		GameObject gameObject = LoadingText.gameObject;
		gameObject.SetActive(value: false);
	}

	public void OnSaveDataChanged()
	{
		ReSortCosmeticTabs();
	}

	private void ReSortCosmeticTabs()
	{
		HatManager hats = DestroyableSingleton<HatManager>.Instance;
		allHats = GetSortedStoreCosmetics(hats.allHats);
		allSkins = GetSortedStoreCosmetics(hats.allSkins);
		allVisors = GetSortedStoreCosmetics(hats.allVisors);
		allPlates = GetSortedStoreCosmetics(hats.allNamePlates);
		allPets = GetSortedStoreCosmetics(hats.allPets);
		allCubes = DestroyableSingleton<CosmicubeManager>.Instance.GetAvailableCubeData()
			.OrderBy(c => DataManager.Player.purchases.GetPurchase(c.productId, ""))
			.ThenByDescending(c => c.GetDateCreated()).ToArray();
		allStars = hats.allStarBundles.OrderBy(b => b.price).ToArray();
		allBundles = hats.allBundles.Where(b => !b.Free && !b.NotInStore && b.LimitedTimeAvailable.IsAvailable()).ToArray();
		hats.RefreshFeatured();
		allFeaturedItems = hats.allFeaturedItems.ToArray();
		allFeaturedCubes = hats.allFeaturedCubes.Where(c => c.limitedTime.IsAvailable()).ToArray();
		allFeaturedBundles = hats.allFeaturedBundles.Where(b => b.LimitedTimeAvailable.IsAvailable() && b.BundleSize == BundleSizes.Wide).ToArray();
		allLargeFeaturedBundles = hats.allFeaturedBundles.Where(b => b.LimitedTimeAvailable.IsAvailable() && b.BundleSize == BundleSizes.Large).ToArray();
	}

	private void SetRefreshTimers()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void HandleRefreshTimers()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void UpdateRefreshTimers()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private unsafe void AddRefreshTimers(Dictionary<DateTime, List<StoreTab>> dictionary, IEnumerable<CosmeticData> cosmetics, StoreTab[] tabs)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private unsafe void AddRefreshTimers(Dictionary<DateTime, List<StoreTab>> dictionary, IEnumerable<CosmicubeData> cosmicubes, StoreTab[] tabs)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private unsafe void AddRefreshTimers(Dictionary<DateTime, List<StoreTab>> dictionary, IEnumerable<BundleData> bundles, StoreTab[] tabs)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private unsafe void AddRefreshTimers(Dictionary<DateTime, List<StoreTab>> dictionary, IEnumerable<FeaturedItemGroup> features)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void AddRefreshTimers(Dictionary<DateTime, List<StoreTab>> dictionary, DateTime start, DateTime end, StoreTab[] tabs)
	{
		DateTime utcNow = AmongUsDateTime.UtcNow;
		StoreTab[] tabs2 = default;
		if (start > utcNow)
		{
			AddRefreshTimer(dictionary, start, tabs2);
		}
		if (end > utcNow)
		{
			AddRefreshTimer(dictionary, end, tabs2);
		}
	}

	private void AddRefreshTimer(Dictionary<DateTime, List<StoreTab>> dictionary, DateTime time, StoreTab[] tabs)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void DebugRefreshTimers()
	{
	}

	private T[] GetSortedStoreCosmetics<T>(IEnumerable<T> all) where T : CosmeticData
	{
		return all.Where(c => !c.Free && !c.NotInStore &&
			(!c.limitedTime.IsLimitedTime() || c.limitedTime.IsLimitedAndAvailable()) &&
			string.IsNullOrWhiteSpace(c.BundleId))
			.OrderBy(c => DataManager.Player.purchases.GetPurchase(c.ProductId, c.BundleId)).ToArray();
	}

	private unsafe void ShowTab()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private unsafe Vector3 AddProductFromList(Vector3 position, IBuyable[] products, List<PurchaseButton> allBtns, float yOffset, int numPerRow, FloatRange xRange, float spacingMultiplier = 1f, bool centerRemainder = false)
	{
		return default; // TODO(verify): default, original logic unrecoverable
	}

	private unsafe PurchaseButton InsertProduct(Vector3 position, IBuyable item)
	{
		return default; // TODO(verify): default, original logic unrecoverable
	}

	public void BuyStars()
	{
		if (!CurrentButton)
		{
			return;
		}
		PurchaseButton currentButton = CurrentButton;
		if (!currentButton.Purchased && PurchaseState != PurchaseStates.Started)
		{
			GameObject gameObject = plsWaitModal.gameObject;
			gameObject.SetActive(value: true);
			plsWaitModal.Show();
			PlayerData player = DataManager.Player;
			PlayerAccountData account = player.account;
			if (account.loginStatus == EOSManager.AccountLoginStatus.LoggedIn)
			{
				StoreManager instance = DestroyableSingleton<StoreManager>.Instance;
				instance.InitiateStorePurchaseStar(CurrentButton);
				return;
			}
			TranslationController instance2 = DestroyableSingleton<TranslationController>.Instance;
			object[] parts = Array.Empty<object>();
			string error = instance2.GetString(StringNames.LoggedInErrorStarPurchase, parts);
			plsWaitModal.SetResponse(ResponseState.Failed, error);
		}
	}

	public void ResetPurchaseState()
	{
		PurchaseState = PurchaseStates.NotStarted;
	}

	public StoreMenu()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		FloatRange bundleRange = new FloatRange(-3.5f, 2.9f);
		BundleRange = bundleRange;
		XRange = new FloatRange(1.5f, 3f);
		CubesRange = new FloatRange(-3.7f, 2.7f);
		YStart = 1.1f;
		YOffset = 0.8f;
		NumPerRow = 4;
		ColorChips = new List<ColorChip>();
		logger = new Logger(Logger.Category.UI, new string[1] { "StoreMenu" });
		controllerSelectableTabs = new StoreTab[9]
		{
		StoreTab.Home,
		StoreTab.Hats,
		StoreTab.Skins,
		StoreTab.Pets,
		StoreTab.Visors,
		StoreTab.Nameplates,
		StoreTab.Bundles,
		StoreTab.Cubes,
		StoreTab.Stars
		};
		AllObjects = new List<GameObject>();
	}

	private void _003CBuyProduct_003Eb__111_0(ResponseState cb, Response<ResponsePurchase> response)
	{
		InventoryManager instance = DestroyableSingleton<InventoryManager>.Instance;
		int num = default;
		string text = num.ToString();
		beanAmount.text = text;
		InventoryManager instance2 = DestroyableSingleton<InventoryManager>.Instance;
		string text2 = num.ToString();
		starAmount.text = text2;
		ResponseError[] errors = response?.Errors;
		plsWaitModal.SetResponse(cb, errors);
		if (cb == ResponseState.Success)
		{
			CloseConfirm();
			ReSortCosmeticTabs();
			OpenTab(selectedTab);
		}
	}
}
