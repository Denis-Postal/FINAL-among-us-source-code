using System;

namespace Epic.OnlineServices.Stats
{
	public sealed class StatsInterface : Handle
	{
		public const int CopystatbyindexApiLatest = 1;

		public const int CopystatbynameApiLatest = 1;

		public const int GetstatcountApiLatest = 1;

		public const int GetstatscountApiLatest = 1;

		public const int IngestdataApiLatest = 1;

		public const int IngeststatApiLatest = 3;

		public const int MaxIngestStats = 3000;

		public const int MaxQueryStats = 1000;

		public const int QuerystatsApiLatest = 3;

		public const int StatApiLatest = 1;

		public const int TimeUndefined = -1;

		public StatsInterface()
		{
		}

		public StatsInterface(IntPtr innerHandle)
		{
		}

		public Result CopyStatByIndex(ref CopyStatByIndexOptions options, out Stat? outStat)
		{
			outStat = null;
			return Result.Success;
		}

		public Result CopyStatByName(ref CopyStatByNameOptions options, out Stat? outStat)
		{
			outStat = null;
			return Result.Success;
		}

		public uint GetStatsCount(ref GetStatCountOptions options)
		{
			return 0u;
		}

		public void IngestStat(ref IngestStatOptions options, object clientData, OnIngestStatCompleteCallback completionDelegate)
		{
		}

		public void QueryStats(ref QueryStatsOptions options, object clientData, OnQueryStatsCompleteCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnIngestStatCompleteCallbackInternal))]
		internal static void OnIngestStatCompleteCallbackInternalImplementation(ref IngestStatCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryStatsCompleteCallbackInternal))]
		internal static void OnQueryStatsCompleteCallbackInternalImplementation(ref OnQueryStatsCompleteCallbackInfoInternal data)
		{
		}
	}
}
