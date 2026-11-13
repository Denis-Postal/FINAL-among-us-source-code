using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Settings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class VideoSettingsData : AbstractSaveDataGroup
	{
		public const bool DEFAULT_VSYNC = false;

		[JsonProperty("vsync")]
		public bool vsync;

		private Action m_OnVSyncChanged;

		public bool VSync
		{
			get
			{
				return vsync;
			}
			set
			{
				SetValue(ref vsync, value, m_OnVSyncChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public event Action OnVSyncChanged
		{
			add
			{
				m_OnVSyncChanged = (Action)System.Delegate.Combine(m_OnVSyncChanged, value);
			}
			remove
			{
				m_OnVSyncChanged = (Action)System.Delegate.Remove(m_OnVSyncChanged, value);
			}
		}
	}
}
