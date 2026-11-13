using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using Hazel;
using UnityEngine;

public abstract class RoleBehaviour : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass74_0
	{
		public Vector2 myPos;

		internal int _003CGetPlayersInAbilityRangeSorted_003Eb__0(PlayerControl a, PlayerControl b)
		{
			float num = (a.GetTruePosition() - myPos).magnitude;
			float value = (b.GetTruePosition() - myPos).magnitude;
			return num.CompareTo(value);
		}
	}

	private sealed class _003C_003Ec__DisplayClass78_0
	{
		public Vector2 myPos;

		internal int _003CGetBodiesInAbilityRangeSorted_003Eb__0(DeadBody a, DeadBody b)
		{
			float num = (a.TruePosition - myPos).magnitude;
			float value = (b.TruePosition - myPos).magnitude;
			return num.CompareTo(value);
		}
	}

	public RoleTypes Role;

	public StringNames StringName = StringNames.Crewmate;

	public StringNames BlurbName = StringNames.CrewmateBlurb;

	public StringNames BlurbNameMed = StringNames.CrewmateBlurb;

	public StringNames BlurbNameLong = StringNames.CrewmateBlurb;

	public Sprite RoleIconSolid;

	public Sprite RoleIconWhite;

	public Sprite RoleIconColor;

	public Sprite RoleScreenshot;

	public Color NameColor= Color.white;

	public bool TasksCountTowardProgress = true;

	public bool CanUseKillButton;

	public bool CanBeKilled= true;

	public bool CanVent;

	public bool AffectedByLightAffectors= true;

	public int MaxCount= 15;

	public RoleTeamTypes TeamType;

	public AbilityButtonSettings Ability;

	public AbilityButtonSettings SecondaryAbility;

	public RoleTypes DefaultGhostRole;

	public AudioClip UseSound;

	public AudioClip IntroSound;

	public List<BaseGameSetting> AllGameSettings;

	public OverlayKillAnimation[] CustomKillAnimations;

	protected PlayerControl Player;

	protected AbilityButton buttonManager;

	protected SecondaryAbilityButton secondaryButtonManager;

	public static List<PlayerControl> tempPlayerList= new List<PlayerControl>();

	private static List<DeadBody> tempBodyList= new List<DeadBody>();

	public bool IsImpostor => TeamType == RoleTeamTypes.Impostor;

	public abstract bool IsDead { get; }

	public string NiceName => DestroyableSingleton<TranslationController>.Instance.GetString(StringName);

	public string Blurb => DestroyableSingleton<TranslationController>.Instance.GetString(BlurbName);

	public string BlurbMed => DestroyableSingleton<TranslationController>.Instance.GetString(BlurbNameMed);

	public string BlurbLong => DestroyableSingleton<TranslationController>.Instance.GetString(BlurbNameLong);

	public bool IsSimpleRole
	{
		get
		{
			if (Role != RoleTypes.Crewmate)
			{
				return Role == RoleTypes.Impostor;
			}
			return true;
		}
	}

	public Color TeamColor
	{
		get
		{
			if (!IsImpostor)
			{
				return Palette.CrewmateBlue;
			}
			return Palette.ImpostorRed;
		}
	}

	public virtual bool IsAffectedByComms
	{
		get
		{
			if (CommsSabotaged)
			{
				return !Player.Data.Role.IsImpostor;
			}
			return false;
		}
	}

	protected bool CommsSabotaged
	{
		get
		{
			if (Player != null)
			{
				return PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(Player);
			}
			return false;
		}
	}

	protected static void AppendTaskHint(RoleBehaviour role, StringBuilder taskStringBuilder)
	{
		taskStringBuilder.AppendFormat("\n{0} {1}\n{2}", role.NiceName, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoleHint), role.BlurbMed);
	}

	public virtual bool CanUse(IUsable console)
	{
		return false;
	}

	public virtual bool DidWin(GameOverReason gameOverReason)
	{
		return false;
	}

	public virtual void Deinitialize(PlayerControl targetPlayer)
	{
	}

	public virtual void SpawnTaskHeader(PlayerControl playerControl)
	{
	}

	public virtual void UseAbility()
	{
	}

	public virtual void UseSecondaryAbility()
	{
	}

	public virtual void OnMeetingStart()
	{
		InitializeMeetingAbilityButton();
	}

	public virtual void OnVotingComplete()
	{
	}

	public virtual void OnExileComplete()
	{
	}

	public virtual void OnDeath(DeathReason reason)
	{
	}

	public virtual void OnRoleSet()
	{
	}

	public virtual void Initialize(PlayerControl player)
	{
		Player = player;
		if (!player.AmOwner)
		{
			return;
		}
		if (IsImpostor)
		{
			if (CanUseKillButton)
			{
				DestroyableSingleton<HudManager>.Instance.KillButton.Show();
				player.SetKillTimer(10f);
				DestroyableSingleton<HudManager>.Instance.KillButton.ResetKillButton();
			}
			DestroyableSingleton<HudManager>.Instance.SabotageButton.Show();
			DestroyableSingleton<HudManager>.Instance.AdminButton.Show();
			DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.Show();
		}
		DestroyableSingleton<HudManager>.Instance.SetHudActive(player, this, isActive: true);
		PlayerNameColor.SetForRoleDirectly(player, this);
		InitializeAbilityButton();
		InitializeSecondaryAbilityButton();
	}

	public virtual void SetUsableTarget(IUsable target)
	{
	}

	public virtual void SetPlayerTarget(PlayerControl target)
	{
	}

	public virtual void SetCooldown()
	{
	}

	protected void InitializeMeetingAbilityButton()
	{
		if (Player.AmOwner && (bool)Ability && Ability.AbilityType != AbilityTypes.GameOnly && (bool)MeetingHud.Instance)
		{
			buttonManager = MeetingHud.Instance.MeetingAbilityButton;
			if ((bool)Ability)
			{
				buttonManager.SetFromSettings(Ability);
				buttonManager.SetEnabled();
				buttonManager.gameObject.SetActive(value: true);
				buttonManager.SetCoolDown(0f, 1f);
			}
			else
			{
				buttonManager.gameObject.SetActive(value: false);
			}
		}
	}

	private void InitializeAbilityButton()
	{
		if ((bool)Ability && Ability.AbilityType != AbilityTypes.MeetingOnly)
		{
			buttonManager = DestroyableSingleton<HudManager>.Instance.AbilityButton;
			if ((bool)Ability)
			{
				buttonManager.SetFromSettings(Ability);
				buttonManager.SetEnabled();
				buttonManager.gameObject.SetActive(value: true);
				buttonManager.SetCoolDown(0f, 1f);
			}
			else
			{
				buttonManager.gameObject.SetActive(value: false);
			}
		}
	}

	private void InitializeSecondaryAbilityButton()
	{
		secondaryButtonManager = DestroyableSingleton<HudManager>.Instance.SecondaryAbilityButton;
		if ((bool)SecondaryAbility)
		{
			secondaryButtonManager.SetFromSettings(SecondaryAbility);
			secondaryButtonManager.SetEnabled();
			secondaryButtonManager.gameObject.SetActive(value: true);
			secondaryButtonManager.SetCoolDown(0f, 1f);
		}
		else
		{
			secondaryButtonManager.gameObject.SetActive(value: false);
		}
	}

	private void SetAbilityUsesRemaining(int num)
	{
		buttonManager.SetUsesRemaining(num);
	}

	protected virtual bool IsValidTarget(NetworkedPlayerInfo target)
	{
		if (target == null || target.Disconnected || target.IsDead || target.PlayerId == Player.PlayerId || target.Role == null || target.Object == null || target.Object.inVent || target.Object.inMovingPlat || !target.Object.Visible)
		{
			return false;
		}
		return true;
	}

	public virtual PlayerControl FindClosestTarget()
	{
		return null;
	}

	public virtual DeadBody FindClosestBody()
	{
		return null;
	}

	protected virtual float GetAbilityDistance()
	{
		return GameManager.Instance.LogicOptions.GetKillDistance();
	}

	protected static List<PlayerControl> GetTempPlayerList()
	{
		return tempPlayerList;
	}

	public List<PlayerControl> GetPlayersInAbilityRangeSorted(List<PlayerControl> outputList)
	{
		return GetPlayersInAbilityRangeSorted(outputList, ignoreColliders: false);
	}

	protected List<PlayerControl> GetPlayersInAbilityRangeSorted(List<PlayerControl> outputList, bool ignoreColliders)
	{
		outputList.Clear();
		float abilityDistance = GetAbilityDistance();
		Vector2 myPos = Player.GetTruePosition();
		List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
		for (int i = 0; i < allPlayers.Count; i++)
		{
			NetworkedPlayerInfo networkedPlayerInfo = allPlayers[i];
			if (!IsValidTarget(networkedPlayerInfo))
			{
				continue;
			}
			PlayerControl playerControl = networkedPlayerInfo.Object;
			if ((bool)playerControl && playerControl.Collider.enabled)
			{
				Vector2 vector = playerControl.GetTruePosition() - myPos;
				float magnitude = vector.magnitude;
				if (magnitude <= abilityDistance && (ignoreColliders || !PhysicsHelpers.AnyNonTriggersBetween(myPos, vector.normalized, magnitude, Constants.ShipAndObjectsMask)))
				{
					outputList.Add(playerControl);
				}
			}
		}
		outputList.Sort((PlayerControl a, PlayerControl b) =>
		{
			float magnitude2 = (a.GetTruePosition() - myPos).magnitude;
			float magnitude3 = (b.GetTruePosition() - myPos).magnitude;
			if (magnitude2 > magnitude3)
			{
				return 1;
			}
			return (magnitude2 < magnitude3) ? (-1) : 0;
		});
		return outputList;
	}

	protected static List<DeadBody> GetTempBodyList()
	{
		return tempBodyList;
	}

	public List<DeadBody> GetBodiesInAbilityRangeSorted(List<DeadBody> outputList)
	{
		return GetBodiesInAbilityRangeSorted(outputList, ignoreColliders: false);
	}

	private List<DeadBody> GetBodiesInAbilityRangeSorted(List<DeadBody> outputList, bool ignoreColliders)
	{
		outputList.Clear();
		float abilityDistance = GetAbilityDistance();
		Vector2 myPos = Player.GetTruePosition();
		List<DeadBody> list = Object.FindObjectsOfType<DeadBody>().ToList();
		for (int i = 0; i < list.Count; i++)
		{
			DeadBody deadBody = list[i];
			Vector2 vector = deadBody.TruePosition - myPos;
			float magnitude = vector.magnitude;
			if (magnitude <= abilityDistance && (ignoreColliders || !PhysicsHelpers.AnyNonTriggersBetween(myPos, vector.normalized, magnitude, Constants.ShipAndObjectsMask)))
			{
				outputList.Add(deadBody);
			}
		}
		outputList.Sort((DeadBody a, DeadBody b) =>
		{
			float magnitude2 = (a.TruePosition - myPos).magnitude;
			float magnitude3 = (b.TruePosition - myPos).magnitude;
			if (magnitude2 > magnitude3)
			{
				return 1;
			}
			return (magnitude2 < magnitude3) ? (-1) : 0;
		});
		return outputList;
	}

	public virtual void AdjustTasks(PlayerControl player)
	{
	}

	public virtual void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		AppendTaskHint(this, taskStringBuilder);
	}

	public virtual void HandleRoleRpc(byte callId, MessageReader reader)
	{
	}

	public virtual void KillAnimSpecialSetup(DeadBody deadBody, PlayerControl killer, PlayerControl victim)
	{
	}

	protected RoleBehaviour()
	{ }

	static RoleBehaviour()
	{ }
}
