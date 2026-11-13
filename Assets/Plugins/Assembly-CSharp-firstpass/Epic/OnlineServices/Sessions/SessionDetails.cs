using System;

namespace Epic.OnlineServices.Sessions
{
	public sealed class SessionDetails : Handle
	{
		public const int SessiondetailsAttributeApiLatest = 1;

		public const int SessiondetailsCopyinfoApiLatest = 1;

		public const int SessiondetailsCopysessionattributebyindexApiLatest = 1;

		public const int SessiondetailsCopysessionattributebykeyApiLatest = 1;

		public const int SessiondetailsGetsessionattributecountApiLatest = 1;

		public const int SessiondetailsInfoApiLatest = 2;

		public const int SessiondetailsSettingsApiLatest = 4;

		public SessionDetails()
		{
		}

		public SessionDetails(IntPtr innerHandle)
		{
		}

		public Result CopyInfo(ref SessionDetailsCopyInfoOptions options, out SessionDetailsInfo? outSessionInfo)
		{
			outSessionInfo = null;
			return Result.Success;
		}

		public Result CopySessionAttributeByIndex(ref SessionDetailsCopySessionAttributeByIndexOptions options, out SessionDetailsAttribute? outSessionAttribute)
		{
			outSessionAttribute = null;
			return Result.Success;
		}

		public Result CopySessionAttributeByKey(ref SessionDetailsCopySessionAttributeByKeyOptions options, out SessionDetailsAttribute? outSessionAttribute)
		{
			outSessionAttribute = null;
			return Result.Success;
		}

		public uint GetSessionAttributeCount(ref SessionDetailsGetSessionAttributeCountOptions options)
		{
			return 0u;
		}

		public void Release()
		{
		}
	}
}
