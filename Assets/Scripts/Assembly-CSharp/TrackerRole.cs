using System;
using System.Collections.Generic;
using System.Text;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class TrackerRole : CrewmateRole
{
	private PlayerControl currentTarget;

	private float cooldownSecondsRemaining;

	private float durationSecondsRemaining;

	private float delaySecondsRemaining;

	private bool pauseTrackingTime;

	[SerializeField]
	private AudioClip cancelSound;

	private bool isTrackingActive;

	private const float DelaySeconds = 1f;

	private bool IsCoolingDown => cooldownSecondsRemaining > 0f;

	private bool IsDelayed => delaySecondsRemaining > 0f;

	public override void UseAbility()
	{
		if (Player.AmOwner && !IsAffectedByComms && !Player.Data.IsDead && Player.moveable && !Minigame.Instance && !IsCoolingDown && !IsDelayed)
		{
			if (isTrackingActive)
			{
				CancelTracking();
			}
			else if (!(currentTarget == null))
			{
				Player.StartPlayerTracking(currentTarget, currentTarget.cosmetics.ColorId);
				DestroyableSingleton<HudManager>.Instance.AbilityButton.OverrideText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.TrackerAbilityUndo));
				SoundManager.Instance.PlaySound(UseSound, loop: false);
				SetPlayerTarget(null);
				isTrackingActive = true;
				durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration);
				delaySecondsRemaining = 1f;
			}
		}
	}

	private void CancelTracking(bool setCooldown = true)
	{
		if (isTrackingActive)
		{
			isTrackingActive = false;
			Player.CancelPlayerTracking();
			if (MeetingHud.Instance == null)
			{
				SoundManager.Instance.PlaySound(cancelSound, loop: false);
			}
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			if (setCooldown)
			{
				SetCooldown();
			}
		}
	}

	public override void SetPlayerTarget(PlayerControl target)
	{
		if (!Player.AmOwner || Player.Data.IsDead)
		{
			return;
		}
		if (isTrackingActive)
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			return;
		}
		RoleTeamTypes teamType = Player.Data.Role.TeamType;
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

	private void ClearPlayerTarget()
	{
		if (Player.AmOwner)
		{
			if (currentTarget != null)
			{
				currentTarget.ToggleHighlight(active: false, Player.Data.Role.TeamType);
			}
			currentTarget = null;
		}
	}

	public override PlayerControl FindClosestTarget()
	{
		if (!Player.AmOwner)
		{
			return null;
		}
		List<PlayerControl> playersInAbilityRangeSorted = GetPlayersInAbilityRangeSorted(RoleBehaviour.GetTempPlayerList());
		if (playersInAbilityRangeSorted.Count <= 0)
		{
			return null;
		}
		return playersInAbilityRangeSorted[0];
	}

	protected override float GetAbilityDistance()
	{
		return 1.8f;
	}

	private bool ShouldUpdateCancelTimer()
	{
		if (!Player || !Player.AmOwner || (bool)MeetingHud.Instance)
		{
			return false;
		}
		return true;
	}

	public override void OnMeetingStart()
	{
		base.OnMeetingStart();
		pauseTrackingTime = true;
		if (Player.AmOwner)
		{
			CancelTracking(setCooldown: false);
		}
	}

	public void CheckCancelTracking(PlayerControl pc)
	{
		if (Player.AmOwner && isTrackingActive && Player.trackedPlayer == pc)
		{
			CancelTracking();
		}
	}

	public override void OnVotingComplete()
	{
		base.OnVotingComplete();
		pauseTrackingTime = false;
		if (Player.AmOwner)
		{
			CancelTracking(setCooldown: false);
			cooldownSecondsRemaining = 0f;
			float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFillUp(roleFloat, roleFloat);
			float roleFloat2 = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerCooldown);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, roleFloat2);
		}
	}

	public override void SetCooldown()
	{
		float num = 1f;
		if (durationSecondsRemaining > GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration) / 2f)
		{
			num = 0.5f;
		}
		cooldownSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerCooldown) * num;
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, cooldownSecondsRemaining);
	}

	private void FixedUpdate()
	{
		if (!Player || !Player.AmOwner || (bool)MeetingHud.Instance)
		{
			return;
		}
		if (IsDelayed)
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
			delaySecondsRemaining -= Time.fixedDeltaTime;
			if (delaySecondsRemaining <= 0f)
			{
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			}
		}
		if (!Player.moveable || Player.Data.IsDead)
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		}
		if (cooldownSecondsRemaining > 0f && !pauseTrackingTime)
		{
			cooldownSecondsRemaining -= Time.fixedDeltaTime;
			float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerCooldown);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, roleFloat);
		}
		if (isTrackingActive && durationSecondsRemaining > 0f && ShouldUpdateCancelTimer())
		{
			durationSecondsRemaining -= Time.fixedDeltaTime;
			float roleFloat2 = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFillUp(durationSecondsRemaining, roleFloat2);
			if (durationSecondsRemaining <= 0f || IsAffectedByComms || Player.trackedPlayer.Data.Disconnected)
			{
				CancelTracking();
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			}
		}
	}

	public override void OnDeath(DeathReason reason)
	{
		if (Player.AmOwner)
		{
			CancelTracking(setCooldown: false);
			ClearPlayerTarget();
		}
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
	}

	private void OnDestroy()
	{
		if (Player.AmOwner)
		{
			isTrackingActive = false;
			Player.CancelPlayerTracking();
		}
	}
}
