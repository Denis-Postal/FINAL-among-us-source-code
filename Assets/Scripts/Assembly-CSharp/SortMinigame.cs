using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class SortMinigame : Minigame
{
	public SortGameObject[] Objects;

	public BoxCollider2D AnimalBox;

	public BoxCollider2D PlantBox;

	public BoxCollider2D MineralBox;

	public AudioClip[] PickUpSounds;

	public AudioClip[] DropSounds;

	private Controller myController= new Controller();

	public Transform selectorObject;

	public SpriteRenderer selectorHand;

	private bool prevHadButton;

	private int currentlyGrabbedObject= -1;

	public unsafe void Start()
{
		Objects.Shuffle();
		for (int i = 0; i < Objects.Length; i++)
		{
			SortGameObject sortGameObject = Objects[i];
			sortGameObject.transform.localPosition = new Vector3(Mathf.Lerp(-2f, 2f, (float)i / ((float)Objects.Length - 1f)), FloatRange.Next(-2.25f, -1.7f), -1f);
			CheckBox(sortGameObject, dropped: true);
		}
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(selectorHand);
		SetupInput(disableCursor: false);
	}

	public unsafe void Update()
{
		if (amClosing != CloseState.None)
		{
			return;
		}
		myController.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			bool button = ReInput.players.GetPlayer(0).GetButton(11);
			Vector3 position = selectorObject.transform.position;
			position.x = VirtualCursor.currentPosition.x;
			position.y = VirtualCursor.currentPosition.y;
			selectorObject.transform.position = position;
			if (button)
			{
				if (!prevHadButton)
				{
					float num = 0f;
					currentlyGrabbedObject = -1;
					for (int i = 0; i < Objects.Length; i++)
					{
						float sqrMagnitude = ((Vector2)(Objects[i].transform.localPosition - selectorObject.transform.localPosition)).sqrMagnitude;
						if (sqrMagnitude <= 2f && (currentlyGrabbedObject == -1 || sqrMagnitude < num))
						{
							num = sqrMagnitude;
							currentlyGrabbedObject = i;
						}
					}
					if (currentlyGrabbedObject != -1)
					{
						SortGameObject obj = Objects[currentlyGrabbedObject];
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(PickUpSounds.Random(), loop: false);
						}
						obj.StopAllCoroutines();
						obj.StartCoroutine(obj.CoShadowRise());
					}
				}
				else if (currentlyGrabbedObject != -1)
				{
					SortGameObject sortGameObject = Objects[currentlyGrabbedObject];
					Vector3 position2 = sortGameObject.transform.position;
					position2.x = position.x;
					position2.y = position.y;
					sortGameObject.transform.position = position2;
					CheckBox(sortGameObject, dropped: false);
				}
				prevHadButton = true;
			}
			else
			{
				if (prevHadButton && currentlyGrabbedObject != -1)
				{
					SortGameObject sortGameObject2 = Objects[currentlyGrabbedObject];
					bool flag = true;
					for (int j = 0; j < Objects.Length; j++)
					{
						SortGameObject obj2 = Objects[j];
						flag &= CheckBox(obj2, dropped: true);
					}
					sortGameObject2.StopAllCoroutines();
					sortGameObject2.StartCoroutine(sortGameObject2.CoShadowFall(CheckBox(sortGameObject2, dropped: true), DropSounds.Random()));
					if (flag)
					{
						MyNormTask.NextStep();
						StartCoroutine(CoStartClose());
					}
				}
				prevHadButton = false;
				currentlyGrabbedObject = -1;
			}
			if (currentlyGrabbedObject != -1 && selectorObject.gameObject.activeSelf)
			{
				selectorObject.gameObject.SetActive(value: false);
			}
			else if (currentlyGrabbedObject == -1 && !selectorObject.gameObject.activeSelf)
			{
				selectorObject.gameObject.SetActive(value: true);
			}
			return;
		}
		if (selectorObject.gameObject.activeSelf)
		{
			selectorObject.gameObject.SetActive(value: false);
		}
		if (currentlyGrabbedObject != -1)
		{
			SortGameObject sortGameObject3 = Objects[currentlyGrabbedObject];
			bool flag2 = true;
			for (int k = 0; k < Objects.Length; k++)
			{
				SortGameObject obj3 = Objects[k];
				flag2 &= CheckBox(obj3, dropped: true);
			}
			sortGameObject3.StopAllCoroutines();
			sortGameObject3.StartCoroutine(sortGameObject3.CoShadowFall(CheckBox(sortGameObject3, dropped: true), DropSounds.Random()));
			if (flag2)
			{
				MyNormTask.NextStep();
				StartCoroutine(CoStartClose());
			}
			currentlyGrabbedObject = -1;
			prevHadButton = false;
		}
		for (int l = 0; l < Objects.Length; l++)
		{
			SortGameObject sortGameObject4 = Objects[l];
			switch (myController.CheckDrag(sortGameObject4.Collider))
			{
			case DragState.TouchStart:
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(PickUpSounds.Random(), loop: false);
				}
				sortGameObject4.StopAllCoroutines();
				sortGameObject4.StartCoroutine(sortGameObject4.CoShadowRise());
				break;
			case DragState.Dragging:
			{
				Vector2 dragPosition = myController.DragPosition;
				Vector3 position3 = sortGameObject4.transform.position;
				position3.x = dragPosition.x;
				position3.y = dragPosition.y;
				sortGameObject4.transform.position = position3;
				CheckBox(sortGameObject4, dropped: false);
				break;
			}
			case DragState.Released:
			{
				bool flag3 = true;
				for (int m = 0; m < Objects.Length; m++)
				{
					SortGameObject obj4 = Objects[m];
					flag3 &= CheckBox(obj4, dropped: true);
				}
				sortGameObject4.StopAllCoroutines();
				sortGameObject4.StartCoroutine(sortGameObject4.CoShadowFall(CheckBox(sortGameObject4, dropped: true), DropSounds.Random()));
				if (flag3)
				{
					MyNormTask.NextStep();
					StartCoroutine(CoStartClose());
				}
				break;
			}
			}
		}
	}

	private unsafe bool CheckBox(SortGameObject obj, bool dropped)
	{ /* spliced-from-2019 */
		BoxCollider2D collider = null;
		switch (obj.MyType)
		{
		case SortGameObject.ObjType.Animal:
			collider = AnimalBox;
			break;
		case SortGameObject.ObjType.Mineral:
			collider = MineralBox;
			break;
		case SortGameObject.ObjType.Plant:
			collider = PlantBox;
			break;
		}
		if (obj.Collider.IsTouching(collider))
		{
			obj.Shadow.material.SetFloat("_Outline", 1f);
			obj.Shadow.material.SetColor("_OutlineColor", new Color(0f, 0.8f, 1f));
			return true;
		}
		if (dropped)
		{
			obj.Shadow.material.SetFloat("_Outline", 1f);
			obj.Shadow.material.SetColor("_OutlineColor", Color.red);
		}
		else
		{
			obj.Shadow.material.SetFloat("_Outline", 0f);
		}
		return false;
	}

	public SortMinigame()
	{ }
}
