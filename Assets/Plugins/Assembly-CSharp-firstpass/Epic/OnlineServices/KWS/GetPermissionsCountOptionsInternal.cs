using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct GetPermissionsCountOptionsInternal : ISettable<GetPermissionsCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public void Set(ref GetPermissionsCountOptions other)
		{
		}

		public void Set(ref GetPermissionsCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
