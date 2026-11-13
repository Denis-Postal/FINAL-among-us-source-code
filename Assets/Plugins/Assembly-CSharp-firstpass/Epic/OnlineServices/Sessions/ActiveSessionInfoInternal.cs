using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 40)]
	internal struct ActiveSessionInfoInternal : IGettable<ActiveSessionInfo>, ISettable<ActiveSessionInfo>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_SessionName;

		private IntPtr m_LocalUserId;

		private OnlineSessionState m_State;

		private IntPtr m_SessionDetails;

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

		public OnlineSessionState State
		{
			get
			{
				return OnlineSessionState.NoSession;
			}
			set
			{
			}
		}

		public SessionDetailsInfo? SessionDetails
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref ActiveSessionInfo other)
		{
		}

		public void Set(ref ActiveSessionInfo? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ActiveSessionInfo output)
		{
			output = default;
		}
	}
}
