using System;
using AmongUs;
using Cpp2ILInjected;

public static class AmongUsDateTime
{
	private static IAmongUsDateTimeRetriever defaultRetriever= new AmongUsDefaultDateTime();

	private static IAmongUsDateTimeRetriever overrideRetriever;

	public static IAmongUsDateTimeRetriever Retriever
	{
		get
		{
			if (overrideRetriever != null)
			{
				return overrideRetriever;
			}
			return defaultRetriever;
		}
	}

	public static DateTime Now => Retriever.Now();

	public static DateTime UtcNow => Retriever.UtcNow();

	static AmongUsDateTime()
	{ }
}
