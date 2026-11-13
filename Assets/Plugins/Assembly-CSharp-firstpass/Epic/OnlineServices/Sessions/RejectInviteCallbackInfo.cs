namespace Epic.OnlineServices.Sessions
{
	public struct RejectInviteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref RejectInviteCallbackInfoInternal other)
		{
		}
	}
}
