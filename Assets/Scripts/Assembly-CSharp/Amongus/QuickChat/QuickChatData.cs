using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Settings;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatData : ScriptableObject
	{
		[Serializable]
		private sealed class _003C_003Ec
		{
			public static readonly _003C_003Ec _003C_003E9;

			public static Func<QuickChatPhrase, int> _003C_003E9__8_0;

			static _003C_003Ec()
			{
				_003C_003Ec obj = new _003C_003Ec();
				_003C_003E9 = obj;
			}

			internal int _003CGetPhrases_003Eb__8_0(QuickChatPhrase x)
			{
				return (x.type == QuickChatPhraseType.PlayerId) ? 1 : (DataManager.Settings.multiplayer.HasQuickChatFavorite(x.phraseKey) ? 0 : 1);
			}
		}

		[SerializeField]
		public QuickChatTagAssets tags;

		[SerializeField]
		private QuickChatPhraseAssets phrases;

		private Dictionary<QuickChatTag, IQuickChatPhraseCollector> phraseCollectors;

		public QuickChatTagAssets Tags => tags;

		public QuickChatPhraseAssets Phrases => phrases;

		public QuickChatTag[] GetVisibleTags()
		{
			List<QuickChatTag> list = new List<QuickChatTag>();
			QuickChatContext quickChatContext = QuickChatContext.Current(this);
			QuickChatTag[] array = null;
			if (quickChatContext.Phase != QuickChatContextPhase.Game)
			{
				array = ((quickChatContext.GameMode == GameModes.Normal) ? Tags.VisibleInClassicLobby : Tags.VisibleInHideAndSeekLobby);
			}
			else
			{
				array = Tags.VisibleInGame;
			}
			QuickChatTag[] array2 = array;
			foreach (QuickChatTag quickChatTag in array2)
			{
				if (GetPhrases(quickChatContext, quickChatTag).Length != 0)
				{
					list.Add(quickChatTag);
				}
			}
			return list.ToArray();
		}

		public QuickChatPhrase[] GetPhrases(QuickChatContext context, params QuickChatTag[] tags)
		{
			List<QuickChatPhrase> list = new List<QuickChatPhrase>();
			foreach (QuickChatTag tag in tags)
			{
				CollectPhrasesFromTag(tag, context, list);
			}
			return list.OrderBy((QuickChatPhrase x) => (x.Type == QuickChatPhraseType.PlayerId || !DataManager.Settings.Multiplayer.HasQuickChatFavorite(x.PhraseKey)) ? 1 : 0).ToArray();
		}

		private void RegisterCollectorMethods()
		{
			if (phraseCollectors == null)
			{
				phraseCollectors = new Dictionary<QuickChatTag, IQuickChatPhraseCollector>
				{
					{
						tags.Crew,
						new QuickChatCrewPhraseCollector()
					},
					{
						tags.DeadCrew,
						new QuickChatDeadPhraseCollector()
					},
					{
						tags.Favorite,
						new QuickChatFavoritePhraseCollector()
					},
					{
						tags.Location,
						new QuickChatLocationPhraseCollector()
					},
					{
						tags.Role,
						new QuickChatRolePhraseCollector()
					},
					{
						tags.System,
						new QuickChatSystemPhraseCollector()
					},
					{
						tags.Task,
						new QuickChatTaskPhraseCollector()
					}
				};
			}
		}

		private void CollectPhrasesFromTag(QuickChatTag tag, QuickChatContext context, List<QuickChatPhrase> output)
		{
			Phrases.GetAssetPhrases(tag, context, output);
			RegisterCollectorMethods();
			if (phraseCollectors.ContainsKey(tag))
			{
				phraseCollectors[tag].Collect(context, output);
			}
		}
	}
}
