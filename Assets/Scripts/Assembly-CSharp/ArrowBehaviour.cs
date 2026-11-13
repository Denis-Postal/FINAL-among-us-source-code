using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;

public class ArrowBehaviour : PoolableBehavior
{
	public Vector3 target;

	public float perc = 0.925f;

	public float MaxScale = 1f;

	public bool alwaysMaxSize;

	protected float minDistanceToShowArrow = 0.3f;

	[HideInInspector]
	public SpriteRenderer image;

	public new virtual void Awake()
	{
		image = GetComponent<SpriteRenderer>();
	}

	public void Update()
	{
		// from the 2019 baseline Update; current build splits it into UpdatePosition (TODO(verify): Update body was a damaged decompile)
		UpdatePosition();
	}

	public unsafe virtual void UpdatePosition()
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
		transform.LookAt2d(target);
	}

	public virtual void SetImageEnabled(bool isOn)
	{
		if (image != null)
		{
			image.enabled = isOn;
		}
	}

	protected bool Between(float value, float min, float max)
	{
		if (value > min)
		{
			return value < max;
		}
		return false;
	}

	protected unsafe virtual void CloseBehaviour(Vector2 del, float delLen)
	{
		transform.position = (Vector2)target - del.normalized * 0.6f;
		if (alwaysMaxSize)
		{
			transform.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
			return;
		}
		float num = Mathf.Clamp(delLen, 0f, MaxScale);
		transform.localScale = new Vector3(num, num, num);
	}

	protected unsafe virtual void DistancedBehaviour(Vector2 vpPoint, Vector2 del, float delLen, Camera cam)
	{
		Vector2 vector = new Vector2(Mathf.Clamp(vpPoint.x * 2f - 1f, -1f, 1f), Mathf.Clamp(vpPoint.y * 2f - 1f, -1f, 1f));
		float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(cam);
		float num = safeOrthographicSize * cam.aspect;
		Vector3 vector2 = new Vector3(Mathf.LerpUnclamped(0f, num * 0.88f, vector.x), Mathf.LerpUnclamped(0f, safeOrthographicSize * 0.79f, vector.y), 0f);
		transform.position = cam.transform.position + vector2;
		transform.localScale = new Vector3(MaxScale, MaxScale, MaxScale);
	}
}
