using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct GetRelayControlOptionsInternal : ISettable<GetRelayControlOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref GetRelayControlOptions other)
		{
		}

		public void Set(ref GetRelayControlOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
