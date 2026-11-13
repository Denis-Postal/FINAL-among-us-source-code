using Cpp2ILInjected;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpriteParticleSystem : MonoBehaviour
{
	public Sprite sprite;

	public ParticleSystemRenderer ren;

	private MaterialPropertyBlock block;

	private void OnEnable()
	{
		block = new MaterialPropertyBlock();
		ren = GetComponent<Renderer>() as ParticleSystemRenderer;
		SetPropertyBlock();
	}

	private void SetPropertyBlock()
	{
		if (block == null)
		{
			block = new MaterialPropertyBlock();
		}
		ren.GetPropertyBlock(block);
		block.SetTexture("_MainTex", sprite.texture);
		ren.SetPropertyBlock(block);
	}

	private void OnValidate()
	{
		SetPropertyBlock();
	}
}
