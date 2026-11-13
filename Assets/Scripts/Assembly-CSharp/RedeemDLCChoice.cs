using TMPro;
using UnityEngine;

public class RedeemDLCChoice : MonoBehaviour
{
	public PassiveButton goAheadButton;

	public PassiveButton notRightNowButton;

	public PassiveButton noAskAgain;

	public TextMeshPro showAccountButtonText;

	public TextMeshPro infoText;

	private bool accountIDShow;

	public void OnClose()
	{
		gameObject.SetActive(value: false);
	}

	public void ToggleAccountId()
	{
		accountIDShow = !accountIDShow;
		UpdateAccountText();
	}

	public void UpdateAccountText()
	{
		string text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ShowAccountID);
		string text2 = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.HiddenAccountID);
		if (accountIDShow)
		{
			text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.HideAccountID);
			text2 = DestroyableSingleton<EOSManager>.Instance.ProductUserId;
		}
		infoText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RedeemPurcahsedItemsExplain, text2);
		showAccountButtonText.text = text;
	}
}
