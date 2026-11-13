using System.Collections;
using UnityEngine;

public class PulseWaitingText : MonoBehaviour
{
	public Transform textSource;

	private float duration = 1f;

	private void Awake()
	{
		StartCoroutine(GetBig());
	}

	private IEnumerator GetBig()
	{
		Vector3 vec = default;
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			float t2 = t / duration;
			float num = Mathf.SmoothStep(1f, 1.25f, t2);
			vec.Set(num, num, num);
			transform.localScale = vec;
			yield return null;
		}
		vec.Set(0f, 0f, 0f);
		transform.localScale = vec;
		StartCoroutine(GetSmall());
	}

	private IEnumerator GetSmall()
	{
		Vector3 vec = default;
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			float t2 = t / duration;
			float num = Mathf.SmoothStep(1.25f, 1f, t2);
			vec.Set(num, num, num);
			transform.localScale = vec;
			yield return null;
		}
		vec.Set(1f, 1f, 1f);
		transform.localScale = vec;
		StartCoroutine(GetBig());
	}
}
