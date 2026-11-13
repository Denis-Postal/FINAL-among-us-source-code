using System.Collections.Generic;

public class MeetingRoomManager : IDisconnectHandler
{
	public static readonly MeetingRoomManager Instance= new MeetingRoomManager();

	private PlayerControl reporter;

	private NetworkedPlayerInfo target;

	public bool IsPersistent => false;

	public void AssignSelf(PlayerControl reporter, NetworkedPlayerInfo target)
	{
		this.reporter = reporter;
		this.target = target;
		AmongUsClient.Instance.DisconnectHandlers.AddUnique(this);
	}

	public void RemoveSelf()
	{
		AmongUsClient.Instance.DisconnectHandlers.Remove(this);
	}

	public void HandleDisconnect(PlayerControl pc, DisconnectReasons reason)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			reporter.CmdReportDeadBody(target);
		}
	}

	public void HandleDisconnect()
	{
		HandleDisconnect(null, DisconnectReasons.Unknown);
	}

	static MeetingRoomManager()
	{ }
}
