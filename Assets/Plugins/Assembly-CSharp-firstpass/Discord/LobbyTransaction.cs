using System;
using System.Runtime.InteropServices;

namespace Discord
{
	public struct LobbyTransaction
	{
		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result SetTypeMethod(IntPtr methodsPtr, LobbyType type);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result SetOwnerMethod(IntPtr methodsPtr, long ownerId);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result SetCapacityMethod(IntPtr methodsPtr, uint capacity);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result SetMetadataMethod(IntPtr methodsPtr, string key, string value);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result DeleteMetadataMethod(IntPtr methodsPtr, string key);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result SetLockedMethod(IntPtr methodsPtr, bool locked);

			internal SetTypeMethod SetType;

			internal SetOwnerMethod SetOwner;

			internal SetCapacityMethod SetCapacity;

			internal SetMetadataMethod SetMetadata;

			internal DeleteMetadataMethod DeleteMetadata;

			internal SetLockedMethod SetLocked;
		}

		internal IntPtr MethodsPtr;

		internal object MethodsStructure;

		private FFIMethods Methods => default;

		public void SetType(LobbyType type)
		{
		}

		public void SetOwner(long ownerId)
		{
		}

		public void SetCapacity(uint capacity)
		{
		}

		public void SetMetadata(string key, string value)
		{
		}

		public void DeleteMetadata(string key)
		{
		}

		public void SetLocked(bool locked)
		{
		}
	}
}
