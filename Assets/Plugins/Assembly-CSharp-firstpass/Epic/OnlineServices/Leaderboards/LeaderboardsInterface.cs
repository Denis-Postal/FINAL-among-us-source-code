using System;

namespace Epic.OnlineServices.Leaderboards
{
	public sealed class LeaderboardsInterface : Handle
	{
		public const int CopyleaderboarddefinitionbyindexApiLatest = 1;

		public const int CopyleaderboarddefinitionbyleaderboardidApiLatest = 1;

		public const int CopyleaderboardrecordbyindexApiLatest = 2;

		public const int CopyleaderboardrecordbyuseridApiLatest = 2;

		public const int CopyleaderboarduserscorebyindexApiLatest = 1;

		public const int CopyleaderboarduserscorebyuseridApiLatest = 1;

		public const int DefinitionApiLatest = 1;

		public const int GetleaderboarddefinitioncountApiLatest = 1;

		public const int GetleaderboardrecordcountApiLatest = 1;

		public const int GetleaderboarduserscorecountApiLatest = 1;

		public const int LeaderboardrecordApiLatest = 2;

		public const int LeaderboarduserscoreApiLatest = 1;

		public const int QueryleaderboarddefinitionsApiLatest = 2;

		public const int QueryleaderboardranksApiLatest = 2;

		public const int QueryleaderboarduserscoresApiLatest = 2;

		public const int TimeUndefined = -1;

		public const int UserscoresquerystatinfoApiLatest = 1;

		public LeaderboardsInterface()
		{
		}

		public LeaderboardsInterface(IntPtr innerHandle)
		{
		}

		public Result CopyLeaderboardDefinitionByIndex(ref CopyLeaderboardDefinitionByIndexOptions options, out Definition? outLeaderboardDefinition)
		{
			outLeaderboardDefinition = null;
			return Result.Success;
		}

		public Result CopyLeaderboardDefinitionByLeaderboardId(ref CopyLeaderboardDefinitionByLeaderboardIdOptions options, out Definition? outLeaderboardDefinition)
		{
			outLeaderboardDefinition = null;
			return Result.Success;
		}

		public Result CopyLeaderboardRecordByIndex(ref CopyLeaderboardRecordByIndexOptions options, out LeaderboardRecord? outLeaderboardRecord)
		{
			outLeaderboardRecord = null;
			return Result.Success;
		}

		public Result CopyLeaderboardRecordByUserId(ref CopyLeaderboardRecordByUserIdOptions options, out LeaderboardRecord? outLeaderboardRecord)
		{
			outLeaderboardRecord = null;
			return Result.Success;
		}

		public Result CopyLeaderboardUserScoreByIndex(ref CopyLeaderboardUserScoreByIndexOptions options, out LeaderboardUserScore? outLeaderboardUserScore)
		{
			outLeaderboardUserScore = null;
			return Result.Success;
		}

		public Result CopyLeaderboardUserScoreByUserId(ref CopyLeaderboardUserScoreByUserIdOptions options, out LeaderboardUserScore? outLeaderboardUserScore)
		{
			outLeaderboardUserScore = null;
			return Result.Success;
		}

		public uint GetLeaderboardDefinitionCount(ref GetLeaderboardDefinitionCountOptions options)
		{
			return 0u;
		}

		public uint GetLeaderboardRecordCount(ref GetLeaderboardRecordCountOptions options)
		{
			return 0u;
		}

		public uint GetLeaderboardUserScoreCount(ref GetLeaderboardUserScoreCountOptions options)
		{
			return 0u;
		}

		public void QueryLeaderboardDefinitions(ref QueryLeaderboardDefinitionsOptions options, object clientData, OnQueryLeaderboardDefinitionsCompleteCallback completionDelegate)
		{
		}

		public void QueryLeaderboardRanks(ref QueryLeaderboardRanksOptions options, object clientData, OnQueryLeaderboardRanksCompleteCallback completionDelegate)
		{
		}

		public void QueryLeaderboardUserScores(ref QueryLeaderboardUserScoresOptions options, object clientData, OnQueryLeaderboardUserScoresCompleteCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryLeaderboardDefinitionsCompleteCallbackInternal))]
		internal static void OnQueryLeaderboardDefinitionsCompleteCallbackInternalImplementation(ref OnQueryLeaderboardDefinitionsCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryLeaderboardRanksCompleteCallbackInternal))]
		internal static void OnQueryLeaderboardRanksCompleteCallbackInternalImplementation(ref OnQueryLeaderboardRanksCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryLeaderboardUserScoresCompleteCallbackInternal))]
		internal static void OnQueryLeaderboardUserScoresCompleteCallbackInternalImplementation(ref OnQueryLeaderboardUserScoresCompleteCallbackInfoInternal data)
		{
		}
	}
}
