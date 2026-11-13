using AmongUs.Data;
using InnerNet;
using TMPro;
using UnityEngine;

public class ChatModeToggle : MonoBehaviour, IChatModeDisplay
{
	public SpriteRenderer quickChatBackgroundSprite;

	public SpriteRenderer freeChatBackgroundSprite;

	public TextMeshPro infoText;

	public TextMeshPro warningText;

	private float warningTimer;

	public void Awake()
	{
		DestroyableSingleton<AccountManager>.Instance.SetChatModeButtonForUpdates(this);
	}

	public void OnEnable()
	{
		UpdateDisplay();
	}

	private void Update()
	{
		if (warningTimer > 0f)
		{
			warningTimer -= Time.deltaTime;
			warningText.alpha = warningTimer;
			if (warningTimer <= 0f)
			{
				warningText.gameObject.SetActive(value: false);
			}
		}
	}

	public void ClickQuickChat()
	{
		DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.QuickChatOnly;
		DataManager.Settings.Save();
		warningText.gameObject.SetActive(value: false);
		UpdateDisplay();
	}

	public void ClickFreeChat()
	{
		if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
		{
			warningText.gameObject.SetActive(value: true);
			warningText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeChatWarning);
			warningTimer = 10f;
		}
		else
		{
			warningText.gameObject.SetActive(value: false);
			DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
			DataManager.Settings.Save();
			UpdateDisplay();
		}
	}

	public void UpdateDisplay()
	{
		switch (DataManager.Settings.Multiplayer.ChatMode)
		{
		case QuickChatModes.QuickChatOnly:
			if (quickChatBackgroundSprite != null)
			{
				quickChatBackgroundSprite.enabled = true;
			}
			if (freeChatBackgroundSprite != null)
			{
				freeChatBackgroundSprite.enabled = false;
			}
			infoText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QCOnlyInfo);
			break;
		case QuickChatModes.FreeChatOrQuickChat:
			if (quickChatBackgroundSprite != null)
			{
				quickChatBackgroundSprite.enabled = false;
			}
			if (freeChatBackgroundSprite != null)
			{
				freeChatBackgroundSprite.enabled = true;
				freeChatBackgroundSprite.color = Color.green;
			}
			infoText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeChatInfo);
			warningText.gameObject.SetActive(value: false);
			break;
		}
		if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed() && freeChatBackgroundSprite != null)
		{
			freeChatBackgroundSprite.enabled = true;
			freeChatBackgroundSprite.color = Color.grey;
		}
	}
}
