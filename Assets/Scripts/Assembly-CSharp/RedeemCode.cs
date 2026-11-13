using System;
using System.Collections;
using System.Text;
using AmongUs.Data;
using Assets.InnerNet;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class RedeemCode : MonoBehaviour, IConnectButton
{
	public class RedeemCostRequest
	{
		[JsonProperty("code")]
		public string Code;

		public static Request<RedeemCostRequest> RedeemCodeCosmetic(string code)
		{
			RedeemCostRequest redeemCostRequest = new RedeemCostRequest();
			redeemCostRequest.Code = code;
			return new Request<RedeemCostRequest>
			{
				Data = new Request<RedeemCostRequest>.RequestData
				{
					Attributes = redeemCostRequest,
					Type = "code"
				}
			};
		}
	}

	public class RedeemCodeResponse
	{
		[JsonProperty("code")]
		public string Code;

		[JsonProperty("item_id")]
		public string ItemId;

		[JsonProperty("multi_use")]
		public bool MultiUse;

		[JsonProperty("platform")]
		public string Platform;

		[JsonProperty("redeemed_by")]
		public string ReedemedBy;

		[JsonProperty("request_id")]
		public string requestID;
	}

	public class RedeemCodeError
	{
		[JsonProperty("code")]
		public string Code;

		[JsonProperty("title")]
		public string Title;

		[JsonProperty("keyword")]
		public string Keyword;

		[JsonProperty("detail")]
		public string detail;
	}

	[SerializeField]
	private TextBoxTMP redeemCodeField;

	[SerializeField]
	private PassiveButton redeemCodeButton;

	[SerializeField]
	private PassiveButton backButton;

	[SerializeField]
	private GameObject loadingIcon;

	[SerializeField]
	private RedeemCosmeticPopup cosmeticPopup;

	private const int CODE_LENGTH = 14;

	private const int CODE_ERROR_INVALID = 1300;

	private const int CODE_ERROR_REDEEMED = 3200;

	private const int CODE_ERROR_REDEEMED2 = 3201;

	private bool loading;

	private readonly Logger logger = new Logger(Logger.Category.Network, "RedeemCode");

	private void OnEnable()
	{
		if ((bool)redeemCodeField)
		{
			redeemCodeField.ResetField();
		}
		loading = false;
		UpdateRedeemButton();
		ControllerManager.Instance.SetBackButton(backButton);
	}

	// Offline remake: redeeming a code is a request to the live Innersloth service (RedeemCostRequest). That is not used here; the
	// button stays disabled and nothing is sent. (Every cosmetic is already unlocked: PlayerPurchasesData.UnlockEverythingForTesting.)
	public void UpdateRedeemButton()
	{
		if ((bool)redeemCodeButton)
		{
			redeemCodeButton.SetButtonEnableState(false);
		}
	}

	public void TryRedeemCode()
	{
		logger.Warning("RedeemCode: redeeming codes is unavailable in the offline build");
		if ((bool)redeemCodeField)
		{
			StartCoroutine(Effects.SwayX(redeemCodeField.transform));
		}
	}

	// Offline remake: the original POSTs the code to BackendEndpoints.RedeemCode; this build never sends it (not reachable from TryRedeemCode).
	private IEnumerator CoRedeemCode(string code)
	{
		logger.Warning("RedeemCode: CoRedeemCode ignored in the offline build");
		loading = false;
		yield break;
	}

	private CosmeticData GetCosmetic(string itemID)
	{
		CosmeticData cosmeticData = null;
		string text = "hat_NoHat";
		string text2 = "skin_None";
		string text3 = "pet_EmptyPet";
		string text4 = "visor_EmptyVisor";
		string text5 = "nameplate_NoPlate";
		cosmeticData = DestroyableSingleton<HatManager>.Instance.GetHatById(itemID);
		if (cosmeticData != null && cosmeticData.ProdId != text)
		{
			logger.Info("Found hat " + cosmeticData.ProdId);
			return cosmeticData;
		}
		cosmeticData = DestroyableSingleton<HatManager>.Instance.GetSkinById(itemID);
		if (cosmeticData != null && cosmeticData.ProdId != text2)
		{
			logger.Info("Found skin " + cosmeticData.ProdId);
			return cosmeticData;
		}
		cosmeticData = DestroyableSingleton<HatManager>.Instance.GetPetById(itemID);
		if (cosmeticData != null && cosmeticData.ProdId != text3)
		{
			logger.Info("Found Pet " + cosmeticData.ProdId);
			return cosmeticData;
		}
		cosmeticData = DestroyableSingleton<HatManager>.Instance.GetVisorById(itemID);
		if (cosmeticData != null && cosmeticData.ProdId != text4)
		{
			logger.Info("Found Visor " + cosmeticData.ProdId);
			return cosmeticData;
		}
		cosmeticData = DestroyableSingleton<HatManager>.Instance.GetNamePlateById(itemID);
		if (cosmeticData != null && cosmeticData.ProdId != text5)
		{
			logger.Info("Found Nameplate " + cosmeticData.ProdId);
			return cosmeticData;
		}
		return null;
	}

	private void OnDisable()
	{
		loading = false;
		StopIcon();
		redeemCodeField.ResetField();
		redeemCodeField.text = string.Empty;
	}

	public void StartIcon()
	{
		if ((bool)loadingIcon)
		{
			loadingIcon.SetActive(value: true);
		}
	}

	public void StopIcon()
	{
		if ((bool)loadingIcon)
		{
			loadingIcon.SetActive(value: false);
		}
	}
}
