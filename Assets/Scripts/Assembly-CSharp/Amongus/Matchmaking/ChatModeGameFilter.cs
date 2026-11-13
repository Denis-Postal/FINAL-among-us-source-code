using System;
using Cpp2ILInjected;

namespace AmongUs.Matchmaking
{
	[Serializable]
	public class ChatModeGameFilter : ISubFilter
	{
		public byte AcceptedValues;

		public string FilterType { get; } = "chat";

		public ChatModeGameFilter()
		{ }
	}
}
