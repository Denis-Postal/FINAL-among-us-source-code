using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using UnityEngine;

public class HatManager : DestroyableSingleton<HatManager>
{
	public Material DefaultShader;

	public Material PlayerMaterial;

	public Material MaskedPlayerMaterial;

	public Material MaskedMaterial;

	public PetData[] allPets;

	public HatData[] allHats;

	public SkinData[] allSkins;

	public VisorData[] allVisors;

	public NamePlateData[] allNamePlates;

	public List<StarBundle> allStarBundles = new List<StarBundle>();

	public List<BundleData> allBundles = new List<BundleData>();

	public List<CosmeticData> allFeaturedItems = new List<CosmeticData>();

	public List<BundleData> allFeaturedBundles = new List<BundleData>();

	public List<CosmicubeData> allFeaturedCubes = new List<CosmicubeData>();

	private List<CosmeticData> longModeBlackList;

	private List<CosmeticData> CosmeticBlackList;

	public IReadOnlyList<HatData> AllHats => allHats;

	public IReadOnlyList<SkinData> AllSkins => allSkins;

	public IReadOnlyList<NamePlateData> AllNamePlates => allNamePlates;

	public IReadOnlyList<VisorData> AllVisors => allVisors;

	public IReadOnlyList<PetData> AllPets => allPets;

	public IReadOnlyList<BundleData> AllBundles => allBundles;

	public IReadOnlyList<StarBundle> AllStarBundles => allStarBundles;

	public IReadOnlyList<CosmeticData> AllFeaturedItems => allFeaturedItems;

	public IReadOnlyList<BundleData> AllFeaturedBundles => allFeaturedBundles;

	public IReadOnlyList<CosmicubeData> AllFeaturedCubes => allFeaturedCubes;

	public void Initialize()
	{
		ReferenceDataManager instance = DestroyableSingleton<ReferenceDataManager>.Instance;
		longModeBlackList = instance.Refdata.LongModeBlacklist;
		CosmeticBlackList = instance.Refdata.CosmeticBlacklist;
		allPets = GetSorted(instance.Refdata.pets);
		allHats = GetSorted(instance.Refdata.hats);
		allSkins = GetSorted(instance.Refdata.skins);
		allVisors = GetSorted(instance.Refdata.visors);
		allNamePlates = GetSorted(instance.Refdata.nameplates);
		allStarBundles = instance.Refdata.starBundles;
		allBundles = instance.Refdata.bundles;
		RefreshFeatured();
	}

	public void RefreshFeatured()
	{
		DestroyableSingleton<ReferenceDataManager>.Instance.GetCurrentFeaturedItems(out var groups);
		allFeaturedItems.Clear();
		allFeaturedBundles.Clear();
		allFeaturedCubes.Clear();
		foreach (FeaturedItemGroup item in groups)
		{
			foreach (CosmeticData featuredItem in item.featuredItems)
			{
				if (!allFeaturedItems.Contains(featuredItem))
				{
					allFeaturedItems.Add(featuredItem);
				}
			}
			foreach (BundleData featuredBundle in item.featuredBundles)
			{
				if (!allFeaturedBundles.Contains(featuredBundle))
				{
					allFeaturedBundles.Add(featuredBundle);
				}
			}
			foreach (CosmicubeData featuredCube in item.featuredCubes)
			{
				if (!allFeaturedCubes.Contains(featuredCube))
				{
					allFeaturedCubes.Add(featuredCube);
				}
			}
		}
	}

	public PetData GetPetById(string petId)
	{
		return allPets.FirstOrDefault((PetData p) => string.Equals(p.ProdId, petId)) ?? allPets.First((PetData p) => p.IsEmpty);
	}

	public PetData[] GetUnlockedPets()
	{
		return GetUnlocked(AllPets);
	}

	public HatData GetHatById(string hatId)
	{
		return allHats.FirstOrDefault((HatData h) => string.Equals(h.ProdId, hatId)) ?? allHats.First((HatData h) => h.IsEmpty);
	}

	public HatData[] GetUnlockedHats()
	{
		return GetUnlocked(allHats);
	}

	public SkinData[] GetUnlockedSkins()
	{
		return GetUnlocked(allSkins);
	}

	public SkinData GetSkinById(string skinId)
	{
		return allSkins.FirstOrDefault((SkinData s) => string.Equals(s.ProdId, skinId)) ?? allSkins.First((SkinData s) => s.IsEmpty);
	}

	public VisorData[] GetUnlockedVisors()
	{
		return GetUnlocked(allVisors);
	}

	public VisorData GetVisorById(string visorId)
	{
		return allVisors.FirstOrDefault((VisorData v) => string.Equals(v.ProdId, visorId)) ?? allVisors.First((VisorData v) => v.IsEmpty);
	}

	public NamePlateData[] GetUnlockedNamePlates()
	{
		return GetUnlocked(allNamePlates);
	}

	public NamePlateData GetNamePlateById(string namePlateId)
	{
		return allNamePlates.FirstOrDefault((NamePlateData n) => string.Equals(n.ProdId, namePlateId)) ?? allNamePlates.First((NamePlateData np) => np.IsEmpty);
	}

	public bool CheckValidCosmetic(string cosmeticID)
	{
		foreach (CosmeticData cosmeticBlack in CosmeticBlackList)
		{
			if (string.Equals(cosmeticBlack.ProdId, cosmeticID))
			{
				return false;
			}
		}
		return true;
	}

	public bool CheckLongModeValidCosmetic(string cosmeticID, bool ignoreLongMode = false)
	{
		if (AprilFoolsMode.ShouldLongAround() && !ignoreLongMode)
		{
			foreach (CosmeticData longModeBlack in longModeBlackList)
			{
				if (string.Equals(longModeBlack.ProdId, cosmeticID))
				{
					return false;
				}
			}
		}
		return true;
	}

	private T[] GetSorted<T>(List<T> all) where T : CosmeticData
	{
		CosmeticGroups groups = DestroyableSingleton<ReferenceDataManager>.Instance.Refdata.Groups;
		T[] array = (from x in all
			orderby x.displayOrder, groups.GetReleaseGroupDate(x) descending, groups.GetReleaseGroupIndex(x)
			select x).ToArray();
		List<T> list = new List<T>();
		T[] array2 = array;
		foreach (T val in array2)
		{
			if (list.Contains(val))
			{
				continue;
			}
			if (CheckValidCosmetic(val.ProdId))
			{
				list.Add(val);
			}
			if (!groups.TryGetVariantGroup(val, out var output) || output == null || output.Count < 1 || output.IDs[0] != val.ProductId)
			{
				continue;
			}
			foreach (string variantID in output.IDs)
			{
				if (!string.IsNullOrWhiteSpace(variantID) && !(variantID == val.ProductId))
				{
					list.RemoveAll((T x) => x.ProductId == variantID);
					T val2 = array.Single((T s) => s.ProductId == variantID);
					if (val2 != null && CheckValidCosmetic(val2.ProdId))
					{
						list.Add(val2);
					}
				}
			}
		}
		return list.ToArray();
	}

	private T[] GetUnlocked<T>(IEnumerable<T> all) where T : CosmeticData
	{
		return all.Where((T x) => x.Free || DataManager.Player.Purchases.GetPurchase(x.ProductId, x.BundleId)).ToArray();
	}
}
