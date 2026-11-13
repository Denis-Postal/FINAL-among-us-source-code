using System;
using System.Collections.Generic;

[Serializable]
public struct RulesCategory
{
	public StringNames CategoryName;

	public List<BaseGameSetting> AllGameSettings;
}
