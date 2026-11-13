using System;
using Cpp2ILInjected;
using UnityEngine;

public class FungleSecurityCamera : MonoBehaviour
{
	public Action<Collision2D> OnCollision;

	public Action<Collision2D> OnExitCollision;

	[SerializeField]
	public Camera cam;

	[SerializeField]
	public Rigidbody2D rigidbody;

	public RenderTexture RenderTexture
	{
		set
		{
			cam.targetTexture = value;
		}
	}

	public float OrthographicSize
	{
		set
		{
			cam.orthographicSize = value;
		}
	}

	public bool CollisionsEnabled
	{
		get
		{
			return !rigidbody.isKinematic;
		}
		set
		{
			rigidbody.isKinematic = !value;
		}
	}

	public void Move(Vector3 delta)
	{
		if (CollisionsEnabled)
		{
			rigidbody.MovePosition(transform.position + delta);
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		OnCollision(col);
	}

	private void OnCollisionStay2D(Collision2D col)
	{
		OnCollision(col);
	}

	private void OnCollisionExit2D(Collision2D col)
	{
		OnExitCollision(col);
	}
}
