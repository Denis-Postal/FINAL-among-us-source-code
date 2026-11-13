using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class SpecimenGame : Minigame
{
	public Collider2D[] Specimens;

	public Transform[] Slots;

	private Controller cont= new Controller();

	public AudioClip[] PlaceSounds;

	private SpriteRenderer[] SpecimenSprites;

	public Color highlightColor;

	private bool completed;

	private bool prevHadStick;

	private bool prevHadButton;

	private int prevSelectedSpecimen= -1;

	private int selectedSpecimen;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: true);
		SpecimenSprites = new SpriteRenderer[Specimens.Length];
		for (int i = 0; i < Specimens.Length; i++)
		{
			SpecimenSprites[i] = Specimens[i].GetComponent<SpriteRenderer>();
		}
	}

	private void PlaySnapSound()
	{
		if (Constants.ShouldPlaySfx())
		{
			AudioMixerGroup audioMixer = default;
			AudioSource audioSource = SoundManager.Instance.PlaySound(Extensions.Random(PlaceSounds), loop: false, 1f, audioMixer);
			audioSource.pitch = FloatRange.Next(0.8f, 1.2f);
		}
	}

	// TODO(verify): the controller (joystick) selection/grab path is omitted; only the mouse/touch drag path is restored.
	public unsafe void Update()
	{
		cont.Update();
		if (selectedSpecimen != prevSelectedSpecimen)
		{
			if (prevSelectedSpecimen != -1)
			{
				SpecimenSprites[prevSelectedSpecimen].material.SetFloat("_Outline", 0f);
			}
			prevSelectedSpecimen = selectedSpecimen;
			SpecimenSprites[prevSelectedSpecimen].material.SetFloat("_Outline", 10f);
		}
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			if (amOpening || amClosing != CloseState.None)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
			if (player.GetButton(11))
			{
				if (!prevHadButton)
				{
					inputHandler.disableVirtualCursor = false;
					VirtualCursor.instance.SetWorldPosition(Specimens[selectedSpecimen].transform.position);
				}
				else
				{
					Vector3 position = Specimens[selectedSpecimen].transform.position;
					position.x = VirtualCursor.currentPosition.x;
					position.y = VirtualCursor.currentPosition.y;
					if (Vector2.Distance(position, Slots[selectedSpecimen].position) < 0.25f)
					{
						position = Slots[selectedSpecimen].position;
					}
					Specimens[selectedSpecimen].transform.position = position;
				}
				prevHadButton = true;
				return;
			}
			if (prevHadButton)
			{
				inputHandler.disableVirtualCursor = true;
				if (Vector2.Distance(Specimens[selectedSpecimen].transform.position, Slots[selectedSpecimen].position) < 0.25f)
				{
					Specimens[selectedSpecimen].transform.position = Slots[selectedSpecimen].position;
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(PlaceSounds.Random(), loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
					}
				}
				CheckForWin();
			}
			prevHadButton = false;
			if (axis2DRaw.sqrMagnitude > 0.7f)
			{
				if (!prevHadStick)
				{
					Vector2 normalized = axis2DRaw.normalized;
					int num = -1;
					float num2 = 0f;
					for (int i = 0; i < Specimens.Length; i++)
					{
						Vector2 lhs = Specimens[i].transform.position - Specimens[selectedSpecimen].transform.position;
						float magnitude = lhs.magnitude;
						lhs /= magnitude;
						float num3 = Vector2.Dot(lhs, normalized) / Mathf.Lerp(magnitude, 1f, 0.95f);
						if (i != selectedSpecimen && (num == -1 || (num3 > num2 && num3 > 0f)))
						{
							num2 = num3;
							num = i;
						}
					}
					if (num != -1)
					{
						selectedSpecimen = num;
					}
				}
				prevHadStick = true;
			}
			else
			{
				prevHadStick = false;
			}
			return;
		}
		prevHadButton = false;
		prevHadStick = false;
		if (prevSelectedSpecimen != -1)
		{
			SpecimenSprites[prevSelectedSpecimen].material.SetFloat("_Outline", 0f);
			prevSelectedSpecimen = -1;
		}
		for (int j = 0; j < Specimens.Length; j++)
		{
			Collider2D collider2D = Specimens[j];
			switch (cont.CheckDrag(collider2D))
			{
			case DragState.Dragging:
			{
				Vector3 localPosition = (Vector3)cont.DragPosition - transform.position;
				localPosition.z = 0f;
				collider2D.transform.localPosition = localPosition;
				if (Vector2.Distance(collider2D.transform.position, Slots[j].position) < 0.25f)
				{
					collider2D.transform.position = Slots[j].position;
				}
				break;
			}
			case DragState.Released:
				if (Vector2.Distance(collider2D.transform.position, Slots[j].position) < 0.25f)
				{
					collider2D.transform.position = Slots[j].position;
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(PlaceSounds.Random(), loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
					}
				}
				CheckForWin();
				break;
			}
		}
	}

	private void CheckForWin()
	{
		bool flag = true;
		for (int i = 0; i < Specimens.Length; i++)
		{
			if (Vector2.Distance(Specimens[i].transform.position, Slots[i].position) > 0.25f)
			{
				flag = false;
				break;
			}
		}
		if (flag && !completed)
		{
			completed = true;
			if (prevSelectedSpecimen != -1)
			{
				SpecimenSprites[prevSelectedSpecimen].material.SetFloat("_Outline", 0f);
			}
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose());
		}
	}

	public SpecimenGame()
	{ }
}
