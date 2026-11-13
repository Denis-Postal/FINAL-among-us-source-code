using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using InnerNet;
using UnityEngine;

public class RoleManager : DestroyableSingleton<RoleManager>
{
	public class RoleAssignmentData
	{
		public RoleBehaviour Role;

		public int Count;

		public int Chance;

		public RoleAssignmentData(RoleBehaviour role, int count, int chance)
		{
			Role = role;
			Count = count;
			Chance = chance;
		}
	}

	private static readonly HashSet<RoleTypes> GhostRoles = new HashSet<RoleTypes>
	{
		RoleTypes.GuardianAngel,
		RoleTypes.CrewmateGhost,
		RoleTypes.ImpostorGhost,
		RoleTypes.SpiritGuide
	};

	public List<RoleBehaviour> AllRoles;

	public RoleEffectAnimation shapeshiftAnim;

	public RoleEffectAnimation protectAnim;

	public RoleEffectAnimation protectLoopAnim;

	public RoleEffectAnimation vanish_ChargeAnim;

	public RoleEffectAnimation vanish_PoofAnim;

	public RoleEffectAnimation appear_PoofAnim;

	public override void Awake()
	{
		base.Awake();
		AllRoles.AddRange(Resources.LoadAll<RoleBehaviour>("RolePrototypes"));
	}

	public RoleBehaviour GetRole(RoleTypes roleType)
	{
		foreach (RoleBehaviour allRole in AllRoles)
		{
			if (allRole.Role == roleType)
			{
				return allRole;
			}
		}
		return null;
	}

	public void SetRole(PlayerControl targetPlayer, RoleTypes roleType)
	{
		if (!targetPlayer)
		{
			return;
		}
		NetworkedPlayerInfo data = targetPlayer.Data;
		if (data == null)
		{
			Debug.LogError("It shouldn't be possible, but " + targetPlayer.name + " still doesn't have PlayerData during role selection.");
			return;
		}
		if ((bool)data.Role)
		{
			data.Role.Deinitialize(targetPlayer);
			Object.Destroy(data.Role.gameObject);
		}
		RoleBehaviour roleBehaviour = Object.Instantiate(AllRoles.First((RoleBehaviour r) => r.Role == roleType), data.gameObject.transform);
		roleBehaviour.Initialize(targetPlayer);
		targetPlayer.Data.Role = roleBehaviour;
		targetPlayer.Data.RoleType = roleType;
		if (roleType != RoleTypes.ImpostorGhost && roleType != RoleTypes.CrewmateGhost)
		{
			targetPlayer.Data.RoleWhenAlive = roleType;
		}
		roleBehaviour.AdjustTasks(targetPlayer);
		if (roleBehaviour.IsDead && !targetPlayer.Data.IsDead)
		{
			targetPlayer.Die(DeathReason.Kill, assignGhostRole: false);
		}
		else if (!roleBehaviour.IsDead && targetPlayer.Data.IsDead)
		{
			targetPlayer.Revive();
		}
	}

	public void SelectRoles()
	{
		List<ClientData> list = new List<ClientData>();
		AmongUsClient.Instance.GetAllClients(list);
		List<NetworkedPlayerInfo> list2 = (from c in list
			where c.Character != null
			where c.Character.Data != null
			where !c.Character.Data.Disconnected && !c.Character.Data.IsDead
			orderby c.Id
			select c.Character.Data).ToList();
		foreach (NetworkedPlayerInfo allPlayer in GameData.Instance.AllPlayers)
		{
			if (allPlayer.Object != null && allPlayer.Object.isDummy)
			{
				list2.Add(allPlayer);
			}
		}
		IGameOptions currentGameOptions = GameOptionsManager.Instance.CurrentGameOptions;
		int numImpostors = GameOptionsManager.Instance.CurrentGameOptions.GetAdjustedNumImpostors(list2.Count);
		DebugRoleAssignments(list2, ref numImpostors);
		GameManager.Instance.LogicRoleSelection.AssignRolesForTeam(list2, currentGameOptions, RoleTeamTypes.Impostor, numImpostors, RoleTypes.Impostor);
		GameManager.Instance.LogicRoleSelection.AssignRolesForTeam(list2, currentGameOptions, RoleTeamTypes.Crewmate, int.MaxValue, RoleTypes.Crewmate);
	}

	public void AssignRoleOnDeath(PlayerControl player, bool specialRolesAllowed)
	{
		if (!(player == null) && player.Data.IsDead)
		{
			if (specialRolesAllowed)
			{
				TryAssignSpecialGhostRoles(player, player.Data.Role.IsImpostor);
			}
			if (!IsGhostRole(player.Data.Role.Role))
			{
				player.RpcSetRole(player.Data.Role.DefaultGhostRole);
			}
		}
	}

	private void DebugRoleAssignments(List<NetworkedPlayerInfo> players, ref int numImpostors)
	{
	}

	private static void TryAssignSpecialGhostRoles(PlayerControl player, bool impostorRoles)
	{
		IRoleOptionsCollection roleOptions = GameOptionsManager.Instance.CurrentGameOptions.RoleOptions;
		List<RoleTypes> list = ((!impostorRoles) ? SpecialCrewmateGhostRoles() : SpecialImpostorGhostRoles());
		list.Shuffle();
		foreach (RoleTypes specialGhostRole in list)
		{
			int num = PlayerControl.AllPlayerControls.Count((PlayerControl p) => p.Data.Role.Role == specialGhostRole);
			if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
			{
				player.RpcSetRole(specialGhostRole);
				break;
			}
			if (num < roleOptions.GetNumPerGame(specialGhostRole))
			{
				int chancePerGame = roleOptions.GetChancePerGame(specialGhostRole);
				if (HashRandom.Next(101) < chancePerGame)
				{
					player.RpcSetRole(specialGhostRole);
					break;
				}
			}
		}
	}

	public static bool IsGhostRole(RoleTypes role)
	{
		return GhostRoles.Contains(role);
	}

	private static List<RoleTypes> SpecialCrewmateGhostRoles()
	{
		return new HashSet<RoleTypes>
		{
			RoleTypes.GuardianAngel,
			RoleTypes.SpiritGuide
		}.ToList();
	}

	private static List<RoleTypes> SpecialImpostorGhostRoles()
	{
		return new HashSet<RoleTypes>().ToList();
	}

	public static bool IsImpostorRole(RoleTypes roleType)
	{
		RoleBehaviour roleBehaviour = DestroyableSingleton<RoleManager>.Instance.AllRoles.First((RoleBehaviour r) => r.Role == roleType);
		if (roleBehaviour != null)
		{
			return roleBehaviour.TeamType == RoleTeamTypes.Impostor;
		}
		return false;
	}
}
