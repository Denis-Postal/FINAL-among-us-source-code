using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct LogPlayerDespawnOptionsInternal : ISettable<LogPlayerDespawnOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_DespawnedPlayerHandle;

		public IntPtr DespawnedPlayerHandle
		{
			set
			{
			}
		}

		public void Set(ref LogPlayerDespawnOptions other)
		{
		}

		public void Set(ref LogPlayerDespawnOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
