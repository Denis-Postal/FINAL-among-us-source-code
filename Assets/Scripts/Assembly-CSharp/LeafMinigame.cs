using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using PowerTools;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class LeafMinigame : Minigame
{
	public LeafBehaviour LeafPrefab;

	public Vector2Range ValidArea;

	public SpriteAnim[] Arrows;

	public AnimationClip[] Inactive;

	public AnimationClip[] Active;

	public AnimationClip[] Complete;

	private Collider2D[] Leaves;

	public AudioClip[] LeaveSounds;

	public AudioClip[] SuckSounds;

	private Controller myController= new Controller();

	public Transform interactionCursor;

	public Transform interactionCursorCenterDot;

	private Collider2D[] overlapResults;

	private bool prevHadOverlaps;

	private bool prevRightStickInput;

	public unsafe override void Begin(PlayerTask task)
{
		base.Begin(task);
		Leaves = new Collider2D[MyNormTask.MaxStep - MyNormTask.taskStep];
		for (int i = 0; i < Leaves.Length; i++)
		{
			LeafBehaviour leafBehaviour = Object.Instantiate(LeafPrefab);
			leafBehaviour.transform.SetParent(transform);
			leafBehaviour.Parent = this;
			Vector3 localPosition = ValidArea.Next();
			localPosition.z = -1f;
			leafBehaviour.transform.localPosition = localPosition;
			Leaves[i] = leafBehaviour.GetComponent<Collider2D>();
		}
		overlapResults = new Collider2D[Leaves.Length * 3];
		SetupInput(disableCursor: true);
	}

	public unsafe void FixedUpdate()
{
		myController.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			Vector2 vector = new Vector2(player.GetAxis(13), player.GetAxis(14));
			Vector2 vector2 = new Vector2(player.GetAxis(16), player.GetAxis(17));
			if (vector2.sqrMagnitude > 1f)
			{
				vector2 = vector2.normalized;
			}
			Vector3 localPosition = interactionCursor.localPosition;
			Vector3 localPosition2 = interactionCursorCenterDot.localPosition;
			localPosition.x += vector.x * Time.deltaTime * 5f;
			localPosition.y += vector.y * Time.deltaTime * 5f;
			localPosition.x = Mathf.Clamp(localPosition.x, ValidArea.min.x, ValidArea.max.x);
			localPosition.y = Mathf.Clamp(localPosition.y, ValidArea.min.y, ValidArea.max.y);
			localPosition2.x = vector2.x;
			localPosition2.y = vector2.y;
			interactionCursor.localPosition = localPosition;
			interactionCursorCenterDot.localPosition = localPosition2;
			int num = Physics2D.OverlapCircleNonAlloc(interactionCursor.position, interactionCursor.transform.localScale.x, overlapResults, LayerMask.GetMask("UI"));
			int num2 = 0;
			bool flag = vector2.sqrMagnitude > 0.5f;
			bool flag2 = flag && !prevRightStickInput;
			for (int i = 0; i < num; i++)
			{
				LeafBehaviour component = overlapResults[i].GetComponent<LeafBehaviour>();
				if (!component)
				{
					continue;
				}
				num2++;
				if (flag2)
				{
					flag2 = false;
					component.body.linearVelocity = vector2.normalized * Mathf.Min(3f, vector2.magnitude * 3f);
					VibrationManager.Vibrate(0.15f, 0.15f, 0.15f);
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(LeaveSounds.Random(), loop: false);
					}
				}
			}
			prevRightStickInput = flag;
			if (num2 > 0)
			{
				if (!prevHadOverlaps)
				{
					for (int j = 0; j < Arrows.Length; j++)
					{
						Arrows[j].Play(Active[j]);
					}
				}
				prevHadOverlaps = true;
				return;
			}
			if (prevHadOverlaps)
			{
				for (int k = 0; k < Arrows.Length; k++)
				{
					Arrows[k].Play(Inactive[k]);
					Arrows[k].GetComponent<SpriteRenderer>().sprite = null;
				}
			}
			prevHadOverlaps = false;
			return;
		}
		for (int l = 0; l < Leaves.Length; l++)
		{
			Collider2D collider2D = Leaves[l];
			if (!collider2D)
			{
				continue;
			}
			LeafBehaviour component2 = collider2D.GetComponent<LeafBehaviour>();
			switch (myController.CheckDrag(collider2D))
			{
			case DragState.TouchStart:
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(LeaveSounds.Random(), loop: false);
				}
				for (int n = 0; n < Arrows.Length; n++)
				{
					Arrows[n].Play(Active[n]);
				}
				component2.Held = true;
				break;
			}
			case DragState.Dragging:
			{
				Vector2 vector3 = myController.DragPosition - component2.body.position;
				component2.body.linearVelocity = vector3.normalized * Mathf.Min(3f, vector3.magnitude * 3f);
				break;
			}
			case DragState.Released:
			{
				component2.Held = false;
				for (int m = 0; m < Arrows.Length; m++)
				{
					Arrows[m].Play(Inactive[m]);
					Arrows[m].GetComponent<SpriteRenderer>().sprite = null;
				}
				break;
			}
			}
		}
	}

	public void LeafDone(LeafBehaviour leaf)
{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(SuckSounds.Random(), loop: false);
		}
		VibrationManager.Vibrate(0.15f, 0f, 0.25f);
		Object.Destroy(leaf.gameObject);
		if (!MyNormTask)
		{
			return;
		}
		MyNormTask.NextStep();
		if (MyNormTask.IsComplete)
		{
			for (int i = 0; i < Arrows.Length; i++)
			{
				Arrows[i].Play(Complete[i]);
			}
			StartCoroutine(CoStartClose());
		}
	}

	public LeafMinigame()
	{ }
}
