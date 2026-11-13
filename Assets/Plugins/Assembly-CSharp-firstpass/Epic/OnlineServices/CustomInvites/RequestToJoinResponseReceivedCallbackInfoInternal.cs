using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct RequestToJoinResponseReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RequestToJoinResponseReceivedCallbackInfo>, ISettable<RequestToJoinResponseReceivedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_FromUserId;

		private IntPtr m_ToUserId;

		private RequestToJoinResponse m_Response;

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

		public ProductUserId FromUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public ProductUserId ToUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public RequestToJoinResponse Response
		{
			get
			{
				return RequestToJoinResponse.Accepted;
			}
			set
			{
			}
		}

		public void Set(ref RequestToJoinResponseReceivedCallbackInfo other)
		{
		}

		public void Set(ref RequestToJoinResponseReceivedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RequestToJoinResponseReceivedCallbackInfo output)
		{
			output = default;
		}
	}
}
