using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void PS4SubmitGraphicsCommandsCallbackInternal(ref PS4SubmitGraphicsCommandsCallbackInfoInternal data);
}
