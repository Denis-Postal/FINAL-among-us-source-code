namespace Epic.OnlineServices.Lobby
{
	public struct QueryInvitesCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref QueryInvitesCallbackInfoInternal other)
		{
		}
	}
}
