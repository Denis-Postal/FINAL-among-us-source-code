namespace Epic.OnlineServices.Mods
{
	public struct UninstallModCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public object ClientData { get; set; }

		public ModIdentifier? Mod { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref UninstallModCallbackInfoInternal other)
		{
		}
	}
}
