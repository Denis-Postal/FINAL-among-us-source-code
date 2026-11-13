using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Hazel;
using UnityEngine;

public class MedScanSystem : ISystemType
{
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public byte playerId;

		internal bool _003CUpdateSystem_003Eb__0(byte v)
		{
			//IL_000f: Expected Obj, but got I4
			object obj = v - playerId;
			return obj == null;
		}
	}

	public const byte Request = 128;

	public const byte Release = 64;

	public const byte NumMask = 31;

	public const byte NoPlayer = byte.MaxValue;

	public List<byte> UsersList= new List<byte>();

	public byte CurrentUser { get; private set; } = byte.MaxValue;

	public bool IsDirty { get; private set; }

	public void Deteriorate(float deltaTime)
	{
		// TODO(verify): rebuilt from the 2019 baseline Detoriorate (current decompile took the empty-queue branch unconditionally); IsDirty replaces the bool return
		if (UsersList.Count > 0)
		{
			if (CurrentUser != UsersList[0])
			{
				if (CurrentUser != byte.MaxValue)
				{
					Debug.Log("Released scanner from: " + CurrentUser);
				}
				CurrentUser = UsersList[0];
				Debug.Log("Acquired scanner for: " + CurrentUser);
				IsDirty = true;
			}
		}
		else if (CurrentUser != byte.MaxValue)
		{
			Debug.Log("Released scanner from: " + CurrentUser);
			CurrentUser = byte.MaxValue;
			IsDirty = true;
		}
	}

	public unsafe void UpdateSystem(PlayerControl player, MessageReader msgReader)
	{
		// from the 2019 RepairDamage; the decompiled RemoveAll predicate (`v - playerId == null`) never matched
		byte b = msgReader.ReadByte();
		byte playerId = (byte)(b & 0x1F);
		if ((b & 0x80) != 0)
		{
			if (!UsersList.Contains(playerId))
			{
				Debug.Log("Added to queue: " + playerId);
				UsersList.Add(playerId);
			}
		}
		else if ((b & 0x40) != 0)
		{
			Debug.Log("Removed from queue: " + playerId);
			UsersList.RemoveAll((byte v) => v == playerId);
		}
		IsDirty = true;
	}

	public void MarkClean()
	{
		IsDirty = false;
	}

	public void Serialize(MessageWriter writer, bool initialState)
	{
		writer.WritePacked(UsersList.Count);
		for (int i = 0; i < UsersList.Count; i++)
		{
			writer.Write(UsersList[i]);
		}
	}

	public void Deserialize(MessageReader reader, bool initialState)
	{ /* spliced-from-2019 */
		UsersList.Clear();
		int num = reader.ReadPackedInt32();
		for (int i = 0; i < num; i++)
		{
			UsersList.Add(reader.ReadByte());
		}
	}

	public MedScanSystem()
	{ }
}
