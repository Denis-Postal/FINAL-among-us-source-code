using Object = UnityEngine.Object;
using System;
using System.Collections;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class FungleTimer : MonoBehaviour
{
	private const string DONE_TIMER_TEXT = "!!";

	[SerializeField]
	private TextMeshPro timerText;

	public void Show()
	{
		gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		gameObject.SetActive(value: false);
	}

	public void SetValue(float taskTimer)
	{
		timerText.text = Mathf.CeilToInt(taskTimer).ToString();
	}

	public void SetDone()
	{
		timerText.text = "!!";
	}

	public void Shake()
	{
		StartCoroutine(Effects.Shake(transform, 0.5f, 0.1f, taper: true, returnToOrigin: true));
	}
}
