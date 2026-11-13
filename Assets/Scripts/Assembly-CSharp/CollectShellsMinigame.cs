using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class CollectShellsMinigame : Minigame
{
	[SerializeField]
	private CollectableShell[] shellPrefabs;

	[SerializeField]
	private Transform[] spawnPositions;

	[SerializeField]
	private IntRange numShellsRange;

	[SerializeField]
	private GameObject controllerCursor;

	[SerializeField]
	private GameObject handWipeObject;

	[SerializeField]
	private AudioClip[] dustSounds;

	private List<CollectableShell> shells= new List<CollectableShell>();

	private Controller controller= new Controller();

	private Vector2 prevMousePos;

	private Vector2 prevControllerStickPos;

	private float wipeDistance= 0.5f;

	private float maxProgress= 2f;

	private float controllerDifficulty= 0.5f;

	private float mouseDifficulty= 1f;

	public unsafe override void Begin(PlayerTask task)
{
		base.Begin(task);
		SetupInput(disableCursor: true);
		SpriteRenderer[] componentsInChildren = controllerCursor.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer playerMaterialColors in componentsInChildren)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
		if (MyNormTask.MaxStep == 1)
		{
			MyNormTask.MaxStep = numShellsRange.Next();
			MyNormTask.ShowTaskStep = true;
		}
		int num = MyNormTask.MaxStep - MyNormTask.TaskStep;
		List<Transform> list = new List<Transform>(spawnPositions);
		for (int j = 0; j < num; j++)
		{
			CollectableShell original = shellPrefabs.Random();
			Transform transform = list.Random();
			list.Remove(transform);
			CollectableShell collectableShell = Object.Instantiate(original, transform, worldPositionStays: false);
			collectableShell.transform.localPosition = Vector3.zero;
			collectableShell.SetOnSandClearedAction(OnSandCleared);
			shells.Add(collectableShell);
		}
	}

	public void Update()
	{
		if (amClosing == CloseState.None)
		{
			controller.Update();
			if (Controller.currentTouchType == Controller.TouchType.Joystick)
			{
				UpdateController();
			}
			else
			{
				UpdateTouch();
			}
		}
	}

	private unsafe void UpdateTouch()
{
		controllerCursor.gameObject.SetActive(value: false);
		Controller controller = DestroyableSingleton<PassiveButtonManager>.Instance.controller;
		if (!controller.AnyTouch)
		{
			return;
		}
		Vector2 position = controller.Touches[0].Position;
		foreach (CollectableShell shell in shells)
		{
			if (!shell.isActiveAndEnabled)
			{
				continue;
			}
			for (int i = 0; i < shell.SandButtons.Count; i++)
			{
				PassiveButton passiveButton = shell.SandButtons[i];
				if (passiveButton.isActiveAndEnabled && passiveButton.Colliders[0].OverlapPoint(position))
				{
					float num = Mathf.Abs(prevMousePos.x - position.x) + Mathf.Abs(prevMousePos.y - position.y);
					num *= mouseDifficulty;
					SpriteRenderer sandSprite = shell.GetSandSprite(i);
					WipeSand(passiveButton, sandSprite, num);
					prevMousePos = position;
				}
			}
			shell.CheckIfAllSandCleared();
		}
	}

	private unsafe void UpdateController()
{
		controllerCursor.gameObject.SetActive(value: true);
		Player player = ReInput.players.GetPlayer(0);
		Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
		Vector2 axis2DRaw2 = player.GetAxis2DRaw(16, 17);
		Vector3 localPosition = controllerCursor.transform.localPosition;
		Vector3 localPosition2 = handWipeObject.transform.localPosition;
		localPosition.x += axis2DRaw.x * Time.deltaTime * 3f;
		localPosition.y += axis2DRaw.y * Time.deltaTime * 3f;
		localPosition2.x = axis2DRaw2.x * wipeDistance;
		localPosition2.y = axis2DRaw2.y * wipeDistance;
		controllerCursor.transform.localPosition = localPosition;
		handWipeObject.transform.localPosition = localPosition2;
		float num = Mathf.Abs(prevControllerStickPos.x - axis2DRaw2.x) + Mathf.Abs(prevControllerStickPos.y - axis2DRaw2.y);
		num *= controllerDifficulty;
		prevControllerStickPos = axis2DRaw2;
		foreach (CollectableShell shell in shells)
		{
			if (!shell.isActiveAndEnabled)
			{
				continue;
			}
			for (int i = 0; i < shell.SandButtons.Count; i++)
			{
				PassiveButton passiveButton = shell.SandButtons[i];
				if (passiveButton.isActiveAndEnabled && passiveButton.Colliders[0].OverlapPoint(controllerCursor.transform.position))
				{
					SpriteRenderer sandSprite = shell.GetSandSprite(i);
					WipeSand(passiveButton, sandSprite, num);
				}
			}
			shell.CheckIfAllSandCleared();
		}
	}

	private unsafe void WipeSand(PassiveButton sand, SpriteRenderer sprite, float diff)
{
		diff = Mathf.Min(diff, Time.deltaTime * maxProgress);
		if (!(diff <= 0f))
		{
			VibrationManager.Vibrate(0.3f, 0.3f);
			Color color = sprite.color;
			color.a -= diff;
			sprite.color = color;
			if (color.a <= 0.05f)
			{
				sand.gameObject.SetActive(value: false);
			}
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(dustSounds.Random(), loop: false);
			}
		}
	}

	private void OnSandCleared()
	{
		MyNormTask.NextStep();
		VibrationManager.Vibrate(0.3f, 0.3f);
		if (MyNormTask.IsComplete)
		{
			StartCoroutine(CoStartClose());
		}
	}

	public CollectShellsMinigame()
	{ }
}
