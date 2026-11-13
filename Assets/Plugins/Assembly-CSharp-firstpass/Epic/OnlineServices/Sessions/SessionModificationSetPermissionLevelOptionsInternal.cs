using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SessionModificationSetPermissionLevelOptionsInternal : ISettable<SessionModificationSetPermissionLevelOptions>, IDisposable
	{
		private int m_ApiVersion;

		private OnlineSessionPermissionLevel m_PermissionLevel;

		public OnlineSessionPermissionLevel PermissionLevel
		{
			set
			{
			}
		}

		public void Set(ref SessionModificationSetPermissionLevelOptions other)
		{
		}

		public void Set(ref SessionModificationSetPermissionLevelOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
