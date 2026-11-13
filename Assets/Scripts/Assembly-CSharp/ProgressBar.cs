using System.Collections;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
	private const float CurrentCapScale = 1f;

	public float Value = 0.5f;

	public float CapValue = 0.5f;

	public float MaxValue = 1f;

	public float maskScale = 1f;

	public SpriteRenderer Mask;

	[Header("Cap")]
	public SpriteRenderer cap;

	public SpriteRenderer capGlow;

	public float capScale = 1f;

	public float capGlowSizePadding = 0.2f;

	private float lastValue = float.MinValue;

	private float lastCapDiff = float.MinValue;

	public float GlowAlpha
	{
		get
		{
			return cap.color.a;
		}
		set
		{
			cap.color = cap.color.SetAlpha(value);
			capGlow.color = cap.color.SetAlpha(value);
		}
	}

	public IEnumerator ScaleDownY(float duration)
	{
		yield return Effects.All(Effects.Lerp(duration, (float t) =>
		{
			Transform transform = cap.transform;
			Vector3 localPosition = transform.localPosition;
			localPosition = new Vector3(localPosition.x, Mathf.Lerp(0f, (0f - cap.size.y) / 2f, Effects.ExpOut(t)), localPosition.z);
			transform.localPosition = localPosition;
			Vector3 localScale = transform.localScale;
			localScale = new Vector3(localScale.x, Mathf.Lerp(1f, 0f, Effects.ExpOut(t)), localScale.z);
			transform.localScale = localScale;
		}));
	}

	public void ResetScale()
	{
		cap.transform.localScale = Vector3.one;
	}

	private void Update()
	{
		if (MaxValue != 0f)
		{
			float num = Value / MaxValue * maskScale;
			SetFillWidth(num);
			SetCapWidth(num);
		}
	}

	private void SetFillWidth(float xScale)
	{
		if (lastValue != Value)
		{
			lastValue = Value;
			Transform transform = Mask.transform;
			Vector3 localScale = transform.localScale;
			localScale.x = xScale;
			transform.localScale = localScale;
			Vector3 localPosition = transform.localPosition;
			localPosition.x = (0f - Mask.sprite.bounds.size.x) * (maskScale - xScale) / 2f;
			transform.localPosition = localPosition;
		}
	}

	private void SetCapWidth(float xScale)
	{
		CapValue = Mathf.Max(CapValue, lastValue);
		if (!Mathf.Approximately(lastCapDiff, CapValue - Value) && (bool)cap)
		{
			lastCapDiff = CapValue - Value;
			Transform transform = cap.transform;
			Vector2 size = new Vector2(lastCapDiff / MaxValue * capScale, cap.size.y);
			cap.size = size;
			capGlow.size = new Vector2(size.x + capGlowSizePadding, capGlow.size.y);
			if (size.x > 0f)
			{
				float num = xScale * Mask.sprite.bounds.size.x;
				float num2 = size.x * (capScale / size.x - 1f);
				transform.localPosition = new Vector3(num - num2 / 2f, 0f, 0f);
			}
		}
	}
}
