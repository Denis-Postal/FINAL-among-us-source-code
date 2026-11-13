using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Hazel;
using UnityEngine;

public class VentilationSystem : ISystemType
{
	public struct VentMoveInfo
	{
		public Operation Op;

		public byte VentId;

		public byte PlayerId;
	}

	public enum Operation
	{
		StartCleaning,
		StopCleaning,
		Enter,
		Exit,
		Move,
		BootImpostors
	}

	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<SequenceBuffer<VentMoveInfo>> _003C_003E9__11_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal SequenceBuffer<VentMoveInfo> _003CUpdate_003Eb__11_0()
		{
			return new SequenceBuffer<VentMoveInfo>(0);
		}
	}

	private sealed class _003C_003Ec__DisplayClass15_0
	{
		public ushort opId;

		internal SequenceBuffer<VentMoveInfo> _003CUpdateSystem_003Eb__0()
		{
			ushort sidStart = default;
			SequenceBuffer<VentMoveInfo> result = new SequenceBuffer<VentMoveInfo>(sidStart);
			sidStart = (ushort)(opId - 1);
			return result;
		}
	}

	private const ushort OP_ID_START = 0;

	private readonly Dictionary<byte, SequenceBuffer<VentMoveInfo>> SeqBuffers= new Dictionary<byte, SequenceBuffer<VentMoveInfo>>();

	private readonly Dictionary<byte, byte> PlayersCleaningVents= new Dictionary<byte, byte>();

	private readonly Dictionary<byte, byte> PlayersInsideVents= new Dictionary<byte, byte>();

	private readonly HashSet<byte> ToRemove= new HashSet<byte>();

	public bool IsDirty { get; private set; }

	// TODO(verify): rebuilt from decomp9 (message layout: ushort opId, byte op, byte ventId); the PerformVentOp switch was lost in decomp9
	public static void Update(Operation op, int ventId)
	{
		if (ShipStatus.Instance.Systems[SystemTypes.Ventilation] is VentilationSystem ventilationSystem)
		{
			byte playerId = PlayerControl.LocalPlayer.PlayerId;
			SequenceBuffer<VentMoveInfo> valueOrSetDefault = ventilationSystem.SeqBuffers.GetValueOrSetDefault(playerId, () => new SequenceBuffer<VentMoveInfo>(0));
			ushort num = (ushort)(valueOrSetDefault.LastSid + 1);
			MessageWriter messageWriter = MessageWriter.Get(SendOption.Reliable);
			messageWriter.Write(num);
			messageWriter.Write((byte)op);
			messageWriter.Write((byte)ventId);
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Ventilation, messageWriter);
			messageWriter.Recycle();
			valueOrSetDefault.LastSid = num;
		}
	}

	public void Deteriorate(float deltaTime)
	{
		if (!GameData.Instance)
		{
			return;
		}
		foreach (KeyValuePair<byte, byte> playersCleaningVent in PlayersCleaningVents)
		{
			byte key = playersCleaningVent.Key;
			NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(key);
			if (playerById == null || playerById.Disconnected)
			{
				ToRemove.Add(key);
			}
		}
		if (ToRemove.Count <= 0)
		{
			return;
		}
		foreach (byte item in ToRemove)
		{
			PlayersCleaningVents.Remove(item);
			PlayersInsideVents.Remove(item);
		}
		ToRemove.Clear();
		UpdateVentArrows();
		IsDirty = true;
	}

	public bool IsVentCurrentlyBeingCleaned(int id)
	{
		foreach (KeyValuePair<byte, byte> playersCleaningVent in PlayersCleaningVents)
		{
			if (playersCleaningVent.Value == id)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsImpostorInsideVent(int ventId)
	{
		foreach (KeyValuePair<byte, byte> playersInsideVent in PlayersInsideVents)
		{
			if (playersInsideVent.Value == ventId && !IsInvisiblePhantomOnVent(playersInsideVent.Key))
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateSystem(PlayerControl player, MessageReader msgReader)
	{
		ushort opId = msgReader.ReadUInt16();
		Operation operation = (Operation)msgReader.ReadByte();
		byte b = msgReader.ReadByte();
		byte playerId = player.PlayerId;
		SequenceBuffer<VentMoveInfo> valueOrSetDefault = SeqBuffers.GetValueOrSetDefault(playerId, () => new SequenceBuffer<VentMoveInfo>((ushort)(opId - 1)));
		if (valueOrSetDefault.IsInvalidSid(opId))
		{
			Debug.LogError($"Invalid Sid {opId} received; Op {operation} on vent {b} by player {playerId}. Last Sid was: {valueOrSetDefault.LastSid}");
			return;
		}
		if (valueOrSetDefault.IsNextSid(opId))
		{
			PerformVentOp(playerId, operation, b, valueOrSetDefault);
		}
		else
		{
			valueOrSetDefault.Add(opId, new VentMoveInfo
			{
				Op = operation,
				VentId = b,
				PlayerId = playerId
			});
		}
		foreach (VentMoveInfo item in valueOrSetDefault.SubsequentObjs())
		{
			PerformVentOp(item.PlayerId, item.Op, item.VentId, valueOrSetDefault);
		}
	}

	// TODO(verify): switch rebuilt from the Operation names (decomp9 lost it: invalid jump targets)
	private void PerformVentOp(byte playerId, Operation op, byte ventId, SequenceBuffer<VentMoveInfo> seqBuffer)
	{
		seqBuffer.BumpSid();
		switch (op)
		{
		case Operation.StartCleaning:
			BootImpostorsFromVent(ventId);
			PlayersCleaningVents[playerId] = ventId;
			break;
		case Operation.StopCleaning:
		{
			KeyValuePair<byte, byte>[] array = PlayersCleaningVents.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<byte, byte> keyValuePair = array[i];
				if (keyValuePair.Value == ventId)
				{
					PlayersCleaningVents.Remove(keyValuePair.Key);
				}
			}
			break;
		}
		case Operation.Enter:
			PlayersInsideVents[playerId] = ventId;
			break;
		case Operation.Exit:
			PlayersInsideVents.Remove(playerId);
			break;
		case Operation.Move:
			PlayersInsideVents[playerId] = ventId;
			break;
		case Operation.BootImpostors:
			BootImpostorsFromVent(ventId);
			break;
		}
		IsDirty = true;
		UpdateVentArrows();
	}

	public void MarkClean()
	{
		IsDirty = false;
	}

	public void Serialize(MessageWriter writer, bool initialState)
	{
		writer.WritePacked(PlayersCleaningVents.Count);
		foreach (KeyValuePair<byte, byte> playersCleaningVent in PlayersCleaningVents)
		{
			writer.Write(playersCleaningVent.Key);
			writer.Write(playersCleaningVent.Value);
		}
		writer.WritePacked(PlayersInsideVents.Count);
		foreach (KeyValuePair<byte, byte> playersInsideVent in PlayersInsideVents)
		{
			writer.Write(playersInsideVent.Key);
			writer.Write(playersInsideVent.Value);
		}
	}

	public void Deserialize(MessageReader reader, bool initialState)
	{
		PlayersCleaningVents.Clear();
		int num = reader.ReadPackedInt32();
		for (int i = 0; i < num; i++)
		{
			byte key = reader.ReadByte();
			byte value = reader.ReadByte();
			PlayersCleaningVents[key] = value;
		}
		PlayersInsideVents.Clear();
		int num2 = reader.ReadPackedInt32();
		for (int j = 0; j < num2; j++)
		{
			byte key2 = reader.ReadByte();
			byte value2 = reader.ReadByte();
			PlayersInsideVents[key2] = value2;
		}
		UpdateVentArrows();
	}

	private void BootImpostorsFromVent(int ventId)
	{
		KeyValuePair<byte, byte>[] array = PlayersInsideVents.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			KeyValuePair<byte, byte> keyValuePair = array[i];
			if (keyValuePair.Value == ventId)
			{
				byte key = keyValuePair.Key;
				if (!IsInvisiblePhantomOnVent(key))
				{
					PlayersInsideVents.Remove(key);
					BootImpostorFromVent(ventId, key);
					IsDirty = true;
				}
			}
		}
		if (IsDirty)
		{
			UpdateVentArrows();
		}
	}

	private bool IsInvisiblePhantomOnVent(byte playerId)
	{
		foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
		{
			if (allPlayerControl.PlayerId == playerId)
			{
				if (!(allPlayerControl.Data.Role is PhantomRole { IsInvisible: var isInvisible }))
				{
					return false;
				}
				return isInvisible;
			}
		}
		return false;
	}

	private void BootImpostorFromVent(int ventId, byte playerId)
	{
		foreach (PlayerControl allPlayerControl in PlayerControl.AllPlayerControls)
		{
			if (allPlayerControl.PlayerId == playerId)
			{
				allPlayerControl.MyPhysics.RpcBootFromVent(ventId);
				break;
			}
		}
	}

	private void UpdateVentArrows()
	{
		Vent[] allVents = ShipStatus.Instance.AllVents;
		for (int i = 0; i < allVents.Length; i++)
		{
			allVents[i].UpdateArrows(this);
		}
	}

	public VentilationSystem()
	{ }
}
