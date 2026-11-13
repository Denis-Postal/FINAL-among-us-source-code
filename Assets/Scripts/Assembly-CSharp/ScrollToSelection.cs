using Object = UnityEngine.Object;
using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

[RequireComponent(typeof(Scroller))]
public class ScrollToSelection : MonoBehaviour
{
	[SerializeField]
	private float scrollOffset;

	private Scroller scrollRect;

	public bool manuallyScrolling;

	private UIScrollbarHelper[] childElements;

	public Vector2 wantedValue;

	public GameObject lastSelectedObject;

	public bool cursorEnabled;

	public bool killScroll;

	public bool onePage;

	private void Awake()
	{
		scrollRect = GetComponent<Scroller>();
	}

	private void OnEnable()
	{
		childElements = GetComponentsInChildren<UIScrollbarHelper>(includeInactive: false);
		scrollRect = GetComponent<Scroller>();
		wantedValue = Vector2.zero;
		ClearSelections();
		for (int i = 0; i < childElements.Length; i++)
		{
			childElements[i].index = i;
			childElements[i].isDirectChild = childElements[i].transform.parent == transform;
		}
	}

	private void Start()
	{
		childElements = GetComponentsInChildren<UIScrollbarHelper>(includeInactive: false);
		for (int i = 0; i < childElements.Length; i++)
		{
			childElements[i].index = i;
			childElements[i].isDirectChild = childElements[i].transform.parent == scrollRect.Inner;
		}
	}

	public void Toggle(bool on)
	{
		manuallyScrolling = !on;
	}

	public void ClearSelections()
	{
		lastSelectedObject = null;
	}

	private unsafe void LateUpdate()
	{
		if (!enabled)
		{
			return;
		}
		if (!scrollRect.enabled)
		{
			onePage = true;
			return;
		}
		cursorEnabled = Controller.currentTouchType != Controller.TouchType.Joystick;
		if (cursorEnabled)
		{
			wantedValue = scrollRect.Inner.localPosition;
			return;
		}
		onePage = false;
		GameObject gameObject = (ControllerManager.Instance.CurrentUiState.CurrentSelection ? ControllerManager.Instance.CurrentUiState.CurrentSelection.gameObject : null);
		if (lastSelectedObject != null && lastSelectedObject != gameObject && manuallyScrolling)
		{
			Toggle(on: true);
		}
		if (lastSelectedObject != gameObject)
		{
			if ((bool)gameObject && gameObject.transform.IsChildOf(transform))
			{
				ScrollToRect(gameObject.transform);
			}
			lastSelectedObject = gameObject;
			if (!gameObject)
			{
				killScroll = true;
			}
		}
		if (manuallyScrolling)
		{
			return;
		}
		if (scrollRect.allowY)
		{
			if (!killScroll && scrollRect.Inner.localPosition.y != wantedValue.y)
			{
				Vector3 localPosition = scrollRect.Inner.localPosition;
				localPosition.y = Mathf.Lerp(localPosition.y, wantedValue.y, Time.unscaledDeltaTime * 3f);
				scrollRect.Inner.localPosition = localPosition;
				scrollRect.UpdateScrollBars();
			}
			else if (killScroll)
			{
				wantedValue.y = scrollRect.Inner.localPosition.y;
			}
		}
		if (scrollRect.allowX)
		{
			if (!killScroll && scrollRect.Inner.localPosition.x != wantedValue.x)
			{
				Vector3 localPosition2 = scrollRect.Inner.localPosition;
				localPosition2.x = Mathf.Lerp(localPosition2.x, wantedValue.x, Time.unscaledDeltaTime * 3f);
				scrollRect.Inner.localPosition = localPosition2;
				scrollRect.UpdateScrollBars();
			}
			else if (killScroll)
			{
				wantedValue.x = scrollRect.Inner.localPosition.x;
			}
		}
	}

	private void ScrollToRect(Transform targetRectTransform)
	{
		if (manuallyScrolling || (!scrollRect.allowY && !scrollRect.allowX))
		{
			return;
		}
		UIScrollbarHelper uIScrollbarHelper = targetRectTransform.GetComponent<UIScrollbarHelper>();
		if (uIScrollbarHelper == null)
		{
			uIScrollbarHelper = targetRectTransform.GetComponentInParent<UIScrollbarHelper>();
		}
		Vector3 vector = Vector3.zero;
		if (scrollRect.Colliders.Length != 0)
		{
			Transform transform = scrollRect.Colliders[0].transform;
			if (transform.parent == base.transform)
			{
				vector = scrollRect.Colliders[0].transform.localPosition;
			}
			else if (transform != scrollRect.transform)
			{
				Vector3 position = scrollRect.Colliders[0].transform.position;
				vector = base.transform.InverseTransformPoint(position);
			}
		}
		if (uIScrollbarHelper == null)
		{
			wantedValue = -scrollRect.Inner.transform.localPosition + vector;
			killScroll = true;
			return;
		}
		if (childElements.Length == 0)
		{
			childElements = GetComponentsInChildren<UIScrollbarHelper>(includeInactive: false);
			for (int i = 0; i < childElements.Length; i++)
			{
				childElements[i].index = i;
			}
			if (childElements.Length == 0)
			{
				return;
			}
		}
		killScroll = false;
		Vector2 localPosition = uIScrollbarHelper.GetLocalPosition(scrollRect);
		if (scrollRect.allowY)
		{
			wantedValue.y = Mathf.Clamp(0f - localPosition.y + vector.y, scrollRect.GetYBounds().min, scrollRect.GetYBounds().max) + scrollOffset;
		}
		if (scrollRect.allowX)
		{
			wantedValue.x = Mathf.Clamp(0f - localPosition.x + vector.x, scrollRect.GetXBounds().min, scrollRect.GetXBounds().max);
		}
	}
}
