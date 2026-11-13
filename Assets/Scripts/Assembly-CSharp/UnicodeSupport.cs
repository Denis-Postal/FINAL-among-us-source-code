using System;
using System.Text;
using Cpp2ILInjected;
using TMPro;

public static class UnicodeSupport
{
	private static StringBuilder sb= new StringBuilder();

	public static string FilterUnsupportedCharacters(TextMeshPro tr, FontData data)
	{
		sb.Clear();
		for (int i = 0; i < tr.text.Length; i++)
		{
			if (tr.text[i] > ' ' && !data.charMap.ContainsKey(tr.text[i]))
			{
				sb.Append('□');
			}
			else
			{
				sb.Append(tr.text[i]);
			}
		}
		return sb.ToString();
	}

	static UnicodeSupport()
	{ }
}
