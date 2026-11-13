using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct AddExternalIntegrityCatalogOptionsInternal : ISettable<AddExternalIntegrityCatalogOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_PathToBinFile;

		public Utf8String PathToBinFile
		{
			set
			{
			}
		}

		public void Set(ref AddExternalIntegrityCatalogOptions other)
		{
		}

		public void Set(ref AddExternalIntegrityCatalogOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
