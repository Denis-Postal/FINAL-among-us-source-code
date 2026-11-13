using System.Collections.Generic;
using AmongUs.GameOptions;
using UnityEngine;

public class GameSettingsCategoryList : ScriptableObject
{
	public StringNames GameModeName;

	public Sprite StandardRulesImage;

	public Sprite AlternateRulesImage;

	public StringNames StandardRulesName;

	public StringNames AlternateRulesName;

	public StringNames StandardPresetDescription;

	public StringNames AlternatePresetDescription;

	public RulesPresets AlternateRulesType;

	public BaseGameSetting MapNameSetting;

	public List<BaseGameSetting> OverviewSettings;

	public List<RulesCategory> AllCategories;
}
