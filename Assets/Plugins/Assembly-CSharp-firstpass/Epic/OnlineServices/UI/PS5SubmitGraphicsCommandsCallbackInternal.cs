using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void PS5SubmitGraphicsCommandsCallbackInternal(ref PS5SubmitGraphicsCommandsCallbackInfoInternal data);
}
