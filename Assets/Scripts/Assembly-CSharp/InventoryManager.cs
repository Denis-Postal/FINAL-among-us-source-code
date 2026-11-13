using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AmongUs.Data;
using Assets.InnerNet;
using Epic.OnlineServices;
using Epic.OnlineServices.Inventory;
using Epic.OnlineServices.Platform;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Steamworks;
using UnityEngine.Networking;

public class InventoryManager : DestroyableSingleton<InventoryManager>
{
	private InventoryInterface inventoryInterface;

	private const string InventoryName = "profile";

	private const string PlayerXpPath = "player/xp";

	private const string PlayerRequiredXpPath = "player/xp_required_for_next_level";

	private const string PlayerLevelPath = "player/level";

	private const string PodsFolder = "pods";

	private const string StarsPath = "stars/normal";

	private const string StarsSwitchPath = "stars/switch";

	private const string BeansPath = "beans/normal";

	public RedeemCosmeticPopup redeemPopup;

	private static readonly string[] AllItemContainers = new string[7] { "hats/unlocked", "skins/unlocked", "visors/unlocked", "cubes/unlocked", "pets/unlocked", "bundles/unlocked", "nameplates/unlocked" };

	private Dictionary<string, int> podCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

	private readonly Logger logger = new Logger(Logger.Category.Network, "InventoryManager");

	private List<string> outsideStarPurchases;

	public UpdateState InventoryLoadState { get; private set; }

	public int UnusedBeans { get; set; }

	public int UnusedStars { get; private set; }

	public void RefreshAll(PlatformInterface platformInterface)
	{
		logger.Info("Refreshing inventory");
		if (platformInterface == null)
		{
			logger.Warning("Platform Interface was null.");
			InventoryLoadState = UpdateState.Success;
			DestroyableSingleton<EOSManager>.Instance.EndFinalPartsOfLoginFlow();
			return;
		}
		if (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.ProductUserId))
		{
			logger.Warning("Product User Id was null.");
			InventoryLoadState = UpdateState.Success;
			DestroyableSingleton<EOSManager>.Instance.EndFinalPartsOfLoginFlow();
			return;
		}
		inventoryInterface = platformInterface.GetInventoryInterface();
		if (inventoryInterface == null)
		{
			logger.Warning("Inventory interface was null.");
			InventoryLoadState = UpdateState.Success;
			DestroyableSingleton<EOSManager>.Instance.EndFinalPartsOfLoginFlow();
			return;
		}
		try
		{
			OpenOptions options = new OpenOptions
			{
				InventoryName = "profile",
				LocalUserId = ProductUserId.FromString(DestroyableSingleton<EOSManager>.Instance.ProductUserId),
				TargetUserId = ProductUserId.FromString(DestroyableSingleton<EOSManager>.Instance.ProductUserId)
			};
			inventoryInterface.Open(ref options, null, OnInventoryOpenCallback);
		}
		catch (Exception ex)
		{
			logger.Warning("Inventory Open exception: " + ex.Message);
		}
		if (InventoryLoadState != UpdateState.Success)
		{
			InventoryLoadState = UpdateState.Success;
			DestroyableSingleton<EOSManager>.Instance.EndFinalPartsOfLoginFlow();
		}
	}

	private void OnInventoryOpenCallback(ref OnOpenCompleteCallbackInfo data)
	{
		if (data.ResultCode != Result.Success)
		{
			logger.Error(string.Format("Couldn't open inventory {0}: {1}", "profile", data.ResultCode));
			InventoryLoadState = UpdateState.Failed;
			DestroyableSingleton<EOSManager>.Instance.EndFinalPartsOfLoginFlow();
			return;
		}
		InventoryLoadState = UpdateState.Success;
		ResetInventory();
		CheckStars(data.InventoryItemHandle);
		CheckBeans(data.InventoryItemHandle);
		CheckExpAndLevel(data.InventoryItemHandle);
		CheckPods(data.InventoryItemHandle);
		string[] allItemContainers = AllItemContainers;
		foreach (string path in allItemContainers)
		{
			CheckUnlockedItems(data.InventoryItemHandle, path);
		}
		CheckEquipped();
		CloseOptions options = new CloseOptions
		{
			InventoryName = "profile",
			LocalUserId = ProductUserId.FromString(DestroyableSingleton<EOSManager>.Instance.ProductUserId),
			TargetUserId = ProductUserId.FromString(DestroyableSingleton<EOSManager>.Instance.ProductUserId)
		};
		inventoryInterface.Close(ref options, null, delegate
		{
			StartCoroutine(CheckForNewUnlockables());
		});
	}

	private IEnumerator CheckForNewUnlockables()
	{
		if (DataManager.Player.Account.LoginStatus == EOSManager.AccountLoginStatus.LoggedIn)
		{
			List<CosmeticData> list = new List<CosmeticData>();
			list.AddAll(((IEnumerable<CosmeticData>)DestroyableSingleton<HatManager>.Instance.AllHats.Where((HatData x) => x.freeRedeemableCosmetic)).ToList());
			list.AddAll(((IEnumerable<CosmeticData>)DestroyableSingleton<HatManager>.Instance.AllPets.Where((PetData x) => x.freeRedeemableCosmetic)).ToList());
			list.AddAll(((IEnumerable<CosmeticData>)DestroyableSingleton<HatManager>.Instance.AllSkins.Where((SkinData x) => x.freeRedeemableCosmetic)).ToList());
			list.AddAll(((IEnumerable<CosmeticData>)DestroyableSingleton<HatManager>.Instance.AllVisors.Where((VisorData x) => x.freeRedeemableCosmetic)).ToList());
			list.AddAll(((IEnumerable<CosmeticData>)DestroyableSingleton<HatManager>.Instance.AllNamePlates.Where((NamePlateData x) => x.freeRedeemableCosmetic)).ToList());
			List<CosmeticData> source = (from x in list.Where((CosmeticData x) => x.unlockOnSelectPlatforms == null || x.unlockOnSelectPlatforms.Count == 0 || x.unlockOnSelectPlatforms.Contains(Constants.GetPlatformType())).ToList()
				where x.LimitedTimeAvailable.IsAvailable()
				select x).ToList();
			new List<CosmeticData>();
			List<CosmeticData> list2 = source.Where((CosmeticData x) => !DataManager.Player.Purchases.GetPurchase(x.ProductId, x.BundleId)).ToList();
			foreach (CosmeticData unlockable in list2)
			{
				redeemPopup.gameObject.SetActive(value: true);
				redeemPopup.SetRedeemablePopup(unlockable);
				while (redeemPopup.gameObject.activeSelf)
				{
					yield return null;
				}
				new List<string>().Add(unlockable.ProductId);
				CompletionToken<ResponseState> validateSpecialItems = new CompletionToken<ResponseState>(ResponseState.Failed);
				yield return DestroyableSingleton<InventoryManager>.Instance.CoPurchaseItem(unlockable.ProductId, CurrencyTypes.Beans, (ResponseState state, Response<ResponsePurchase> response) =>
				{
					validateSpecialItems.MarkComplete(state);
				});
				if (validateSpecialItems.Value == ResponseState.Failed)
				{
					logger.Error("Redemption of " + unlockable.ProductId + " failed. Error " + validateSpecialItems.errorStr);
				}
			}
		}
		DestroyableSingleton<EOSManager>.Instance.EndFinalPartsOfLoginFlow();
	}

	private void CheckBeans(InventoryItem rootItem)
	{
		if (TryFollowPath(rootItem, "beans/normal", out var targetHandle))
		{
			InventoryItemGetValueAsInt64Options options = default;
			targetHandle.GetValueAsInt64(ref options, out var outValue);
			UnusedBeans = (int)outValue;
		}
	}

	private void CheckStars(InventoryItem rootItem)
	{
		if (TryFollowPath(rootItem, "stars/normal", out var targetHandle))
		{
			InventoryItemGetValueAsInt64Options options = default;
			targetHandle.GetValueAsInt64(ref options, out var outValue);
			UnusedStars = (int)outValue;
			DestroyableSingleton<StoreMenu>.Instance.UpdateStarDisplay();
		}
	}

	private void CheckExpAndLevel(InventoryItem rootItem)
	{
		logger.Info("CheckExpAndLevel() :: Caching Level and Exp from EOS");
		if (TryFollowPath(rootItem, "player/xp", out var targetHandle))
		{
			InventoryItemGetValueAsInt64Options options = default;
			targetHandle.GetValueAsInt64(ref options, out var outValue);
			DataManager.Player.Stats.Xp = (uint)outValue;
			logger.Info(string.Format("{0}() :: Player XP = {1}", "CheckExpAndLevel", DataManager.Player.Stats.Xp));
		}
		if (TryFollowPath(rootItem, "player/xp_required_for_next_level", out var targetHandle2))
		{
			InventoryItemGetValueAsInt64Options options2 = default;
			targetHandle2.GetValueAsInt64(ref options2, out var outValue2);
			DataManager.Player.Stats.XpForNextLevel = (uint)outValue2;
			logger.Info(string.Format("{0}() :: Player Required Xp = {1}", "CheckExpAndLevel", DataManager.Player.Stats.XpForNextLevel));
		}
		if (TryFollowPath(rootItem, "player/level", out var targetHandle3))
		{
			InventoryItemGetValueAsInt64Options options3 = default;
			targetHandle3.GetValueAsInt64(ref options3, out var outValue3);
			DataManager.Player.Stats.Level = (uint)outValue3;
			logger.Info(string.Format("{0}() :: Player Level = {1}", "CheckExpAndLevel", DataManager.Player.Stats.Level));
		}
		else
		{
			DataManager.Player.Stats.Level = 0u;
		}
		DataManager.Player.Save();
	}

	private void CheckPods(InventoryItem rootItem)
	{
		if (!TryFollowPath(rootItem, "pods", out var targetHandle))
		{
			return;
		}
		InventoryItemGetSubItemsCountOptions options = default;
		uint subItemsCount = targetHandle.GetSubItemsCount(ref options);
		InventoryItemGetSubItemByIndexOptions options2 = default;
		InventoryItemGetKeyOptions options3 = default;
		InventoryItemGetValueAsInt64Options options4 = default;
		for (uint num = 0u; num < subItemsCount; num++)
		{
			options2.SubItemIndex = num;
			Result subItemByIndex = targetHandle.GetSubItemByIndex(ref options2, out var outInventoryItemHandle);
			if (subItemByIndex != Result.Success)
			{
				logger.Warning($"Failed to get pods subitem {num}: {subItemByIndex}");
				InventoryLoadState = UpdateState.PartialSuccess;
				continue;
			}
			Result key = outInventoryItemHandle.GetKey(ref options3, out var outKey);
			if (key != Result.Success)
			{
				logger.Warning($"Failed to get key for pods subitem {num}: {key}");
				InventoryLoadState = UpdateState.PartialSuccess;
				continue;
			}
			outInventoryItemHandle.GetValueAsInt64(ref options4, out var outValue);
			if (!podCounts.ContainsKey(outKey))
			{
				logger.Info($"Player has {outValue} {outKey} pods");
				podCounts.Add(outKey, (int)outValue);
			}
		}
	}

	private void CheckUnlockedItems(InventoryItem rootItem, string path)
	{
		if (!TryFollowPath(rootItem, path, out var targetHandle))
		{
			return;
		}
		InventoryItemGetValueCountOptions options = default;
		uint valueCount = targetHandle.GetValueCount(ref options);
		logger.Info($"User has {valueCount} items in {path}");
		InventoryItemGetStringSetValueByIndexOptions options2 = default;
		for (uint num = 0u; num < valueCount; num++)
		{
			options2.SetValueIndex = num;
			if (targetHandle.GetStringSetValueByIndex(ref options2, out var outValue) == Result.Success)
			{
				logger.Info("Unlocked item: " + outValue);
				DataManager.Player.Purchases.SetPurchased(outValue);
				if (path.Contains("bundles"))
				{
					DataManager.Player.Purchases.SetPurchased(outValue.ToString().Replace("bundle", "map"));
				}
			}
		}
	}

	private bool TryFollowPath(InventoryItem rootHandle, string path, out InventoryItem targetHandle)
	{
		InventoryItemGetSubItemOptions options = default;
		targetHandle = rootHandle;
		string[] array = path.Split('/');
		foreach (string text in array)
		{
			options.Key = text;
			Result subItem = targetHandle.GetSubItem(ref options, out targetHandle);
			if (subItem != Result.Success)
			{
				logger.Warning($"Couldn't find folder '{text}' in path '{path}': {subItem}");
				return false;
			}
		}
		return true;
	}

	public int GetPodCount(string podType)
	{
		if (podCounts.TryGetValue("/pods/" + podType, out var value))
		{
			return value;
		}
		return 0;
	}

	public int ChangePodCount(string podType, int podDelta)
	{
		if (string.IsNullOrEmpty(podType))
		{
			return 0;
		}
		if (!podType.StartsWith("/pods/"))
		{
			podType = "/pods/" + podType;
		}
		podCounts.TryGetValue(podType, out var value);
		int num = podDelta + value;
		podCounts[podType] = num;
		return num;
	}

	public IEnumerator WaitForInventoryLoad()
	{
		while (InventoryLoadState == UpdateState.Connecting)
		{
			yield return null;
		}
	}

	private void ResetInventory()
	{
		UnusedStars = 0;
		UnusedBeans = 0;
		DataManager.Player.Stats.Xp = 0u;
		DataManager.Player.Stats.Level = 0u;
		DataManager.Player.Stats.XpForNextLevel = 0u;
		podCounts = new Dictionary<string, int>();
		DataManager.Player.Purchases.ClearAllPurchases();
		foreach (BundleData allBundle in DestroyableSingleton<HatManager>.Instance.AllBundles)
		{
			if (allBundle.Free && !allBundle.NotInStore)
			{
				DataManager.Player.Purchases.SetPurchased(allBundle.productId);
			}
		}
		DataManager.Player.Save();
	}

	private void CheckEquipped()
	{
		HatData hatById = DestroyableSingleton<HatManager>.Instance.GetHatById(DataManager.Player.Customization.Hat);
		if (hatById.BlocksVisors)
		{
			DataManager.Player.Customization.Visor = "visor_EmptyVisor";
		}
		if (!hatById.Free && !DataManager.Player.Purchases.GetPurchase(hatById.ProductId, hatById.BundleId))
		{
			DataManager.Player.Customization.Hat = "";
		}
		PetData petById = DestroyableSingleton<HatManager>.Instance.GetPetById(DataManager.Player.Customization.Pet);
		if (!petById.Free && !DataManager.Player.Purchases.GetPurchase(petById.ProductId, petById.BundleId))
		{
			DataManager.Player.Customization.Pet = "";
		}
		SkinData skinById = DestroyableSingleton<HatManager>.Instance.GetSkinById(DataManager.Player.Customization.Skin);
		if (!skinById.Free && !DataManager.Player.Purchases.GetPurchase(skinById.ProductId, skinById.BundleId))
		{
			DataManager.Player.Customization.Skin = "";
		}
		VisorData visorById = DestroyableSingleton<HatManager>.Instance.GetVisorById(DataManager.Player.Customization.Visor);
		if (!visorById.Free && !DataManager.Player.Purchases.GetPurchase(visorById.ProductId, visorById.BundleId))
		{
			DataManager.Player.Customization.Visor = "";
		}
		NamePlateData namePlateById = DestroyableSingleton<HatManager>.Instance.GetNamePlateById(DataManager.Player.Customization.NamePlate);
		if (!namePlateById.Free && !DataManager.Player.Purchases.GetPurchase(namePlateById.ProductId, namePlateById.BundleId))
		{
			DataManager.Player.Customization.NamePlate = "";
		}
		CosmicubeData cubeDataByID = DestroyableSingleton<CosmicubeManager>.Instance.GetCubeDataByID(DataManager.Player.Store.ActiveCosmicube);
		if (!cubeDataByID.Free && !DataManager.Player.Purchases.GetPurchase(cubeDataByID.ProdId, ""))
		{
			DataManager.Player.Store.ActiveCosmicube = "";
		}
		PlayerStorageManager.CloudPlayerPrefs playerPrefs = DestroyableSingleton<PlayerStorageManager>.Instance.PlayerPrefs;
		playerPrefs.ActivePodType = DestroyableSingleton<CosmicubeManager>.Instance.GetCubeDataByID(DataManager.Player.Store.ActiveCosmicube).podId;
		DestroyableSingleton<PlayerStorageManager>.Instance.SaveChanges(playerPrefs);
		if (AmongUsClient.Instance.AmConnected)
		{
			AmongUsClient.Instance.SetActivePodType(playerPrefs.ActivePodType);
		}
		DataManager.Player.Save();
		DestroyableSingleton<AccountManager>.Instance.UpdatePlayerCustomizationVisuals();
	}

	public void UpdateStarCount(int starsToAdd)
	{
		UnusedStars += starsToAdd;
		DestroyableSingleton<StoreMenu>.Instance.UpdateStarDisplay();
	}

	public void AddOutsideStarPurchase(string starPurchase)
	{
		if (outsideStarPurchases == null)
		{
			outsideStarPurchases = new List<string>();
		}
		outsideStarPurchases.Add(starPurchase);
	}

	public IEnumerator CoPurchaseItem(string productId, CurrencyTypes currency, Action<ResponseState, Response<ResponsePurchase>> resultCallback)
	{
		yield return RequestPurchaseCo(RedeemCurrency.ForNonCubePurchase(productId, currency), resultCallback, productId);
	}

	public void UnlockCubeItem(string productId, string cubeName, Action<ResponseState, Response<ResponsePurchase>> resultCallback)
	{
		StartCoroutine(RequestPurchaseCo(RedeemCurrency.ForCubePurchase(productId, cubeName), (ResponseState cb, Response<ResponsePurchase> response) =>
		{
			resultCallback(cb, response);
			if (cb == ResponseState.Success)
			{
				DataManager.Player.Purchases.SetPurchased(productId);
			}
		}, productId));
	}

	private IEnumerator RequestPurchaseCo(Request<RedeemCurrency> requestData, Action<ResponseState, Response<ResponsePurchase>> resultCallback, string productId)
	{
		string redeemCurrency = BackendEndpoints.RedeemCurrency;
		string text = JsonConvert.SerializeObject(requestData);
		UnityWebRequest request = UnityWebRequest.PostWwwForm(redeemCurrency, text);
		request.SetRequestHeader("Content-Type", "application/vnd.api+json");
		request.SetRequestHeader("Accept", "application/vnd.api+json");
		if (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.UserIDToken))
		{
			logger.Error("Cannot make purchase, user is not signed in");
			resultCallback(ResponseState.Failed, JsonConvert.DeserializeObject<Response<ResponsePurchase>>(Constants.UserIdTokenError));
			yield break;
		}
		request.SetRequestHeader("Authorization", "Bearer " + DestroyableSingleton<EOSManager>.Instance.UserIDToken);
		UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(Encoding.ASCII.GetBytes(text));
		uploadHandlerRaw.contentType = "application/vnd.api+json";
		request.uploadHandler = uploadHandlerRaw;
		logger.Info("Sent RequestPurchase request for " + productId);
		yield return request.SendWebRequest();
		logger.Info("RequestPurchase Request done for " + productId);
		if (!HttpUtils.IsSuccess(request.responseCode))
		{
			logger.Error("Request failed, " + request.responseCode + ": " + (HttpStatusCode)request.responseCode/*cast due to constrained. prefix*/);
			logger.Error(request.error);
			Response<ResponsePurchase> response = JsonConvert.DeserializeObject<Response<ResponsePurchase>>(request.downloadHandler.text);
			string text2 = "null";
			if (response?.Errors != null)
			{
				text2 = string.Join("\r\n", response.Errors.Select((ResponseError s) => s.ToString()));
			}
			logger.Error("Failed to purchase " + productId + ": " + text2);
			resultCallback(ResponseState.Failed, response);
			yield break;
		}
		Response<ResponsePurchase> response2 = JsonConvert.DeserializeObject<Response<ResponsePurchase>>(request.downloadHandler.text);
		if (response2.Data.Attributes.CurrencyType.Equals("/beans/normal", StringComparison.OrdinalIgnoreCase))
		{
			UnusedBeans -= response2.Data.Attributes.ItemCost;
		}
		else if (response2.Data.Attributes.CurrencyType.Equals("/stars/normal", StringComparison.OrdinalIgnoreCase) || response2.Data.Attributes.CurrencyType.Equals("/stars/switch", StringComparison.OrdinalIgnoreCase))
		{
			UnusedStars -= response2.Data.Attributes.ItemCost;
		}
		else if (podCounts.ContainsKey(response2.Data.Attributes.CurrencyType))
		{
			podCounts[response2.Data.Attributes.CurrencyType] -= response2.Data.Attributes.ItemCost;
		}
		else
		{
			logger.Error($"No valid currency found to update. json {response2}");
		}
		DataManager.Player.Purchases.SetPurchased(productId);
		resultCallback(ResponseState.Success, response2);
		logger.Info("Successful purchase of " + productId);
	}

	public void ValidatePreviousReceipts()
	{
		if (!SteamManager.Initialized)
		{
			// Remake: Steam is not available (offline); nothing to validate.
			return;
		}
		CSteamID steamID = SteamUser.GetSteamID();
		CompletionToken<PurchaseStates> completionToken = new CompletionToken<PurchaseStates>(PurchaseStates.NotStarted);
		StartCoroutine(CoWaitForRedemptionCompletion(completionToken));
		DestroyableSingleton<InventoryManager>.Instance.ValidateDlcReceipt(steamID.ToString(), completionToken);
	}

	public void WaitForRedemptionCompletion(CompletionToken<PurchaseStates> completionToken)
	{
		StartCoroutine(CoWaitForRedemptionCompletion(completionToken));
	}

	public IEnumerator CoWaitForRedemptionCompletion(CompletionToken<PurchaseStates> completionToken)
	{
		logger.Debug("wait for completion?");
		yield return completionToken.WaitForCompletion();
		DestroyableSingleton<EOSManager>.Instance.validateOldDLC?.MarkComplete(PurchaseStates.Success);
	}

	public void ValidateIapReceipt(string receiptData, CompletionToken<PurchaseStates> queryDone, Action starPurchaseCallback = null)
	{
		ValidateReceiptCall(ValidateReceipt.Create("in-app-purchase", receiptData), queryDone, starPurchaseCallback);
	}

	public void ValidateIapReceipt(List<string> receiptData, CompletionToken<PurchaseStates> queryDone)
	{
		if (receiptData.Count == 1)
		{
			ValidateReceiptCall(ValidateReceipt.Create("in-app-purchase", receiptData[0]), queryDone);
		}
		else
		{
			ValidateReceiptCall(ValidateReceiptBatch.Create("in-app-purchase", receiptData), queryDone);
		}
	}

	public void ValidateDlcReceipt(string receiptData, CompletionToken<PurchaseStates> queryDone)
	{
		ValidateReceiptCall(ValidateReceipt.Create("dlc", receiptData), queryDone);
	}

	public void ValidateDlcReceipt(List<string> receiptData, CompletionToken<PurchaseStates> queryDone)
	{
		if (receiptData.Count == 1)
		{
			ValidateReceiptCall(ValidateReceipt.Create("dlc", receiptData[0]), queryDone);
		}
		else
		{
			ValidateReceiptCall(ValidateReceiptBatch.Create("dlc", receiptData), queryDone);
		}
	}

	public void ValidateInnerslothReceipt(string receiptData, CompletionToken<PurchaseStates> queryDone)
	{
		ValidateReceiptCall(ValidateReceipt.Create("innersloth-receipt", receiptData), queryDone);
	}

	public IEnumerator CoValidateTrustedReceipt(string[] productIds, CompletionToken<PurchaseStates> queryDone)
	{
		JObject jObject = new JObject();
		jObject.Add("items", new JArray(productIds));
		jObject.Add("eos_token", DestroyableSingleton<EOSManager>.Instance.GetPlatformAuthToken());
		string requestJson = JsonConvert.SerializeObject(ValidateReceipt.Create("innersloth-receipt", jObject.ToString()));
		string validateReceipt = BackendEndpoints.ValidateReceipt;
		yield return CoValidateReceipt(requestJson, validateReceipt, queryDone);
	}

	private void ValidateReceiptCall(Request<ValidateReceiptBatch> requestData, CompletionToken<PurchaseStates> queryDone)
	{
		string requestJson = JsonConvert.SerializeObject(requestData);
		string validateReceiptBatch = BackendEndpoints.ValidateReceiptBatch;
		StartCoroutine(CoValidateReceipt(requestJson, validateReceiptBatch, queryDone));
	}

	private void ValidateReceiptCall(Request<ValidateReceipt> requestData, CompletionToken<PurchaseStates> queryDone, Action starPurchaseCallback = null)
	{
		string requestJson = JsonConvert.SerializeObject(requestData);
		string validateReceipt = BackendEndpoints.ValidateReceipt;
		StartCoroutine(CoValidateReceipt(requestJson, validateReceipt, queryDone, starPurchaseCallback));
	}

	private IEnumerator CoValidateReceipt(string requestJson, string requestEndpoint, CompletionToken<PurchaseStates> queryDone, Action starPurchaseCallback = null)
	{
		logger.Debug("JSON content: ");
		logger.Debug(requestJson);
		UnityWebRequest request = UnityWebRequest.PostWwwForm(requestEndpoint, requestJson);
		request.SetRequestHeader("Content-Type", "application/vnd.api+json");
		request.SetRequestHeader("Accept", "application/vnd.api+json");
		if (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.UserIDToken))
		{
			logger.Error("UserIDToken didn't exist, cannot validate receipt");
			queryDone?.MarkComplete(PurchaseStates.Fail, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.UserIdTokenError));
			yield break;
		}
		request.SetRequestHeader("Authorization", "Bearer " + DestroyableSingleton<EOSManager>.Instance.UserIDToken);
		UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(Encoding.ASCII.GetBytes(requestJson));
		uploadHandlerRaw.contentType = "application/vnd.api+json";
		request.uploadHandler = uploadHandlerRaw;
		string text = Encoding.ASCII.GetString(request.uploadHandler.data);
		logger.Debug("Full ValidateReceipt request: " + text);
		yield return request.SendWebRequest();
		logger.Debug("ValidateReceipt Request done");
		logger.Debug("Response " + request.responseCode + " - " + request.downloadHandler.text + " - " + request.result);
		if (!HttpUtils.IsSuccess(request.responseCode))
		{
			logger.Error("ValidateReceipt Request failed, " + request.responseCode + ": " + (HttpStatusCode)request.responseCode/*cast due to constrained. prefix*/);
			logger.Error(request.error);
			logger.Error(request.downloadHandler.text);
			string text2 = request.downloadHandler.text;
			Response<ResponsePurchase> response;
			try
			{
				response = JsonConvert.DeserializeObject<Response<ResponsePurchase>>(text2);
			}
			catch (Exception ex)
			{
				logger.Error("ERROR: Response from server wasn't valid json. (Exception: " + ex.Message + ")");
				response = null;
			}
			string text3 = "null";
			logger.Error("Failed to validate receipt: " + text3);
			string text4 = "";
			if (response?.Errors != null && response.Errors.Length != 0)
			{
				int num = ((response.Errors[0].Detail == 0) ? response.Errors[0].Code : response.Errors[0].Detail);
				text4 = text4 + "\n" + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FailPurchaseUnknown, num);
			}
			queryDone?.MarkComplete(PurchaseStates.Fail, text4);
		}
		else
		{
			string text5 = request.downloadHandler.text;
			logger.Debug("Finished redeeming old DLC " + text5);
			starPurchaseCallback?.Invoke();
			queryDone?.MarkComplete(PurchaseStates.Success);
		}
	}

	public void InitSteamMicroTxn(Request<SteamTxnInit> initTxn, CompletionToken<PurchaseStates> queryDone)
	{
		string requestJson = JsonConvert.SerializeObject(initTxn);
		string steamTransactionInitialize = BackendEndpoints.SteamTransactionInitialize;
		StartCoroutine(CoSteamPurchaseStars(requestJson, steamTransactionInitialize, queryDone));
	}

	public void FinalizeSteamMicroTxn(Request<SteamTxnFin> finTxn, CompletionToken<PurchaseStates> queryDone)
	{
		string requestJson = JsonConvert.SerializeObject(finTxn);
		string steamTransactionFinalize = BackendEndpoints.SteamTransactionFinalize;
		StartCoroutine(CoSteamPurchaseStars(requestJson, steamTransactionFinalize, queryDone));
	}

	private IEnumerator CoSteamPurchaseStars(string requestJson, string requestEndpoint, CompletionToken<PurchaseStates> queryDone)
	{
		logger.Info("Purchasing for " + requestEndpoint);
		UnityWebRequest request = UnityWebRequest.PostWwwForm(requestEndpoint, requestJson);
		request.SetRequestHeader("Content-Type", "application/vnd.api+json");
		request.SetRequestHeader("Accept", "application/vnd.api+json");
		if (string.IsNullOrEmpty(DestroyableSingleton<EOSManager>.Instance.UserIDToken))
		{
			logger.Error("UserIDToken didn't exist, cannot purchase stars");
			queryDone.MarkComplete(PurchaseStates.Fail, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.UserIdTokenError));
			yield break;
		}
		request.SetRequestHeader("Authorization", "Bearer " + DestroyableSingleton<EOSManager>.Instance.UserIDToken);
		UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(Encoding.ASCII.GetBytes(requestJson));
		uploadHandlerRaw.contentType = "application/vnd.api+json";
		request.uploadHandler = uploadHandlerRaw;
		string text = Encoding.ASCII.GetString(request.uploadHandler.data);
		logger.Debug("Full steam star purchase request: " + text);
		yield return request.SendWebRequest();
		logger.Debug("Full response: " + request.downloadHandler.text);
		if (!HttpUtils.IsSuccess(request.responseCode))
		{
			logger.Error("steam star purchase request failed, " + request.responseCode + ": " + (HttpStatusCode)request.responseCode/*cast due to constrained. prefix*/);
			Response<ResponsePurchase> response = JsonConvert.DeserializeObject<Response<ResponsePurchase>>(request.downloadHandler.text);
			string text2 = "null";
			if (response?.Errors != null)
			{
				text2 = string.Join("\r\n", response.Errors.Select((ResponseError s) => s.ToString()));
			}
			logger.Error("Failed to purchase steam stars: " + text2);
			string text3 = "";
			if (response?.Errors != null && response.Errors.Length != 0)
			{
				int num = ((response.Errors[0].Detail == 0) ? response.Errors[0].Code : response.Errors[0].Detail);
				text3 = text3 + "\n" + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FailPurchaseUnknown, num);
			}
			queryDone.MarkComplete(PurchaseStates.Fail, text3);
		}
		else
		{
			JsonConvert.DeserializeObject<Response<ResponseSteamTxnInit>>(request.downloadHandler.text);
			logger.Debug(request.downloadHandler.text);
			queryDone.MarkComplete(PurchaseStates.Success);
		}
	}
}
