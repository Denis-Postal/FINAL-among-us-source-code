using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct RequestToJoinReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<RequestToJoinReceivedCallbackInfo>, ISettable<RequestToJoinReceivedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_FromUserId;

		private IntPtr m_ToUserId;

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

		public void Set(ref RequestToJoinReceivedCallbackInfo other)
		{
		}

		public void Set(ref RequestToJoinReceivedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out RequestToJoinReceivedCallbackInfo output)
		{
			output = default;
		}
	}
}
