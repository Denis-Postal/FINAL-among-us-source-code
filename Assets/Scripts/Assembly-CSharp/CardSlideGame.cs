using System.Collections;
using Rewired;
using TMPro;
using UnityEngine;

public class CardSlideGame : Minigame
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

	private FloatRange XRange = new FloatRange(-2.38f, 2.38f);

	public FloatRange AcceptedTime = new FloatRange(0.4f, 0.6f);

	public Collider2D col;

	public SpriteRenderer redLight;

	public SpriteRenderer greenLight;

	public TextMeshPro StatusText;

	public AudioClip AcceptSound;

	public AudioClip DenySound;

	public AudioClip[] CardMove;

	public AudioClip WalletOut;

	public float dragTime;

	private bool moving;

	private TouchpadBehavior touchpad;

	private Vector2 prevStickInput = Vector2.zero;

	private float xPos = -2.38f;

	private bool hadPrev;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardPleaseInsert);
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	public void Update()
	{
		if (MyNormTask.IsComplete)
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
				if (touchpad.IsTouching() && touchpad.GetTouchVector().y > 1f)
				{
					moving = false;
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
						float x = localPosition.x;
						localPosition.x += num2;
						if (num2 > 0.01f)
						{
							dragTime += Time.deltaTime;
							redLight.color = gray;
							greenLight.color = gray;
							if (!moving)
							{
								moving = true;
								if (Constants.ShouldPlaySfx())
								{
									SoundManager.Instance.PlaySound(CardMove.Random(), loop: false);
								}
							}
						}
						localPosition.x = XRange.Clamp(localPosition.x);
						float num3 = localPosition.x - x;
						float num4 = XRange.ReverseLerp(localPosition.x);
						float num5 = 0.8f * num3;
						VibrationManager.Vibrate(num5 * (1f - num4), num5 * num4, 0.01f);
					}
					else if ((double)XRange.ReverseLerp(localPosition.x) <= 0.05)
					{
						dragTime = 0f;
					}
					prevStickInput = axis2DRaw;
					hadPrev = true;
					break;
				}
				if (touchpad.IsTouching())
				{
					localPosition.x = XRange.Clamp(touchpad.GetTouchVector().x + xPos);
					hadPrev = true;
					if (!moving)
					{
						moving = true;
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(CardMove.Random(), loop: false);
						}
					}
					if (moving)
					{
						dragTime += Time.deltaTime;
					}
					break;
				}
				if (hadPrev)
				{
					if (XRange.max - localPosition.x < 0.05f)
					{
						if (AcceptedTime.Contains(dragTime))
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(AcceptSound, loop: false);
							}
							State = TaskStages.After;
							StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardAccepted);
							StartCoroutine(PutCardBack());
							if ((bool)MyNormTask)
							{
								MyNormTask.NextStep();
							}
							redLight.color = gray;
							greenLight.color = green;
						}
						else
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(DenySound, loop: false);
							}
							if (AcceptedTime.max < dragTime)
							{
								StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardTooSlow);
							}
							else
							{
								StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardTooFast);
							}
							redLight.color = Color.red;
							greenLight.color = gray;
							DestroyableSingleton<AchievementManager>.Instance.OnTaskFailure(TaskType);
						}
					}
					else
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(DenySound, loop: false);
						}
						StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardBadRead);
						redLight.color = Color.red;
						greenLight.color = gray;
						DestroyableSingleton<AchievementManager>.Instance.OnTaskFailure(TaskType);
					}
				}
				localPosition.x = Mathf.Lerp(localPosition.x, XRange.min, Time.deltaTime * 4f);
				hadPrev = false;
				if ((double)XRange.ReverseLerp(localPosition.x) <= 0.05)
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
				vector.x = XRange.Clamp(vector.x);
				if (vector.x - localPosition.x > 0.01f)
				{
					dragTime += Time.deltaTime;
					redLight.color = gray;
					greenLight.color = gray;
					if (!moving)
					{
						moving = true;
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(CardMove.Random(), loop: false);
						}
					}
				}
				localPosition.x = vector.x;
				break;
			}
			case DragState.NoTouch:
				if (State == TaskStages.Inserted)
				{
					localPosition.x = Mathf.Lerp(localPosition.x, XRange.min, Time.deltaTime * 4f);
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
					if (XRange.max - localPosition.x < 0.05f)
					{
						if (AcceptedTime.Contains(dragTime))
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(AcceptSound, loop: false);
							}
							State = TaskStages.After;
							StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardAccepted);
							StartCoroutine(PutCardBack());
							if ((bool)MyNormTask)
							{
								MyNormTask.NextStep();
							}
							redLight.color = gray;
							greenLight.color = green;
						}
						else
						{
							if (Constants.ShouldPlaySfx())
							{
								SoundManager.Instance.PlaySound(DenySound, loop: false);
							}
							if (AcceptedTime.max < dragTime)
							{
								StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardTooSlow);
							}
							else
							{
								StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardTooFast);
							}
							DestroyableSingleton<AchievementManager>.Instance.OnTaskFailure(TaskType);
							redLight.color = Color.red;
							greenLight.color = gray;
						}
					}
					else
					{
						if (Constants.ShouldPlaySfx())
						{
							SoundManager.Instance.PlaySound(DenySound, loop: false);
						}
						StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardBadRead);
						redLight.color = Color.red;
						greenLight.color = gray;
						DestroyableSingleton<AchievementManager>.Instance.OnTaskFailure(TaskType);
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
		Vector3 pos = col.transform.localPosition;
		Vector3 targ = new Vector3(-1.11f, -1.9f, pos.z);
		float time = 0f;
		while (true)
		{
			float t = Mathf.Min(1f, time / 0.6f);
			col.transform.localPosition = Vector3.Lerp(pos, targ, t);
			col.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.75f, t);
			if (time > 0.6f)
			{
				break;
			}
			yield return null;
			time += Time.deltaTime;
		}
		StartCoroutine(CoStartClose());
	}

	private IEnumerator InsertCard()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(WalletOut, loop: false);
		}
		Vector3 pos = col.transform.localPosition;
		Vector3 targ = new Vector3(XRange.min, 0.75f, pos.z);
		float time = 0f;
		while (true)
		{
			float t = Mathf.Min(1f, time / 0.6f);
			col.transform.localPosition = Vector3.Lerp(pos, targ, t);
			col.transform.localScale = Vector3.Lerp(Vector3.one * 0.75f, Vector3.one, t);
			if (time > 0.6f)
			{
				break;
			}
			yield return null;
			time += Time.deltaTime;
		}
		StatusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SwipeCardPleaseSwipe);
		greenLight.color = green;
		State = TaskStages.Inserted;
	}
}
