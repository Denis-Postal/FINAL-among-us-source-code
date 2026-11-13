namespace Epic.OnlineServices.Presence
{
	public struct SetPresenceCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref SetPresenceCallbackInfoInternal other)
		{
		}
	}
}
