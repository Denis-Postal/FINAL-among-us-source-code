namespace Epic.OnlineServices.Inventory
{
	public struct CheckpointOptions
	{
		public ProductUserId LocalUserId { get; set; }

		public ProductUserId TargetUserId { get; set; }

		public Utf8String InventoryName { get; set; }
	}
}
