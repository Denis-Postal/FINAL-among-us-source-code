using System.Collections.Generic;
using AmongUs.GameOptions;
using Hazel;

public class LogicRoleSelectionHnS : LogicRoleSelection
{
	private HideAndSeekManager hnsManager;

	public LogicRoleSelectionHnS(HideAndSeekManager manager)
		: base(manager)
	{
		hnsManager = manager;
	}

	public override void OnGameStart()
	{
	}

	public override void OnGameEnd()
	{
	}

	public override void FixedUpdate()
	{
	}

	public override void OnDestroy()
	{
	}

	public override bool Serialize(MessageWriter writer)
	{
		return false;
	}

	public override void Deserialize(MessageReader reader)
	{
	}

	public override void AssignRolesForTeam(List<NetworkedPlayerInfo> players, IGameOptions opts, RoleTeamTypes team, int teamMax, RoleTypes? defaultRole)
	{
		if (team == RoleTeamTypes.Crewmate)
		{
			players.ForEach((NetworkedPlayerInfo player) =>
			{
				player.Object.RpcSetRole(RoleTypes.Engineer);
			});
			return;
		}
		bool flag = !AmongUsClient.Instance.IsGamePublic;
		if ((hnsManager.LogicOptionsHnS.HasImpostorPlayerID() && hnsManager.LogicOptionsHnS.ValidateImpostorPlayerID(players)) & flag)
		{
			NetworkedPlayerInfo networkedPlayerInfo = players.Find((NetworkedPlayerInfo p) => p.PlayerId == hnsManager.LogicOptionsHnS.ImpostorPlayerID());
			networkedPlayerInfo.Object.RpcSetRole(RoleTypes.Impostor);
			players.Remove(networkedPlayerInfo);
			if (teamMax > 1)
			{
				PickSeekersRoundRobin(players, teamMax - 1);
			}
		}
		else
		{
			PickSeekersRoundRobin(players, teamMax);
		}
	}

	private void PickSeekersRoundRobin(List<NetworkedPlayerInfo> players, int teamMax)
	{
		for (int i = 0; i < teamMax; i++)
		{
			if (players.Count <= 0)
			{
				break;
			}
			PseudoRandomList<NetworkedPlayerInfo> pseudoRandomList = new PseudoRandomList<NetworkedPlayerInfo>(AmongUsClient.Instance.GameId);
			pseudoRandomList.AddRange(players);
			for (int j = 0; j < GameData.RoundsPlayedInSession; j++)
			{
				pseudoRandomList.PickRandom();
			}
			NetworkedPlayerInfo networkedPlayerInfo = pseudoRandomList.PickRandom();
			networkedPlayerInfo.Object.RpcSetRole(RoleTypes.Impostor);
			players.Remove(networkedPlayerInfo);
		}
	}

	public override void OnPlayerDeath(PlayerControl player, bool assignGhostRole)
	{
		if (AmongUsClient.Instance.AmHost & assignGhostRole)
		{
			DestroyableSingleton<RoleManager>.Instance.AssignRoleOnDeath(player, specialRolesAllowed: false);
		}
	}
}
