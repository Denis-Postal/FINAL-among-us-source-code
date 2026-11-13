using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using UnityEngine;

public class HudOverrideTask : SabotageTask, IHudOverrideTask
{
	private bool isComplete;

	private HudOverrideSystemType system;

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
		system = instance.Systems[SystemTypes.Comms] as HudOverrideSystemType;
		SetupArrows();
	}

	private void FixedUpdate()
	{ /* spliced-from-2019 */
		if (!isComplete && !system.IsActive)
		{
			Complete();
		}
	}

	public override bool ValidConsole(Console console)
	{ /* spliced-from-2019 */
		return console.TaskTypes.Contains(TaskTypes.FixComms);
	}

	public override void Complete()
	{
		isComplete = true;
		PlayerControl.LocalPlayer.RemoveTask(this);
		if (didContribute)
		{
			DataManager.Player.Stats.IncrementStat(StatID.SabotagesFixed);
		}
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageFixed(SystemTypes.Comms);
		DestroyableSingleton<AchievementManager>.Instance.OnSabotageFixed(didContribute);
	}

	public unsafe override void AppendTaskText(StringBuilder sb)
	{
		even = !even;
		Color color = (even ? Color.yellow : Color.red);
		sb.Append(color.ToTextColor());
		sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(TaskTypes.FixComms));
		sb.Append("</color>");
		for (int i = 0; i < Arrows.Length; i++)
		{
			Arrows[i].image.color = color;
		}
	}

	public HudOverrideTask()
	{ }
}
