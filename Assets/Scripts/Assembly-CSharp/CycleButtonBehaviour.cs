using AmongUs.Data;
using InnerNet;
using TMPro;
using UnityEngine;

public class CycleButtonBehaviour : MonoBehaviour, ITranslatedText
{
	public StringNames[] options;

	public StringNames BaseText;

	public TextMeshPro Text;

	public SpriteRenderer Background;

	public ButtonRolloverHandler Rollover;

	public GameObject helpButton;

	private int curSelection;

	public TextMeshPro quickChatInstructions;

	public void Start()
	{
		DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Add(this);
	}

	public void OnDestroy()
	{
		if (DestroyableSingleton<TranslationController>.InstanceExists)
		{
			DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Remove(this);
		}
	}

	public void ResetText()
	{
		if (helpButton != null)
		{
			helpButton.SetActive(DataManager.Settings.Multiplayer.ChatMode == QuickChatModes.QuickChatOnly);
		}
		Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(options[curSelection]);
	}

	public void UpdateText(int selection)
	{
		curSelection = selection;
		ResetText();
		if (DataManager.Settings.Multiplayer.ChatMode == QuickChatModes.QuickChatOnly)
		{
			quickChatInstructions.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QuickChatInstructionsStart) + " ";
			if (DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn && DestroyableSingleton<EOSManager>.Instance.IsMinorOrWaiting() && !DestroyableSingleton<EOSManager>.Instance.IsFreechatAllowed())
			{
				quickChatInstructions.text += DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QuickChatInstructionsChild);
			}
			TextMeshPro textMeshPro = quickChatInstructions;
			textMeshPro.text = textMeshPro.text + "\n\n" + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.QuickChatInstructionsFull);
		}
	}
}
