using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AmongUs.Data;
using AmongUs.GameOptions;
using AmongUs.InnerNet.GameDataMessages;
using AmongUs.QuickChat;
using Assets.CoreScripts;
using Hazel;
using InnerNet;
using Innersloth.Assets;
using PowerTools;
using UnityEngine;

public class PlayerControl : InnerNetObject
{
	public class ColliderComparer : IEqualityComparer<Collider2D>
	{
		public static readonly ColliderComparer Instance = new ColliderComparer();

		public bool Equals(Collider2D x, Collider2D y)
		{
			return x == y;
		}

		public int GetHashCode(Collider2D obj)
		{
			return obj.GetEntityId().GetHashCode();
		}
	}

	public class UsableComparer : IEqualityComparer<IUsable>
	{
		public static readonly UsableComparer Instance = new UsableComparer();

		public bool Equals(IUsable x, IUsable y)
		{
			return x == y;
		}

		public int GetHashCode(IUsable obj)
		{
			return obj.GetHashCode();
		}
	}

	private const int MAX_NETOBJECT_INIT_ATTEMPTS = 60;

	private const float NETOBJECT_INIT_ATTEMPT_WAIT_SECONDS = 0.1f;

	public byte PlayerId = byte.MaxValue;

	public string FriendCode;

	public string Puid;

	public float MaxReportDistance = 5f;

	public bool moveable = true;

	public CosmeticsLayer cosmetics;

	[HideInInspector]
	public bool ForceKillTimerContinue;

	public bool inVent;

	public bool walkingToVent;

	public bool petting;

	public bool inMovingPlat;

	public bool onLadder;

	public bool protectedByGuardianThisRound;

	public bool shapeshifting;

	public bool waitingForShapeshiftResponse;

	public bool isKilling;

	private float invisibilityAlpha = 1f;

	private const float OFFSET_ANIM_CHARGE = -0.05f;

	public static PlayerControl LocalPlayer;

	private NetworkedPlayerInfo CachedPlayerData;

	private int protectedByGuardianId = -1;

	private float flashlightAngle;

	private int shapeshiftTargetPlayerId = -1;

	private bool shouldAppearInvisible;

	public bool isTrackingPlayer;

	public PlayerControl trackedPlayer;

	public int trackedPlayerColorID;

	public AudioSource FootSteps;

	public AudioClip KillSfx;

	public KillAnimation[] KillAnimations;

	[SerializeField]
	private float killTimer;

	public int RemainingEmergencies;

	public LightSource LightPrefab;

	private LightSource lightSource;

	[HideInInspector]
	public Collider2D Collider;

	[HideInInspector]
	public PlayerPhysics MyPhysics;

	[HideInInspector]
	public CustomNetworkTransform NetTransform;

	[SerializeField]
	private Collider2D clickKillCollider;

	public Vector3 defaultCosmeticsScale = new Vector3(0.5f, 0.5f, 1f);

	public List<PlayerTask> myTasks = new List<PlayerTask>();

	public List<RoleEffectAnimation> currentRoleAnimations;

	public GameObject TargetFlashlight;

	public bool isDummy;

	public bool notRealPlayer;

	[SerializeField]
	private Transform socialMediumFeedTransform;

	private readonly Logger logger = new Logger(Logger.Category.Gameplay, "PlayerControl");

	private readonly List<IPlayerVisibleItem> visibilityItems = new List<IPlayerVisibleItem>();

	private Collider2D[] hitBuffer = new Collider2D[60];

	private IUsable closest;

	private bool isNew = true;

	private bool hasBeenSerialized;

	private Rigidbody2D rigidbody2D;

	public static List<PlayerControl> AllPlayerControls = new List<PlayerControl>();

	private Dictionary<Collider2D, IUsable[]> cache = new Dictionary<Collider2D, IUsable[]>(ColliderComparer.Instance);

	private List<IUsable> itemsInRange = new List<IUsable>();

	private List<IUsable> newItemsInRange = new List<IUsable>();

	private byte scannerCount;

	private bool roleAssigned;

	private int LastStartCounter;

	public bool CanMove
	{
		get
		{
			if (moveable && !inVent && !MyPhysics.DoingCustomAnimation && !shapeshifting && !waitingForShapeshiftResponse && !Minigame.Instance && (!DestroyableSingleton<HudManager>.InstanceExists || (!DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening && !DestroyableSingleton<HudManager>.Instance.KillOverlay.IsOpen && !DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen && !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed)) && (!ControllerManager.Instance || !ControllerManager.Instance.IsUiControllerActive) && (!MapBehaviour.Instance || !MapBehaviour.Instance.IsOpenStopped) && !MeetingHud.Instance && !PlayerCustomizationMenu.Instance && !ExileController.Instance)
			{
				return !IntroCutscene.Instance;
			}
			return false;
		}
	}

	public bool IsKillTimerEnabled
	{
		get
		{
			if ((moveable || petting) && !inVent && !shapeshifting && (!DestroyableSingleton<HudManager>.InstanceExists || (!DestroyableSingleton<HudManager>.Instance.KillOverlay.IsOpen && !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed)) && !MeetingHud.Instance && !PlayerCustomizationMenu.Instance && !ExileController.Instance)
			{
				return !IntroCutscene.Instance;
			}
			return false;
		}
	}

	public NetworkedPlayerInfo.PlayerOutfit CurrentOutfit
	{
		get
		{
			if (Data == null)
			{
				logger.Warning("Tried to access CurrentOutfit before Data was set");
				return new NetworkedPlayerInfo.PlayerOutfit();
			}
			return Data.Outfits[CurrentOutfitType];
		}
	}

	public PlayerOutfitType CurrentOutfitType { get; private set; }

	public Transform SocialMediumFeedTransform => socialMediumFeedTransform;

	public float CalculatedAlpha => invisibilityAlpha;

	public float FlashlightAngle
	{
		get
		{
			return flashlightAngle;
		}
		set
		{
			if (value != flashlightAngle)
			{
				flashlightAngle = value;
			}
		}
	}

	public bool PhantomFadeActive => invisibilityAlpha < 1f;

	public NetworkedPlayerInfo Data
	{
		get
		{
			if (CachedPlayerData == null)
			{
				if (!GameData.Instance)
				{
					return null;
				}
				CachedPlayerData = GameData.Instance.GetPlayerById(PlayerId);
			}
			return CachedPlayerData;
		}
	}

	public bool Visible
	{
		get
		{
			if (cosmetics.Visible)
			{
				return !shouldAppearInvisible;
			}
			return false;
		}
		set
		{
			cosmetics.Visible = value && !shouldAppearInvisible;
			foreach (IPlayerVisibleItem visibilityItem in visibilityItems)
			{
				if (visibilityItem is MushroomMixupPlayerAnimation && (LocalPlayer.Data.Role.IsImpostor || LocalPlayer.Data.IsDead))
				{
					visibilityItem.Visible = true;
				}
				else
				{
					visibilityItem.Visible = value && !shouldAppearInvisible;
				}
			}
		}
	}

	public PlayerBodyTypes BodyType => GameManager.Instance.GetBodyType(this);

	public void SetKillTimer(float time)
	{
		if (Data.Role.CanUseKillButton)
		{
			float num = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
			if (!(num <= 0f))
			{
				killTimer = Mathf.Clamp(time, 0f, num);
				DestroyableSingleton<HudManager>.Instance.KillButton.SetCoolDown(killTimer, num);
			}
		}
	}

	private void Awake()
	{
		MyPhysics = GetComponent<PlayerPhysics>();
		NetTransform = GetComponent<CustomNetworkTransform>();
		Collider = GetComponent<Collider2D>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		currentRoleAnimations = new List<RoleEffectAnimation>();
		if (!notRealPlayer)
		{
			AllPlayerControls.Add(this);
		}
	}

	private void OnEnable()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged += SetColorBlindTag;
		DataManager.Settings.Input.OnInputModeChanged += AdjustLighting;
		AdjustLighting();
	}

	private void OnDisable()
	{
		DataManager.Settings.Accessibility.OnColorBlindModeChanged -= SetColorBlindTag;
		DataManager.Settings.Input.OnInputModeChanged -= AdjustLighting;
	}

	private IEnumerator AssertWithTimeout(Func<bool> assertion, Action onTimeout, float timeoutInSeconds)
	{
		bool failed = true;
		for (float timer = 0f; timer < timeoutInSeconds; timer += Time.deltaTime)
		{
			if (assertion())
			{
				failed = false;
				break;
			}
			yield return null;
		}
		if (failed)
		{
			onTimeout?.Invoke();
		}
	}

	private IEnumerator Start()
	{
		yield return AssertWithTimeout(() => PlayerId != byte.MaxValue, () =>
		{
			AmongUsClient.Instance.EnqueueDisconnect(DisconnectReasons.Error, "Timeout while waiting for player ID assignment");
		}, 30f);
		yield return AssertWithTimeout(() => GameManager.Instance != null && GameData.Instance != null && Data != null, () =>
		{
			AmongUsClient.Instance.EnqueueDisconnect(DisconnectReasons.Error, "Timeout while waiting for player data containers");
		}, 30f);
		RemainingEmergencies = GameManager.Instance.LogicOptions.GetNumEmergencyMeetings();
		SetColorBlindTag();
		cosmetics.UpdateVisibility();
		if (AmOwner)
		{
			lightSource = UnityEngine.Object.Instantiate(LightPrefab, transform, worldPositionStays: false);
			lightSource.Initialize(Collider.offset * 0.5f);
			LocalPlayer = this;
			cosmetics.SetAsLocalPlayer();
			Camera.main.GetComponent<FollowerCamera>().SetTarget(this);
			SetName(DataManager.Player.Customization.Name);
			SetColor(DataManager.Player.Customization.Color);
			if (Application.targetFrameRate > 30)
			{
				MyPhysics.EnableInterpolation();
			}
			CmdCheckName(DataManager.Player.Customization.Name);
			CmdCheckColor(DataManager.Player.Customization.Color);
			RpcSetPet(DataManager.Player.Customization.Pet);
			RpcSetHat(DataManager.Player.Customization.Hat);
			RpcSetSkin(DataManager.Player.Customization.Skin);
			if (DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat).BlocksVisors)
			{
				DataManager.Player.Customization.Visor = "visor_EmptyVisor";
			}
			RpcSetVisor(DataManager.Player.Customization.Visor);
			RpcSetNamePlate(DataManager.Player.Customization.NamePlate);
			RpcSetLevel(DataManager.Player.Stats.Level);
			yield return null;
		}
		else
		{
			StartCoroutine(ClientInitialize());
		}
		if (!Data.Role)
		{
			Data.Role = UnityEngine.Object.Instantiate(GameData.Instance.DefaultRole);
			Data.Role.Initialize(this);
		}
		MyPhysics.SetBodyType(BodyType);
		if (isNew)
		{
			isNew = false;
			StartCoroutine(MyPhysics.CoSpawnPlayer(LobbyBehaviour.Instance));
		}
		if (LocalPlayer == this)
		{
			clickKillCollider.enabled = false;
		}
	}

	private IEnumerator ClientInitialize()
	{
		Visible = false;
		yield return AssertWithTimeout(() => GameData.Instance != null && Data != null && !Data.IsIncomplete, () =>
		{
			AmongUsClient.Instance.EnqueueDisconnect(DisconnectReasons.Error, "Timeout while waiting for other player data");
		}, 30f);
		NetworkedPlayerInfo.PlayerOutfit defaultOutfit = Data.DefaultOutfit;
		SetName(defaultOutfit.PlayerName);
		SetColor(defaultOutfit.ColorId);
		SetHat(defaultOutfit.HatId, defaultOutfit.ColorId);
		SetSkin(defaultOutfit.SkinId, defaultOutfit.ColorId);
		SetPet(defaultOutfit.PetId);
		SetVisor(defaultOutfit.VisorId, defaultOutfit.ColorId);
		SetNamePlate(defaultOutfit.NamePlateId);
		SetLevel(Data.PlayerLevel);
		Visible = true;
		yield return null;
	}

	public override void OnDestroy()
	{
		if (!notRealPlayer)
		{
			AllPlayerControls.Remove(this);
		}
		base.OnDestroy();
	}

	private void FixedUpdate()
	{
		if (!GameData.Instance)
		{
			return;
		}
		NetworkedPlayerInfo data = Data;
		if (data == null || data.Role == null)
		{
			return;
		}
		if (data.IsDead && (bool)LocalPlayer && LocalPlayer.Data != null)
		{
			Visible = LocalPlayer.Data.IsDead;
			cosmetics.SetPetVisible(v: true);
		}
		if (!AmOwner)
		{
			return;
		}
		if ((bool)ShipStatus.Instance && (bool)lightSource)
		{
			float num = ShipStatus.Instance.CalculateLightRadius(data);
			if (!Mathf.Approximately(num, lightSource.ViewDistance))
			{
				AdjustLighting();
			}
			lightSource.SetViewDistance(num);
		}
		PlayerControl playerControl = data.Role.FindClosestTarget();
		bool flag = data.Role.CanUseKillButton && !data.IsDead;
		if ((IsKillTimerEnabled || ForceKillTimerContinue) & flag)
		{
			SetKillTimer(killTimer - Time.fixedDeltaTime);
			DestroyableSingleton<HudManager>.Instance.KillButton.SetTarget(playerControl);
		}
		else
		{
			DestroyableSingleton<HudManager>.Instance.KillButton.SetTarget(null);
			Data.Role.SetPlayerTarget(playerControl);
		}
		if (CanMove && LocalPlayer != null && LocalPlayer.MyPhysics.inputHandler.enabled)
		{
			LocalPlayer.MyPhysics.inputHandler.enabled = false;
		}
		if (CanMove || inVent)
		{
			newItemsInRange.Clear();
			bool flag2 = (GameOptionsManager.Instance.CurrentGameOptions.GetBool(BoolOptionNames.GhostsDoTasks) || !data.IsDead) && (!AmongUsClient.Instance || !AmongUsClient.Instance.IsGameOver) && CanMove;
			Vector2 truePosition = GetTruePosition();
			int num2 = Physics2D.OverlapCircleNonAlloc(truePosition, MaxReportDistance, hitBuffer, Constants.Usables);
			IUsable usable = null;
			float num3 = float.MaxValue;
			bool flag3 = false;
			List<Vent> list = new List<Vent>();
			for (int i = 0; i < num2; i++)
			{
				Collider2D collider2D = hitBuffer[i];
				if (!cache.TryGetValue(collider2D, out var value))
				{
					IUsable[] array = (cache[collider2D] = collider2D.GetComponents<IUsable>());
					value = array;
				}
				if (value != null && (flag2 || inVent))
				{
					IUsable[] array = value;
					foreach (IUsable usable2 in array)
					{
						float num4 = usable2.CanUse(data, out var canUse, out var couldUse);
						if (canUse | couldUse)
						{
							newItemsInRange.Add(usable2);
						}
						if (canUse && num4 < num3)
						{
							if (usable2 is Vent)
							{
								list.Add(usable2 as Vent);
								continue;
							}
							num3 = num4;
							usable = usable2;
						}
					}
				}
				if (flag2 && !data.IsDead && !flag3 && collider2D.tag == "DeadBody")
				{
					DeadBody component = collider2D.GetComponent<DeadBody>();
					if (component.enabled && !component.Reported && Vector2.Distance(truePosition, component.TruePosition) <= MaxReportDistance && !PhysicsHelpers.AnythingBetween(truePosition, component.TruePosition, Constants.ShipAndObjectsMask, useTriggers: false))
					{
						flag3 = true;
					}
				}
			}
			Vent vent = ((list.Count > 0) ? list.First() : null);
			for (int num5 = itemsInRange.Count - 1; num5 > -1; num5--)
			{
				IUsable item = itemsInRange[num5];
				int num6 = newItemsInRange.FindIndex((IUsable usable4) => usable4 == item);
				if (num6 == -1)
				{
					item.SetOutline(on: false, mainTarget: false);
					itemsInRange.RemoveAt(num5);
				}
				else
				{
					newItemsInRange.RemoveAt(num6);
					bool flag4 = false;
					flag4 = ((!(item is Vent)) ? (usable == item) : (item as Vent == vent));
					item.SetOutline(on: true, flag4);
				}
			}
			for (int num7 = 0; num7 < newItemsInRange.Count; num7++)
			{
				IUsable usable3 = newItemsInRange[num7];
				bool flag5 = false;
				flag5 = ((!(usable3 is Vent)) ? (usable == usable3) : (usable3 as Vent == vent));
				usable3.SetOutline(on: true, flag5);
				itemsInRange.Add(usable3);
			}
			closest = usable;
			DestroyableSingleton<HudManager>.Instance.ToggleUseAndPetButton(usable, flag2, CanPet());
			DestroyableSingleton<HudManager>.Instance.ReportButton.SetActive(flag3);
			DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.SetTarget(vent);
			Data.Role.SetUsableTarget(vent);
		}
		else
		{
			closest = null;
			DestroyableSingleton<HudManager>.Instance.UseButton.SetTarget(null);
			DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.SetTarget(Vent.currentVent);
			DestroyableSingleton<HudManager>.Instance.PetButton.SetDisabled();
			DestroyableSingleton<HudManager>.Instance.ReportButton.SetActive(isActive: false);
			Data.Role.SetUsableTarget(Vent.currentVent);
			if ((bool)PlayerCustomizationMenu.Instance)
			{
				DestroyableSingleton<HudManager>.Instance.UseButton.gameObject.SetActive(value: false);
			}
		}
		DestroyableSingleton<HudManager>.Instance.SabotageButton.Refresh();
		DestroyableSingleton<HudManager>.Instance.AdminButton.Refresh();
	}

	public void AnimateCustom(AnimationClip anim)
	{
		MyPhysics.AnimateCustom(anim);
	}

	public void OnGameStart()
	{
		cosmetics.ToggleNameVisible(GameManager.Instance.LogicOptions.GetShowCrewmateNames());
		if (cosmetics.CurrentPet != null)
		{
			cosmetics.CurrentPet.SetGettingPet(petting: false, cosmetics.CurrentPet.transform.position);
		}
	}

	public void OnGameEnd()
	{
		cosmetics.ToggleNameVisible(visible: true);
	}

	public void UseClosest()
	{
		if (closest != null)
		{
			closest.Use();
			closest = null;
			DestroyableSingleton<HudManager>.Instance.UseButton.SetTarget(null);
		}
	}

	public void RegisterVisibilityItem(IPlayerVisibleItem obj)
	{
		visibilityItems.Add(obj);
		if (obj is MushroomMixupPlayerAnimation && (LocalPlayer.Data.Role.IsImpostor || LocalPlayer.Data.IsDead))
		{
			obj.Visible = true;
		}
		else
		{
			obj.Visible = Visible;
		}
	}

	public void UnregisterVisibilityItem(IPlayerVisibleItem obj)
	{
		visibilityItems.Remove(obj);
	}

	public void TryPet()
	{
		if (CanPet() && !PhysicsHelpers.AnythingBetween(Collider, Collider.bounds.center, cosmetics.CurrentPet.PettingPlayerPosition, Constants.ShipAndAllObjectsMask, useTriggers: false))
		{
			MyPhysics.RpcPet(cosmetics.CurrentPet.PettingPlayerPosition, cosmetics.CurrentPet.transform.position);
		}
		closest = null;
		DestroyableSingleton<HudManager>.Instance.PetButton.SetTarget();
	}

	public void ReportClosest()
	{
		if (AmongUsClient.Instance.IsGameOver || LocalPlayer.Data.IsDead)
		{
			return;
		}
		Collider2D[] array = Physics2D.OverlapCircleAll(GetTruePosition(), MaxReportDistance, Constants.PlayersOnlyMask);
		foreach (Collider2D collider2D in array)
		{
			if (collider2D.tag != "DeadBody")
			{
				continue;
			}
			DeadBody component = collider2D.GetComponent<DeadBody>();
			if ((bool)component && !component.Reported)
			{
				component.OnClick();
				if (component.Reported)
				{
					break;
				}
			}
		}
	}

	public void PlayStepSound()
	{
		if (!Constants.ShouldPlaySfx() || LocalPlayer != this)
		{
			return;
		}
		if ((bool)LobbyBehaviour.Instance)
		{
			for (int i = 0; i < LobbyBehaviour.Instance.AllRooms.Length; i++)
			{
				SoundGroup soundGroup = LobbyBehaviour.Instance.AllRooms[i].MakeFootstep(this);
				if ((bool)soundGroup)
				{
					AudioClip clip = soundGroup.Random();
					FootSteps.clip = clip;
					FootSteps.Play();
					break;
				}
			}
		}
		if (!ShipStatus.Instance)
		{
			return;
		}
		for (int j = 0; j < ShipStatus.Instance.AllStepWatchers.Length; j++)
		{
			SoundGroup soundGroup2 = ShipStatus.Instance.AllStepWatchers[j].MakeFootstep(this);
			if ((bool)soundGroup2)
			{
				AudioClip clip2 = soundGroup2.Random();
				FootSteps.clip = clip2;
				FootSteps.Play();
				break;
			}
		}
	}

	private void SetScanner(bool on, byte cnt)
	{
		if (cnt >= scannerCount)
		{
			scannerCount = cnt;
			MyPhysics.Animations.PlayScanner(on, Data.IsDead, cosmetics.FlipX);
		}
	}

	public Vector2 GetTruePosition()
	{
		return (Vector2)transform.position + Collider.offset;
	}

	public void SetTasks(List<NetworkedPlayerInfo.TaskInfo> tasks)
	{
		StartCoroutine(CoSetTasks(tasks));
	}

	private IEnumerator CoSetTasks(List<NetworkedPlayerInfo.TaskInfo> tasks)
	{
		while (!ShipStatus.Instance)
		{
			yield return null;
		}
		if (AmOwner)
		{
			DestroyableSingleton<HudManager>.Instance.TaskStuff.SetActive(value: true);
			DataManager.Player.Stats.IncrementStat(StatID.GamesStarted);
			if (!DestroyableSingleton<TutorialManager>.InstanceExists)
			{
				DestroyableSingleton<AchievementManager>.Instance.OnMatchStart(Data.Role.Role);
			}
		}
		myTasks.DestroyAll();
		Data.Role.SpawnTaskHeader(this);
		for (int i = 0; i < tasks.Count; i++)
		{
			NetworkedPlayerInfo.TaskInfo taskInfo = tasks[i];
			NormalPlayerTask taskById = ShipStatus.Instance.GetTaskById(taskInfo.TypeId);
			NormalPlayerTask normalPlayerTask = UnityEngine.Object.Instantiate(taskById, transform);
			normalPlayerTask.Id = taskInfo.Id;
			normalPlayerTask.Index = taskById.Index;
			normalPlayerTask.Owner = this;
			normalPlayerTask.Initialize();
			logger.Info($"Assigned task {normalPlayerTask.name} to {PlayerId}");
			myTasks.Add(normalPlayerTask);
		}
	}

	public PlayerTask AddSystemTask(SystemTypes system)
	{
		if (!AmOwner)
		{
			return null;
		}
		PlayerTask playerTask = UnityEngine.Object.Instantiate(ShipStatus.Instance.GetSabotageTask(system), transform);
		playerTask.Id = 255u;
		playerTask.Owner = this;
		playerTask.Initialize();
		myTasks.Add(playerTask);
		return playerTask;
	}

	public void RemoveTask(PlayerTask task)
	{
		task.OnRemove();
		myTasks.Remove(task);
		GameData.Instance.TutOnlyRemoveTask(PlayerId, task.Id);
		DestroyableSingleton<HudManager>.Instance.UseButton.SetTarget(null);
		UnityEngine.Object.Destroy(task.gameObject);
	}

	public void ClearTasks()
	{
		for (int i = 0; i < myTasks.Count; i++)
		{
			PlayerTask playerTask = myTasks[i];
			playerTask.OnRemove();
			UnityEngine.Object.Destroy(playerTask.gameObject);
		}
		myTasks.Clear();
	}

	public void Die(DeathReason reason, bool assignGhostRole)
	{
		logger.Debug($"Player {PlayerId} dying for reason {reason}");
		if (!DestroyableSingleton<TutorialManager>.InstanceExists && AmOwner)
		{
			DataManager.Player.Ban.BanPoints--;
			DataManager.Player.Ban.PreviousGameStartDate = DateTime.MinValue;
		}
		GameData.LastDeathReason = reason;
		if (inMovingPlat && ShipStatus.Instance is FungleShipStatus fungleShipStatus)
		{
			fungleShipStatus.Zipline.CancelZiplineUseForPlayer(this);
		}
		cosmetics.AnimatePetMourning();
		FixMixedUpOutfit();
		Data.IsDead = true;
		clickKillCollider.enabled = false;
		gameObject.layer = LayerMask.NameToLayer("Ghost");
		cosmetics.SetNameMask(alive: false);
		cosmetics.PettingHand.StopPetting();
		if (walkingToVent)
		{
			inVent = false;
			Vent.currentVent = null;
			moveable = true;
			MyPhysics.StopAllCoroutines();
		}
		Data.Role.OnDeath(reason);
		GameManager.Instance.OnPlayerDeath(this, assignGhostRole);
		if (!AmOwner)
		{
			return;
		}
		DestroyableSingleton<HudManager>.Instance.Chat.SetVisible(visible: true);
		DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(value: false);
		AdjustLighting();
		foreach (PlayerControl allPlayerControl in AllPlayerControls)
		{
			allPlayerControl.cosmetics.ToggleNameVisible(GameManager.Instance.LogicOptions.GetShowCrewmateNames());
		}
	}

	public void Revive()
	{
		Data.IsDead = false;
		gameObject.layer = LayerMask.NameToLayer("Players");
		MyPhysics.ResetMoveState();
		clickKillCollider.enabled = true;
		cosmetics.SetPetSource(this);
		cosmetics.SetNameMask(alive: true);
		if (AmOwner)
		{
			DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(value: true);
			DestroyableSingleton<HudManager>.Instance.KillButton.ToggleVisible(Data.Role.IsImpostor);
			DestroyableSingleton<HudManager>.Instance.AdminButton.ToggleVisible(Data.Role.IsImpostor);
			DestroyableSingleton<HudManager>.Instance.SabotageButton.ToggleVisible(Data.Role.IsImpostor);
			DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.ToggleVisible(Data.Role.IsImpostor);
			DestroyableSingleton<HudManager>.Instance.Chat.ForceClosed();
			DestroyableSingleton<HudManager>.Instance.Chat.SetVisible(visible: false);
		}
	}

	public void OnClick()
	{
		if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek && LocalPlayer.Data.RoleType == RoleTypes.Impostor && Data.RoleType != RoleTypes.Impostor)
		{
			DestroyableSingleton<HudManager>.Instance.KillButton.CheckClick(this);
		}
	}

	public void CheckSeekerHand_AnimEvent()
	{
		if (cosmetics.HasHat())
		{
			cosmetics.HatHand.gameObject.SetActive(value: true);
			cosmetics.ToggleHat(b: true);
		}
	}

	public void PlayAnimation(byte animType)
	{
		switch ((TaskTypes)animType)
		{
		case TaskTypes.ClearAsteroids:
			ShipStatus.Instance.FireWeapon();
			break;
		case TaskTypes.PrimeShields:
			ShipStatus.Instance.StartShields();
			break;
		case TaskTypes.EmptyChute:
		case TaskTypes.EmptyGarbage:
			ShipStatus.Instance.OpenHatch();
			break;
		}
	}

	public void CompleteTask(uint idx)
	{
		PlayerTask playerTask = myTasks.Find((PlayerTask p) => p.Id == idx);
		if ((bool)playerTask)
		{
			if (AmOwner && Data.Role.Role == RoleTypes.Scientist && (Data.Role as ScientistRole).Recharge())
			{
				DataManager.Player.Stats.IncrementStat(StatID.Role_Scientist_ChargesGained);
			}
			GameData.Instance.CompleteTask(this, idx);
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.TaskCompleted(Data, playerTask);
			playerTask.Complete();
			logger.Debug($"Player {PlayerId} completed task {playerTask.TaskType}");
		}
		else
		{
			logger.Warning(PlayerId + ": Server didn't have task: " + idx);
		}
		GameManager.Instance.FinishTask(playerTask);
	}

	public bool AllTasksCompleted()
	{
		for (int i = 0; i < myTasks.Count; i++)
		{
			PlayerTask playerTask = myTasks[i];
			if (playerTask is NormalPlayerTask && !playerTask.IsComplete)
			{
				return false;
			}
		}
		return true;
	}

	public bool MustCleanVent(int ventId)
	{
		if (!Data.Role.TasksCountTowardProgress)
		{
			return false;
		}
		NormalPlayerTask normalPlayerTask = myTasks.Find((PlayerTask p) => p.TaskType == TaskTypes.VentCleaning) as NormalPlayerTask;
		if (normalPlayerTask == null)
		{
			return false;
		}
		if (!normalPlayerTask.IsComplete)
		{
			return normalPlayerTask.Data[0] == ventId;
		}
		return false;
	}

	private IEnumerator CoSetRole(RoleTypes role, bool canOverride)
	{
		bool ghostRole = RoleManager.IsGhostRole(role);
		if (!DestroyableSingleton<TutorialManager>.InstanceExists && roleAssigned && !ghostRole)
		{
			yield break;
		}
		if (!canOverride)
		{
			roleAssigned = true;
		}
		int attempts = 0;
		while ((!Data || GameManager.Instance == null || !GameManager.Instance) && attempts < 60)
		{
			attempts++;
			yield return Effects.Wait(0.1f);
		}
		if (!Data)
		{
			Debug.LogWarning("CoSetRole timed out waiting for NetworkedPlayerInfo");
			yield break;
		}
		if (GameManager.Instance == null || !GameManager.Instance)
		{
			Debug.LogWarning("CoSetRole timed out waiting for GameManager");
			yield break;
		}
		if (ghostRole)
		{
			DestroyableSingleton<RoleManager>.Instance.SetRole(this, role);
			Data.Role.SpawnTaskHeader(this);
			if (AmOwner)
			{
				DestroyableSingleton<HudManager>.Instance.ReportButton.gameObject.SetActive(value: false);
			}
			yield break;
		}
		RemainingEmergencies = GameManager.Instance.LogicOptions.GetNumEmergencyMeetings();
		DestroyableSingleton<RoleManager>.Instance.SetRole(this, role);
		Data.Role.SpawnTaskHeader(this);
		MyPhysics.SetBodyType(BodyType);
		if (AmOwner)
		{
			if (Data.Role.IsImpostor)
			{
				DataManager.Player.Stats.IncrementStat(StatID.GamesAsImpostor);
				DataManager.Player.Stats.ResetStat(StatID.CrewmateStreak);
			}
			else
			{
				DataManager.Player.Stats.IncrementStat(StatID.GamesAsCrewmate);
				DataManager.Player.Stats.IncrementStat(StatID.CrewmateStreak);
			}
			DestroyableSingleton<HudManager>.Instance.MapButton.gameObject.SetActive(value: true);
			DestroyableSingleton<HudManager>.Instance.MatchInfoButton.gameObject.SetActive(value: true);
			DestroyableSingleton<HudManager>.Instance.ReportButton.gameObject.SetActive(value: true);
			DestroyableSingleton<HudManager>.Instance.UseButton.gameObject.SetActive(value: true);
		}
		if (!DestroyableSingleton<TutorialManager>.InstanceExists && AllPlayerControls.All((PlayerControl pc) => pc.Data != null && (pc.roleAssigned || pc.Data.Disconnected)))
		{
			AllPlayerControls.ForEach((PlayerControl pc) =>
			{
				PlayerNameColor.Set(pc);
			});
			StopAllCoroutines();
			DestroyableSingleton<HudManager>.Instance.StartCoroutine(DestroyableSingleton<HudManager>.Instance.CoShowIntro());
			DestroyableSingleton<HudManager>.Instance.HideGameLoader();
		}
	}

	public void Exiled()
	{
		Die(DeathReason.Exile, assignGhostRole: true);
		if (AmOwner)
		{
			DataManager.Player.Stats.IncrementStat(StatID.TimesEjected);
		}
	}

	public void CheckName(string playerName)
	{
		if (!Data)
		{
			Debug.LogWarning("CheckName was called while NetworkedPlayerInfo was null");
			return;
		}
		List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
		if (allPlayers.Any((NetworkedPlayerInfo i) => i.PlayerId != PlayerId && i.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase)))
		{
			for (int num = 1; num < 100; num++)
			{
				string text = playerName + " " + num;
				bool flag = false;
				for (int num2 = 0; num2 < allPlayers.Count; num2++)
				{
					if (allPlayers[num2].PlayerId != PlayerId && allPlayers[num2].PlayerName.Equals(text, StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					playerName = text;
					break;
				}
			}
		}
		Data.PlayerName = playerName;
		Data.CensorNameAsync(() =>
		{
			RpcSetName(Data.PlayerName);
		});
	}

	public void SetKinematic(bool b)
	{
		rigidbody2D.isKinematic = b;
	}

	public void SetName(string playerName)
	{
		StartCoroutine(CoSetName(playerName));
	}

	private IEnumerator CoSetName(string playerName)
	{
		int attempts = 0;
		while (!Data && attempts < 60)
		{
			attempts++;
			yield return Effects.Wait(0.1f);
		}
		if (!Data)
		{
			Debug.LogWarning("CoSetName timed out waiting for NetworkedPlayerInfo");
			yield break;
		}
		Data.PlayerName = playerName;
		Data.CensorNameAsync(() =>
		{
			RawSetName(Data.PlayerName);
		});
	}

	public void CheckColor(byte bodyColor)
	{
		List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
		int num = 0;
		while (num++ < 100 && (bodyColor >= Palette.PlayerColors.Length || allPlayers.Any((NetworkedPlayerInfo p) => !p.Disconnected && p.PlayerId != PlayerId && p.DefaultOutfit.ColorId == bodyColor)))
		{
			bodyColor = (byte)((bodyColor + 1) % Palette.PlayerColors.Length);
		}
		RpcSetColor(bodyColor);
	}

	public void SetHatAndVisorAlpha(float a)
	{
		Color white = Color.white;
		white.a = a;
		cosmetics.SetHatColor(white);
		cosmetics.SetVisorAlpha(a);
	}

	public void SetColor(int bodyColor)
	{
		StartCoroutine(CoSetColor(bodyColor));
	}

	private IEnumerator CoSetColor(int bodyColor)
	{
		int attempts = 0;
		while (!Data && attempts < 60)
		{
			attempts++;
			yield return Effects.Wait(0.1f);
		}
		if (!Data)
		{
			Debug.LogWarning("CoSetColor timed out waiting for NetworkedPlayerInfo");
			yield break;
		}
		Data.UpdateColor(bodyColor);
		RawSetColor(bodyColor);
	}

	public void SetNamePlate(string namePlateId)
	{
		Data?.UpdateNamePlate(namePlateId);
	}

	public void SetVisor(string visorId, int colorId)
	{
		Data?.UpdateVisor(visorId);
		RawSetVisor(visorId, colorId);
	}

	public void SetLevel(uint level)
	{
		StartCoroutine(CoSetLevel(level));
	}

	private IEnumerator CoSetLevel(uint level)
	{
		int attempts = 0;
		while (!Data && attempts < 60)
		{
			attempts++;
			yield return Effects.Wait(0.1f);
		}
		if (!Data)
		{
			Debug.LogWarning("CoSetLevel timed out waiting for NetworkedPlayerInfo");
		}
		else
		{
			Data.UpdateLevel(level);
		}
	}

	public void SetSkin(string skinId, int color)
	{
		Data?.UpdateSkin(skinId);
		RawSetSkin(skinId, color);
	}

	public void SetHat(string hatId, int colorId)
	{
		Data?.UpdateHat(hatId);
		RawSetHat(hatId, colorId);
	}

	public void SetPet(string petId)
	{
		Data?.UpdatePet(petId);
		if (Data != null && Data.DefaultOutfit != null)
		{
			RawSetPet(petId, Data.DefaultOutfit.ColorId);
		}
	}

	public void SetPet(string petId, int colorId)
	{
		Data?.UpdatePet(petId);
		RawSetPet(petId, colorId);
	}

	public void SetPetPosition(Vector3 newPosition)
	{
		cosmetics.SetPetPosition(newPosition);
	}

	public PetBehaviour GetPet()
	{
		return cosmetics.GetPet();
	}

	public static void SetSkinImageAsync(SkinData skin, int colorId, SpriteRenderer target)
	{
		target.gameObject.LoadAssetAsync((IAddressableAssetProvider<SkinViewData>)skin, (Action<SkinViewData>)((SkinViewData skinView) =>
		{
			if (!(target == null) && !target.gameObject.IsDestroyedOrNull())
			{
				target.sprite = skinView.IdleFrame;
				if (skinView.MatchPlayerColor)
				{
					target.sharedMaterial = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;
					PlayerMaterial.SetColors(colorId, target);
				}
				else
				{
					target.sharedMaterial = DestroyableSingleton<HatManager>.Instance.DefaultShader;
				}
			}
		}), (Action)null);
	}

	private void ReportDeadBody(NetworkedPlayerInfo target)
	{
		if (AmongUsClient.Instance.IsGameOver || (bool)MeetingHud.Instance || (target == null && LocalPlayer.myTasks.Any(PlayerTask.TaskIsEmergency)) || Data.IsDead)
		{
			return;
		}
		MeetingRoomManager.Instance.AssignSelf(this, target);
		if (AmongUsClient.Instance.AmHost && !GameManager.Instance.CheckTaskCompletion())
		{
			if (target == null)
			{
				logger.Debug("Calling emergency meeting");
			}
			else
			{
				logger.Debug($"Reporting dead body {target.PlayerId}");
			}
			DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(this);
			RpcStartMeeting(target);
		}
	}

	private void CheckUseZipline(PlayerControl target, ZiplineBehaviour ziplineBehaviour, bool fromTop)
	{
		logger.Debug($"Checking if {PlayerId} can use zipline");
		if (AmongUsClient.Instance.IsGameOver || !AmongUsClient.Instance.AmHost)
		{
			return;
		}
		if (!target)
		{
			logger.Warning("Invalid zipline use, player is null");
			return;
		}
		NetworkedPlayerInfo data = target.Data;
		if (data == null || data.IsDead || target.inMovingPlat)
		{
			logger.Warning($"Invalid zipline use from {target.PlayerId}");
			return;
		}
		if ((bool)MeetingHud.Instance)
		{
			logger.Warning("Tried to zipline while a meeting was starting");
			return;
		}
		Vector3 vector = ziplineBehaviour.GetHandlePos(fromTop) - target.transform.position;
		if (vector.magnitude > 3f)
		{
			logger.Info($"{target} was denied the zipline: distance={vector.magnitude}");
		}
		else
		{
			RpcUseZipline(target, ziplineBehaviour, fromTop);
		}
	}

	private void CheckSporeTrigger(Mushroom mushroom)
	{
		logger.Debug($"Checking if mushroom id {mushroom.Id} can trigger spores");
		if (AmongUsClient.Instance.IsGameOver || !AmongUsClient.Instance.AmHost)
		{
			logger.Warning("Tried trigger mushroom while a game is over or as non-host");
		}
		else if ((bool)MeetingHud.Instance)
		{
			logger.Warning("Tried trigger mushroom while a meeting was starting");
		}
		else
		{
			RpcTriggerSpores(mushroom);
		}
	}

	public void StartMeeting(NetworkedPlayerInfo target)
	{
		bool flag = target == null;
		DestroyableSingleton<UnityTelemetry>.Instance.WriteMeetingStarted(flag);
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.MeetingStarted(Data, target != null);
		ShipStatus.Instance.StartMeeting(this, target);
		if (AmOwner)
		{
			if (flag)
			{
				RemainingEmergencies--;
				DataManager.Player.Stats.IncrementStat(StatID.EmergenciesCalled);
			}
			else
			{
				DataManager.Player.Stats.IncrementStat(StatID.BodiesReported);
			}
		}
	}

	public void ResetForMeeting()
	{
		if (!GetComponent<DummyBehaviour>().enabled)
		{
			MyPhysics.ExitAllVents();
			ShipStatus.Instance.SpawnPlayer(this, GameData.Instance.PlayerCount, initialSpawn: false);
		}
		RemoveProtection();
		NetTransform.enabled = true;
		MyPhysics.ResetMoveState();
		for (int i = 0; i < currentRoleAnimations.Count; i++)
		{
			if (currentRoleAnimations[i] != null && currentRoleAnimations[i].gameObject != null)
			{
				UnityEngine.Object.Destroy(currentRoleAnimations[i].gameObject);
				logger.Error("Encountered a null Role Animation while destroying.");
			}
		}
		inMovingPlat = false;
		isKilling = false;
		currentRoleAnimations.Clear();
		if (!(cosmetics.CurrentPet != null))
		{
			return;
		}
		if (cosmetics.petHiddenByViper)
		{
			cosmetics.TogglePet(active: true);
			Vector2 petPos = transform.position;
			if (ShipStatus.Instance is AirshipStatus)
			{
				List<Vector2> list = new List<Vector2>
				{
					new Vector2(8.2f, 15.2f),
					new Vector2(8.25f, 15.9f),
					new Vector2(8.2f, 14.3f),
					new Vector2(11f, 14.3f),
					new Vector2(9.8f, 14.3f),
					new Vector2(13f, 14.3f)
				};
				petPos = list[UnityEngine.Random.Range(0, list.Count)];
			}
			cosmetics.CurrentPet.SetGettingPet(petting: false, petPos);
		}
		else
		{
			cosmetics.CurrentPet.SetGettingPet(petting: false, cosmetics.CurrentPet.transform.position);
		}
	}

	public void CheckMurder(PlayerControl target)
	{
		logger.Debug(string.Format("Checking if {0} murdered {1}", PlayerId, (target == null) ? "null player" : target.PlayerId.ToString()));
		isKilling = false;
		if (AmongUsClient.Instance.IsGameOver || !AmongUsClient.Instance.AmHost)
		{
			return;
		}
		if (!target || Data.IsDead || !Data.Role.IsImpostor || Data.Disconnected)
		{
			int num = (target ? target.PlayerId : (-1));
			logger.Warning($"Bad kill from {PlayerId} to {num}");
			RpcMurderPlayer(target, didSucceed: false);
			return;
		}
		NetworkedPlayerInfo data = target.Data;
		if (data == null || data.IsDead || target.inVent || target.MyPhysics.Animations.IsPlayingEnterVentAnimation() || target.MyPhysics.Animations.IsPlayingAnyLadderAnimation() || target.inMovingPlat)
		{
			logger.Warning("Invalid target data for kill");
			RpcMurderPlayer(target, didSucceed: false);
		}
		else if ((bool)MeetingHud.Instance)
		{
			logger.Warning("Tried to kill while a meeting was starting");
			RpcMurderPlayer(target, didSucceed: false);
		}
		else
		{
			isKilling = true;
			RpcMurderPlayer(target, didSucceed: true);
		}
	}

	private void CheckShapeshift(PlayerControl target, bool shouldAnimate)
	{
		logger.Debug(string.Format("Checking if {0} can shapeshift into {1}", PlayerId, (target == null) ? "null player" : target.PlayerId.ToString()));
		if (!AmongUsClient.Instance.IsGameOver && AmongUsClient.Instance.AmHost)
		{
			if (!target || target.Data == null || Data.IsDead || Data.Role.Role != RoleTypes.Shapeshifter || Data.Disconnected)
			{
				int num = (target ? target.PlayerId : (-1));
				logger.Warning($"Bad shapeshift from {PlayerId} to {num}");
				RpcRejectShapeshift();
			}
			else if (target.IsMushroomMixupActive() & shouldAnimate)
			{
				logger.Warning("Tried to shapeshift while mushroom mixup was active");
				RpcRejectShapeshift();
			}
			else if ((bool)MeetingHud.Instance & shouldAnimate)
			{
				logger.Warning("Tried to shapeshift while a meeting was starting");
				RpcRejectShapeshift();
			}
			else
			{
				RpcShapeshift(target, shouldAnimate);
			}
		}
	}

	public bool IsMushroomMixupActive()
	{
		FungleShipStatus fungleShipStatus = ShipStatus.Instance as FungleShipStatus;
		if (fungleShipStatus != null && fungleShipStatus.IsMushroomMixupActive())
		{
			return true;
		}
		return CurrentOutfitType == PlayerOutfitType.MushroomMixup;
	}

	public bool AreCommsAffected()
	{
		if (myTasks.Find((PlayerTask x) => x.TaskType == TaskTypes.FixComms) != null)
		{
			return true;
		}
		return false;
	}

	public void MurderPlayer(PlayerControl target, MurderResultFlags resultFlags)
	{
		isKilling = false;
		logger.Debug($"{PlayerId} trying to murder {target.PlayerId}");
		NetworkedPlayerInfo data = target.Data;
		if (resultFlags.HasFlag(MurderResultFlags.FailedError))
		{
			return;
		}
		if (resultFlags.HasFlag(MurderResultFlags.FailedProtected) || (resultFlags.HasFlag(MurderResultFlags.DecisionByHost) && target.protectedByGuardianId > -1))
		{
			target.protectedByGuardianThisRound = true;
			bool flag = LocalPlayer.Data.Role.Role == RoleTypes.GuardianAngel;
			if (flag && LocalPlayer.Data.PlayerId == target.protectedByGuardianId)
			{
				DataManager.Player.Stats.IncrementStat(StatID.Role_GuardianAngel_CrewmatesProtected);
				DestroyableSingleton<AchievementManager>.Instance.OnProtectACrewmate();
			}
			if (AmOwner | flag)
			{
				target.ShowFailedMurder();
				SetKillTimer(GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown) / 2f);
			}
			else
			{
				target.RemoveProtection();
			}
			logger.Debug($"{PlayerId} failed to murder {target.PlayerId} due to guardian angel protection");
		}
		else
		{
			if (!resultFlags.HasFlag(MurderResultFlags.Succeeded) && !resultFlags.HasFlag(MurderResultFlags.DecisionByHost))
			{
				return;
			}
			DestroyableSingleton<DebugAnalytics>.Instance.Analytics.Kill(target.Data, Data);
			if (AmOwner)
			{
				if (GameManager.Instance.IsHideAndSeek())
				{
					DataManager.Player.Stats.IncrementStat(StatID.HideAndSeek_ImpostorKills);
				}
				else
				{
					DataManager.Player.Stats.IncrementStat(StatID.ImpostorKills);
				}
				if (CurrentOutfitType == PlayerOutfitType.Shapeshifted)
				{
					DataManager.Player.Stats.IncrementStat(StatID.Role_Shapeshifter_ShiftedKills);
				}
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(KillSfx, loop: false, 0.8f);
				}
				SetKillTimer(GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown));
			}
			DestroyableSingleton<UnityTelemetry>.Instance.WriteMurder();
			target.gameObject.layer = LayerMask.NameToLayer("Ghost");
			if (target.AmOwner)
			{
				DataManager.Player.Stats.IncrementStat(StatID.TimesMurdered);
				if ((bool)Minigame.Instance)
				{
					try
					{
						Minigame.Instance.Close();
						Minigame.Instance.Close();
					}
					catch
					{
					}
				}
				DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(Data, data);
				target.cosmetics.SetNameMask(alive: false);
				target.RpcSetScanner(value: false);
			}
			DestroyableSingleton<AchievementManager>.Instance.OnMurder(AmOwner, target.AmOwner, CurrentOutfitType == PlayerOutfitType.Shapeshifted, shapeshiftTargetPlayerId, target.PlayerId);
			MyPhysics.StartCoroutine(KillAnimations.Random().CoPerformKill(this, target));
			logger.Debug($"{PlayerId} succeeded in murdering {target.PlayerId}");
		}
	}

	public void CheckProtect(PlayerControl target)
	{
		logger.Debug($"{PlayerId} trying to protect {target.PlayerId}");
		if (AmongUsClient.Instance.IsGameOver || !AmongUsClient.Instance.AmHost)
		{
			return;
		}
		if (!target || Data.Role.Role != RoleTypes.GuardianAngel || Data.Role.IsImpostor || Data.Disconnected)
		{
			int num = (target ? target.PlayerId : (-1));
			logger.Warning($"Bad protect from {PlayerId} to {num}");
			return;
		}
		NetworkedPlayerInfo data = target.Data;
		if (data == null || data.IsDead)
		{
			logger.Warning("Invalid target for protection");
		}
		else
		{
			RpcProtectPlayer(target, Data.DefaultOutfit.ColorId);
		}
	}

	public void ProtectPlayer(PlayerControl target, int colorId)
	{
		logger.Debug($"{PlayerId} protected {target.PlayerId}");
		if (AmOwner)
		{
			Data.Role.SetCooldown();
		}
		target.TurnOnProtection(LocalPlayer.Data.IsDead || (LocalPlayer.Data.Role.TeamType == RoleTeamTypes.Impostor && GameOptionsManager.Instance.CurrentGameOptions.GetBool(BoolOptionNames.ImpostorsCanSeeProtect)), colorId, Data.PlayerId);
	}

	public void SetPlayerMaterialColors(Renderer rend)
	{
		PlayerMaterial.SetColors(GameData.Instance.GetPlayerById(PlayerId)?.DefaultOutfit.ColorId ?? 0, rend);
	}

	public static void HideCursorTemporarily()
	{
		if (LocalPlayer.AmOwner)
		{
			LocalPlayer.MyPhysics.inputHandler.enabled = true;
		}
	}

	public void SetAppearanceFromSaveData()
	{
		MyPhysics.ResetMoveState();
		SetName(DataManager.Player.Customization.Name);
		SetColor(DataManager.Player.Customization.Color);
		SetHat(DataManager.Player.Customization.Hat, DataManager.Player.Customization.Color);
		SetSkin(DataManager.Player.Customization.Skin, DataManager.Player.Customization.Color);
		if (DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat).BlocksVisors)
		{
			DataManager.Player.Customization.Visor = "visor_EmptyVisor";
		}
		SetVisor(DataManager.Player.Customization.Visor, DataManager.Player.Customization.Color);
		SetPet(DataManager.Player.Customization.Pet, DataManager.Player.Customization.Color);
		SetNamePlate(DataManager.Player.Customization.NamePlate);
		SpriteAnimNodeSync[] componentsInChildren = GetComponentsInChildren<SpriteAnimNodeSync>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].LateUpdate();
		}
	}

	public void ToggleHighlight(bool active, RoleTeamTypes targeterTeam)
	{
		if (active)
		{
			cosmetics.SetOutline(active: true, (targeterTeam == RoleTeamTypes.Crewmate) ? Palette.CrewmateBlue : Palette.ImpostorRed);
		}
		else
		{
			cosmetics.SetOutline(active: false);
		}
	}

	public void SetOutfit(NetworkedPlayerInfo.PlayerOutfit newOutfit, PlayerOutfitType type)
	{
		SetName(newOutfit.PlayerName);
		SetColor(newOutfit.ColorId);
		SetHat(newOutfit.HatId, newOutfit.ColorId);
		SetSkin(newOutfit.SkinId, newOutfit.ColorId);
		SetVisor(newOutfit.VisorId, newOutfit.ColorId);
		SetPet(newOutfit.PetId, newOutfit.ColorId);
		Visible = Visible;
		PlayerOutfitType currentOutfitType = CurrentOutfitType;
		CurrentOutfitType = type;
		if (currentOutfitType != PlayerOutfitType.Default)
		{
			Data.Outfits.Remove(currentOutfitType);
		}
		if (type != PlayerOutfitType.Default)
		{
			Data.SetOutfit(CurrentOutfitType, newOutfit);
		}
	}

	public void RawSetOutfit(NetworkedPlayerInfo.PlayerOutfit newOutfit, PlayerOutfitType type)
	{
		RawSetName(newOutfit.PlayerName);
		RawSetColor(newOutfit.ColorId);
		RawSetHat(newOutfit.HatId, newOutfit.ColorId);
		RawSetSkin(newOutfit.SkinId, newOutfit.ColorId);
		RawSetVisor(newOutfit.VisorId, newOutfit.ColorId);
		RawSetPet(newOutfit.PetId, newOutfit.ColorId);
		Visible = Visible;
		PlayerOutfitType currentOutfitType = CurrentOutfitType;
		CurrentOutfitType = type;
		if (currentOutfitType != PlayerOutfitType.Default)
		{
			Data.Outfits.Remove(currentOutfitType);
		}
		if (type != PlayerOutfitType.Default)
		{
			Data.SetOutfit(CurrentOutfitType, newOutfit);
		}
	}

	public void RejectShapeshift()
	{
		waitingForShapeshiftResponse = false;
	}

	public void Shapeshift(PlayerControl targetPlayer, bool animate)
	{
		waitingForShapeshiftResponse = false;
		if (CurrentOutfitType == PlayerOutfitType.MushroomMixup)
		{
			logger.Info("Ignoring shapeshift message for " + ((targetPlayer == null) ? "null player" : targetPlayer.PlayerId.ToString()) + " because of mushroom mixup");
			if (AmOwner)
			{
				DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Data.Role.Ability);
				Data.Role.SetCooldown();
			}
			return;
		}
		NetworkedPlayerInfo targetPlayerInfo = targetPlayer.Data;
		NetworkedPlayerInfo.PlayerOutfit newOutfit;
		if (targetPlayerInfo.PlayerId == Data.PlayerId)
		{
			newOutfit = Data.Outfits[PlayerOutfitType.Default];
		}
		else
		{
			newOutfit = targetPlayer.Data.Outfits[PlayerOutfitType.Default];
		}
		Action changeOutfit = () =>
		{
			if (targetPlayerInfo.PlayerId == Data.PlayerId)
			{
				RawSetOutfit(newOutfit, PlayerOutfitType.Default);
				logger.Info($"Player {PlayerId} Shapeshift is reverting");
				shapeshiftTargetPlayerId = -1;
				if (AmOwner)
				{
					DestroyableSingleton<HudManager>.Instance.AbilityButton.SetFromSettings(Data.Role.Ability);
				}
			}
			else
			{
				RawSetOutfit(newOutfit, PlayerOutfitType.Shapeshifted);
				logger.Info($"Player {PlayerId} is shapeshifting into {targetPlayer.PlayerId}");
				shapeshiftTargetPlayerId = targetPlayer.PlayerId;
				if (AmOwner)
				{
					DestroyableSingleton<HudManager>.Instance.AbilityButton.OverrideText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ShapeshiftAbilityUndo));
				}
			}
		};
		if (animate)
		{
			shapeshifting = true;
			MyPhysics.SetNormalizedVelocity(Vector2.zero);
			if (AmOwner && !Minigame.Instance)
			{
				HideCursorTemporarily();
			}
			RoleEffectAnimation roleEffectAnimation = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.shapeshiftAnim, gameObject.transform);
			roleEffectAnimation.SetMaskLayerBasedOnWhoShouldSee(AmOwner);
			roleEffectAnimation.SetMaterialColor(Data.Outfits[PlayerOutfitType.Default].ColorId);
			if (cosmetics.FlipX)
			{
				roleEffectAnimation.transform.position -= new Vector3(0.14f, 0f, 0f);
			}
			roleEffectAnimation.MidAnimCB = () =>
			{
				changeOutfit();
				cosmetics.SetScale(MyPhysics.Animations.DefaultPlayerScale, defaultCosmeticsScale);
				(Data.Role as ShapeshifterRole).SetEvidence();
			};
			float shapeshiftScale = MyPhysics.Animations.ShapeshiftScale;
			if (AprilFoolsMode.ShouldLongAround())
			{
				cosmetics.ShowLongModeParts(show: false);
				cosmetics.SetHatVisorVisible(isVisible: false);
			}
			StartCoroutine(ScalePlayer(shapeshiftScale, 0.25f));
			roleEffectAnimation.Play(this, () =>
			{
				shapeshifting = false;
				if (AprilFoolsMode.ShouldLongAround())
				{
					cosmetics.ShowLongModeParts(show: true);
					cosmetics.SetHatVisorVisible(isVisible: true);
				}
			}, LocalPlayer.cosmetics.FlipX, RoleEffectAnimation.SoundType.Local);
		}
		else
		{
			changeOutfit();
		}
	}

	public int ShapeshiftedPlayerID()
	{
		return shapeshiftTargetPlayerId;
	}

	private void SetRoleInvisibility(bool isActive, bool shouldAnimate = true, bool playFullAnimation = false)
	{
		PhantomRole phantomRole = Data.Role as PhantomRole;
		if (!(phantomRole != null))
		{
			return;
		}
		if (shouldAnimate)
		{
			if (isActive)
			{
				RoleEffectAnimation roleEffectAnimation = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.vanish_ChargeAnim, gameObject.transform);
				roleEffectAnimation.SetMaskLayerBasedOnWhoShouldSee(AmOwner);
				roleEffectAnimation.SetMaterialColor(cosmetics.ColorId);
				phantomRole.SetFading(isFading: true);
				phantomRole.SetServerApproval(approved: false);
				roleEffectAnimation.Play(this, () =>
				{
					phantomRole.SetInvisible(invisibility: true);
					if (playFullAnimation || phantomRole.ServerApproved)
					{
						ContinueVanish();
					}
					else if (!phantomRole.ServerApproved)
					{
						LoopChargeAnim(phantomRole);
					}
				}, LocalPlayer.cosmetics.FlipX, RoleEffectAnimation.SoundType.Local, 0.5f, parentTransform: true, -0.05f);
			}
			else if (LocalPlayer == this)
			{
				RoleEffectAnimation roleEffectAnimation2 = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.vanish_ChargeAnim, gameObject.transform);
				roleEffectAnimation2.SetMaskLayerBasedOnWhoShouldSee(AmOwner);
				roleEffectAnimation2.SetMaterialColor(cosmetics.ColorId);
				phantomRole.SetFading(isFading: true);
				phantomRole.PlayPhantomAppearSound();
				roleEffectAnimation2.Play(this, () =>
				{
					phantomRole.SetInvisible(invisibility: false);
					RoleEffectAnimation roleEffectAnimation3 = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.appear_PoofAnim, gameObject.transform);
					roleEffectAnimation3.SetMaterialColor(cosmetics.ColorId);
					roleEffectAnimation3.SetMaskLayerBasedOnWhoShouldSee(AmOwner);
					roleEffectAnimation3.Play(this, () =>
					{
						SetInvisibility(isActive: false);
						phantomRole.SetFading(isFading: false);
					}, LocalPlayer.cosmetics.FlipX, RoleEffectAnimation.SoundType.Local);
				}, LocalPlayer.cosmetics.FlipX, RoleEffectAnimation.SoundType.Local, 1f, parentTransform: true, -0.05f);
			}
			else
			{
				AppearOtherClients();
			}
		}
		else
		{
			phantomRole.SetInvisible(isActive);
			SetInvisibility(isActive);
		}
	}

	private void LoopChargeAnim(PhantomRole phantomRole)
	{
		if (!(phantomRole != null))
		{
			return;
		}
		RoleEffectAnimation roleEffectAnimation = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.vanish_ChargeAnim, gameObject.transform);
		roleEffectAnimation.SetMaskLayerBasedOnWhoShouldSee(AmOwner);
		roleEffectAnimation.SetMaterialColor(cosmetics.ColorId);
		phantomRole.SetFading(isFading: true);
		roleEffectAnimation.Play(this, () =>
		{
			if (phantomRole.ServerApproved)
			{
				ContinueVanish();
			}
			else
			{
				LoopChargeAnim(phantomRole);
			}
		}, LocalPlayer.cosmetics.FlipX, RoleEffectAnimation.SoundType.Local, 0.5f, parentTransform: true, -0.05f);
	}

	public void ForcePhantomVisible()
	{
		SetInvisibility(isActive: false);
	}

	private void AppearOtherClients()
	{
		PhantomRole phantomRole = Data.Role as PhantomRole;
		if (!(phantomRole != null))
		{
			return;
		}
		phantomRole.AppearClients(() =>
		{
			phantomRole.SetInvisible(invisibility: false);
			RoleEffectAnimation roleEffectAnimation = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.appear_PoofAnim, gameObject.transform);
			roleEffectAnimation.SetMaterialColor(cosmetics.ColorId);
			roleEffectAnimation.SetMaskLayerBasedOnWhoShouldSee(AmOwner);
			roleEffectAnimation.Play(this, () =>
			{
				SetInvisibility(isActive: false);
				phantomRole.SetFading(isFading: false);
			}, LocalPlayer.cosmetics.FlipX, RoleEffectAnimation.SoundType.Local);
		});
	}

	private void ContinueVanish()
	{
		PhantomRole phantomRole = Data.Role as PhantomRole;
		if (phantomRole != null)
		{
			phantomRole.PlayPhantomVanishSound();
		}
		RoleEffectAnimation roleEffectAnimation = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.vanish_ChargeAnim, gameObject.transform);
		roleEffectAnimation.SetMaskLayerBasedOnWhoShouldSee(AmOwner);
		roleEffectAnimation.SetMaterialColor(cosmetics.ColorId);
		roleEffectAnimation.Play(this, () =>
		{
			RoleEffectAnimation roleEffectAnimation2 = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.vanish_PoofAnim, gameObject.transform);
			roleEffectAnimation2.SetMaterialColor(cosmetics.ColorId);
			SetInvisibility(isActive: true);
			roleEffectAnimation2.Play(this, () =>
			{
				if (phantomRole != null)
				{
					phantomRole.SetFading(isFading: false);
					phantomRole.SetServerApproval(approved: false);
					phantomRole.StartDuration();
				}
			}, LocalPlayer.cosmetics.FlipX, RoleEffectAnimation.SoundType.Local, 0f, parentTransform: false);
		}, LocalPlayer.cosmetics.FlipX, RoleEffectAnimation.SoundType.Local, 0.5f, parentTransform: true, -0.05f);
	}

	private void SetInvisibility(bool isActive)
	{
		if (Data.Role as PhantomRole == null)
		{
			return;
		}
		if (AmOwner)
		{
			invisibilityAlpha = (isActive ? 0.5f : 1f);
		}
		else
		{
			if (!isActive)
			{
				shouldAppearInvisible = false;
				Visible = true;
			}
			if (LocalPlayer.Data.IsDead || LocalPlayer.Data.Role.IsImpostor)
			{
				invisibilityAlpha = (isActive ? 0.5f : 1f);
			}
			else
			{
				invisibilityAlpha = ((!isActive) ? 1 : 0);
			}
		}
		cosmetics.SetPhantomRoleAlpha(invisibilityAlpha);
		if ((!isActive || (!LocalPlayer.Data.IsDead && !LocalPlayer.Data.Role.IsImpostor)) && !AmOwner)
		{
			shouldAppearInvisible = isActive;
			if (inVent)
			{
				Visible = false;
			}
			else
			{
				Visible = !isActive;
			}
		}
	}

	public void StartPlayerTracking(PlayerControl playerToTrack, int colorID)
	{
		isTrackingPlayer = true;
		trackedPlayer = playerToTrack;
		trackedPlayerColorID = colorID;
	}

	public void CancelPlayerTracking()
	{
		isTrackingPlayer = false;
		trackedPlayer = null;
		MapBehaviour.Instance?.DisableTrackerOverlays();
	}

	public void DisableCurrentTrackers()
	{
		foreach (PlayerControl allPlayerControl in AllPlayerControls)
		{
			if (allPlayerControl.Data.Role is TrackerRole trackerRole)
			{
				trackerRole.CheckCancelTracking(this);
			}
		}
	}

	public void MixUpOutfit(NetworkedPlayerInfo.PlayerOutfit playerOutfit)
	{
		if (Data.IsDead)
		{
			return;
		}
		if (CurrentOutfitType == PlayerOutfitType.Shapeshifted)
		{
			if (AmOwner)
			{
				CmdCheckRevertShapeshift(shouldAnimate: false);
			}
			else
			{
				Shapeshift(this, animate: false);
			}
		}
		RawSetOutfit(playerOutfit, PlayerOutfitType.MushroomMixup);
		RawSetName(Data.DefaultOutfit.PlayerName);
		if (!LocalPlayer.Data.Role.IsImpostor)
		{
			cosmetics.ToggleNameVisible(visible: false);
		}
	}

	public IEnumerator ScalePlayer(float targetScale, float duration)
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			cosmetics.SetScale(Vector3.Lerp(MyPhysics.Animations.DefaultPlayerScale, MyPhysics.Animations.DefaultPlayerScale * targetScale, elapsedTime / duration), Vector3.Lerp(defaultCosmeticsScale, defaultCosmeticsScale * targetScale, elapsedTime / duration));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		cosmetics.SetScale(new Vector3(MyPhysics.Animations.DefaultPlayerScale.x * targetScale, MyPhysics.Animations.DefaultPlayerScale.y * targetScale, 1f), new Vector3(defaultCosmeticsScale.x * targetScale, defaultCosmeticsScale.y * targetScale, 1f));
	}

	public void CmdCheckRevertShapeshift(bool shouldAnimate)
	{
		CmdCheckShapeshift(this, shouldAnimate);
	}

	public void FixMixedUpOutfit()
	{
		if (CurrentOutfitType == PlayerOutfitType.MushroomMixup)
		{
			RawSetOutfit(Data.DefaultOutfit, PlayerOutfitType.Default);
			cosmetics.ToggleNameVisible(GameManager.Instance.LogicOptions.GetShowCrewmateNames());
		}
		PlayerTask playerTask = myTasks.Find((PlayerTask x) => x.TaskType == TaskTypes.MushroomMixupSabotage);
		if (playerTask != null)
		{
			RemoveTask(playerTask);
		}
	}

	private void RawSetName(string name)
	{
		gameObject.name = name;
		cosmetics.SetName(name);
		cosmetics.SetNameMask(alive: true);
	}

	private void RawSetColor(int bodyColor)
	{
		cosmetics.SetColor(bodyColor);
	}

	private void RawSetVisor(string visorId, int color)
	{
		cosmetics.SetVisor(visorId, color);
	}

	private void RawSetSkin(string skinId, int color)
	{
		MyPhysics.SetSkin(skinId, color);
	}

	private void RawSetHat(string hatId, int colorId)
	{
		cosmetics.SetHat(hatId, colorId);
		cosmetics.SetNamePosition(new Vector3(0f, string.IsNullOrEmpty(hatId) ? 0.8f : 1f, -0.5f));
	}

	private void RawSetPet(string petId, int colorId)
	{
		cosmetics.SetPetIdle(petId, colorId, () =>
		{
			cosmetics.SetPetSource(this);
			if (inMovingPlat)
			{
				cosmetics.SetPetVisible(v: false);
			}
		});
	}

	private void ShowFailedMurder()
	{
		RoleEffectAnimation roleEffectAnimation = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.protectAnim, gameObject.transform);
		roleEffectAnimation.SetMaskLayerBasedOnWhoShouldSee(shouldBeVisible: true);
		roleEffectAnimation.Play(this, null, cosmetics.FlipX, RoleEffectAnimation.SoundType.Global);
		RemoveProtection();
	}

	private void RemoveProtection()
	{
		for (int num = currentRoleAnimations.Count - 1; num >= 0; num--)
		{
			if (currentRoleAnimations[num] != null && currentRoleAnimations[num].effectType == RoleEffectAnimation.EffectType.ProtectLoop)
			{
				UnityEngine.Object.Destroy(currentRoleAnimations[num].gameObject);
				currentRoleAnimations.RemoveAt(num);
			}
		}
		protectedByGuardianId = -1;
	}

	private void TurnOnProtection(bool visible, int colorId, int guardianPlayerId)
	{
		protectedByGuardianId = guardianPlayerId;
		float duration = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.ProtectionDurationSeconds);
		if (visible)
		{
			RoleEffectAnimation roleEffectAnimation = UnityEngine.Object.Instantiate(DestroyableSingleton<RoleManager>.Instance.protectLoopAnim, gameObject.transform);
			roleEffectAnimation.SetMaterialColor(colorId);
			roleEffectAnimation.SetMaskLayerBasedOnWhoShouldSee(AmOwner);
			roleEffectAnimation.Play(this, RemoveProtection, LocalPlayer.cosmetics.FlipX, (Data.Role.Role == RoleTypes.GuardianAngel) ? RoleEffectAnimation.SoundType.Local : RoleEffectAnimation.SoundType.None, duration);
		}
		else
		{
			StartCoroutine(Effects.Sequence(Effects.Wait(duration), Effects.Action(RemoveProtection)));
		}
	}

	private void SetColorBlindTag()
	{
		if (!GameManager.Instance.GameHasStarted || GameManager.Instance.LogicOptions.GetShowCrewmateNames())
		{
			cosmetics.SetEnabledColorblind(DataManager.Settings.Accessibility.ColorBlindMode);
		}
	}

	public bool CanPet()
	{
		if (!DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed && !walkingToVent && !inVent && !petting && !Data.IsDead)
		{
			return cosmetics.HasPetEquipped();
		}
		return false;
	}

	public void AdjustLighting()
	{
		if (LocalPlayer != this)
		{
			return;
		}
		float value = 0f;
		if (IsFlashlightEnabled())
		{
			if (Data.Role.IsImpostor)
			{
				GameOptionsManager.Instance.CurrentGameOptions.TryGetFloat(FloatOptionNames.ImpostorFlashlightSize, out value);
			}
			else
			{
				GameOptionsManager.Instance.CurrentGameOptions.TryGetFloat(FloatOptionNames.CrewmateFlashlightSize, out value);
			}
		}
		SetFlashlightInputMethod();
		lightSource.SetupLightingForGameplay(IsFlashlightEnabled(), value, TargetFlashlight.transform);
	}

	private bool IsFlashlightEnabled()
	{
		if (LobbyBehaviour.Instance != null)
		{
			return false;
		}
		if (LocalPlayer.Data.IsDead)
		{
			return false;
		}
		if (GameOptionsManager.Instance.CurrentGameOptions.GameMode != GameModes.HideNSeek)
		{
			return false;
		}
		bool value = false;
		if (!GameOptionsManager.Instance.CurrentGameOptions.TryGetBool(BoolOptionNames.UseFlashlight, out value))
		{
			return false;
		}
		return value;
	}

	private void SetFlashlightInputMethod()
	{
		if (!(LocalPlayer != this))
		{
			if (!IsFlashlightEnabled())
			{
				TargetFlashlight.gameObject.SetActive(value: false);
				StartCoroutine(EnableRightJoystick(enabled: false));
			}
			else if (DataManager.Settings.Input.InputMode == ControlTypes.ScreenJoystick)
			{
				TargetFlashlight?.gameObject.SetActive(value: true);
				StartCoroutine(EnableRightJoystick(enabled: false));
			}
			else
			{
				TargetFlashlight?.gameObject.SetActive(value: false);
				StartCoroutine(EnableRightJoystick(enabled: true));
			}
		}
	}

	private IEnumerator EnableRightJoystick(bool enabled)
	{
		yield return new WaitForEndOfFrame();
		while (!DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return null;
		}
		DestroyableSingleton<HudManager>.Instance.ToggleRightJoystick(enabled);
	}

	private IEnumerator CoSetCosmetic(RpcCalls rpc, string cosmeticId, byte sequenceId)
	{
		int attempts = 0;
		while (!Data && attempts < 60)
		{
			attempts++;
			yield return Effects.Wait(0.1f);
		}
		if (!Data)
		{
			Debug.LogWarning("CoSetCosmetic timed out waiting for NetworkedPlayerInfo");
			yield break;
		}
		if (!ShouldProcessRpc(rpc, sequenceId))
		{
			Debug.Log(string.Format("{0} RPC {1} was received out of order with sequence ID {2}", "CoSetCosmetic", rpc, sequenceId));
			yield break;
		}
		int colorId = Data.DefaultOutfit.ColorId;
		switch (rpc)
		{
		case RpcCalls.SetHatStr:
			Data.DefaultOutfit.HatSequenceId = sequenceId;
			SetHat(cosmeticId, colorId);
			break;
		case RpcCalls.SetSkinStr:
			Data.DefaultOutfit.SkinSequenceId = sequenceId;
			SetSkin(cosmeticId, colorId);
			break;
		case RpcCalls.SetPetStr:
			Data.DefaultOutfit.PetSequenceId = sequenceId;
			SetPet(cosmeticId);
			break;
		case RpcCalls.SetVisorStr:
			Data.DefaultOutfit.VisorSequenceId = sequenceId;
			SetVisor(cosmeticId, colorId);
			break;
		case RpcCalls.SetNamePlateStr:
			Data.DefaultOutfit.NamePlateSequenceId = sequenceId;
			SetNamePlate(cosmeticId);
			break;
		default:
			Debug.LogWarning(string.Format("{0} got an rpc with no handler {1}", "CoSetCosmetic", rpc));
			break;
		}
	}

	private bool ShouldProcessRpc(RpcCalls rpc, byte sequenceId)
	{
		byte prevSid;
		switch (rpc)
		{
		case RpcCalls.SetHatStr:
			prevSid = Data.DefaultOutfit.HatSequenceId;
			break;
		case RpcCalls.SetSkinStr:
			prevSid = Data.DefaultOutfit.SkinSequenceId;
			break;
		case RpcCalls.SetPetStr:
			prevSid = Data.DefaultOutfit.PetSequenceId;
			break;
		case RpcCalls.SetVisorStr:
			prevSid = Data.DefaultOutfit.VisorSequenceId;
			break;
		case RpcCalls.SetNamePlateStr:
			prevSid = Data.DefaultOutfit.NamePlateSequenceId;
			break;
		default:
			Debug.LogError(string.Format("{0} was called for an RPC it doesn't know how to handle: {1}", "ShouldProcessRpc", rpc));
			return false;
		}
		return NetHelpers.SidGreaterThan(sequenceId, prevSid);
	}

	private byte GetNextRpcSequenceId(RpcCalls rpc)
	{
		byte result = 0;
		switch (rpc)
		{
		case RpcCalls.SetHatStr:
			Data.DefaultOutfit.HatSequenceId++;
			result = Data.DefaultOutfit.HatSequenceId;
			break;
		case RpcCalls.SetSkinStr:
			Data.DefaultOutfit.SkinSequenceId++;
			result = Data.DefaultOutfit.SkinSequenceId;
			break;
		case RpcCalls.SetPetStr:
			Data.DefaultOutfit.PetSequenceId++;
			result = Data.DefaultOutfit.PetSequenceId;
			break;
		case RpcCalls.SetVisorStr:
			Data.DefaultOutfit.VisorSequenceId++;
			result = Data.DefaultOutfit.VisorSequenceId;
			break;
		case RpcCalls.SetNamePlateStr:
			Data.DefaultOutfit.NamePlateSequenceId++;
			result = Data.DefaultOutfit.NamePlateSequenceId;
			break;
		default:
			Debug.LogError(string.Format("{0} was called for an RPC it doesn't know how to handle: {1}", "GetNextRpcSequenceId", rpc));
			break;
		}
		return result;
	}

	public void RpcSetScanner(bool value)
	{
		byte b = ++scannerCount;
		if (AmongUsClient.Instance.AmClient)
		{
			SetScanner(value, b);
		}
		if (GameManager.Instance.LogicOptions.GetVisualTasks())
		{
			RpcSetScannerMessage rpcMessage = new RpcSetScannerMessage(NetId, value, b);
			AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
		}
	}

	public void RpcUsePlatform()
	{
		if (AmongUsClient.Instance.AmHost)
		{
			AirshipStatus airshipStatus = ShipStatus.Instance as AirshipStatus;
			if ((bool)airshipStatus)
			{
				airshipStatus.GapPlatform.Use(this);
			}
		}
		else
		{
			RpcUsePlatformMessage rpcMessage = new RpcUsePlatformMessage(NetId);
			AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
		}
	}

	public void CmdCheckUseZipline(PlayerControl target, ZiplineBehaviour ziplineBehaviour, bool fromTop)
	{
		if (AmongUsClient.Instance.AmLocalHost || AmongUsClient.Instance.AmModdedHost)
		{
			CheckUseZipline(target, ziplineBehaviour, fromTop);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 51, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write(fromTop);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void CmdCheckSporeTrigger(Mushroom mushroom)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			CheckSporeTrigger(mushroom);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 54, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.WritePacked(mushroom.Id);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcPlayAnimation(byte animType)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			PlayAnimation(animType);
		}
		if (GameManager.Instance.LogicOptions.GetVisualTasks())
		{
			RpcPlayAnimationMessage rpcMessage = new RpcPlayAnimationMessage(NetId, animType);
			AmongUsClient.Instance.LateBroadcastUnreliableMessage(rpcMessage);
		}
	}

	public void RpcSetStartCounter(int secondsLeft)
	{
		int counterId = LastStartCounter++;
		RpcSetStartCounterMessage rpcMessage = new RpcSetStartCounterMessage(NetId, counterId, (sbyte)secondsLeft);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcCompleteTask(uint idx)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			CompleteTask(idx);
		}
		RpcCompleteTaskMessage rpcMessage = new RpcCompleteTaskMessage(NetId, idx);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcSetRole(RoleTypes roleType, bool canOverrideRole = false)
	{
		Data.Role.OnRoleSet();
		if (AmongUsClient.Instance.AmClient)
		{
			StartCoroutine(CoSetRole(roleType, canOverrideRole));
		}
		RpcSetRoleMessage rpcMessage = new RpcSetRoleMessage(NetId, roleType, canOverrideRole);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void CmdCheckName(string name)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			CheckName(name);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 5, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write(name);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcSetLevel(uint level)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			SetLevel(level);
		}
		RpcSetLevelMessage rpcMessage = new RpcSetLevelMessage(NetId, level);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcSetVisor(string visorId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			int valueOrDefault = (Data?.DefaultOutfit?.ColorId).GetValueOrDefault();
			SetVisor(visorId, valueOrDefault);
		}
		RpcSetVisorStrMessage rpcMessage = new RpcSetVisorStrMessage(NetId, visorId, GetNextRpcSequenceId(RpcCalls.SetVisorStr));
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcSetNamePlate(string namePlateId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			SetNamePlate(namePlateId);
		}
		RpcSetNamePlateStrMessage rpcMessage = new RpcSetNamePlateStrMessage(NetId, namePlateId, GetNextRpcSequenceId(RpcCalls.SetNamePlateStr));
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcSetSkin(string skinId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			int valueOrDefault = (Data?.DefaultOutfit?.ColorId).GetValueOrDefault();
			SetSkin(skinId, valueOrDefault);
		}
		RpcSetSkinStrMessage rpcMessage = new RpcSetSkinStrMessage(NetId, skinId, GetNextRpcSequenceId(RpcCalls.SetSkinStr));
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcSetHat(string hatId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			int valueOrDefault = (Data?.DefaultOutfit?.ColorId).GetValueOrDefault();
			SetHat(hatId, valueOrDefault);
		}
		RpcSetHatStrMessage rpcMessage = new RpcSetHatStrMessage(NetId, hatId, GetNextRpcSequenceId(RpcCalls.SetHatStr));
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcSetPet(string petId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			SetPet(petId);
		}
		RpcSetPetStrMessage rpcMessage = new RpcSetPetStrMessage(NetId, petId, GetNextRpcSequenceId(RpcCalls.SetPetStr));
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcSetName(string name)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			SetName(name);
		}
		RpcSetNameMessage rpcMessage = new RpcSetNameMessage(NetId, Data.NetId, name);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void CmdCheckColor(byte bodyColor)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			CheckColor(bodyColor);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 7, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write(bodyColor);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcSetColor(byte bodyColor)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			SetColor(bodyColor);
		}
		RpcSetColorMessage rpcMessage = new RpcSetColorMessage(NetId, Data.NetId, bodyColor);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public bool RpcSendChat(string chatText)
	{
		chatText = Regex.Replace(chatText, "<.*?>", string.Empty);
		if (string.IsNullOrWhiteSpace(chatText))
		{
			return false;
		}
		if (AmongUsClient.Instance.AmClient && (bool)DestroyableSingleton<HudManager>.Instance)
		{
			DestroyableSingleton<HudManager>.Instance.Chat.AddChat(this, chatText);
		}
		if (chatText.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			DestroyableSingleton<UnityTelemetry>.Instance.SendWho();
		}
		RpcSendChatMessage rpcMessage = new RpcSendChatMessage(NetId, chatText);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
		return true;
	}

	public bool RpcSendQuickChat(QuickChatPhraseBuilderResult data)
	{
		string text = data.ToChatText();
		if (string.IsNullOrWhiteSpace(text) || data == null || !data.IsValid())
		{
			return false;
		}
		if (AmongUsClient.Instance.AmClient && (bool)DestroyableSingleton<HudManager>.Instance)
		{
			DestroyableSingleton<HudManager>.Instance.Chat.AddChat(this, text, censor: false);
		}
		if (text.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			DestroyableSingleton<UnityTelemetry>.Instance.SendWho();
		}
		RpcSendQuickChatMessage rpcMessage = new RpcSendQuickChatMessage(NetId, data);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
		return true;
	}

	public void RpcSendChatNote(byte srcPlayerId, ChatNoteTypes noteType)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(srcPlayerId);
			DestroyableSingleton<HudManager>.Instance.Chat.AddChatNote(playerById, noteType);
		}
		RpcSendChatNoteMessage rpcMessage = new RpcSendChatNoteMessage(NetId, srcPlayerId, noteType);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void CmdReportDeadBody(NetworkedPlayerInfo target)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			ReportDeadBody(target);
			return;
		}
		RpcReportDeadBodyMessage rpcMessage = new RpcReportDeadBodyMessage(NetId, target?.PlayerId ?? byte.MaxValue);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public void RpcStartMeeting(NetworkedPlayerInfo info)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			StartMeeting(info);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 14, SendOption.Reliable);
		messageWriter.Write(info?.PlayerId ?? byte.MaxValue);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void CmdCheckMurder(PlayerControl target)
	{
		isKilling = true;
		if (AmongUsClient.Instance.AmLocalHost || AmongUsClient.Instance.AmModdedHost)
		{
			CheckMurder(target);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 47, SendOption.Reliable);
		messageWriter.WriteNetObject(target);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void CmdCheckProtect(PlayerControl target)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			CheckProtect(target);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 48, SendOption.Reliable);
		messageWriter.WriteNetObject(target);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcMurderPlayer(PlayerControl target, bool didSucceed)
	{
		MurderResultFlags murderResultFlags = (didSucceed ? MurderResultFlags.Succeeded : MurderResultFlags.FailedError);
		MurderResultFlags murderResultFlags2 = MurderResultFlags.DecisionByHost | murderResultFlags;
		if (AmongUsClient.Instance.AmClient)
		{
			MurderPlayer(target, murderResultFlags2);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 12, SendOption.Reliable);
		messageWriter.WriteNetObject(target);
		messageWriter.Write((int)murderResultFlags2);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcUseZipline(PlayerControl target, ZiplineBehaviour ziplineBehaviour, bool fromTop)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			ziplineBehaviour.Use(target, fromTop);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 52, SendOption.Reliable);
		messageWriter.Write(fromTop);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcTriggerSpores(Mushroom mushroom)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			mushroom.TriggerSpores();
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 53, SendOption.Reliable);
		messageWriter.Write(mushroom.Id);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcProtectPlayer(PlayerControl target, int colorId)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			ProtectPlayer(target, colorId);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 45, SendOption.Reliable);
		messageWriter.WriteNetObject(target);
		messageWriter.Write(colorId);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcShapeshift(PlayerControl target, bool shouldAnimate)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			Shapeshift(target, shouldAnimate);
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 46, SendOption.Reliable);
		messageWriter.WriteNetObject(target);
		messageWriter.Write(shouldAnimate);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void CmdCheckShapeshift(PlayerControl target, bool shouldAnimate)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			CheckShapeshift(target, shouldAnimate);
			return;
		}
		waitingForShapeshiftResponse = true;
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 55, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.WriteNetObject(target);
		messageWriter.Write(shouldAnimate);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcRejectShapeshift()
	{
		if (AmongUsClient.Instance.AmHost)
		{
			MessageWriter msg = AmongUsClient.Instance.StartRpcImmediately(NetId, 56, SendOption.Reliable);
			AmongUsClient.Instance.FinishRpcImmediately(msg);
		}
	}

	public void RpcSyncSettings(byte[] optionsByteArray)
	{
		if (AmongUsClient.Instance.AmHost && !DestroyableSingleton<TutorialManager>.InstanceExists)
		{
			MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 2, SendOption.Reliable);
			messageWriter.WriteBytesAndSize(optionsByteArray);
			AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
		}
	}

	public override void ClearOrDecrementDirt()
	{
	}

	public override bool Serialize(MessageWriter writer, bool initialState)
	{
		if (initialState)
		{
			writer.Write(!hasBeenSerialized);
			hasBeenSerialized = true;
		}
		writer.Write(PlayerId);
		return true;
	}

	public override void Deserialize(MessageReader reader, bool initialState)
	{
		if (initialState)
		{
			isNew = reader.ReadBoolean();
		}
		PlayerId = reader.ReadByte();
	}

	public override void HandleRpc(byte callId, MessageReader reader)
	{
		RpcCalls rpcCalls = (RpcCalls)callId;
		switch (rpcCalls)
		{
		case RpcCalls.UsePlatform:
			if (AmongUsClient.Instance.AmHost)
			{
				AirshipStatus airshipStatus = ShipStatus.Instance as AirshipStatus;
				if ((bool)airshipStatus)
				{
					airshipStatus.GapPlatform.Use(this);
					SetDirtyBit(4096u);
				}
			}
			break;
		case RpcCalls.UseZipline:
		{
			FungleShipStatus fungleShipStatus3 = ShipStatus.Instance as FungleShipStatus;
			if ((bool)fungleShipStatus3)
			{
				bool fromTop = reader.ReadBoolean();
				fungleShipStatus3.Zipline.Use(this, fromTop);
			}
			break;
		}
		case RpcCalls.CheckZipline:
		{
			FungleShipStatus fungleShipStatus4 = ShipStatus.Instance as FungleShipStatus;
			if ((bool)fungleShipStatus4)
			{
				bool fromTop2 = reader.ReadBoolean();
				CheckUseZipline(this, fungleShipStatus4.Zipline, fromTop2);
			}
			break;
		}
		case RpcCalls.CheckSpore:
		{
			int mushroomId2 = reader.ReadPackedInt32();
			FungleShipStatus fungleShipStatus2 = ShipStatus.Instance as FungleShipStatus;
			if ((bool)fungleShipStatus2)
			{
				Mushroom mushroomFromId = fungleShipStatus2.GetMushroomFromId(mushroomId2);
				CheckSporeTrigger(mushroomFromId);
			}
			break;
		}
		case RpcCalls.TriggerSpores:
		{
			int mushroomId = reader.ReadPackedInt32();
			FungleShipStatus fungleShipStatus = ShipStatus.Instance as FungleShipStatus;
			if ((bool)fungleShipStatus)
			{
				fungleShipStatus.TriggerSporesFromMushroom(mushroomId);
			}
			break;
		}
		case RpcCalls.PlayAnimation:
			PlayAnimation(reader.ReadByte());
			break;
		case RpcCalls.Exiled:
			Exiled();
			break;
		case RpcCalls.CheckName:
			CheckName(reader.ReadString());
			break;
		case RpcCalls.SetName:
			reader.ReadUInt32();
			SetName(reader.ReadString());
			break;
		case RpcCalls.CheckColor:
			CheckColor(reader.ReadByte());
			break;
		case RpcCalls.SetColor:
			reader.ReadUInt32();
			SetColor(reader.ReadByte());
			break;
		case RpcCalls.SetHatStr:
		case RpcCalls.SetSkinStr:
		case RpcCalls.SetPetStr:
		case RpcCalls.SetVisorStr:
		case RpcCalls.SetNamePlateStr:
		{
			string cosmeticId = reader.ReadString();
			byte sequenceId = reader.ReadByte();
			StartCoroutine(CoSetCosmetic(rpcCalls, cosmeticId, sequenceId));
			break;
		}
		case RpcCalls.SetLevel:
			SetLevel(reader.ReadPackedUInt32());
			break;
		case RpcCalls.SetRole:
		{
			RoleTypes role = (RoleTypes)reader.ReadUInt16();
			bool canOverride = reader.ReadBoolean();
			StartCoroutine(CoSetRole(role, canOverride));
			break;
		}
		case RpcCalls.ReportDeadBody:
		{
			NetworkedPlayerInfo playerById3 = GameData.Instance.GetPlayerById(reader.ReadByte());
			ReportDeadBody(playerById3);
			break;
		}
		case RpcCalls.StartMeeting:
		{
			NetworkedPlayerInfo playerById2 = GameData.Instance.GetPlayerById(reader.ReadByte());
			StartMeeting(playerById2);
			break;
		}
		case RpcCalls.CheckMurder:
		{
			PlayerControl target5 = reader.ReadNetObject<PlayerControl>();
			CheckMurder(target5);
			break;
		}
		case RpcCalls.CheckProtect:
		{
			PlayerControl target4 = reader.ReadNetObject<PlayerControl>();
			CheckProtect(target4);
			break;
		}
		case RpcCalls.CheckShapeshift:
		{
			PlayerControl target3 = reader.ReadNetObject<PlayerControl>();
			CheckShapeshift(target3, reader.ReadBoolean());
			break;
		}
		case RpcCalls.RejectShapeshift:
			RejectShapeshift();
			break;
		case RpcCalls.MurderPlayer:
		{
			PlayerControl target2 = reader.ReadNetObject<PlayerControl>();
			MurderResultFlags resultFlags = (MurderResultFlags)reader.ReadInt32();
			MurderPlayer(target2, resultFlags);
			break;
		}
		case RpcCalls.ProtectPlayer:
		{
			PlayerControl target = reader.ReadNetObject<PlayerControl>();
			ProtectPlayer(target, reader.ReadByte());
			break;
		}
		case RpcCalls.SetStartCounter:
		{
			int num = reader.ReadPackedInt32();
			sbyte startCounter = reader.ReadSByte();
			if (DestroyableSingleton<GameStartManager>.InstanceExists && LastStartCounter < num)
			{
				LastStartCounter = num;
				DestroyableSingleton<GameStartManager>.Instance.SetStartCounter(startCounter);
			}
			break;
		}
		case RpcCalls.CompleteTask:
			CompleteTask(reader.ReadPackedUInt32());
			break;
		case RpcCalls.SendChat:
		{
			string chatText = reader.ReadString();
			if ((bool)DestroyableSingleton<HudManager>.Instance)
			{
				DestroyableSingleton<HudManager>.Instance.Chat.AddChat(this, chatText);
			}
			break;
		}
		case RpcCalls.SendQuickChat:
		{
			QuickChatPhraseBuilderResult quickChatPhraseBuilderResult = QuickChatNetData.Deserialize(reader);
			if ((bool)DestroyableSingleton<HudManager>.Instance)
			{
				DestroyableSingleton<HudManager>.Instance.Chat.AddChat(this, quickChatPhraseBuilderResult.ToChatText(), censor: false);
			}
			break;
		}
		case RpcCalls.SendChatNote:
		{
			PlayerId playerId = reader.ReadByte();
			ChatNoteTypes chatNoteTypes = (ChatNoteTypes)reader.ReadByte();
			if (chatNoteTypes == ChatNoteTypes.DidVote)
			{
				MeetingHud.Instance.MarkPlayerAsHasVoted(playerId);
			}
			NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(playerId);
			DestroyableSingleton<HudManager>.Instance.Chat.AddChatNote(playerById, chatNoteTypes);
			break;
		}
		case RpcCalls.SetScanner:
			SetScanner(reader.ReadBoolean(), reader.ReadByte());
			break;
		case RpcCalls.Shapeshift:
		{
			PlayerControl targetPlayer = reader.ReadNetObject<PlayerControl>();
			Shapeshift(targetPlayer, reader.ReadBoolean());
			break;
		}
		default:
			HandleRoleRpc(callId, reader);
			break;
		}
	}

	public virtual void HandleRoleRpc(byte callId, MessageReader reader)
	{
		Data.Role.HandleRoleRpc(callId, reader);
	}

	public void HandleServerVanish()
	{
		if (LocalPlayer != this)
		{
			SetRoleInvisibility(isActive: true, shouldAnimate: true, playFullAnimation: true);
			return;
		}
		PhantomRole phantomRole = Data.Role as PhantomRole;
		if (phantomRole != null)
		{
			if (phantomRole.IsFading)
			{
				phantomRole.SetServerApproval(approved: true);
			}
			else
			{
				ContinueVanish();
			}
		}
	}

	public void HandleServerAppear(bool shouldAnimate)
	{
		SetRoleInvisibility(isActive: false, shouldAnimate);
	}

	public void CheckVanish()
	{
		if (AmongUsClient.Instance.AmHost)
		{
			if (AmongUsClient.Instance.AmClient)
			{
				SetRoleInvisibility(isActive: true, shouldAnimate: true, playFullAnimation: true);
			}
			RpcVanish();
		}
	}

	public void RpcVanish()
	{
		MessageWriter msg = AmongUsClient.Instance.StartRpcImmediately(NetId, 63, SendOption.Reliable);
		AmongUsClient.Instance.FinishRpcImmediately(msg);
	}

	public void CheckAppear(bool shouldAnimate)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			if (AmongUsClient.Instance.AmClient)
			{
				SetRoleInvisibility(isActive: false, shouldAnimate, playFullAnimation: true);
			}
			RpcAppear(shouldAnimate);
		}
	}

	public void RpcAppear(bool shouldAnimate)
	{
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 65, SendOption.Reliable);
		messageWriter.Write(shouldAnimate);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void CmdCheckVanish(float maxDuration)
	{
		if (AmongUsClient.Instance.AmLocalHost)
		{
			CheckVanish();
			return;
		}
		SetRoleInvisibility(isActive: true);
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 62, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write(maxDuration);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void CmdCheckAppear(bool shouldAnimate)
	{
		if (AmongUsClient.Instance.AmLocalHost)
		{
			CheckAppear(shouldAnimate);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 64, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write(shouldAnimate);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}
}
