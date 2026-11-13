using Cpp2ILInjected;
using UnityEngine;

public class FungleSunsetParallax : MonoBehaviour
{
	[SerializeField]
	private Transform content;

	[SerializeField]
	private Transform parallaxRoot;

	[SerializeField]
	private Vector2 parallaxMultiplier;

	private Transform camera;

	private Vector3 origin;

	private void Awake()
	{
		origin = content.position;
	}

	private unsafe void LateUpdate()
	{
		if (camera == null)
		{
			if (Camera.main != null)
			{
				camera = Camera.main.transform;
			}
			if (camera == null)
			{
				return;
			}
		}
		Vector2 a = parallaxRoot.position - camera.position;
		content.position = origin - (Vector3)a.Mul(parallaxMultiplier);
	}
}
