using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAdmin
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyUserTokenByUserIdOptionsInternal : ISettable<CopyUserTokenByUserIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private uint m_QueryId;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public uint QueryId
		{
			set
			{
			}
		}

		public void Set(ref CopyUserTokenByUserIdOptions other)
		{
		}

		public void Set(ref CopyUserTokenByUserIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
