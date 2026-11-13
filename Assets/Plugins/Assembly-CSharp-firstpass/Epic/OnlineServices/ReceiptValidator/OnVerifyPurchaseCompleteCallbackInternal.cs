using System.Runtime.InteropServices;

namespace Epic.OnlineServices.ReceiptValidator
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnVerifyPurchaseCompleteCallbackInternal(ref VerifyPurchaseCompleteCallbackInfoInternal data);
}
