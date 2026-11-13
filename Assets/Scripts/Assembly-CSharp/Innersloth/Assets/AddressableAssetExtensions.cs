using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Innersloth.Assets
{
	// TODO(verify): rewritten by the Unity-project agent (decompiled iterator bodies were unusable). Loads complete synchronously.
	public static class AddressableAssetExtensions
	{
		private static T LoadRef<T>(AssetReference reference) where T : UnityEngine.Object
		{
			if (reference == null) return null;
			AsyncOperationHandle<T> handle = reference.LoadAssetAsync<T>();
			return handle.Result;
		}

		public static IEnumerator CoLoadAssetAsync(this MonoBehaviour behavior, AddressableAsset asset, Action onFinished)
		{
			yield return asset.CoLoadAsync(onFinished);
		}

		public static IEnumerator CoLoadAssetAsync(this GameObject gameObject, AddressableAsset asset, Action onFinished)
		{
			yield return asset.CoLoadAsync(onFinished);
		}

		public static IEnumerator CoLoadAssetAsync<T>(this MonoBehaviour behavior, AssetReference reference, Action<T> onFinished) where T : UnityEngine.Object
		{
			onFinished?.Invoke(LoadRef<T>(reference));
			yield break;
		}

		public static IEnumerator CoLoadAssetAsync<T>(this GameObject gameObject, AssetReference reference, Action<T> onFinished) where T : UnityEngine.Object
		{
			onFinished?.Invoke(LoadRef<T>(reference));
			yield break;
		}

		public static IEnumerator CoLoadAssetAsync<T>(this MonoBehaviour behavior, IAddressableAssetProvider<T> provider, Action<T> onFinished) where T : UnityEngine.Object
		{
			onFinished?.Invoke(LoadRef<T>(provider.GetAssetReference()));
			yield break;
		}

		public static IEnumerator CoLoadAssetAsync<T>(this GameObject gameObject, IAddressableAssetProvider<T> provider, Action<T> onFinished) where T : UnityEngine.Object
		{
			onFinished?.Invoke(LoadRef<T>(provider.GetAssetReference()));
			yield break;
		}

		public static void UnloadAddressableAsset(this MonoBehaviour behavior, AddressableAsset asset)
		{
			asset?.Unload();
		}

		public static void UnloadAddressableAsset<T>(this MonoBehaviour behavior, IAddressableAssetProvider<T> provider) where T : UnityEngine.Object
		{
			// nothing to release (no addressable handles in the offline shim)
		}

		public static void LoadAssetAsync(this MonoBehaviour behavior, AddressableAsset asset, Action onSuccess, Action onError = null)
		{
			asset.LoadAsync(onSuccess, onError);
		}

		public static void LoadAssetAsync(this GameObject gameObject, AddressableAsset asset, Action onSuccess, Action onError = null)
		{
			asset.LoadAsync(onSuccess, onError);
		}

		public static void LoadAssetAsync<T>(this MonoBehaviour behavior, AssetReference reference, Action<T> onSuccess, Action onError = null) where T : UnityEngine.Object
		{
			T result = LoadRef<T>(reference);
			if (result != null) onSuccess?.Invoke(result); else onError?.Invoke();
		}

		public static void LoadAssetAsync<T>(this GameObject gameObject, AssetReference reference, Action<T> onSuccess, Action onError = null) where T : UnityEngine.Object
		{
			T result = LoadRef<T>(reference);
			if (result != null) onSuccess?.Invoke(result); else onError?.Invoke();
		}

		public static void LoadAssetAsync<T>(this MonoBehaviour behavior, IAddressableAssetProvider<T> provider, Action<T> onSuccess, Action onError = null) where T : UnityEngine.Object
		{
			LoadAssetAsync<T>(behavior, provider.GetAssetReference(), onSuccess, onError);
		}

		public static void LoadAssetAsync<T>(this GameObject gameObject, IAddressableAssetProvider<T> provider, Action<T> onSuccess, Action onError = null) where T : UnityEngine.Object
		{
			LoadAssetAsync<T>(gameObject, provider.GetAssetReference(), onSuccess, onError);
		}

		public static AddressableAssetGroup GetAssetGroup(this MonoBehaviour behaviour)
		{
			GameObject gameObject = behaviour.gameObject;
			AddressableAssetHandler handler = gameObject.GetComponent<AddressableAssetHandler>();
			if (handler == null)
			{
				handler = gameObject.AddComponent<AddressableAssetHandler>();
			}
			return handler.assetGroup;
		}
	}
}
