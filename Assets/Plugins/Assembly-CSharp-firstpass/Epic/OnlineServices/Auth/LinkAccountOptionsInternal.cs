using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct LinkAccountOptionsInternal : ISettable<LinkAccountOptions>, IDisposable
	{
		private int m_ApiVersion;

		private LinkAccountFlags m_LinkAccountFlags;

		private IntPtr m_ContinuanceToken;

		private IntPtr m_LocalUserId;

		public LinkAccountFlags LinkAccountFlags
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

		public EpicAccountId LocalUserId
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
