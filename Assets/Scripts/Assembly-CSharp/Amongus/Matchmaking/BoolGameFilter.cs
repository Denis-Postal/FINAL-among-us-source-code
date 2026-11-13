using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class BoolGameFilter : ISubFilter
	{
		public List<bool> AcceptedValues;

		public BoolOptionNames OptionEnum;

		public string FilterType { get; } = "bool";

		public BoolGameFilter()
		{ }
	}
}
