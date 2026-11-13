using System;

namespace Epic.OnlineServices.Achievements
{
	public sealed class AchievementsInterface : Handle
	{
		public const int AchievementUnlocktimeUndefined = -1;

		public const int AddnotifyachievementsunlockedApiLatest = 1;

		public const int Addnotifyachievementsunlockedv2ApiLatest = 2;

		public const int Copyachievementdefinitionv2ByachievementidApiLatest = 2;

		public const int Copyachievementdefinitionv2ByindexApiLatest = 2;

		public const int CopydefinitionbyachievementidApiLatest = 1;

		public const int CopydefinitionbyindexApiLatest = 1;

		public const int Copydefinitionv2ByachievementidApiLatest = 2;

		public const int Copydefinitionv2ByindexApiLatest = 2;

		public const int CopyplayerachievementbyachievementidApiLatest = 2;

		public const int CopyplayerachievementbyindexApiLatest = 2;

		public const int CopyunlockedachievementbyachievementidApiLatest = 1;

		public const int CopyunlockedachievementbyindexApiLatest = 1;

		public const int DefinitionApiLatest = 1;

		public const int Definitionv2ApiLatest = 2;

		public const int GetachievementdefinitioncountApiLatest = 1;

		public const int GetplayerachievementcountApiLatest = 1;

		public const int GetunlockedachievementcountApiLatest = 1;

		public const int PlayerachievementApiLatest = 2;

		public const int PlayerstatinfoApiLatest = 1;

		public const int QuerydefinitionsApiLatest = 3;

		public const int QueryplayerachievementsApiLatest = 2;

		public const int StatthresholdApiLatest = 1;

		public const int StatthresholdsApiLatest = 1;

		public const int UnlockachievementsApiLatest = 1;

		public const int UnlockedachievementApiLatest = 1;

		public AchievementsInterface()
		{
		}

		public AchievementsInterface(IntPtr innerHandle)
		{
		}

		public ulong AddNotifyAchievementsUnlocked(ref AddNotifyAchievementsUnlockedOptions options, object clientData, OnAchievementsUnlockedCallback notificationFn)
		{
			return 0uL;
		}

		public ulong AddNotifyAchievementsUnlockedV2(ref AddNotifyAchievementsUnlockedV2Options options, object clientData, OnAchievementsUnlockedCallbackV2 notificationFn)
		{
			return 0uL;
		}

		public Result CopyAchievementDefinitionByAchievementId(ref CopyAchievementDefinitionByAchievementIdOptions options, out Definition? outDefinition)
		{
			outDefinition = null;
			return Result.Success;
		}

		public Result CopyAchievementDefinitionByIndex(ref CopyAchievementDefinitionByIndexOptions options, out Definition? outDefinition)
		{
			outDefinition = null;
			return Result.Success;
		}

		public Result CopyAchievementDefinitionV2ByAchievementId(ref CopyAchievementDefinitionV2ByAchievementIdOptions options, out DefinitionV2? outDefinition)
		{
			outDefinition = null;
			return Result.Success;
		}

		public Result CopyAchievementDefinitionV2ByIndex(ref CopyAchievementDefinitionV2ByIndexOptions options, out DefinitionV2? outDefinition)
		{
			outDefinition = null;
			return Result.Success;
		}

		public Result CopyPlayerAchievementByAchievementId(ref CopyPlayerAchievementByAchievementIdOptions options, out PlayerAchievement? outAchievement)
		{
			outAchievement = null;
			return Result.Success;
		}

		public Result CopyPlayerAchievementByIndex(ref CopyPlayerAchievementByIndexOptions options, out PlayerAchievement? outAchievement)
		{
			outAchievement = null;
			return Result.Success;
		}

		public Result CopyUnlockedAchievementByAchievementId(ref CopyUnlockedAchievementByAchievementIdOptions options, out UnlockedAchievement? outAchievement)
		{
			outAchievement = null;
			return Result.Success;
		}

		public Result CopyUnlockedAchievementByIndex(ref CopyUnlockedAchievementByIndexOptions options, out UnlockedAchievement? outAchievement)
		{
			outAchievement = null;
			return Result.Success;
		}

		public uint GetAchievementDefinitionCount(ref GetAchievementDefinitionCountOptions options)
		{
			return 0u;
		}

		public uint GetPlayerAchievementCount(ref GetPlayerAchievementCountOptions options)
		{
			return 0u;
		}

		public uint GetUnlockedAchievementCount(ref GetUnlockedAchievementCountOptions options)
		{
			return 0u;
		}

		public void QueryDefinitions(ref QueryDefinitionsOptions options, object clientData, OnQueryDefinitionsCompleteCallback completionDelegate)
		{
		}

		public void QueryPlayerAchievements(ref QueryPlayerAchievementsOptions options, object clientData, OnQueryPlayerAchievementsCompleteCallback completionDelegate)
		{
		}

		public void RemoveNotifyAchievementsUnlocked(ulong inId)
		{
		}

		public void UnlockAchievements(ref UnlockAchievementsOptions options, object clientData, OnUnlockAchievementsCompleteCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnAchievementsUnlockedCallbackInternal))]
		internal static void OnAchievementsUnlockedCallbackInternalImplementation(ref OnAchievementsUnlockedCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnAchievementsUnlockedCallbackV2Internal))]
		internal static void OnAchievementsUnlockedCallbackV2InternalImplementation(ref OnAchievementsUnlockedCallbackV2InfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryDefinitionsCompleteCallbackInternal))]
		internal static void OnQueryDefinitionsCompleteCallbackInternalImplementation(ref OnQueryDefinitionsCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryPlayerAchievementsCompleteCallbackInternal))]
		internal static void OnQueryPlayerAchievementsCompleteCallbackInternalImplementation(ref OnQueryPlayerAchievementsCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnUnlockAchievementsCompleteCallbackInternal))]
		internal static void OnUnlockAchievementsCompleteCallbackInternalImplementation(ref OnUnlockAchievementsCompleteCallbackInfoInternal data)
		{
		}
	}
}
