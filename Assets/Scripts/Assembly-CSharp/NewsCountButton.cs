using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class NewsCountButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro countText;

	[SerializeField]
	private GameObject notifIcon;

	private void Start()
	{
		UpdateCountText();
		DataManager.Player.Announcements.OnReadAnnouncement += UpdateCountText;
	}

	private void OnDestroy()
	{
		DataManager.Player.Announcements.OnReadAnnouncement -= UpdateCountText;
	}

	public void UpdateCountText()
	{
		int num = DataManager.Player.Announcements.AllAnnouncements.Count - DataManager.Player.Announcements.AnnouncementsRead.Count;
		notifIcon.SetActive(num > 0);
		if (num > 9)
		{
			countText.text = "9+";
		}
		else
		{
			countText.text = num.ToString();
		}
	}
}
