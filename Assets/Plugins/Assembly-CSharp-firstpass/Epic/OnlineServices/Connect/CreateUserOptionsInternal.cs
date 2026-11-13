using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 16)]
	internal struct CreateUserOptionsInternal : ISettable<CreateUserOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_ContinuanceToken;

		public ContinuanceToken ContinuanceToken
		{
			set
			{
			}
		}

		public void Set(ref CreateUserOptions other)
		{
		}

		public void Set(ref CreateUserOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
