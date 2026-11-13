using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveLocation : MonoBehaviour
{
	public SystemTypes associatedRoom;

	public List<NetworkedPlayerInfo> playersInRoom= new List<NetworkedPlayerInfo>();

	public bool IsPlayerInRoom(NetworkedPlayerInfo player)
	{
		return playersInRoom.Contains(player);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponentInChildren<PlayerControl>() != null)
		{
			playersInRoom.Add(other.GetComponentInChildren<PlayerControl>().Data);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.GetComponentInChildren<PlayerControl>() != null)
		{
			playersInRoom.Remove(other.GetComponentInChildren<PlayerControl>().Data);
		}
	}

	public DetectiveLocation()
	{ }
}
