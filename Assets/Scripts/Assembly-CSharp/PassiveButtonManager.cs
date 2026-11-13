using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassiveButtonManager : DestroyableSingleton<PassiveButtonManager>
{
	private enum ButtonStates
	{
		Up,
		Down,
		Drag
	}

	private class DepthComparer : IComparer<PassiveUiElement>
	{
		public static readonly DepthComparer Instance = new DepthComparer();

		public int Compare(PassiveUiElement x, PassiveUiElement y)
		{
			if (x == null)
			{
				return 1;
			}
			if (y == null)
			{
				return -1;
			}
			return x.CachedZ.CompareTo(y.CachedZ);
		}
	}

	public List<PassiveUiElement> Buttons = new List<PassiveUiElement>();

	private List<IFocusHolder> FocusHolders = new List<IFocusHolder>();

	private PassiveUiElement currentOver;

	public Controller controller = new Controller();

	private ButtonStates currentState;

	private Collider2D[] results = new Collider2D[40];

	public void RegisterOne(PassiveUiElement button)
	{
		Buttons.Add(button);
	}

	public void RemoveOne(PassiveUiElement passiveButton)
	{
		Buttons.Remove(passiveButton);
	}

	public void RegisterOne(IFocusHolder focusHolder)
	{
		FocusHolders.Add(focusHolder);
	}

	public void RemoveOne(IFocusHolder focusHolder)
	{
		FocusHolders.Remove(focusHolder);
	}

	public void Update()
	{
		if (!Application.isFocused)
		{
			return;
		}
		controller.Update();
		for (int i = 0; i < Buttons.Count; i++)
		{
			if (Buttons[i].transform.hasChanged)
			{
				Buttons[i].CachedZ = Buttons[i].transform.position.z;
				Buttons[i].transform.hasChanged = false;
			}
		}
		for (int j = 1; j < Buttons.Count; j++)
		{
			if (DepthComparer.Instance.Compare(Buttons[j - 1], Buttons[j]) > 0)
			{
				Buttons.Sort(DepthComparer.Instance);
				break;
			}
		}
		HandleMouseOut();
		for (int k = 0; k < Buttons.Count; k++)
		{
			PassiveUiElement passiveUiElement = Buttons[k];
			if (!passiveUiElement)
			{
				Buttons.RemoveAt(k);
				k--;
			}
			else
			{
				if (!passiveUiElement.isActiveAndEnabled)
				{
					continue;
				}
				if ((bool)passiveUiElement.ClickMask)
				{
					Controller.TouchState touch = controller.GetTouch(0);
					Vector2 position = touch.Position;
					if (touch.IsDown && !passiveUiElement.ClickMask.OverlapPoint(position))
					{
						continue;
					}
				}
				for (int l = 0; l < passiveUiElement.Colliders.Length; l++)
				{
					Collider2D col = passiveUiElement.Colliders[l];
					if (!col || !col.isActiveAndEnabled)
					{
						continue;
					}
					HandleMouseOver(passiveUiElement, col);
					switch (controller.CheckDrag(col))
					{
					case DragState.TouchStart:
						if (passiveUiElement.HandleDown)
						{
							passiveUiElement.ReceiveClickDown();
						}
						if (passiveUiElement.HandleDownClickGraphic)
						{
							passiveUiElement.ReceiveClickDownGraphic();
						}
						break;
					case DragState.Holding:
						if (passiveUiElement.HandleRepeat)
						{
							passiveUiElement.ReceiveRepeatDown();
						}
						break;
					case DragState.Dragging:
						if (passiveUiElement.HandleDrag)
						{
							Vector2 dragDelta = controller.DragPosition - controller.DragStartPosition;
							passiveUiElement.ReceiveClickDrag(dragDelta);
							controller.ResetDragPosition();
						}
						else if (passiveUiElement.HandleRepeat)
						{
							passiveUiElement.ReceiveRepeatDown();
						}
						else if (Buttons.Any((PassiveUiElement b2) => b2.HandleDrag && b2.isActiveAndEnabled && b2.transform.position.z > col.transform.position.z))
						{
							controller.ClearTouch();
						}
						break;
					case DragState.Released:
						if (passiveUiElement.HandleUp)
						{
							passiveUiElement.ReceiveClickUp();
						}
						if (passiveUiElement.HandleRepeat)
						{
							passiveUiElement.ReleaseButton();
						}
						if (passiveUiElement.HandleUpClickGraphic)
						{
							passiveUiElement.ReceiveClickUpGraphic();
						}
						break;
					}
				}
			}
		}
		if (controller.AnyTouchDown)
		{
			Vector2 touch2 = GetTouch(getDownTouch: false);
			HandleFocus(touch2);
		}
	}

	private void HandleFocus(Vector2 pt)
	{
		bool flag = false;
		for (int i = 0; i < FocusHolders.Count; i++)
		{
			IFocusHolder focusHolder = FocusHolders[i];
			if (!(focusHolder as MonoBehaviour))
			{
				FocusHolders.RemoveAt(i);
				i--;
			}
			else
			{
				if (!focusHolder.CheckCollision(pt))
				{
					continue;
				}
				float depth = (focusHolder as MonoBehaviour).transform.position.z;
				if (Buttons.Any((PassiveUiElement top) => top.isActiveAndEnabled && top.transform.position.z < depth && top.Colliders.Any((Collider2D c) => c.OverlapPoint(pt))))
				{
					break;
				}
				flag = true;
				focusHolder.GiveFocus();
				for (int num = 0; num < FocusHolders.Count; num++)
				{
					if (num != i)
					{
						FocusHolders[num].LoseFocus();
					}
				}
				break;
			}
		}
		if (!flag)
		{
			for (int num2 = 0; num2 < FocusHolders.Count; num2++)
			{
				FocusHolders[num2].LoseFocus();
			}
		}
	}

	public void LoseFocusForAll()
	{
		for (int i = 0; i < FocusHolders.Count; i++)
		{
			FocusHolders[i].LoseFocus();
		}
	}

	private void HandleMouseOut()
	{
		if (!currentOver)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < controller.Touches.Length; i++)
		{
			Controller.TouchState pt = controller.GetTouch(i);
			if (pt.active && currentOver.Colliders.Any((Collider2D c) => c.OverlapPoint(pt.Position)))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			currentOver.ReceiveMouseOut();
			currentOver = null;
		}
	}

	private void HandleMouseOver(PassiveUiElement button, Collider2D col)
	{
		if (!button.HandleOverOut || button == currentOver)
		{
			return;
		}
		if ((bool)button.ClickMask)
		{
			Vector2 position = controller.GetTouch(0).Position;
			if (!button.ClickMask.OverlapPoint(position))
			{
				return;
			}
		}
		if ((bool)currentOver && button.transform.position.z > currentOver.transform.position.z)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < controller.Touches.Length; i++)
		{
			if (controller.Touches[i].active && col.OverlapPoint(controller.GetTouch(i).Position))
			{
				flag = true;
			}
		}
		if (flag)
		{
			if ((bool)currentOver && currentOver != button)
			{
				currentOver.ReceiveMouseOut();
			}
			currentOver = button;
			currentOver.ReceiveMouseOver();
		}
	}

	private Vector2 GetTouch(bool getDownTouch)
	{
		if (getDownTouch)
		{
			for (int i = 0; i < controller.Touches.Length; i++)
			{
				if (controller.Touches[i].TouchStart)
				{
					return controller.Touches[i].Position;
				}
			}
		}
		else
		{
			for (int j = 0; j < controller.Touches.Length; j++)
			{
				if (controller.Touches[j].TouchEnd)
				{
					return controller.Touches[j].Position;
				}
			}
		}
		return new Vector2(-5000f, -5000f);
	}
}
