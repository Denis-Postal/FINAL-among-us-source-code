namespace Epic.OnlineServices.RTCAdmin
{
	public struct SetParticipantHardMuteCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref SetParticipantHardMuteCompleteCallbackInfoInternal other)
		{
		}
	}
}
