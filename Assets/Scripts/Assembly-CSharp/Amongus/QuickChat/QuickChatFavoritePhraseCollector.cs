using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;

namespace AmongUs.QuickChat
{
	public class QuickChatFavoritePhraseCollector : IQuickChatPhraseCollector
	{
		private sealed class _003C_003Ec__DisplayClass0_0
		{
			public QuickChatContext context;

			internal bool _003CCollect_003Eb__0(QuickChatTag x)
			{
				return x != context.data.tags.favorite;
			}
		}

		public void Collect(QuickChatContext context, List<QuickChatPhrase> output)
		{
			QuickChatPhrase[] phrases = context.Data.GetPhrases(context, context.Data.Tags.All.Where((QuickChatTag x) => x != context.Data.Tags.Favorite).ToArray());
			foreach (QuickChatPhrase quickChatPhrase in phrases)
			{
				if (DataManager.Settings.Multiplayer.HasQuickChatFavorite(quickChatPhrase.PhraseKey) && !output.Contains(quickChatPhrase))
				{
					output.Add(quickChatPhrase);
				}
			}
		}
	}
}
