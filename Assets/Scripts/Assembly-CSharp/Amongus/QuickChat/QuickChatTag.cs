using System;
using Cpp2ILInjected;
using UnityEngine;

namespace AmongUs.QuickChat
{
	public class QuickChatTag : ScriptableObject
	{
		[SerializeField]
		public StringNames tagKey;

		[SerializeField]
		private StringNames inputKey;

		[SerializeField]
		public Color color;

		[SerializeField]
		private string[] parsingKeys;

		public StringNames TagNameKey => tagKey;

		public StringNames InputNameKey => inputKey;

		public string LocalizedTagName => DestroyableSingleton<TranslationController>.Instance.GetString(tagKey);

		public string LocalizedInputName => DestroyableSingleton<TranslationController>.Instance.GetString(InputNameKey);

		public unsafe Color Color => color;

		public bool ContainsParsingKey(string key)
		{
			if (parsingKeys == null || parsingKeys.Length < 1)
			{
				return false;
			}
			string[] array = parsingKeys;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ToLower() == key.ToLower())
				{
					return true;
				}
			}
			return false;
		}
	}
}
