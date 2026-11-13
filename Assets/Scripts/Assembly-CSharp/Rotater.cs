using UnityEngine;

public class Rotater : MonoBehaviour
{
	public float DegreesPerSecond = 360f;

	private void Update()
	{
		transform.localEulerAngles = new Vector3(0f, 0f, Time.time * DegreesPerSecond);
	}
}
