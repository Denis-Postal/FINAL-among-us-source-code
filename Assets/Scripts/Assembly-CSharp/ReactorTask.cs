using System.Linq;
using System.Text;
using AmongUs.Data;
using UnityEngine;

public class ReactorTask : SabotageTask
{
	private bool isComplete;

	private ICriticalSabotage reactor;

	private bool even;

	public override int TaskStep => reactor.UserCount;

	public override bool IsComplete => isComplete;

	public override void Initialize()
	{
		SetupArrows();
	}

	public void Awake()
	{
		reactor = (ICriticalSabotage)ShipStatus.Instance.Systems[StartAt];
		DestroyableSingleton<HudManager>.Instance.StartReactorFlash();
		if (ShipStatus.Instance.AllRooms.FirstOrDefault((PlainShipRoom r) => r.RoomId == StartAt) is IReactorShipRoom reactorShipRoom)
		{
			reactorShipRoom.StartMeltdown();
		}
	}

	private void FixedUpdate()
	{
		if (!isComplete && !reactor.IsActive)
		{
			Complete();
		}
	}

	public override bool ValidConsole(Console console)
	{
		return console.TaskTypes.Contains(TaskType);
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
		DestroyableSingleton<DebugAnalytics>.Instance.Analytics.SabotageFixed(SystemTypes.Reactor);
		DestroyableSingleton<AchievementManager>.Instance.OnSabotageFixed(didContribute);
	}

	public override void AppendTaskText(StringBuilder sb)
	{
		even = !even;
		Color color = (even ? Color.yellow : Color.red);
		sb.Append(color.ToTextColor());
		sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(TaskType));
		sb.Append(" ");
		sb.Append((int)reactor.Countdown);
		sb.AppendLine($" ({reactor.UserCount}/{(byte)2})</color>");
		for (int i = 0; i < Arrows.Length; i++)
		{
			Arrows[i].image.color = color;
		}
	}
}
