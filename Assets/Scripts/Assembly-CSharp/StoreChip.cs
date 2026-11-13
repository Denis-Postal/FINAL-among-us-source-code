using AmongUs.Data;
using Innersloth.Assets;
using UnityEngine;

public class StoreChip : ColorChip
{
	public CosmeticData product;

	public StoreMannequin storeMannequin;

	public SpriteRenderer NameplateConfirmPreview;

	public StoreMenu Parent { get; set; }

	public void SetData(CosmeticData prod)
	{
		product = prod;
		if (prod is NamePlateData)
		{
			storeMannequin.gameObject.SetActive(value: false);
			NameplateConfirmPreview.gameObject.SetActive(value: true);
			AddressableAsset<NamePlateViewData> viewAsset = (prod as NamePlateData).CreateAddressableAsset();
			StartCoroutine(this.CoLoadAssetAsync(viewAsset, () =>
			{
				NameplateConfirmPreview.sprite = viewAsset.GetAsset()?.Image;
			}));
		}
		else
		{
			storeMannequin.gameObject.SetActive(value: true);
			NameplateConfirmPreview.gameObject.SetActive(value: false);
			storeMannequin.WearThing(prod, PlayerMaterial.MaskType.SimpleUI, DataManager.Player.Customization.Color);
		}
	}
}
