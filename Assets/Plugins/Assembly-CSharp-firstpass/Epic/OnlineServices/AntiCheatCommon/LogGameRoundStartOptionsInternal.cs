using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct LogGameRoundStartOptionsInternal : ISettable<LogGameRoundStartOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionIdentifier;

		private IntPtr m_LevelName;

		private IntPtr m_ModeName;

		private uint m_RoundTimeSeconds;

		private AntiCheatCommonGameRoundCompetitionType m_CompetitionType;

		public Utf8String SessionIdentifier
		{
			set
			{
			}
		}

		public Utf8String LevelName
		{
			set
			{
			}
		}

		public Utf8String ModeName
		{
			set
			{
			}
		}

		public uint RoundTimeSeconds
		{
			set
			{
			}
		}

		public AntiCheatCommonGameRoundCompetitionType CompetitionType
		{
			set
			{
			}
		}

		public void Set(ref LogGameRoundStartOptions other)
		{
		}

		public void Set(ref LogGameRoundStartOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
