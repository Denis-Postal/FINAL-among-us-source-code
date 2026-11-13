public class KillOverlayInitData
{
	public NetworkedPlayerInfo.PlayerOutfit killerOutfit;

	public PlayerBodyTypes killerBodyType;

	public NetworkedPlayerInfo.PlayerOutfit victimOutfit;

	public PlayerBodyTypes victimBodyType;

	public KillOverlayInitData(NetworkedPlayerInfo.PlayerOutfit killerOutfit, PlayerBodyTypes killerBodyType, NetworkedPlayerInfo.PlayerOutfit victimOutfit, PlayerBodyTypes victimBodyType)
	{
		this.killerOutfit = killerOutfit;
		this.killerBodyType = killerBodyType;
		this.victimOutfit = victimOutfit;
		this.victimBodyType = victimBodyType;
	}

	public KillOverlayInitData(NetworkedPlayerInfo killer, NetworkedPlayerInfo victim)
	{
		PlayerControl playerControl = PlayerControl.AllPlayerControls.Find((PlayerControl p) => p.PlayerId == killer.PlayerId);
		killerOutfit = playerControl.CurrentOutfit;
		killerBodyType = playerControl.BodyType;
		PlayerControl playerControl2 = PlayerControl.AllPlayerControls.Find((PlayerControl p) => p.PlayerId == victim.PlayerId);
		victimOutfit = playerControl2.CurrentOutfit;
		victimBodyType = playerControl2.BodyType;
	}
}
