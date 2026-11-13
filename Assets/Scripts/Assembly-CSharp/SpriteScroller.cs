using UnityEngine;

public class SpriteScroller : MonoBehaviour
{
	public Renderer rend;

	public float XRate = 1f;

	public float YRate = 1f;

	private void Update()
	{
		if ((bool)rend)
		{
			rend.material.SetTextureOffset("_MainTex", new Vector2(Time.time * XRate, Time.time * YRate));
		}
	}
}
