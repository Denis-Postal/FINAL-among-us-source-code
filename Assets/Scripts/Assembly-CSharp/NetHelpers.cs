using System;
using Cpp2ILInjected;
using Hazel;
using UnityEngine;

public static class NetHelpers
{
	private static readonly FloatRange XRange= new FloatRange(-50f, 50f);

	private static readonly FloatRange YRange= new FloatRange(-50f, 50f);

	// TODO(verify): sequence-id wraparound compare reconstructed from decomp9 (damaged); semantics: newSid is "newer" within half the range
	public static bool SidGreaterThan(ushort newSid, ushort prevSid)
	{
		ushort num = (ushort)(prevSid + 32767);
		if (prevSid < num)
		{
			if (newSid > prevSid)
			{
				return newSid <= num;
			}
			return false;
		}
		if (newSid <= prevSid)
		{
			return newSid <= num;
		}
		return true;
	}

	public static bool SidGreaterThan(byte newSid, byte prevSid)
	{
		byte b = (byte)(prevSid + 127);
		if (prevSid < b)
		{
			if (newSid > prevSid)
			{
				return newSid <= b;
			}
			return false;
		}
		if (newSid <= prevSid)
		{
			return newSid <= b;
		}
		return true;
	}

	// wire format: u16 x, u16 y, each quantised over [-50, 50] (cvttss2si truncation)
	public static void WriteVector2(Vector2 vec, MessageWriter writer)
	{
		ushort value = (ushort)(XRange.ReverseLerp(vec.x) * 65535f);
		ushort value2 = (ushort)(YRange.ReverseLerp(vec.y) * 65535f);
		writer.Write(value);
		writer.Write(value2);
	}

	public static Vector2 ReadVector2(MessageReader reader)
	{
		float v = (float)(int)reader.ReadUInt16() / 65535f;
		float v2 = (float)(int)reader.ReadUInt16() / 65535f;
		return new Vector2(XRange.Lerp(v), YRange.Lerp(v2));
	}

	static NetHelpers()
	{ }
}
