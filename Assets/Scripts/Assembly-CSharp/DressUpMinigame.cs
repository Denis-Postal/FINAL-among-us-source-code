using Object = UnityEngine.Object;
using System.Collections;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class DressUpMinigame : Minigame
{
	public SpriteRenderer DummyHat;

	public SpriteRenderer DummyAccessory;

	public SpriteRenderer DummyClothes;

	public SpriteRenderer ActualHat;

	public SpriteRenderer ActualAccessory;

	public SpriteRenderer ActualClothes;

	public DressUpCosmetic[] buttons;

	public Sprite[] Hats;

	public Sprite[] Accessories;

	public Sprite[] Clothes;

	public Collider2D hatHitbox;

	public Collider2D faceHitbox;

	public Collider2D bodyHitbox;

	public SpriteRenderer draggable;

	public AudioClip hatSound;

	public AudioClip faceSound;

	public AudioClip clothesSound;

	public AudioClip correctSound;

	public AudioClip incorrectSound;

	public AudioClip finishedSound;

	private Controller controller= new Controller();

	public SpriteRenderer grabbyHand;

	public Transform cursorObject;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		int num = Hats.RandomIdx();
		DummyHat.transform.SetLocalZ((num == 1) ? 1 : (-1));
		DummyHat.sprite = Hats[num];
		DummyAccessory.sprite = Accessories.Random();
		DummyClothes.sprite = Clothes.Random();
		draggable.enabled = false;
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(grabbyHand);
		SetupInput(disableCursor: false);
	}

	public unsafe void Update()
{
		if (amClosing != CloseState.None)
		{
			return;
		}
		Vector3 position = cursorObject.position;
		position.x = VirtualCursor.currentPosition.x;
		position.y = VirtualCursor.currentPosition.y;
		cursorObject.position = position;
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick)
		{
			if (grabbyHand.enabled == draggable.enabled)
			{
				grabbyHand.enabled = !draggable.enabled;
			}
		}
		else if (grabbyHand.enabled)
		{
			grabbyHand.enabled = false;
		}
		controller.Update();
		DressUpCosmetic[] array = buttons;
		foreach (DressUpCosmetic dressUpCosmetic in array)
		{
			Vector2 dragPosition = controller.DragPosition;
			switch (controller.CheckDrag(dressUpCosmetic.Hitbox))
			{
			case DragState.TouchStart:
				draggable.enabled = true;
				draggable.sprite = dressUpCosmetic.Rend.sprite;
				draggable.transform.position = (Vector3)controller.DragPosition - draggable.sprite.bounds.center;
				switch (dressUpCosmetic.Slot)
				{
				case CosmeticType.Hat:
				{
					int num = Hats.IndexOf(dressUpCosmetic.Rend.sprite);
					ActualHat.transform.SetLocalZ((num == 1) ? 1 : (-1));
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(hatSound, loop: false);
					}
					break;
				}
				case CosmeticType.Accessory:
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(faceSound, loop: false);
					}
					break;
				case CosmeticType.Skin:
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(clothesSound, loop: false);
					}
					break;
				}
				break;
			case DragState.Dragging:
			{
				Vector3 position2 = controller.DragPosition;
				position2 -= draggable.sprite.bounds.center;
				position2.z = -5f + transform.position.z;
				switch (dressUpCosmetic.Slot)
				{
				case CosmeticType.Hat:
					if (hatHitbox.OverlapPoint(controller.DragPosition))
					{
						position2 = ActualHat.transform.position;
					}
					break;
				case CosmeticType.Accessory:
					if (faceHitbox.OverlapPoint(controller.DragPosition))
					{
						position2 = ActualAccessory.transform.position;
					}
					break;
				case CosmeticType.Skin:
					if (bodyHitbox.OverlapPoint(controller.DragPosition))
					{
						position2 = ActualClothes.transform.position;
					}
					break;
				}
				draggable.transform.position = position2;
				break;
			}
			case DragState.Released:
				draggable.enabled = false;
				switch (dressUpCosmetic.Slot)
				{
				case CosmeticType.Hat:
					if (hatHitbox.OverlapPoint(dragPosition))
					{
						SetHat(Hats.IndexOf(dressUpCosmetic.Rend.sprite));
					}
					break;
				case CosmeticType.Accessory:
					if (faceHitbox.OverlapPoint(dragPosition))
					{
						SetAccessory(Accessories.IndexOf(dressUpCosmetic.Rend.sprite));
					}
					break;
				case CosmeticType.Skin:
					if (bodyHitbox.OverlapPoint(dragPosition))
					{
						SetClothes(Clothes.IndexOf(dressUpCosmetic.Rend.sprite));
					}
					break;
				}
				break;
			}
		}
	}

	public void SetHat(int i)
	{
		if (amClosing != CloseState.None || i < 0)
		{
			return;
		}
		ActualHat.transform.SetLocalZ((i == 1) ? 1 : (-1));
		ActualHat.sprite = Hats[i];
		if (DummyHat.sprite == ActualHat.sprite)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(correctSound, loop: false);
			}
		}
		else if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(incorrectSound, loop: false);
		}
		CheckOutfit();
	}

	public void SetAccessory(int i)
	{
		if (amClosing != CloseState.None || i < 0)
		{
			return;
		}
		ActualAccessory.sprite = Accessories[i];
		if (DummyAccessory.sprite == ActualAccessory.sprite)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(correctSound, loop: false);
			}
		}
		else if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(incorrectSound, loop: false);
		}
		CheckOutfit();
	}

	public void SetClothes(int i)
	{
		if (amClosing != CloseState.None || i < 0)
		{
			return;
		}
		ActualClothes.sprite = Clothes[i];
		if (DummyClothes.sprite == ActualClothes.sprite)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(correctSound, loop: false);
			}
		}
		else if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(incorrectSound, loop: false);
		}
		CheckOutfit();
	}

	private void CheckOutfit()
	{
		if (DummyHat.sprite == ActualHat.sprite && DummyClothes.sprite == ActualClothes.sprite && DummyAccessory.sprite == ActualAccessory.sprite)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(finishedSound, loop: false);
			}
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose(0.7f));
		}
	}

	public DressUpMinigame()
	{ }
}
