using System;
using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using Assets.InnerNet;
using Innersloth.Assets;
using Rewired;
using TMPro;
using UnityEngine;

public class CosmicubeMenu : MonoBehaviour
{
	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public GameObject zoomInGlyph;

	public GameObject zoomOutGlyph;

	public TextMeshPro cubeTitle;

	public TextMeshPro percentCompletedText;

	public TextMeshPro selectedItemName;

	public TextMeshPro selectedItemCategory;

	public TextMeshPro[] selectedItemPrice;

	public TextMeshPro confirmPurchaseText;

	public GameObject confirmWindow;

	public GameObject equipButton;

	public GameObject equippedText;

	public GameObject loadingIndicator;

	public SpriteRenderer podConfirmIcon;

	public CubeCellPurchaseButton purchaseButton;

	public GameObject lockedButton;

	public PoolablePlayer dressMeUp;

	public PlayerVoteArea nameplatePreview;

	public StoreMannequin storeMannequin;

	public SpriteRenderer nameplateConfirmPreview;

	public SpriteRenderer podIcon;

	public TextMeshPro podAmount;

	public Scroller scroller;

	public GameObject cubeHolder;

	public SpriteRenderer zoomOutButton;

	public SpriteRenderer zoomInButton;

	public AudioClip cubeCellUnlockSound;

	public UpdateProductTimerDisplay limitedTimeText;

	public PurchasePopUp plsWaitModal;

	public static int CellMaskLayer = 61;

	private float zoomLevel = 1f;

	private float minZoom = 0.5f;

	private float maxZoom = 1.5f;

	private bool previewMode;

	private bool animatingPurchase;

	private Cosmicube currentCube;

	private CosmicubeData currentCubeData;

	private CosmicubeCell currentCell;

	private readonly Logger logger = new Logger(Logger.Category.UI, "CosmicubeMenu");

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		currentCell = null;
		currentCube = null;
	}

	public void ZoomIn()
	{
		AdjustZoom(0.5f);
	}

	public void ZoomOut()
	{
		AdjustZoom(-0.5f);
	}

	public void Show(string cubeID, bool previewMode = false)
	{
		this.previewMode = previewMode;
		animatingPurchase = false;
		cubeTitle.text = "Loading...";
		podAmount.text = "-";
		loadingIndicator.SetActive(value: true);
		gameObject.SetActive(value: true);
		if (!previewMode)
		{
			dressMeUp.ResetCosmetics();
			dressMeUp.ToggleName(active: false);
		}
		dressMeUp.SetBodyColor(DataManager.Player.Customization.Color);
		currentCube = GetComponentInChildren<Cosmicube>();
		if ((bool)currentCube)
		{
			UnityEngine.Object.Destroy(currentCube.gameObject);
		}
		CellMaskLayer = 61;
		currentCubeData = DestroyableSingleton<CosmicubeManager>.Instance.GetCubeDataByID(cubeID);
		StartCoroutine(this.CoLoadAssetAsync(currentCubeData.podIconRef, (Sprite icon) =>
		{
			podIcon.sprite = icon;
			podConfirmIcon.sprite = icon;
		}));
		StartCoroutine(this.CoLoadAssetAsync(currentCubeData.CosmicubePrefabRef, (Cosmicube cube) =>
		{
			currentCube = UnityEngine.Object.Instantiate(cube, cubeHolder.transform);
			currentCube.SetMenu(this);
			LoadCube();
			podAmount.text = DestroyableSingleton<InventoryManager>.Instance.GetPodCount(currentCubeData.podId).ToString();
			PassiveButton firstCell = currentCube.GetFirstCell();
			ControllerManager.Instance.OpenOverlayMenu(name, BackButton, firstCell, new List<UiElement>(), gridNav: true);
			foreach (PassiveButton allButton in currentCube.GetAllButtons())
			{
				allButton.ClickMask = scroller.Hitbox;
				ControllerManager.Instance.AddSelectableUiElement(allButton);
			}
			scroller.UpdateScrollBars();
			selectedItemName.text = string.Empty;
			selectedItemCategory.text = string.Empty;
			currentCell = null;
			purchaseButton.gameObject.SetActive(value: false);
			equipButton.SetActive(value: false);
			equippedText.SetActive(value: false);
		}));
		if (limitedTimeText != null)
		{
			limitedTimeText.SetProduct(currentCubeData);
			limitedTimeText.gameObject.SetActive(GetCurrentProductLimitedTime().IsLimitedAndAvailable());
		}
		UpdatePercentComplete();
	}

	private void UpdatePercentComplete()
	{
		if (!(percentCompletedText == null))
		{
			float completionProgress = DestroyableSingleton<CosmicubeManager>.Instance.GetCompletionProgress(currentCubeData);
			percentCompletedText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.CosmicubeProgression, (int)(completionProgress * 100f));
		}
	}

	public LimitedTimeStartEnd GetCurrentProductLimitedTime()
	{
		if (currentCubeData == null)
		{
			return default;
		}
		return currentCubeData.LimitedTimeAvailable;
	}

	public void Close()
	{
		if (plsWaitModal.gameObject.activeSelf)
		{
			plsWaitModal.gameObject.SetActive(value: false);
		}
		else
		{
			GetComponent<TransitionOpen>().Close();
		}
	}

	public void EquipSelectedItem()
	{
		if (currentCell == null || !currentCell.unlocked || currentCell.CanBePurchased())
		{
			logger.Error("Somehow tried to equip an item that's not available to equip");
			return;
		}
		PlayerCustomization.Equip(currentCell.CosmeticData.unlockableCosmetic);
		equipButton.SetActive(value: false);
		equippedText.SetActive(value: true);
		logger.Info("Equipped " + currentCell.CosmeticData.GetProdId());
	}

	public void SelectItem(CosmicubeCell cell)
	{
		if (animatingPurchase)
		{
			return;
		}
		currentCell?.SetDeselected();
		currentCell = cell;
		currentCell.SetSelected();
		storeMannequin.gameObject.SetActive(!(cell.CosmeticData.unlockableCosmetic is NamePlateData));
		nameplateConfirmPreview.gameObject.SetActive(cell.CosmeticData.unlockableCosmetic is NamePlateData);
		dressMeUp.gameObject.SetActive(!(cell.CosmeticData.unlockableCosmetic is NamePlateData));
		nameplatePreview.gameObject.SetActive(cell.CosmeticData.unlockableCosmetic is NamePlateData);
		nameplatePreview.MaskArea.gameObject.SetActive(cell.CosmeticData.unlockableCosmetic is NamePlateData);
		if (cell.CosmeticData.unlockableCosmetic is NamePlateData)
		{
			NamePlateData namePlateData = cell.CosmeticData.unlockableCosmetic as NamePlateData;
			nameplatePreview.PreviewNameplate(namePlateData.ProdId);
			StartCoroutine(((MonoBehaviour)this).CoLoadAssetAsync((IAddressableAssetProvider<NamePlateViewData>)namePlateData, (Action<NamePlateViewData>)((NamePlateViewData viewData) =>
			{
				nameplateConfirmPreview.sprite = viewData?.Image;
			})));
		}
		else if (cell.CosmeticData.unlockableCosmetic is PetData)
		{
			dressMeUp.ResetCosmetics();
			dressMeUp.ToggleName(active: false);
			PetData petData = cell.CosmeticData.unlockableCosmetic as PetData;
			dressMeUp.SetPetIdle(petData, DataManager.Player.Customization.Color);
		}
		else
		{
			cell.CosmeticData.unlockableCosmetic.PreviewOnPlayer(dressMeUp, DataManager.Player.Customization.Color);
		}
		selectedItemName.text = cell.CosmeticData.unlockableCosmetic.GetItemName();
		selectedItemCategory.text = cell.CosmeticData.unlockableCosmetic.GetItemCategory();
		if (previewMode)
		{
			selectedItemName.enabled = false;
			selectedItemCategory.enabled = false;
		}
		selectedItemPrice.ForEach((TextMeshPro t) =>
		{
			t.text = cell.CosmeticData.cost.ToString();
		});
		bool flag = false;
		bool flag2 = cell.unlocked && !previewMode;
		if (flag2)
		{
			flag = PlayerCustomization.IsEquipped(cell.CosmeticData.unlockableCosmetic);
		}
		equipButton.SetActive(flag2 && !flag);
		equippedText.SetActive(flag2 & flag);
		purchaseButton.gameObject.SetActive(cell.CanBePurchased() && !previewMode);
		lockedButton.SetActive(!cell.CanBePurchased() && !cell.unlocked && !previewMode);
		if (purchaseButton.gameObject.activeSelf)
		{
			purchaseButton.podIcon.sprite = podIcon.sprite;
			if (DestroyableSingleton<InventoryManager>.Instance.GetPodCount(currentCubeData.podId) < cell.CosmeticData.cost)
			{
				purchaseButton.SetInactive();
			}
			else
			{
				purchaseButton.SetUseable();
			}
		}
	}

	private void LoadCube()
	{
		cubeTitle.text = currentCubeData.GetCosmicubeName();
		confirmPurchaseText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ConfirmPurchaseText, currentCubeData.GetCosmicubeName());
		scroller.SetBounds(currentCubeData.YRange, currentCubeData.XRange);
		loadingIndicator.SetActive(value: false);
	}

	public void ClickPurchase()
	{
		if (previewMode || animatingPurchase || DestroyableSingleton<InventoryManager>.Instance.GetPodCount(currentCubeData.podId) < currentCell.CosmeticData.cost)
		{
			return;
		}
		plsWaitModal.gameObject.SetActive(value: true);
		plsWaitModal.Show();
		DestroyableSingleton<InventoryManager>.Instance.UnlockCubeItem(currentCell.CosmeticData.GetProdId(), currentCubeData.ProdId, (ResponseState cb, Response<ResponsePurchase> response) =>
		{
			if (cb == ResponseState.Success)
			{
				StartCoroutine(CoAnimatePurchase());
			}
			else
			{
				plsWaitModal.SetResponse(cb, response.Errors);
			}
		});
	}

	private IEnumerator CoAnimatePurchase()
	{
		animatingPurchase = true;
		SoundManager.Instance.PlaySoundImmediate(cubeCellUnlockSound, loop: false, 0.5f);
		plsWaitModal.gameObject.SetActive(value: false);
		podAmount.text = DestroyableSingleton<InventoryManager>.Instance.GetPodCount(currentCube.Data.podId).ToString();
		yield return currentCell.CoPurchase();
		animatingPurchase = false;
		UpdatePercentComplete();
		SelectItem(currentCell);
	}

	public void ShowConfirmPurchase()
	{
		if (currentCell != null && currentCell.CanBePurchased() && DestroyableSingleton<InventoryManager>.Instance.GetPodCount(currentCube.Data.podId) >= currentCell.CosmeticData.cost)
		{
			confirmWindow.SetActive(value: true);
			if (currentCell.CosmeticData.unlockableCosmetic is PetData)
			{
				PetData product = currentCell.CosmeticData.unlockableCosmetic as PetData;
				storeMannequin.WearThing(product, PlayerMaterial.MaskType.None);
			}
			else
			{
				storeMannequin.WearThing(currentCell.CosmeticData.unlockableCosmetic, PlayerMaterial.MaskType.SimpleUI, DataManager.Player.Customization.Color);
			}
			logger.Info("Showing confirm window for " + currentCell.cosmetic.GetProdId());
		}
	}

	private void Update()
	{
		if (confirmWindow.activeSelf)
		{
			return;
		}
		Player player = ReInput.players.GetPlayer(0);
		if (player.GetButtonDown(35))
		{
			ZoomOut();
		}
		if (player.GetButtonDown(34))
		{
			ZoomIn();
		}
		if (currentCell != null && player.GetButtonDown(29))
		{
			if (currentCell.CanBePurchased())
			{
				ShowConfirmPurchase();
			}
			else
			{
				EquipSelectedItem();
			}
		}
	}

	private void AdjustZoom(float amount)
	{
		zoomInButton.color = Palette.White;
		zoomOutButton.color = Palette.White;
		zoomLevel += amount;
		if (zoomLevel <= minZoom)
		{
			zoomLevel = minZoom;
			zoomOutButton.color = Palette.DisabledGrey;
		}
		if (zoomLevel >= maxZoom)
		{
			zoomLevel = maxZoom;
			zoomInButton.color = Palette.DisabledGrey;
		}
		scroller.DragScrollSpeed = 0.3f / zoomLevel;
		scroller.transform.localScale = Vector3.one * zoomLevel;
		bool active = ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick && zoomLevel > minZoom;
		zoomOutGlyph.SetActive(active);
		bool active2 = ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick && zoomLevel < maxZoom;
		zoomInGlyph.SetActive(active2);
	}

	private void OnDestroy()
	{
		_ = (bool)currentCube;
	}
}
