using System;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class ModFilter : ISubFilter
	{
		public Guid AcceptedValues;

		public string FilterType { get; } = "mod";

		public ModFilter()
		{ }
	}
}
