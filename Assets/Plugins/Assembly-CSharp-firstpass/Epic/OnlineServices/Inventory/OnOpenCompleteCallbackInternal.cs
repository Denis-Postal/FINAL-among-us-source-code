using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Inventory
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void OnOpenCompleteCallbackInternal(ref OnOpenCompleteCallbackInfoInternal data);
}
