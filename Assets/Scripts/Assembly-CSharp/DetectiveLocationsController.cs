using System;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using UnityEngine;

public class DetectiveLocationsController : MonoBehaviour
{
	public static DetectiveLocationsController Instance;

	public List<DetectiveLocation> DetectiveLocations= new List<DetectiveLocation>();

	public List<DetectiveDeadBodyInfo> DeadBodyInfos= new List<DetectiveDeadBodyInfo>();

	public SystemTypes DefaultLocation;

	private void Awake()
	{
		Instance = this;
		DetectiveLocations = GetComponentsInChildren<DetectiveLocation>().ToList();
	}

	public DetectiveLocation GetPlayerLocation(NetworkedPlayerInfo player)
	{
		DetectiveLocation result = null;
		foreach (DetectiveLocation detectiveLocation in DetectiveLocations)
		{
			if (detectiveLocation.IsPlayerInRoom(player))
			{
				result = detectiveLocation;
				break;
			}
		}
		return result;
	}

	public void AddBodyInfo(DetectiveDeadBodyInfo bodyInfo)
	{
		DeadBodyInfos.Add(bodyInfo);
	}

	public DetectiveLocationsController()
	{ }
}
