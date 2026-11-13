using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class KeyMinigame : Minigame
{
	public KeySlotBehaviour[] Slots;

	private int keyState;

	public SpriteRenderer KeyImage;

	public Sprite normalImage;

	public Sprite insertImage;

	public BoxCollider2D key;

	private int targetSlotId;

	private Controller controller= new Controller();

	public AudioClip KeyGrab;

	public AudioClip KeyInsert;

	public AudioClip KeyOpen;

	public AudioClip KeyTurn;

	private TouchpadBehavior touchpad;

	private Vector3 initialPos;

	private bool prevHadInput;

	private Vector2 prevInputDir;

	private float currentAngle;

	public GameObject moveKeyGlyph;

	public GameObject turnKeyGlyph;

	public void Start()
	{
		targetSlotId = (PlayerControl.LocalPlayer ? PlayerControl.LocalPlayer.PlayerId : 0).Wrap(Slots.Length);
		Slots[targetSlotId].SetHighlight();
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	public unsafe void Update()
{
		if (keyState == 2)
		{
			return;
		}
		KeySlotBehaviour keySlotBehaviour = Slots[targetSlotId];
		controller.Update();
		switch (keyState)
		{
		case 0:
			if (!moveKeyGlyph.activeSelf)
			{
				moveKeyGlyph.SetActive(value: true);
			}
			if (turnKeyGlyph.activeSelf)
			{
				turnKeyGlyph.SetActive(value: false);
			}
			break;
		case 1:
			if (moveKeyGlyph.activeSelf)
			{
				moveKeyGlyph.SetActive(value: false);
			}
			if (!turnKeyGlyph.activeSelf)
			{
				turnKeyGlyph.SetActive(value: true);
			}
			break;
		}
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			Vector2 zero = Vector2.zero;
			zero = player.GetAxis2DRaw(13, 14);
			if ((double)zero.sqrMagnitude > 0.5)
			{
				if (!prevHadInput)
				{
					if (keyState == 0)
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(KeyGrab, loop: false);
						}
					}
					else if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(KeyTurn, loop: false);
					}
				}
				if (keyState == 0)
				{
					Vector3 vector = zero * Time.deltaTime * 5f;
					key.transform.localPosition += vector;
					if (key.IsTouching(keySlotBehaviour.Hitbox))
					{
						KeyImage.sprite = insertImage;
					}
					else
					{
						KeyImage.sprite = normalImage;
					}
				}
				else
				{
					zero = zero.normalized;
					if (prevHadInput)
					{
						currentAngle += Vector2.SignedAngle(prevInputDir, zero);
						currentAngle = Mathf.Clamp(currentAngle, 0f, 90f);
						key.transform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
					}
					else
					{
						currentAngle = key.transform.localEulerAngles.z;
					}
					prevInputDir = zero;
				}
				prevHadInput = true;
			}
			else if (touchpad.IsTouching())
			{
				if (touchpad.IsFirstTouch())
				{
					initialPos = key.transform.localPosition;
				}
				zero = touchpad.GetTouchVector();
				if (!prevHadInput)
				{
					if (keyState == 0)
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(KeyGrab, loop: false);
						}
					}
					else if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(KeyTurn, loop: false);
					}
				}
				if (keyState == 0)
				{
					key.transform.localPosition = (Vector3)zero + initialPos;
					if (key.IsTouching(keySlotBehaviour.Hitbox))
					{
						KeyImage.sprite = insertImage;
					}
					else
					{
						KeyImage.sprite = normalImage;
					}
				}
				else
				{
					zero = touchpad.GetCenterToTouch().normalized;
					if (prevHadInput)
					{
						currentAngle += Vector2.SignedAngle(prevInputDir, zero);
						currentAngle = Mathf.Clamp(currentAngle, 0f, 90f);
						key.transform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
					}
					else
					{
						currentAngle = key.transform.localEulerAngles.z;
					}
					prevInputDir = zero;
				}
				prevHadInput = true;
			}
			else
			{
				if (!prevHadInput)
				{
					return;
				}
				if (keyState == 0)
				{
					if (key.IsTouching(keySlotBehaviour.Hitbox))
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(KeyInsert, loop: false);
						}
						keyState = 1;
						key.size = new Vector2(2f, 2f);
						Vector3 position = keySlotBehaviour.transform.position;
						position.z--;
						key.transform.position = position;
						KeyImage.sprite = insertImage;
						keySlotBehaviour.SetInserted();
					}
				}
				else
				{
					float num = key.transform.localEulerAngles.z;
					if (num > 180f)
					{
						num -= 360f;
					}
					num %= 360f;
					if (Mathf.Abs(num) > 80f)
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(KeyOpen, loop: false);
						}
						keySlotBehaviour.SetFinished();
						key.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
						keyState = 2;
						MyNormTask.NextStep();
						StartCoroutine(CoStartClose());
					}
					else
					{
						key.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					}
				}
				prevHadInput = false;
			}
			return;
		}
		switch (controller.CheckDrag(key))
		{
		case DragState.TouchStart:
			if (keyState == 0)
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(KeyGrab, loop: false);
				}
			}
			else if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(KeyTurn, loop: false);
			}
			break;
		case DragState.Dragging:
			if (keyState == 0)
			{
				Vector2 vector2 = controller.DragPosition - (Vector2)transform.position;
				key.transform.localPosition = vector2;
				if (key.IsTouching(keySlotBehaviour.Hitbox))
				{
					KeyImage.sprite = insertImage;
				}
				else
				{
					KeyImage.sprite = normalImage;
				}
			}
			else
			{
				Vector2 vector3 = key.transform.position;
				float value = Vector2.SignedAngle(controller.DragStartPosition - vector3, controller.DragPosition - vector3);
				key.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Clamp(value, -90f, 90f));
			}
			break;
		case DragState.Released:
		{
			if (keyState == 0)
			{
				if (key.IsTouching(keySlotBehaviour.Hitbox))
				{
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(KeyInsert, loop: false);
					}
					keyState = 1;
					key.size = new Vector2(2f, 2f);
					Vector3 position2 = keySlotBehaviour.transform.position;
					position2.z--;
					key.transform.position = position2;
					KeyImage.sprite = insertImage;
					keySlotBehaviour.SetInserted();
				}
				break;
			}
			float num2 = key.transform.localEulerAngles.z;
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			num2 %= 360f;
			if (Mathf.Abs(num2) > 80f)
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(KeyOpen, loop: false);
				}
				keySlotBehaviour.SetFinished();
				key.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
				keyState = 2;
				MyNormTask.NextStep();
				StartCoroutine(CoStartClose());
			}
			else
			{
				key.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
			break;
		}
		case DragState.Holding:
			break;
		}
	}

	public KeyMinigame()
	{ }
}
