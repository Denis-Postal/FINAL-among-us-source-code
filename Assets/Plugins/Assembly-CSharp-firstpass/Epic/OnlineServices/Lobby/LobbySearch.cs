using System;

namespace Epic.OnlineServices.Lobby
{
	public sealed class LobbySearch : Handle
	{
		public const int LobbysearchCopysearchresultbyindexApiLatest = 1;

		public const int LobbysearchFindApiLatest = 1;

		public const int LobbysearchGetsearchresultcountApiLatest = 1;

		public const int LobbysearchRemoveparameterApiLatest = 1;

		public const int LobbysearchSetlobbyidApiLatest = 1;

		public const int LobbysearchSetmaxresultsApiLatest = 1;

		public const int LobbysearchSetparameterApiLatest = 1;

		public const int LobbysearchSettargetuseridApiLatest = 1;

		public LobbySearch()
		{
		}

		public LobbySearch(IntPtr innerHandle)
		{
		}

		public Result CopySearchResultByIndex(ref LobbySearchCopySearchResultByIndexOptions options, out LobbyDetails outLobbyDetailsHandle)
		{
			outLobbyDetailsHandle = null;
			return Result.Success;
		}

		public void Find(ref LobbySearchFindOptions options, object clientData, LobbySearchOnFindCallback completionDelegate)
		{
		}

		public uint GetSearchResultCount(ref LobbySearchGetSearchResultCountOptions options)
		{
			return 0u;
		}

		public void Release()
		{
		}

		public Result RemoveParameter(ref LobbySearchRemoveParameterOptions options)
		{
			return Result.Success;
		}

		public Result SetLobbyId(ref LobbySearchSetLobbyIdOptions options)
		{
			return Result.Success;
		}

		public Result SetMaxResults(ref LobbySearchSetMaxResultsOptions options)
		{
			return Result.Success;
		}

		public Result SetParameter(ref LobbySearchSetParameterOptions options)
		{
			return Result.Success;
		}

		public Result SetTargetUserId(ref LobbySearchSetTargetUserIdOptions options)
		{
			return Result.Success;
		}

		[MonoPInvokeCallback(typeof(LobbySearchOnFindCallbackInternal))]
		internal static void OnFindCallbackInternalImplementation(ref LobbySearchFindCallbackInfoInternal data)
		{
		}
	}
}
