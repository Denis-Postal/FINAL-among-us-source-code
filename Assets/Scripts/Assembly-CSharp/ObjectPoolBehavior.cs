using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Threading;
using Cpp2ILInjected;
using UnityEngine;

public class ObjectPoolBehavior : IObjectPool
{
	public int poolSize = 20;

	[SerializeField]
	private List<PoolableBehavior> inactiveChildren= new List<PoolableBehavior>();

	[SerializeField]
	public List<PoolableBehavior> activeChildren= new List<PoolableBehavior>();

	public PoolableBehavior Prefab;

	public bool AutoInit;

	public bool DetachOnGet;

	private int childIndex;

	public override int InUse => activeChildren.Count;

	public override int NotInUse => inactiveChildren.Count;

	public virtual void Awake()
	{
		if (AutoInit)
		{
			InitPool(Prefab);
		}
	}

	public void InitPool(PoolableBehavior prefab)
	{ /* spliced-from-2019 */
		AutoInit = false;
		for (int i = 0; i < poolSize; i++)
		{
			CreateOneInactive(prefab);
		}
	}

	private void CreateOneInactive(PoolableBehavior prefab)
	{
		PoolableBehavior poolableBehavior = Object.Instantiate(prefab);
		// TODO(verify): exported hidden parents can have zero world scale; retain prefab local geometry.
        poolableBehavior.transform.SetParent(transform, worldPositionStays: false);
		poolableBehavior.gameObject.SetActive(value: false);
		poolableBehavior.OwnerPool = this;
		inactiveChildren.Add(poolableBehavior);
	}

	public void ReclaimOldest()
	{ /* spliced-from-2019 */
		if (activeChildren.Count > 0)
		{
			Reclaim(activeChildren[0]);
		}
		else
		{
			InitPool(Prefab);
		}
	}

	public void ReclaimAll()
	{ /* spliced-from-2019 */
		PoolableBehavior[] array = activeChildren.ToArray();
		foreach (PoolableBehavior obj in array)
		{
			Reclaim(obj);
		}
	}

	public override T Get<T>()
	{
		PoolableBehavior poolableBehavior;
		lock (inactiveChildren)
		{
			if (inactiveChildren.Count == 0)
			{
				if (activeChildren.Count == 0)
				{
					InitPool(Prefab);
				}
				else
				{
					CreateOneInactive(Prefab);
				}
			}
			poolableBehavior = inactiveChildren[inactiveChildren.Count - 1];
			inactiveChildren.RemoveAt(inactiveChildren.Count - 1);
			activeChildren.Add(poolableBehavior);
			poolableBehavior.PoolIndex = childIndex++;
			if (childIndex > poolSize)
			{
				childIndex = 0;
			}
		}
		if (DetachOnGet)
		{
			poolableBehavior.transform.SetParent(null, worldPositionStays: false);
		}
		poolableBehavior.gameObject.SetActive(value: true);
		poolableBehavior.Reset();
		return poolableBehavior as T;
	}

	public override void Reclaim(PoolableBehavior obj)
	{
		if (!this)
		{
			DefaultPool.Instance.Reclaim(obj);
			return;
		}
		obj.gameObject.SetActive(value: false);
		obj.transform.SetParent(transform, worldPositionStays: false);
		lock (inactiveChildren)
		{
			if (activeChildren.Remove(obj))
			{
				inactiveChildren.Add(obj);
				return;
			}
			if (inactiveChildren.Contains(obj))
			{
				Debug.Log("ObjectPoolBehavior: :| Something was reclaimed without being gotten");
				return;
			}
			Debug.Log("ObjectPoolBehavior: Destroying this thing I don't own");
			Object.Destroy(obj.gameObject);
		}
	}

	public ObjectPoolBehavior()
	{ }
}
