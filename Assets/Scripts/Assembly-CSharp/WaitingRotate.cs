using UnityEngine;

public class WaitingRotate : MonoBehaviour
{
	public float speed = 20f;

	public float distanceFromBottom = 0.25f;

	public unsafe void Start()
	{
		Camera main = Camera.main;
		float num = main.orthographicSize * main.aspect * 1.1f;
		transform.localPosition = new Vector3(num - 1f, 0f - main.orthographicSize + distanceFromBottom, -1f);
	}

	public unsafe void Update()
	{
		transform.RotateAround(transform.position, Vector3.forward, speed * Time.deltaTime);
	}
}
