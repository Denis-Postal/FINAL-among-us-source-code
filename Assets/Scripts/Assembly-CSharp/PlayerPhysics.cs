using System;
using System.Collections;
using System.Linq;
using AmongUs.Data;
using AmongUs.GameOptions;
using AmongUs.InnerNet.GameDataMessages;
using Hazel;
using InnerNet;
using Rewired;
using UnityEngine;

public class PlayerPhysics : InnerNetObject
{
	public AudioClip ImpostorDiscoveredSound;

	public PlayerAnimations Animations;

	[NonSerialized]
	public SpecialInputHandler inputHandler;

	public float Speed = 4.5f;

	public float GhostSpeed = 3f;

	private const float ClimbPositionTolerance = 0.0005f;

	private readonly Logger logger = new Logger(Logger.Category.Gameplay, "PlayerPhysics");

	private Rigidbody2D body;

	private PlayerControl myPlayer;

	private PlayerBodyTypes bodyType;

	private Coroutine petCoroutine;

	private byte lastClimbLadderSid;

	private bool loggedBlockedAnimationDiagnostics;

	public bool DoingCustomAnimation { get; private set; }

	public float TrueSpeed => Speed * SpeedMod;

	public float SpeedMod
	{
		get
		{
			if (GameManager.Instance == null)
			{
				return 1f;
			}
			float playerSpeedMod = GameManager.Instance.LogicOptions.GetPlayerSpeedMod(myPlayer);
			if (myPlayer != null && myPlayer.Data != null && myPlayer.Data.IsDead)
			{
				return playerSpeedMod * GhostSpeed / Speed;
			}
			return playerSpeedMod;
		}
	}

	private bool FlipX
	{
		get
		{
			return myPlayer.cosmetics.FlipX;
		}
		set
		{
			myPlayer.cosmetics.SetFlipXWithoutPet(value);
		}
	}

	public Vector2 Velocity => body.linearVelocity;

	public void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		myPlayer = GetComponent<PlayerControl>();
		SetBodyType(bodyType);
		FlipX = false;
		if (inputHandler == null)
		{
			inputHandler = gameObject.AddComponent<SpecialInputHandler>();
			inputHandler.disableVirtualCursor = true;
			inputHandler.enabled = false;
		}
	}

	public void EnableInterpolation()
	{
		body.interpolation = RigidbodyInterpolation2D.Interpolate;
	}

	public void SetBodyType(PlayerBodyTypes bodyType)
	{
		this.bodyType = bodyType;
		myPlayer.cosmetics.EnsureInitialized(bodyType);
		Animations.SetBodyType(bodyType, myPlayer.cosmetics.FlippedCosmeticOffset, myPlayer.cosmetics.NormalCosmeticOffset);
		Animations.PlayIdleAnimation();
	}

	private void FixedUpdate()
	{
		NetworkedPlayerInfo data = myPlayer.Data;
		bool amDead = data != null && data.IsDead;
		HandleAnimation(amDead);
		if (AmOwner)
		{
			if (myPlayer.CanMove && (bool)GameData.Instance && DestroyableSingleton<HudManager>.InstanceExists && DestroyableSingleton<HudManager>.Instance.joystick != null)
			{
				SetNormalizedVelocity(DestroyableSingleton<HudManager>.Instance.joystick.DeltaL);
			}
			CheckCancelPetting();
		}
		if (myPlayer.PhantomFadeActive)
		{
			myPlayer.cosmetics.SetPhantomRoleAlpha(myPlayer.CalculatedAlpha);
		}
	}

	public Vector2 GetVelocity()
	{
		return body.linearVelocity;
	}

	public void SetNormalizedVelocity(Vector2 direction)
	{
		body.linearVelocity = direction * TrueSpeed;
	}

	private void LateUpdate()
	{
		Vector3 position = transform.position;
		position.z = position.y / 1000f;
		transform.position = position;
	}

	public Vector3 Vec2ToPosition(Vector2 pos)
	{
		return new Vector3(pos.x, pos.y, pos.y / 1000f);
	}

	public void SetSkin(string skinId, int color)
	{
		myPlayer.cosmetics.SetSkin(skinId, color, () =>
		{
			if (Animations.IsPlayingSpawnAnimation())
			{
				myPlayer.cosmetics.AnimateSkinSpawn(Animations.Time);
			}
			ShipStatus instance = ShipStatus.Instance;
			if ((object)instance != null && instance.Type == ShipStatus.MapType.Fungle)
			{
				if (myPlayer.inMovingPlat)
				{
					myPlayer.cosmetics.AnimateSkinJump();
				}
				if (Animations.IsPlayingClimbAnimation())
				{
					bool down = !(Velocity.y > 0f);
					Animations.PlayClimbAnimation(down);
					myPlayer.cosmetics.AnimateClimb(down);
				}
			}
		});
	}

	public void StartClimb(bool down)
	{
		FlipX = false;
		Animations.PlayClimbAnimation(down);
		myPlayer.cosmetics.AnimateClimb(down);
		myPlayer.cosmetics.TogglePetVisible(visible: false);
		if (bodyType.GetVisorOptions().HideDuringClimb)
		{
			myPlayer.cosmetics.ToggleVisor(active: false);
		}
	}

	private void ClimbLadder(Ladder source, byte climbLadderSid)
	{
		if (NetHelpers.SidGreaterThan(climbLadderSid, lastClimbLadderSid))
		{
			lastClimbLadderSid = climbLadderSid;
			ResetMoveState();
			StartCoroutine(CoClimbLadder(source, climbLadderSid));
		}
	}

	private void PetPet(Vector2 pos, Vector2 petPos)
	{
		petCoroutine = StartCoroutine(CoPet(pos, petPos));
	}

	private IEnumerator CoClimbLadder(Ladder source, byte climbLadderSid)
	{
		float ladderTime = Time.time;
		float totalTime = Time.time;
		myPlayer.moveable = false;
		myPlayer.ForceKillTimerContinue = true;
		myPlayer.onLadder = true;
		myPlayer.NetTransform.enabled = false;
		myPlayer.SetKinematic(b: true);
		myPlayer.NetTransform.ClearPositionQueues();
		if (myPlayer.AmOwner)
		{
			myPlayer.MyPhysics.inputHandler.enabled = true;
		}
		GameServerUtils.DebugEstimatedWalkTo("walkToSeconds (estimated)", transform.position, myPlayer.Collider.offset, source.transform.position, Speed, 0.0005f);
		yield return WalkPlayerTo(source.transform.position, 0.0005f);
		GameServerUtils.DebugTime("walkToSeconds", ref ladderTime, Time.time);
		yield return Effects.Wait(0.1f);
		GameServerUtils.DebugTime("waitSeconds", ref ladderTime, Time.time);
		StartClimb(source.IsTop);
		if (Constants.ShouldPlaySfx() && PlayerControl.LocalPlayer == myPlayer)
		{
			myPlayer.FootSteps.clip = source.UseSound;
			myPlayer.FootSteps.loop = true;
			myPlayer.FootSteps.Play();
		}
		GameServerUtils.DebugEstimatedWalkTo("climbSeconds (estimated)", transform.position, myPlayer.Collider.offset, source.Destination.transform.position, Speed * (float)((!source.IsTop) ? 1 : 2), 0.001f);
		yield return WalkPlayerTo(source.Destination.transform.position, 0.001f, (!source.IsTop) ? 1 : 2);
		GameServerUtils.DebugTime("climbSeconds", ref ladderTime, Time.time);
		myPlayer.SetPetPosition(myPlayer.transform.position);
		ResetAnimState();
		yield return Effects.Wait(0.1f);
		GameServerUtils.DebugTime("waitSeconds", ref ladderTime, Time.time);
		GameServerUtils.DebugTime("totalLadderSeconds (compare this with server log output)", ref totalTime, Time.time);
		if (myPlayer.AmOwner)
		{
			source.SetDestinationCooldown();
		}
		myPlayer.moveable = true;
		myPlayer.NetTransform.enabled = true;
		myPlayer.SetKinematic(b: false);
		myPlayer.ForceKillTimerContinue = false;
		myPlayer.onLadder = false;
	}

	public void ResetMoveState(bool stopCoroutines = true)
	{
		if (stopCoroutines)
		{
			myPlayer?.StopAllCoroutines();
			StopAllCoroutines();
			if ((bool)inputHandler && inputHandler.enabled)
			{
				inputHandler.enabled = false;
			}
		}
		enabled = true;
		if ((bool)myPlayer)
		{
			myPlayer.petting = false;
			myPlayer.inVent = false;
			myPlayer.isKilling = false;
			myPlayer.walkingToVent = false;
			myPlayer.onLadder = false;
			myPlayer.shapeshifting = false;
			myPlayer.waitingForShapeshiftResponse = false;
			myPlayer.cosmetics.SetScale(Animations.DefaultPlayerScale, myPlayer.defaultCosmeticsScale);
			myPlayer.cosmetics.PettingHand.StopPetting();
			myPlayer.NetTransform.enabled = true;
			myPlayer.NetTransform.SetPaused(isPaused: false);
			myPlayer.SetKinematic(b: false);
			if (myPlayer.Data == null)
			{
				logger.Error(myPlayer.name + " has no PlayerInfo");
			}
			myPlayer.Collider.enabled = true;
		}
		else
		{
			logger.Error("Encountered a null myPlayer while Resetting Move State");
		}
		ResetAnimState();
	}

	public void ResetAnimState()
	{
		myPlayer.FootSteps.Stop();
		myPlayer.FootSteps.loop = false;
		myPlayer.cosmetics.SetHatAndVisorIdle(myPlayer.CurrentOutfit.ColorId);
		NetworkedPlayerInfo data = myPlayer.Data;
		if (data == null || !data.IsDead)
		{
			myPlayer.cosmetics.AnimateSkinIdle();
			Animations.PlayIdleAnimation();
			myPlayer.Visible = true;
			myPlayer.SetHatAndVisorAlpha(1f);
			return;
		}
		myPlayer.cosmetics.SetGhost();
		if (data.Role != null)
		{
			if (data.Role.Role == RoleTypes.GuardianAngel)
			{
				Animations.PlayGuardianAngelIdleAnimation();
			}
			else
			{
				Animations.PlayGhostIdleAnimation();
			}
		}
		myPlayer.SetHatAndVisorAlpha(0.5f);
	}

	private void HandleAnimation(bool amDead)
	{
		if (Animations.IsPlayingSpawnAnimation() || DoingCustomAnimation || !GameData.Instance)
		{
			if (body.linearVelocity.sqrMagnitude >= 0.05f && !loggedBlockedAnimationDiagnostics)
			{
				logger.Error($"[PlayerRenderDiag] Movement animation blocked: spawn={Animations.IsPlayingSpawnAnimation()}, custom={DoingCustomAnimation}, gameDataAvailable={(bool)GameData.Instance}, velocity={body.linearVelocity}", this);
				loggedBlockedAnimationDiagnostics = true;
			}
			return;
		}
		Vector2 velocity = body.linearVelocity;
		if (Animations.IsPlayingClimbAnimation())
		{
			if (velocity.sqrMagnitude >= 0.05f && !loggedBlockedAnimationDiagnostics)
			{
				logger.Error($"[PlayerRenderDiag] Movement animation blocked by climb state: isClimbing={Animations.IsPlayingClimbAnimation()}, velocity={velocity}", this);
				loggedBlockedAnimationDiagnostics = true;
			}
			return;
		}
		if (!amDead)
		{
			if (velocity.sqrMagnitude >= 0.05f)
			{
				bool flipX = FlipX;
				if (velocity.x < -0.01f)
				{
					FlipX = true;
				}
				else if (velocity.x > 0.01f)
				{
					FlipX = false;
				}
				if (!Animations.IsPlayingRunAnimation() || flipX != FlipX || !myPlayer.cosmetics.IsSkinPlayingRunAnim())
				{
					Animations.PlayRunAnimation();
					myPlayer.cosmetics.AnimateSkinRun();
				}
			}
			else if (Animations.IsPlayingRunAnimation() || Animations.IsPlayingSpawnAnimation() || !Animations.IsPlayingSomeAnimation())
			{
				myPlayer.cosmetics.AnimateSkinIdle();
				Animations.PlayIdleAnimation();
				myPlayer.SetHatAndVisorAlpha(1f);
			}
			return;
		}
		myPlayer.cosmetics.SetGhost();
		if (myPlayer.Data.Role.Role == RoleTypes.GuardianAngel)
		{
			if (!Animations.IsPlayingGuardianAngelIdleAnimation())
			{
				Animations.PlayGuardianAngelIdleAnimation();
				myPlayer.SetHatAndVisorAlpha(0.5f);
			}
		}
		else if (!Animations.IsPlayingGhostIdleAnimation())
		{
			Animations.PlayGhostIdleAnimation();
			myPlayer.SetHatAndVisorAlpha(0.5f);
		}
		if (velocity.x < -0.01f)
		{
			FlipX = true;
		}
		else if (velocity.x > 0.01f)
		{
			FlipX = false;
		}
	}

	public void AnimateCustom(AnimationClip anim)
	{
		if (!DoingCustomAnimation)
		{
			DoingCustomAnimation = true;
			StartCoroutine(CoAnimateCustom(anim));
		}
	}

	private IEnumerator CoAnimateCustom(AnimationClip anim)
	{
		myPlayer.moveable = false;
		if (myPlayer.AmOwner)
		{
			inputHandler.enabled = true;
		}
		yield return Animations.CoPlayCustomAnimation(anim);
		myPlayer.cosmetics.AnimateSkinIdle();
		Animations.PlayIdleAnimation();
		myPlayer.moveable = true;
		if (myPlayer.AmOwner)
		{
			inputHandler.enabled = false;
		}
		DoingCustomAnimation = false;
		myPlayer.cosmetics.SetBodyCosmeticsVisible(b: true);
	}

	public IEnumerator CoSpawnPlayer(LobbyBehaviour lobby)
	{
		if ((bool)lobby)
		{
			if (myPlayer.AmOwner)
			{
				inputHandler.enabled = true;
			}
			int spawnSeatId = myPlayer.PlayerId % lobby.SpawnPositions.Length;
			Vector3 spawnPos = Vec2ToPosition(lobby.SpawnPositions[spawnSeatId]);
			myPlayer.cosmetics.ToggleName(active: false);
			myPlayer.Collider.enabled = false;
			myPlayer.NetTransform.enabled = false;
			KillAnimation.SetMovement(myPlayer, canMove: false);
			yield return new WaitForFixedUpdate();
			myPlayer.cosmetics.SetForcedVisible(isVisible: true);
			bool amFlipped = spawnSeatId > 4;
			SoundManager.Instance.PlaySound(lobby.SpawnSound, loop: false).volume = 0.75f;
			FlipX = amFlipped;
			myPlayer.cosmetics.AnimateSkinSpawn();
			myPlayer.transform.position = spawnPos;
			yield return Animations.CoPlaySpawnAnimation(FlipX);
			transform.position = spawnPos + new Vector3(amFlipped ? (-0.3f) : 0.3f, -0.24f);
			enabled = true;
			ResetAnimState();
			Vector2 vector = (-spawnPos).normalized;
			yield return WalkPlayerTo((Vector2)spawnPos + vector);
			ResetMoveState(stopCoroutines: false);
			myPlayer.Collider.enabled = true;
			myPlayer.moveable = true;
			myPlayer.NetTransform.ClearPositionQueues();
			myPlayer.cosmetics.ToggleName(active: true);
			if (myPlayer.AmOwner)
			{
				inputHandler.enabled = false;
			}
		}
	}

	public void ExitAllVents()
	{
		ConsoleJoystick.SetMode_Gameplay();
		Vent.currentVent = null;
		ResetMoveState();
		myPlayer.moveable = true;
		Vent[] allVents = ShipStatus.Instance.AllVents;
		for (int i = 0; i < allVents.Length; i++)
		{
			allVents[i].SetButtons(enabled: false);
		}
	}

	private IEnumerator CoPet(Vector2 pos, Vector2 petPos)
	{
		if (myPlayer.AmOwner)
		{
			inputHandler.enabled = true;
		}
		myPlayer.petting = true;
		myPlayer.moveable = false;
		myPlayer.cosmetics.CurrentPet.SetGettingPet(petting: true, petPos);
		yield return WalkPlayerTo(pos);
		body.linearVelocity = Vector2.zero;
		myPlayer.moveable = false;
		FlipX = false;
		myPlayer.cosmetics.AnimateSkinIdle();
		myPlayer.cosmetics.PettingHand.StartPet(myPlayer.cosmetics.CurrentPet);
		yield return myPlayer.cosmetics.PettingHand.WaitUntilFinishedPetting();
		if (myPlayer.AmOwner)
		{
			DataManager.Player.Stats.IncrementStat(StatID.TimesPettedPet);
		}
		CancelPet();
	}

	private void CheckCancelPetting()
	{
		if (myPlayer.cosmetics.PettingHand.gameObject.activeSelf && myPlayer.petting)
		{
			Vector2 deltaL = DestroyableSingleton<HudManager>.Instance.joystick.DeltaL;
			if (DestroyableSingleton<HudManager>.Instance.joystick is ConsoleJoystick)
			{
				Player player = ReInput.players.GetPlayer(0);
				deltaL.x = player.GetAxis(9);
				deltaL.y = player.GetAxis(10);
			}
			if (deltaL.sqrMagnitude > 0f)
			{
				RpcCancelPet();
			}
		}
	}

	private void CancelPet()
	{
		if (petCoroutine != null)
		{
			StopCoroutine(petCoroutine);
		}
		myPlayer.cosmetics.PettingHand.StopPetting();
		myPlayer.moveable = true;
		Animations.PlayIdleAnimation();
		myPlayer.cosmetics.CurrentPet.SetGettingPet(petting: false, myPlayer.cosmetics.CurrentPet.transform.position);
		myPlayer.petting = false;
		if (myPlayer.AmOwner)
		{
			inputHandler.enabled = false;
		}
	}

	private IEnumerator CoEnterVent(int id)
	{
		if (!MeetingHud.Instance)
		{
			Vent vent = ShipStatus.Instance.AllVents.First((Vent v) => v.Id == id);
			myPlayer.NetTransform.SetPaused(isPaused: true);
			if (myPlayer.AmOwner)
			{
				inputHandler.enabled = true;
			}
			myPlayer.walkingToVent = true;
			myPlayer.moveable = false;
			yield return WalkPlayerTo(vent.transform.position + vent.Offset);
			myPlayer.inVent = true;
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.VentUsed(myPlayer.Data);
			vent.EnterVent(myPlayer);
			myPlayer.cosmetics.AnimateSkinEnterVent();
			yield return Animations.CoPlayEnterVentAnimation(vent.NumFramesUntilPlayerDisappears);
			myPlayer.cosmetics.AnimateSkinIdle();
			Animations.PlayIdleAnimation();
			myPlayer.Visible = false;
			myPlayer.walkingToVent = false;
			myPlayer.currentRoleAnimations.ForEach((RoleEffectAnimation an) =>
			{
				an.ToggleRenderer(on: false);
			});
			if (myPlayer.AmOwner)
			{
				VentilationSystem.Update(VentilationSystem.Operation.Enter, id);
				inputHandler.enabled = false;
			}
			logger.Debug($"Player {myPlayer.PlayerId} entered vent {id}");
		}
	}

	private IEnumerator CoExitVent(int id)
	{
		Vent vent = ShipStatus.Instance.AllVents.First((Vent v) => v.Id == id);
		Vector3 position = vent.transform.position;
		position -= (Vector3)myPlayer.Collider.offset;
		myPlayer.NetTransform.SnapTo(position);
		if (myPlayer.AmOwner)
		{
			inputHandler.enabled = true;
			VentilationSystem.Update(VentilationSystem.Operation.Exit, id);
		}
		yield return vent.ExitVent(myPlayer);
		myPlayer.Visible = true;
		myPlayer.inVent = false;
		myPlayer.cosmetics.AnimateSkinExitVent();
		yield return Animations.CoPlayExitVentAnimation();
		myPlayer.cosmetics.AnimateSkinIdle();
		Animations.PlayIdleAnimation();
		myPlayer.moveable = true;
		myPlayer.currentRoleAnimations.ForEach((RoleEffectAnimation an) =>
		{
			an.ToggleRenderer(on: true);
		});
		if (myPlayer.AmOwner)
		{
			inputHandler.enabled = false;
			if (myPlayer.Data.Role.Role == RoleTypes.Engineer)
			{
				myPlayer.Data.Role.SetCooldown();
			}
		}
		myPlayer.NetTransform.SetPaused(isPaused: false);
		logger.Debug($"Player {myPlayer.PlayerId} exited vent {id}");
	}

	public IEnumerator WalkPlayerTo(Vector2 worldPos, float tolerance = 0.01f, float speedMul = 1f, bool ignoreColliderOffset = false)
	{
		if (!ignoreColliderOffset)
		{
			worldPos -= myPlayer.Collider.offset;
		}
		Rigidbody2D body = this.body;
		Vector2 del = worldPos - (Vector2)transform.position;
		while (del.sqrMagnitude > tolerance)
		{
			float num = Mathf.Clamp(del.magnitude * 2f, 0.05f, 1f);
			body.linearVelocity = del.normalized * Speed * num * speedMul;
			yield return null;
			if (body.linearVelocity.magnitude < 0.005f && (double)del.sqrMagnitude < 0.1)
			{
				break;
			}
			del = worldPos - (Vector2)transform.position;
		}
		body.linearVelocity = Vector2.zero;
	}

	public override void ClearOrDecrementDirt()
	{
	}

	public override bool Serialize(MessageWriter writer, bool initialState)
	{
		return false;
	}

	public override void Deserialize(MessageReader reader, bool initialState)
	{
	}

	private void BootFromVent(int ventId)
	{
		if (AmOwner)
		{
			ShipStatus.Instance.AllVents.ForEach((Vent v) =>
			{
				v.SetButtons(enabled: false);
			});
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ImpostorDiscoveredSound, loop: false, 0.8f);
			}
		}
		StopAllCoroutines();
		StartCoroutine(CoExitVent(ventId));
	}

	public void RpcClimbLadder(Ladder source)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			ClimbLadder(source, (byte)(lastClimbLadderSid + 1));
		}
		else
		{
			lastClimbLadderSid++;
		}
		RpcClimbLadderMessage rpcMessage = new RpcClimbLadderMessage(NetId, source.Id, lastClimbLadderSid);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcEnterVent(int ventId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			StopAllCoroutines();
			StartCoroutine(CoEnterVent(ventId));
		}
		RpcEnterVentMessage rpcMessage = new RpcEnterVentMessage(NetId, ventId);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcExitVent(int ventId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			StopAllCoroutines();
			StartCoroutine(CoExitVent(ventId));
		}
		RpcExitVentMessage rpcMessage = new RpcExitVentMessage(NetId, ventId);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcBootFromVent(int ventId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			BootFromVent(ventId);
		}
		RpcBootFromVentMessage rpcMessage = new RpcBootFromVentMessage(NetId, ventId);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcPet(Vector2 pos, Vector2 petPos)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			StopAllCoroutines();
			petCoroutine = StartCoroutine(CoPet(pos, petPos));
		}
		RpcPetMessage rpcMessage = new RpcPetMessage(NetId, pos, petPos);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcCancelPet()
	{
		if (AmongUsClient.Instance.AmClient)
		{
			CancelPet();
		}
		RpcCancelPetMessage rpcMessage = new RpcCancelPetMessage(NetId);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public override void HandleRpc(byte callId, MessageReader reader)
	{
		switch ((RpcCalls)callId)
		{
		case RpcCalls.ClimbLadder:
		{
			ShipStatus instance = ShipStatus.Instance;
			byte ladderId = reader.ReadByte();
			byte climbLadderSid = reader.ReadByte();
			ClimbLadder(instance.Ladders.First((Ladder f) => f.Id == ladderId), climbLadderSid);
			break;
		}
		case RpcCalls.EnterVent:
		{
			int id2 = reader.ReadPackedInt32();
			StopAllCoroutines();
			StartCoroutine(CoEnterVent(id2));
			break;
		}
		case RpcCalls.ExitVent:
		{
			int id = reader.ReadPackedInt32();
			StopAllCoroutines();
			StartCoroutine(CoExitVent(id));
			break;
		}
		case RpcCalls.BootFromVent:
		{
			int ventId = reader.ReadPackedInt32();
			BootFromVent(ventId);
			break;
		}
		case RpcCalls.Pet:
		{
			Vector2 pos = NetHelpers.ReadVector2(reader);
			Vector2 petPos = NetHelpers.ReadVector2(reader);
			PetPet(pos, petPos);
			break;
		}
		case RpcCalls.CancelPet:
			CancelPet();
			break;
		}
	}
}
