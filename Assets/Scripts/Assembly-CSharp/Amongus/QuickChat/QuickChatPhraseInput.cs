using UnityEngine;
using System;

namespace AmongUs.QuickChat
{
	[Serializable]
	public class QuickChatPhraseInput
	{
		[SerializeField]
		public QuickChatTag[] tags;

		public QuickChatTag[] Tags => tags;

		public QuickChatPhraseInput()
		{
		}

		public QuickChatPhraseInput(QuickChatTag[] tags)
		{
			this.tags = tags;
		}
	}
}
