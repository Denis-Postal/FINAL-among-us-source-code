using Cpp2ILInjected;
using UnityEngine;

public class HorizontalGauge : MonoBehaviour
{
	public float Value = 0.5f;

	public float MaxValue = 1f;

	public float maskScale = 1f;

	public SpriteMask Mask;

	public SpriteRenderer MaskArea;

	private float lastValue = float.MinValue;

	public unsafe void Update()
	{
		if (MaxValue != 0f && lastValue != Value)
		{
			lastValue = Value;
			float num = lastValue / MaxValue * maskScale;
			if ((bool)Mask)
			{
				Mask.transform.localScale = new Vector3(num, Mask.transform.localScale.y, 1f);
				Mask.transform.localPosition = new Vector3((0f - Mask.sprite.bounds.size.x) * (maskScale - num) / 2f, 0f, 0f);
			}
			if ((bool)MaskArea)
			{
				MaskArea.transform.localScale = new Vector3(num, MaskArea.transform.localScale.y, 1f);
				MaskArea.transform.localPosition = new Vector3((0f - MaskArea.size.x) * (maskScale - num) / 2f, 0f, 0f);
			}
		}
	}
}
