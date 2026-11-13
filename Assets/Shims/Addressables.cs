// Offline shim for Unity Addressables. AssetReference serializes like the real one (m_AssetGUID...) so exported
// prefabs/scenes bind; at runtime guids resolve through AssetGuidRegistry (Resources/AssetGuidRegistry.asset, built by the
// editor tool AmongUs/Build Asset Registry). TODO(verify): local re-implementation, not original behaviour.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace UnityEngine.ResourceManagement.AsyncOperations
{
	public enum AsyncOperationStatus { None, Succeeded, Failed }
	public struct AsyncOperationHandle : IEnumerator
	{
		internal object result; internal AsyncOperationStatus status; internal bool valid; internal Exception ex;
		public AsyncOperationStatus Status => status;
		public bool IsDone => valid && status != AsyncOperationStatus.None;
		public bool IsValid() => valid;
		public object Result => result;
		public float PercentComplete => IsDone ? 1f : 0f;
		public Exception OperationException => ex;
		public event Action<AsyncOperationHandle> Completed { add { value(this); } remove { } }
		public AsyncOperationHandle<T> Convert<T>() => new AsyncOperationHandle<T>(this);
		public object WaitForCompletion() => result;
		public bool MoveNext() => !IsDone;
		public void Reset() { }
		public object Current => null;
		internal static AsyncOperationHandle Done(object r, bool ok = true) => new AsyncOperationHandle { result = r, status = ok ? AsyncOperationStatus.Succeeded : AsyncOperationStatus.Failed, valid = true };
	}
	public struct AsyncOperationHandle<T> : IEnumerator
	{
		internal AsyncOperationHandle h;
		internal AsyncOperationHandle(AsyncOperationHandle h) { this.h = h; }
		public static implicit operator AsyncOperationHandle(AsyncOperationHandle<T> o) => o.h;
		public AsyncOperationStatus Status => h.status;
		public bool IsDone => h.IsDone;
		public bool IsValid() => h.valid;
		public T Result => h.result is T t ? t : default;
		public float PercentComplete => h.PercentComplete;
		public Exception OperationException => h.ex;
		public event Action<AsyncOperationHandle<T>> Completed { add { value(this); } remove { } }
		public T WaitForCompletion() => Result;
		public bool MoveNext() => !IsDone;
		public void Reset() { }
		public object Current => null;
		internal static AsyncOperationHandle<T> Done(T r, bool ok = true) => new AsyncOperationHandle<T>(AsyncOperationHandle.Done(r, ok));
	}
}
namespace UnityEngine.ResourceManagement { }
namespace UnityEngine.AddressableAssets
{
	// AssetGuidRegistry lives in its own file
	public static class AssetGuidRegistryAccess
	{
		static Dictionary<string, Object> map;
		public static Object Resolve(string guid)
		{
			if (string.IsNullOrEmpty(guid)) return null;
			if (map == null)
			{
				map = new Dictionary<string, Object>(StringComparer.OrdinalIgnoreCase);
				var reg = Resources.Load<AssetGuidRegistry>("AssetGuidRegistry");
				if (reg != null) for (int i = 0; i < Math.Min(reg.guids.Count, reg.objects.Count); i++) map[reg.guids[i]] = reg.objects[i];
			}
			return map.TryGetValue(guid, out var o) ? o : null;
		}
	}
	[Serializable]
	public class AssetReference
	{
		[SerializeField] protected string m_AssetGUID;
		[SerializeField] string m_SubObjectName;
		[SerializeField] string m_SubObjectType;
		[SerializeField] bool m_EditorAssetChanged;
		public AssetReference() { }
		public AssetReference(string guid) { m_AssetGUID = guid; }
		public virtual string AssetGUID => m_AssetGUID;
		public string SubObjectName { get => m_SubObjectName; set => m_SubObjectName = value; }
		public virtual object RuntimeKey => m_AssetGUID;
		public virtual bool RuntimeKeyIsValid() => !string.IsNullOrEmpty(m_AssetGUID);
		public virtual bool IsValid() => RuntimeKeyIsValid();
		public virtual bool IsDone => true;
		public Object Asset
		{
			get
			{
				if (!string.IsNullOrEmpty(m_SubObjectName))
				{
					var sub = AssetGuidRegistryAccess.Resolve(m_AssetGUID + "#" + m_SubObjectName);
					if (sub != null) return sub;
				}
				return AssetGuidRegistryAccess.Resolve(m_AssetGUID);
			}
		}
		public virtual Object editorAsset => Asset;
		AsyncOperationHandle op;
		public AsyncOperationHandle OperationHandle => op;
		public AsyncOperationHandle<TObject> LoadAssetAsync<TObject>()
		{
			var o = Asset;
			if (o is Texture2D && typeof(TObject) == typeof(Sprite)) o = AssetGuidRegistryAccess.Resolve(m_AssetGUID + "#") ?? o; // texture without a named sub-sprite: its first sprite
			if (o is GameObject go && !typeof(TObject).IsAssignableFrom(typeof(GameObject)) && typeof(Component).IsAssignableFrom(typeof(TObject))) o = go.GetComponent(typeof(TObject));
			var h = AsyncOperationHandle<TObject>.Done(o is TObject t ? t : default, o is TObject);
			op = h; return h;
		}
		public AsyncOperationHandle<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			var go = Asset as GameObject;
			return AsyncOperationHandle<GameObject>.Done(go != null ? Object.Instantiate(go, position, rotation, parent) : null, go != null);
		}
		public AsyncOperationHandle<GameObject> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
		{
			var go = Asset as GameObject;
			return AsyncOperationHandle<GameObject>.Done(go != null ? Object.Instantiate(go, parent, instantiateInWorldSpace) : null, go != null);
		}
		public virtual void ReleaseAsset() { op = default; }
		public void ReleaseInstance(GameObject instance) { if (instance != null) Object.Destroy(instance); }
		public virtual bool ValidateAsset(Object obj) => true;
		public virtual bool ValidateAsset(string path) => true;
		public override string ToString() => "[" + m_AssetGUID + "]";
	}
	[Serializable] public class AssetReferenceT<TObject> : AssetReference where TObject : Object { public AssetReferenceT() { } public AssetReferenceT(string guid) : base(guid) { } }
	[Serializable] public class AssetReferenceGameObject : AssetReferenceT<GameObject> { public AssetReferenceGameObject() { } public AssetReferenceGameObject(string guid) : base(guid) { } }
	[Serializable] public class AssetReferenceSprite : AssetReferenceT<Sprite> { public AssetReferenceSprite() { } public AssetReferenceSprite(string guid) : base(guid) { } }
	[Serializable] public class AssetReferenceTexture2D : AssetReferenceT<Texture2D> { public AssetReferenceTexture2D() { } public AssetReferenceTexture2D(string guid) : base(guid) { } }
	[Serializable] public class AssetLabelReference { [SerializeField] string m_LabelString; public string labelString { get => m_LabelString; set => m_LabelString = value; } }
	public static class Addressables
	{
		public static AsyncOperationHandle<T> LoadAssetAsync<T>(AssetReference r) => r.LoadAssetAsync<T>();
		public static AsyncOperationHandle<T> LoadAssetAsync<T>(object key)
		{
			if (key is AssetReference reference) return reference.LoadAssetAsync<T>();
			var o = key is string s ? AssetGuidRegistryAccess.Resolve(s) ?? Resources.Load(s) : null;
			if (o is GameObject go && typeof(Component).IsAssignableFrom(typeof(T))) o = go.GetComponent(typeof(T));
			return AsyncOperationHandle<T>.Done(o is T t ? t : default, o is T);
		}
		public static void Release(AsyncOperationHandle h) { }
		public static void Release<T>(AsyncOperationHandle<T> h) { }
		public static bool ReleaseInstance(GameObject go) { if (go) Object.Destroy(go); return true; }
		public static AsyncOperationHandle<long> GetDownloadSizeAsync(object key) => AsyncOperationHandle<long>.Done(0);
		public static AsyncOperationHandle DownloadDependenciesAsync(object key, bool autoRelease = false) => AsyncOperationHandle.Done(null);
		public static AsyncOperationHandle InitializeAsync() => AsyncOperationHandle.Done(null);
	}
}
