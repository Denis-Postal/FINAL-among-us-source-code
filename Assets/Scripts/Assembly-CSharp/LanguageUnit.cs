using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cpp2ILInjected;
using UnityEngine;

public class LanguageUnit
{
	private static StringBuilder builder = new StringBuilder();

	public bool IsEnglish;

	public SupportedLangs languageID;

	private Dictionary<string, string> AllStrings;

	public Dictionary<ImageNames, Sprite> AllImages;

	public LanguageUnit(TranslatedImageSet locSet)
	{
		// TODO(verify): rebuilt from decomp9 shape (noisy) + TSV format in CDN/ReferenceData/Languages
		AllStrings = new Dictionary<string, string>();
		AllImages = new Dictionary<ImageNames, Sprite>();
		if (locSet == null)
		{
			throw new NullReferenceException("LanguageUnit: " + (locSet == null ? "locSet null" : "locSet.Data null for " + locSet.languageID));
		}
		languageID = locSet.languageID;
		IsEnglish = languageID == SupportedLangs.English;
		if (locSet.Data == null)
		{
			throw new NullReferenceException("LanguageUnit: " + (locSet == null ? "locSet null" : "locSet.Data null for " + locSet.languageID));
		}
		ParseTSV(locSet.Data.text, AllStrings);
		if (!IsEnglish)
		{
			List<StringNames> missing = new List<StringNames>();
			foreach (StringNames value in Enum.GetValues(typeof(StringNames)))
			{
				if (!AllStrings.ContainsKey(value.ToString()))
				{
					missing.Add(value);
				}
			}
			if (missing.Count > 0)
			{
				Debug.LogWarning($"No translation found in {languageID} for {missing.Count} StringNames");
			}
		}
	}

	public static bool TryParseTsvLine(string line, out string key, out string text)
	{
		// TODO(verify): rebuilt from decomp9 shape
		key = null;
		text = null;
		if (string.IsNullOrEmpty(line))
		{
			return false;
		}
		int num = line.IndexOf('\t');
		if (num < 0)
		{
			Debug.LogWarning("Couldn't parse: " + line + " because there was no tab");
			return false;
		}
		key = line.Substring(0, num);
		text = UnescapeCodes(line, num + 1);
		return !string.IsNullOrEmpty(key);
	}

	public static void ParseTSV(string tsvText, Dictionary<string, string> allStrings)
	{
		// TODO(verify): rebuilt from decomp9 shape; first (header) line is "KEY ID<tab>Language" and is skipped
		using (System.IO.StringReader reader = new System.IO.StringReader(tsvText))
		{
			bool header = true;
			for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
			{
				if (header)
				{
					header = false;
					if (line.StartsWith("KEY ID"))
					{
						continue;
					}
				}
				if (!TryParseTsvLine(line, out var key, out var text))
				{
					continue;
				}
				string name = key;
				if (!key.StartsWith("Cosmetic.") && !key.StartsWith("Bundle.") && Enum.TryParse<StringNames>(key, out var result))
				{
					name = result.ToString();
				}
				if (!allStrings.ContainsKey(name))
				{
					allStrings.Add(name, text);
				}
				else if (allStrings[name] != text)
				{
					Debug.LogWarning("Duplicate translation for " + name + ": '" + text + "' and '" + allStrings[name] + "'");
				}
			}
		}
	}

	public static string UnescapeCodes(string src, int startAt)
	{
		// TODO(verify): rebuilt from decomp9: handles \n and \t
		builder.Clear();
		for (int i = startAt; i < src.Length; i++)
		{
			char c = src[i];
			if (c == '\\' && i + 1 < src.Length)
			{
				char c2 = src[i + 1];
				if (c2 == 'n')
				{
					builder.Append('\n');
					i++;
					continue;
				}
				if (c2 == 't')
				{
					builder.Append('\t');
					i++;
					continue;
				}
			}
			builder.Append(c);
		}
		return builder.ToString();
	}

	public string GetString(string idString, object[] parts)
	{
		if (AllStrings != null)
		{
			if (!AllStrings.TryGetValue(idString, out var value) || Extensions.IsNullOrWhiteSpace((string)value))
			{
				return string.Empty;
			}
			if (parts == null || parts.Length == 0)
			{
				return value;
			}
			return string.Format(value, parts);
		}
		return string.Empty;
	}

	public Sprite GetImage(ImageNames id)
	{
		if (AllImages != null)
		{
			bool flag = AllImages.TryGetValue(id, out var value);
			return (Sprite)value;
		}
		return (Sprite)(object)new NullReferenceException();
	}

	static LanguageUnit()
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		builder = stringBuilder;
	}
}
