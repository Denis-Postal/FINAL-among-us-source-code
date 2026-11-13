using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class MapDecor : MonoBehaviour
{
	[SerializeField]
	private List<DateHide> AllDecor= new List<DateHide>();

	private unsafe void Awake()
	{
		foreach (DateHide item in AllDecor)
		{
			item.gameObject.SetActive(value: false);
		}
		foreach (DateHide item2 in AllDecor)
		{
			if (!item2.ShouldHide())
			{
				item2.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public MapDecor()
	{ }
}
