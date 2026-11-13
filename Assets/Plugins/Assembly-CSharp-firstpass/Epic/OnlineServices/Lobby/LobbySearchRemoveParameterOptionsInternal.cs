using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LobbySearchRemoveParameterOptionsInternal : ISettable<LobbySearchRemoveParameterOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Key;

		private ComparisonOp m_ComparisonOp;

		public Utf8String Key
		{
			set
			{
			}
		}

		public ComparisonOp ComparisonOp
		{
			set
			{
			}
		}

		public void Set(ref LobbySearchRemoveParameterOptions other)
		{
		}

		public void Set(ref LobbySearchRemoveParameterOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
