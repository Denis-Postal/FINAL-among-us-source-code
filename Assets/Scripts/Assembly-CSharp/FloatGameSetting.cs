using UnityEngine;
using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;

public class FloatGameSetting : BaseGameSetting
{
	public FloatOptionNames OptionName;

	public float Value;

	public float Increment;

	public FloatRange ValidRange;

	public bool ZeroIsInfinity;

	public NumberSuffixes SuffixType;

	public string FormatString;

	public override string GetValueString(float value)
	{
		string empty = string.Empty;
		if (ZeroIsInfinity && Mathf.Abs(value) < 0.0001f)
		{
			return "<b>∞</b>";
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
