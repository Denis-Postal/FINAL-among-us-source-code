using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct QueryActivePlayerSanctionsOptionsInternal : ISettable<QueryActivePlayerSanctionsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_TargetUserId;

		private IntPtr m_LocalUserId;

		public ProductUserId TargetUserId
		{
			set
			{
			}
		}

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref QueryActivePlayerSanctionsOptions other)
		{
		}

		public void Set(ref QueryActivePlayerSanctionsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
