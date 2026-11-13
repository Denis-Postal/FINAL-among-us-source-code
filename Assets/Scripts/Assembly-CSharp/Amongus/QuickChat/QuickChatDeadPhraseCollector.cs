using System.Collections.Generic;
using Cpp2ILInjected;

namespace AmongUs.QuickChat
{
	public class QuickChatDeadPhraseCollector : IQuickChatPhraseCollector
	{
		public void Collect(QuickChatContext context, List<QuickChatPhrase> output)
		{
			PlayerControl[] deadPlayers = context.DeadPlayers;
			foreach (PlayerControl playerControl in deadPlayers)
			{
				output.Add(QuickChatPhrase.NewPlayerId(playerControl.PlayerId));
			}
		}
	}
}
