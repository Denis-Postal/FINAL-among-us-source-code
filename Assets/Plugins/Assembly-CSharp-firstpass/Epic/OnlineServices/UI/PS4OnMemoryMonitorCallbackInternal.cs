using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void PS4OnMemoryMonitorCallbackInternal(ref PS4MemoryMonitorCallbackInfoInternal data);
}
