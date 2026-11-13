using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct QueryNATTypeOptionsInternal : ISettable<QueryNATTypeOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref QueryNATTypeOptions other)
		{
		}

		public void Set(ref QueryNATTypeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
