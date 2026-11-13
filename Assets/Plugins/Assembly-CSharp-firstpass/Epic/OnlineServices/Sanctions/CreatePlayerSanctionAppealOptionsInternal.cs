using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 32)]
	internal struct CreatePlayerSanctionAppealOptionsInternal : ISettable<CreatePlayerSanctionAppealOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private SanctionAppealReason m_Reason;

		private IntPtr m_ReferenceId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public SanctionAppealReason Reason
		{
			set
			{
			}
		}

		public Utf8String ReferenceId
		{
			set
			{
			}
		}

		public void Set(ref CreatePlayerSanctionAppealOptions other)
		{
		}

		public void Set(ref CreatePlayerSanctionAppealOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
