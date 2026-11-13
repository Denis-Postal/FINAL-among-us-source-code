using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using Hazel;

public class LogicRoleSelectionNormal : LogicRoleSelection
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<_003C_003Ef__AnonymousType0<RoleBehaviour, int>, bool> _003C_003E9__7_2;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CAssignRolesForTeam_003Eb__7_2(_003C_003Ef__AnonymousType0<RoleBehaviour, int> _003C_003Eh__TransparentIdentifier0)
		{
			return _003C_003Eh__TransparentIdentifier0.chance > 0 && _003C_003Eh__TransparentIdentifier0.chance < 100;
		}
	}

	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public RoleTeamTypes team;

		public IRoleOptionsCollection roleOptions;

		public Func<RoleBehaviour, bool> _003C_003E9__4;

		public Func<RoleBehaviour, RoleManager.RoleAssignmentData> _003C_003E9__5;

		internal bool _003CAssignRolesForTeam_003Eb__0(RoleBehaviour role)
		{
			return role.TeamType == team && !RoleManager.IsGhostRole(role.Role);
		}

		internal bool _003CAssignRolesForTeam_003Eb__4(RoleBehaviour role)
		{
			return roleOptions.GetChancePerGame(role.Role) == 100;
		}

		internal RoleManager.RoleAssignmentData _003CAssignRolesForTeam_003Eb__5(RoleBehaviour role)
		{
			if ((object)role != null && roleOptions != null)
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1800D26A0");
				int count = default;
				return new RoleManager.RoleAssignmentData(role, count, 100);
			}
			return (RoleManager.RoleAssignmentData)(object)new NullReferenceException();
		}

		internal _003C_003Ef__AnonymousType0<RoleBehaviour, int> _003CAssignRolesForTeam_003Eb__1(RoleBehaviour role)
		{
			if ((object)role != null && roleOptions != null)
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1800D26A0");
				int chance = default;
				return new _003C_003Ef__AnonymousType0<RoleBehaviour, int>(role, chance);
			}
			return (_003C_003Ef__AnonymousType0<RoleBehaviour, int>)(object)new NullReferenceException();
		}

		internal RoleManager.RoleAssignmentData _003CAssignRolesForTeam_003Eb__3(_003C_003Ef__AnonymousType0<RoleBehaviour, int> _003C_003Eh__TransparentIdentifier0)
		{
			return new RoleManager.RoleAssignmentData(_003C_003Eh__TransparentIdentifier0.role, roleOptions.GetNumPerGame(_003C_003Eh__TransparentIdentifier0.role.Role), _003C_003Eh__TransparentIdentifier0.chance);
		}
	}

	public LogicRoleSelectionNormal(GameManager manager)
		: base(manager)
	{
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

	public unsafe override void AssignRolesForTeam(List<NetworkedPlayerInfo> players, IGameOptions opts, RoleTeamTypes team, int teamMax, RoleTypes? defaultRole)
	{
		int rolesAssigned = 0;
		IEnumerable<RoleBehaviour> source = DestroyableSingleton<RoleManager>.Instance.AllRoles.Where((RoleBehaviour role) => role.TeamType == team && !RoleManager.IsGhostRole(role.Role));
		List<RoleTypes> list = new List<RoleTypes>();
		IRoleOptionsCollection roleOptions = opts.RoleOptions;
		foreach (RoleManager.RoleAssignmentData item in from role in source
			where roleOptions.GetChancePerGame(role.Role) == 100
			select new RoleManager.RoleAssignmentData(role, roleOptions.GetNumPerGame(role.Role), 100))
		{
			while (item.Count-- > 0)
			{
				list.Add(item.Role.Role);
			}
		}
		AssignRolesFromList(players, teamMax, list, ref rolesAssigned);
		List<RoleManager.RoleAssignmentData> list2 = (from role in source
			let chance = roleOptions.GetChancePerGame(role.Role)
			where chance > 0 && chance < 100
			select new RoleManager.RoleAssignmentData(role, roleOptions.GetNumPerGame(role.Role), chance)).ToList();
		list.Clear();
		foreach (RoleManager.RoleAssignmentData item2 in list2)
		{
			for (int num = 0; num < item2.Count; num++)
			{
				if (HashRandom.Next(101) < item2.Chance)
				{
					list.Add(item2.Role.Role);
				}
			}
		}
		AssignRolesFromList(players, teamMax, list, ref rolesAssigned);
		if (defaultRole.HasValue)
		{
			while (list.Count < players.Count && list.Count + rolesAssigned < teamMax)
			{
				list.Add(defaultRole.Value);
			}
			AssignRolesFromList(players, teamMax, list, ref rolesAssigned);
		}
	}

	private void AssignRolesFromList(List<NetworkedPlayerInfo> players, int teamMax, List<RoleTypes> roleList, ref int rolesAssigned)
	{
		while (roleList.Count > 0 && players.Count > 0 && rolesAssigned < teamMax)
		{
			int index = HashRandom.FastNext(roleList.Count);
			RoleTypes roleType = roleList[index];
			roleList.RemoveAt(index);
			int index2 = HashRandom.FastNext(players.Count);
			players[index2].Object.RpcSetRole(roleType);
			players.RemoveAt(index2);
			rolesAssigned++;
		}
	}

	public override void OnPlayerDeath(PlayerControl player, bool assignGhostRole)
	{
		if (AmongUsClient.Instance.AmHost & assignGhostRole)
		{
			DestroyableSingleton<RoleManager>.Instance.AssignRoleOnDeath(player, specialRolesAllowed: true);
		}
	}
}
