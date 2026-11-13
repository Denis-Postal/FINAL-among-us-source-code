using System.Collections;
using PowerTools;
using UnityEngine;

public class MushroomWallMushroom : MonoBehaviour
{
	[SerializeField]
	private SpriteAnim animator;

	[SerializeField]
	private SpriteRenderer sprite;

	[SerializeField]
	private AnimationClip showClip;

	[SerializeField]
	private AnimationClip hideClip;

	private Coroutine hideCoroutine;

	public void Show()
	{
		sprite.enabled = true;
		if (hideCoroutine != null)
		{
			StopCoroutine(hideCoroutine);
		}
		animator.Play(showClip);
	}

	public void Hide()
	{
		hideCoroutine = StartCoroutine(CoHideAndThenDisableSprite());
	}

	private IEnumerator CoHideAndThenDisableSprite()
	{
		yield return new WaitForAnimationFinish(animator, hideClip);
		sprite.enabled = false;
		hideCoroutine = null;
	}
}
