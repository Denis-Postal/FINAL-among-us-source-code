using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Settings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class GameplaySettingsData : AbstractSaveDataGroup
	{
		public const bool DEFAULT_SCREEN_SHAKE = true;

		public const bool DEFAULT_STREAMER_MODE = false;

		[JsonProperty("screenShake")]
		public bool screenShake = true;

		[JsonProperty("streamerMode")]
		public bool streamerMode;

		private Action m_OnScreenShakeChanged;

		private Action m_OnStreamerModeChanged;

		public bool ScreenShake
		{
			get
			{
				return screenShake;
			}
			set
			{
				SetValue(ref screenShake, value, m_OnScreenShakeChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public bool StreamerMode
		{
			get
			{
				return streamerMode;
			}
			set
			{
				SetValue(ref streamerMode, value, m_OnStreamerModeChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public event Action OnScreenShakeChanged
		{
			add
			{
				m_OnScreenShakeChanged = (Action)System.Delegate.Combine(m_OnScreenShakeChanged, value);
			}
			remove
			{
				m_OnScreenShakeChanged = (Action)System.Delegate.Remove(m_OnScreenShakeChanged, value);
			}
		}

		public event Action OnStreamerModeChanged
		{
			add
			{
				m_OnStreamerModeChanged = (Action)System.Delegate.Combine(m_OnStreamerModeChanged, value);
			}
			remove
			{
				m_OnStreamerModeChanged = (Action)System.Delegate.Remove(m_OnStreamerModeChanged, value);
			}
		}
	}
}
