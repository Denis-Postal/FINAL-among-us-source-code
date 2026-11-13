using Cpp2ILInjected;
using UnityEngine;

public class AutoParallaxController : MonoBehaviour
{
	public ParallaxChild[] Children;

	public float XScale = 1f;

	public float YScale = 1f;

	public void Start()
	{
		Children = GetComponentsInChildren<ParallaxChild>();
	}

	public unsafe void Update()
	{
		if (!PlayerControl.LocalPlayer)
		{
			return;
		}
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		for (int i = 0; i < Children.Length; i++)
		{
			ParallaxChild obj = Children[i];
			Vector3 basePosition = obj.BasePosition;
			if (basePosition.z < 0f)
			{
				basePosition.z = 0f - basePosition.z;
			}
			basePosition.x -= truePosition.x / (basePosition.z * XScale);
			basePosition.y -= truePosition.y / (basePosition.z * YScale);
			obj.transform.localPosition = basePosition;
		}
	}
}
