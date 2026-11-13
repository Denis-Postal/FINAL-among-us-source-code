using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Hazel;
using InnerNet;
using UnityEngine;

public class SocialMediumFeedSystem : MonoBehaviour
{
	private const float MIN_SECONDS_BEFORE_IMAGE_SWITCH = 3f;

	private const float MESSAGE_LIFETIME_SECONDS = 5f;

	[SerializeField]
	private GameObject spiritGuideMessagePrefab;

	[SerializeField]
	public List<Sprite> possibleSprites= new List<Sprite>();

	private readonly Queue<SpiritGuideReceivedMessage> messageFeed= new Queue<SpiritGuideReceivedMessage>();

	private readonly List<SpiritGuideReceivedMessage> activeMessages= new List<SpiritGuideReceivedMessage>();

	private float secondsSinceLastImage;

	public bool isCommsSabotageActive;

	public List<Sprite> PossibleSpriteList => possibleSprites;

	private bool CommsSabotaged
	{
		get
		{
			if (PlayerControl.LocalPlayer != null)
			{
				return PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer);
			}
			return false;
		}
	}

	public void FixedUpdate()
	{
		if (MeetingHud.Instance != null)
		{
			return;
		}
		if (isCommsSabotageActive)
		{
			if (CommsSabotaged)
			{
				return;
			}
			isCommsSabotageActive = false;
		}
		if (CommsSabotaged)
		{
			secondsSinceLastImage = 0f;
			isCommsSabotageActive = true;
			ClearAllMessages();
			return;
		}
		for (int num = activeMessages.Count - 1; num >= 0; num--)
		{
			SpiritGuideReceivedMessage spiritGuideReceivedMessage = activeMessages[num];
			if (spiritGuideReceivedMessage.TimeElapsedSeconds > 5f)
			{
				activeMessages.RemoveAt(num);
				Object.Destroy(spiritGuideReceivedMessage.gameObject);
			}
		}
		secondsSinceLastImage += Time.deltaTime;
		if (messageFeed.Count > 0 && (activeMessages.Count == 0 || secondsSinceLastImage > 3f))
		{
			SpiritGuideReceivedMessage spiritGuideReceivedMessage2 = messageFeed.Dequeue();
			spiritGuideReceivedMessage2.Run();
			activeMessages.Add(spiritGuideReceivedMessage2);
			secondsSinceLastImage = 0f;
		}
	}

	public void SendSpiritGuideMessage(NetId shipStatusNetId, byte[] spriteIndices, ClientId targetId)
	{
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(shipStatusNetId, 67, SendOption.Reliable, targetId);
		messageWriter.WriteBytesAndSize(spriteIndices);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void HandleSpiritGuideMessage(MessageReader reader)
	{
		if (!isCommsSabotageActive && !(MeetingHud.Instance != null))
		{
			AddImageToFeed(reader.ReadBytesAndSize());
		}
	}

	public void OnMeetingStart()
	{
		ClearAllMessages();
	}

	public unsafe void AddImageToFeed(byte[] spriteIndices)
	{
		Transform socialMediumFeedTransform = PlayerControl.LocalPlayer.SocialMediumFeedTransform;
		GameObject gameObject = Object.Instantiate(spiritGuideMessagePrefab, socialMediumFeedTransform);
		gameObject.transform.localPosition = Vector3.zero;
		SpiritGuideReceivedMessage component = gameObject.GetComponent<SpiritGuideReceivedMessage>();
		List<Sprite> list = new List<Sprite>();
		int num = possibleSprites.Count - 1;
		foreach (int num2 in spriteIndices)
		{
			if (num2 > num)
			{
				Debug.LogWarning($"Received invalid sprite index {num2} for a spirit guide message.");
			}
			else
			{
				list.Add(possibleSprites[num2]);
			}
		}
		component.SetImages(list);
		messageFeed.Enqueue(component);
	}

	public void ClearAllMessages()
	{
		messageFeed.Clear();
		foreach (SpiritGuideReceivedMessage activeMessage in activeMessages)
		{
			Object.Destroy(activeMessage.gameObject);
		}
		activeMessages.Clear();
	}

	public SocialMediumFeedSystem()
	{ }
}
