using System;
using UnityEngine;

public class TakePartsMinigame : Minigame
{
	[SerializeField]
	private PassiveButton[] parts;

	[SerializeField]
	private GameObject[] inventoryParts;

	[SerializeField]
	private PassiveButton boxLid;

	[SerializeField]
	private GameObject boxOpen;

	[SerializeField]
	private GameObject hand;

	[Header("Audio")]
	[SerializeField]
	private AudioClip openToolboxSfx;

	[SerializeField]
	private AudioClip[] pickUpSfx;

	private readonly Controller controller = new Controller();

	private int partsTaken;

	public override bool SkipMultistageOverlayMenuSetup => true;

	private void Update()
	{
		controller.Update();
		hand.transform.position = new Vector3(VirtualCursor.currentPosition.x, VirtualCursor.currentPosition.y, hand.transform.position.z);
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: false);
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
		UpdateCursorVisibility();
		PassiveButton[] array = parts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		SpriteRenderer[] componentsInChildren = hand.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer playerMaterialColors in componentsInChildren)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
		partsTaken = 0;
		boxLid.OnClick.AddListener(OnToolboxOpen);
	}

	public override void Close()
	{
		base.Close();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
	}

	private void OnToolboxOpen()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(openToolboxSfx, loop: false);
		}
		VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		boxLid.gameObject.SetActive(value: false);
		boxOpen.gameObject.SetActive(value: true);
		PassiveButton[] array = parts;
		foreach (PassiveButton part in array)
		{
			part.enabled = true;
			part.OnClick.AddListener(() =>
			{
				int num = parts.IndexOf(part);
				inventoryParts[num].SetActive(value: true);
				SavePart();
				part.gameObject.SetActive(value: false);
			});
		}
	}

	private void SavePart()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(pickUpSfx.Random(), loop: false);
		}
		VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
		partsTaken++;
		if (partsTaken >= parts.Length)
		{
			MyNormTask.NextStep();
			transform.parent.GetComponent<MultistageMinigame>().Close();
		}
	}

	private void OnInputMethodChanged()
	{
		UpdateCursorVisibility();
	}

	private void UpdateCursorVisibility()
	{
		hand.gameObject.SetActive(!MyNormTask.IsComplete && ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
	}
}
