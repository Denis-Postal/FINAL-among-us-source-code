using System;
using System.Collections;
using UnityEngine;

public class FindCritterEggMinigame : Minigame
{
	[Header("Egg")]
	[SerializeField]
	private Transform[] spawnPoints;

	[SerializeField]
	private Transform egg;

	[SerializeField]
	private PassiveButton eggButton;

	[SerializeField]
	[Header("Leaves")]
	private EggCoveringLeaf[] leaves;

	[SerializeField]
	[Header("Cursor")]
	private Transform hand;

	[Header("Audio")]
	[SerializeField]
	private AudioClip eggGetSfx;

	[SerializeField]
	[Range(0f, 1f)]
	[Space(10f)]
	private float leafSfxPitchRange = 0.2f;

	[SerializeField]
	private AudioClip[] leafSfx;

	private Controller controller = new Controller();

	private bool completed;

	public override bool SkipMultistageOverlayMenuSetup => true;

	private void Update()
	{
		controller.Update();
		hand.position = VirtualCursor.currentPosition;
		hand.SetLocalZ(0f);
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: false);
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
		UpdateCursorVisibility();
		SpriteRenderer[] componentsInChildren = hand.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer playerMaterialColors in componentsInChildren)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
		egg.position = spawnPoints.Random().position;
		eggButton.OnClick.AddListener(GetEgg);
		EggCoveringLeaf[] array = leaves;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].OnCollectedEvent += OnLeafCollected;
		}
	}

	public override void Close()
	{
		base.Close();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
	}

	private void OnInputMethodChanged()
	{
		UpdateCursorVisibility();
	}

	private void UpdateCursorVisibility()
	{
		hand.gameObject.SetActive(!completed && ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
	}

	private void GetEgg()
	{
		if (!completed)
		{
			StartCoroutine(CoGetEgg());
		}
	}

	private IEnumerator CoGetEgg()
	{
		completed = true;
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(eggGetSfx, loop: false);
		}
		UpdateCursorVisibility();
		eggButton.SetButtonEnableState(enabled: false);
		MyNormTask.NextStep();
		VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		yield return Effects.Bounce(egg, 0.75f);
		yield return Effects.Wait(0.25f);
		transform.parent.GetComponent<MultistageMinigame>().Close();
	}

	private void OnLeafCollected()
	{
		VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(leafSfx.Random(), loop: false, 1f, UnityEngine.Random.Range(1f - leafSfxPitchRange, 1f + leafSfxPitchRange));
		}
	}
}
