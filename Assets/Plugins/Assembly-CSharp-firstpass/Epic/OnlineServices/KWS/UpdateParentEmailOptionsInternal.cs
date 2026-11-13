using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct UpdateParentEmailOptionsInternal : ISettable<UpdateParentEmailOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_ParentEmail;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String ParentEmail
		{
			set
			{
			}
		}

		public void Set(ref UpdateParentEmailOptions other)
		{
		}

		public void Set(ref UpdateParentEmailOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
