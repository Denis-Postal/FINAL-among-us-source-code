using System.Collections;
using UnityEngine;

public class NoisemakerArrow : ArrowBehaviour
{
	public delegate void OnFadeTrigger();

	public const float FadeLength = 1f;

	[SerializeField]
	private Transform pivot;

	private float duration;

	public event OnFadeTrigger onFadeTrigger;

	public override void UpdatePosition()
	{
		Camera main = Camera.main;
		Vector2 del = target - main.transform.position;
		float num = del.magnitude / (main.orthographicSize * perc);
		if (image != null)
		{
			image.enabled = num > minDistanceToShowArrow;
		}
		Vector2 vpPoint = main.WorldToViewportPoint(target);
		if (Between(vpPoint.x, 0f, 1f) && Between(vpPoint.y, 0f, 1f))
		{
			CloseBehaviour(del, num);
		}
		else
		{
			DistancedBehaviour(vpPoint, del, num, main);
		}
		pivot.LookAt2d(target);
	}

	protected override void DistancedBehaviour(Vector2 vpPoint, Vector2 del, float delLen, Camera cam)
	{
		Vector2 vector = new Vector2(Mathf.Clamp(vpPoint.x * 2f - 1f, -1f, 1f), Mathf.Clamp(vpPoint.y * 2f - 1f, -1f, 1f));
		float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(cam);
		float num = safeOrthographicSize * cam.aspect;
		Vector3 vector2 = new Vector3(Mathf.LerpUnclamped(0f, num * 0.8f, vector.x), Mathf.LerpUnclamped(0f, safeOrthographicSize * 0.7f, vector.y), 0f);
		transform.position = cam.transform.position + vector2;
		transform.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
	}

	public void SetDuration(float _duration)
	{
		duration = _duration;
		StartCoroutine(WaitDisappear());
	}

	private IEnumerator WaitDisappear()
	{
		bool fadeTriggered = false;
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			_ = t / duration;
			if (t > duration - 1f && !fadeTriggered)
			{
				fadeTriggered = true;
				onFadeTrigger();
			}
			yield return null;
		}
		Object.Destroy(transform.gameObject);
	}
}
