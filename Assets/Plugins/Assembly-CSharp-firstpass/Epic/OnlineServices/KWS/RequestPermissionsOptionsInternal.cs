using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RequestPermissionsOptionsInternal : ISettable<RequestPermissionsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private uint m_PermissionKeyCount;

		private IntPtr m_PermissionKeys;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String[] PermissionKeys
		{
			set
			{
			}
		}

		public void Set(ref RequestPermissionsOptions other)
		{
		}

		public void Set(ref RequestPermissionsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
