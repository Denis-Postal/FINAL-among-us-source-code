using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct SetGameSessionIdOptionsInternal : ISettable<SetGameSessionIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_GameSessionId;

		public Utf8String GameSessionId
		{
			set
			{
			}
		}

		public void Set(ref SetGameSessionIdOptions other)
		{
		}

		public void Set(ref SetGameSessionIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
