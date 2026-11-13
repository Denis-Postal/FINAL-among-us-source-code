using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class IntGameFilter : ISubFilter
	{
		public List<int> AcceptedValues;

		public Int32OptionNames OptionEnum;

		public string FilterType { get; } = "int";

		public IntGameFilter()
		{ }
	}
}
