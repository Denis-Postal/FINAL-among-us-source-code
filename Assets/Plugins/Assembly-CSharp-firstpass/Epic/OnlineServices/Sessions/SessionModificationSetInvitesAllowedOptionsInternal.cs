using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SessionModificationSetInvitesAllowedOptionsInternal : ISettable<SessionModificationSetInvitesAllowedOptions>, IDisposable
	{
		private int m_ApiVersion;

		private int m_InvitesAllowed;

		public bool InvitesAllowed
		{
			set
			{
			}
		}

		public void Set(ref SessionModificationSetInvitesAllowedOptions other)
		{
		}

		public void Set(ref SessionModificationSetInvitesAllowedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
