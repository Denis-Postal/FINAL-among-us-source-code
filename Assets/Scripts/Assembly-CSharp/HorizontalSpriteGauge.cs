using Cpp2ILInjected;
using UnityEngine;

public class HorizontalSpriteGauge : MonoBehaviour
{
	public float Value = 0.5f;

	public float MaxValue = 1f;

	public float maskScale = 1f;

	public SpriteRenderer Mask;

	private float lastValue = float.MinValue;

	public unsafe void Update()
	{
		if (MaxValue != 0f && lastValue != Value)
		{
			lastValue = Value;
			float num = lastValue / MaxValue * maskScale;
			Vector3 localScale = Mask.transform.localScale;
			localScale.x = num;
			Mask.transform.localScale = localScale;
			Vector3 localPosition = Mask.transform.localPosition;
			localPosition.x = (0f - Mask.sprite.bounds.size.x) * (maskScale - num) / 2f;
			Mask.transform.localPosition = localPosition;
		}
	}
}
