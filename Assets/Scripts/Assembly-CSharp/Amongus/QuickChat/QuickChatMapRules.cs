using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmongUs.QuickChat
{
	[Serializable]
	public class QuickChatMapRules : AbstractQuickChatPhraseRuleGroup
	{
		[SerializeField]
		private MapNames[] maps;

		public QuickChatMapRules(params MapNames[] maps)
		{
			this.maps = maps;
		}

		public override bool Evaluate(QuickChatContext context)
		{
			if (context.Phase == QuickChatContextPhase.Lobby)
			{
				return true;
			}
			return maps.Contains(context.Map);
		}
	}
}
