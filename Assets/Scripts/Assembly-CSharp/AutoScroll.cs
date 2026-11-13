using UnityEngine;

public class AutoScroll : MonoBehaviour
{
	public Transform t;

	public float speed;

	public float stoppingPoint;

	public float initialDelay = 2f;

	public bool FinishedScrolling;

	private unsafe void Start()
	{
		t.localPosition = Vector3.zero;
		initialDelay = 2f;
	}

	private unsafe void OnEnable()
	{
		FinishedScrolling = false;
		t.localPosition = Vector3.zero;
		initialDelay = 2f;
	}

	private unsafe void Update()
	{
		if (initialDelay > 0f)
		{
			initialDelay -= Time.deltaTime;
			return;
		}
		if (t.localPosition.y < stoppingPoint)
		{
			t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y + Time.deltaTime * speed, t.localPosition.z);
		}
		else
		{
			FinishedScrolling = true;
		}
		if (t.localPosition.y > stoppingPoint)
		{
			t.localPosition = new Vector3(t.localPosition.x, stoppingPoint, t.localPosition.z);
		}
	}
}
