using System;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class MapGameFilter : ISubFilter
	{
		public byte AcceptedValues;

		public string FilterType { get; } = "map";

		public MapGameFilter()
		{ }
	}
}
