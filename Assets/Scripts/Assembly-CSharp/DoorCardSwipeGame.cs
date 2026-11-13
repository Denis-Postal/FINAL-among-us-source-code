using System.Collections;
using Rewired;
using TMPro;
using UnityEngine;

public class DoorCardSwipeGame : Minigame, IDoorMinigame
{
	private enum TaskStages
	{
		Before,
		Animating,
		Inserted,
		After
	}

	private Color gray = new Color(0.45f, 0.45f, 0.45f);

	private Color green = new Color(0f, 0.8f, 0f);

	private TaskStages State;

	private Controller myController = new Controller();

	private FloatRange YRange = new FloatRange(-1.77f, 2f);

	public float minAcceptedTime = 0.3f;

	public Collider2D col;

	public SpriteRenderer confirmSymbol;

	public Sprite AcceptSymbol;

	public Sprite RejectSymbol;

	public TextMeshPro StatusText;

	public AudioClip AcceptSound;

	public AudioClip DenySound;

	public AudioClip[] CardMove;

	public AudioClip WalletOut;

	public float dragTime;

	private bool moving;

	private Vector2 prevStickInput = Vector2.zero;

	private bool hadPrev;

	private OpenableDoor MyDoor;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardPleaseInsert);
		SetupInput(disableCursor: true);
	}

	public void Update()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		Vector3 localPosition = col.transform.localPosition;
		myController.Update();
		if (Controller.currentTouchType == Controller.TouchType.Joystick)
		{
			Player player = ReInput.players.GetPlayer(0);
			Vector2 axis2DRaw = player.GetAxis2DRaw(13, 14);
			float magnitude = axis2DRaw.magnitude;
			switch (State)
			{
			case TaskStages.Before:
				if (player.GetAnyButtonDown())
				{
					State = TaskStages.Animating;
					StartCoroutine(InsertCard());
				}
				break;
			case TaskStages.Inserted:
				if (magnitude > 0.9f)
				{
					axis2DRaw = axis2DRaw.normalized;
					if (hadPrev)
					{
						float num = prevStickInput.AngleSigned(axis2DRaw);
						if (num > 180f)
						{
							num -= 360f;
						}
						if (num < -180f)
						{
							num += 360f;
						}
						float num2 = Mathf.Abs(num) * 0.025f;
						float y = localPosition.y;
						localPosition.y -= num2;
						if (num2 > 0.01f)
						{
							dragTime += Time.deltaTime;
							if (!moving)
							{
								moving = true;
								if (Constants.ShouldPlaySfx())
								{
									SoundManager.Instance.PlaySound(CardMove.Random(), loop: false);
								}
							}
						}
						localPosition.y = YRange.Clamp(localPosition.y);
						float num3 = localPosition.y - y;
						float num4 = YRange.ReverseLerp(localPosition.y);
						float num5 = 0.8f * num3;
						VibrationManager.Vibrate(num5 * (1f - num4), num5 * num4, 0.01f);
					}
					else if ((double)YRange.ReverseLerp(localPosition.y) <= 0.05)
					{
						dragTime = 0f;
					}
					prevStickInput = axis2DRaw;
					hadPrev = true;
					break;
				}
				if (hadPrev)
				{
					if (localPosition.y - YRange.min < 0.05f && !BoolRange.Next(0.01f))
					{
						if (dragTime > minAcceptedTime)
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(AcceptSound, loop: false);
							}
							State = TaskStages.After;
							StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardAccepted);
							StartCoroutine(PutCardBack());
							ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Doors, (byte)(MyDoor.Id | 0x40));
							MyDoor.SetDoorway(open: true);
							StartCoroutine(CoStartClose(0.4f));
							confirmSymbol.sprite = AcceptSymbol;
						}
						else
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(DenySound, loop: false);
							}
							StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardTooFast);
							confirmSymbol.sprite = RejectSymbol;
						}
					}
					else
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(DenySound, loop: false);
						}
						StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardBadRead);
						confirmSymbol.sprite = RejectSymbol;
					}
				}
				localPosition.y = Mathf.Lerp(localPosition.y, YRange.max, Time.deltaTime * 4f);
				hadPrev = false;
				if ((double)YRange.ReverseLerp(localPosition.y) <= 0.05)
				{
					dragTime = 0f;
				}
				break;
			}
		}
		else
		{
			switch (myController.CheckDrag(col))
			{
			case DragState.TouchStart:
				dragTime = 0f;
				break;
			case DragState.Dragging:
			{
				if (State != TaskStages.Inserted)
				{
					break;
				}
				Vector2 vector = myController.DragPosition - (Vector2)transform.position;
				vector.y = YRange.Clamp(vector.y);
				if (localPosition.y - vector.y > 0.01f)
				{
					dragTime += Time.deltaTime;
					confirmSymbol.sprite = null;
					if (!moving)
					{
						moving = true;
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(CardMove.Random(), loop: false);
						}
					}
				}
				localPosition.y = vector.y;
				break;
			}
			case DragState.NoTouch:
				if (State == TaskStages.Inserted)
				{
					localPosition.y = Mathf.Lerp(localPosition.y, YRange.max, Time.deltaTime * 4f);
				}
				break;
			case DragState.Released:
				moving = false;
				if (State == TaskStages.Before)
				{
					State = TaskStages.Animating;
					StartCoroutine(InsertCard());
				}
				else
				{
					if (State != TaskStages.Inserted)
					{
						break;
					}
					if (localPosition.y - YRange.min < 0.05f && !BoolRange.Next(0.01f))
					{
						if (dragTime > minAcceptedTime)
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(AcceptSound, loop: false);
							}
							State = TaskStages.After;
							StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardAccepted);
							StartCoroutine(PutCardBack());
							ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Doors, (byte)(MyDoor.Id | 0x40));
							MyDoor.SetDoorway(open: true);
							StartCoroutine(CoStartClose(0.4f));
							confirmSymbol.sprite = AcceptSymbol;
						}
						else
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(DenySound, loop: false);
							}
							StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardTooFast);
							confirmSymbol.sprite = RejectSymbol;
						}
					}
					else
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(DenySound, loop: false);
						}
						StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardBadRead);
						confirmSymbol.sprite = RejectSymbol;
					}
				}
				break;
			}
		}
		col.transform.localPosition = localPosition;
	}

	private IEnumerator PutCardBack()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(WalletOut, loop: false);
		}
		Vector3 localPosition = col.transform.localPosition;
		Vector3 dest = new Vector3(0.452f, -1.9f, 0f);
		yield return Effects.All(Effects.Rotate2D(col.transform, 90f, 0f, 0.4f), Effects.Slide3D(col.transform, localPosition, dest, 0.4f));
		StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardPleaseInsert);
		State = TaskStages.Before;
	}

	private IEnumerator InsertCard()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(WalletOut, loop: false);
		}
		Vector3 localPosition = col.transform.localPosition;
		Vector3 dest = new Vector3(-1.43f, YRange.max, 0f);
		yield return Effects.All(Effects.Rotate2D(col.transform, 0f, 90f, 0.4f), Effects.Slide3D(col.transform, localPosition, dest, 0.4f));
		StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardPleaseSwipe);
		State = TaskStages.Inserted;
	}

	public void SetDoor(OpenableDoor door)
	{
		MyDoor = door;
	}
}
