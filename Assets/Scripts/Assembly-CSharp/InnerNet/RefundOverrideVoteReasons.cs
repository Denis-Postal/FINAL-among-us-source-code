namespace InnerNet
{
	public enum RefundOverrideVoteReasons : byte
	{
		Invalid,
		OverrideAlreadyActive,
		TargetDoesNotExist,
		OverriderDisconnected,
		TargetDisconnected
	}
}
