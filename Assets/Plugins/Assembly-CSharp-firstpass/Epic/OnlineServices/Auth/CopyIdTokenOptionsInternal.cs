using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CopyIdTokenOptionsInternal : ISettable<CopyIdTokenOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_AccountId;

		public EpicAccountId AccountId
		{
			set
			{
			}
		}

		public void Set(ref CopyIdTokenOptions other)
		{
		}

		public void Set(ref CopyIdTokenOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
