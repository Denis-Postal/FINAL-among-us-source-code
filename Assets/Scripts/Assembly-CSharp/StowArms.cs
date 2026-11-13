using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class StowArms : Minigame
{
	public GameObject GunContent;

	public GameObject RifleContent;

	public Transform selectorObject;

	public GameObject[] selectorSubobjects;

	public SpriteRenderer[] handSprites;

	public AudioClip pickupGun;

	public AudioClip placeGun;

	public Collider2D[] GunColliders;

	public DragSlot[] GunsSlots;

	public AudioClip pickupRifle;

	public AudioClip placeRifle;

	public Collider2D[] RifleColliders;

	public DragSlot[] RifleSlots;

	private Controller cont= new Controller();

	private Collider2D currentGrabbedObject;

	private Vector3 grabOffset;

	private AudioClip PickupSound
	{
		get
		{
			if (ConsoleId != 1)
			{
				return pickupRifle;
			}
			return pickupGun;
		}
	}

	private AudioClip PlaceSound
	{
		get
		{
			if (ConsoleId != 1)
			{
				return placeRifle;
			}
			return placeGun;
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		if (ConsoleId == 1)
		{
			GunContent.SetActive(value: true);
			RifleContent.SetActive(value: false);
		}
		else
		{
			GunContent.SetActive(value: false);
			RifleContent.SetActive(value: true);
		}
		SpriteRenderer[] array = handSprites;
		foreach (SpriteRenderer playerMaterialColors in array)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
		SetupInput(disableCursor: false);
	}

	public unsafe void Update()
	{
		cont.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			if (!selectorObject.gameObject.activeSelf)
			{
				selectorObject.gameObject.SetActive(value: true);
			}
		}
		else if (selectorObject.gameObject.activeSelf)
		{
			selectorObject.gameObject.SetActive(value: false);
		}
		if (ConsoleId == 1)
		{
			DoUpdate(GunColliders, GunsSlots);
		}
		else
		{
			DoUpdate(RifleColliders, RifleSlots);
		}
		if ((bool)currentGrabbedObject)
		{
			selectorObject.position = currentGrabbedObject.transform.position;
			selectorObject.SetLocalZ(0f);
			ValidateSelectorActive(selectorSubobjects[0], shouldBeActive: false);
			ValidateSelectorActive(selectorSubobjects[1], ConsoleId == 1);
			ValidateSelectorActive(selectorSubobjects[2], ConsoleId != 1);
		}
		else
		{
			selectorObject.position = VirtualCursor.currentPosition;
			selectorObject.SetLocalZ(0f);
			ValidateSelectorActive(selectorSubobjects[0], shouldBeActive: true);
			ValidateSelectorActive(selectorSubobjects[1], shouldBeActive: false);
			ValidateSelectorActive(selectorSubobjects[2], shouldBeActive: false);
		}
		static void ValidateSelectorActive(GameObject selector, bool shouldBeActive)
		{
			if (selector.gameObject.activeSelf != shouldBeActive)
			{
				selector.gameObject.SetActive(shouldBeActive);
			}
		}
	}

	private unsafe void DoUpdate(Collider2D[] colliders, DragSlot[] slots)
	{
		currentGrabbedObject = null;
		for (int i = 0; i < colliders.Length; i++)
		{
			Collider2D collider2D = colliders[i];
			DragSlot dragSlot = slots[i];
			if (dragSlot.Occupant == collider2D)
			{
				continue;
			}
			switch (cont.CheckDrag(collider2D))
			{
			case DragState.TouchStart:
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(PickupSound, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
				}
				currentGrabbedObject = collider2D;
				grabOffset = (Vector2)collider2D.transform.position - cont.DragStartPosition;
				break;
			case DragState.Holding:
			case DragState.Dragging:
			{
				currentGrabbedObject = collider2D;
				Vector3 position = transform.position;
				Vector3 position2 = (Vector3)cont.DragPosition + grabOffset;
				position2.z = position.z;
				collider2D.transform.position = position2;
				if (Vector2.Distance(collider2D.transform.position, dragSlot.TargetPosition) < 0.25f && !dragSlot.Occupant)
				{
					collider2D.transform.position = dragSlot.TargetPosition;
				}
				break;
			}
			case DragState.Released:
				if (Vector2.Distance(collider2D.transform.position, dragSlot.TargetPosition) < 0.25f && !dragSlot.Occupant)
				{
					collider2D.transform.position = dragSlot.TargetPosition;
					dragSlot.Occupant = collider2D;
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(PlaceSound, loop: false).pitch = FloatRange.Next(0.8f, 1.2f);
					}
				}
				CheckForWin(colliders, slots);
				break;
			}
		}
	}

	private void CheckForWin(Collider2D[] colliders, DragSlot[] slots)
	{
		bool flag = true;
		for (int i = 0; i < colliders.Length; i++)
		{
			if (Vector2.Distance(colliders[i].transform.position, slots[i].TargetPosition) > 0.25f)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose());
		}
	}

	public StowArms()
	{ }

	internal static void _003CUpdate_003Eg__ValidateSelectorActive_007C21_0(GameObject selector, bool shouldBeActive)
	{
		GameObject gameObject = selector.gameObject;
		bool activeSelf = gameObject.activeSelf;
		if (activeSelf != shouldBeActive)
		{
			GameObject gameObject2 = selector.gameObject;
			gameObject2.SetActive(shouldBeActive);
		}
	}
}
