using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct OnRequestToJoinAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnRequestToJoinAcceptedCallbackInfo>, ISettable<OnRequestToJoinAcceptedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_TargetUserId;

		private IntPtr m_LocalUserId;

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

		public ProductUserId TargetUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

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

		public void Set(ref OnRequestToJoinAcceptedCallbackInfo other)
		{
		}

		public void Set(ref OnRequestToJoinAcceptedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnRequestToJoinAcceptedCallbackInfo output)
		{
			output = default;
		}
	}
}
