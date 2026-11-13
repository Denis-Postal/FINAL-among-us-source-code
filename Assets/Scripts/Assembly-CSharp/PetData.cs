using System;
using Cpp2ILInjected;
using Innersloth.Assets;
using UnityEngine.AddressableAssets;

public class PetData : CosmeticData, IAddressableAssetProvider<PetBehaviour>
{
	public const string EmptyId = "pet_EmptyPet";

	public StringNames StoreName;

	public AssetReference PetPrefabRef;

	public bool IsEmpty => ProdId == "pet_EmptyPet";

	public AddressableAsset<PetBehaviour> CreateAddressableAsset()
	{
		return new AddressableAsset<PetBehaviour>(PetPrefabRef);
	}

	public AssetReference GetAssetReference()
	{
		return PetPrefabRef;
	}

	public override string GetItemCategory()
	{
		return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PlayerPet);
	}

	public override void PreviewOnPlayer(PoolablePlayer p, int colorId, string resetIgnoreType = "")
	{
		base.PreviewOnPlayer(p, colorId);
		p.SetPetIdle(this, colorId);
	}
}
