using Object = UnityEngine.Object;
using System;
using System.Text;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

public class ShapeshifterRole : ImpostorRole
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Predicate<RoleEffectAnimation> _003C_003E9__7_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CUseAbility_003Eb__7_0(RoleEffectAnimation r)
		{
			return r.effectType == RoleEffectAnimation.EffectType.Shapeshift;
		}
	}

	public ShapeshifterEvidence EvidencePrefab;

	public Vector3 EvidenceOffset;

	public ShapeshifterMinigame ShapeshifterMenu;

	private float cooldownSecondsRemaining;

	private float durationSecondsRemaining;

	private bool IsCoolingDown => cooldownSecondsRemaining > 0f;

	public unsafe override void UseAbility()
	{
		if (!Player.Data.IsDead && Player.moveable && !Minigame.Instance && !IsCoolingDown && !(Player.currentRoleAnimations.Find((RoleEffectAnimation r) => r.effectType == RoleEffectAnimation.EffectType.Shapeshift) != null) && !Player.IsMushroomMixupActive() && (!MatchInfoGuide.Instance || !MatchInfoGuide.Instance.IsActive))
		{
			Player.NetTransform.Halt();
			if (Player.CurrentOutfitType == PlayerOutfitType.Shapeshifted)
			{
				RevertPlayerToNormal();
				return;
			}
			ShapeshifterMinigame shapeshifterMinigame = Object.Instantiate(ShapeshifterMenu);
			shapeshifterMinigame.transform.SetParent(Camera.main.transform, worldPositionStays: false);
			shapeshifterMinigame.transform.localPosition = new Vector3(0f, 0f, -50f);
			shapeshifterMinigame.Begin(null);
			durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration);
		}
	}

	public void RevertPlayerToNormal(bool shouldAnimate = true, bool setCooldown = true)
	{
		if (Player.CurrentOutfitType == PlayerOutfitType.Shapeshifted || (bool)MeetingHud.Instance)
		{
			Player.CmdCheckRevertShapeshift(shouldAnimate);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			if (setCooldown)
			{
				SetCooldown();
			}
		}
	}

	public override void OnVotingComplete()
	{
		base.OnVotingComplete();
		if (Player.AmOwner)
		{
			RevertPlayerToNormal(shouldAnimate: false, setCooldown: false);
			float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFillUp(roleFloat, roleFloat);
			cooldownSecondsRemaining = 0f;
			float roleFloat2 = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterCooldown);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, roleFloat2);
		}
	}

	public override void SetCooldown()
	{
		cooldownSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterCooldown);
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, cooldownSecondsRemaining);
	}

	private bool ShouldUpdateUnshiftTimer()
	{
		if (!Player || !Player.AmOwner || (bool)MeetingHud.Instance)
		{
			return false;
		}
		if (Player.inMovingPlat || Player.onLadder)
		{
			return false;
		}
		return true;
	}

	private void FixedUpdate()
	{
		if (!Player || !Player.AmOwner || (bool)MeetingHud.Instance)
		{
			return;
		}
		if (!Player.moveable || Player.Data.IsDead || Player.IsMushroomMixupActive())
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		}
		else
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
		}
		if (Player.IsMushroomMixupActive())
		{
			cooldownSecondsRemaining = 0f;
			float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterCooldown);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, roleFloat);
			return;
		}
		if (cooldownSecondsRemaining > 0f)
		{
			cooldownSecondsRemaining -= Time.deltaTime;
			float roleFloat2 = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterCooldown);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, roleFloat2);
		}
		if (Player.CurrentOutfitType == PlayerOutfitType.Shapeshifted && durationSecondsRemaining > 0f && ShouldUpdateUnshiftTimer())
		{
			durationSecondsRemaining -= Time.deltaTime;
			float roleFloat3 = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFillUp(durationSecondsRemaining, roleFloat3);
			if (durationSecondsRemaining <= 0f)
			{
				Player.NetTransform.Halt();
				RevertPlayerToNormal(!Player.inVent);
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			}
		}
	}

	public unsafe void SetEvidence()
	{
		if (GameManager.Instance.LogicOptions.GetRoleBool(BoolOptionNames.ShapeshifterLeaveSkin))
		{
			ShapeshifterEvidence shapeshifterEvidence = Object.Instantiate(EvidencePrefab);
			Vector3 position = Player.transform.position + EvidenceOffset * 0.7f;
			position.z = position.y / 1000f;
			shapeshifterEvidence.transform.position = position;
		}
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
	}
}
