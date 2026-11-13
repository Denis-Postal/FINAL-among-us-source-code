using System;

namespace Epic.OnlineServices.Sanctions
{
	public sealed class SanctionsInterface : Handle
	{
		public const int CopyplayersanctionbyindexApiLatest = 1;

		public const int CreateplayersanctionappealApiLatest = 1;

		public const int GetplayersanctioncountApiLatest = 1;

		public const int PlayersanctionApiLatest = 2;

		public const int QueryactiveplayersanctionsApiLatest = 2;

		public SanctionsInterface()
		{
		}

		public SanctionsInterface(IntPtr innerHandle)
		{
		}

		public Result CopyPlayerSanctionByIndex(ref CopyPlayerSanctionByIndexOptions options, out PlayerSanction? outSanction)
		{
			outSanction = null;
			return Result.Success;
		}

		public void CreatePlayerSanctionAppeal(ref CreatePlayerSanctionAppealOptions options, object clientData, CreatePlayerSanctionAppealCallback completionDelegate)
		{
		}

		public uint GetPlayerSanctionCount(ref GetPlayerSanctionCountOptions options)
		{
			return 0u;
		}

		public void QueryActivePlayerSanctions(ref QueryActivePlayerSanctionsOptions options, object clientData, OnQueryActivePlayerSanctionsCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(CreatePlayerSanctionAppealCallbackInternal))]
		internal static void CreatePlayerSanctionAppealCallbackInternalImplementation(ref CreatePlayerSanctionAppealCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryActivePlayerSanctionsCallbackInternal))]
		internal static void OnQueryActivePlayerSanctionsCallbackInternalImplementation(ref QueryActivePlayerSanctionsCallbackInfoInternal data)
		{
		}
	}
}
