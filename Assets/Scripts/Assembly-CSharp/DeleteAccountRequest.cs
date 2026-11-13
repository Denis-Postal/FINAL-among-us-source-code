using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DeleteAccountRequest : MonoBehaviour, ITranslatedText
{
	public TextMeshPro buttonText;

	public DeleteAccountPopupBox popupConfirmation;

	public InfoTextBox popupConfirmationUndelete;

	public InfoTextBox somethingWentWrong;

	private void Start()
	{
		UpdateText();
		popupConfirmation.button1.OnClick.AddListener(() =>
		{
			DeleteAccount();
		});
		DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Add(this);
	}

	private void OnDestroy()
	{
		if (DestroyableSingleton<TranslationController>.InstanceExists)
		{
			try
			{
				DestroyableSingleton<TranslationController>.Instance.ActiveTexts.Remove(this);
			}
			catch
			{
			}
		}
	}

	public void ShowConfirmationScreen()
	{
		if (DestroyableSingleton<AccountManager>.Instance.IsAccountWaitingForDeletion)
		{
			popupConfirmationUndelete.gameObject.SetActive(value: true);
			popupConfirmationUndelete.SetTwoButtons();
		}
		else
		{
			popupConfirmation.gameObject.SetActive(value: true);
		}
	}

	public void ResetText()
	{
		UpdateText();
	}

	private void SomethingWentWrong(string error)
	{
		somethingWentWrong.bodyText.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OtherDownloadError, error));
		somethingWentWrong.gameObject.SetActive(value: true);
		somethingWentWrong.SetOneButton();
		somethingWentWrong.button1Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Okay));
		somethingWentWrong.button1.OnClick.RemoveAllListeners();
		somethingWentWrong.button1.OnClick.AddListener(() =>
		{
			UpdateText();
		});
	}

	private void DeleteAccount()
	{
		StartCoroutine(AddDeleteRequest());
	}

	private void UnDeleteAccount()
	{
		StartCoroutine(RemoveDeleteRequest());
	}

	// Offline remake: no account-deletion request is sent anywhere; the deletion flag is only toggled locally.
	private IEnumerator AddDeleteRequest()
	{
		DestroyableSingleton<AccountManager>.Instance.UpdateAccountDeletionStatus(shouldDelete: true);
		UpdateText();
		yield break;
	}

	private IEnumerator RemoveDeleteRequest()
	{
		DestroyableSingleton<AccountManager>.Instance.UpdateAccountDeletionStatus(shouldDelete: false);
		UpdateText();
		yield break;
	}

	private void UpdateText()
	{
		if (!DestroyableSingleton<AccountManager>.Instance.IsAccountWaitingForDeletion)
		{
			buttonText.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DeleteAccount));
			return;
		}
		popupConfirmationUndelete.button1Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Confirm));
		popupConfirmationUndelete.button2Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Cancel));
		popupConfirmationUndelete.titleTexxt.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DoNotDeleteAccount));
		popupConfirmationUndelete.bodyText.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AccountUnDeleteHelp));
		buttonText.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DoNotDeleteAccount));
		popupConfirmationUndelete.button1.OnClick.RemoveAllListeners();
		popupConfirmationUndelete.button1.OnClick.AddListener(() =>
		{
			UnDeleteAccount();
		});
	}
}
