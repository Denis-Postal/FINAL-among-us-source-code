using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct QueryAgeGateOptionsInternal : ISettable<QueryAgeGateOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref QueryAgeGateOptions other)
		{
		}

		public void Set(ref QueryAgeGateOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
