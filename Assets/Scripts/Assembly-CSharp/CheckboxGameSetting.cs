using UnityEngine;
using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;

public class CheckboxGameSetting : BaseGameSetting
{
	public BoolOptionNames OptionName;

	public override string GetValueString(float value)
	{
		return DestroyableSingleton<TranslationController>.Instance.GetString((value == 1f) ? StringNames.SettingsOn : StringNames.SettingsOff);
	}
}
