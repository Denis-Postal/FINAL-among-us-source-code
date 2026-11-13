using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class PingBehaviour : ArrowBehaviour
{
	[SerializeField]
	private AudioClip soundOnEnable;

	public bool AmSeeker;

	public override void Awake()
	{
		base.Awake();
		minDistanceToShowArrow = 0f;
	}

	public unsafe override void UpdatePosition()
	{
		Camera main = Camera.main;
		Vector2 del = target - main.transform.position;
		float num = del.magnitude / (main.orthographicSize * perc);
		if (image != null)
		{
			image.enabled = num > minDistanceToShowArrow;
		}
		Vector2 vpPoint = main.WorldToViewportPoint(target);
		if (AmSeeker && Between(vpPoint.x, 0f, 1f) && Between(vpPoint.y, 0f, 1f))
		{
			CloseBehaviour(del, num);
		}
		else
		{
			DistancedBehaviour(vpPoint, del, num, main);
		}
	}

	protected unsafe override void CloseBehaviour(Vector2 del, float delLen)
	{
		transform.position = (Vector2)target;
		float num = Mathf.Clamp(delLen, 0f, MaxScale);
		transform.localScale = new Vector3(num, num, num);
	}

	public override void SetImageEnabled(bool isOn)
	{
		base.SetImageEnabled(isOn);
		if (image.enabled)
		{
			SoundManager.Instance.PlaySound(soundOnEnable, loop: false);
		}
	}
}
