using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Player
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PlayerAccountData : AbstractSaveDataGroup
	{
		public const EOSManager.AccountLoginStatus DEFAULT_LOGIN_STATUS = EOSManager.AccountLoginStatus.Offline;

		public const string DEFAULT_GUARDIAN_EMAIL = "";

		public const bool DEFAULT_DELETE_DEVICE_ID_FAILED = false;

		[JsonProperty("loginStatus")]
		public EOSManager.AccountLoginStatus loginStatus;

		[JsonProperty("guardianEmail")]
		public string guardianEmail;

		[JsonProperty("deleteDeviceIDFailed")]
		public bool deleteDeviceIDFailed;

		private Action m_OnLoginStatusChanged;

		private Action m_OnGuardianEmailChanged;

		private Action m_OnDeleteDeviceIDFailedChanged;

		public EOSManager.AccountLoginStatus LoginStatus
		{
			get
			{
				return loginStatus;
			}
			set
			{
				SetValue(ref loginStatus, value, m_OnLoginStatusChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public string GuardianEmail
		{
			get
			{
				return guardianEmail;
			}
			set
			{
				SetValue(ref guardianEmail, value, m_OnGuardianEmailChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public bool DeleteDeviceIDFailed
		{
			get
			{
				return deleteDeviceIDFailed;
			}
			set
			{
				SetValue(ref deleteDeviceIDFailed, value, m_OnDeleteDeviceIDFailedChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public event Action OnLoginStatusChanged
		{
			add
			{
				m_OnLoginStatusChanged = (Action)System.Delegate.Combine(m_OnLoginStatusChanged, value);
			}
			remove
			{
				m_OnLoginStatusChanged = (Action)System.Delegate.Remove(m_OnLoginStatusChanged, value);
			}
		}

		public event Action OnGuardianEmailChanged
		{
			add
			{
				m_OnGuardianEmailChanged = (Action)System.Delegate.Combine(m_OnGuardianEmailChanged, value);
			}
			remove
			{
				m_OnGuardianEmailChanged = (Action)System.Delegate.Remove(m_OnGuardianEmailChanged, value);
			}
		}

		public event Action OnDeleteDeviceIDFailedChanged
		{
			add
			{
				m_OnDeleteDeviceIDFailedChanged = (Action)System.Delegate.Combine(m_OnDeleteDeviceIDFailedChanged, value);
			}
			remove
			{
				m_OnDeleteDeviceIDFailedChanged = (Action)System.Delegate.Remove(m_OnDeleteDeviceIDFailedChanged, value);
			}
		}

		public PlayerAccountData()
		{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
			guardianEmail = "";
		}
	}
}
