using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct LogEventOptionsInternal : ISettable<LogEventOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ClientHandle;

		private uint m_EventId;

		private uint m_ParamsCount;

		private IntPtr m_Params;

		public IntPtr ClientHandle
		{
			set
			{
			}
		}

		public uint EventId
		{
			set
			{
			}
		}

		public LogEventParamPair[] Params
		{
			set
			{
			}
		}

		public void Set(ref LogEventOptions other)
		{
		}

		public void Set(ref LogEventOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
