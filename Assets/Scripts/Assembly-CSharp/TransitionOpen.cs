using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionOpen : MonoBehaviour
{
	public float duration = 0.2f;

	public Button.ButtonClickedEvent OnClose = new Button.ButtonClickedEvent();

	public Action OnFinishAnimOpen;

	[SerializeField]
	private float targetSize = 1f;

	public void OnEnable()
	{
		StartCoroutine(AnimateOpen());
	}

	public void Toggle()
	{
		StopAllCoroutines();
		if (isActiveAndEnabled)
		{
			Close();
			return;
		}
		gameObject.SetActive(value: true);
		OnEnable();
	}

	public void Close()
	{
		if (isActiveAndEnabled)
		{
			StartCoroutine(AnimateClose());
		}
	}

	private IEnumerator AnimateClose()
	{
		Vector3 vec = default;
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			float t2 = t / duration;
			float num = Mathf.SmoothStep(targetSize, 0f, t2);
			vec.Set(num, num, num);
			transform.localScale = vec;
			yield return null;
		}
		vec.Set(0f, 0f, 0f);
		transform.localScale = vec;
		OnClose.Invoke();
	}

	private IEnumerator AnimateOpen()
	{
		Vector3 vec = default;
		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			float t2 = t / duration;
			float num = Mathf.SmoothStep(0f, targetSize, t2);
			vec.Set(num, num, num);
			transform.localScale = vec;
			yield return null;
		}
		vec.Set(targetSize, targetSize, targetSize);
		transform.localScale = vec;
		OnFinishAnimOpen?.Invoke();
	}
}
