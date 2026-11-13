namespace Epic.OnlineServices.Sessions
{
	public struct RegisterPlayersCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public ProductUserId[] RegisteredPlayers { get; set; }

		public ProductUserId[] SanctionedPlayers { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref RegisterPlayersCallbackInfoInternal other)
		{
		}
	}
}
