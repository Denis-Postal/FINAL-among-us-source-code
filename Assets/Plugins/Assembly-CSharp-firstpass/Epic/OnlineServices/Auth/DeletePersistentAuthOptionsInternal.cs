using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct DeletePersistentAuthOptionsInternal : ISettable<DeletePersistentAuthOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_RefreshToken;

		public Utf8String RefreshToken
		{
			set
			{
			}
		}

		public void Set(ref DeletePersistentAuthOptions other)
		{
		}

		public void Set(ref DeletePersistentAuthOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
