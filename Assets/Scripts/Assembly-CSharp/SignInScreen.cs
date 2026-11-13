using TMPro;
using UnityEngine;

public class SignInScreen : MonoBehaviour
{
	public PassiveButton signInButton;

	public GameObject ppButton;

	public GameObject tosButton;

	public GameObject TermsOfUsePlayStationPopup;

	public TextMeshPro TermsOfUsePlayStationEULAText;

	private void Awake()
	{
	}

	public bool IsOpen()
	{
		return gameObject.activeSelf;
	}

	public void Open()
	{
		gameObject.SetActive(value: true);
	}

	public void OnClose()
	{
		gameObject.SetActive(value: false);
	}

	public void OpenPrivacyPolicy()
	{
		Constants.OpenURL(Constants.PrivacyPolicyURL);
	}

	public void OpenTermsOfUse()
	{
		ReallyOpenTermsOfUse();
	}

	public void ReallyOpenTermsOfUse()
	{
		Constants.OpenURL(Constants.TermsOfUseURL);
	}
}
