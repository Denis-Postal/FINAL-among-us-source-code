using UnityEngine;

public class DialBehaviour : MonoBehaviour
{
	public FloatRange DialRange;

	public Collider2D collider;

	public Controller myController= new Controller();

	public float Value;

	public bool Engaged;

	public Transform DialTrans;

	public Transform DialShadTrans;

	public void Update()
	{
		Engaged = false;
		myController.Update();
		if (myController.CheckDrag(collider) == DragState.Dragging)
		{
			Vector2 vector = myController.DragPosition - (Vector2)transform.position;
			float num = Vector2.up.AngleSigned(vector);
			if (num < -180f)
			{
				num += 360f;
			}
			num = DialRange.Clamp(num);
			SetValue(num);
			Engaged = true;
		}
	}

	public unsafe void SetValue(float angle)
	{ /* spliced-from-2019 */
		Value = angle;
		Vector3 localEulerAngles = new Vector3(0f, 0f, angle);
		DialTrans.localEulerAngles = localEulerAngles;
		DialShadTrans.localEulerAngles = localEulerAngles;
	}

	public DialBehaviour()
	{ }
}
