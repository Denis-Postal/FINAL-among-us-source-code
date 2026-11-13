using System;
using UnityEngine;

public class UIScrollbarHelper : MonoBehaviour
{
	public int index;

	public bool isDirectChild;

	public unsafe Vector2 GetLocalPosition(Scroller scrollRect)
	{
		if (isDirectChild)
		{
			return transform.localPosition;
		}
		Vector3 position = transform.position;
		return scrollRect.Inner.transform.InverseTransformPoint(position);
	}
}
