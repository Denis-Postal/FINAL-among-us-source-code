using System;
using System.Collections.Generic;
using System.Text;
using Cpp2ILInjected;
using UnityEngine;

public class MushroomMixupSabotageTask : PlayerTask
{
	private bool isComplete;

	private MushroomMixupSabotageSystem sabotage;

	public override int TaskStep
	{
		get
		{
			bool flag = IsComplete;
			bool flag2 = !flag;
			return (!flag2) ? 1 : 0;
		}
	}

	public override bool IsComplete => isComplete;

	public override void Initialize()
	{
		sabotage = ShipStatus.Instance.Systems[SystemTypes.MushroomMixupSabotage] as MushroomMixupSabotageSystem;
	}

	public override bool ValidConsole(Console console)
	{
		return false;
	}

	public override void Complete()
	{
		PlayerControl.LocalPlayer.RemoveTask(this);
		isComplete = true;
	}

	// TODO(verify): text colour not recoverable from decompile (red assumed)
	public unsafe override void AppendTaskText(StringBuilder sb)
	{
		string text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MushroomMixupSabotage, new object[1] { (int)sabotage.CurrentSecondsUntilHeal });
		sb.AppendLine(Extensions.ToTextColor(Color.red) + text + "</color>");
	}
}
