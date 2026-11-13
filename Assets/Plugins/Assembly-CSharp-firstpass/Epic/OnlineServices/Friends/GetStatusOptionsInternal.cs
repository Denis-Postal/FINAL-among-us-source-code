using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct GetStatusOptionsInternal : ISettable<GetStatusOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

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

		public void Set(ref GetStatusOptions other)
		{
		}

		public void Set(ref GetStatusOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
