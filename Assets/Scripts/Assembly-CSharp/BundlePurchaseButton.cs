using AmongUs.Data;
using Assets.InnerNet;

public class BundlePurchaseButton : PurchaseButton
{
	public BundleData ourBundle;

	public void UpdateBundle()
	{
		Purchased = DataManager.Player.Purchases.GetPurchase(ProductId, "");
		PurchasedIcon.enabled = Purchased;
		PurchasedOverlay.enabled = Purchased;
		RefreshPriceText();
	}

	protected override void Start()
	{
		base.Start();
		Parent = DestroyableSingleton<StoreMenu>.Instance;
		Product = ourBundle;
		Name = name;
		ProductId = ourBundle.ProdId;
		Purchased = DataManager.Player.Purchases.GetPurchase(ProductId, "");
		name = ourBundle.ProdId;
		PurchasedIcon.enabled = Purchased;
		PurchasedOverlay.enabled = Purchased;
		NameText.text = ourBundle.GetItemName();
		HatImage.FrontLayer.sprite = ourBundle.StoreImage;
		RefreshPriceText();
	}

	private void OnDisable()
	{
		PassiveButton.ReceiveMouseOut();
	}

	private void RefreshPriceText()
	{
		if (Purchased)
		{
			PriceText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Owned);
		}
		else if (ourBundle.StarCost > 0)
		{
			Price = ourBundle.StarCost.ToString();
			PriceText.text = "<sprite name=\"UI_Currency_Star\"> " + Price;
		}
		else if (ourBundle.BeanCost > 0)
		{
			Price = ourBundle.BeanCost.ToString();
			PriceText.text = "<sprite name=\"UI_Currency_Bean\"> " + Price;
		}
	}

	public override void SetItem(IBuyable product, string productId, string name, string price, bool purchased, CurrencyTypes currency)
	{
		base.SetItem(product, productId, name, price, purchased, currency);
		if (Product is BundleData)
		{
			BundleData bundleData = (BundleData)Product;
			ImageMask.enabled = true;
			ourBundle = bundleData;
		}
	}
}
