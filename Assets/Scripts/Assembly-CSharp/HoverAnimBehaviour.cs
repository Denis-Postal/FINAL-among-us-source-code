using Cpp2ILInjected;
using UnityEngine;

public class HoverAnimBehaviour : MonoBehaviour
{
	public FloatRange YMovement;

	public float Speed = 1f;

	public float Shift = 1f;

	private float offset;

	private void Start()
	{
		offset = FloatRange.Next(0f, 10f);
	}

	private unsafe void Update()
	{
		Vector3 localPosition = transform.localPosition;
		float v = Mathf.Sin((Time.time + offset) * Speed) / 2f + Shift;
		localPosition.y = YMovement.Lerp(v);
		transform.localPosition = localPosition;
	}
}
