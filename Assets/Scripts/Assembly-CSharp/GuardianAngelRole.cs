using System;
using System.Collections.Generic;
using System.Text;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class GuardianAngelRole : CrewmateGhostRole
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Comparison<PlayerControl> _003C_003E9__12_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal int _003CFindClosestTarget_003Eb__12_0(PlayerControl a, PlayerControl b)
		{
			// crewmates sort before impostors
			bool flag = a.Data.Role.TeamType == RoleTeamTypes.Impostor;
			bool flag2 = b.Data.Role.TeamType == RoleTeamTypes.Impostor;
			if (flag && !flag2)
			{
				return 1;
			}
			if (!flag && flag2)
			{
				return -1;
			}
			return 0;
		}
	}

	private PlayerControl currentTarget;

	private float cooldownSecondsRemaining;

	private bool IsCoolingDown => cooldownSecondsRemaining > 0f;

	public override bool IsAffectedByComms => CommsSabotaged;

	public override void UseAbility()
	{
		if (isActiveAndEnabled && (bool)currentTarget && !IsCoolingDown && PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.CanMove && !IsAffectedByComms)
		{
			PlayerControl.LocalPlayer.CmdCheckProtect(currentTarget);
			SoundManager.Instance.PlaySound(UseSound, loop: false);
			SetPlayerTarget(null);
		}
	}

	public override void SetPlayerTarget(PlayerControl target)
	{
		RoleTeamTypes teamType = PlayerControl.LocalPlayer.Data.Role.TeamType;
		if (target != currentTarget && currentTarget != null)
		{
			currentTarget.ToggleHighlight(active: false, teamType);
		}
		currentTarget = target;
		if ((bool)currentTarget && !target.Data.IsDead && !IsAffectedByComms)
		{
			currentTarget.ToggleHighlight(active: true, teamType);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
		}
		else
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		}
	}

	public override void OnVotingComplete()
	{
		base.OnVotingComplete();
		if (Player.AmOwner)
		{
			cooldownSecondsRemaining = 0f;
			UpdateCooldown();
		}
	}

	public override void SetCooldown()
	{
		cooldownSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.GuardianAngelCooldown);
		UpdateCooldown();
	}

	private void FixedUpdate()
	{
		if ((bool)Player && Player.AmOwner && !MeetingHud.Instance && cooldownSecondsRemaining > 0f)
		{
			cooldownSecondsRemaining -= Time.deltaTime;
			UpdateCooldown();
		}
	}

	private void UpdateCooldown()
	{
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.GuardianAngelCooldown));
	}

	public override PlayerControl FindClosestTarget()
	{
		List<PlayerControl> playersInAbilityRangeSorted = GetPlayersInAbilityRangeSorted(RoleBehaviour.GetTempPlayerList(), ignoreColliders: true);
		if (playersInAbilityRangeSorted.Count <= 0)
		{
			return null;
		}
		playersInAbilityRangeSorted.Sort((PlayerControl a, PlayerControl b) =>
		{
			if (a.Data == null || b.Data == null || a.Data.Role == null || b.Data.Role == null)
			{
				return 0;
			}
			if (a.Data.Role.IsImpostor && !b.Data.Role.IsImpostor)
			{
				return 1;
			}
			return (!a.Data.Role.IsImpostor && b.Data.Role.IsImpostor) ? (-1) : 0;
		});
		return playersInAbilityRangeSorted[0];
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
	}
}
