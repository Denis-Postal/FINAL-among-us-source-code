using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class NotificationPopper : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public NotificationPopper _003C_003E4__this;

		public LobbyNotificationMessage newMessage;

		internal void _003CAddDisconnectMessage_003Eb__0()
		{
			_003C_003E4__this.OnMessageDestroy(newMessage);
		}
	}

	private sealed class _003C_003Ec__DisplayClass17_0
	{
		public NotificationPopper _003C_003E4__this;

		public LobbyNotificationMessage newMessage;

		internal void _003CSettingsChangeMessageLogic_003Eb__0()
		{
			_003C_003E4__this.OnMessageDestroy(newMessage);
		}
	}

	[SerializeField]
	private LobbyNotificationMessage notificationMessageOrigin;

	[SerializeField]
	private Sprite playerDisconnectSprite;

	[SerializeField]
	private Sprite settingsChangeSprite;

	[SerializeField]
	private AudioClip playerDisconnectSound;

	[SerializeField]
	private AudioClip settingsChangeSound;

	[SerializeField]
	private float spacingY;

	[SerializeField]
	private int maxMessages = 10;

	[SerializeField]
	private Color disconnectColor;

	[SerializeField]
	private Color settingsChangeColor;

	[SerializeField]
	private AspectPosition aspectPosition;

	private List<LobbyNotificationMessage> activeMessages;

	private int lastMessageKey = -1;

	private void Awake()
	{
		activeMessages = new List<LobbyNotificationMessage>();
	}

	private void Update()
	{
	}

	public unsafe void AddDisconnectMessage(string item)
	{
		LobbyNotificationMessage newMessage = Object.Instantiate(notificationMessageOrigin, Vector3.zero, Quaternion.identity, transform);
		newMessage.transform.localPosition = new Vector3(0f, 0f, -2f);
		newMessage.SetUp(item, playerDisconnectSprite, disconnectColor, () =>
		{
			OnMessageDestroy(newMessage);
		});
		SoundManager.Instance.PlaySound(playerDisconnectSound, loop: false);
		lastMessageKey = -1;
		ShiftMessages();
		AddMessageToQueue(newMessage);
	}

	public unsafe void AddSettingsChangeMessage(StringNames key, string value, bool playSound = true, RoleTypes associatedRole = RoleTypes.Crewmate)
	{
		string empty = string.Empty;
		if (associatedRole != RoleTypes.Crewmate)
		{
			string text = (RoleManager.IsImpostorRole(associatedRole) ? Palette.ImpostorRed.ToTextColor() : Palette.CrewmateSettingChangeText.ToTextColor());
			empty = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyChangeSettingNotification, "<sprite name=\"" + associatedRole.ToString() + "Role\"> <font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + text + DestroyableSingleton<TranslationController>.Instance.GetString(key) + "</color></font>", "<font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + value + "</font>");
		}
		else
		{
			empty = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyChangeSettingNotification, "<font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + DestroyableSingleton<TranslationController>.Instance.GetString(key) + "</font>", "<font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + value + "</font>");
		}
		SettingsChangeMessageLogic(key, empty, playSound);
	}

	public unsafe void AddRoleSettingsChangeMessage(StringNames key, int roleCount, int roleChance, RoleTeamTypes teamType, bool playSound = true)
	{
		string empty = string.Empty;
		string text = ((teamType == RoleTeamTypes.Crewmate) ? Palette.CrewmateSettingChangeText.ToTextColor() : Palette.ImpostorRed.ToTextColor());
		empty = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyChangeSettingNotificationRole, "<sprite name=\"" + key.ToString() + "\"> <font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + text + DestroyableSingleton<TranslationController>.Instance.GetString(key) + "</color></font>", "<font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + roleCount + "</font>", "<font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + roleChance + "%");
		SettingsChangeMessageLogic(key, empty, playSound);
	}

	private unsafe void SettingsChangeMessageLogic(StringNames key, string item, bool playSound)
	{
		if (lastMessageKey == (int)key && activeMessages.Count > 0)
		{
			activeMessages[activeMessages.Count - 1].UpdateMessage(item);
		}
		else
		{
			lastMessageKey = (int)key;
			LobbyNotificationMessage newMessage = Object.Instantiate(notificationMessageOrigin, Vector3.zero, Quaternion.identity, transform);
			newMessage.transform.localPosition = new Vector3(0f, 0f, -2f);
			newMessage.SetUp(item, settingsChangeSprite, settingsChangeColor, () =>
			{
				OnMessageDestroy(newMessage);
			});
			ShiftMessages();
			AddMessageToQueue(newMessage);
		}
		if (playSound)
		{
			SoundManager.Instance.PlaySoundImmediate(settingsChangeSound, loop: false);
		}
	}

	private void AddMessageToQueue(LobbyNotificationMessage newMessage)
	{
		while (activeMessages.Count >= maxMessages)
		{
			Object.Destroy(activeMessages[0].gameObject);
			activeMessages.RemoveAt(0);
		}
		activeMessages.Add(newMessage);
	}

	private unsafe void ShiftMessages()
	{
		foreach (LobbyNotificationMessage activeMessage in activeMessages)
		{
			activeMessage.transform.localPosition += new Vector3(0f, spacingY, 0f);
		}
	}

	private void OnMessageDestroy(LobbyNotificationMessage m)
	{
		activeMessages.Remove(m);
	}

	public NotificationPopper()
	{
		//IL_001a: Expected I4, but got I8
	}
}
