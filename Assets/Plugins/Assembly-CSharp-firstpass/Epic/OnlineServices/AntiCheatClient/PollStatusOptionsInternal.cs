using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct PollStatusOptionsInternal : ISettable<PollStatusOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_OutMessageLength;

		public uint OutMessageLength
		{
			set
			{
			}
		}

		public void Set(ref PollStatusOptions other)
		{
		}

		public void Set(ref PollStatusOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
