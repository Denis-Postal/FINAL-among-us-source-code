using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class CounterArea : MonoBehaviour
{
	public SystemTypes RoomType;

	public ObjectPoolBehavior pool;

	private List<PoolableBehavior> myIcons= new List<PoolableBehavior>();

	public float XOffset= 0.3f;

	public float YOffset= 0.3f;

	public int MaxWidth= 5;

	public int MaxColumns= 5;

	public bool DetectiveExclusiveLocation;

	public unsafe void UpdateCount(int cnt)
	{
		bool flag = myIcons.Count != cnt;
		while (myIcons.Count < cnt)
		{
			PoolableBehavior item = pool.Get<PoolableBehavior>();
			myIcons.Add(item);
		}
		while (myIcons.Count > cnt)
		{
			PoolableBehavior poolableBehavior = myIcons[myIcons.Count - 1];
			myIcons.RemoveAt(myIcons.Count - 1);
			poolableBehavior.OwnerPool.Reclaim(poolableBehavior);
		}
		if (flag)
		{
			for (int i = 0; i < myIcons.Count; i++)
			{
				int num = i % MaxColumns;
				int num2 = i / MaxColumns;
				float num3 = (float)(Mathf.Min(cnt - num2 * MaxColumns, MaxColumns) - 1) * XOffset / -2f;
				myIcons[i].transform.position = transform.position + new Vector3(num3 + (float)num * XOffset, (float)num2 * YOffset, -1f);
			}
		}
	}

	public CounterArea()
	{ }
}
