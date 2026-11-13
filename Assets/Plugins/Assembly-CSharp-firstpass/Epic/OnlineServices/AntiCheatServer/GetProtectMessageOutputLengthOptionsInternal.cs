using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatServer
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct GetProtectMessageOutputLengthOptionsInternal : ISettable<GetProtectMessageOutputLengthOptions>, IDisposable
	{
		private int m_ApiVersion;

		private uint m_DataLengthBytes;

		public uint DataLengthBytes
		{
			set
			{
			}
		}

		public void Set(ref GetProtectMessageOutputLengthOptions other)
		{
		}

		public void Set(ref GetProtectMessageOutputLengthOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
