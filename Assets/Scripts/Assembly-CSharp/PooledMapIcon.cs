using System;
using Cpp2ILInjected;
using UnityEngine;

public class PooledMapIcon : PoolableBehavior
{
	public float NormalSize = 0.3f;

	public int lastMapTaskStep = -1;

	public SpriteRenderer rend;

	public AlphaPulse alphaPulse;

	public unsafe void Update()
	{
		if (alphaPulse.enabled)
		{
			float num = Mathf.Abs(Mathf.Cos((alphaPulse.Offset + Time.time) * MathF.PI / alphaPulse.Duration));
			if ((double)num > 0.9)
			{
				num -= 0.9f;
				num = NormalSize + num;
				transform.localScale = new Vector3(num, num, num);
			}
		}
	}

	public override void Reset()
	{ /* spliced-from-2019 */
		lastMapTaskStep = -1;
		alphaPulse.enabled = false;
		rend.material.SetFloat("_Outline", 0f);
		base.Reset();
	}

	public PooledMapIcon()
	{
		//IL_001a: Expected I4, but got I8
	}
}
