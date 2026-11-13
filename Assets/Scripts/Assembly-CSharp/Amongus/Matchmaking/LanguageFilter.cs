using System;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class LanguageFilter : ISubFilter
	{
		public uint AcceptedValues;

		public string FilterType { get; } = "languages";

		public LanguageFilter()
		{ }
	}
}
