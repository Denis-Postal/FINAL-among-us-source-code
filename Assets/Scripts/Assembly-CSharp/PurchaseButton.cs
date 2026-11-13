using System;
using AmongUs.Data;
using Assets.InnerNet;
using Innersloth.Assets;
using TMPro;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
	private const float normalSize = 1.75f;

	[Header("Icons")]
	public SpriteRenderer PurchasedIcon;

	public SpriteRenderer LimitedTimeIcon;

	public UpdateProductTimerDisplay LimitedTimeText;

	public SpriteRenderer CrewHeadImage;

	public HatParent HatImage;

	public VisorLayer VisorLayer;

	public Sprite MannequinFrame;

	public LoadingSpinner LoadingIndicator;

	[Header("Background")]
	public SpriteRenderer Background;

	public SpriteRenderer SelectionHighlight;

	public SpriteRenderer GlassSprite;

	public SpriteRenderer GlassMask;

	public SpriteRenderer ImageMask;

	public SpriteRenderer PurchasedOverlay;

	[Header("Button")]
	public BoxCollider2D clickCollider;

	public PassiveButton PassiveButton;

	[Header("Product Info")]
	public TextMeshPro NameText;

	public TextMeshPro PriceText;

	public bool Purchased;

	public string Name;

	public string Price;

	public string ProductId;

	public GameObject PriceLabel;

	[Header("Label")]
	public SpriteRenderer LabelMask;

	public SpriteRenderer LabelBackground;

	public SpriteRenderer LabelGlass;

	public IBuyable Product;

	private Action onDestroy;

	public StoreMenu Parent { get; set; }

	protected virtual void Start()
	{
		if ((bool)ImageMask)
		{
			int num = 47;
			if ((bool)GlassMask)
			{
				GlassMask.material.SetInt(PlayerMaterial.MaskLayer, num);
			}
			ImageMask.material.SetInt(PlayerMaterial.MaskLayer, num);
			if ((bool)HatImage?.FrontLayer)
			{
				HatImage.FrontLayer.material.SetInt(PlayerMaterial.MaskLayer, num);
			}
			if ((bool)HatImage?.BackLayer)
			{
				HatImage.BackLayer.material.SetInt(PlayerMaterial.MaskLayer, num);
			}
			if ((bool)LoadingIndicator)
			{
				LoadingIndicator.SetMaskLayer(num);
			}
		}
		if ((bool)LabelMask)
		{
			int value = 46;
			LabelMask.material.SetInt(PlayerMaterial.MaskLayer, value);
			LabelBackground.material.SetInt(PlayerMaterial.MaskLayer, value);
			LabelGlass.material.SetInt(PlayerMaterial.MaskLayer, value);
		}
	}

	public virtual void SetItem(IBuyable product, string productId, string name, string price, bool purchased, CurrencyTypes currency)
	{
		Product = product;
		Purchased = purchased;
		Name = name;
		Price = price;
		ProductId = productId;
		base.name = productId;
		PurchasedIcon.enabled = Purchased;
		ImageMask.enabled = false;
		if (PurchasedOverlay != null)
		{
			PurchasedOverlay.enabled = Purchased;
		}
		if (!product.LimitedTimeAvailable.HasEndDateAndAvailable() || Purchased)
		{
			LimitedTimeIcon.enabled = false;
		}
		else
		{
			LimitedTimeIcon.enabled = true;
		}
		LimitedTimeText.SetProduct(Product);
		LimitedTimeText.gameObject.SetActive(LimitedTimeIcon.enabled);
		if (Purchased)
		{
			PriceText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Owned);
		}
		else
		{
			switch (currency)
			{
			case CurrencyTypes.Beans:
				PriceText.text = "<sprite name=\"UI_Currency_Bean\"> " + Price;
				break;
			case CurrencyTypes.Stars:
				PriceText.text = "<sprite name=\"UI_Currency_Star\"> " + Price;
				break;
			}
		}
		if (Product is HatData)
		{
			HatData prod = (HatData)Product;
			((MonoBehaviour)this).LoadAssetAsync((IAddressableAssetProvider<HatViewData>)prod, (Action<HatViewData>)((HatViewData viewAsset) =>
			{
				if (!this.IsDestroyedOrNull() && !gameObject.IsDestroyedOrNull() && !HatImage.IsDestroyedOrNull())
				{
					HatImage.SetHat(prod, 0);
					HatImage.SetMaskType(PlayerMaterial.MaskType.SimpleUI);
					HatImage.FrontLayer.transform.localPosition = new Vector3(0f, 0f, (prod.InFront || ((bool)viewAsset && (bool)viewAsset.MainImage && (bool)viewAsset.BackImage)) ? (-0.01f) : 0.01f);
					HatImage.FrontLayer.transform.localScale = Vector3.one * 0.5f;
				}
			}), (Action)null);
			HatImage.BackLayer.transform.localPosition = new Vector3(0f, 0f, 0.001f);
			HatImage.BackLayer.transform.localScale = Vector3.one * 0.5f;
			NameText.text = prod.GetItemName();
			SetSquare();
		}
		if (Product is NamePlateData)
		{
			NamePlateData namePlateData = (NamePlateData)Product;
			((MonoBehaviour)this).LoadAssetAsync((IAddressableAssetProvider<NamePlateViewData>)namePlateData, (Action<NamePlateViewData>)((NamePlateViewData viewAsset) =>
			{
				if (!this.IsDestroyedOrNull() && !gameObject.IsDestroyedOrNull() && !CrewHeadImage.gameObject.IsDestroyedOrNull())
				{
					CrewHeadImage.sprite = viewAsset.Image;
				}
			}), (Action)null);
			NameText.text = namePlateData.GetItemName();
		}
		else if (Product is SkinData)
		{
			SkinData skinData = (SkinData)Product;
			CrewHeadImage.sprite = MannequinFrame;
			CrewHeadImage.transform.localPosition = new Vector3(0f, 0.2f, -0.01f);
			CrewHeadImage.transform.localScale = Vector3.one * 0.5f;
			HatImage.FrontLayer.transform.localPosition = new Vector3(0f, 0f, -1f);
			HatImage.FrontLayer.transform.localScale = Vector3.one;
			PlayerControl.SetSkinImageAsync(skinData, 0, HatImage.FrontLayer);
			NameText.text = skinData.GetItemName();
			SetSquare();
		}
		else if (Product is VisorData)
		{
			VisorData visorData = (VisorData)Product;
			CrewHeadImage.sprite = MannequinFrame;
			CrewHeadImage.transform.localPosition = new Vector3(0f, 0.2f, -0.01f);
			CrewHeadImage.transform.localScale = Vector3.one * 0.5f;
			VisorLayer.transform.localPosition = new Vector3(0f, 0.59f, -0.1f);
			VisorLayer.transform.localScale = Vector3.one;
			VisorLayer.SetVisor(visorData.ProdId, 0);
			VisorLayer.SetMaskType(PlayerMaterial.MaskType.SimpleUI);
			NameText.text = visorData.GetItemName();
			SetSquare();
		}
		else if (Product is PetData)
		{
			PetData petData = (PetData)Product;
			CrewHeadImage.enabled = false;
			petData.SetPreview(HatImage.FrontLayer, DataManager.Player.Customization.Color);
			HatImage.FrontLayer.transform.localPosition = new Vector3(0f, 0.35f, -0.01f);
			HatImage.FrontLayer.transform.localScale = Vector3.one * 0.5f;
			NameText.text = petData.GetItemName();
			SetSquare();
		}
		else if (Product is CosmicubeData)
		{
			CosmicubeData cosmicubeData = (CosmicubeData)Product;
			NameText.text = cosmicubeData.GetCosmicubeName();
			LoadingIndicator.Toggle(on: true);
			PassiveButton.enabled = false;
			PriceLabel.SetActive(value: false);
			Parent.StartCoroutine(this.CoLoadAssetAsync(cosmicubeData.StoreImageRef, (Sprite sprite) =>
			{
				PriceLabel.SetActive(value: true);
				PassiveButton.enabled = true;
				LoadingIndicator.Toggle(on: false);
				HatImage.FrontLayer.transform.localRotation = Quaternion.identity;
				HatImage.FrontLayer.sprite = sprite;
			}));
			HatImage.FrontLayer.maskInteraction = SpriteMaskInteraction.None;
			HatImage.FrontLayer.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			HatImage.FrontLayer.transform.localPosition = new Vector3(0f, 0.2f, -0.0001f);
			ImageMask.enabled = true;
			CrewHeadImage.enabled = false;
			SetDoubleBig();
		}
		else if (Product is StarBundle)
		{
			StarBundle starBundle = (StarBundle)Product;
			PriceText.text = "$" + starBundle.price;
			NameText.text = starBundle.GetItemName();
			HatImage.FrontLayer.sprite = starBundle.StoreImage;
			HatImage.BackLayer.sprite = starBundle.StoreBackImage;
			HatImage.FrontLayer.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
			HatImage.FrontLayer.transform.localPosition = new Vector3(0f, 0.39f, -1f);
			HatImage.FrontLayer.maskInteraction = SpriteMaskInteraction.None;
			HatImage.BackLayer.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
			HatImage.BackLayer.transform.localPosition = new Vector3(0f, 0.39f, -0.001f);
			HatImage.BackLayer.maskInteraction = SpriteMaskInteraction.None;
			ImageMask.enabled = true;
			CrewHeadImage.enabled = false;
			SetDoubleBig();
		}
	}

	private void SetDoubleBig()
	{
		if (PurchasedOverlay != null)
		{
			PurchasedOverlay.size = new Vector2(3.5f, 4f);
		}
		Background.size = new Vector2(3.5f, 4f);
		Background.GetComponent<BoxCollider2D>().size = new Vector2(3.5f, 4f);
		SelectionHighlight.size = new Vector2(3.5f, 4f);
		LabelMask.size = new Vector2(3.5f, 2f);
		GlassSprite.transform.localScale = new Vector2(GlassSprite.transform.localScale.x * 2f, GlassSprite.transform.localScale.y);
		ImageMask.transform.localScale = new Vector3(0.62f, 0.62f, 1f);
		ImageMask.size = new Vector2(5.41f, 6.15f);
		ImageMask.transform.localPosition = new Vector2(0f, 0.23f);
		clickCollider.size *= 2f;
		PurchasedIcon.transform.localPosition = new Vector3(1.92f, 1.35f, PurchasedIcon.transform.localPosition.z);
		LimitedTimeIcon.transform.localPosition = new Vector3(1.92f, 1.35f, PurchasedIcon.transform.localPosition.z);
		LimitedTimeText.transform.localPosition = new Vector3(1.68f, 0.85f, LimitedTimeText.transform.localPosition.z);
		LimitedTimeText.transform.localScale = new Vector3(1.1f, 1f, LimitedTimeText.transform.localScale.z);
		PriceLabel.transform.localPosition = new Vector3(0f, -0.97f, PriceLabel.transform.localPosition.z);
	}

	private void SetSquare()
	{
		NameText.enabled = false;
		if (PurchasedOverlay != null)
		{
			PurchasedOverlay.size = new Vector2(1.75f, 2f);
		}
		Background.size = new Vector2(1.75f, 2f);
		Background.GetComponent<BoxCollider2D>().size = new Vector2(1.75f, 2f);
		SelectionHighlight.size = new Vector2(1.75f, 2f);
		LabelMask.size = new Vector2(1.75f, 2f);
	}

	public void DoPurchase()
	{
		Parent.ShowConfirm(this);
	}

	private void OnDestroy()
	{
		if (onDestroy != null)
		{
			onDestroy();
			onDestroy = null;
		}
	}
}
