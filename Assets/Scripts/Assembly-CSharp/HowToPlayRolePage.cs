using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using UnityEngine;

[Serializable]
public class HowToPlayRolePage
{
	public RoleTypes role;

	public Sprite roleIcon;

	public List<HowToPlayScenePage> rolePages;
}
