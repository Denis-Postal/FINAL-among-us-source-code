using System;

namespace Epic.OnlineServices.ReceiptValidator
{
	public sealed class ReceiptValidatorInterface : Handle
	{
		public const int AndroidIapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int IosIapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int Ps4DlcverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int Ps4IapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int Ps5DlcverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int Ps5IapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int EgsDlcverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int EgsIapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int SteamDlcverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int SteamIapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int VerifypurchaseApiLatest = 1;

		public const int SwitchIapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int XboxonegdkDlcverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int XboxonegdkIapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int XsxDlcverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public const int XsxIapverifypurchaseoptionsplatformspecificoptionsApiLatest = 1;

		public void VerifyPurchase(ref AndroidIAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref IOSIAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref PS4IAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref PS4DLCVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref PS5IAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref PS5DLCVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public ReceiptValidatorInterface()
		{
		}

		public ReceiptValidatorInterface(IntPtr innerHandle)
		{
		}

		public void VerifyPurchase(ref VerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref SteamIAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref SteamDLCVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref EGSIAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref EGSDLCVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		[MonoPInvokeCallback(typeof(OnVerifyPurchaseCompleteCallbackInternal))]
		internal static void OnVerifyPurchaseCompleteCallbackInternalImplementation(ref VerifyPurchaseCompleteCallbackInfoInternal data)
		{
		}

		public void VerifyPurchase(ref SwitchIAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref XboxOneGDKIAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref XboxOneGDKDLCVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref XSXIAPVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}

		public void VerifyPurchase(ref XSXDLCVerifyPurchaseOptions options, object clientData, OnVerifyPurchaseCompleteCallback completionDelegate)
		{
		}
	}
}
