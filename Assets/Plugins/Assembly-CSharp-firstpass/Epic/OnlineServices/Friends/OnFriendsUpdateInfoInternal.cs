using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct OnFriendsUpdateInfoInternal : ICallbackInfoInternal, IGettable<OnFriendsUpdateInfo>, ISettable<OnFriendsUpdateInfo>, IDisposable
	{
		private IntPtr m_ClientData;

		private IntPtr m_LocalUserId;

		private IntPtr m_TargetUserId;

		private FriendsStatus m_PreviousStatus;

		private FriendsStatus m_CurrentStatus;

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

		public FriendsStatus PreviousStatus
		{
			get
			{
				return FriendsStatus.NotFriends;
			}
			set
			{
			}
		}

		public FriendsStatus CurrentStatus
		{
			get
			{
				return FriendsStatus.NotFriends;
			}
			set
			{
			}
		}

		public void Set(ref OnFriendsUpdateInfo other)
		{
		}

		public void Set(ref OnFriendsUpdateInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out OnFriendsUpdateInfo output)
		{
			output = default;
		}
	}
}
