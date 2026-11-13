namespace Epic.OnlineServices.Ecom
{
	public struct QueryOwnershipCallbackInfo : ICallbackInfo
	{
		public Result ResultCode { get; set; }

		public object ClientData { get; set; }

		public EpicAccountId LocalUserId { get; set; }

		public ItemOwnership[] ItemOwnership { get; set; }

		public Result? GetResultCode()
		{
			return null;
		}

		internal void Set(ref QueryOwnershipCallbackInfoInternal other)
		{
		}
	}
}
