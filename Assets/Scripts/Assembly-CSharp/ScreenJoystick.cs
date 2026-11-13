using Cpp2ILInjected;
using UnityEngine;

public class ScreenJoystick : MonoBehaviour, IVirtualJoystick
{
	private Collider2D[] hitBuffer= new Collider2D[20];

	private Vector2 DeltaL_bf;

	private Controller myController= new Controller();

	private int touchId= -1;

	public Vector2 DeltaL { get; private set; }

	public Vector2 DeltaR => Vector2.zero;

	public void ToggleVisuals(bool on)
	{
	}

	private unsafe void FixedUpdate()
	{
		myController.Update();
		if (touchId > -1)
		{
			Controller.TouchState touchState = myController.Touches[touchId];
			if (touchState.IsDown)
			{
				Vector2 vector = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
				DeltaL = (touchState.Position - vector).normalized;
			}
			else
			{
				touchId = -1;
				DeltaL = Vector2.zero;
			}
			return;
		}
		for (int i = 0; i < myController.Touches.Length; i++)
		{
			Controller.TouchState touchState2 = myController.Touches[i];
			if (!touchState2.TouchStart)
			{
				continue;
			}
			bool flag = false;
			int num = Physics2D.OverlapPointNonAlloc(touchState2.Position, hitBuffer, Constants.NotShipMask);
			for (int j = 0; j < num; j++)
			{
				Collider2D collider2D = hitBuffer[j];
				if ((bool)collider2D.GetComponent<ButtonBehavior>() || (bool)collider2D.GetComponent<PassiveButton>())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				touchId = i;
				break;
			}
		}
	}

	public ScreenJoystick()
	{ }
}
