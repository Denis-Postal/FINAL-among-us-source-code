using System.Runtime.InteropServices;

namespace Epic.OnlineServices
{
	public static class Config
	{
		#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
		public const string LibraryName = "EOSSDK-Linux-Shipping";
		#else
		public const string LibraryName = "EOSSDK-Win64-Shipping";
		#endif

		public const CallingConvention LibraryCallingConvention = CallingConvention.Cdecl;
	}
}
