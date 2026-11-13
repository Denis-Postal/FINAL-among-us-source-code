namespace Epic.OnlineServices.CustomInvites
{
	public struct SendCustomInviteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public ProductUserId[] TargetUserIds { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref SendCustomInviteCallbackInfoInternal other)
		{
		}
	}
}
