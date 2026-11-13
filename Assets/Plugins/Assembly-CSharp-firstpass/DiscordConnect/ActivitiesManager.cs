using System;
using System.Runtime.InteropServices;

namespace DiscordConnect
{
	public class ActivitiesManager
	{
		private struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void UpdateMethod(IntPtr instance, ref Activity activity);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void ClearMethod(IntPtr instance);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate Result GetMethod(IntPtr instance, ref Activity activity);

			public UpdateMethod Update;

			public ClearMethod Clear;

			public GetMethod Get;
		}

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		private FFIMethods Methods => default;

		internal ActivitiesManager(IntPtr ptr)
		{
		}

		public Activity GetCurrent()
		{
			return default;
		}

		public void Update(Activity activity)
		{
		}

		public void Clear()
		{
		}

		public void Share(string message)
		{
		}
	}
}
