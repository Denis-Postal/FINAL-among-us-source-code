using System.Collections.Generic;
using Cpp2ILInjected;

public class DetectiveDeadBodyInfo
{
	public byte victimID;

	public Dictionary<byte, string> PlayerLocationsAtDeath= new Dictionary<byte, string>();

	public DetectiveDeadBodyInfo(byte _victimID)
	{
		victimID = _victimID;
	}

	public string GetPlayerLocation(byte playerID)
	{
		return PlayerLocationsAtDeath.GetValueOrDefault(playerID, "n/a");
	}
}
