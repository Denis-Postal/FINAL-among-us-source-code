using System;

namespace Epic.OnlineServices.Ecom
{
	public sealed class EcomInterface : Handle
	{
		public const int CatalogitemApiLatest = 1;

		public const int CatalogitemEntitlementendtimestampUndefined = -1;

		public const int CatalogofferApiLatest = 5;

		public const int CatalogofferEffectivedatetimestampUndefined = -1;

		public const int CatalogofferExpirationtimestampUndefined = -1;

		public const int CatalogofferReleasedatetimestampUndefined = -1;

		public const int CatalogreleaseApiLatest = 1;

		public const int CheckoutApiLatest = 1;

		public const int CheckoutMaxEntries = 10;

		public const int CheckoutentryApiLatest = 1;

		public const int CopyentitlementbyidApiLatest = 2;

		public const int CopyentitlementbyindexApiLatest = 1;

		public const int CopyentitlementbynameandindexApiLatest = 1;

		public const int CopyitembyidApiLatest = 1;

		public const int CopyitemimageinfobyindexApiLatest = 1;

		public const int CopyitemreleasebyindexApiLatest = 1;

		public const int CopylastredeemedentitlementbyindexApiLatest = 1;

		public const int CopyofferbyidApiLatest = 3;

		public const int CopyofferbyindexApiLatest = 3;

		public const int CopyofferimageinfobyindexApiLatest = 1;

		public const int CopyofferitembyindexApiLatest = 1;

		public const int CopytransactionbyidApiLatest = 1;

		public const int CopytransactionbyindexApiLatest = 1;

		public const int EntitlementApiLatest = 2;

		public const int EntitlementEndtimestampUndefined = -1;

		public const int EntitlementidMaxLength = 32;

		public const int GetentitlementsbynamecountApiLatest = 1;

		public const int GetentitlementscountApiLatest = 1;

		public const int GetitemimageinfocountApiLatest = 1;

		public const int GetitemreleasecountApiLatest = 1;

		public const int GetlastredeemedentitlementscountApiLatest = 1;

		public const int GetoffercountApiLatest = 1;

		public const int GetofferimageinfocountApiLatest = 1;

		public const int GetofferitemcountApiLatest = 1;

		public const int GettransactioncountApiLatest = 1;

		public const int ItemownershipApiLatest = 1;

		public const int KeyimageinfoApiLatest = 1;

		public const int QueryentitlementsApiLatest = 2;

		public const int QueryentitlementsMaxEntitlementIds = 256;

		public const int QueryentitlementtokenApiLatest = 1;

		public const int QueryentitlementtokenMaxEntitlementIds = 32;

		public const int QueryoffersApiLatest = 1;

		public const int QueryownershipApiLatest = 2;

		public const int QueryownershipMaxCatalogIds = 400;

		public const int QueryownershipMaxSandboxIds = 10;

		public const int QueryownershipbysandboxidsoptionsApiLatest = 1;

		public const int QueryownershiptokenApiLatest = 2;

		public const int QueryownershiptokenMaxCatalogitemIds = 32;

		public const int RedeementitlementsApiLatest = 2;

		public const int RedeementitlementsMaxIds = 32;

		public const int TransactionidMaximumLength = 64;

		public EcomInterface()
		{
		}

		public EcomInterface(IntPtr innerHandle)
		{
		}

		public void Checkout(ref CheckoutOptions options, object clientData, OnCheckoutCallback completionDelegate)
		{
		}

		public Result CopyEntitlementById(ref CopyEntitlementByIdOptions options, out Entitlement? outEntitlement)
		{
			outEntitlement = null;
			return Result.Success;
		}

		public Result CopyEntitlementByIndex(ref CopyEntitlementByIndexOptions options, out Entitlement? outEntitlement)
		{
			outEntitlement = null;
			return Result.Success;
		}

		public Result CopyEntitlementByNameAndIndex(ref CopyEntitlementByNameAndIndexOptions options, out Entitlement? outEntitlement)
		{
			outEntitlement = null;
			return Result.Success;
		}

		public Result CopyItemById(ref CopyItemByIdOptions options, out CatalogItem? outItem)
		{
			outItem = null;
			return Result.Success;
		}

		public Result CopyItemImageInfoByIndex(ref CopyItemImageInfoByIndexOptions options, out KeyImageInfo? outImageInfo)
		{
			outImageInfo = null;
			return Result.Success;
		}

		public Result CopyItemReleaseByIndex(ref CopyItemReleaseByIndexOptions options, out CatalogRelease? outRelease)
		{
			outRelease = null;
			return Result.Success;
		}

		public Result CopyLastRedeemedEntitlementByIndex(ref CopyLastRedeemedEntitlementByIndexOptions options, out Utf8String outRedeemedEntitlementId)
		{
			outRedeemedEntitlementId = null;
			return Result.Success;
		}

		public Result CopyOfferById(ref CopyOfferByIdOptions options, out CatalogOffer? outOffer)
		{
			outOffer = null;
			return Result.Success;
		}

		public Result CopyOfferByIndex(ref CopyOfferByIndexOptions options, out CatalogOffer? outOffer)
		{
			outOffer = null;
			return Result.Success;
		}

		public Result CopyOfferImageInfoByIndex(ref CopyOfferImageInfoByIndexOptions options, out KeyImageInfo? outImageInfo)
		{
			outImageInfo = null;
			return Result.Success;
		}

		public Result CopyOfferItemByIndex(ref CopyOfferItemByIndexOptions options, out CatalogItem? outItem)
		{
			outItem = null;
			return Result.Success;
		}

		public Result CopyTransactionById(ref CopyTransactionByIdOptions options, out Transaction outTransaction)
		{
			outTransaction = null;
			return Result.Success;
		}

		public Result CopyTransactionByIndex(ref CopyTransactionByIndexOptions options, out Transaction outTransaction)
		{
			outTransaction = null;
			return Result.Success;
		}

		public uint GetEntitlementsByNameCount(ref GetEntitlementsByNameCountOptions options)
		{
			return 0u;
		}

		public uint GetEntitlementsCount(ref GetEntitlementsCountOptions options)
		{
			return 0u;
		}

		public uint GetItemImageInfoCount(ref GetItemImageInfoCountOptions options)
		{
			return 0u;
		}

		public uint GetItemReleaseCount(ref GetItemReleaseCountOptions options)
		{
			return 0u;
		}

		public uint GetLastRedeemedEntitlementsCount(ref GetLastRedeemedEntitlementsCountOptions options)
		{
			return 0u;
		}

		public uint GetOfferCount(ref GetOfferCountOptions options)
		{
			return 0u;
		}

		public uint GetOfferImageInfoCount(ref GetOfferImageInfoCountOptions options)
		{
			return 0u;
		}

		public uint GetOfferItemCount(ref GetOfferItemCountOptions options)
		{
			return 0u;
		}

		public uint GetTransactionCount(ref GetTransactionCountOptions options)
		{
			return 0u;
		}

		public void QueryEntitlementToken(ref QueryEntitlementTokenOptions options, object clientData, OnQueryEntitlementTokenCallback completionDelegate)
		{
		}

		public void QueryEntitlements(ref QueryEntitlementsOptions options, object clientData, OnQueryEntitlementsCallback completionDelegate)
		{
		}

		public void QueryOffers(ref QueryOffersOptions options, object clientData, OnQueryOffersCallback completionDelegate)
		{
		}

		public void QueryOwnership(ref QueryOwnershipOptions options, object clientData, OnQueryOwnershipCallback completionDelegate)
		{
		}

		public void QueryOwnershipBySandboxIds(ref QueryOwnershipBySandboxIdsOptions options, object clientData, OnQueryOwnershipBySandboxIdsCallback completionDelegate)
		{
		}

		public void QueryOwnershipToken(ref QueryOwnershipTokenOptions options, object clientData, OnQueryOwnershipTokenCallback completionDelegate)
		{
		}

		public void RedeemEntitlements(ref RedeemEntitlementsOptions options, object clientData, OnRedeemEntitlementsCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnCheckoutCallbackInternal))]
		internal static void OnCheckoutCallbackInternalImplementation(ref CheckoutCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryEntitlementTokenCallbackInternal))]
		internal static void OnQueryEntitlementTokenCallbackInternalImplementation(ref QueryEntitlementTokenCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryEntitlementsCallbackInternal))]
		internal static void OnQueryEntitlementsCallbackInternalImplementation(ref QueryEntitlementsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryOffersCallbackInternal))]
		internal static void OnQueryOffersCallbackInternalImplementation(ref QueryOffersCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryOwnershipBySandboxIdsCallbackInternal))]
		internal static void OnQueryOwnershipBySandboxIdsCallbackInternalImplementation(ref QueryOwnershipBySandboxIdsCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryOwnershipCallbackInternal))]
		internal static void OnQueryOwnershipCallbackInternalImplementation(ref QueryOwnershipCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnQueryOwnershipTokenCallbackInternal))]
		internal static void OnQueryOwnershipTokenCallbackInternalImplementation(ref QueryOwnershipTokenCallbackInfoInternal data)
		{
		}

		[MonoPInvokeCallback(typeof(OnRedeemEntitlementsCallbackInternal))]
		internal static void OnRedeemEntitlementsCallbackInternalImplementation(ref RedeemEntitlementsCallbackInfoInternal data)
		{
		}
	}
}
