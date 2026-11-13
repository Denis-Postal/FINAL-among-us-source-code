using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct SetCustomInviteOptionsInternal : ISettable<SetCustomInviteOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_Payload;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String Payload
		{
			set
			{
			}
		}

		public void Set(ref SetCustomInviteOptions other)
		{
		}

		public void Set(ref SetCustomInviteOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
