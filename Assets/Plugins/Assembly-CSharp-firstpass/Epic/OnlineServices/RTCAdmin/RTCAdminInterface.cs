using System;

namespace Epic.OnlineServices.RTCAdmin
{
	public sealed class RTCAdminInterface : Handle
	{
		public const int CopyusertokenbyindexApiLatest = 2;

		public const int CopyusertokenbyuseridApiLatest = 2;

		public const int KickApiLatest = 1;

		public const int QueryjoinroomtokenApiLatest = 2;

		public const int SetparticipanthardmuteApiLatest = 1;

		public const int UsertokenApiLatest = 1;

		public RTCAdminInterface()
		{
		}

		public RTCAdminInterface(IntPtr innerHandle)
		{
		}

		public Result CopyUserTokenByIndex(ref CopyUserTokenByIndexOptions options, out UserToken? outUserToken)
		{
			outUserToken = null;
			return Result.Success;
		}

		public Result CopyUserTokenByUserId(ref CopyUserTokenByUserIdOptions options, out UserToken? outUserToken)
		{
			outUserToken = null;
			return Result.Success;
		}

		public void Kick(ref KickOptions options, object clientData, OnKickCompleteCallback completionDelegate)
		{
		}

		public void QueryJoinRoomToken(ref QueryJoinRoomTokenOptions options, object clientData, OnQueryJoinRoomTokenCompleteCallback completionDelegate)
		{
		}

		public void SetParticipantHardMute(ref SetParticipantHardMuteOptions options, object clientData, OnSetParticipantHardMuteCompleteCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnKickCompleteCallbackInternal))]
		internal static void OnKickCompleteCallbackInternalImplementation(ref KickCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryJoinRoomTokenCompleteCallbackInternal))]
		internal static void OnQueryJoinRoomTokenCompleteCallbackInternalImplementation(ref QueryJoinRoomTokenCompleteCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnSetParticipantHardMuteCompleteCallbackInternal))]
		internal static void OnSetParticipantHardMuteCompleteCallbackInternalImplementation(ref SetParticipantHardMuteCompleteCallbackInfoInternal data)
		{
		}
	}
}
