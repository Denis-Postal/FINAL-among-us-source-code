using AmongUs.Data;
using InnerNet;
using TMPro;
using UnityEngine;

public class ChatModeCycle : MonoBehaviour, IChatModeDisplay
{
	public CycleButtonBehaviour chatMode;

	public TextMeshPro chatModeText;

	public SpriteRenderer backgroundSprite;

	public void Awake()
	{
		DestroyableSingleton<AccountManager>.Instance.SetChatModeButtonForUpdates(this);
	}

	public void OnEnable()
	{
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		chatMode.options = new StringNames[2]
		{
			StringNames.FreeOrQuickChat,
			StringNames.QuickChatOnly
		};
		if (!DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
		{
			DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.QuickChatOnly;
			chatMode.Rollover.OverColor = Color.grey;
			chatMode.Rollover.OutColor = Color.grey;
			chatMode.Text.color = Color.grey;
			chatModeText.color = Color.grey;
			if (backgroundSprite != null)
			{
				backgroundSprite.color = Color.grey;
			}
		}
		else
		{
			chatMode.Rollover.OverColor = Color.green;
			chatMode.Rollover.OutColor = Color.white;
			chatMode.Text.color = Color.white;
			chatModeText.color = Color.white;
			if (backgroundSprite != null)
			{
				backgroundSprite.color = Color.white;
			}
		}
		chatMode.UpdateText((int)(DataManager.Settings.Multiplayer.ChatMode - 1));
	}

	public void CycleChatMode()
	{
		if (DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
		{
			int num = (int)(DataManager.Settings.Multiplayer.ChatMode + 1);
			num %= 3;
			if (num == 0)
			{
				num++;
			}
			DataManager.Settings.Multiplayer.ChatMode = (QuickChatModes)num;
			DataManager.Settings.Save();
			chatMode.UpdateText((int)(DataManager.Settings.Multiplayer.ChatMode - 1));
		}
	}
}
