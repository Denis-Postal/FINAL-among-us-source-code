using UnityEngine;

public class DangerMeter : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer[] barSprites;

	[SerializeField]
	private SpriteRenderer crewmateSeekerSprite;

	[SerializeField]
	private Color emptyColor;

	[SerializeField]
	private Color safeColor;

	[SerializeField]
	private Color cautionColor;

	[SerializeField]
	private Color dangerColor;

	private Vector3 originalLocalPosition;

	private Coroutine shakeCoroutine;

	public void SetDangerValue(float dangerLevel1, float dangerLevel2)
	{
		if (dangerLevel1 <= 0f)
		{
			SetFirstNBarColors(5, emptyColor);
		}
		else if (dangerLevel1 <= 0.25f)
		{
			SetFirstNBarColors(1, safeColor);
		}
		else if (dangerLevel1 > 0.25f && dangerLevel1 < 0.5f)
		{
			SetFirstNBarColors(2, safeColor);
		}
		else if (dangerLevel1 > 0.5f && dangerLevel2 <= 0f)
		{
			SetFirstNBarColors(3, cautionColor);
		}
		else if (dangerLevel2 > 0f && dangerLevel2 < 0.5f)
		{
			SetFirstNBarColors(4, cautionColor);
		}
		else
		{
			SetFirstNBarColors(5, dangerColor);
		}
		if (shakeCoroutine == null && dangerLevel2 > 0.5f)
		{
			originalLocalPosition = transform.localPosition;
			if (isActiveAndEnabled)
			{
				shakeCoroutine = StartCoroutine(Effects.ShakeForever(transform, 0.02f));
			}
		}
		else if (dangerLevel2 < 0.5f && shakeCoroutine != null)
		{
			StopCoroutine(shakeCoroutine);
			transform.localPosition = originalLocalPosition;
			shakeCoroutine = null;
		}
	}

	private void SetFirstNBarColors(int numBars, Color color)
	{
		for (int i = 0; i < barSprites.Length; i++)
		{
			if (i < numBars)
			{
				barSprites[i].color = color;
			}
			else
			{
				barSprites[i].color = emptyColor;
			}
		}
		if (color == Color.clear)
		{
			crewmateSeekerSprite.color = emptyColor;
		}
		else
		{
			crewmateSeekerSprite.color = color;
		}
	}
}
