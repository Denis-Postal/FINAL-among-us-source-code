using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	[StructLayout((LayoutKind)0, Pack = 8, Size = 4)]
	internal struct TransactionGetEntitlementsCountOptionsInternal : ISettable<TransactionGetEntitlementsCountOptions>, IDisposable
	{
		private int m_ApiVersion;

		public void Set(ref TransactionGetEntitlementsCountOptions other)
		{
		}

		public void Set(ref TransactionGetEntitlementsCountOptions? other)
		{
		}

		public void Dispose()
		{
		}
	}
}
