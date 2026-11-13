using System;
using System.Runtime.InteropServices;

namespace DiscordConnect
{
	public class UsersManager
	{
		private struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate User GetCurrentMethod(IntPtr instance);

			public GetCurrentMethod GetCurrent;
		}

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		private FFIMethods Methods => default;

		internal UsersManager(IntPtr ptr)
		{
		}

		public User GetCurrent()
		{
			return default;
		}
	}
}
