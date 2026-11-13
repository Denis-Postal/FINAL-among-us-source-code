using System.Collections;
using PowerTools;
using UnityEngine;

public class HandZiplinePoolable : PoolableBehavior
{
	[SerializeField]
	private SpriteRenderer handRenderer;

	[SerializeField]
	private SpriteAnim spriteAnim;

	[SerializeField]
	private AnimationClip DownIntroAnim;

	[SerializeField]
	private AnimationClip DownLoopAnim;

	[SerializeField]
	private AnimationClip DownOutroAnim;

	[SerializeField]
	private AnimationClip UpIntroAnim;

	[SerializeField]
	private AnimationClip UpLoopAnim;

	[SerializeField]
	private AnimationClip UpOutroAnim;

	public void StartUpAnimation()
	{
		StartCoroutine(CoUpAnimation());
	}

	public void StartUpOutroAnimation()
	{
		StartCoroutine(CoUpOutroAnimation());
	}

	public void StartDownAnimation()
	{
		StartCoroutine(CoDownAnimation());
	}

	public void StartDownOutroAnimation()
	{
		StartCoroutine(CoDownOutroAnimation());
	}

	public void SetPlayerColor(NetworkedPlayerInfo.PlayerOutfit outfit, PlayerMaterial.MaskType maskType, float alpha)
	{
		handRenderer.sharedMaterial = CosmeticsLayer.GetBodyMaterial(maskType);
		PlayerMaterial.SetColors(outfit.ColorId, handRenderer);
		handRenderer.color = handRenderer.color.SetAlpha(alpha);
	}

	private IEnumerator CoDownAnimation()
	{
		spriteAnim.Play(DownIntroAnim);
		yield return new WaitForAnimationFinish(spriteAnim, DownIntroAnim);
		spriteAnim.Play(DownLoopAnim);
	}

	private IEnumerator CoDownOutroAnimation()
	{
		spriteAnim.Play(DownOutroAnim);
		yield return new WaitForAnimationFinish(spriteAnim, DownOutroAnim);
		gameObject.SetActive(value: false);
	}

	private IEnumerator CoUpAnimation()
	{
		spriteAnim.Play(UpIntroAnim);
		yield return new WaitForAnimationFinish(spriteAnim, UpIntroAnim);
		spriteAnim.Play(UpLoopAnim);
	}

	private IEnumerator CoUpOutroAnimation()
	{
		spriteAnim.Play(UpOutroAnim);
		yield return new WaitForAnimationFinish(spriteAnim, UpOutroAnim);
		gameObject.SetActive(value: false);
	}

	public void Hide()
	{
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = Color.clear;
		}
	}
}
