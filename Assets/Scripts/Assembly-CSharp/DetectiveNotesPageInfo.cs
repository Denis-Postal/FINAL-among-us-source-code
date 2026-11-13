using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

[Serializable]
public class DetectiveNotesPageInfo
{
	public bool locationSet;

	public string locationName = string.Empty;

	public bool prepositionSet;

	public StringNames prepositionStringName;

	public NetworkedPlayerInfo victimPlayer;

	public List<DetectiveSuspect> suspects;

	public bool impostorSet;

	public string impostorString;

	public DetectiveNotesPageInfo(NetworkedPlayerInfo _victimPlayer)
	{
		victimPlayer = _victimPlayer;
		suspects = new List<DetectiveSuspect>();
	}

	public void AddSuspect(NetworkedPlayerInfo suspect)
	{
		foreach (DetectiveSuspect suspect2 in suspects)
		{
			if (suspect2.playerInfo != suspect && suspect.Object.ShapeshiftedPlayerID() == -1)
			{
				continue;
			}
			if (suspect.Object.ShapeshiftedPlayerID() != -1)
			{
				if (suspect2.playerInfo.PlayerId == suspect.Object.ShapeshiftedPlayerID() && suspect2.shapeshifterPlayerInfo?.PlayerId == suspect.Object.PlayerId)
				{
					return;
				}
			}
			else if (suspect2.shapeshifterPlayerInfo == null)
			{
				return;
			}
		}
		if (!((float)suspects.Count >= GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.DetectiveSuspectLimit)))
		{
			DataManager.Player.Stats.IncrementStat(StatID.Role_Detective_CrewmatesQuestioned);
			if (suspect.Object.ShapeshiftedPlayerID() != -1)
			{
				suspects.Add(new DetectiveSuspect(GameData.Instance.GetPlayerById((byte)suspect.Object.ShapeshiftedPlayerID()), suspect));
			}
			else
			{
				suspects.Add(new DetectiveSuspect(suspect));
			}
			if ((float)suspects.Count >= GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.DetectiveSuspectLimit))
			{
				DataManager.Player.Stats.IncrementStat(StatID.Role_Detective_CasesFilled);
			}
			Debug.Log(suspect.PlayerName + " was added to the suspect list");
		}
	}

	public void SetPreposition(StringNames _prepositionString)
	{
		prepositionSet = true;
		prepositionStringName = _prepositionString;
	}

	public void SetLocation(string _locationName)
	{
		locationSet = true;
		locationName = _locationName;
	}

	public void SetImpostorType(string impString)
	{
		impostorSet = true;
		impostorString = impString;
	}
}
