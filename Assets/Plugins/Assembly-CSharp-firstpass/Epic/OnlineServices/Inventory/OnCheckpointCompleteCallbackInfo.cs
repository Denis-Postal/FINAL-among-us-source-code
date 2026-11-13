namespace Epic.OnlineServices.Inventory
{
	public struct OnCheckpointCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnCheckpointCompleteCallbackInfoInternal other)
		{
		}
	}
}
