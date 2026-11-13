using Innersloth.Assets;
using TMPro;
using UnityEngine;

public class RedeemCosmeticPopup : MonoBehaviour
{
	public TextMeshPro cosmeticText;

	public StoreMannequin storeMannequin;

	public SpriteRenderer dummyNameplate;

	public void SetRedeemablePopup(CosmeticData cosmetic, bool isFromCode = false)
	{
		if (cosmetic is NamePlateData namePlateData)
		{
			storeMannequin.gameObject.SetActive(value: false);
			AddressableAsset<NamePlateViewData> viewAsset = namePlateData.CreateAddressableAsset();
			StartCoroutine(this.CoLoadAssetAsync(viewAsset, () =>
			{
				dummyNameplate.gameObject.SetActive(value: true);
				dummyNameplate.sprite = viewAsset.GetAsset()?.Image;
			}));
		}
		else
		{
			dummyNameplate.gameObject.SetActive(value: false);
			storeMannequin.gameObject.SetActive(value: true);
			storeMannequin.WearThing(cosmetic, PlayerMaterial.MaskType.None, cosmetic.redeemPopUpColor);
		}
		if (isFromCode)
		{
			cosmeticText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.CodeRedeemPopupLabel, cosmetic.GetItemName());
		}
		else
		{
			cosmeticText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RedeemPopup, cosmetic.GetItemName());
		}
	}
}
