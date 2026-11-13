using System;
using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Player;
using UnityEngine;

public class NameTextBehaviour : MonoBehaviour
{
	public static readonly HashSet<char> SymbolChars= new HashSet<char>
	{
		'?', '!', ',', '.', '\'', ':', ';', '(', ')', '/',
		'\\', '%', '^', '&', '-', '=', '\r', '\n', '[', ']'
	};

	public static NameTextBehaviour Instance;

	public TextBoxTMP nameSource;

	public void Start()
	{
		Instance = this;
		if (nameSource != null)
		{
			nameSource.characterLimit = -1;
			nameSource.allowAllCharacters = true;
			nameSource.AllowSymbols = true;
		}
		if (!IsValidName(DataManager.Player.Customization.Name))
		{
			nameSource.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.EnterName));
		}
		else
		{
			nameSource.SetText(DataManager.Player.Customization.Name);
		}
	}

	public void UpdateName()
	{
		if (!ShakeIfInvalid())
		{
			string text = nameSource.text.Trim(' ');
			DataManager.Player.Customization.Name = text;
			DataManager.Player.Save();
		}
	}

	public static bool IsValidName(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return false;
		}
		if (text.Equals(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.EnterName), StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		foreach (char c in text)
		{
			if (c < ' ')
			{
				return false;
			}
		}
		return true;
	}

	public bool ShakeIfInvalid()
	{ /* spliced-from-2019 */
		if (!IsValidName(nameSource.text))
		{
			StartCoroutine(Effects.SwayX(nameSource.transform));
			return true;
		}
		return false;
	}

	static NameTextBehaviour()
	{ }
}
