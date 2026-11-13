using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct VerifyIdTokenOptionsInternal : ISettable<VerifyIdTokenOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_IdToken;

		public IdToken? IdToken
		{
			set
			{
			}
		}

		public void Set(ref VerifyIdTokenOptions other)
		{
		}

		public void Set(ref VerifyIdTokenOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
