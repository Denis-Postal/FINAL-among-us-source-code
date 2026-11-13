using Newtonsoft.Json;
﻿using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Settings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class LanguageSettingsData : AbstractSaveDataGroup
	{
		public const string DEFAULT_LANGUAGE = "none";

		public const SupportedLangs DEFAULT_CACHED_LANGUAGE = (SupportedLangs)(-1);

		[JsonProperty("currentLanguage")]
		public string language;

		public SupportedLangs cachedLanguage;

		private Action m_OnLanguageChanged;

		public unsafe SupportedLangs CurrentLanguage
		{
			get
			{
				return cachedLanguage;
			}
			set
			{
				// TODO(verify): reconstructed from damaged decompile (IsDefined -> store name, cache, raise event)
				if (Enum.IsDefined(typeof(SupportedLangs), value))
				{
					SetValue(ref language, value.ToString(), m_OnLanguageChanged);
				}
				cachedLanguage = value;
			}
		}

		public event Action OnLanguageChanged
		{
			add
			{
				m_OnLanguageChanged = (Action)System.Delegate.Combine(m_OnLanguageChanged, value);
			}
			remove
			{
				m_OnLanguageChanged = (Action)System.Delegate.Remove(m_OnLanguageChanged, value);
			}
		}

		protected unsafe override void OnSaveStart()
		{
			// TODO(verify): reconstructed from damaged decompile
			if (Enum.IsDefined(typeof(SupportedLangs), cachedLanguage))
			{
				language = cachedLanguage.ToString();
			}
		}

		protected unsafe override void OnLoadComplete()
		{
			if (!TranslationController.TryParseLanguage(language, out cachedLanguage))
			{
				DataManager.logger.Error(new string[1] { "LanguageSettingsData" }, "Loading language failed! Could not load `" + language + "` as 'SupportedLangs' value.");
			}
		}

		public LanguageSettingsData()
		{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
			language = "none";
			cachedLanguage = (SupportedLangs)(-1);
		}
	}
}
