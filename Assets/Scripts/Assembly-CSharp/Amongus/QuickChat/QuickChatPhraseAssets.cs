using System;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatPhraseAssets : ScriptableObject
	{
		[SerializeField]
		public QuickChatPhraseAsset[] phraseAssets;

		public void GetAssetPhrases(QuickChatTag tag, QuickChatContext context, List<QuickChatPhrase> output)
		{
			QuickChatPhraseAsset[] array = phraseAssets;
			foreach (QuickChatPhraseAsset quickChatPhraseAsset in array)
			{
				if (quickChatPhraseAsset.Tags != null && quickChatPhraseAsset.Tags.Length >= 1 && quickChatPhraseAsset.Tags.Contains(tag) && quickChatPhraseAsset.EvaluateContext(context))
				{
					output.Add(quickChatPhraseAsset.ToPhrase());
				}
			}
		}

		public QuickChatPhraseAsset GetAssetByName(string name)
		{
			QuickChatPhraseAsset[] array = phraseAssets;
			foreach (QuickChatPhraseAsset quickChatPhraseAsset in array)
			{
				if (quickChatPhraseAsset.name == name)
				{
					return quickChatPhraseAsset;
				}
			}
			return null;
		}
	}
}
