using UnityEngine;
using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;

public class IntGameSetting : BaseGameSetting
{
	public Int32OptionNames OptionName;

	public int Value;

	public int Increment;

	public IntRange ValidRange;

	public bool ZeroIsInfinity;

	public NumberSuffixes SuffixType;

	public string FormatString;

	public override string GetValueString(float value)
	{
		string empty = string.Empty;
		if (ZeroIsInfinity && Mathf.Abs(value) < 0.0001f)
		{
			return "∞";
		}
		if (SuffixType == NumberSuffixes.None)
		{
			return value.ToString(FormatString);
		}
		if (SuffixType == NumberSuffixes.Multiplier)
		{
			return value.ToString(FormatString) + "x";
		}
		return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameSecondsAbbrev, value.ToString(FormatString));
	}
}
