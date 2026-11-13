using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Settings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class AudioSettingsData : AbstractSaveDataGroup
	{
		public const float DEFAULT_MUSIC_VOLUME = 0.6f;

		public const float DEFAULT_AMBIENCE_VOLUME = 0.6f;

		public const float DEFAULT_SFX_VOLUME = 0.6f;

		[JsonProperty("musicVolume")]
		public float musicVolume = 0.6f;

		[JsonProperty("ambienceVolume")]
		public float ambienceVolume = 0.6f;

		[JsonProperty("sfxVolume")]
		public float sfxVolume = 0.6f;

		private Action m_OnMusicVolumeChanged;

		private Action m_OnAmbienceVolumeChanged;

		private Action m_OnSfxVolumeChanged;

		public float MusicVolume
		{
			get
			{
				return musicVolume;
			}
			set
			{
				SetValue(ref musicVolume, value, m_OnMusicVolumeChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public float AmbienceVolume
		{
			get
			{
				return ambienceVolume;
			}
			set
			{
				SetValue(ref ambienceVolume, value, m_OnAmbienceVolumeChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public float SfxVolume
		{
			get
			{
				return sfxVolume;
			}
			set
			{
				SetValue(ref sfxVolume, value, m_OnSfxVolumeChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public event Action OnMusicVolumeChanged
		{
			add
			{
				m_OnMusicVolumeChanged = (Action)System.Delegate.Combine(m_OnMusicVolumeChanged, value);
			}
			remove
			{
				m_OnMusicVolumeChanged = (Action)System.Delegate.Remove(m_OnMusicVolumeChanged, value);
			}
		}

		public event Action OnAmbienceVolumeChanged
		{
			add
			{
				m_OnAmbienceVolumeChanged = (Action)System.Delegate.Combine(m_OnAmbienceVolumeChanged, value);
			}
			remove
			{
				m_OnAmbienceVolumeChanged = (Action)System.Delegate.Remove(m_OnAmbienceVolumeChanged, value);
			}
		}

		public event Action OnSfxVolumeChanged
		{
			add
			{
				m_OnSfxVolumeChanged = (Action)System.Delegate.Combine(m_OnSfxVolumeChanged, value);
			}
			remove
			{
				m_OnSfxVolumeChanged = (Action)System.Delegate.Remove(m_OnSfxVolumeChanged, value);
			}
		}
	}
}
