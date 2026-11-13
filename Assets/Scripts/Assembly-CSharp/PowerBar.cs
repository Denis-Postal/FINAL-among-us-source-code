using Object = UnityEngine.Object;
using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class PowerBar : MonoBehaviour
{
	public SpriteRenderer SegmentPrefab;

	public Sprite greenImage;

	public Sprite yellowImage;

	public Sprite redImage;

	public Sprite greenEmptyImage;

	public Sprite yellowEmptyImage;

	public Sprite redEmptyImage;

	public int numberGreen = 11;

	public int numberYellow = 6;

	public int numberRed = 3;

	public float Width;

	public float value = 0.5f;

	private SpriteRenderer[] Segments;

	public int NumSegments => numberGreen + numberRed + numberYellow;

	public unsafe void Awake()
	{
		Segments = new SpriteRenderer[NumSegments];
		float num = Width / (float)(NumSegments - 1);
		float num2 = Width / -2f;
		for (int i = 0; i < Segments.Length; i++)
		{
			SpriteRenderer spriteRenderer = (Segments[i] = Object.Instantiate(SegmentPrefab, transform));
			spriteRenderer.transform.localPosition = new Vector3(num2 + (float)i * num, 0f, 0.0001f);
			if (i < numberGreen)
			{
				spriteRenderer.sprite = greenImage;
			}
			else if (i < numberGreen + numberYellow)
			{
				spriteRenderer.sprite = yellowImage;
			}
			else
			{
				spriteRenderer.sprite = redImage;
			}
		}
	}

	public void SetValue(float value)
	{
		this.value = value;
		Update();
	}

	public unsafe void Update()
	{
		for (int i = 0; i < Segments.Length; i++)
		{
			SpriteRenderer spriteRenderer = Segments[i];
			if ((float)i < value * (float)NumSegments)
			{
				if (i < numberGreen)
				{
					spriteRenderer.sprite = greenImage;
				}
				else if (i < numberGreen + numberYellow)
				{
					spriteRenderer.sprite = yellowImage;
				}
				else
				{
					spriteRenderer.sprite = redImage;
				}
				spriteRenderer.color = Color.white;
			}
			else
			{
				if (i < numberGreen)
				{
					spriteRenderer.sprite = greenEmptyImage;
				}
				else if (i < numberGreen + numberYellow)
				{
					spriteRenderer.sprite = yellowEmptyImage;
				}
				else
				{
					spriteRenderer.sprite = redEmptyImage;
				}
				spriteRenderer.color = Color.gray;
			}
		}
	}
}
