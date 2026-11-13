using Newtonsoft.Json;
using System;
using Cpp2ILInjected;

namespace AmongUs.Data.Player
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PlayerBanData : AbstractSaveDataGroup
	{
		private const int POINTS_UNTIL_BAN_STARTS = 2;

		private const int MINUTES_PER_BAN_POINT = 5;

		[JsonProperty("banPoints")]
		public float banPoints;

		[JsonProperty("previousGameStartDate")]
		private long previousGameStartDate;

		private Action m_OnBanPointsChanged;

		private Action m_OnPreviousGameStartedDateChanged;

		public float BanPoints
		{
			get
			{
				return banPoints;
			}
			set
			{
				SetValue(ref banPoints, value, m_OnBanPointsChanged); // TODO(verify): setter rebuilt (decomp stub)
			}
		}

		public DateTime PreviousGameStartDate
		{
				get
			{
				// TODO(verify): rebuilt (decomp stub)
				return new DateTime(previousGameStartDate);
			}
				set
			{
				// TODO(verify): rebuilt (decomp stub)
				SetValue(ref previousGameStartDate, value.Ticks, m_OnPreviousGameStartedDateChanged);
			}
		}

		public bool IsBanned
		{
			get
			{
				return BanMinutesLeft > 0;
			}
		}

		public float BanMinutes
		{
			get
			{
				//IL_001b: Invalid comparison between F4 and I4
				//IL_003b: Expected F4, but got I4
				float num = banPoints - 2f;
				if (num < 0f)
				{
					num = 0f;
				}
				return num * 5f;
			}
		}

		public int BanMinutesLeft
		{
			get
			{ // from decomp_mono (the il2cpp decompile lost this body)
				TimeSpan timeSpan = AmongUsDateTime.UtcNow - PreviousGameStartDate;
				int num = UnityEngine.Mathf.CeilToInt(BanMinutes - (float)timeSpan.TotalMinutes);
				if (num > 1440 || timeSpan.TotalDays > 1.0)
				{
					BanPoints = 0f;
					return 0;
				}
				return num;
			}
		}

		public event Action OnBanPointsChanged
		{
			add
			{
				m_OnBanPointsChanged = (Action)System.Delegate.Combine(m_OnBanPointsChanged, value);
			}
			remove
			{
				m_OnBanPointsChanged = (Action)System.Delegate.Remove(m_OnBanPointsChanged, value);
			}
		}

		public event Action OnPreviousGameStartedDateChanged
		{
			add
			{
				m_OnPreviousGameStartedDateChanged = (Action)System.Delegate.Combine(m_OnPreviousGameStartedDateChanged, value);
			}
			remove
			{
				m_OnPreviousGameStartedDateChanged = (Action)System.Delegate.Remove(m_OnPreviousGameStartedDateChanged, value);
			}
		}
	}
}
