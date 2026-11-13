using System;
using System.Runtime.InteropServices;

namespace Discord
{
	public struct LobbySearchQuery
	{
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result FilterMethod(IntPtr methodsPtr, string key, LobbySearchComparison comparison, LobbySearchCast cast, string value);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result SortMethod(IntPtr methodsPtr, string key, LobbySearchCast cast, string value);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result LimitMethod(IntPtr methodsPtr, uint limit);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result DistanceMethod(IntPtr methodsPtr, LobbySearchDistance distance);

			internal FilterMethod Filter;

			internal SortMethod Sort;

			internal LimitMethod Limit;

			internal DistanceMethod Distance;
		}

		internal IntPtr MethodsPtr;

		internal object MethodsStructure;

		private FFIMethods Methods => default;

		public void Filter(string key, LobbySearchComparison comparison, LobbySearchCast cast, string value)
		{
		}

		public void Sort(string key, LobbySearchCast cast, string value)
		{
		}

		public void Limit(uint limit)
		{
		}

		public void Distance(LobbySearchDistance distance)
		{
		}
	}
}
