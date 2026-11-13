using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct JoinGameAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinGameAcceptedCallbackInfo>, ISettable<JoinGameAcceptedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_JoinInfo;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

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

		public Utf8String JoinInfo
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public EpicAccountId LocalUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public EpicAccountId TargetUserId
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

		public void Set(ref JoinGameAcceptedCallbackInfo other)
		{
		}

		public void Set(ref JoinGameAcceptedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out JoinGameAcceptedCallbackInfo output)
		{
			output = default;
		}
	}
}
