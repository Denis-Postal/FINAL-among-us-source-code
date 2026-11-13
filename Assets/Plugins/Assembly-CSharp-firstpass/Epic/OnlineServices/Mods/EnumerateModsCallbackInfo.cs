namespace Epic.OnlineServices.Mods
{
	public struct EnumerateModsCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public object ClientData { get; set; }

		public ModEnumerationType Type { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref EnumerateModsCallbackInfoInternal other)
		{
		}
	}
}
