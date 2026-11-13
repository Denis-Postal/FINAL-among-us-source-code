using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct QueryPermissionsOptionsInternal : ISettable<QueryPermissionsOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref QueryPermissionsOptions other)
		{
		}

		public void Set(ref QueryPermissionsOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
