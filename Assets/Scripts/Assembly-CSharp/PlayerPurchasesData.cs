using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Player;
using UnityEngine;

public class PlayerPurchasesData
{
	// Offline remake / testing: there is no store, so every cosmetic, bundle and cosmicube item counts as owned (inventory, nameplates,
	// cosmicube, shop "purchased" state all go through GetPurchase). Set to false to use only the saved purchase list.
	public const bool UnlockEverythingForTesting = true;

	private List<string> purchases;

	public PlayerPurchasesData()
	{
		purchases = new List<string>();
	}

	public void UpdateLegacyPurchases()
	{
		string[] keys = PlayerStoreData.LEGACY_PURCHASE_KEYS;
		foreach (string key in keys)
		{
			ClearPurchase(key);
		}
		keys = DataManager.Player.Store.GetLegacyPurchases();
		foreach (string purchased in keys)
		{
			SetPurchased(purchased);
		}
	}

	public bool GetPurchase(string itemKey, string bundleKey)
	{
		if (UnlockEverythingForTesting)
		{
			return true;
		}
		bool owned = false;
		if (!string.IsNullOrEmpty(bundleKey))
		{
			owned = purchases.Contains(bundleKey);
		}
		if (!owned && !string.IsNullOrEmpty(itemKey))
		{
			owned = purchases.Contains(itemKey);
		}
		return owned;
	}

	public void SetPurchased(string key)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			Debug.LogWarning("[PlayerPurchasesData] Trying to SetPurchased for EMPTY key. Aborting.");
			return;
		}
		if (key == "bought_ads")
		{
			DataManager.Player.Ads.AdsState = ShowAdsState.Purchased;
			DataManager.Player.Save();
		}
		if (!purchases.Contains(key))
		{
			purchases.Add(key);
		}
	}

	public void ClearPurchase(string key)
	{
		purchases.Remove(key);
	}

	public void ClearAllPurchases()
	{
		List<string> kept = new List<string>();
		foreach (string purchase in purchases)
		{
			if (PlayerStoreData.LEGACY_PURCHASE_KEYS.Contains(purchase))
			{
				kept.Add(purchase);
			}
		}
		purchases = kept;
	}
}
