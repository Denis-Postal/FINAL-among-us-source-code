namespace Epic.OnlineServices.Inventory
{
	public struct OnOpenCompleteCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public InventoryItem InventoryItemHandle { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref OnOpenCompleteCallbackInfoInternal other)
		{
		}
	}
}
