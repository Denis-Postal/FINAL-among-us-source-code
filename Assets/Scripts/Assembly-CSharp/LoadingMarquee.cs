using Cpp2ILInjected;
using UnityEngine;

public class LoadingMarquee : MonoBehaviour
{
	public float duration = 5f;

	public float distanceFromBottom = 0.25f;

	private FloatRange XRange;

	private float timer;

	public unsafe void Start()
	{
		Camera main = Camera.main;
		float num = main.orthographicSize * main.aspect * 1.1f;
		XRange = new FloatRange(num, 0f - num);
		transform.localPosition = new Vector3(num, 0f - main.orthographicSize + distanceFromBottom, -1f);
	}

	public unsafe void Update()
	{
		timer += Time.deltaTime;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = XRange.Lerp(timer / duration % 1f);
		transform.localPosition = localPosition;
	}
}
