using System;
using Cpp2ILInjected;
using UnityEngine;

public static class StringExtensions
{
	private static char[] buffer= new char[256];

	public unsafe static string Lerp(string a, string b, float t)
	{ /* spliced-from-2019 */
		int num = Mathf.Max(a.Length, b.Length);
		int num2 = (int)Mathf.Lerp(0f, num, t);
		for (int i = 0; i < num; i++)
		{
			if (i < num2)
			{
				if (i < b.Length)
				{
					buffer[i] = b[i];
				}
				else
				{
					buffer[i] = ' ';
				}
			}
			else if (i < a.Length)
			{
				buffer[i] = a[i];
			}
		}
		return new string(buffer, 0, num);
	}

	public unsafe static string Color(this string toColor, Color color)
	{
		return toColor.Color("#" + ColorUtility.ToHtmlStringRGBA(color));
	}

	public static string Color(this string toColor, string color)
	{
		return "<color=" + color + ">" + toColor + "</color>";
	}

	static StringExtensions()
	{ }
}
