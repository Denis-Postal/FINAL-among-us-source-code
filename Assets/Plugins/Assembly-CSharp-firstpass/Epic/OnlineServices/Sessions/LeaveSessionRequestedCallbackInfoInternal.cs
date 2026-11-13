using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LeaveSessionRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LeaveSessionRequestedCallbackInfo>, ISettable<LeaveSessionRequestedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_SessionName;

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

		public Utf8String SessionName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref LeaveSessionRequestedCallbackInfo other)
		{
		}

		public void Set(ref LeaveSessionRequestedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out LeaveSessionRequestedCallbackInfo output)
		{
			output = default;
		}
	}
}
