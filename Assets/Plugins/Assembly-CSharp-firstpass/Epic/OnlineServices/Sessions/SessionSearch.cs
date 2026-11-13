using System;

namespace Epic.OnlineServices.Sessions
{
	public sealed class SessionSearch : Handle
	{
		public const int SessionsearchCopysearchresultbyindexApiLatest = 1;

		public const int SessionsearchFindApiLatest = 2;

		public const int SessionsearchGetsearchresultcountApiLatest = 1;

		public const int SessionsearchRemoveparameterApiLatest = 1;

		public const int SessionsearchSetmaxsearchresultsApiLatest = 1;

		public const int SessionsearchSetparameterApiLatest = 1;

		public const int SessionsearchSetsessionidApiLatest = 1;

		public const int SessionsearchSettargetuseridApiLatest = 1;

		public SessionSearch()
		{
		}

		public SessionSearch(IntPtr innerHandle)
		{
		}

		public Result CopySearchResultByIndex(ref SessionSearchCopySearchResultByIndexOptions options, out SessionDetails outSessionHandle)
		{
			outSessionHandle = null;
			return Result.Success;
		}

		public void Find(ref SessionSearchFindOptions options, object clientData, SessionSearchOnFindCallback completionDelegate)
		{
		}

		public uint GetSearchResultCount(ref SessionSearchGetSearchResultCountOptions options)
		{
			return 0u;
		}

		public void Release()
		{
		}

		public Result RemoveParameter(ref SessionSearchRemoveParameterOptions options)
		{
			return Result.Success;
		}

		public Result SetMaxResults(ref SessionSearchSetMaxResultsOptions options)
		{
			return Result.Success;
		}

		public Result SetParameter(ref SessionSearchSetParameterOptions options)
		{
			return Result.Success;
		}

		public Result SetSessionId(ref SessionSearchSetSessionIdOptions options)
		{
			return Result.Success;
		}

		public Result SetTargetUserId(ref SessionSearchSetTargetUserIdOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(SessionSearchOnFindCallbackInternal))]
		internal static void OnFindCallbackInternalImplementation(ref SessionSearchFindCallbackInfoInternal data)
		{
		}
	}
}
