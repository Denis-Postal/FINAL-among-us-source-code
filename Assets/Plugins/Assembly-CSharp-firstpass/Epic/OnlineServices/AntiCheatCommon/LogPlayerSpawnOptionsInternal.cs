using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LogPlayerSpawnOptionsInternal : ISettable<LogPlayerSpawnOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SpawnedPlayerHandle;

		private uint m_TeamId;

		private uint m_CharacterId;

		public IntPtr SpawnedPlayerHandle
		{
			set
			{
			}
		}

		public uint TeamId
		{
			set
			{
			}
		}

		public uint CharacterId
		{
			set
			{
			}
		}

		public void Set(ref LogPlayerSpawnOptions other)
		{
		}

		public void Set(ref LogPlayerSpawnOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
