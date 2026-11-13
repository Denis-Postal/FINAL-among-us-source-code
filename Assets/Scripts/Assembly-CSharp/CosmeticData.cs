using System;
using System.Collections;
using System.Collections.Generic;
using Innersloth.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CosmeticData : ScriptableObject, IBuyable
{
	public const string TranslationPrefix = "Cosmetic.";

	public List<Platforms> unlockOnSelectPlatforms;

	public bool freeRedeemableCosmetic;

	public int redeemPopUpColor = -1;

	public string epicId = "";

	public string BundleId;

	public string ProductId;

	public Vector2 ChipOffset;

	public int beanCost;

	public int starCost;

	public bool paidOnMobile;

	public LimitedTimeStartEnd limitedTime;

	public int displayOrder;

	public bool NotInStore;

	public bool Free;

	public AssetReference PreviewData;

	public bool PreviewCrewmateColor;

	public string ProdId => ProductId;

	public int BeanCost => beanCost;

	public int StarCost => starCost;

	public string EpicId => epicId;

	public bool PaidOnMobile => paidOnMobile;

	public LimitedTimeStartEnd LimitedTimeAvailable => limitedTime;

	public virtual void PreviewOnPlayer(PoolablePlayer p, int colorId, string ignoreType = "")
	{
		p.ResetCosmetics(ignoreType);
	}

	public string GetItemName()
	{
		return DestroyableSingleton<TranslationController>.Instance.GetString("Cosmetic." + ProductId, name);
	}

	public virtual string GetItemCategory()
	{
		return string.Empty;
	}

	public virtual void SetProdId()
	{
	}

	public virtual IEnumerator CoLoadIcon(Action<Sprite, AddressableAsset> onLoaded)
	{
		yield return null;
		onLoaded(null, null);
	}

	public void CoLoadPreview(Action<Sprite, AddressableAsset> onLoaded)
	{
		Sprite sprite = null;
		if (PreviewData.RuntimeKeyIsValid())
		{
			AddressableAsset<PreviewViewData> asset = new AddressableAsset<PreviewViewData>(PreviewData);
			asset.LoadAsync(() =>
			{
				sprite = asset.GetAsset()?.PreviewSprite;
				onLoaded(sprite, asset);
			});
		}
	}

	public void SetPreview(SpriteRenderer renderer, int color)
	{
		CoLoadPreview((Sprite sprite, AddressableAsset asset) =>
		{
			if (renderer != null)
			{
				renderer.sprite = sprite;
				AddressableAssetHandler.AddToGameObject(renderer.gameObject, asset);
				if (Application.isPlaying)
				{
					PlayerMaterial.SetColors(color, renderer);
				}
			}
			else
			{
				asset.Unload();
			}
		});
	}
}
