using Cpp2ILInjected;
using UnityEngine;

public class TargetFlashlight : MonoBehaviour
{
	private const float DOUBLE_CLICK_TIME = 0.3f;

	private bool isDragging;

	private float timeLastClick;

	private Controller myController= new Controller();

	private Collider2D col;

	private void Awake()
	{
		col = GetComponent<Collider2D>();
	}

	private unsafe void Update()
	{
		myController.Update();
		switch (myController.CheckDrag(col))
		{
		case DragState.TouchStart:
			isDragging = true;
			break;
		case DragState.Dragging:
		{
			Vector2 dragPosition = myController.DragPosition;
			transform.position = new Vector3(dragPosition.x, dragPosition.y, transform.position.z);
			break;
		}
		case DragState.Released:
			isDragging = false;
			break;
		}
		if (!isDragging)
		{
			Controller.TouchState[] touches = myController.Touches;
			foreach (Controller.TouchState touchState in touches)
			{
				if (touchState.TouchStart)
				{
					if (Time.time - timeLastClick <= 0.3f)
					{
						Vector2 position = touchState.Position;
						transform.position = new Vector3(position.x, position.y, transform.position.z);
					}
					timeLastClick = Time.time;
				}
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(vector.x, vector.y, transform.position.z);
		}
	}

	public TargetFlashlight()
	{ }
}
