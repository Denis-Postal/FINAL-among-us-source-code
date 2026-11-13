using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using Hazel;
using UnityEngine;
using UnityEngine.Audio;

public class PhantomRole : ImpostorRole
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Predicate<RoleEffectAnimation> _003C_003E9__20_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CUseAbility_003Eb__20_0(RoleEffectAnimation r)
		{
			return r.effectType == RoleEffectAnimation.EffectType.Vanish_Charge;
		}
	}

	private sealed class _003CCoAppearClients_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public Action callback;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoAppearClients_003Ed__23(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			if (_003C_003E1__state == 0)
			{
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			}
			if (_003C_003E1__state == 1)
			{
				_003C_003E1__state = -1;
				callback();
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		void IEnumerator.Reset()
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			NotSupportedException ex = new NotSupportedException();
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			throw ex;
		}
	}

	[SerializeField]
	private AudioClip appearSound;

	private const string SFX_VANISH_NAME = "PhantomVanish";

	private const string SFX_APPEAR_NAME = "PhantomAppear";

	private float cooldownSecondsRemaining;

	private float durationSecondsRemaining;

	public bool isInvisible;

	private bool fading;

	public bool serverApproved;

	public bool IsInvisible => isInvisible;

	private bool IsCoolingDown => cooldownSecondsRemaining > 0f;

	private static float RoleCooldownValue => GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.PhantomCooldown);

	public bool IsFading => fading;

	public bool ServerApproved => serverApproved;

	public bool SetFading(bool isFading)
	{
		return fading = isFading;
	}

	public bool SetServerApproval(bool approved)
	{
		return serverApproved = approved;
	}

	public override void UseAbility()
	{
		if (Player.AmOwner && !Player.Data.IsDead && Player.moveable && !Minigame.Instance && !IsCoolingDown && !fading && !Player.currentRoleAnimations.Find((RoleEffectAnimation r) => r.effectType == RoleEffectAnimation.EffectType.Vanish_Charge) && !Player.walkingToVent && !Player.inMovingPlat)
		{
			if (isInvisible)
			{
				MakePlayerVisible();
				return;
			}
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetSecondImage(Ability);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.OverrideText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PhantomAbilityUndo));
			Player.CmdCheckVanish(GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.PhantomDuration));
		}
	}

	public void SetInvisible(bool invisibility)
	{
		isInvisible = invisibility;
	}

	public void AppearClients(Action callback)
	{
		StartCoroutine(CoAppearClients(callback));
	}

	private IEnumerator CoAppearClients(Action callback)
	{
		yield return new WaitForSeconds(1f);
		callback();
	}

	private void MakePlayerVisible(bool shouldAnimate = true, bool setCooldown = true)
	{
		if (isInvisible || (bool)MeetingHud.Instance)
		{
			if (shouldAnimate)
			{
				SetFading(isFading: true);
			}
			Player.CmdCheckAppear(shouldAnimate);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			if (setCooldown)
			{
				SetCooldown();
			}
		}
	}

	protected override bool IsValidTarget(NetworkedPlayerInfo target)
	{
		if (base.IsValidTarget(target) && !isInvisible)
		{
			return !fading;
		}
		return false;
	}

	private bool ShouldUpdateCancelTimer()
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

	public override void OnVotingComplete()
	{
		base.OnVotingComplete();
		Player.ForcePhantomVisible();
		SetInvisible(invisibility: false);
		if (Player.AmOwner)
		{
			SetFading(isFading: false);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.PhantomDuration);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFillUp(roleFloat, roleFloat);
			cooldownSecondsRemaining = 0f;
			GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.PhantomCooldown);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, RoleCooldownValue);
		}
	}

	public override void SetCooldown()
	{
		cooldownSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.PhantomCooldown);
		DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, cooldownSecondsRemaining);
	}

	public void StartDuration()
	{
		durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.PhantomDuration);
	}

	private void FixedUpdate()
	{
		if (!Player || !Player.AmOwner || (bool)MeetingHud.Instance)
		{
			return;
		}
		if (!Player.moveable || Player.Data.IsDead || fading)
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		}
		else
		{
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
		}
		if (cooldownSecondsRemaining > 0f)
		{
			cooldownSecondsRemaining -= Time.fixedDeltaTime;
			float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.PhantomCooldown);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(cooldownSecondsRemaining, roleFloat);
		}
		if (isInvisible && durationSecondsRemaining > 0f && ShouldUpdateCancelTimer() && !fading)
		{
			durationSecondsRemaining -= Time.fixedDeltaTime;
			float roleFloat2 = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.PhantomDuration);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFillUp(durationSecondsRemaining, roleFloat2);
			if (durationSecondsRemaining <= 0f)
			{
				Player.NetTransform.Halt();
				MakePlayerVisible(!Player.inVent);
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Ability);
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetEnabled();
			}
		}
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
	}

	public override bool CanUse(IUsable usable)
	{
		if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
		{
			return false;
		}
		if (usable is Vent || usable is ZiplineConsole || usable is Ladder || usable is PlatformConsole)
		{
			if (fading)
			{
				return false;
			}
			return true;
		}
		if (isInvisible)
		{
			return false;
		}
		Console console = usable as Console;
		if (console != null)
		{
			return console.AllowImpostor;
		}
		return true;
	}

	public void PlayPhantomVanishSound()
	{
		if (!(PlayerControl.LocalPlayer != Player) && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlayDynamicSound("PhantomVanish", UseSound, loop: false, SoundDynamics, SoundManager.Instance.SfxChannel);
		}
	}

	public void PlayPhantomAppearSound()
	{
		if (!(PlayerControl.LocalPlayer != Player) && Constants.ShouldPlaySfx() && !MeetingHud.Instance)
		{
			SoundManager.Instance.PlayDynamicSound("PhantomAppear", appearSound, loop: false, SoundDynamics, SoundManager.Instance.SfxChannel);
		}
	}

	public override void HandleRoleRpc(byte callId, MessageReader reader)
	{
		switch ((RpcCalls)callId)
		{
		case RpcCalls.CheckVanish:
			Player.CheckVanish();
			break;
		case RpcCalls.StartVanish:
			Player.HandleServerVanish();
			break;
		case RpcCalls.CheckAppear:
			Player.CheckAppear(reader.ReadBoolean());
			break;
		case RpcCalls.StartAppear:
			Player.HandleServerAppear(reader.ReadBoolean());
			break;
		}
	}

	private void SoundDynamics(AudioSource source, float dt)
	{
		if (!PlayerControl.LocalPlayer)
		{
			source.volume = 0f;
			return;
		}
		source.volume = 1f;
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		source.volume = SoundManager.GetSoundVolume(Player.GetTruePosition(), truePosition, 3f, 7f);
	}
}
