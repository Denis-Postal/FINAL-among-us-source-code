using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LobbyModificationAddAttributeOptionsInternal : ISettable<LobbyModificationAddAttributeOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Attribute;

		private LobbyAttributeVisibility m_Visibility;

		public AttributeData? Attribute
		{
			set
			{
			}
		}

		public LobbyAttributeVisibility Visibility
		{
			set
			{
			}
		}

		public void Set(ref LobbyModificationAddAttributeOptions other)
		{
		}

		public void Set(ref LobbyModificationAddAttributeOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
