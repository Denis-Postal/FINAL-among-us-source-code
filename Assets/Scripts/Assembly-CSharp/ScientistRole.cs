using Object = UnityEngine.Object;
using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

public class ScientistRole : RoleBehaviour
{
	public VitalsMinigame VitalsPrefab;

	private VitalsMinigame minigame;

	private float currentCharge;

	private float currentCooldown;

	public override bool IsDead => false;

	private bool IsCoolingDown => currentCooldown > 0f;

	private float RoleCooldownValue => GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ScientistCooldown);

	private float RoleBatteryCharge => GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ScientistBatteryCharge);

	public override void Initialize(PlayerControl player)
	{
		base.Initialize(player);
		Recharge();
	}

	public override void OnVotingComplete()
	{
		base.OnVotingComplete();
		if (Player.AmOwner && Player.AllTasksCompleted())
		{
			currentCooldown = 0f;
			Recharge();
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(currentCooldown, RoleCooldownValue);
		}
	}

	private void Update()
	{
		if (!Player || !Player.AmOwner || (bool)MeetingHud.Instance)
		{
			return;
		}
		if (minigame != null)
		{
			currentCharge -= Time.deltaTime;
			minigame.BatteryText.text = ((int)currentCharge).ToString();
			if (currentCharge <= 0f)
			{
				minigame.Close();
				if (Player.AllTasksCompleted())
				{
					currentCooldown = RoleCooldownValue;
				}
				else
				{
					RefreshAbilityButton();
				}
			}
		}
		else if (IsCoolingDown)
		{
			currentCooldown -= Time.deltaTime;
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(currentCooldown, RoleCooldownValue);
			if (currentCooldown <= 0f)
			{
				Recharge();
			}
		}
		else
		{
			RefreshAbilityButton();
		}
	}

	public bool Recharge()
	{
		float roleBatteryCharge = RoleBatteryCharge;
		bool result = currentCharge < roleBatteryCharge;
		currentCharge = roleBatteryCharge;
		RefreshAbilityButton();
		return result;
	}

	public unsafe override void UseAbility()
	{
		if (currentCharge <= 0f || IsAffectedByComms || !PlayerControl.LocalPlayer.CanMove)
		{
			if (minigame != null)
			{
				minigame.Close();
			}
			return;
		}
		Player.NetTransform.Halt();
		minigame = Object.Instantiate(VitalsPrefab);
		minigame.transform.SetParent(Camera.main.transform, worldPositionStays: false);
		minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
		minigame.BatteryText.gameObject.SetActive(value: true);
		minigame.Begin(null);
	}

	public override bool CanUse(IUsable console)
	{
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

	private void RefreshAbilityButton()
	{
		if (currentCharge > 0f && !IsAffectedByComms)
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
		}
		else
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		}
	}

	public override PlayerControl FindClosestTarget()
	{
		return null;
	}

	public ScientistRole()
	{ }
}
