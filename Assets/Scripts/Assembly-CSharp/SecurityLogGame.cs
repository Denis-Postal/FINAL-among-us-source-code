using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class SecurityLogGame : Minigame
{
	public float EntryHeight = 0.4f;

	public ObjectPoolBehavior EntryPool;

	public Sprite[] LocationBgs;

	public TextMeshPro SabText;

	public float ScreenHeight = 4f;

	public Scroller scroller;

	private SecurityLogBehaviour Logger;

	public void Update()
	{
		if (!PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			if (SabText.isActiveAndEnabled)
			{
				SabText.gameObject.SetActive(value: false);
				RefreshScreen();
			}
			else if (Logger.HasNew)
			{
				Logger.HasNew = false;
				RefreshScreen();
			}
		}
		else if (!SabText.isActiveAndEnabled)
		{
			EntryPool.ReclaimAll();
			SabText.gameObject.SetActive(value: true);
		}
	}

	private void Awake()
	{
		SetupInput(disableCursor: false);
		Logger = ShipStatus.Instance.GetComponent<SecurityLogBehaviour>();
		if (!PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer))
		{
			RefreshScreen();
		}
		else
		{
			SabText.gameObject.SetActive(value: true);
		}
	}

	// NOTE: depends on LogEntryBubble.SetMaskLayer/DecorateLog (other group).
	private unsafe void RefreshScreen()
	{
		EntryPool.ReclaimAll();
		for (int i = 0; i < Logger.LogEntries.Count; i++)
		{
			SecurityLogBehaviour.SecurityLogEntry entry = Logger.LogEntries[i];
			LogEntryBubble logEntryBubble = EntryPool.Get<LogEntryBubble>();
			logEntryBubble.SetMaskLayer(50 + i);
			logEntryBubble.DecorateLog(entry, LocationBgs[(byte)entry.Location]);
			logEntryBubble.transform.localPosition = new Vector3(0f, (float)i * (0f - EntryHeight), 0f);
		}
		float max = Mathf.Max(0f, (float)Logger.LogEntries.Count * EntryHeight - ScreenHeight);
		float scrollPercY = scroller.GetScrollPercY();
		scroller.SetBounds(new FloatRange(0f, max), null);
		if (scrollPercY > 0.95f)
		{
			scroller.ScrollPercentY(1f);
		}
	}
}
