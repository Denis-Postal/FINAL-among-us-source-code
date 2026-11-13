using Cpp2ILInjected;
using UnityEngine;

public class Scrollbar : PassiveUiElement
{
	[SerializeField]
	private Scroller parent;

	[SerializeField]
	public SpriteRenderer graphic;

	[SerializeField]
	private SpriteRenderer trackGraphic;

	[SerializeField]
	private bool horizontal;

	[SerializeField]
	private float dragSpeed;

	private bool dragable = true;

	public override bool HandleDrag => dragable;

	public void Toggle(bool on)
	{
		graphic.enabled = on;
		if (trackGraphic != null)
		{
			trackGraphic.enabled = on;
		}
		dragable = on;
	}

	public unsafe void UpdatePosition(Vector3 position)
	{
		graphic.transform.localPosition = position;
	}

	public unsafe override void ReceiveClickDrag(Vector2 dragDelta)
	{
		if (horizontal)
		{
			dragDelta.y = 0f;
			dragDelta.x = 0f - dragDelta.x;
			parent.ScrollToScrollbarPositionX(graphic.transform.localPosition - new Vector3(dragDelta.x, dragDelta.y));
		}
		else
		{
			dragDelta.x = 0f;
			dragDelta.y = 0f - dragDelta.y;
			parent.ScrollToScrollbarPositionY(graphic.transform.localPosition - new Vector3(dragDelta.x, dragDelta.y));
		}
	}

	public Scrollbar()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		// TODO(verify): lost struct initializer (decomp: ControllerNav = (ControllerNavigation)0;)
	}
}
