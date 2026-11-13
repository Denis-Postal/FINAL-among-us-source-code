using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct IsUserInSessionOptionsInternal : ISettable<IsUserInSessionOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionName;

		private IntPtr m_TargetUserId;

		public Utf8String SessionName
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

		public void Set(ref IsUserInSessionOptions other)
		{
		}

		public void Set(ref IsUserInSessionOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
