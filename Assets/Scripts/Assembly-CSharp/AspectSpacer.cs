using System;
using System.Collections;
using Cpp2ILInjected;
using UnityEngine;

public class AspectSpacer : MonoBehaviour
{
	public float xSpacing;

	public float defaultAspectRatio = 1.7777778f;

	public bool spaceWiderAspectRatios;

	public unsafe void OnEnable()
	{
		Camera main = Camera.main;
		if (main.aspect > defaultAspectRatio && !spaceWiderAspectRatios)
		{
			return;
		}
		float num = xSpacing * (main.aspect / defaultAspectRatio);
		float num2 = Mathf.Ceil((float)(-base.transform.childCount) / 2f) * num;
		foreach (Transform item in base.transform)
		{
			item.localPosition = new Vector3(num2 + (float)item.GetSiblingIndex() * num, item.localPosition.y, item.localPosition.z);
		}
	}
}
