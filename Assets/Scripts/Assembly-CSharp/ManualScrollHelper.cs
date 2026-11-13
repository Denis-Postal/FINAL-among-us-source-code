using Cpp2ILInjected;
using Rewired;
using UnityEngine;

[RequireComponent(typeof(Scroller))]
public class ManualScrollHelper : MonoBehaviour
{
	public bool doVertical = true;

	public RewiredConstsEnum.Action verticalAxis;

	public bool doHorizontal;

	public RewiredConstsEnum.Action horizontalAxis;

	public float scrollSpeed = 3f;

	private ScrollToSelection scrollToSelection;

	private Scroller scroller;

	private void Start()
	{
		scroller = GetComponent<Scroller>();
		scrollToSelection = GetComponent<ScrollToSelection>();
		if ((bool)scrollToSelection)
		{
			scrollToSelection.Toggle(on: false);
		}
		if (!scroller)
		{
			enabled = false;
		}
	}

	private void Update()
	{
		Player player = ReInput.players.GetPlayer(0);
		Vector2 dragDelta = default;
		float axisRaw = player.GetAxisRaw((int)verticalAxis);
		float axisRaw2 = player.GetAxisRaw((int)horizontalAxis);
		if (doVertical)
		{
			dragDelta.y = (0f - axisRaw) * Time.deltaTime * scrollSpeed;
		}
		if (doHorizontal)
		{
			dragDelta.x = (0f - axisRaw2) * Time.deltaTime * scrollSpeed;
		}
		if (axisRaw != 0f || axisRaw2 != 0f)
		{
			if (scrollToSelection != null)
			{
				scrollToSelection.Toggle(on: false);
			}
			scroller.ScrollRelative(dragDelta);
		}
	}

	private void OnDisable()
	{
		if ((bool)scrollToSelection)
		{
			scrollToSelection.ClearSelections();
			scrollToSelection.enabled = false;
			scrollToSelection.Toggle(on: false);
		}
	}

	private void OnEnable()
	{
		if ((bool)scrollToSelection)
		{
			scrollToSelection.ClearSelections();
			scrollToSelection.enabled = true;
			scrollToSelection.Toggle(on: false);
		}
	}
}
