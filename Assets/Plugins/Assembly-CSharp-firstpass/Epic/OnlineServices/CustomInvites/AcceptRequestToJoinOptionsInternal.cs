using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct AcceptRequestToJoinOptionsInternal : ISettable<AcceptRequestToJoinOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public void Set(ref AcceptRequestToJoinOptions other)
		{
		}

		public void Set(ref AcceptRequestToJoinOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
