using System;
using UnityEngine;

[Serializable]
public abstract class BaseGameSetting : ScriptableObject
{
	public StringNames Title;

	public OptionTypes Type;

	public abstract string GetValueString(float value);
}
