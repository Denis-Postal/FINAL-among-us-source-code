namespace Epic.OnlineServices.AntiCheatCommon
{
	public enum AntiCheatCommonClientActionReason
	{
		Invalid = 0,
		InternalError = 1,
		InvalidMessage = 2,
		AuthenticationFailed = 3,
		NullClient = 4,
		HeartbeatTimeout = 5,
		ClientViolation = 6,
		BackendViolation = 7,
		TemporaryCooldown = 8,
		TemporaryBanned = 9,
		PermanentBanned = 10
	}
}
