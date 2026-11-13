using System;
using System.Runtime.CompilerServices;

public static class HashRandom
{
	private static XXHash src= new XXHash((int)DateTime.UtcNow.Ticks);

	private static int cnt= 0;

	public static uint Next()
	{ /* spliced-from-2019 */
		return src.GetHash(cnt++);
	}

	public static int FastNext(int maxInt)
	{
		return (int)(Next() % maxInt);
	}

	public static int Next(int maxInt)
	{ /* spliced-from-2019 */
		uint num = uint.MaxValue / (uint)maxInt;
		uint num2 = num * (uint)maxInt;
		uint num3;
		do
		{
			num3 = Next();
		}
		while (num3 > num2);
		return (int)(num3 / num);
	}

	public static int Next(int minInt, int maxInt)
	{ /* spliced-from-2019 */
		return Next(maxInt - minInt) + minInt;
	}

	static HashRandom()
	{ }
}
