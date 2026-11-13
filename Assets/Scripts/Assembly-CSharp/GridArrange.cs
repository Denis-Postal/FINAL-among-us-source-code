using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using UnityEngine;

public class GridArrange : MonoBehaviour
{
	public enum StartAlign
	{
		Left,
		Right
	}

	public Vector2 CellSize;

	public StartAlign Alignment;

	public int MaxColumns = 3;

	private List<Transform> cells;

	private static List<Transform> currentChildren= new List<Transform>();

	private void Start()
	{
		cells = new List<Transform>();
		GetChildsActive();
		CheckCurrentChildren();
	}

	private void FixedUpdate()
	{
		CheckCurrentChildren();
	}

	private unsafe void CheckCurrentChildren()
	
	{
		// TODO(verify): from decomp9
		GetChildsActive();
		if (cells.SequenceEqual(currentChildren))
		{
			return;
		}
		cells.Clear();
		foreach (Transform currentChild in currentChildren)
		{
			cells.Add(currentChild);
		}
		ArrangeChilds();
	}

	private unsafe void GetChildsActive()
	
	{
		currentChildren.Clear();
		foreach (Transform item in base.transform)
		{
			if (item.gameObject.activeSelf)
			{
				currentChildren.Add(item);
			}
		}
	}

	private unsafe void ArrangeChilds()
	{
		float num = ((Alignment == StartAlign.Left) ? (0f - CellSize.x) : CellSize.x);
		for (int i = 0; i < cells.Count; i++)
		{
			float num2 = (float)(i % MaxColumns) * num;
			num2 += base.transform.position.x;
			float num3 = Mathf.Floor(i / MaxColumns) * CellSize.y;
			num3 += base.transform.position.y;
			Transform transform = cells[i];
			transform.position = new Vector3(num2, num3, transform.position.z);
		}
	}

	static GridArrange()
	{ }
}
