using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void PS5OnMemoryMonitorCallbackInternal(ref PS5MemoryMonitorCallbackInfoInternal data);
}
