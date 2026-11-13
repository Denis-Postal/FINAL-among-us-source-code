using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using UnityEngine;

public class NoOxyTask : SabotageTask
{
	private bool isComplete;

	private LifeSuppSystemType reactor;

	private bool even;

	public int targetNumber;

	public override int TaskStep => reactor.UserCount;

	public override bool IsComplete => isComplete;

	public override void Initialize()
	{ /* spliced-from-2019 */
		targetNumber = IntRange.Next(0, 99999);
		ShipStatus instance = ShipStatus.Instance;
		reactor = (LifeSuppSystemType)instance.Systems[SystemTypes.LifeSupp];
		DestroyableSingleton<HudManager>.Instance.StartOxyFlash();
		SetupArrows();
	}

	private void FixedUpdate()
	{ /* spliced-from-2019 */
		if (isComplete)
		{
			return;
		}
		if (!reactor.IsActive)
		{
			Complete();
			return;
		}
		for (int i = 0; i < Arrows.Length; i++)
		{
			Arrows[i].gameObject.SetActive(!reactor.GetConsoleComplete(i));
		}
	}

	public override bool ValidConsole(Console console)
	{ /* spliced-from-2019 */
		if (!reactor.GetConsoleComplete(console.ConsoleId))
		{
			return console.TaskTypes.Contains(TaskTypes.RestoreOxy);
		}
		return false;
	}

	public override void OnRemove()
	{
	}

	public override void Complete()
	{
		isComplete = true;
		PlayerControl.LocalPlayer.RemoveTask(this);
		if (didContribute)
		{
			DataManager.Player.Stats.IncrementStat(StatID.SabotagesFixed);
		}
		DestroyableSingleton<AchievementManager>.Instance.OnSabotageFixed(didContribute);
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageFixed(SystemTypes.LifeSupp);
	}

	public unsafe override void AppendTaskText(StringBuilder sb)
	{
		even = !even;
		Color color = (even ? Color.yellow : Color.red);
		if (reactor != null)
		{
			sb.Append(color.ToTextColor());
			sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(TaskTypes.RestoreOxy));
			sb.Append(" ");
			sb.Append(Mathf.CeilToInt(reactor.Countdown));
			sb.AppendLine($" ({reactor.UserCount}/{(byte)2})</color>");
		}
		else
		{
			sb.AppendLine(color.ToTextColor() + "Oxygen depleting</color>");
		}
		for (int i = 0; i < Arrows.Length; i++)
		{
			try
			{
				Arrows[i].image.color = color;
			}
			catch
			{
			}
		}
	}

	public NoOxyTask()
	{ }
}
