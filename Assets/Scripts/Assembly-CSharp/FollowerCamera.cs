using System;
using System.Collections;
using AmongUs.Data;
using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
	public MonoBehaviour Target;

	public Vector2 Offset;

	public bool Locked;

	public float shakeAmount;

	public float shakePeriod = 1f;

	[NonSerialized]
	public bool OverrideScreenShakeEnabled = true;

	private Vector2 centerPosition;

	public void Update()
	{
		if ((bool)Target && !Locked)
		{
			centerPosition = Vector3.Lerp(centerPosition, Target.transform.position + (Vector3)Offset, 5f * Time.deltaTime);
			Vector2 vector = centerPosition;
			if (shakeAmount > 0f && DataManager.Settings.Gameplay.ScreenShake && OverrideScreenShakeEnabled)
			{
				float num = Time.fixedTime * shakePeriod;
				float num2 = Mathf.PerlinNoise(0.5f, num) * 2f - 1f;
				float num3 = Mathf.PerlinNoise(num, 0.5f) * 2f - 1f;
				vector.x += num2 * shakeAmount;
				vector.y += num3 * shakeAmount;
			}
			transform.position = vector;
		}
	}

	public void ShakeScreen(float duration, float severity)
	{
		if (DataManager.Settings.Gameplay.ScreenShake)
		{
			StartCoroutine(CoShakeScreen(duration, severity));
		}
	}

	private IEnumerator CoShakeScreen(float duration, float severity)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		for (float t = duration; t > 0f; t -= Time.fixedDeltaTime)
		{
			float num = t / duration;
			Offset = UnityEngine.Random.insideUnitCircle * num * severity;
			yield return wait;
		}
		Offset = Vector2.zero;
	}

	internal void SetTarget(MonoBehaviour target)
	{
		Target = target;
		SnapToTarget();
	}

	public void SnapToTarget()
	{
		transform.position = Target.transform.position + (Vector3)Offset;
	}
}
