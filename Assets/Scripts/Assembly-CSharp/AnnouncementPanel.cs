using System;
using System.Collections.Generic;
using AmongUs.Data;
using Assets.InnerNet;
using TMPro;
using UnityEngine;

public class AnnouncementPanel : MonoBehaviour
{
	public PassiveButton PassiveButton;

	[SerializeField]
	private TextMeshPro TitleText;

	[SerializeField]
	private TextMeshPro DateText;

	[SerializeField]
	private SpriteRenderer Background;

	[SerializeField]
	public SpriteRenderer RightBorder;

	[SerializeField]
	private SpriteRenderer NewIcon;

	private Announcement announcement;

	private Color32 ReadColor = new Color32(62, 79, 79, 122);

	private Color32 SelectedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 143);

	public int AnnouncementNumber => announcement.Number;

	public void SetUp(Announcement announcement)
	{
		this.announcement = announcement;
		Background.enabled = false;
		TitleText.text = announcement.ShortTitle;
		DateText.text = DateTime.Parse(announcement.Date).ToLocalTime().ToString(DestroyableSingleton<TranslationController>.Instance.dateFormats[DataManager.Settings.Language.CurrentLanguage]);
	}

	public void Select()
	{
		List<int> list = new List<int>(DataManager.Player.Announcements.AnnouncementsRead);
		if (!list.Contains(announcement.Number))
		{
			list.Add(announcement.Number);
		}
		DataManager.Player.Announcements.AnnouncementsRead = list;
		RightBorder.enabled = false;
		Background.enabled = true;
		Background.color = SelectedColor;
		NewIcon.enabled = false;
	}

	public void UnSelect()
	{
		RightBorder.enabled = true;
		MarkAsRead();
	}

	public void MarkAsRead()
	{
		NewIcon.enabled = false;
		Background.color = ReadColor;
		Background.enabled = true;
	}
}
