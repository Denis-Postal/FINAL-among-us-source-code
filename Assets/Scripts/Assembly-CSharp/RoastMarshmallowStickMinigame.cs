using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoastMarshmallowStickMinigame : Minigame
{
	private enum State
	{
		Ready,
		Ending,
		Ended
	}

	[SerializeField]
	[Header("Stick Minigame")]
	private BoxCollider2D[] spawnRegions;

	[SerializeField]
	private Transform stickTransform;

	[SerializeField]
	private GameObject stickSprite;

	[SerializeField]
	private ParticleSystem stickParticles;

	[SerializeField]
	private Transform[] otherRandomObjects;

	[SerializeField]
	private PassiveButton stickButton;

	[SerializeField]
	private Transform hand;

	private State state;

	public override bool SkipMultistageOverlayMenuSetup => true;

	private void Update()
	{
		hand.position = VirtualCursor.currentPosition;
		hand.SetLocalZ(0.5f);
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
		state = State.Ready;
		stickButton.OnClick.AddListener(OnStickPressed);
		List<BoxCollider2D> list = new List<BoxCollider2D>(spawnRegions);
		list.Shuffle();
		stickTransform.position = new Vector3(UnityEngine.Random.Range(list[0].bounds.min.x, list[0].bounds.max.x), UnityEngine.Random.Range(list[0].bounds.min.y, list[0].bounds.max.y), stickTransform.position.z);
		list.RemoveAt(0);
		for (int j = 0; j < otherRandomObjects.Length; j++)
		{
			BoxCollider2D boxCollider2D = ((j >= list.Count) ? spawnRegions.Random() : list[j]);
			Transform transform = otherRandomObjects[j];
			transform.position = new Vector3(UnityEngine.Random.Range(boxCollider2D.bounds.min.x, boxCollider2D.bounds.max.x), UnityEngine.Random.Range(boxCollider2D.bounds.min.y, boxCollider2D.bounds.max.y), transform.position.z);
		}
	}

	public override void Close()
	{
		base.Close();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(OnInputMethodChanged));
	}

	private IEnumerator CoComplete()
	{
		state = State.Ending;
		stickParticles.Play();
		stickSprite.SetActive(value: false);
		UpdateCursorVisibility();
		yield return new WaitForSeconds(1f);
		MyNormTask.NextStep();
		transform.parent.GetComponent<MultistageMinigame>().Close();
		state = State.Ended;
	}

	private void OnStickPressed()
	{
		if (state == State.Ready)
		{
			VibrationManager.Vibrate(0.3f, 0.3f, 0.1f);
			StartCoroutine(CoComplete());
		}
	}

	private void OnInputMethodChanged()
	{
		UpdateCursorVisibility();
	}

	private void UpdateCursorVisibility()
	{
		hand.gameObject.SetActive(state == State.Ready && ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
	}
}
