using System;

[Serializable]
public struct DetectiveSuspect
{
	public NetworkedPlayerInfo playerInfo;

	public NetworkedPlayerInfo shapeshifterPlayerInfo;

	public DetectiveSuspect(NetworkedPlayerInfo _playerInfo, NetworkedPlayerInfo _shapeshifterPlayerInfo = null)
	{
		playerInfo = _playerInfo;
		shapeshifterPlayerInfo = _shapeshifterPlayerInfo;
	}
}
