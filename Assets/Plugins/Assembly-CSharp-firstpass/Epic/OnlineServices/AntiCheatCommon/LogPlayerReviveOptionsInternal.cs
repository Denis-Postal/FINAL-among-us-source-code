using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LogPlayerReviveOptionsInternal : ISettable<LogPlayerReviveOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_RevivedPlayerHandle;

		private IntPtr m_ReviverPlayerHandle;

		public IntPtr RevivedPlayerHandle
		{
			set
			{
			}
		}

		public IntPtr ReviverPlayerHandle
		{
			set
			{
			}
		}

		public void Set(ref LogPlayerReviveOptions other)
		{
		}

		public void Set(ref LogPlayerReviveOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
