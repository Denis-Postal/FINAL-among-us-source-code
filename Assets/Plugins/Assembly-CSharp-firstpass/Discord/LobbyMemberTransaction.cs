using System;
using System.Runtime.InteropServices;

namespace Discord
{
	public struct LobbyMemberTransaction
	{
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, string key, string value);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, string key);

			internal SetMetadataMethod SetMetadata;

			internal DeleteMetadataMethod DeleteMetadata;
		}

		internal IntPtr MethodsPtr;

		internal object MethodsStructure;

		private FFIMethods Methods => default;

		public void SetMetadata(string key, string value)
		{
		}

		public void DeleteMetadata(string key)
		{
		}
	}
}
