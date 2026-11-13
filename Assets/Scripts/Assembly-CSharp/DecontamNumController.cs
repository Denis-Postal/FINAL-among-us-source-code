using System;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using UnityEngine;

public class DecontamNumController : MonoBehaviour
{
	public SpriteRenderer[] Images;

	public Sprite[] NumImages;

	public unsafe void SpaceEvenly()
	{ /* spliced-from-2019 */
		List<float> list = FloatRange.SpreadToEdges(-2.9f, 2.9f, Images.Length).ToList();
		for (int i = 0; i < Images.Length; i++)
		{
			Images[i].transform.localPosition = new Vector3(0f, list[i], 0.1f);
		}
	}

	internal void SetSecond(float curSecond, float maxSecond)
	{ /* spliced-from-2019 */
		for (int i = 0; i < Images.Length; i++)
		{
			int num = Mathf.CeilToInt(Mathf.Lerp(0f, NumImages.Length - 1, curSecond / maxSecond));
			Images[i].sprite = NumImages[num];
		}
	}
}
