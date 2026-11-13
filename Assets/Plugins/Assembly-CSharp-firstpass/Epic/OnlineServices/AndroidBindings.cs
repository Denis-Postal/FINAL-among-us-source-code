using System.Runtime.InteropServices;
using Epic.OnlineServices.Platform;

namespace Epic.OnlineServices
{
	public static class AndroidBindings
	{
		[PreserveSig]
		internal static extern Result EOS_Initialize(ref AndroidInitializeOptionsInternal options);
	}
}
