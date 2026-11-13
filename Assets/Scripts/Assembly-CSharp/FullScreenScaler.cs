using UnityEngine;

public class FullScreenScaler : MonoBehaviour
{
	public SpriteRenderer OptionalTarget;

	public bool ScaleWidth = true;

	public bool ScaleHeight= true;

	public unsafe void OnEnable()
	
	{
		Camera main = Camera.main;
		float num = main.orthographicSize * 2f;
		float num2 = num * main.aspect;
		if ((bool)OptionalTarget)
		{
			if (OptionalTarget.drawMode != SpriteDrawMode.Simple)
			{
				Vector3 vector = OptionalTarget.size;
				if (ScaleWidth)
				{
					vector.x = num2;
				}
				if (ScaleHeight)
				{
					vector.y = num;
				}
				OptionalTarget.size = vector;
				return;
			}
			Vector3 localScale = transform.localScale;
			if (ScaleWidth)
			{
				localScale.x = num2;
			}
			if (ScaleHeight)
			{
				localScale.y = num;
			}
			if ((bool)OptionalTarget.sprite)
			{
				Vector3 size = OptionalTarget.sprite.bounds.size;
				if (ScaleWidth)
				{
					localScale.x = num2 / size.x;
				}
				if (ScaleHeight)
				{
					localScale.y = num / size.y;
				}
			}
			transform.localScale = localScale;
		}
		else
		{
			Vector3 localScale2 = transform.localScale;
			if (ScaleWidth)
			{
				localScale2.x = num2;
			}
			if (ScaleHeight)
			{
				localScale2.y = num;
			}
			transform.localScale = localScale2;
		}
	}
}
