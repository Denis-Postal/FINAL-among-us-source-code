using System;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public static class PlayerNameColor
{
	// TODO(verify): rebuilt from decomp9 + 2019 behaviour. Decomp9 Get(role) had a lost virtual call and an apparently inverted
	// team compare; implemented as: same team as the local player -> the other role's NameColor (impostors see impostors red),
	// otherwise white.
	public static Color Get(NetworkedPlayerInfo pc)
	{
		return Get(pc?.Role);
	}

	public static void Set(PlayerControl pc)
	{
		Set(pc, pc.Data?.Role);
	}

	public static void Set(NetworkedPlayerInfo playerInfo, TextMeshPro name)
	{
		Set(name, playerInfo?.Role);
	}

	public static void SetForRoleDirectly(PlayerControl player, RoleBehaviour role)
	{
		if (player.AmOwner)
		{
			player.cosmetics.SetNameColor(role.NameColor);
		}
	}

	private static Color Get(RoleBehaviour otherPlayerRole)
	{
		if (PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.Data == null || PlayerControl.LocalPlayer.Data.Role == null || otherPlayerRole == null)
		{
			return Color.white;
		}
		bool flag = PlayerControl.LocalPlayer.Data.Role.TeamType == otherPlayerRole.TeamType;
		bool revealTeams = GameManager.Instance.RevealTeams;
		if (!(flag | revealTeams))
		{
			return Color.white;
		}
		return otherPlayerRole.NameColor;
	}

	private static void Set(PlayerControl otherPlayer, RoleBehaviour otherPlayerRole)
	{
		otherPlayer.cosmetics.SetNameColor(Get(otherPlayerRole));
	}

	private static void Set(TextMeshPro nameText, RoleBehaviour otherPlayerRole)
	{
		nameText.color = Get(otherPlayerRole);
	}
}
