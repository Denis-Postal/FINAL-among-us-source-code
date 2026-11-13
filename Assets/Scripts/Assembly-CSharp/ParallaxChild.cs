using UnityEngine;

public class ParallaxChild : MonoBehaviour
{
	[HideInInspector]
	public Vector3 BasePosition;

	public void OnEnable()
	{
		BasePosition = transform.localPosition;
	}
}
