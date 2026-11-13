using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CreateUserOptionsInternal : ISettable<CreateUserOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_DateOfBirth;

		private IntPtr m_ParentEmail;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String DateOfBirth
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

		public void Set(ref CreateUserOptions other)
		{
		}

		public void Set(ref CreateUserOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
