using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteAccountPopupBox : MonoBehaviour
{
	private List<SelectableHyperLink> selectableHyperLinks = new List<SelectableHyperLink>();

	public SpriteRenderer background;

	public TextMeshPro titleTexxt;

	public TextMeshPro bodyText;

	public TextMeshPro button1Text;

	public TextMeshPro button2Text;

	public PassiveButton button1;

	public PassiveButton button2;

	public Checkbox checkbox;

	public TextMeshPro checkboxText;

	public TextBoxTMP deleteConfirm;

	public void Close()
	{
		checkbox.IsChecked = false;
		deleteConfirm.SetText("");
		GetComponent<TransitionOpen>().Close();
	}

	private void OnEnable()
	{
		button1Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Confirm));
		button2Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Cancel));
		titleTexxt.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DeleteAccount));
		bodyText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AccountDeleteHelp);
		button1.SetButtonEnableState(enabled: false);
		// Offline remake: no EOS connect interface, so there are no linked external accounts to list.
		string text = "";
		if (!string.IsNullOrEmpty(text))
		{
			checkboxText.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ConfirmDeleteAccounts, text));
		}
		else
		{
			checkboxText.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ConfirmDeleteAccountsEmpty, text));
		}
	}

	private void Update()
	{
		if (!ControllerManager.Instance.IsMenuActiveAtAll(gameObject.name))
		{
			ControllerNavMenu component = GetComponent<ControllerNavMenu>();
			Logger.GlobalInstance.Info("EOS: DeleteAccountPopupBox overlay menu wasn't active, opening it...");
			component.OpenMenu(force: true);
		}
		button1.SetButtonEnableState(checkbox.IsChecked && deleteConfirm.text.Equals("delete", StringComparison.OrdinalIgnoreCase));
	}
}
