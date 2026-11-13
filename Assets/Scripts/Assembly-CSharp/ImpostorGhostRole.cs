using Object = UnityEngine.Object;
using System;
using System.Text;
using Cpp2ILInjected;
using UnityEngine;

public class ImpostorGhostRole : RoleBehaviour
{
	public Minigame HauntMenu;

	public override bool IsDead => true;

	public bool WasManuallyPicked { get; set; }

	public override bool IsAffectedByComms => false;

	public override bool CanUse(IUsable usable)
	{
		if (usable is Vent)
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

	public override void UseAbility()
	{
		if (DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening)
		{
			return;
		}
		if ((bool)Minigame.Instance)
		{
			if (Minigame.Instance is HauntMenuMinigame)
			{
				Minigame.Instance.Close();
			}
		}
		else
		{
			Minigame minigame = Object.Instantiate(HauntMenu);
			minigame.transform.SetParent(DestroyableSingleton<HudManager>.Instance.AbilityButton.transform, worldPositionStays: false);
			minigame.transform.SetLocalZ(-5f);
			minigame.Begin(null);
			DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
		}
	}

	public override void SpawnTaskHeader(PlayerControl playerControl)
	{
		playerControl.ClearTasks();
		PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl).Text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GhostImpostor);
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
	}

	public ImpostorGhostRole()
	{ }
}
