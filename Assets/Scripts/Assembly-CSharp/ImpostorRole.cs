using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

public class ImpostorRole : RoleBehaviour
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<PlayerTask, bool> _003C_003E9__2_0;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CDeinitialize_003Eb__2_0(PlayerTask t)
		{
			return t.name == "ImpostorRole";
		}
	}

	public override bool IsDead => false;

	public override void Deinitialize(PlayerControl targetPlayer)
	{
		PlayerTask playerTask = targetPlayer.myTasks.FirstOrDefault((PlayerTask t) => t.name == "ImpostorRole");
		if ((bool)playerTask)
		{
			targetPlayer.myTasks.Remove(playerTask);
			Object.Destroy(playerTask.gameObject);
		}
	}

	public unsafe override void SpawnTaskHeader(PlayerControl playerControl)
	{
		if (!(playerControl != PlayerControl.LocalPlayer))
		{
			ImportantTextTask orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
			switch (GameOptionsManager.Instance.CurrentGameOptions.GameMode)
			{
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				orCreateTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RuleOneImpostor);
				break;
			case GameModes.Normal:
			case GameModes.NormalFools:
				orCreateTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ImpostorTask) + "\r\n" + Palette.ImpostorRed.ToTextColor() + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FakeTasks) + "</color>";
				break;
			}
		}
	}

	public override bool CanUse(IUsable usable)
	{
		if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
		{
			return false;
		}
		Console console = usable as Console;
		if (console != null)
		{
			return console.AllowImpostor;
		}
		return true;
	}

	public override bool DidWin(GameOverReason gameOverReason)
	{
		return GameManager.Instance.DidImpostorsWin(gameOverReason);
	}

	protected override bool IsValidTarget(NetworkedPlayerInfo target)
	{
		if (base.IsValidTarget(target))
		{
			return target.Role.CanBeKilled;
		}
		return false;
	}

	public override PlayerControl FindClosestTarget()
	{
		List<PlayerControl> playersInAbilityRangeSorted = GetPlayersInAbilityRangeSorted(RoleBehaviour.GetTempPlayerList());
		if (playersInAbilityRangeSorted.Count <= 0)
		{
			return null;
		}
		return playersInAbilityRangeSorted[0];
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
	}

	public ImpostorRole()
	{ }
}
