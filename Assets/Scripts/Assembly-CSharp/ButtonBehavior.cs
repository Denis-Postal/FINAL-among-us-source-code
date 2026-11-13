using Object = UnityEngine.Object;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : UiElement
{
	public bool OnUp = true;

	public bool OnDown;

	public bool Repeat;

	public Button.ButtonClickedEvent OnClick= new Button.ButtonClickedEvent();

	private Controller myController= new Controller();

	private Collider2D[] colliders;

	private float downTime;

	public SpriteRenderer spriteRenderer;

	private bool checkedClickEvent;

	public void OnEnable()
	{
		colliders = GetComponents<Collider2D>();
		myController.Reset();
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (checkedClickEvent)
		{
			return;
		}
		checkedClickEvent = true;
		int persistentEventCount = OnClick.GetPersistentEventCount();
		CloseButtonConsoleBehaviour component = GetComponent<CloseButtonConsoleBehaviour>();
		string text = gameObject.name.ToLower();
		if ((bool)component)
		{
			return;
		}
		for (int i = 0; i < persistentEventCount; i++)
		{
			string persistentMethodName = OnClick.GetPersistentMethodName(i);
			if (text.Contains("close") && persistentMethodName.ToLower().Contains("close"))
			{
				gameObject.AddComponent<CloseButtonConsoleBehaviour>();
				break;
			}
		}
	}

	public void Update()
	{ /* spliced-from-2019 */
		myController.Update();
		Collider2D[] array = colliders;
		foreach (Collider2D coll in array)
		{
			switch (myController.CheckDrag(coll))
			{
			case DragState.TouchStart:
				if (OnDown)
				{
					OnClick.Invoke();
				}
				break;
			case DragState.Released:
				if (OnUp)
				{
					OnClick.Invoke();
				}
				break;
			case DragState.Dragging:
				if (Repeat)
				{
					downTime += Time.fixedDeltaTime;
					if (downTime >= 0.3f)
					{
						downTime = 0f;
						OnClick.Invoke();
					}
				}
				else
				{
					downTime = 0f;
				}
				break;
			}
		}
	}

	public void ReceiveClick()
	{
		OnClick.Invoke();
	}

	public ButtonBehavior()
	{ }
}
