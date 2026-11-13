using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetNATTypeOptionsInternal : ISettable<GetNATTypeOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetNATTypeOptions other)
		{
		}

		public void Set(ref GetNATTypeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
