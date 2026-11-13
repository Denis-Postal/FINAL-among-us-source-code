using System;
using System.Collections;
using UnityEngine;

public class TransitionFade : DestroyableSingleton<TransitionFade>
{
	public SpriteRenderer overlay;

	public void DoTransitionFade(GameObject transitionFrom, GameObject transitionTo, Action callback)
	{
		transitionTo?.SetActive(value: false);
		StartCoroutine(CoShow(transitionFrom, transitionTo, callback));
	}

	private IEnumerator CoShow(GameObject transitionFrom, GameObject transitionTo, Action callback)
	{
		yield return Effects.ColorFade(overlay, Color.clear, Color.black, 0.1f);
		if ((bool)transitionFrom && (bool)transitionFrom.gameObject)
		{
			transitionFrom?.gameObject.SetActive(value: false);
		}
		if ((bool)transitionTo && (bool)transitionTo.gameObject)
		{
			transitionTo?.gameObject.SetActive(value: true);
		}
		yield return null;
		yield return Effects.ColorFade(overlay, Color.black, Color.clear, 0.1f);
		callback?.Invoke();
	}
}
