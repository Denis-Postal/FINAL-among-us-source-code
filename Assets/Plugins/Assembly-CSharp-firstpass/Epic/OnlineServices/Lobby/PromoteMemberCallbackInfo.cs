namespace Epic.OnlineServices.Lobby
{
	public struct PromoteMemberCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Utf8String LobbyId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref PromoteMemberCallbackInfoInternal other)
		{
		}
	}
}
