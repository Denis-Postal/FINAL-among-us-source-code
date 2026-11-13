using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UserInfo
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CopyBestDisplayNameWithPlatformOptionsInternal : ISettable<CopyBestDisplayNameWithPlatformOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private uint m_TargetPlatformType;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public EpicAccountId TargetUserId
		{
			set
			{
			}
		}

		public uint TargetPlatformType
		{
			set
			{
			}
		}

		public void Set(ref CopyBestDisplayNameWithPlatformOptions other)
		{
		}

		public void Set(ref CopyBestDisplayNameWithPlatformOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
