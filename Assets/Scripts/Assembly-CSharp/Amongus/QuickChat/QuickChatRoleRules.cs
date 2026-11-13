using UnityEngine;
using System;
using AmongUs.GameOptions;

namespace AmongUs.QuickChat
{
	[Serializable]
	public class QuickChatRoleRules : AbstractQuickChatPhraseRuleGroup
	{
		[SerializeField]
		private RoleTypes[] requiredRoles;

		public QuickChatRoleRules(params RoleTypes[] roles)
		{
			requiredRoles = roles;
		}

		public unsafe override bool Evaluate(QuickChatContext context)
		{
			RoleTypes[] array = requiredRoles;
			foreach (RoleTypes role in array)
			{
				if (!context.HasPlayersWithRole(role))
				{
					return false;
				}
			}
			return true;
		}
	}
}
