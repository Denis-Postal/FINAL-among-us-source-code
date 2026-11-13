using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct JoinSessionAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinSessionAcceptedCallbackInfo>, ISettable<JoinSessionAcceptedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private ulong m_UiEventId;

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

		public ulong UiEventId
		{
			get
			{
				return 0uL;
			}
			set
			{
			}
		}

		public void Set(ref JoinSessionAcceptedCallbackInfo other)
		{
		}

		public void Set(ref JoinSessionAcceptedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out JoinSessionAcceptedCallbackInfo output)
		{
			output = default;
		}
	}
}
