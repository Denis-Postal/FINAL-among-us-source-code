namespace Epic.OnlineServices.Sessions
{
	public struct UpdateSessionCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Utf8String SessionName { get; set; }

		public Utf8String SessionId { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref UpdateSessionCallbackInfoInternal other)
		{
		}
	}
}
