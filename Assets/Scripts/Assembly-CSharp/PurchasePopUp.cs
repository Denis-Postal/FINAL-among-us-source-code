using Assets.InnerNet;
using TMPro;
using UnityEngine;

public class PurchasePopUp : MonoBehaviour
{
	public enum PurchasePopUpMode
	{
		PurchaseItem,
		CheckingPurchases
	}

	public GameObject waitingText;

	public TextMeshPro titleText;

	public TextMeshPro infoText;

	public GameObject closeButton;

	public GameObject controllerFocusHolder;

	public void Show(PurchasePopUpMode mode = PurchasePopUpMode.PurchaseItem)
	{
		infoText.text = string.Empty;
		waitingText.gameObject.SetActive(value: false);
		infoText.gameObject.SetActive(value: true);
		infoText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WifiPleaseWait) + "...";
		closeButton.SetActive(value: false);
		controllerFocusHolder.SetActive(value: true);
		switch (mode)
		{
		case PurchasePopUpMode.PurchaseItem:
			titleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.PurchasingLabel);
			break;
		case PurchasePopUpMode.CheckingPurchases:
			titleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.CheckingPurchasesLabel);
			break;
		}
	}

	public void SetResponse(ResponseState responseSate, ResponseError[] errors)
	{
		string text = "";
		if (errors != null && errors.Length != 0)
		{
			text = text + "\n" + GetErrorString(errors[0]);
		}
		SetResponse(responseSate, text);
	}

	public void SetResponse(ResponseState responseSate, string error)
	{
		waitingText.gameObject.SetActive(value: false);
		if (responseSate == ResponseState.Success)
		{
			infoText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Success);
		}
		else
		{
			string text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FailPurchase);
			if (!string.IsNullOrEmpty(error))
			{
				text = text + "\n" + error;
			}
			infoText.text = text;
		}
		infoText.gameObject.SetActive(value: true);
		controllerFocusHolder.SetActive(value: false);
		closeButton.SetActive(value: true);
	}

	private string GetErrorString(ResponseError error)
	{
		switch (error.Detail)
		{
		case 1337420:
			return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.UserIdTokenError);
		case 3304:
			return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FailPurchaseAlreadyOwn);
		case 3305:
		case 3306:
			return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FailPurchaseCurrency);
		case 3308:
			return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FailPurchaseCubeOwn);
		default:
		{
			int num = ((error.Detail == 0) ? error.Code : error.Detail);
			return DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FailPurchaseUnknown, num);
		}
		}
	}
}
