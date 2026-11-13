using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 24)]
	internal struct CopyTransactionByIdOptionsInternal : ISettable<CopyTransactionByIdOptions>, IDisposable
	{
		private int m_ApiVersion;

		private IntPtr m_LocalUserId;

		private IntPtr m_TransactionId;

		public EpicAccountId LocalUserId
		{
			set
			{
			}
		}

		public Utf8String TransactionId
		{
			set
			{
			}
		}

		public void Set(ref CopyTransactionByIdOptions other)
		{
		}

		public void Set(ref CopyTransactionByIdOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
