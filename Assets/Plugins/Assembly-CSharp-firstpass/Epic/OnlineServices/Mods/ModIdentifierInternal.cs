using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 48)]
	internal struct ModIdentifierInternal : IGettable<ModIdentifier>, ISettable<ModIdentifier>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_NamespaceId;

		private IntPtr m_ItemId;

		private IntPtr m_ArtifactId;

		private IntPtr m_Title;

		private IntPtr m_Version;

		public Utf8String NamespaceId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ItemId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String ArtifactId
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Title
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Utf8String Version
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void Set(ref ModIdentifier other)
		{
		}

		public void Set(ref ModIdentifier? other)
		{
		}

		public void Dispose()
		{
		}

		public void Get(out ModIdentifier output)
		{
			output = default;
		}
	}
}
