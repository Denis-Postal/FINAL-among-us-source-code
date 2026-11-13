using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendListNotification : MonoBehaviour
{
	public enum IconType
	{
		Friend,
		Block,
		LobbyInvite,
		Failed,
		Sent
	}

	[Serializable]
	public class FriendNotificationIcon
	{
		public IconType type;

		public Sprite icon;

		public float scale;

		public Vector2 offset;
	}

	public List<FriendNotificationIcon> IconTypes;

	public SpriteRenderer Icon;

	public TextMeshPro Text;

	private float timeOnScreen;

	public void SetUp(string text, IconType type)
	{
		if (!ShipStatus.Instance)
		{
			timeOnScreen = 5f;
			gameObject.SetActive(value: true);
			Text.text = text;
			FriendNotificationIcon friendNotificationIcon = IconTypes.Find((FriendNotificationIcon i) => i.type == type);
			Icon.sprite = friendNotificationIcon.icon;
			Icon.transform.localScale = Vector2.one * friendNotificationIcon.scale;
			Icon.transform.localPosition = new Vector3(friendNotificationIcon.offset.x, friendNotificationIcon.offset.y, -1f);
		}
	}

	private void Update()
	{
		timeOnScreen -= Time.deltaTime;
		if (timeOnScreen <= 0f)
		{
			gameObject.SetActive(value: false);
		}
	}
}
