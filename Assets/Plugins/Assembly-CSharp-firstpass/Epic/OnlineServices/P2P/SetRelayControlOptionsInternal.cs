using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 8)]
	internal struct SetRelayControlOptionsInternal : ISettable<SetRelayControlOptions>, IDisposable
	{
		private int m_ApiVersion;

		private RelayControl m_RelayControl;

		public RelayControl RelayControl
		{
			set
			{
			}
		}

		public void Set(ref SetRelayControlOptions other)
		{
		}

		public void Set(ref SetRelayControlOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
