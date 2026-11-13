using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnCloseCompleteCallbackInternal(ref OnCloseCompleteCallbackInfoInternal data);
}
