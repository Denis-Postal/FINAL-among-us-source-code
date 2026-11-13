using System;
using System.Collections.Generic;
using Cpp2ILInjected;

public static class MemSafeStringExtensions
{
	public static void SafeSplit(SubString subString, List<SubString> output, char delim)
	{
		int length = default;
		SafeSplit(subString.Source, output, delim, subString.Start, length);
	}

	public static void SafeSplit(string source, List<SubString> output, char delim)
	{
		int length = default;
		SafeSplit(source, output, delim, 0, length);
	}

	public unsafe static void SafeSplit(string source, List<SubString> output, char delim, int start, int length)
	{ /* spliced-from-2019 */
		output.Clear();
		int num = start;
		int num2 = start + length;
		for (int i = start; i < num2; i++)
		{
			if (source[i] == delim)
			{
				if (num != i)
				{
					output.Add(new SubString(source, num, i - num));
				}
				num = i + 1;
			}
		}
		if (num != num2)
		{
			output.Add(new SubString(source, num, num2 - num));
		}
	}
}
