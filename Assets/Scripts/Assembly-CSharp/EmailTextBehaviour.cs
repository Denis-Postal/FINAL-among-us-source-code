using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cpp2ILInjected;
using UnityEngine;

public class EmailTextBehaviour : MonoBehaviour
{
	public static readonly HashSet<char> SymbolChars= new HashSet<char>
	{
		'?', '!', ',', '\'', ':', ';', '(', ')', '/', '\\',
		'%', '^', '&', '=', '\r', '\n', '[', ']'
	};

	public TextBoxTMP nameSource;

	public string GetEmailValidEmail()
	{
		if (ShakeIfInvalid())
		{
			return "";
		}
		return nameSource.text;
	}

	public string GetEmailWithoutValidityCheck()
	{
		return nameSource.text;
	}

	private bool IsValidEmail(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return false;
		}
		if (!new Regex("^([+\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,63})+)$").Match(text).Success)
		{
			return false;
		}
		foreach (char c in text)
		{
			if (c < ' ')
			{
				return false;
			}
			if (SymbolChars.Contains(c))
			{
				return false;
			}
		}
		return true;
	}

	public bool ShakeIfInvalid()
	{
		if (!IsValidEmail(nameSource.text))
		{
			StartCoroutine(Effects.SwayX(nameSource.transform));
			return true;
		}
		return false;
	}

	public bool ShakeIfDoesntMatch(string email1, string email2)
	{
		if (email1 != email2)
		{
			StartCoroutine(Effects.SwayX(nameSource.transform));
			return true;
		}
		return false;
	}

	static EmailTextBehaviour()
	{ }
}
