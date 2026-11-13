using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SessionModificationSetJoinInProgressAllowedOptionsInternal : ISettable<SessionModificationSetJoinInProgressAllowedOptions>, IDisposable
	{
		private int m_ApiVersion;

		private int m_AllowJoinInProgress;

		public bool AllowJoinInProgress
		{
			set
			{
			}
		}

		public void Set(ref SessionModificationSetJoinInProgressAllowedOptions other)
		{
		}

		public void Set(ref SessionModificationSetJoinInProgressAllowedOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
