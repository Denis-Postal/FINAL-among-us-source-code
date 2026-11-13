using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;

public class BurgerMinigame : Minigame
{
	public BurgerTopping Plate;

	public BurgerTopping[] Toppings;

	public Sprite[] PaperToppings;

	public SpriteRenderer[] PaperSlots;

	private BurgerToppingTypes[] ExpectedToppings = new BurgerToppingTypes[6];

	private Stack<BurgerTopping> burger = new Stack<BurgerTopping>();

	private Controller controller = new Controller();

	public Transform Paper;

	private const float PaperOpenedY = 0f;

	private const float PaperClosedY = 4.8f;

	private bool PaperClosed;

	public SpriteRenderer Shadow;

	public Transform stickCursor;

	public SpriteRenderer handSprite;

	public BoxCollider2D stickSelectionCenterPoint;

	private Vector3 selectionCenterStart;

	private BurgerTopping stickSelectedTopping;

	private bool grabbedTopping;

	private Vector3 internalCursorPos;

	public float cursorLerpSpeed = 5f;

	public float cursorHoverLerpSpeed = 1f;

	private float stackHeight;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		burger.Push(Plate);
		Shadow.enabled = false;
		ExpectedToppings[0] = BurgerToppingTypes.Plate;
		if (BoolRange.Next(0.1f))
		{
			ExpectedToppings[1] = BurgerToppingTypes.Lettuce;
			ExpectedToppings[5] = BurgerToppingTypes.Lettuce;
		}
		else
		{
			ExpectedToppings[1] = BurgerToppingTypes.BottomBun;
			ExpectedToppings[5] = BurgerToppingTypes.TopBun;
		}
		for (int i = 2; i < 5; i++)
		{
			BurgerToppingTypes top = (BurgerToppingTypes)IntRange.Next(2, 6);
			if (ExpectedToppings.Count((BurgerToppingTypes t) => t == top) >= 2)
			{
				i--;
			}
			else
			{
				ExpectedToppings[i] = top;
			}
		}
		if (BoolRange.Next(0.01f))
		{
			BurgerToppingTypes burgerToppingTypes = ExpectedToppings[5];
			ExpectedToppings[5] = ExpectedToppings[4];
			ExpectedToppings[4] = burgerToppingTypes;
		}
		for (int num = 1; num < ExpectedToppings.Length; num++)
		{
			PaperSlots[num - 1].sprite = PaperToppings[(int)ExpectedToppings[num]];
		}
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(handSprite);
		internalCursorPos = stickCursor.localPosition;
		selectionCenterStart = stickSelectionCenterPoint.transform.localPosition;
		SetupInput(disableCursor: true);
	}

	private void SelectTopping(BurgerTopping topping)
	{
		if (stickSelectedTopping != null)
		{
			stickSelectedTopping.GetComponent<SpriteRenderer>().material.SetFloat("_Outline", 0f);
		}
		stickSelectedTopping = topping;
		if (stickSelectedTopping != null)
		{
			stickSelectedTopping.GetComponent<SpriteRenderer>().material.SetFloat("_Outline", 10f);
		}
	}

	public void Update()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		inputHandler.disableVirtualCursor = false;
		controller.Update();
		if (PaperClosed)
		{
			BurgerTopping[] toppings;
			if (Controller.currentTouchType == Controller.TouchType.Joystick)
			{
				if (grabbedTopping)
				{
					if (stickCursor.gameObject.activeSelf)
					{
						stickCursor.gameObject.SetActive(value: false);
					}
				}
				else if (!stickCursor.gameObject.activeSelf)
				{
					stickCursor.gameObject.SetActive(value: true);
				}
				if (burger.Count > 0)
				{
					stickSelectionCenterPoint.transform.localPosition = burger.Peek().transform.localPosition;
				}
				else
				{
					stickSelectionCenterPoint.transform.localPosition = selectionCenterStart;
				}
				Vector2 axis2DRaw = ReInput.players.GetPlayer(0).GetAxis2DRaw(13, 14);
				bool flag = false;
				if (!grabbedTopping)
				{
					toppings = Toppings;
					foreach (BurgerTopping burgerTopping in toppings)
					{
						if (controller.CheckHover(burgerTopping.Hitbox))
						{
							SelectTopping(burgerTopping);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					SelectTopping(null);
				}
				Vector3 b = stickSelectionCenterPoint.transform.localPosition + axis2DRaw.x * Vector3.right * stickSelectionCenterPoint.size.x * 0.5f + axis2DRaw.y * Vector3.up * stickSelectionCenterPoint.size.y * 0.5f;
				internalCursorPos = Vector3.Lerp(internalCursorPos, b, Time.deltaTime * ((flag && !grabbedTopping) ? cursorHoverLerpSpeed : cursorLerpSpeed));
				stickCursor.localPosition = internalCursorPos;
				_ = (Vector2)stickCursor.position;
				VirtualCursor.instance.SetWorldPosition(stickCursor.position);
			}
			else if (stickCursor.gameObject.activeSelf)
			{
				stickCursor.gameObject.SetActive(value: false);
			}
			grabbedTopping = false;
			toppings = Toppings;
			foreach (BurgerTopping burgerTopping2 in toppings)
			{
				switch (controller.CheckDrag(burgerTopping2.Hitbox))
				{
				case DragState.TouchStart:
				{
					if (burgerTopping2 == burger.Peek())
					{
						burger.Pop();
					}
					if (burger.Contains(burgerTopping2))
					{
						controller.ClearTouch();
						break;
					}
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(burgerTopping2.GrabSound, loop: false);
					}
					VibrationManager.Vibrate(0.15f, 0.15f, 0.1f, VibrationManager.VibrationFalloff.Linear);
					stackHeight = burger.Sum((BurgerTopping s) => s.Offset) + burgerTopping2.Offset;
					Shadow.enabled = true;
					Vector3 vector = (Vector3)controller.DragPosition - transform.position;
					vector.z = -1f;
					burgerTopping2.transform.localPosition = vector;
					Shadow.transform.localPosition = vector + new Vector3(0f, 0f - stackHeight, 0.8f);
					break;
				}
				case DragState.Holding:
					grabbedTopping = true;
					break;
				case DragState.Dragging:
				{
					grabbedTopping = true;
					Vector3 vector2 = (Vector3)controller.DragPosition - transform.position;
					vector2.z = -1f;
					burgerTopping2.transform.localPosition = vector2;
					Shadow.transform.localPosition = vector2 + new Vector3(0f, 0f - stackHeight, 0.8f);
					break;
				}
				case DragState.Released:
				{
					if (Constants.ShouldPlaySfx())
					{
						SoundManager.Instance.PlaySound(burgerTopping2.DropSound, loop: false);
					}
					VibrationManager.Vibrate(0.35f, 0.35f, 0.1f, VibrationManager.VibrationFalloff.Linear);
					Shadow.enabled = false;
					BurgerTopping burgerTopping3 = burger.Peek();
					if (!burgerTopping3.Hitbox.IsTouching(burgerTopping2.Hitbox))
					{
						break;
					}
					burgerTopping2.transform.position = burgerTopping3.transform.position + new Vector3(0f, burgerTopping2.Offset, -0.001f);
					burger.Push(burgerTopping2);
					if (burger.Count != ExpectedToppings.Length)
					{
						break;
					}
					bool flag2 = true;
					BurgerToppingTypes[] array = burger.Select((BurgerTopping tt) => tt.ToppingType).Reverse().ToArray();
					for (int num = 0; num < array.Length; num++)
					{
						if (array[num] != ExpectedToppings[num])
						{
							flag2 = false;
							break;
						}
					}
					if (flag2)
					{
						MyNormTask.NextStep();
						StartCoroutine(CoStartClose(0.5f));
					}
					break;
				}
				}
			}
		}
		else if (stickCursor.gameObject.activeSelf)
		{
			stickCursor.gameObject.SetActive(value: false);
		}
		Vector3 localPosition = Paper.localPosition;
		localPosition.y = Mathf.Lerp(localPosition.y, PaperClosed ? 4.8f : 0f, Time.deltaTime * 10f);
		Paper.localPosition = localPosition;
	}

	public void TogglePaper()
	{
		PaperClosed = !PaperClosed;
	}
}
