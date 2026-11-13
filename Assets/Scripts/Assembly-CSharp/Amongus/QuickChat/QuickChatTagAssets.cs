using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatTagAssets : ScriptableObject
	{
		[SerializeField]
		private QuickChatTag accusation;

		[SerializeField]
		public QuickChatTag crew;

		[SerializeField]
		public QuickChatTag deadCrew;

		[SerializeField]
		private QuickChatTag defense;

		[SerializeField]
		public QuickChatTag favorite;

		[SerializeField]
		public QuickChatTag location;

		[SerializeField]
		private QuickChatTag question;

		[SerializeField]
		public QuickChatTag remark;

		[SerializeField]
		public QuickChatTag role;

		[SerializeField]
		private QuickChatTag self;

		[SerializeField]
		public QuickChatTag system;

		[SerializeField]
		public QuickChatTag task;

		[SerializeField]
		private QuickChatTag lobbyImpostor;

		[SerializeField]
		private QuickChatTag lobbyMeeting;

		[SerializeField]
		private QuickChatTag lobbyHiding;

		[SerializeField]
		private QuickChatTag lobbyFlashlight;

		[SerializeField]
		public QuickChatTag[] visibleInClassicLobby;

		[SerializeField]
		public QuickChatTag[] visibleInHideAndSeekLobby;

		[SerializeField]
		public QuickChatTag[] visibleInGame;

		[SerializeField]
		public QuickChatTag[] all;

		public QuickChatTag Accusation => accusation;

		public QuickChatTag Crew => crew;

		public QuickChatTag DeadCrew => deadCrew;

		public QuickChatTag Defense => defense;

		public QuickChatTag Favorite => favorite;

		public QuickChatTag Location => location;

		public QuickChatTag Question => question;

		public QuickChatTag Remark => remark;

		public QuickChatTag Role => role;

		public QuickChatTag Self => self;

		public QuickChatTag System => system;

		public QuickChatTag Task => task;

		public QuickChatTag LobbyImpostor => lobbyImpostor;

		public QuickChatTag LobbyMeeting => lobbyMeeting;

		public QuickChatTag LobbyHiding => lobbyHiding;

		public QuickChatTag LobbyFlashlight => lobbyFlashlight;

		public QuickChatTag[] VisibleInClassicLobby => visibleInClassicLobby;

		public QuickChatTag[] VisibleInHideAndSeekLobby => visibleInHideAndSeekLobby;

		public QuickChatTag[] VisibleInGame => visibleInGame;

		public QuickChatTag[] All => all;

		public QuickChatTag GetTagByName(string name)
		{
			QuickChatTag[] array = All;
			foreach (QuickChatTag quickChatTag in array)
			{
				string text = quickChatTag.name.Replace("QCTag_", "");
				if (name == text)
				{
					return quickChatTag;
				}
			}
			return null;
		}

		public QuickChatTag[] GetTagsByParsingKeys(params string[] parsingKeys)
		{
			if (parsingKeys == null || parsingKeys.Length < 1)
			{
				return null;
			}
			List<QuickChatTag> list = new List<QuickChatTag>();
			QuickChatTag[] array = All;
			foreach (QuickChatTag quickChatTag in array)
			{
				foreach (string key in parsingKeys)
				{
					if (quickChatTag.ContainsParsingKey(key) && !list.Contains(quickChatTag))
					{
						list.Add(quickChatTag);
					}
				}
			}
			return list.ToArray();
		}
	}
}
