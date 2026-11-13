using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class ToggleButtonBehaviour : MonoBehaviour, ITranslatedText
{
	public StringNames BaseText;

	public TextMeshPro Text;

	public SpriteRenderer Background;

	public ButtonRolloverHandler Rollover;

	private bool onState;

	public void Start()
	{ /* spliced-from-2019 */
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
		Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(BaseText) + ": " + DestroyableSingleton<TranslationController>.Instance.GetString(onState ? StringNames.SettingsOn : StringNames.SettingsOff);
	}

	public unsafe void UpdateText(bool on)
	{
		onState = on;
		Color color = (on ? new Color(0f, 1f, 42f / 255f, 1f) : Color.white);
		Background.color = color;
		ResetText();
		if ((bool)Rollover)
		{
			Rollover.ChangeOutColor(color);
		}
	}
}
