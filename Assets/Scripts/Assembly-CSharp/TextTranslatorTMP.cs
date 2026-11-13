using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextTranslatorTMP : MonoBehaviour, ITranslatedText
{
	public StringNames TargetText;

	public string defaultStr;

	public bool ToUpper;

	public bool ResetOnlyWhenNoDefault;

	public UnityEvent OnTranslate;

	public unsafe void ResetText()
	{
		// TODO(verify): reimplemented (decomp stub)
		if (ResetOnlyWhenNoDefault && !(string.IsNullOrEmpty(defaultStr))) return;
		TextMeshPro tmp = GetComponent<TextMeshPro>();
		string text = defaultStr;
		if (TargetText != StringNames.NoTranslation || string.IsNullOrEmpty(defaultStr))
		{
			text = DestroyableSingleton<TranslationController>.Instance.GetString(TargetText, System.Array.Empty<object>());
		}
		if (ToUpper) text = text.ToUpperInvariant();
		if (tmp == null)
		{
			TextMeshProUGUI ugui = GetComponent<TextMeshProUGUI>();
			if (ugui != null) { ugui.text = text; ugui.ForceMeshUpdate(); }
		}
		else
		{
			tmp.text = text;
			tmp.ForceMeshUpdate();
		}
		if (OnTranslate != null) OnTranslate.Invoke();
	}

	public void Start()
	{
		// TODO(verify): reimplemented (decomp stub)
		DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Add(this);
		ResetText();
	}

	public void OnDestroy()
	{
		if (DestroyableSingleton<TranslationController>.InstanceExists)
		{
			TranslationController instance = DestroyableSingleton<TranslationController>.Instance;
			bool flag = instance.ActiveTexts.Remove(this);
		}
	}
}
