using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using AmongUs.GameOptions;
using Cpp2ILInjected;

public class StringGameSetting : BaseGameSetting
{
	public Int32OptionNames OptionName;

	public StringNames[] Values;

	public int Index;

	public unsafe override string GetValueString(float value)
	{
		return DestroyableSingleton<TranslationController>.Instance.GetString(Values[(int)value]);
	}
}
