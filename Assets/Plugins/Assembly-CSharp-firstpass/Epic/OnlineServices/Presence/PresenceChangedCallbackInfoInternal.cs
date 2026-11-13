using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct PresenceChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<PresenceChangedCallbackInfo>, ISettable<PresenceChangedCallbackInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_PresenceUserId;

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

		public EpicAccountId PresenceUserId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref PresenceChangedCallbackInfo other)
		{
		}

		public void Set(ref PresenceChangedCallbackInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out PresenceChangedCallbackInfo output)
		{
			output = default;
		}
	}
}
