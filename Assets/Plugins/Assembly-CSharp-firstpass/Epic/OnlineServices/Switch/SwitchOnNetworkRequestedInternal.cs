using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Switch
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int SwitchOnNetworkRequestedInternal();
}
