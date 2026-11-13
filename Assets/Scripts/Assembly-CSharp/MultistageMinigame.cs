using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class MultistageMinigame : Minigame
{
	public Minigame[] Stages;

	private Minigame stage;

	private bool hasOverlayMenu;

	public UiElement BackButton;

	public List<UiElement> ControllerSelectable;

	// Stage selection recovered from decomp9 (offsets: Data[1] for Fuel/ExtractFuel (byte[] element +0x21, same as the 2019 RefuelMinigame Stages[Data[1]]),
	// taskStep (+4C) / MaxStep (+50) for CatchFish; every other multistage task uses Stages[taskStep]).
	public override void Begin(PlayerTask task)
	{
		NormalPlayerTask normalPlayerTask = task as NormalPlayerTask;
		if (normalPlayerTask.TaskType == TaskTypes.FuelEngines || normalPlayerTask.TaskType == TaskTypes.ExtractFuel)
		{
			stage = Stages[normalPlayerTask.Data[1]];
		}
		else if (normalPlayerTask.TaskType == TaskTypes.CatchFish)
		{
			if (normalPlayerTask.TaskStep == 0 || normalPlayerTask.MaxStep == 3)
			{
				stage = Stages[0];
			}
			else
			{
				stage = Stages[normalPlayerTask.taskStep];
			}
		}
		else
		{
			stage = Stages[normalPlayerTask.taskStep];
		}
		stage.gameObject.SetActive(value: true);
		stage.Begin(task);
		Minigame.Instance = this;
		UiElement defaultSelection = null;
		foreach (UiElement item in ControllerSelectable)
		{
			if (item.isActiveAndEnabled)
			{
				defaultSelection = item;
				break;
			}
		}
		if (!stage.SkipMultistageOverlayMenuSetup)
		{
			hasOverlayMenu = true;
			ControllerManager.Instance.OpenOverlayMenu(gameObject.name, BackButton, defaultSelection, ControllerSelectable);
		}
	}

	public override void Close()
	{
		Minigame.Instance = null;
		stage.Close();
		base.Close();
		if (hasOverlayMenu)
		{
			hasOverlayMenu = false;
			ControllerManager.Instance.CloseOverlayMenu(gameObject.name);
		}
	}
}
