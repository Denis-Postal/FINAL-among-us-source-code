using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

// TODO(verify): rewritten by the Unity-project agent from the 2019 baseline + decomp9 shape (decompile was unusable).
public class DestroyableSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public bool DontDestroy;

	public static bool InstanceExists => _instance;

	public static T Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = Object.FindObjectOfType<T>();
				if (!_instance)
				{
					_instance = new GameObject().AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	public virtual void Awake()
	{
		if (!_instance)
		{
			_instance = this as T;
			if (DontDestroy)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
		}
		else if (_instance != this)
		{
			Object.Destroy(gameObject);
		}
	}

	public virtual void OnDestroy()
	{
		if (!DontDestroy && _instance == this)
		{
			_instance = null;
		}
	}
}
