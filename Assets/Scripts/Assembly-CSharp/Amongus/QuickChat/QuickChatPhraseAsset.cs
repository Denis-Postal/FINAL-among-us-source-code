using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatPhraseAsset : ScriptableObject
	{
		[SerializeField]
		private string text;

		[SerializeField]
		private StringNames key;

		[SerializeField]
		private Sprite icon;

		[SerializeField]
		private int order;

		[SerializeField]
		private bool isInput;

		[SerializeField]
		private bool isNegative;

		[SerializeField]
		public QuickChatTag[] tags;

		[SerializeField]
		private QuickChatPhraseInput[] inputs;

		[SerializeField]
		private QuickChatClassicPhraseRules classicRules;

		[SerializeField]
		private QuickChatHideAndSeekPhraseRules hideAndSeekRules;

		private QuickChatPhrase cachedPhrase;

		public string Text => text;

		public StringNames Key => key;

		public Sprite Icon => icon;

		public int Order => order;

		public bool IsInput => isInput;

		public bool IsNegative => isNegative;

		public QuickChatTag[] Tags => tags;

		public QuickChatPhraseInput[] Inputs => inputs;

		public bool EvaluateContext(QuickChatContext context)
		{
			switch (context.GameMode)
			{
			default:
				QuickChatMenu.Logger.Warning(string.Format("[{0}] {1}() :: {2}.{3} has not been configured. Did you forget to implement exclusion rules for this game mode? Defaulting to FALSE.", "QuickChatPhraseAsset", "EvaluateContext", "GameModes", context.GameMode));
				return false;
			case GameModes.Normal:
			case GameModes.NormalFools:
				return classicRules.Evaluate(context);
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				return hideAndSeekRules.Evaluate(context);
			}
		}

		public QuickChatPhrase ToPhrase()
		{
			if (cachedPhrase == null)
			{
				if (inputs == null || inputs.Length < 1)
				{
					cachedPhrase = QuickChatPhrase.NewSimplePhrase(key, isInput, isNegative);
				}
				else
				{
					QuickChatPhraseInput[] array = new QuickChatPhraseInput[inputs.Length];
					for (int i = 0; i < inputs.Length; i++)
					{
						array[i] = inputs[i];
					}
					cachedPhrase = QuickChatPhrase.NewComplexPhrase(key, array, isNegative);
				}
			}
			return cachedPhrase;
		}
	}
}
