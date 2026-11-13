using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ReferenceDataManager : DestroyableSingleton<ReferenceDataManager>
{
	[SerializeField]
	private AssetReference referenceDataAsset;

	[NonSerialized]
	public bool showLoadingBar = true;

	public Dictionary<StoreTab, string> StoreTabVersions;

	public ReferenceData Refdata { get; private set; }

	// ReferenceData.asset guid (CDN/ReferenceData/ReferenceData.asset); resolved through the offline AssetGuidRegistry.
	private const string OfflineRefdataGuid = "55c71aa2de7bbe043be164fd8fc79d78";

	public bool IsRefdataLoaded => Refdata != null;

	// TODO(verify): the real MoveNext streamed ReferenceData from the Innersloth CCD addressables bucket; the offline
	// remake loads the local ReferenceData.asset instead (rule: no contact with Innersloth servers).
	public IEnumerator Initialize()
	{
		LoadOffline();
		yield break;
	}

	public void LoadOffline()
	{
		if (Refdata != null)
		{
			return;
		}
		ReferenceData data = AssetGuidRegistryAccess.Resolve(OfflineRefdataGuid) as ReferenceData;
#if UNITY_EDITOR
		if (data == null)
		{
			data = UnityEditor.AssetDatabase.LoadAssetAtPath<ReferenceData>("Assets/CDN/ReferenceData/ReferenceData.asset");
		}
#endif
		if (data == null)
		{
			Debug.LogError("ReferenceDataManager: offline ReferenceData asset not found in AssetGuidRegistry (run AmongUs > Build Asset Registry)");
			return;
		}
		Refdata = data;
		SetStoreVersions();
	}

	// Splash scene normally does ReferenceDataManager.Initialize + TranslationController.Initialize; MainMenu is our entry scene.
	public static void EnsureBooted()
	{
		ReferenceDataManager rdm = DestroyableSingleton<ReferenceDataManager>.Instance;
		rdm.LoadOffline();
		// The Splash scene (skipped when MainMenu is the entry scene) normally owns these as persistent singletons;
		// without this they die at the first scene change (TranslationController then reports STRMISS in game).
		PersistRoot(rdm.gameObject);
		PersistRoot(DestroyableSingleton<TranslationController>.Instance.gameObject);
		if (rdm.Refdata != null && !TranslationController.IsInitialized)
		{
			DestroyableSingleton<TranslationController>.Instance.Initialize();
		}
	}

	private static void PersistRoot(GameObject go)
	{
		if (go.transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(go);
		}
	}

	private void SetStoreVersions()
	{
		Dictionary<StoreTab, string> dictionary = new Dictionary<StoreTab, string>();
		ReferenceData referenceData = Refdata;
		dictionary.Add((StoreTab)1, referenceData.StoreBundlesVersion);
		ReferenceData referenceData2 = Refdata;
		dictionary.Add((StoreTab)4, referenceData2.StoreHatsVersion);
		ReferenceData referenceData3 = Refdata;
		dictionary.Add((StoreTab)0, referenceData3.StoreCubesVersion);
		ReferenceData referenceData4 = Refdata;
		dictionary.Add((StoreTab)5, referenceData4.StoreOutfitsVersion);
		ReferenceData referenceData5 = Refdata;
		dictionary.Add((StoreTab)2, referenceData5.StorePetsVersion);
		ReferenceData referenceData6 = Refdata;
		dictionary.Add((StoreTab)7, referenceData6.StoreNameplatesVersion);
		ReferenceData referenceData7 = Refdata;
		dictionary.Add((StoreTab)6, referenceData7.StoreVisorsVersion);
		StoreTabVersions = dictionary;
	}

	public FeaturedItemGroup[] GetAllFeaturedItems()
	{
		return Refdata.FeaturedItemGroups.ToArray();
	}

	public void GetCurrentFeaturedItems(out List<FeaturedItemGroup> groups)
	{
		groups = new List<FeaturedItemGroup>();
		foreach (FeaturedItemGroup group in Refdata.FeaturedItemGroups)
		{
			if (group.limitedTime.IsAvailable())
			{
				groups.Add(group);
			}
		}
	}
}
