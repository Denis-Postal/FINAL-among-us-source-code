using System;
using System.Collections.Generic;
using Assets.CoreScripts;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class AutoTaskConsole : Console
{
	public AudioClip useSound;

	private PlayerTask FindTask(PlayerControl pc)
	{
		for (int i = 0; i < pc.myTasks.Count; i++)
		{
			PlayerTask playerTask = pc.myTasks[i];
			if (!playerTask.IsComplete && playerTask.ValidConsole(this))
			{
				return playerTask;
			}
		}
		return null;
	}

	public override void Use()
	{
		CanUse(PlayerControl.LocalPlayer.Data, out var canUse, out var _);
		if (canUse)
		{
			PlayerControl localPlayer = PlayerControl.LocalPlayer;
			NormalPlayerTask normalPlayerTask = (NormalPlayerTask)FindTask(localPlayer);
			AfterUse(normalPlayerTask);
			DestroyableSingleton<UnityTelemetry>.Instance.WriteUse(localPlayer.PlayerId, normalPlayerTask.TaskType, transform.position);
		}
	}

	// TODO(verify): the colour applied after use is assumed to be Palette.DisabledGrey (the decompile lost the Color).
	protected virtual void AfterUse(NormalPlayerTask task)
	{
		if ((bool)useSound && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(useSound, loop: false);
		}
		task.NextStep();
		Image.color = Palette.HalfWhite;
	}

	public AutoTaskConsole()
	{ }
}
