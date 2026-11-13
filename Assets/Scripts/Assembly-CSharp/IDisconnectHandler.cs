public interface IDisconnectHandler
{
	bool IsPersistent { get; }

	void HandleDisconnect(PlayerControl pc, DisconnectReasons reason);

	void HandleDisconnect();
}
