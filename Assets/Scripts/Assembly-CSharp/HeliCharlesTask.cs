using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using UnityEngine;

public class HeliCharlesTask : SabotageTask
{
	private bool isComplete;

	private ICriticalSabotage sabotage;

	private bool even;

	public override int TaskStep => sabotage.UserCount;

	public override bool IsComplete => isComplete;

	public override void Initialize()
	{
		SetupArrows();
	}

	public void Awake()
	{
		sabotage = (ICriticalSabotage)ShipStatus.Instance.Systems[StartAt];
		DestroyableSingleton<HudManager>.Instance.StartReactorFlash();
	}

	private void FixedUpdate()
	{
		if (!isComplete && !sabotage.IsActive)
		{
			Complete();
		}
	}

	public override bool ValidConsole(Console console)
	{
		return console.TaskTypes.Contains(TaskType);
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
	}

	public unsafe override void AppendTaskText(StringBuilder sb)
	{
		even = !even;
		Color color = (even ? Color.yellow : Color.red);
		sb.Append(color.ToTextColor());
		sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(TaskType));
		sb.Append(" ");
		sb.Append((int)sabotage.Countdown);
		sb.AppendLine($" ({sabotage.UserCount}/{(byte)2})</color>");
		for (int i = 0; i < Arrows.Length; i++)
		{
			Arrows[i].image.color = color;
		}
	}

	public HeliCharlesTask()
	{ }
}
