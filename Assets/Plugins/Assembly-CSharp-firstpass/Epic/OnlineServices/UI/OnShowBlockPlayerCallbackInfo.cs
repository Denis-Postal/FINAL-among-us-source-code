namespace Epic.OnlineServices.UI
{
	public struct OnShowBlockPlayerCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public EpicAccountId TargetUserId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnShowBlockPlayerCallbackInfoInternal other)
		{
		}
	}
}
