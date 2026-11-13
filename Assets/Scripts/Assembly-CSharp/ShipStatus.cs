using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Hazel;
using InnerNet;
using PowerTools;
using UnityEngine;

public class ShipStatus : InnerNetObject
{
	public enum MapType
	{
		Ship,
		Hq,
		Pb,
		Fungle
	}

	public class SystemTypeComparer : IEqualityComparer<SystemTypes>
	{
		public static readonly SystemTypeComparer Instance = new SystemTypeComparer();

		public bool Equals(SystemTypes x, SystemTypes y)
		{
			return x == y;
		}

		public int GetHashCode(SystemTypes obj)
		{
			return (int)obj;
		}
	}

	public static ShipStatus Instance;

	public Color CameraColor = Color.black;

	public float MaxLightRadius = 5f;

	public float MinLightRadius = 1f;

	public float MapScale = 4.4f;

	public MapBehaviour MapPrefab;

	public ExileController ExileCutscenePrefab;

	public MeetingCalledAnimation EmergencyOverlay;

	public MeetingCalledAnimation ReportOverlay;

	public Sprite MeetingBackground;

	public Sprite BrokenEmergencyButton;

	public SystemConsole EmergencyButton;

	public Vector2 InitialSpawnCenter;

	public Vector2 MeetingSpawnCenter;

	public Vector2 MeetingSpawnCenter2;

	public float SpawnRadius = 1.55f;

	public NormalPlayerTask[] CommonTasks;

	public NormalPlayerTask[] LongTasks;

	public NormalPlayerTask[] ShortTasks;

	public PlayerTask[] SpecialTasks;

	public Transform[] DummyLocations;

	[NonSerialized]
	public SurvCamera[] AllCameras;

	public OpenableDoor[] AllDoors;

	[NonSerialized]
	public Console[] AllConsoles;

	[NonSerialized]
	public Ladder[] Ladders;

	public Dictionary<SystemTypes, ISystemType> Systems;

	public StringNames[] SystemNames;

	public StringNames[] ExtraTaskNames;

	public AudioClip SabotageSound;

	public AnimationClip[] WeaponFires;

	public SpriteAnim WeaponsImage;

	public AudioClip[] VentMoveSounds;

	public AudioClip VentEnterSound;

	public AudioClip VentExitSound;

	public AnimationClip HatchActive;

	public SpriteAnim Hatch;

	public ParticleSystem HatchParticles;

	public AnimationClip ShieldsActive;

	public SpriteAnim[] ShieldsImages;

	public SpriteRenderer ShieldBorder;

	public Sprite ShieldBorderOn;

	public MedScannerBehaviour MedScanner;

	private int WeaponFireIdx;

	public float Timer;

	public float EmergencyCooldown;

	public MapType Type;

	protected readonly Logger logger = new Logger(Logger.Category.Gameplay, "ShipStatus");

	[SerializeField]
	private SocialMediumFeedSystem socialMediumFeedSystemPrefab;

	protected SocialMediumFeedSystem socialMediumFeedSystem;

	private int numScans;

	public IStepWatcher[] AllStepWatchers { get; private set; }

	public PlainShipRoom[] AllRooms { get; private set; }

	public Dictionary<SystemTypes, PlainShipRoom> FastRooms { get; private set; }

	public Vent[] AllVents { get; private set; }

	public float HideCountdown { get; set; }

	public CosmeticsCache CosmeticsCache { get; set; }

	public override bool IsDirty
	{
		get
		{
			foreach (KeyValuePair<SystemTypes, ISystemType> system in Systems)
			{
				if (system.Value.IsDirty)
				{
					return true;
				}
			}
			return false;
		}
	}

	protected virtual void OnEnable()
	{
		if (Systems != null)
		{
			return;
		}
		Systems = new Dictionary<SystemTypes, ISystemType>(SystemTypeComparer.Instance)
		{
			{
				SystemTypes.Electrical,
				new SwitchSystem()
			},
			{
				SystemTypes.MedBay,
				new MedScanSystem()
			}
		};
		Camera main = Camera.main;
		main.backgroundColor = CameraColor;
		FollowerCamera component = main.GetComponent<FollowerCamera>();
		switch (Type)
		{
		case MapType.Pb:
			DestroyableSingleton<HudManager>.Instance.ShadowQuad.material.SetInt("_Mask", 7);
			Systems.Add(SystemTypes.Doors, new DoorsSystemType());
			Systems.Add(SystemTypes.Comms, new HudOverrideSystemType());
			Systems.Add(SystemTypes.Security, new SecurityCameraSystemType());
			Systems.Add(SystemTypes.Laboratory, new ReactorSystemType(60f, SystemTypes.Laboratory));
			if ((bool)component)
			{
				component.shakeAmount = 0f;
				component.shakePeriod = 0f;
			}
			break;
		case MapType.Hq:
			Systems.Add(SystemTypes.Comms, new HqHudSystemType());
			Systems.Add(SystemTypes.Reactor, new ReactorSystemType(45f, SystemTypes.Reactor));
			Systems.Add(SystemTypes.LifeSupp, new LifeSuppSystemType(45f));
			Systems.Add(SystemTypes.Ventilation, new VentilationSystem());
			if ((bool)component)
			{
				component.shakeAmount = 0f;
				component.shakePeriod = 0f;
			}
			break;
		case MapType.Ship:
			Systems.Add(SystemTypes.Doors, new AutoDoorsSystemType());
			Systems.Add(SystemTypes.Comms, new HudOverrideSystemType());
			Systems.Add(SystemTypes.Security, new SecurityCameraSystemType());
			Systems.Add(SystemTypes.Reactor, new ReactorSystemType(30f, SystemTypes.Reactor));
			Systems.Add(SystemTypes.LifeSupp, new LifeSuppSystemType(30f));
			Systems.Add(SystemTypes.Ventilation, new VentilationSystem());
			if ((bool)component)
			{
				component.shakeAmount = 0.02f;
				component.shakePeriod = 0.3f;
			}
			break;
		}
		Systems.Add(SystemTypes.Sabotage, new SabotageSystemType(Systems.Values.Where((ISystemType i) => i is IActivatable).Cast<IActivatable>().ToArray()));
	}

	public virtual void RepairCriticalSabotages()
	{
		UpdateSystem(SystemTypes.Laboratory, PlayerControl.LocalPlayer, 16);
		UpdateSystem(SystemTypes.Reactor, PlayerControl.LocalPlayer, 16);
		UpdateSystem(SystemTypes.LifeSupp, PlayerControl.LocalPlayer, 16);
		DestroyableSingleton<AchievementManager>.Instance.OnSabotageCleared();
	}

	private void Awake()
	{
		AllStepWatchers = (from s in GetComponentsInChildren<IStepWatcher>()
			orderby s.Priority descending
			select s).ToArray();
		AllRooms = GetComponentsInChildren<PlainShipRoom>();
		FastRooms = AllRooms.Where((PlainShipRoom p) => p.RoomId != SystemTypes.Hallway).ToDictionary((PlainShipRoom d) => d.RoomId);
		AllCameras = GetComponentsInChildren<SurvCamera>();
		AllConsoles = GetComponentsInChildren<Console>();
		AllVents = GetComponentsInChildren<Vent>();
		Ladders = GetComponentsInChildren<Ladder>();
		AssignTaskIndexes();
		CosmeticsCache = new CosmeticsCache();
		InitializeExtraTaskNames();
		socialMediumFeedSystem = UnityEngine.Object.Instantiate(socialMediumFeedSystemPrefab, transform);
		Instance = this;
	}

	protected virtual void Start()
	{
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			DestroyableSingleton<HudManager>.Instance.Chat.ForceClosed();
			DestroyableSingleton<HudManager>.Instance.Chat.SetVisible(visible: false);
		}
		DeconSystem[] componentsInChildren = GetComponentsInChildren<DeconSystem>();
		foreach (DeconSystem deconSystem in componentsInChildren)
		{
			Systems.Add(deconSystem.TargetSystem, deconSystem);
		}
	}

	public override void OnDestroy()
	{
		CosmeticsCache.Destroy();
		if ((bool)SoundManager.Instance)
		{
			SoundManager.Instance.StopAllSound();
		}
		base.OnDestroy();
	}

	public virtual void SpawnPlayer(PlayerControl player, int numPlayers, bool initialSpawn)
	{
		Vector2 up = Vector2.up;
		up = up.Rotate((float)(player.PlayerId - 1) * (360f / (float)numPlayers));
		up *= SpawnRadius;
		Vector2 position = (initialSpawn ? InitialSpawnCenter : MeetingSpawnCenter) + up + new Vector2(0f, 0.3636f);
		if (player.AmOwner)
		{
			player.NetTransform.RpcSnapTo(position);
		}
		else
		{
			player.NetTransform.SnapTo(position);
		}
	}

	public void StartShields()
	{
		for (int i = 0; i < ShieldsImages.Length; i++)
		{
			ShieldsImages[i].Play(ShieldsActive);
		}
		ShieldBorder.sprite = ShieldBorderOn;
	}

	public void FireWeapon()
	{
		if ((bool)WeaponsImage && !WeaponsImage.IsPlaying())
		{
			WeaponsImage.Play(WeaponFires[WeaponFireIdx]);
			WeaponFireIdx = (WeaponFireIdx + 1) % WeaponFires.Length;
		}
	}

	public NormalPlayerTask GetTaskById(byte idx)
	{
		return CommonTasks.FirstOrDefault((NormalPlayerTask t) => t.Index == idx) ?? LongTasks.FirstOrDefault((NormalPlayerTask t) => t.Index == idx) ?? ShortTasks.FirstOrDefault((NormalPlayerTask t) => t.Index == idx);
	}

	public PlayerTask[] GetAllTasks()
	{
		return CommonTasks.Concat(LongTasks).Concat(ShortTasks).ToArray();
	}

	public bool HasTaskType<TTaskType>() where TTaskType : PlayerTask
	{
		return HasTaskTypes(typeof(TTaskType));
	}

	public bool HasTaskTypes(params Type[] types)
	{
		PlayerTask[] commonTasks = CommonTasks;
		if (!CheckTaskArray(commonTasks))
		{
			commonTasks = LongTasks;
			if (!CheckTaskArray(commonTasks))
			{
				commonTasks = ShortTasks;
				return CheckTaskArray(commonTasks);
			}
		}
		return true;
		bool CheckTaskArray(PlayerTask[] tasks)
		{
			foreach (PlayerTask playerTask in tasks)
			{
				Type[] array = types;
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j].IsAssignableFrom(playerTask.GetType()))
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	private void InitializeExtraTaskNames()
	{
		List<StringNames> list = new List<StringNames>(ExtraTaskNames);
		if (HasTaskTypes(typeof(UploadDataTask), typeof(AirshipUploadTask)))
		{
			list.Add(StringNames.UploadData);
			list.Add(StringNames.DownloadData);
		}
		ExtraTaskNames = list.Distinct().ToArray();
	}

	public void OpenHatch()
	{
		if ((bool)Hatch && !Hatch.IsPlaying())
		{
			Hatch.Play(HatchActive);
			HatchParticles.Play();
		}
	}

	public void CloseDoorsOfType(SystemTypes room)
	{
		logger.Info($"Closing doors of room {room}");
		((IDoorSystem)Systems[SystemTypes.Doors]).CloseDoorsOfType(room);
	}

	public void UpdateSystem(SystemTypes systemType, PlayerControl player, byte amount)
	{
		if (Systems.TryGetValue(systemType, out var value))
		{
			MessageWriter messageWriter = MessageWriter.Get();
			messageWriter.Write(amount);
			MessageReader msgReader = MessageReader.Get(messageWriter.ToByteArray(includeHeader: false));
			logger.Info($"Player {player.PlayerId} modifying system {systemType} by amount {amount}");
			value.UpdateSystem(player, msgReader);
			messageWriter.Recycle();
		}
	}

	public void UpdateSystem(SystemTypes systemType, PlayerControl player, MessageReader msgReader)
	{
		if (Systems.TryGetValue(systemType, out var value))
		{
			logger.Info($"Player {player.PlayerId} modifying system {systemType}");
			value.UpdateSystem(player, msgReader);
		}
	}

	private void AssignTaskIndexes()
	{
		int num = 0;
		for (int i = 0; i < CommonTasks.Length; i++)
		{
			CommonTasks[i].Index = num++;
		}
		for (int j = 0; j < LongTasks.Length; j++)
		{
			LongTasks[j].Index = num++;
		}
		for (int k = 0; k < ShortTasks.Length; k++)
		{
			ShortTasks[k].Index = num++;
		}
	}

	public virtual void OnMeetingCalled()
	{
		socialMediumFeedSystem.OnMeetingStart();
	}

	public List<Sprite> GetSocialMediumSpriteList()
	{
		return socialMediumFeedSystem.PossibleSpriteList;
	}

	public void SendSpiritGuideMessage(byte[] spriteIndices, ClientId targetId)
	{
		socialMediumFeedSystem.SendSpiritGuideMessage(NetId, spriteIndices, targetId);
	}

	public virtual void StartSFX()
	{
	}

	public virtual IEnumerator PrespawnStep()
	{
		yield break;
	}

	public void Begin()
	{
		numScans = 0;
		AssignTaskIndexes();
		IGameOptions currentGameOptions = GameOptionsManager.Instance.CurrentGameOptions;
		List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
		HashSet<TaskTypes> hashSet = new HashSet<TaskTypes>();
		List<byte> list = new List<byte>(10);
		List<NormalPlayerTask> list2 = CommonTasks.ToList();
		list2.Shuffle();
		list2.ForEach((NormalPlayerTask t) =>
		{
			t.Length = NormalPlayerTask.TaskLength.Common;
		});
		int start = 0;
		int num = currentGameOptions.GetInt(Int32OptionNames.NumCommonTasks);
		AddTasksFromList(ref start, num, list, hashSet, list2);
		for (int num2 = 0; num2 < num; num2++)
		{
			if (list2.Count == 0)
			{
				Debug.LogWarning("Not enough common tasks");
				break;
			}
			int index = list2.RandomIdx();
			list.Add((byte)list2[index].Index);
			list2.RemoveAt(index);
		}
		List<NormalPlayerTask> list3 = LongTasks.ToList();
		list3.ForEach((NormalPlayerTask t) =>
		{
			t.Length = NormalPlayerTask.TaskLength.Long;
		});
		list3.Shuffle();
		List<NormalPlayerTask> list4 = ShortTasks.ToList();
		list4.ForEach((NormalPlayerTask t) =>
		{
			t.Length = NormalPlayerTask.TaskLength.Short;
		});
		list4.Shuffle();
		int start2 = 0;
		int start3 = 0;
		int num3 = currentGameOptions.GetInt(Int32OptionNames.NumShortTasks);
		int num4 = currentGameOptions.GetInt(Int32OptionNames.NumCommonTasks);
		int num5 = currentGameOptions.GetInt(Int32OptionNames.NumLongTasks);
		if (num4 + num5 + num3 == 0)
		{
			num3 = 1;
		}
		for (byte b = 0; b < allPlayers.Count; b++)
		{
			hashSet.Clear();
			list.RemoveRange(num4, list.Count - num4);
			AddTasksFromList(ref start2, num5, list, hashSet, list3);
			AddTasksFromList(ref start3, num3, list, hashSet, list4);
			NetworkedPlayerInfo networkedPlayerInfo = allPlayers[b];
			if ((bool)networkedPlayerInfo.Object && !networkedPlayerInfo.Object.GetComponent<DummyBehaviour>().enabled)
			{
				byte[] taskTypeIds = list.ToArray();
				networkedPlayerInfo.RpcSetTasks(taskTypeIds);
			}
		}
		PlayerControl.LocalPlayer.cosmetics.SetAsLocalPlayer();
	}

	private void AddTasksFromList(ref int start, int count, List<byte> tasks, HashSet<TaskTypes> usedTaskTypes, List<NormalPlayerTask> unusedTasks)
	{
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			if (num++ == 1000)
			{
				break;
			}
			if (start >= unusedTasks.Count)
			{
				start = 0;
				unusedTasks.Shuffle();
				if (unusedTasks.All((NormalPlayerTask t) => usedTaskTypes.Contains(t.TaskType)))
				{
					logger.Info("Not enough task types - resetting pool of tasks");
					usedTaskTypes.Clear();
				}
			}
			NormalPlayerTask normalPlayerTask = unusedTasks[start++];
			if (usedTaskTypes.Add(normalPlayerTask.TaskType))
			{
				tasks.Add((byte)normalPlayerTask.Index);
				if (!GameManager.Instance.LogicOptions.GetVisualTasks() && normalPlayerTask.TaskType == TaskTypes.SubmitScan && numScans++ > 1)
				{
					unusedTasks.Remove(normalPlayerTask);
				}
			}
			else
			{
				i--;
			}
		}
	}

	public void FixedUpdate()
	{
		if (!AmongUsClient.Instance || !PlayerControl.LocalPlayer)
		{
			return;
		}
		Timer += Time.fixedDeltaTime;
		EmergencyCooldown -= Time.fixedDeltaTime;
		if ((bool)GameData.Instance)
		{
			GameData.Instance.RecomputeTaskCounts();
		}
		if (!AmongUsClient.Instance.AmClient)
		{
			return;
		}
		for (int i = 0; i < SystemTypeHelpers.AllTypes.Length; i++)
		{
			SystemTypes key = SystemTypeHelpers.AllTypes[i];
			if (Systems.TryGetValue(key, out var value))
			{
				value.Deteriorate(Time.fixedDeltaTime);
			}
		}
	}

	public virtual float CalculateLightRadius(NetworkedPlayerInfo player)
	{
		if (player == null || player.IsDead)
		{
			return MaxLightRadius;
		}
		if (player.Role.IsImpostor)
		{
			return MaxLightRadius * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.ImpostorLightMod);
		}
		float t = 1f;
		if (Systems.TryGetValue(SystemTypes.Electrical, out var value))
		{
			t = (float)(int)(value as SwitchSystem).Value / 255f;
		}
		return Mathf.Lerp(MinLightRadius, MaxLightRadius, t) * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.CrewLightMod);
	}

	public void StartMeeting(PlayerControl reporter, NetworkedPlayerInfo target)
	{
		StartCoroutine(CoStartMeeting(reporter, target));
	}

	public PlayerTask GetSabotageTask(SystemTypes system)
	{
		TaskTypes taskTypeForSystem = system switch
		{
			SystemTypes.Reactor => TaskTypes.ResetReactor, 
			SystemTypes.Laboratory => TaskTypes.ResetSeismic, 
			SystemTypes.LifeSupp => TaskTypes.RestoreOxy, 
			SystemTypes.Electrical => TaskTypes.FixLights, 
			SystemTypes.Comms => TaskTypes.FixComms, 
			SystemTypes.MushroomMixupSabotage => TaskTypes.MushroomMixupSabotage, 
			SystemTypes.HeliSabotage => TaskTypes.StopCharles, 
			_ => TaskTypes.None, 
		};
		return SpecialTasks.First((PlayerTask task) => task.TaskType == taskTypeForSystem);
	}

	public IEnumerator CoStartMeeting(PlayerControl reporter, NetworkedPlayerInfo target)
	{
		while (!MeetingHud.Instance)
		{
			yield return null;
		}
		MeetingRoomManager.Instance.RemoveSelf();
		DestroyableSingleton<HudManager>.Instance.InitMap();
		MapBehaviour.Instance.SetPreMeetingPosition(PlayerControl.LocalPlayer.transform.position);
		for (int i = 0; i < PlayerControl.AllPlayerControls.Count; i++)
		{
			PlayerControl.AllPlayerControls[i]?.ResetForMeeting();
		}
		if ((bool)MapBehaviour.Instance)
		{
			MapBehaviour.Instance.Close();
		}
		if ((bool)Minigame.Instance)
		{
			Minigame.Instance.ForceClose();
		}
		Instance.OnMeetingCalled();
		KillAnimation.SetMovement(reporter, canMove: true);
		List<DeadBody> list = UnityEngine.Object.FindObjectsOfType<DeadBody>().ToList();
		List<byte> list2 = new List<byte>();
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j] != null && list[j].gameObject != null)
			{
				list2.Add(list[j].ParentId);
				UnityEngine.Object.Destroy(list[j].gameObject);
			}
			else
			{
				Debug.LogError("Encountered a null Dead Body while destroying.");
			}
		}
		NetworkedPlayerInfo[] deadBodies = list2.Select((byte b) => GameData.Instance.GetPlayerById(b)).ToArray();
		ShapeshifterEvidence[] array = UnityEngine.Object.FindObjectsOfType<ShapeshifterEvidence>();
		for (int num = 0; num < array.Length; num++)
		{
			if (array[num] != null && array[num].gameObject != null)
			{
				UnityEngine.Object.Destroy(array[num].gameObject);
			}
			else
			{
				Debug.LogError("Encountered a null Evidence while destroying.");
			}
		}
		NoisemakerArrow[] array2 = UnityEngine.Object.FindObjectsOfType<NoisemakerArrow>();
		for (int num2 = 0; num2 < array2.Length; num2++)
		{
			if (array2[num2] != null && array2[num2].gameObject != null)
			{
				UnityEngine.Object.Destroy(array2[num2].gameObject);
			}
			else
			{
				Debug.LogError("Encounted a null NoisemakerArrow while destroying.");
			}
		}
		for (int num3 = 0; num3 < GameData.Instance.PlayerCount; num3++)
		{
			PlayerControl playerControl = GameData.Instance.AllPlayers[num3].Object;
			if (playerControl != null && playerControl.Data != null && (bool)playerControl.Data.Role)
			{
				playerControl.Data.Role.OnMeetingStart();
			}
		}
		GameData.TimeLastMeetingStarted = Time.realtimeSinceStartup;
		MeetingHud.Instance.StartCoroutine(MeetingHud.Instance.CoIntro(reporter.Data, target, deadBodies));
	}

	public void BreakEmergencyButton()
	{
		EmergencyButton.Image.sprite = BrokenEmergencyButton;
		EmergencyButton.enabled = false;
	}

	private void ValidateAllTasksOnShip()
	{
	}

	public void RpcCloseDoorsOfType(SystemTypes type)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			CloseDoorsOfType(type);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 27, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write((byte)type);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcUpdateSystem(SystemTypes systemType, byte amount)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			UpdateSystem(systemType, PlayerControl.LocalPlayer, amount);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 35, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write((byte)systemType);
		messageWriter.WriteNetObject(PlayerControl.LocalPlayer);
		messageWriter.Write(amount);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public void RpcUpdateSystem(SystemTypes systemType, MessageWriter msgWriter)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			MessageReader msgReader = MessageReader.Get(msgWriter.ToByteArray(includeHeader: false));
			UpdateSystem(systemType, PlayerControl.LocalPlayer, msgReader);
			return;
		}
		MessageWriter messageWriter = AmongUsClient.Instance.StartRpcImmediately(NetId, 35, SendOption.Reliable, AmongUsClient.Instance.HostId);
		messageWriter.Write((byte)systemType);
		messageWriter.WriteNetObject(PlayerControl.LocalPlayer);
		messageWriter.Write(msgWriter, includeHeader: false);
		AmongUsClient.Instance.FinishRpcImmediately(messageWriter);
	}

	public override void ClearOrDecrementDirt()
	{
		for (short num = 0; num < SystemTypeHelpers.AllTypes.Length; num++)
		{
			SystemTypes key = SystemTypeHelpers.AllTypes[num];
			if (Systems.TryGetValue(key, out var value) && value.IsDirty)
			{
				value.MarkClean();
			}
		}
	}

	public override bool Serialize(MessageWriter writer, bool initialState)
	{
		bool result = false;
		for (short num = 0; num < SystemTypeHelpers.AllTypes.Length; num++)
		{
			SystemTypes systemTypes = SystemTypeHelpers.AllTypes[num];
			if (Systems.TryGetValue(systemTypes, out var value) && (initialState || value.IsDirty))
			{
				result = true;
				writer.StartMessage((byte)systemTypes);
				value.Serialize(writer, initialState);
				writer.EndMessage();
			}
		}
		return result;
	}

	public override void Deserialize(MessageReader reader, bool initialState)
	{
		while (reader.Position < reader.Length)
		{
			MessageReader messageReader = reader.ReadMessage();
			SystemTypes key = (SystemTypes)messageReader.Tag;
			if (Systems.TryGetValue(key, out var value))
			{
				value.Deserialize(messageReader, initialState);
			}
		}
	}

	public override void HandleRpc(byte callId, MessageReader reader)
	{
		switch ((RpcCalls)callId)
		{
		case RpcCalls.CloseDoorsOfType:
			CloseDoorsOfType((SystemTypes)reader.ReadByte());
			break;
		case RpcCalls.UpdateSystem:
			UpdateSystem((SystemTypes)reader.ReadByte(), reader.ReadNetObject<PlayerControl>(), reader);
			break;
		case RpcCalls.SpiritGuideMessage:
			socialMediumFeedSystem.HandleSpiritGuideMessage(reader);
			break;
		}
	}
}
