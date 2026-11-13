using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct AttributeInternal : IGettable<Attribute>, ISettable<Attribute>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_Data;

		private LobbyAttributeVisibility m_Visibility;

		public AttributeData? Data
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public LobbyAttributeVisibility Visibility
		{
			get
			{
				return LobbyAttributeVisibility.Public;
			}
			set
			{
			}
		}

		public void Set(ref Attribute other)
		{
		}

		public void Set(ref Attribute? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out Attribute output)
		{
			output = default;
		}
	}
}
