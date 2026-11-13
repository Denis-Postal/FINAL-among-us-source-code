using System.Linq;
using UnityEngine;

public class DoorBreakerGame : Minigame, IDoorMinigame
{
	public OpenableDoor MyDoor;

	public SpriteRenderer[] Buttons;

	public AudioClip FlipSound;

	public void SetDoor(OpenableDoor door)
	{
		MyDoor = door;
	}

	public void Start()
	{
		for (int i = 0; i < Buttons.Length; i++)
		{
			SpriteRenderer obj = Buttons[i];
			obj.color = Color.gray;
			obj.GetComponent<PassiveButton>().enabled = false;
		}
		int num = 0;
		while (num < 4)
		{
			SpriteRenderer spriteRenderer = Buttons.Random();
			if (!spriteRenderer.flipX)
			{
				spriteRenderer.color = Color.white;
				spriteRenderer.GetComponent<PassiveButton>().enabled = true;
				spriteRenderer.flipX = true;
				num++;
			}
		}
	}

	public void FlipSwitch(SpriteRenderer button)
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(FlipSound, loop: false);
		}
		button.color = Color.gray;
		button.flipX = false;
		button.GetComponent<PassiveButton>().enabled = false;
		if (Buttons.All((SpriteRenderer s) => !s.flipX))
		{
			ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Doors, (byte)(MyDoor.Id | 0x40));
			MyDoor.SetDoorway(open: true);
			StartCoroutine(CoStartClose(0.4f));
		}
	}
}
