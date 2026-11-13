using Object = UnityEngine.Object;
using System;
using TMPro;
using UnityEngine;

public class SplashErrorPopup : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro TitleText;

	[SerializeField]
	private TextMeshPro InfoText;

	[SerializeField]
	private AudioSource ShowSFX;

	public bool IsShowing => gameObject.activeSelf;

	public void Show(string titleText, string infoText)
	{
		if (!IsShowing)
		{
			TitleText.text = titleText;
			InfoText.text = infoText;
			gameObject.SetActive(value: true);
			ShowSFX.Play();
		}
	}

	public void OnButtonPressed()
	{
		Application.Quit();
	}
}
