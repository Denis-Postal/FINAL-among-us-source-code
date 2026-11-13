using System.Collections;
using UnityEngine;

public class RestorePurchases : MonoBehaviour
{
	[SerializeField]
	private InfoTextBox errorDisplay;

	private bool isRestoring;

	public void RestoreiOSPurchases()
	{
		if (!isRestoring)
		{
			StartCoroutine(CoRestoreiOSPurchases());
		}
	}

	private IEnumerator CoRestoreiOSPurchases()
	{
		isRestoring = true;
		errorDisplay.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Processing));
		errorDisplay.gameObject.SetActive(value: true);
		errorDisplay.SetOneButton();
		errorDisplay.button1Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Cancel));
		DestroyableSingleton<EOSManager>.Instance.validateOldDLC = new CompletionToken<PurchaseStates>(PurchaseStates.NotStarted);
		DestroyableSingleton<StoreManager>.Instance.ProcessOldReceipts();
		yield return DestroyableSingleton<EOSManager>.Instance.validateOldDLC.WaitForCompletion();
		errorDisplay.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Success));
		errorDisplay.button1Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Okay));
		isRestoring = false;
	}
}
