using System;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using UnityEngine;

public class EngineerRole : RoleBehaviour
{
	private Vent currentTarget;

	private float cooldownSecondsRemaining;

	private float inVentTimeRemaining;

	private int usesRemaining = 1;

	private bool isExitVentQueued;

	public override bool IsDead => false;

	private bool IsCoolingDown => cooldownSecondsRemaining > 0f;

	public override void Initialize(PlayerControl player)
	{
		base.Initialize(player);
		if (GameManager.Instance.LogicOptions is LogicOptionsHnS logicOptionsHnS)
		{
			usesRemaining = logicOptionsHnS.GetCrewmateVentUses();
			DestroyableSingleton<HudManager>.Instance.AbilityButton.OverrideText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.HideActionButton));
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetUsesRemaining(usesRemaining);
		}
	}

	private float GetCooldown()
	{
		return GameManager.Instance.LogicOptions.GetEngineerCooldown();
	}

	private float GetVentTime()
	{
		return GameManager.Instance.LogicOptions.GetEngineerInVentTime();
	}

	public override bool CanUse(IUsable console)
	{
		if (console is Vent)
		{
			if (!IsAffectedByComms || Player.inVent)
			{
				return usesRemaining > 0;
			}
			return false;
		}
		if (console is ZiplineConsole || console is Ladder || console is PlatformConsole)
		{
			return true;
		}
		if (!(console is Console))
		{
			return console is DoorConsole;
		}
		return true;
	}

	public override bool DidWin(GameOverReason gameOverReason)
	{
		return GameManager.Instance.DidHumansWin(gameOverReason);
	}

	public override void SetUsableTarget(IUsable target)
	{
		if (target is Vent)
		{
			currentTarget = target as Vent;
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
		}
		else
		{
			currentTarget = null;
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		}
	}

	public unsafe override void UseAbility()
	{
		PlayerControl localPlayer = PlayerControl.LocalPlayer;
		if (!currentTarget)
		{
			return;
		}
		currentTarget.CanUse(localPlayer.Data, out var canUse, out var _);
		if (!((isActiveAndEnabled && !IsCoolingDown) & canUse))
		{
			return;
		}
		inVentTimeRemaining = GetVentTime();
		bool flag = localPlayer.inVent && !localPlayer.walkingToVent;
		currentTarget.Use();
		if ((GameManager.Instance.LogicOptions is LogicOptionsHnS) & flag)
		{
			usesRemaining--;
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetUsesRemaining(usesRemaining);
		}
		if (!flag)
		{
			if (GameManager.Instance.IsHideAndSeek())
			{
				DataManager.Player.Stats.IncrementStat(StatID.HideAndSeek_TimesVented);
			}
			else
			{
				DataManager.Player.Stats.IncrementStat(StatID.Role_Engineer_Vents);
			}
		}
	}

	public override void SetCooldown()
	{
		cooldownSecondsRemaining = GetCooldown();
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, GetCooldown());
	}

	public override void OnVotingComplete()
	{
		base.OnVotingComplete();
		if (Player.AmOwner)
		{
			cooldownSecondsRemaining = 0f;
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(0f, GetCooldown());
			float ventTime = GetVentTime();
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFillUp(ventTime, ventTime);
		}
	}

	private void FixedUpdate()
	{
		if (!Player || !Player.AmOwner || (bool)MeetingHud.Instance)
		{
			return;
		}
		if (cooldownSecondsRemaining > 0f)
		{
			cooldownSecondsRemaining -= Time.deltaTime;
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, GetCooldown());
		}
		if (Player.inVent)
		{
			float ventTime = GetVentTime();
			if (ventTime > 0f)
			{
				inVentTimeRemaining -= Time.deltaTime;
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFillUp(inVentTimeRemaining, ventTime);
			}
			if ((inVentTimeRemaining < 0f || CommsSabotaged) && isActiveAndEnabled && (bool)currentTarget && !isExitVentQueued)
			{
				currentTarget.Use();
				isExitVentQueued = true;
				if (GameManager.Instance.IsHideAndSeek())
				{
					usesRemaining--;
					DestroyableSingleton<HudManager>.Instance.AbilityButton.SetUsesRemaining(usesRemaining);
				}
			}
		}
		else
		{
			isExitVentQueued = false;
		}
	}

	public override PlayerControl FindClosestTarget()
	{
		return null;
	}

	public EngineerRole()
	{ }
}
