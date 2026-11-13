using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Player
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PlayerAgeData : AbstractSaveDataGroup
	{
		public static readonly DateTime DEFAULT_DATE_OF_BIRTH = DateTime.MinValue;

		public static readonly DateTime DEFAULT_LATEST_SET_DATE = DateTime.MinValue;

		[JsonProperty("dateOfBirth")]
		public DateTime dateOfBirth = DEFAULT_DATE_OF_BIRTH;

		[JsonProperty("latestSetDate")]
		public DateTime latestSetDate = DEFAULT_LATEST_SET_DATE;

		private Action m_OnDateOfBirthChanged;

		private Action m_OnLatestSetDateChanged;

		public DateTime DateOfBirth
		{
			get
			{
				return dateOfBirth;
			}
			set
			{
				SetValue(ref dateOfBirth, value, m_OnDateOfBirthChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public DateTime LatestSetDate
		{
			get
			{
				return latestSetDate;
			}
			set
			{
				SetValue(ref latestSetDate, value, m_OnLatestSetDateChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public event Action OnDateOfBirthChanged
		{
			add
			{
				m_OnDateOfBirthChanged = (Action)System.Delegate.Combine(m_OnDateOfBirthChanged, value);
			}
			remove
			{
				m_OnDateOfBirthChanged = (Action)System.Delegate.Remove(m_OnDateOfBirthChanged, value);
			}
		}

		public event Action OnLatestSetDateChanged
		{
			add
			{
				m_OnLatestSetDateChanged = (Action)System.Delegate.Combine(m_OnLatestSetDateChanged, value);
			}
			remove
			{
				m_OnLatestSetDateChanged = (Action)System.Delegate.Remove(m_OnLatestSetDateChanged, value);
			}
		}
	}
}
