using System;

namespace Epic.OnlineServices
{
	public sealed class Common
	{
		public const ulong InvalidNotificationid = 0uL;

		public const int PagequeryApiLatest = 1;

		public const int PagequeryMaxcountDefault = 10;

		public const int PagequeryMaxcountMaximum = 100;

		public const int PaginationApiLatest = 1;

		public static bool IsOperationComplete(Result result)
		{
			return false;
		}

		public static Utf8String ToString(Result result)
		{
			return null;
		}

		public static Result ToString(ArraySegment<byte> byteArray, out Utf8String outBuffer)
		{
			outBuffer = null;
			return Result.Success;
		}

		public static Utf8String ToString(ArraySegment<byte> byteArray)
		{
			return null;
		}
	}
}
