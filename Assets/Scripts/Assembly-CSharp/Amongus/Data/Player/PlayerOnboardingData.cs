using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Player
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PlayerOnboardingData : AbstractSaveDataGroup
	{
		public const int DEFAULT_PRIVACY_POLICY_VERSION = 0;

		public const bool DEFAULT_VIEWED_ONLINE_HELP_MESSAGE = false;

		public const bool DEFAULT_VIEWED_MIN_PLAYER_WARNING = false;

		public const bool DEFAULT_ALWAYS_SHOW_MIN_PLAYER_WARNING = true;

		public const bool DEFAULT_VIEWED_HIDEANDSEEK_HOWTOPLAY = false;

		[JsonProperty("privacyPolicyVersion")]
		public int privacyPolicyVersion;

		[JsonProperty("viewedOnlineHelpMessage")]
		public bool viewedOnlineHelpMessage;

		[JsonProperty("viewedMinPlayerWarning")]
		public bool viewedMinPlayerWarning;

		[JsonProperty("alwaysShowMinPlayerWarning")]
		public bool alwaysShowMinPlayerWarning = true;

		[JsonProperty("viewedHideAndSeekHowToPlay")]
		public bool viewedHideAndSeekHowToPlay;

		private Action m_OnLastAcceptedPrivacyPolicyVersionChanged;

		private Action m_OnViewedOnlineHelpMessageChanged;

		private Action m_OnViewedMinPlayerWarningChanged;

		private Action m_OnAlwaysShowMinPlayerWarningChanged;

		private Action m_OnViewedHideAndSeekHowToPlay;

		public int LastAcceptedPrivacyPolicyVersion
		{
			get
			{
				return privacyPolicyVersion;
			}
			set
			{
				SetValue(ref privacyPolicyVersion, value, m_OnLastAcceptedPrivacyPolicyVersionChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public bool ViewedOnlineHelpMessage
		{
			get
			{
				return viewedOnlineHelpMessage;
			}
			set
			{
				SetValue(ref viewedOnlineHelpMessage, value, m_OnViewedOnlineHelpMessageChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public bool ViewedMinPlayerWarning
		{
			get
			{
				return viewedMinPlayerWarning;
			}
			set
			{
				SetValue(ref viewedMinPlayerWarning, value, m_OnViewedMinPlayerWarningChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public bool AlwaysShowMinPlayerWarning
		{
			get
			{
				return alwaysShowMinPlayerWarning;
			}
			set
			{
				SetValue(ref alwaysShowMinPlayerWarning, value, m_OnAlwaysShowMinPlayerWarningChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public bool ViewedHideAndSeekHowToPlay
		{
			get
			{
				return viewedHideAndSeekHowToPlay;
			}
			set
			{
				SetValue(ref viewedHideAndSeekHowToPlay, value, m_OnViewedHideAndSeekHowToPlay); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public event Action OnLastAcceptedPrivacyPolicyVersionChanged
		{
			add
			{
				m_OnLastAcceptedPrivacyPolicyVersionChanged = (Action)System.Delegate.Combine(m_OnLastAcceptedPrivacyPolicyVersionChanged, value);
			}
			remove
			{
				m_OnLastAcceptedPrivacyPolicyVersionChanged = (Action)System.Delegate.Remove(m_OnLastAcceptedPrivacyPolicyVersionChanged, value);
			}
		}

		public event Action OnViewedOnlineHelpMessageChanged
		{
			add
			{
				m_OnViewedOnlineHelpMessageChanged = (Action)System.Delegate.Combine(m_OnViewedOnlineHelpMessageChanged, value);
			}
			remove
			{
				m_OnViewedOnlineHelpMessageChanged = (Action)System.Delegate.Remove(m_OnViewedOnlineHelpMessageChanged, value);
			}
		}

		public event Action OnViewedMinPlayerWarningChanged
		{
			add
			{
				m_OnViewedMinPlayerWarningChanged = (Action)System.Delegate.Combine(m_OnViewedMinPlayerWarningChanged, value);
			}
			remove
			{
				m_OnViewedMinPlayerWarningChanged = (Action)System.Delegate.Remove(m_OnViewedMinPlayerWarningChanged, value);
			}
		}

		public event Action OnAlwaysShowMinPlayerWarningChanged
		{
			add
			{
				m_OnAlwaysShowMinPlayerWarningChanged = (Action)System.Delegate.Combine(m_OnAlwaysShowMinPlayerWarningChanged, value);
			}
			remove
			{
				m_OnAlwaysShowMinPlayerWarningChanged = (Action)System.Delegate.Remove(m_OnAlwaysShowMinPlayerWarningChanged, value);
			}
		}

		public event Action OnViewedHideAndSeekHowToPlay
		{
			add
			{
				m_OnViewedHideAndSeekHowToPlay = (Action)System.Delegate.Combine(m_OnViewedHideAndSeekHowToPlay, value);
			}
			remove
			{
				m_OnViewedHideAndSeekHowToPlay = (Action)System.Delegate.Remove(m_OnViewedHideAndSeekHowToPlay, value);
			}
		}
	}
}
