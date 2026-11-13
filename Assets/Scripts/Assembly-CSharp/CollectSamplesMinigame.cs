using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectSamplesMinigame : Minigame
{
	private const int SAMPLE_COUNT = 3;

	[SerializeField]
	private Transform[] spawnPoints;

	[SerializeField]
	private CollectSamplesMinigameCollectableSample[] collectableSamples;

	[SerializeField]
	private Transform hand;

	private Controller controller = new Controller();

	private bool IsComplete => SamplesCollected == 3;

	public override bool SkipMultistageOverlayMenuSetup => true;

	private int SamplesCollected
	{
		get
		{
			return MyNormTask.Data[1];
		}
		set
		{
			MyNormTask.Data[1] = (byte)value;
		}
	}

	private void Update()
	{
		controller.Update();
		hand.position = VirtualCursor.currentPosition;
		hand.SetLocalZ(-0.5f);
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: false);
		InitializeSaveState();
		SpriteRenderer[] componentsInChildren = hand.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer playerMaterialColors in componentsInChildren)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
		UpdateCursorVisibility();
		List<int> list = new List<int>(Enumerable.Range(0, spawnPoints.Length));
		List<int> list2 = new List<int>(Enumerable.Range(0, collectableSamples.Length));
		for (int j = 0; j < 3 - SamplesCollected; j++)
		{
			int num = list2.Random();
			list2.Remove(num);
			CollectSamplesMinigameCollectableSample original = collectableSamples[num];
			int num2 = list.Random();
			list.Remove(num2);
			CollectSamplesMinigameCollectableSample collectable = UnityEngine.Object.Instantiate(original, spawnPoints[num2], worldPositionStays: false);
			collectable.OnPressEvent += (CollectSamplesMinigameCollectableSample x) =>
			{
				StartCoroutine(CoCollectSample(collectable));
			};
		}
	}

	public override void Close()
	{
		base.Close();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
	}

	private void InitializeSaveState()
	{
		if (MyNormTask.Data.Length == 1)
		{
			MyNormTask.Data = new byte[2]
			{
				MyNormTask.Data[0],
				0
			};
		}
	}

	private IEnumerator CoCollectSample(CollectSamplesMinigameCollectableSample collectable)
	{
		if (MyNormTask.taskStep <= 0)
		{
			VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
			yield return Effects.ScaleIn(collectable.transform, 1f, 0f, 0.3f);
			SamplesCollected++;
			if (IsComplete)
			{
				UpdateCursorVisibility();
				MyNormTask.NextStep();
				yield return Effects.Wait(0.5f);
				transform.parent.GetComponent<MultistageMinigame>().Close();
			}
		}
	}

	private void UpdateCursorVisibility()
	{
		hand.gameObject.SetActive(!IsComplete && ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
	}

	private void OnInputMethodChanged()
	{
		UpdateCursorVisibility();
	}
}
