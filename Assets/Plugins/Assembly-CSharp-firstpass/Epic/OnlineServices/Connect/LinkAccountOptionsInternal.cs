using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LinkAccountOptionsInternal : ISettable<LinkAccountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_ContinuanceToken;

		public ProductUserId LocalUserId
		{
			set
			{
			}
		}

		public ContinuanceToken ContinuanceToken
		{
			set
			{
			}
		}

		public void Set(ref LinkAccountOptions other)
		{
		}

		public void Set(ref LinkAccountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
