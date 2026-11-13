using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Lobby
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct ParseConnectStringOptionsInternal : ISettable<ParseConnectStringOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ConnectString;

		public Utf8String ConnectString
		{
			set
			{
			}
		}

		public void Set(ref ParseConnectStringOptions other)
		{
		}

		public void Set(ref ParseConnectStringOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
