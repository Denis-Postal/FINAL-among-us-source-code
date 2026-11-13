using System;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class PlatformGameFilter : ISubFilter
	{
		public uint AcceptedValues;

		public string FilterType { get; } = "platform";

		public PlatformGameFilter()
		{ }
	}
}
