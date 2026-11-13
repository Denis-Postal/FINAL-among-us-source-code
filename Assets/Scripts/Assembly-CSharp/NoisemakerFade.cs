using System.Collections;
using UnityEngine;

public class NoisemakerFade : MonoBehaviour
{
	[SerializeField]
	private NoisemakerArrow noisemakerArrow;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private void OnEnable()
	{
		noisemakerArrow.onFadeTrigger += Fade;
	}

	private void OnDestroy()
	{
		noisemakerArrow.onFadeTrigger -= Fade;
	}

	private void Fade()
	{
		StartCoroutine(CoFade());
	}

	private IEnumerator CoFade()
	{
		float timeElapsed = 0f;
		while (timeElapsed < 1f)
		{
			Color color = spriteRenderer.color;
			color.a = Mathf.Lerp(1f, 0f, timeElapsed / 1f);
			spriteRenderer.color = color;
			timeElapsed += Time.deltaTime;
			yield return null;
		}
	}
}
