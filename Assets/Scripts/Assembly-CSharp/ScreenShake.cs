using System.Collections;
using AmongUs.Data;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
	private Vector3 originalPos;

	private float intensity;

	private static ScreenShake instance;

	public static ScreenShake Instance => instance;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Object.Destroy(gameObject);
		}
	}

	public void ShakeCamera(float duration, float _intensity)
	{
		if (DataManager.Settings.Gameplay.ScreenShake)
		{
			intensity = _intensity;
			StopCoroutine("CoShakeCamera");
			StartCoroutine("CoShakeCamera", duration);
		}
	}

	public IEnumerator CoShakeCamera(float duration)
	{
		originalPos = transform.position;
		float timeShaken = 0f;
		while (timeShaken < duration)
		{
			transform.position = originalPos + Random.insideUnitSphere * intensity;
			timeShaken += Time.deltaTime;
			yield return null;
		}
		transform.position = originalPos;
	}
}
