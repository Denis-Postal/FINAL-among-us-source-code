using System;

namespace Epic.OnlineServices.Ecom
{
	public sealed class Transaction : Handle
	{
		public const int TransactionCopyentitlementbyindexApiLatest = 1;

		public const int TransactionGetentitlementscountApiLatest = 1;

		public Transaction()
		{
		}

		public Transaction(IntPtr innerHandle)
		{
		}

		public Result CopyEntitlementByIndex(ref TransactionCopyEntitlementByIndexOptions options, out Entitlement? outEntitlement)
		{
			outEntitlement = null;
			return Result.Success;
		}

		public uint GetEntitlementsCount(ref TransactionGetEntitlementsCountOptions options)
		{
			return 0u;
		}

		public Result GetTransactionId(out Utf8String outBuffer)
		{
			outBuffer = null;
			return Result.Success;
		}

		public void Release()
		{
		}
	}
}
