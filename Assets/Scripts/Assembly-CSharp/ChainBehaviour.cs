using System;
using Cpp2ILInjected;
using UnityEngine;

public class ChainBehaviour : MonoBehaviour
{
	public FloatRange SwingRange= new FloatRange(0f, 30f);

	public float SwingPeriod= 2f;

	public float swingTime;

	private Vector3 vec;

	public unsafe void Awake()
	{
		swingTime = FloatRange.Next(0f, SwingPeriod);
		vec.z = SwingRange.Lerp(Mathf.Sin(swingTime));
		transform.eulerAngles = vec;
	}

	public unsafe void Update()
	{
		swingTime += Time.deltaTime / SwingPeriod;
		vec.z = SwingRange.Lerp(Mathf.Sin(swingTime * MathF.PI) / 2f + 0.5f);
		transform.eulerAngles = vec;
	}

	public ChainBehaviour()
	{ }
}
