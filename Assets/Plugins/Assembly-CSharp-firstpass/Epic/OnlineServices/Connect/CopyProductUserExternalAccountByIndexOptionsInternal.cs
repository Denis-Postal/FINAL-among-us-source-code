using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyProductUserExternalAccountByIndexOptionsInternal : ISettable<CopyProductUserExternalAccountByIndexOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private uint m_ExternalAccountInfoIndex;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public uint ExternalAccountInfoIndex
		{
			set
			{
			}
		}

		public void Set(ref CopyProductUserExternalAccountByIndexOptions other)
		{
		}

		public void Set(ref CopyProductUserExternalAccountByIndexOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
