using System;

namespace Epic.OnlineServices.Sessions
{
	public sealed class ActiveSession : Handle
	{
		public const int ActivesessionCopyinfoApiLatest = 1;

		public const int ActivesessionGetregisteredplayerbyindexApiLatest = 1;

		public const int ActivesessionGetregisteredplayercountApiLatest = 1;

		public const int ActivesessionInfoApiLatest = 1;

		public ActiveSession()
		{
		}

		public ActiveSession(IntPtr innerHandle)
		{
		}

		public Result CopyInfo(ref ActiveSessionCopyInfoOptions options, out ActiveSessionInfo? outActiveSessionInfo)
		{
			outActiveSessionInfo = null;
			return Result.Success;
		}

		public ProductUserId GetRegisteredPlayerByIndex(ref ActiveSessionGetRegisteredPlayerByIndexOptions options)
		{
			return null;
		}

		public uint GetRegisteredPlayerCount(ref ActiveSessionGetRegisteredPlayerCountOptions options)
		{
			return 0u;
		}

		public void Release()
		{
		}
	}
}
