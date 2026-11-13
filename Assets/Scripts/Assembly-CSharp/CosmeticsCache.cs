using System;
using System.Collections;
using System.Collections.Generic;
using Innersloth.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CosmeticsCache
{
	private readonly AddressableAssetGroup allCachedAssets = new AddressableAssetGroup();

	private Dictionary<string, AddressableAsset<HatViewData>> hats;

	public bool initialized;

	private Dictionary<string, AddressableAsset<NamePlateViewData>> nameplates;

	private Dictionary<string, AddressableAsset<PetBehaviour>> pets;

	private Dictionary<string, AddressableAsset<SkinViewData>> skins;

	private Dictionary<string, AddressableAsset<VisorViewData>> visors;

	public IEnumerator PopulateFromPlayers()
	{
		HashSet<string> hashSet = new HashSet<string> { "hat_NoHat" };
		HashSet<string> skinIds = new HashSet<string> { "skin_None" };
		HashSet<string> visorIds = new HashSet<string> { "visor_EmptyVisor" };
		HashSet<string> namePlateIds = new HashSet<string> { "nameplate_NoPlate" };
		HashSet<string> petIds = new HashSet<string> { "pet_EmptyPet" };
		foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
		{
			if (!(allPlayerControl == null) && allPlayerControl.CurrentOutfit != null && !(allPlayerControl.gameObject == null))
			{
				NetworkedPlayerInfo.PlayerOutfit currentOutfit = allPlayerControl.CurrentOutfit;
				hashSet.Add(currentOutfit.HatId);
				skinIds.Add(currentOutfit.SkinId);
				visorIds.Add(currentOutfit.VisorId);
				namePlateIds.Add(currentOutfit.NamePlateId);
				petIds.Add(currentOutfit.PetId);
			}
		}
		hashSet.RemoveWhere((string x) => string.IsNullOrWhiteSpace(x));
		skinIds.RemoveWhere((string x) => string.IsNullOrWhiteSpace(x));
		visorIds.RemoveWhere((string x) => string.IsNullOrWhiteSpace(x));
		namePlateIds.RemoveWhere((string x) => string.IsNullOrWhiteSpace(x));
		petIds.RemoveWhere((string x) => string.IsNullOrWhiteSpace(x));
		hats = new Dictionary<string, AddressableAsset<HatViewData>>();
		skins = new Dictionary<string, AddressableAsset<SkinViewData>>();
		nameplates = new Dictionary<string, AddressableAsset<NamePlateViewData>>();
		visors = new Dictionary<string, AddressableAsset<VisorViewData>>();
		pets = new Dictionary<string, AddressableAsset<PetBehaviour>>();
		initialized = true;
		foreach (string item in hashSet)
		{
			yield return CoAddHat(item);
		}
		foreach (string item2 in skinIds)
		{
			yield return CoAddSkin(item2);
		}
		foreach (string item3 in visorIds)
		{
			yield return CoAddVisor(item3);
		}
		foreach (string item4 in petIds)
		{
			yield return CoAddPet(item4);
		}
		foreach (string item5 in namePlateIds)
		{
			yield return CoAddNameplate(item5);
		}
		yield return null;
	}

	public IEnumerator CoAddNameplate(string namePlateId)
	{
		if (!nameplates.ContainsKey(namePlateId))
		{
			NamePlateData namePlateById = DestroyableSingleton<HatManager>.Instance.GetNamePlateById(namePlateId);
			if (namePlateById != null)
			{
				AddressableAsset<NamePlateViewData> asset = namePlateById.CreateAddressableAsset();
				allCachedAssets.Add(asset);
				yield return asset.CoLoadAsync();
				nameplates[namePlateId] = asset;
			}
		}
	}

	public IEnumerator CoAddPet(string petId)
	{
		if (!pets.ContainsKey(petId))
		{
			PetData petById = DestroyableSingleton<HatManager>.Instance.GetPetById(petId);
			if (petById != null)
			{
				AddressableAsset<PetBehaviour> asset = petById.CreateAddressableAsset();
				allCachedAssets.Add(asset);
				yield return asset.CoLoadAsync();
				pets[petId] = asset;
			}
		}
	}

	public IEnumerator CoAddVisor(string visorId)
	{
		if (!visors.ContainsKey(visorId))
		{
			VisorData visorById = DestroyableSingleton<HatManager>.Instance.GetVisorById(visorId);
			if (visorById != null)
			{
				AddressableAsset<VisorViewData> asset = visorById.CreateAddressableAsset();
				allCachedAssets.Add(asset);
				yield return asset.CoLoadAsync();
				visors[visorId] = asset;
			}
		}
	}

	public IEnumerator CoAddSkin(string id)
	{
		if (!skins.ContainsKey(id))
		{
			SkinData skinById = DestroyableSingleton<HatManager>.Instance.GetSkinById(id);
			if (skinById != null)
			{
				AddressableAsset<SkinViewData> asset = skinById.CreateAddressableAsset();
				allCachedAssets.Add(asset);
				yield return asset.CoLoadAsync();
				skins[id] = asset;
			}
		}
	}

	public IEnumerator CoAddHat(string id)
	{
		if (!hats.ContainsKey(id))
		{
			HatData hatById = DestroyableSingleton<HatManager>.Instance.GetHatById(id);
			if (hatById != null)
			{
				AddressableAsset<HatViewData> asset = hatById.CreateAddressableAsset();
				allCachedAssets.Add(asset);
				yield return asset.CoLoadAsync();
				hats[id] = asset;
			}
		}
	}

	public SkinViewData GetSkin(string id)
	{
		// TODO(verify): native cache/fallback with synchronous loading through the existing offline asset shim (mirrors GetNameplate).
		if (skins == null) skins = new Dictionary<string, AddressableAsset<SkinViewData>>();
		AddressableAsset<SkinViewData> asset;
		if (id != null && skins.TryGetValue(id, out asset) && asset != null) return asset.GetAsset();
		var data = DestroyableSingleton<HatManager>.Instance.GetSkinById(id);
		if (!data) throw new InvalidOperationException("Missing local skin data: " + id);
		asset = data.CreateAddressableAsset();
		skins[data.ProductId] = asset;
		return asset.GetAsset();
	}

	public HatViewData GetHat(string id)
	{
		// TODO(verify): native cache/fallback with synchronous loading through the existing offline asset shim (mirrors GetNameplate).
		if (hats == null) hats = new Dictionary<string, AddressableAsset<HatViewData>>();
		AddressableAsset<HatViewData> asset;
		if (id != null && hats.TryGetValue(id, out asset) && asset != null) return asset.GetAsset();
		var data = DestroyableSingleton<HatManager>.Instance.GetHatById(id);
		if (!data) throw new InvalidOperationException("Missing local hat data: " + id);
		asset = data.CreateAddressableAsset();
		hats[data.ProductId] = asset;
		return asset.GetAsset();
	}

	public PetBehaviour GetPet(string id)
	{
		// TODO(verify): native cache/fallback with synchronous loading through the existing offline asset shim (mirrors GetNameplate).
		if (pets == null) pets = new Dictionary<string, AddressableAsset<PetBehaviour>>();
		AddressableAsset<PetBehaviour> asset;
		if (id != null && pets.TryGetValue(id, out asset) && asset != null) return asset.GetAsset();
		var data = DestroyableSingleton<HatManager>.Instance.GetPetById(id);
		if (!data) throw new InvalidOperationException("Missing local pet data: " + id);
		asset = data.CreateAddressableAsset();
		pets[data.ProductId] = asset;
		return asset.GetAsset();
	}

	public bool HasVisor(string id)
	{
		if (visors != null && visors.ContainsKey(id))
		{
			return visors[id] != null;
		}
		return false;
	}

	public VisorViewData GetVisor(string id)
	{
		// TODO(verify): native cache/fallback with synchronous loading through the existing offline asset shim (mirrors GetNameplate).
		if (visors == null) visors = new Dictionary<string, AddressableAsset<VisorViewData>>();
		AddressableAsset<VisorViewData> asset;
		if (id != null && visors.TryGetValue(id, out asset) && asset != null) return asset.GetAsset();
		var data = DestroyableSingleton<HatManager>.Instance.GetVisorById(id);
		if (!data) throw new InvalidOperationException("Missing local visor data: " + id);
		asset = data.CreateAddressableAsset();
		visors[data.ProductId] = asset;
		return asset.GetAsset();
	}

	public NamePlateViewData GetNameplate(string id)
	{
		// TODO(verify): native cache/NoPlate fallback with synchronous loading through the existing offline asset shim.
		if (nameplates == null) nameplates = new Dictionary<string, AddressableAsset<NamePlateViewData>>();
		AddressableAsset<NamePlateViewData> asset;
		if (nameplates.TryGetValue(id, out asset) && asset != null) return asset.GetAsset();
		var data = DestroyableSingleton<HatManager>.Instance.GetNamePlateById(id);
		if (!data) throw new InvalidOperationException("Missing local nameplate data: " + id);
		asset = data.CreateAddressableAsset();
		nameplates[data.ProductId] = asset;
		return asset.GetAsset();
	}

	public void Destroy()
	{
		hats?.Clear();
		skins?.Clear();
		nameplates?.Clear();
		pets?.Clear();
		visors?.Clear();
		allCachedAssets?.Destroy();
	}

	public IEnumerator CoPopulateFromOutfit(NetworkedPlayerInfo.PlayerOutfit outfit)
	{
		yield return CoAddHat(outfit.HatId);
		yield return CoAddVisor(outfit.VisorId);
		yield return CoAddSkin(outfit.SkinId);
		yield return CoAddPet(outfit.PetId);
		yield return CoAddNameplate(outfit.NamePlateId);
	}

	// TODO(verify): the original also unloads each player's unused pet/visor/skin AddressableAssets before this; the offline remake has no
	// Addressables bundles to release, so only the engine-side unload is done.
	public static void ClearUnusedCosmetics()
	{
		Resources.UnloadUnusedAssets();
	}
}
