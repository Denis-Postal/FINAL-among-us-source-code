using System;
using System.Collections.Generic;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class CategorizedGameFilter : ISubFilter
	{
		public List<int> AcceptedValues;

		public CategorizedOptionNames OptionEnum;

		public string FilterType { get; } = "cat";

		public CategorizedGameFilter()
		{ }
	}
}
