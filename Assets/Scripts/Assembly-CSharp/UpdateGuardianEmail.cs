using System.Collections;
using AmongUs.Data;
using TMPro;
using UnityEngine;

public class UpdateGuardianEmail : MonoBehaviour
{
	public EmailTextBehaviour emailText;

	public EmailTextBehaviour emailConfirmText;

	public TextMeshPro missingEmailInfo;

	public GameObject cancelButton;

	public IEnumerator Show()
	{
		gameObject.SetActive(value: true);
		missingEmailInfo.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.NeedPermissionText);
		cancelButton.SetActive(value: true);
		while (gameObject.activeSelf)
		{
			yield return null;
		}
	}

	public IEnumerator ShowMissingEmail()
	{
		gameObject.SetActive(value: true);
		missingEmailInfo.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MissingGuardianEmailText);
		cancelButton.SetActive(value: false);
		while (gameObject.activeSelf)
		{
			yield return null;
		}
	}

	public void Close()
	{
		GetComponent<TransitionOpen>().Close();
	}

	public void SendUpdateGuardianEmail()
	{
		if (!emailText.ShakeIfInvalid() && !emailConfirmText.ShakeIfInvalid() && !emailConfirmText.ShakeIfDoesntMatch(emailText.nameSource.text, emailConfirmText.nameSource.text))
		{
			string emailValidEmail = emailText.GetEmailValidEmail();
			DataManager.Player.Account.GuardianEmail = emailValidEmail;
			DataManager.Player.Save();
			DestroyableSingleton<EOSManager>.Instance.UpdateGuardianEmail();
			DestroyableSingleton<AccountManager>.Instance.ShowGuardianEmailSentConfirm();
			Close();
		}
	}
}
