using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct SendCustomInviteOptionsInternal : ISettable<SendCustomInviteOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserIds;

		private uint m_TargetUserIdsCount;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public ProductUserId[] TargetUserIds
		{
			set
			{
			}
		}

		public void Set(ref SendCustomInviteOptions other)
		{
		}

		public void Set(ref SendCustomInviteOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
