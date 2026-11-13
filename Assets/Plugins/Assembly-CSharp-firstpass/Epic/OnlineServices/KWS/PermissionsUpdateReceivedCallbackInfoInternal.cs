using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct PermissionsUpdateReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<PermissionsUpdateReceivedCallbackInfo>, ISettable<PermissionsUpdateReceivedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_KWSUserId;

		private IntPtr m_DateOfBirth;

		private int m_IsMinor;

		private IntPtr m_ParentEmail;

		public object ClientData
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public IntPtr ClientDataAddress => (IntPtr)0;

		public ProductUserId LocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String KWSUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String DateOfBirth
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IsMinor
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public Utf8String ParentEmail
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PermissionsUpdateReceivedCallbackInfo other)
		{
		}

		public void Set(ref PermissionsUpdateReceivedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PermissionsUpdateReceivedCallbackInfo output)
		{
			output = default;
		}
	}
}
