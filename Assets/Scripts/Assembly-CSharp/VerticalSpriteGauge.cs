using Cpp2ILInjected;
using UnityEngine;

public class VerticalSpriteGauge : MonoBehaviour
{
	public float Value = 0.5f;

	public float MaxValue = 1f;

	public FloatRange YRange;

	public SpriteRenderer Mask;

	private float lastValue = float.MinValue;

	public float TopY { get; private set; }

	public unsafe void Update()
	{
		if (MaxValue != 0f && lastValue != Value)
		{
			lastValue = Value;
			Vector3 localPosition = Mask.transform.localPosition;
			YRange.Lerp(lastValue / MaxValue);
			Vector3 localScale = Mask.transform.localScale;
			localScale.y = lastValue / MaxValue * YRange.Width;
			Mask.transform.localScale = localScale;
			localPosition.y = YRange.min + localScale.y / 2f;
			Mask.transform.localPosition = localPosition;
			TopY = YRange.min + localScale.y;
		}
	}
}
