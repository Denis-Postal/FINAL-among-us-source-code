using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class MultiPrefabInstantiator : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	private List<GameObject> instances= new List<GameObject>();

	public int Count => instances.Count;

	public T Create<T>() where T : Component
	{
		GameObject gameObject = Object.Instantiate(prefab, transform);
		instances.Add(gameObject);
		return gameObject.GetComponent<T>();
	}

	public GameObject GetInstance(int index)
	{
		return instances[index];
	}

	public T GetInstanceComponent<T>(int index) where T : Component
	{
		return instances[index].GetComponent<T>();
	}

	public void DestroyInstance(GameObject obj)
	{
		instances.Remove(obj);
		Object.Destroy(obj);
	}

	public void DestroyAll()
	{
		foreach (GameObject instance in instances)
		{
			Object.Destroy(instance);
		}
		instances.Clear();
	}

	public MultiPrefabInstantiator()
	{ }
}
