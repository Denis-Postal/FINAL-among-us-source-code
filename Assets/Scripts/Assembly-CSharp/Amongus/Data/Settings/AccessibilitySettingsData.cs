using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Settings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class AccessibilitySettingsData : AbstractSaveDataGroup
	{
		public const bool DEFAULT_COLOR_BLIND_MODE = false;

		[JsonProperty("colorBlindMode")]
		public bool colorBlindMode;

		private Action m_OnColorBlindModeChanged;

		public bool ColorBlindMode
		{
			get
			{
				return colorBlindMode;
			}
			set
			{
				SetValue(ref colorBlindMode, value, m_OnColorBlindModeChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public event Action OnColorBlindModeChanged
		{
			add
			{
				m_OnColorBlindModeChanged = (Action)System.Delegate.Combine(m_OnColorBlindModeChanged, value);
			}
			remove
			{
				m_OnColorBlindModeChanged = (Action)System.Delegate.Remove(m_OnColorBlindModeChanged, value);
			}
		}
	}
}
