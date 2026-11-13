using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnBlockedUsersUpdateInfoInternal : ICallbackInfoInternal, IGettable<OnBlockedUsersUpdateInfo>, ISettable<OnBlockedUsersUpdateInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private int m_Blocked;

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

		public bool Blocked
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public void Set(ref OnBlockedUsersUpdateInfo other)
		{
		}

		public void Set(ref OnBlockedUsersUpdateInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnBlockedUsersUpdateInfo output)
		{
			output = default;
		}
	}
}
