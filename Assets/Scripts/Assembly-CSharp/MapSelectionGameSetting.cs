using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using AmongUs.GameOptions;
using Cpp2ILInjected;

public class MapSelectionGameSetting : BaseGameSetting
{
	public ByteOptionNames OptionName;

	public StringNames[] Values;

	public unsafe void TryGetInt(IGameOptions gameOptions, out int value)
	{
		gameOptions.TryGetByte(OptionName, out var value2);
		value = ((value2 > 3) ? (value2 - 1) : value2);
	}

	public unsafe override string GetValueString(float value)
	{
		return DestroyableSingleton<TranslationController>.Instance.GetString(Values[(int)value]);
	}
}
