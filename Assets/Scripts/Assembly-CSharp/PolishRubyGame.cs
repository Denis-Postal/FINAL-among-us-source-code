using System.Linq;
using Rewired;
using UnityEngine;

public class PolishRubyGame : Minigame
{
	public PassiveButton[] Buttons;

	public SpriteRenderer[] Sparkles;

	public int[] swipes;

	public Vector2[] directions;

	public int swipesToClean = 6;

	public AudioClip[] rubSounds;

	public AudioClip sparkleSound;

	public Transform cursorObject;

	public Transform handWipeObject;

	public SpriteRenderer[] handSprites;

	public BoxCollider2D rubyCursorBounds;

	private Controller cont = new Controller();

	private bool oldCursorOverlapsSmudge;

	public void Start()
	{
		swipes = new int[Buttons.Length];
		directions = new Vector2[Buttons.Length];
		Buttons.ForEach((PassiveButton b) =>
		{
			b.gameObject.SetActive(value: false);
		});
		int num = IntRange.Next(3, 5);
		for (int num2 = 0; num2 < num; num2++)
		{
			Buttons.Where((PassiveButton t) => !t.isActiveAndEnabled).Random().gameObject.SetActive(value: true);
		}
		SetupInput(disableCursor: true);
		SpriteRenderer[] array = handSprites;
		foreach (SpriteRenderer playerMaterialColors in array)
		{
			PlayerControl.LocalPlayer.SetPlayerMaterialColors(playerMaterialColors);
		}
		UpdateSpriteColor(cursorOverlapsSmudge: false);
	}

	private void UpdateSpriteColor(bool cursorOverlapsSmudge)
	{
		Color white = Color.white;
		white.a = (cursorOverlapsSmudge ? 1f : 0.5f);
		SpriteRenderer[] array = handSprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = white;
		}
	}

	public void PlaySparkleSound()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(sparkleSound, loop: false);
		}
	}

	public void Update()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		cont.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			cursorObject.gameObject.SetActive(value: true);
			Player player = ReInput.players.GetPlayer(0);
			Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
			Vector2 axis2DRaw2 = player.GetAxis2DRaw(16, 17);
			Vector3 localPosition = cursorObject.localPosition;
			Vector3 localPosition2 = handWipeObject.localPosition;
			Vector2 vector = rubyCursorBounds.size * 0.5f;
			localPosition.x += axis2DRaw.x * Time.deltaTime * 3f;
			localPosition.y += axis2DRaw.y * Time.deltaTime * 3f;
			localPosition.x = Mathf.Clamp(localPosition.x, 0f - vector.x, vector.x);
			localPosition.y = Mathf.Clamp(localPosition.y, 0f - vector.y, vector.y);
			localPosition2.x = axis2DRaw2.x * 0.35f;
			localPosition2.y = axis2DRaw2.y * 0.35f;
			cursorObject.transform.localPosition = localPosition;
			handWipeObject.transform.localPosition = localPosition2;
			Vector3 position = cursorObject.position;
			bool flag = false;
			for (int i = 0; i < Buttons.Length; i++)
			{
				PassiveButton passiveButton = Buttons[i];
				if (!passiveButton.isActiveAndEnabled || !passiveButton.Colliders[0].OverlapPoint(position))
				{
					continue;
				}
				if (!Sparkles[i].enabled)
				{
					flag = true;
				}
				Vector2 vector2 = directions[i];
				float y = vector2.y;
				vector2.y = localPosition2.x - vector2.x;
				vector2.x = localPosition2.x;
				directions[i] = vector2;
				if (Mathf.Sign(y) == Mathf.Sign(vector2.y) || vector2.y == 0f)
				{
					continue;
				}
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySoundImmediate(rubSounds.Random(), loop: false);
				}
				VibrationManager.Vibrate(0.1f, 0.1f, 0.05f, VibrationManager.VibrationFalloff.Linear);
				int num = ++swipes[i];
				if (num <= swipesToClean)
				{
					passiveButton.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Palette.ClearWhite, (float)num / (float)swipesToClean);
					if (num == swipesToClean)
					{
						Sparkles[i].enabled = true;
					}
				}
			}
			if (oldCursorOverlapsSmudge != flag)
			{
				oldCursorOverlapsSmudge = flag;
				UpdateSpriteColor(flag);
			}
		}
		else
		{
			cursorObject.gameObject.SetActive(value: false);
			Controller controller = DestroyableSingleton<PassiveButtonManager>.Instance.controller;
			if (controller.AnyTouch)
			{
				Vector2 position2 = controller.Touches[0].Position;
				for (int j = 0; j < Buttons.Length; j++)
				{
					PassiveButton passiveButton2 = Buttons[j];
					if (!passiveButton2.isActiveAndEnabled || !passiveButton2.Colliders[0].OverlapPoint(position2))
					{
						continue;
					}
					Vector2 vector3 = directions[j];
					float y2 = vector3.y;
					vector3.y = position2.x - vector3.x;
					vector3.x = position2.x;
					directions[j] = vector3;
					if (Mathf.Sign(y2) == Mathf.Sign(vector3.y) || vector3.y == 0f)
					{
						continue;
					}
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySoundImmediate(rubSounds.Random(), loop: false);
					}
					int num2 = ++swipes[j];
					if (num2 <= swipesToClean)
					{
						passiveButton2.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Palette.ClearWhite, (float)num2 / (float)swipesToClean);
						if (num2 == swipesToClean)
						{
							Sparkles[j].enabled = true;
						}
					}
				}
			}
		}
		bool flag2 = true;
		for (int k = 0; k < Buttons.Length; k++)
		{
			if (Buttons[k].isActiveAndEnabled && swipes[k] < swipesToClean)
			{
				flag2 = false;
				break;
			}
		}
		if (flag2)
		{
			MyNormTask.NextStep();
			Close();
		}
	}
}
