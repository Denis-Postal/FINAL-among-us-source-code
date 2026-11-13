using System;
using AmongUs.Data;
using AmongUs.Data.Settings;
using UnityEngine;
using UnityEngine.Events;

public class CrossplayChangedEvent : MonoBehaviour
{
	public bool onlyOnPlayStation = true;

	public UnityEvent OnCrossplayChanged;

	private void Awake()
	{
		if (!onlyOnPlayStation)
		{
			DataManager.Settings.Multiplayer.OnCrossPlayChanged += InvokeSelfCrossplayChanged;
		}
	}

	private void OnDestroy()
	{
		if (!onlyOnPlayStation)
		{
			DataManager.Settings.Multiplayer.OnCrossPlayChanged -= InvokeSelfCrossplayChanged;
		}
	}

	private void InvokeSelfCrossplayChanged()
	{
		OnCrossplayChanged.Invoke();
	}
}
