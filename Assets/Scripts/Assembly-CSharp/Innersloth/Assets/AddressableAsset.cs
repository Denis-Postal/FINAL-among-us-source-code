using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Innersloth.Assets
{
	// TODO(verify): rewritten by the Unity-project agent. The decompiled body of this class was unusable; this is a
	// behaviour-compatible offline re-implementation on top of the shim in unity_shims/Addressables.cs (loads are synchronous).
	[Serializable]
	public abstract class AddressableAsset : IDisposable, ISerializationCallbackReceiver
	{
		public enum AssetLoadState
		{
			Null,
			Unloaded,
			Loading,
			Success,
			Error
		}

		protected static readonly Logger logger = new Logger(Logger.Category.Assets);

		void IDisposable.Dispose()
		{
			Destroy();
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			OnSerialize();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			OnDeserialize();
		}

		public bool IsLoading()
		{
			return GetState() == AssetLoadState.Loading;
		}

		public bool IsCompleted()
		{
			AssetLoadState state = GetState();
			return state == AssetLoadState.Success || state == AssetLoadState.Error;
		}

		public bool IsLoaded()
		{
			return GetState() == AssetLoadState.Success;
		}

		protected virtual void OnSerialize()
		{
		}

		protected virtual void OnDeserialize()
		{
		}

		public abstract AssetLoadState GetState();

		public abstract void LoadAsync(Action onSuccessCb = null, Action onErrorcb = null, Action onFinishedcb = null);

		public abstract IEnumerator CoLoadAsync(Action onFinished = null);

		public abstract void Unload();

		public abstract void Destroy();

		public abstract bool ReferenceEquals(AddressableAsset asset);

		public abstract bool ReferenceEquals(AssetReference asset);
	}

	[Serializable]
	public class AddressableAsset<T> : AddressableAsset where T : UnityEngine.Object
	{
		[SerializeField]
		protected AssetReference assetRef;

		private CallbackResultGroup onLoadedCallbackGroup;

		private T loaded;

		private AssetLoadState state = AssetLoadState.Unloaded;

		public CallbackResultGroup OnLoaded => onLoadedCallbackGroup ?? (onLoadedCallbackGroup = new CallbackResultGroup());

		public AddressableAsset()
		{
		}

		public AddressableAsset(AssetReference assetRef)
		{
			this.assetRef = assetRef;
		}

		protected override void OnDeserialize()
		{
			if (onLoadedCallbackGroup == null)
			{
				onLoadedCallbackGroup = new CallbackResultGroup();
			}
		}

		public AddressableAsset<T> Clone()
		{
			return new AddressableAsset<T>(assetRef);
		}

		public virtual T GetAsset()
		{
			if (state != AssetLoadState.Success)
			{
				Resolve();
			}
			return loaded;
		}

		private void Resolve()
		{
			if (assetRef == null || !assetRef.RuntimeKeyIsValid())
			{
				state = AssetLoadState.Null;
				loaded = null;
				return;
			}
			UnityEngine.Object o = assetRef.Asset;
			if (o is GameObject go && !typeof(GameObject).IsAssignableFrom(typeof(T)) && typeof(Component).IsAssignableFrom(typeof(T)))
			{
				o = go.GetComponent(typeof(T));
			}
			loaded = o as T;
			state = loaded != null ? AssetLoadState.Success : AssetLoadState.Error;
		}

		public override IEnumerator CoLoadAsync(Action onFinished = null)
		{
			Resolve();
			CallbackResultGroup group = OnLoaded;
			if (state == AssetLoadState.Success) group.CallSuccessAndClear(); else group.CallErrorAndClear();
			group.CallFinishedAndClear();
			onFinished?.Invoke();
			yield break;
		}

		public override void LoadAsync(Action onSuccessCb = null, Action onErrorcb = null, Action onFinishedcb = null)
		{
			Resolve();
			if (state == AssetLoadState.Success) onSuccessCb?.Invoke(); else onErrorcb?.Invoke();
			onFinishedcb?.Invoke();
		}

		public IEnumerator CoWaitUntilFinished()
		{
			while (!IsCompleted())
			{
				yield return null;
			}
		}

		public override void Unload()
		{
			loaded = null;
			state = AssetLoadState.Unloaded;
		}

		public override void Destroy()
		{
			Unload();
			if (onLoadedCallbackGroup != null)
			{
				onLoadedCallbackGroup.Destroy();
				onLoadedCallbackGroup = null;
			}
			assetRef = null;
		}

		public override bool ReferenceEquals(AddressableAsset asset)
		{
			return asset is AddressableAsset<T> other && ReferenceEquals(other.assetRef);
		}

		public override bool ReferenceEquals(AssetReference asset)
		{
			return assetRef != null && asset != null && assetRef.AssetGUID == asset.AssetGUID;
		}

		public override AssetLoadState GetState()
		{
			return state;
		}
	}
}
