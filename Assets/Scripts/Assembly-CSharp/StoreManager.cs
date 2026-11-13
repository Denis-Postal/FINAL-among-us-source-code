using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class StoreManager : DestroyableSingleton<StoreManager>, IStoreListener
{
	public CompletionToken<PurchaseStates> starPurchaseStateQuery;

	private Product[] allProducts;

	public bool FinishedInitializationFlow;

	private readonly Logger logger = new Logger(Logger.Category.Network, "StoreManager");

	private IStoreController controller;

	private IExtensionProvider extensions;

	private bool initialized;

	// Offline remake: the native Initialize builds Steam products and calls UnityPurchasing.Initialize (live store). There is no store here:
	// the flow is just marked finished with no products so the menus (inventory, shop) can load. Every item is owned via
	// PlayerPurchasesData.UnlockEverythingForTesting; nothing can be purchased and nothing touches the network.
	public void Initialize()
	{
		allProducts = new Product[0];
		FinishedInitializationFlow = true;
		logger.Info("StoreManager: offline store (no products, no purchases)");
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		this.controller = controller;
		this.extensions = extensions;
		if (this.controller == null || this.controller.products == null)
		{
			logger.Error("Failed to initialize store");
			return;
		}
		allProducts = controller.products; // shim IStoreController.products is Product[] (Mono: ProductCollection.all)
		initialized = true;
		FinishedInitializationFlow = true;
		logger.Info("Successfully initialized store");
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		FinishedInitializationFlow = true;
		logger.Warning("Failed to init store:\r\n" + ((object)error/*cast due to constrained. prefix*/).ToString());
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		FinishedInitializationFlow = true;
		logger.Warning("Failed to init store message:\r\n" + ((object)error/*cast due to constrained. prefix*/).ToString() + " message: " + message);
	}

	public void ProcessOldReceiptsRestore()
	{
		if (!initialized)
		{
			Debug.Log("UpdateInventory :: store manager not initialized");
			logger.Warning("Tried to process old receipts before store was finished initializing");
			DestroyableSingleton<EOSManager>.Instance.validateOldDLC.MarkComplete(PurchaseStates.Fail);
		}
		else if (extensions == null)
		{
			Debug.Log("UpdateInventory :: extensions not initialized");
			logger.Error("Somehow StoreManager was initialized, but no IExtensionProvider (this.extensions) was set.");
			DestroyableSingleton<EOSManager>.Instance.validateOldDLC.MarkComplete(PurchaseStates.Fail);
		}
		else
		{
			ProcessOldReceipts();
		}
	}

	public void ProcessOldReceipts()
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		Debug.Log("UpdateInventory :: start process old receipts");
		if (!initialized)
		{
			Debug.Log("UpdateInventory :: tried to process old receipts before the store was finished initializing");
			logger.Warning("Tried to process old receipts before store was finished initializing");
			DestroyableSingleton<EOSManager>.Instance.validateOldDLC.MarkComplete(PurchaseStates.Fail);
			return;
		}
		List<string> list = new List<string>();
		Debug.Log("UpdateInventory :: all products count " + allProducts.Length);
		Product[] array = allProducts;
		foreach (Product val in array)
		{
			if (val != null && (int)val.definition.type == 1)
			{
				Debug.Log($"receipts: {val.hasReceipt}:{val.receipt} - {val.definition.storeSpecificId} - {val.definition.id} - {val.metadata.localizedTitle} - {val.hasReceipt} - {val.transactionID}");
				if (val != null && val.hasReceipt)
				{
					list.Add(val.receipt);
				}
			}
		}
		CompletionToken<PurchaseStates> completionToken = new CompletionToken<PurchaseStates>(PurchaseStates.NotStarted);
		if (list.Count != 0)
		{
			DestroyableSingleton<InventoryManager>.Instance.ValidateIapReceipt(list, completionToken);
			DestroyableSingleton<InventoryManager>.Instance.WaitForRedemptionCompletion(completionToken);
		}
		else
		{
			DestroyableSingleton<EOSManager>.Instance.validateOldDLC?.MarkComplete(PurchaseStates.Success);
		}
	}

	public void InitiateStorePurchaseStar(PurchaseButton button)
	{
		logger.Warning("StoreManager: purchases are unavailable in the offline build");
	}

	private void InitiateStoreHeadlessStarPurchase(PurchaseEventArgs purchaseEventArgs)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		starPurchaseStateQuery = new CompletionToken<PurchaseStates>(PurchaseStates.NotStarted);
		if (purchaseEventArgs.purchasedProduct != null && (int)purchaseEventArgs.purchasedProduct.definition.type == 0)
		{
			StartCoroutine(CoWaitForHeadlessStarPurchaseValidation(starPurchaseStateQuery));
		}
		else
		{
			logger.Warning($"Tried to finish a store purchase that wasn't a star: {purchaseEventArgs}");
		}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Debug.Log($"Process purchase - transaction ID: {e.purchasedProduct.transactionID} - {e.purchasedProduct.hasReceipt} - id: {e.purchasedProduct.definition.id}");
			if ((int)e.purchasedProduct.definition.type == 1)
			{
				return (PurchaseProcessingResult)0;
			}
		}
		catch (Exception) // TODO(verify): Mono catches IAPSecurityException (UnityEngine.Purchasing.Security is not in our reference set)
		{
			Debug.Log("Failed Validation: " + e.purchasedProduct.definition.id);
			return (PurchaseProcessingResult)1;
		}
		Debug.Log("Completed Purchase: " + e.purchasedProduct.definition.id);
		if ((int)e.purchasedProduct.definition.type != 0)
		{
			return (PurchaseProcessingResult)0;
		}
		return (PurchaseProcessingResult)1;
	}

	private IEnumerator CoWaitForStarPurchaseValidation(CompletionToken<PurchaseStates> queryDone, PurchaseButton button)
	{
		yield return queryDone.WaitForCompletion();
		if (queryDone.Value == PurchaseStates.Success)
		{
			if (button.Product is StarBundle)
			{
				StarBundle starBundle = (StarBundle)button.Product;
				Debug.Log(starBundle.starAmount);
				DestroyableSingleton<InventoryManager>.Instance.UpdateStarCount(starBundle.starAmount);
				Product val = allProducts.FirstOrDefault((Product x) =>
				{
					object obj;
					if (x == null)
					{
						obj = null;
					}
					else
					{
						ProductDefinition definition = x.definition;
						obj = ((definition != null) ? definition.id : null);
					}
					return (string)obj == starBundle.GetPurchasingProductId();
				});
				if (val != null && (int)val.definition.type == 0)
				{
					Debug.Log("validating prod " + val.definition.id);
					controller.ConfirmPendingPurchase(val);
				}
			}
			DestroyableSingleton<StoreMenu>.Instance.DisplaySuccess();
			DestroyableSingleton<StoreMenu>.Instance.CloseConfirm();
		}
		else if (queryDone.Value == PurchaseStates.Fail)
		{
			DestroyableSingleton<StoreMenu>.Instance.DisplayFailed(queryDone.errorStr);
		}
		else
		{
			DestroyableSingleton<StoreMenu>.Instance.CloseConfirm();
			DestroyableSingleton<StoreMenu>.Instance.CloseWaitModal();
		}
	}

	private IEnumerator CoWaitForHeadlessStarPurchaseValidation(CompletionToken<PurchaseStates> queryDone)
	{
		yield return queryDone.WaitForCompletion();
		if (queryDone.Value == PurchaseStates.Success)
		{
			DestroyableSingleton<EOSManager>.Instance.UpdateInventory();
		}
	}

	public void OnPurchaseFailed(Product i, PurchaseFailureReason error)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)error == 4)
		{
			DestroyableSingleton<StoreMenu>.Instance.CloseWaitModal();
			DestroyableSingleton<StoreMenu>.Instance.CloseConfirm();
		}
		else
		{
			starPurchaseStateQuery?.MarkComplete(PurchaseStates.Fail, ((object)error/*cast due to constrained. prefix*/).ToString());
			Debug.LogError("Failed: " + ((object)error/*cast due to constrained. prefix*/).ToString());
		}
	}
}
