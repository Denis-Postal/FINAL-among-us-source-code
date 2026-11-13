using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

namespace Innersloth.Assets
{
	// TODO(verify): rewritten by the Unity-project agent.
	public class AddressableAssetHandler : MonoBehaviour
	{
		public AddressableAssetGroup assetGroup= new AddressableAssetGroup();

		public static (AddressableAssetHandler, AddressableAssetGroup) AddToGameObject(GameObject obj, AddressableAsset asset = null)
		{
			AddressableAssetHandler addressableAssetHandler = obj.AddComponent<AddressableAssetHandler>();
			addressableAssetHandler.assetGroup.Add(asset);
			return (addressableAssetHandler, addressableAssetHandler.assetGroup);
		}

		public static (AddressableAssetHandler, AddressableAssetGroup) GetOrCreate(GameObject obj)
		{
			AddressableAssetHandler addressableAssetHandler = obj.GetComponent<AddressableAssetHandler>();
			if (addressableAssetHandler == null)
			{
				addressableAssetHandler = obj.AddComponent<AddressableAssetHandler>();
			}
			return (addressableAssetHandler, addressableAssetHandler.assetGroup);
		}

		private void OnDestroy()
		{
			assetGroup?.Destroy();
			assetGroup = null;
		}

		public AddressableAssetHandler()
		{ }
	}
}
