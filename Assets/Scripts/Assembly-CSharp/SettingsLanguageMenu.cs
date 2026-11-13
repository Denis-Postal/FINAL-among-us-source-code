using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class SettingsLanguageMenu : MonoBehaviour
{
	public TextMeshPro selectedLangText;

	public void Awake()
	{
		selectedLangText.text = DestroyableSingleton<TranslationController>.Instance.Languages[DataManager.Settings.Language.CurrentLanguage].Name;
	}
}
