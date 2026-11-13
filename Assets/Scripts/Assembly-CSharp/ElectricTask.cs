using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using UnityEngine;

public class ElectricTask : SabotageTask
{
	private bool isComplete;

	private SwitchSystem system;

	private bool even;

	public override int TaskStep
	{
		get
		{
			if (!isComplete)
			{
				return 0;
			}
			return 1;
		}
	}

	public override bool IsComplete => isComplete;

	public override void Initialize()
	{ /* spliced-from-2019 */
		ShipStatus instance = ShipStatus.Instance;
		system = (SwitchSystem)instance.Systems[SystemTypes.Electrical];
		SetupArrows();
	}

	private void FixedUpdate()
	{ /* spliced-from-2019 */
		if (!isComplete && system.ExpectedSwitches == system.ActualSwitches)
		{
			Complete();
		}
	}

	public override bool ValidConsole(Console console)
	{ /* spliced-from-2019 */
		return console.TaskTypes.Contains(TaskTypes.FixLights);
	}

	public override void Complete()
	{
		isComplete = true;
		PlayerControl.LocalPlayer.RemoveTask(this);
		if (didContribute)
		{
			DataManager.Player.Stats.IncrementStat(StatID.SabotagesFixed);
		}
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageFixed(SystemTypes.Electrical);
		DestroyableSingleton<AchievementManager>.Instance.OnSabotageFixed(didContribute);
	}

	public unsafe override void AppendTaskText(StringBuilder sb)
	{
		even = !even;
		Color color = (even ? Color.yellow : Color.red);
		sb.Append(color.ToTextColor());
		sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(TaskTypes.FixLights));
		sb.AppendLine(" (%" + (int)(system.Level * 100f) + ")</color>");
		for (int i = 0; i < Arrows.Length; i++)
		{
			if (Arrows[i].isActiveAndEnabled)
			{
				Arrows[i].image.color = color;
			}
		}
	}

	public ElectricTask()
	{ }
}
