// Shim for UnityEngine.Purchasing (IAP package not used offline). TODO(verify): API surface limited to what the game compiles against.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace UnityEngine.Purchasing
{
	public enum ProductType { Consumable, NonConsumable, Subscription }
	public enum PurchaseProcessingResult { Complete, Pending }
	public enum PurchaseFailureReason { PurchasingUnavailable, ExistingPurchasePending, ProductUnavailable, SignatureInvalid, UserCancelled, PaymentDeclined, DuplicateTransaction, Unknown }
	public enum InitializationFailureReason { PurchasingUnavailable, NoProductsAvailable, AppNotKnown }
	public class ProductMetadata { public string localizedPriceString = ""; public string localizedTitle = ""; public string localizedDescription = ""; public string isoCurrencyCode = ""; public decimal localizedPrice; }
	public class ProductDefinition { public string id; public string storeSpecificId; public ProductType type; public bool enabled = true; public ProductDefinition() { } public ProductDefinition(string id, ProductType type) { this.id = id; storeSpecificId = id; this.type = type; } public ProductDefinition(string id, string storeId, ProductType type) { this.id = id; storeSpecificId = storeId; this.type = type; } }
	public class Product { public ProductDefinition definition; public ProductMetadata metadata = new ProductMetadata(); public bool availableToPurchase; public string transactionID; public bool hasReceipt; public string receipt; }
	public class PurchaseEventArgs { public Product purchasedProduct; }
	public interface IExtensionProvider { T GetExtension<T>() where T : IStoreExtension; }
	public interface IStoreExtension { }
	public interface IStoreController { void InitiatePurchase(Product product); void InitiatePurchase(string productId); void ConfirmPendingPurchase(Product product); Product[] products { get; } }
	public interface IStoreListener
	{
		void OnInitialized(IStoreController controller, IExtensionProvider extensions);
		void OnInitializeFailed(InitializationFailureReason error);
		void OnInitializeFailed(InitializationFailureReason error, string message);
		PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent);
		void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason);
	}
}
namespace UnityEngine.Purchasing.Extension
{
	public interface IStoreCallback { void OnSetupFailed(InitializationFailureReason reason); void OnSetupFailed(InitializationFailureReason reason, string message); void OnProductsRetrieved(List<ProductDescription> products); void OnPurchaseSucceeded(string id, string receipt, string transactionId); void OnPurchaseFailed(PurchaseFailureDescription desc); }
	public class ProductDescription { public string storeSpecificId; public ProductMetadata metadata; public string receipt; public string transactionId; public ProductType type; public ProductDescription(string id, ProductMetadata m) { storeSpecificId = id; metadata = m; } public ProductDescription(string id, ProductMetadata m, string receipt, string transactionId) { storeSpecificId = id; metadata = m; this.receipt = receipt; this.transactionId = transactionId; } }
	public class PurchaseFailureDescription { public string productId; public PurchaseFailureReason reason; public string message; public PurchaseFailureDescription(string id, PurchaseFailureReason r, string m) { productId = id; reason = r; message = m; } }
	public interface IPurchasingBinder { void RegisterStore(string name, IStore store); }
	public interface IPurchasingModule { void Configure(IPurchasingBinder binder); }
	public interface IStore { void Initialize(IStoreCallback callback); void RetrieveProducts(ReadOnlyCollection<ProductDefinition> products); void Purchase(ProductDefinition product, string developerPayload); void FinishTransaction(ProductDefinition product, string transactionId); }
}
