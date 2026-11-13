namespace Epic.OnlineServices.Lobby
{
	public struct LobbyInviteAcceptedCallbackInfo : ICallbackInfo
	{
		public object ClientData { get; set; }

		public Utf8String InviteId { get; set; }

		public ProductUserId LocalUserId { get; set; }

		public ProductUserId TargetUserId { get; set; }

		public Utf8String LobbyId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref LobbyInviteAcceptedCallbackInfoInternal other)
		{
		}
	}
}
