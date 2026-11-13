using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using UnityEngine;

public class CosmicubeManager : DestroyableSingleton<CosmicubeManager>
{
	public static Dictionary<UnlockableCosmetic.UnlockableQuality, Color> qualityColorMap = new Dictionary<UnlockableCosmetic.UnlockableQuality, Color>
	{
		{
			UnlockableCosmetic.UnlockableQuality.Nameplate,
			Palette.CosmicubeQuality_NamePlate
		},
		{
			UnlockableCosmetic.UnlockableQuality.Hat,
			Palette.CosmicubeQuality_Hat
		},
		{
			UnlockableCosmetic.UnlockableQuality.Visor,
			Palette.CosmicubeQuality_Visor
		},
		{
			UnlockableCosmetic.UnlockableQuality.Skin,
			Palette.CosmicubeQuality_Skin
		},
		{
			UnlockableCosmetic.UnlockableQuality.Pet,
			Palette.CosmicubeQuality_Pet
		},
		{
			UnlockableCosmetic.UnlockableQuality.Kill,
			Palette.CosmicubeQuality_Kill
		}
	};

	private List<CosmicubeData> allCubes;

	public CosmicubeData GetCubeDataByID(string id)
	{
		return allCubes.FirstOrDefault((CosmicubeData h) => h.ProdId == id) ?? allCubes.Find((CosmicubeData c) => c.IsEmpty);
	}

	public void Initialize()
	{
		allCubes = DestroyableSingleton<ReferenceDataManager>.Instance.Refdata.cosmicubes;
		// Offline remake: the original pre-downloads the cube store images through Addressables (StartDownload). There is no CDN here
		// and the images are part of the build, so nothing is downloaded.
	}

	public IEnumerable<CosmicubeData> GetAllCubeData()
	{
		return allCubes;
	}

	public CosmicubeData[] GetAvailableCubeData()
	{
		return (from h in GetAllCubeData()
			where h.LimitedTimeAvailable.IsAvailable()
			where !h.Free && !h.NotInStore
			select h).ToArray();
	}

	public CosmicubeData[] GetUnlockedCubes()
	{
		return (from o in allCubes.Where((CosmicubeData s, int index) => (s.Free || DataManager.Player.Purchases.GetPurchase(s.ProdId, "")) && !s.IsEmpty)
			orderby o.Order descending, o.ProdId
			select o).ToArray();
	}

	public CosmicubeData GetDataForPodId(string podId)
	{
		return allCubes.FirstOrDefault((CosmicubeData c) => c.podId == podId);
	}

	public float GetCompletionProgress(CosmicubeData cube)
	{
		int num = 0;
		foreach (UnlockableCosmetic allUnlockable in cube.allUnlockables)
		{
			if (allUnlockable == null || DataManager.Player.Purchases.GetPurchase(allUnlockable.GetProdId(), allUnlockable.GetBundleId()))
			{
				num++;
			}
		}
		return (float)num / (float)cube.allUnlockables.Count;
	}

	public bool IsCompleted(CosmicubeData cube)
	{
		int num = 0;
		foreach (UnlockableCosmetic allUnlockable in cube.allUnlockables)
		{
			if (allUnlockable == null || DataManager.Player.Purchases.GetPurchase(allUnlockable.GetProdId(), allUnlockable.GetBundleId()))
			{
				num++;
			}
		}
		return num == cube.allUnlockables.Count;
	}
}
