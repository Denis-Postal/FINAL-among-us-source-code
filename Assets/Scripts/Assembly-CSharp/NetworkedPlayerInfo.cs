using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using AmongUs.InnerNet.GameDataMessages;
using Hazel;
using InnerNet;
using UnityEngine;

public class NetworkedPlayerInfo : InnerNetObject
{
	public class TaskInfo
	{
		public uint Id;

		public byte TypeId;

		public bool Complete;

		public TaskInfo()
		{
		}

		public TaskInfo(byte typeId, uint id)
		{
			Id = id;
			TypeId = typeId;
		}

		public void Serialize(MessageWriter writer)
		{
			writer.WritePacked(Id);
			writer.Write(Complete);
		}

		public void Deserialize(MessageReader reader)
		{
			Id = reader.ReadPackedUInt32();
			Complete = reader.ReadBoolean();
		}
	}

	public class PlayerOutfit
	{
		private const string MISSING_COSMETIC_ID = "missing";

		public int ColorId = -1;

		public string HatId = "missing";

		public string PetId = "missing";

		public string SkinId = "missing";

		public string VisorId = "missing";

		public string NamePlateId = "missing";

		public string PlayerName = string.Empty;

		public byte HatSequenceId;

		public byte PetSequenceId;

		public byte SkinSequenceId;

		public byte VisorSequenceId;

		public byte NamePlateSequenceId;

		public bool IsIncomplete
		{
			get
			{
				if (!string.IsNullOrEmpty(PlayerName) && ColorId != -1 && !(HatId == "missing") && !(PetId == "missing") && !(SkinId == "missing") && !(VisorId == "missing"))
				{
					return NamePlateId == "missing";
				}
				return true;
			}
		}

		public override string ToString()
		{
			return $"name={PlayerName},color={ColorId},hat={HatId},pet={PetId},skin={SkinId},visor={VisorId},nameplate={NamePlateId}";
		}

		public void Serialize(MessageWriter writer)
		{
			writer.Write(PlayerName);
			writer.WritePacked(ColorId);
			writer.Write(HatId);
			writer.Write(PetId);
			writer.Write(SkinId);
			writer.Write(VisorId);
			writer.Write(NamePlateId);
			writer.Write(HatSequenceId);
			writer.Write(PetSequenceId);
			writer.Write(SkinSequenceId);
			writer.Write(VisorSequenceId);
			writer.Write(NamePlateSequenceId);
		}

		public void Deserialize(MessageReader reader)
		{
			PlayerName = reader.ReadString();
			ColorId = reader.ReadPackedInt32();
			HatId = reader.ReadString();
			PetId = reader.ReadString();
			SkinId = reader.ReadString();
			VisorId = reader.ReadString();
			NamePlateId = reader.ReadString();
			HatSequenceId = reader.ReadByte();
			PetSequenceId = reader.ReadByte();
			SkinSequenceId = reader.ReadByte();
			VisorSequenceId = reader.ReadByte();
			NamePlateSequenceId = reader.ReadByte();
		}
	}

	private const int MAX_NETOBJECT_INIT_ATTEMPTS = 60;

	private const float NETOBJECT_INIT_ATTEMPT_WAIT_SECONDS = 0.1f;

	public byte PlayerId;

	public int ClientId;

	public string FriendCode;

	public string Puid;

	public RoleTypes RoleType;

	public RoleTypes? RoleWhenAlive;

	public Dictionary<PlayerOutfitType, PlayerOutfit> Outfits = new Dictionary<PlayerOutfitType, PlayerOutfit> { 
	{
		PlayerOutfitType.Default,
		new PlayerOutfit()
	} };

	public uint PlayerLevel = uint.MaxValue;

	public bool Disconnected;

	public RoleBehaviour Role;

	public List<TaskInfo> Tasks = new List<TaskInfo>();

	public bool IsDead;

	public bool WasEjected;

	private PlayerControl _object;

	public bool IsIncomplete
	{
		get
		{
			if (Outfits.Count != 0 && !Outfits.Values.Any((PlayerOutfit outfit) => outfit.IsIncomplete))
			{
				return PlayerLevel == uint.MaxValue;
			}
			return true;
		}
	}

	public PlayerControl Object
	{
		get
		{
			if (!_object)
			{
				_object = PlayerControl.AllPlayerControls.FirstOrDefault((PlayerControl p) => p.PlayerId == PlayerId);
			}
			return _object;
		}
	}

	public PlayerOutfit DefaultOutfit => Outfits[PlayerOutfitType.Default];

	public string PlayerName
	{
		get
		{
			return GetPlayerName(PlayerOutfitType.Default);
		}
		set
		{
			DefaultOutfit.PlayerName = value;
			gameObject.name = value + "Data";
		}
	}

	public Color Color => Palette.PlayerColors[Outfits[PlayerOutfitType.Default].ColorId];

	public Color ShadowColor => Palette.ShadowColors[Outfits[PlayerOutfitType.Default].ColorId];

	public string ColorName => "(" + GetPlayerColorString() + ")";

	public void Init(PlayerControl pc, int clientId)
	{
		PlayerId = pc.PlayerId;
		ClientId = clientId;
		FriendCode = pc.FriendCode;
		Puid = pc.Puid;
		IsDead = false;
		PlayerLevel = uint.MaxValue;
		_object = pc;
		MarkDirty();
	}

	public void MarkDirty()
	{
		SetDirtyBit(1u);
	}

	public override void ClearOrDecrementDirt()
	{
		ClearDirtyBits();
	}

	public override bool Serialize(MessageWriter writer, bool initialState)
	{
		writer.Write(PlayerId);
		writer.WritePacked(ClientId);
		writer.Write((byte)Outfits.Count);
		foreach (KeyValuePair<PlayerOutfitType, PlayerOutfit> outfit in Outfits)
		{
			writer.Write((byte)outfit.Key);
			outfit.Value.Serialize(writer);
		}
		writer.WritePacked(PlayerLevel);
		byte b = 0;
		if (Disconnected)
		{
			b |= 1;
		}
		if (IsDead)
		{
			b |= 4;
		}
		writer.Write(b);
		writer.Write((ushort)Role.Role);
		writer.Write(RoleWhenAlive.HasValue);
		if (RoleWhenAlive.HasValue)
		{
			writer.Write((ushort)RoleWhenAlive.Value);
		}
		if (Tasks != null)
		{
			writer.Write((byte)Tasks.Count);
			for (int i = 0; i < Tasks.Count; i++)
			{
				Tasks[i].Serialize(writer);
			}
		}
		else
		{
			writer.Write((byte)0);
		}
		writer.Write(FriendCode ?? string.Empty);
		writer.Write(Puid ?? string.Empty);
		return true;
	}

	public override void Deserialize(MessageReader reader, bool initialState)
	{
		PlayerId = reader.ReadByte();
		ClientId = reader.ReadPackedInt32();
		byte b = reader.ReadByte();
		Outfits.Clear();
		for (int i = 0; i < b; i++)
		{
			PlayerOutfitType key = (PlayerOutfitType)reader.ReadByte();
			PlayerOutfit playerOutfit = new PlayerOutfit();
			playerOutfit.Deserialize(reader);
			Outfits[key] = playerOutfit;
		}
		PlayerLevel = reader.ReadPackedUInt32();
		byte b2 = reader.ReadByte();
		Disconnected = (b2 & 1) != 0;
		IsDead = (b2 & 4) != 0;
		RoleType = (RoleTypes)reader.ReadUInt16();
		if (reader.ReadBoolean())
		{
			RoleWhenAlive = (RoleTypes)reader.ReadUInt16();
		}
		byte b3 = reader.ReadByte();
		Tasks.Clear();
		for (int j = 0; j < b3; j++)
		{
			TaskInfo taskInfo = new TaskInfo();
			taskInfo.Deserialize(reader);
			Tasks.Add(taskInfo);
		}
		FriendCode = reader.ReadString();
		Puid = reader.ReadString();
		if (initialState && GameData.Instance.GetPlayerById(PlayerId) == null && !GameData.Instance.IsProcessingInfo(this))
		{
			GameData.Instance.AddPlayerInfo(this);
		}
		if (!initialState && Object != null)
		{
			Object.MyPhysics.ResetAnimState();
		}
		GameData.Instance.RecomputeTaskCounts();
	}

	public string GetPlayerName(PlayerOutfitType outfitType)
	{
		if (!Outfits.ContainsKey(outfitType))
		{
			return string.Empty;
		}
		return Outfits[outfitType].PlayerName;
	}

	public void CensorNameAsync(Action callback = null)
	{
		StartCoroutine(CoCensorNameAsync(callback));
	}

	private IEnumerator CoCensorNameAsync(Action callback = null)
	{
		if (DestroyableSingleton<TutorialManager>.InstanceExists || ((bool)Object && Object.isDummy))
		{
			callback?.Invoke();
			yield break;
		}
		ClientData clientFromPlayerInfo = AmongUsClient.Instance.GetClientFromPlayerInfo(this);
		int attempts = 0;
		while (clientFromPlayerInfo == null && attempts < 60)
		{
			attempts++;
			yield return Effects.Wait(0.1f);
			clientFromPlayerInfo = AmongUsClient.Instance.GetClientFromPlayerInfo(this);
		}
		if (clientFromPlayerInfo == null)
		{
			Debug.LogWarning(string.Format("{0} couldn't find ClientData for player {1}", "CoCensorNameAsync", PlayerId));
			yield break;
		}
		string playerName = GetPlayerName(PlayerOutfitType.Default);
		UpdateName(playerName, clientFromPlayerInfo);
		callback?.Invoke();
	}

	public string GetPlayerColorString(PlayerOutfitType outfitType = PlayerOutfitType.Default)
	{
		if (!Outfits.ContainsKey(outfitType))
		{
			return string.Empty;
		}
		char[] array = Palette.GetColorName(Outfits[outfitType].ColorId).ToCharArray();
		if (array.Length != 0)
		{
			array[0] = char.ToUpper(array[0]);
			for (int i = 1; i < array.Length; i++)
			{
				array[i] = char.ToLower(array[i]);
			}
		}
		return new string(array);
	}

	public TaskInfo FindTaskById(uint taskId)
	{
		for (int i = 0; i < Tasks.Count; i++)
		{
			if (Tasks[i].Id == taskId)
			{
				return Tasks[i];
			}
		}
		return null;
	}

	public void SetOutfit(PlayerOutfitType outfitType, PlayerOutfit outfit)
	{
		Outfits[outfitType] = outfit;
	}

	public void UpdateNamePlate(string namePlate)
	{
		DefaultOutfit.NamePlateId = namePlate;
		MarkDirty();
	}

	public void UpdateVisor(string visor)
	{
		DefaultOutfit.VisorId = visor;
		MarkDirty();
		UpdateHostPanelImage();
	}

	public void UpdateSkin(string skin)
	{
		DefaultOutfit.SkinId = skin;
		MarkDirty();
		UpdateHostPanelImage();
	}

	public void UpdatePet(string petId)
	{
		DefaultOutfit.PetId = petId;
		MarkDirty();
		UpdateHostPanelImage();
	}

	public void UpdateHat(string hat)
	{
		DefaultOutfit.HatId = hat;
		MarkDirty();
		UpdateHostPanelImage();
	}

	public void UpdateColor(int colorId)
	{
		StartCoroutine(CoUpdateColor(colorId));
	}

	private IEnumerator CoUpdateColor(int colorId)
	{
		DefaultOutfit.ColorId = colorId;
		if (!Object || !Object.isDummy)
		{
			ClientData clientData = AmongUsClient.Instance.GetClientFromCharacter(Object);
			int attempts = 0;
			while (clientData == null && attempts < 60)
			{
				attempts++;
				yield return Effects.Wait(0.1f);
				clientData = AmongUsClient.Instance.GetClientFromPlayerInfo(this);
			}
			if (clientData == null)
			{
				Debug.LogWarning(string.Format("{0} couldn't find ClientData for player {1}", "CoUpdateColor", PlayerId));
				yield break;
			}
			clientData.ColorId = colorId;
			UpdateHostPanelImage();
			MarkDirty();
		}
	}

	private void UpdateHostPanelImage()
	{
		if (DestroyableSingleton<GameStartManager>.InstanceExists && GameData.Instance.GetHost() == this)
		{
			DestroyableSingleton<GameStartManager>.Instance.UpdateHostPanelImage(this);
		}
	}

	public void UpdateLevel(uint level)
	{
		PlayerLevel = level;
		MarkDirty();
	}

	private void UpdateName(string playerName, ClientData client)
	{
		PlayerName = playerName;
		client.UpdatePlayerName(playerName);
		MarkDirty();
	}

	public void OnGameStart()
	{
		if (Object != null)
		{
			Object.OnGameStart();
		}
	}

	public void OnGameEnd()
	{
		if (Object != null)
		{
			Object.OnGameEnd();
		}
	}

	private void SetTasks(byte[] taskTypeIds)
	{
		if (Disconnected)
		{
			return;
		}
		if (Object == null)
		{
			Debug.Log("Could not set tasks for player (" + PlayerName + "): " + PlayerId);
			return;
		}
		if (GameManager.Instance.IsHideAndSeek() && RoleType == RoleTypes.Impostor)
		{
			taskTypeIds = new byte[0];
		}
		Tasks.Clear();
		for (int i = 0; i < taskTypeIds.Length; i++)
		{
			Tasks.Add(new TaskInfo(taskTypeIds[i], (uint)i));
		}
		Object.SetTasks(Tasks);
		MarkDirty();
	}

	public void RpcSetTasks(byte[] taskTypeIds)
	{
		if (AmongUsClient.Instance.AmClient)
		{
			SetTasks(taskTypeIds);
		}
		RpcSetTasksMessage rpcMessage = new RpcSetTasksMessage(NetId, taskTypeIds);
		AmongUsClient.Instance.LateBroadcastReliableMessage(rpcMessage);
	}

	public override void HandleRpc(byte callId, MessageReader reader)
	{
		if (callId == 29)
		{
			SetTasks(reader.ReadBytesAndSize());
		}
	}
}
