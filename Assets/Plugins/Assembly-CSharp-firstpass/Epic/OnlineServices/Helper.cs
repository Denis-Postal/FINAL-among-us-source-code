using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices
{
	public sealed class Helper
	{
		private struct Allocation
		{
			public int Size { get; private set; }

			public object Cache { get; private set; }

			public bool? IsArrayItemAllocated { get; private set; }

			public Allocation(int size, object cache, bool? isArrayItemAllocated = null)
			{
				Size = 0;
				Cache = null;
				IsArrayItemAllocated = null;
			}
		}

		private struct PinnedBuffer
		{
			public GCHandle Handle { get; private set; }

			public int RefCount { get; set; }

			public PinnedBuffer(GCHandle handle)
			{
				Handle = default;
				RefCount = 0;
			}
		}

		private class DelegateHolder
		{
			public Delegate Public { get; private set; }

			public Delegate Private { get; private set; }

			public Delegate[] StructDelegates { get; private set; }

			public ulong? NotificationId { get; set; }

			public DelegateHolder(Delegate publicDelegate, Delegate privateDelegate, params Delegate[] structDelegates)
			{
			}
		}

		private static Dictionary<ulong, Allocation> s_Allocations;

		private static Dictionary<ulong, PinnedBuffer> s_PinnedBuffers;

		private static Dictionary<IntPtr, DelegateHolder> s_Callbacks;

		private static Dictionary<string, DelegateHolder> s_StaticCallbacks;

		private static long s_LastClientDataId;

		private static Dictionary<IntPtr, object> s_ClientDatas;

		internal static void AddCallback(out IntPtr clientDataAddress, object clientData, Delegate publicDelegate, Delegate privateDelegate, params Delegate[] structDelegates)
		{
			clientDataAddress = default;
		}

		private static void RemoveCallback(IntPtr clientDataAddress)
		{
		}

		internal static bool TryGetCallback<TCallbackInfoInternal, TCallback, TCallbackInfo>(ref TCallbackInfoInternal callbackInfoInternal, out TCallback callback, out TCallbackInfo callbackInfo) where TCallbackInfoInternal : struct, ICallbackInfoInternal, IGettable<TCallbackInfo> where TCallback : class where TCallbackInfo : struct, ICallbackInfo
		{
			callback = null;
			callbackInfo = default;
			return false;
		}

		internal static bool TryGetAndRemoveCallback<TCallbackInfoInternal, TCallback, TCallbackInfo>(ref TCallbackInfoInternal callbackInfoInternal, out TCallback callback, out TCallbackInfo callbackInfo) where TCallbackInfoInternal : struct, ICallbackInfoInternal, IGettable<TCallbackInfo> where TCallback : class where TCallbackInfo : struct, ICallbackInfo
		{
			callback = null;
			callbackInfo = default;
			return false;
		}

		internal static bool TryGetStructCallback<TCallbackInfoInternal, TCallback, TCallbackInfo>(ref TCallbackInfoInternal callbackInfoInternal, out TCallback callback, out TCallbackInfo callbackInfo) where TCallbackInfoInternal : struct, ICallbackInfoInternal, IGettable<TCallbackInfo> where TCallback : class where TCallbackInfo : struct
		{
			callback = null;
			callbackInfo = default;
			return false;
		}

		internal static void RemoveCallbackByNotificationId(ulong notificationId)
		{
		}

		internal static void AddStaticCallback(string key, Delegate publicDelegate, Delegate privateDelegate)
		{
		}

		internal static bool TryGetStaticCallback<TCallback>(string key, out TCallback callback) where TCallback : class
		{
			callback = null;
			return false;
		}

		internal static void AssignNotificationIdToCallback(IntPtr clientDataAddress, ulong notificationId)
		{
		}

		private static IntPtr AddClientData(object clientData)
		{
			return (IntPtr)0;
		}

		private static void RemoveClientData(IntPtr clientDataAddress)
		{
		}

		private static object GetClientData(IntPtr clientDataAddress)
		{
			return null;
		}

		private static void Convert<THandle>(IntPtr from, out THandle to) where THandle : Handle, new()
		{
			to = null;
		}

		private static void Convert(Handle from, out IntPtr to)
		{
			to = default;
		}

		private static void Convert(byte[] from, out string to)
		{
			to = null;
		}

		private static void Convert(string from, out byte[] to, int fromLength)
		{
			to = null;
		}

		private static void Convert<TArray>(TArray[] from, out int to)
		{
			to = default;
		}

		private static void Convert<TArray>(TArray[] from, out uint to)
		{
			to = default;
		}

		private static void Convert<TArray>(ArraySegment<TArray> from, out int to)
		{
			to = default;
		}

		private static void Convert<T>(ArraySegment<T> from, out uint to)
		{
			to = default;
		}

		private static void Convert(int from, out bool to)
		{
			to = default;
		}

		private static void Convert(bool from, out int to)
		{
			to = default;
		}

		private static void Convert(DateTimeOffset? from, out long to)
		{
			to = default;
		}

		private static void Convert(long from, out DateTimeOffset? to)
		{
			to = null;
		}

		internal static void Get<TArray>(TArray[] from, out int to)
		{
			to = default;
		}

		internal static void Get<TArray>(TArray[] from, out uint to)
		{
			to = default;
		}

		internal static void Get<TArray>(ArraySegment<TArray> from, out uint to)
		{
			to = default;
		}

		internal static void Get<TTo>(IntPtr from, out TTo to) where TTo : Handle, new()
		{
			to = null;
		}

		internal static void Get<TFrom, TTo>(ref TFrom from, out TTo to) where TFrom : struct, IGettable<TTo> where TTo : struct
		{
			to = default;
		}

		internal static void Get(int from, out bool to)
		{
			to = default;
		}

		internal static void Get(bool from, out int to)
		{
			to = default;
		}

		internal static void Get(long from, out DateTimeOffset? to)
		{
			to = null;
		}

		internal static void Get<TTo>(IntPtr from, out TTo[] to, int arrayLength, bool isArrayItemAllocated)
		{
			to = null;
		}

		internal static void Get<TTo>(IntPtr from, out TTo[] to, uint arrayLength, bool isArrayItemAllocated)
		{
			to = null;
		}

		internal static void Get<TTo>(IntPtr from, out TTo[] to, int arrayLength)
		{
			to = null;
		}

		internal static void Get<TTo>(IntPtr from, out TTo[] to, uint arrayLength)
		{
			to = null;
		}

		internal static void Get(IntPtr from, out ArraySegment<byte> to, uint arrayLength)
		{
			to = default;
		}

		internal static void GetHandle<THandle>(IntPtr from, out THandle[] to, uint arrayLength) where THandle : Handle, new()
		{
			to = null;
		}

		internal static void Get<TFrom, TTo>(TFrom[] from, out TTo[] to) where TFrom : struct, IGettable<TTo> where TTo : struct
		{
			to = null;
		}

		internal static void Get<TFrom, TTo>(IntPtr from, out TTo[] to, int arrayLength) where TFrom : struct, IGettable<TTo> where TTo : struct
		{
			to = null;
		}

		internal static void Get<TFrom, TTo>(IntPtr from, out TTo[] to, uint arrayLength) where TFrom : struct, IGettable<TTo> where TTo : struct
		{
			to = null;
		}

		internal static void Get<TTo>(IntPtr from, out TTo? to) where TTo : struct
		{
			to = null;
		}

		internal static void Get(byte[] from, out string to)
		{
			to = null;
		}

		internal static void Get(IntPtr from, out object to)
		{
			to = null;
		}

		internal static void Get(IntPtr from, out Utf8String to)
		{
			to = null;
		}

		internal static void Get<T, TEnum>(T from, out T to, TEnum currentEnum, TEnum expectedEnum)
		{
			to = default;
		}

		internal static void Get<TFrom, TTo, TEnum>(ref TFrom from, out TTo to, TEnum currentEnum, TEnum expectedEnum) where TFrom : struct, IGettable<TTo> where TTo : struct
		{
			to = default;
		}

		internal static void Get<TEnum>(int from, out bool? to, TEnum currentEnum, TEnum expectedEnum)
		{
			to = null;
		}

		internal static void Get<TFrom, TEnum>(TFrom from, out TFrom? to, TEnum currentEnum, TEnum expectedEnum) where TFrom : struct
		{
			to = null;
		}

		internal static void Get<TFrom, TEnum>(IntPtr from, out TFrom to, TEnum currentEnum, TEnum expectedEnum) where TFrom : Handle, new()
		{
			to = null;
		}

		internal static void Get<TEnum>(IntPtr from, out IntPtr? to, TEnum currentEnum, TEnum expectedEnum)
		{
			to = null;
		}

		internal static void Get<TEnum>(IntPtr from, out Utf8String to, TEnum currentEnum, TEnum expectedEnum)
		{
			to = null;
		}

		internal static void Get<TFrom, TTo>(IntPtr from, out TTo to) where TFrom : struct, IGettable<TTo> where TTo : struct
		{
			to = default;
		}

		internal static void Get<TFrom, TTo>(IntPtr from, out TTo? to) where TFrom : struct, IGettable<TTo> where TTo : struct
		{
			to = null;
		}

		internal static void Get<TFrom, TTo>(ref TFrom from, out TTo to, out IntPtr clientDataAddress) where TFrom : struct, ICallbackInfoInternal, IGettable<TTo> where TTo : struct
		{
			to = default;
			clientDataAddress = default;
		}

		public static int GetAllocationCount()
		{
			return 0;
		}

		internal static void Copy(byte[] from, IntPtr to)
		{
		}

		internal static void Copy(ArraySegment<byte> from, IntPtr to)
		{
		}

		internal static void Dispose(ref IntPtr value)
		{
		}

		internal static void Dispose<TDisposable>(ref TDisposable disposable) where TDisposable : IDisposable
		{
		}

		internal static void Dispose<TEnum>(ref IntPtr value, TEnum currentEnum, TEnum expectedEnum)
		{
		}

		private static int GetAnsiStringLength(byte[] bytes)
		{
			return 0;
		}

		private static int GetAnsiStringLength(IntPtr address)
		{
			return 0;
		}

		internal static T GetDefault<T>()
		{
			return default;
		}

		private static void GetAllocation<T>(IntPtr source, out T target)
		{
			target = default;
		}

		private static void GetAllocation<T>(IntPtr source, out T? target) where T : struct
		{
			target = null;
		}

		private static void GetAllocation<THandle>(IntPtr source, out THandle[] target, int arrayLength) where THandle : Handle, new()
		{
			target = null;
		}

		private static void GetAllocation<T>(IntPtr from, out T[] to, int arrayLength, bool isArrayItemAllocated)
		{
			to = null;
		}

		private static void GetAllocation(IntPtr source, out Utf8String target)
		{
			target = null;
		}

		internal static IntPtr AddAllocation(int size)
		{
			return (IntPtr)0;
		}

		internal static IntPtr AddAllocation(uint size)
		{
			return (IntPtr)0;
		}

		private static IntPtr AddAllocation<T>(int size, T cache)
		{
			return (IntPtr)0;
		}

		private static IntPtr AddAllocation<T>(int size, T[] cache, bool? isArrayItemAllocated)
		{
			return (IntPtr)0;
		}

		private static IntPtr AddAllocation<T>(T[] array, bool isArrayItemAllocated)
		{
			return (IntPtr)0;
		}

		private static void RemoveAllocation(ref IntPtr address)
		{
		}

		private static bool TryGetAllocationCache(IntPtr address, out object cache)
		{
			cache = null;
			return false;
		}

		private static IntPtr AddPinnedBuffer(byte[] buffer, int offset)
		{
			return (IntPtr)0;
		}

		private static IntPtr AddPinnedBuffer(Utf8String str)
		{
			return (IntPtr)0;
		}

		internal static IntPtr AddPinnedBuffer(ArraySegment<byte> array)
		{
			return (IntPtr)0;
		}

		private static void RemovePinnedBuffer(ref IntPtr address)
		{
		}

		internal static void Set<T>(ref T from, ref T to) where T : struct
		{
		}

		internal static void Set(object from, ref IntPtr to)
		{
		}

		internal static void Set(Utf8String from, ref IntPtr to)
		{
		}

		internal static void Set(Handle from, ref IntPtr to)
		{
		}

		internal static void Set<T>(T? from, ref IntPtr to) where T : struct
		{
		}

		internal static void Set<T>(T[] from, ref IntPtr to, bool isArrayItemAllocated)
		{
		}

		internal static void Set(ArraySegment<byte> from, ref IntPtr to, out uint arrayLength)
		{
			arrayLength = default;
		}

		internal static void Set<T>(T[] from, ref IntPtr to)
		{
		}

		internal static void Set<T>(T[] from, ref IntPtr to, bool isArrayItemAllocated, out int arrayLength)
		{
			arrayLength = default;
		}

		internal static void Set<T>(T[] from, ref IntPtr to, bool isArrayItemAllocated, out uint arrayLength)
		{
			arrayLength = default;
		}

		internal static void Set<T>(T[] from, ref IntPtr to, out int arrayLength)
		{
			arrayLength = default;
		}

		internal static void Set<T>(T[] from, ref IntPtr to, out uint arrayLength)
		{
			arrayLength = default;
		}

		internal static void Set(DateTimeOffset? from, ref long to)
		{
		}

		internal static void Set(bool from, ref int to)
		{
		}

		internal static void Set(string from, ref byte[] to, int stringLength)
		{
		}

		internal static void Set<T, TEnum>(T from, ref T to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null)
		{
		}

		internal static void Set<TFrom, TEnum, TTo>(ref TFrom from, ref TTo to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null) where TFrom : struct where TTo : struct, ISettable<TFrom>
		{
		}

		internal static void Set<T, TEnum>(T? from, ref T to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null) where T : struct
		{
		}

		internal static void Set<TEnum>(Handle from, ref IntPtr to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null)
		{
		}

		internal static void Set<TEnum>(Utf8String from, ref IntPtr to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null)
		{
		}

		internal static void Set<TEnum>(bool? from, ref int to, TEnum fromEnum, ref TEnum toEnum, IDisposable disposable = null)
		{
		}

		internal static void Set<TFrom, TIntermediate>(ref TFrom from, ref IntPtr to) where TFrom : struct where TIntermediate : struct, ISettable<TFrom>
		{
		}

		internal static void Set<TFrom, TIntermediate>(ref TFrom? from, ref IntPtr to) where TFrom : struct where TIntermediate : struct, ISettable<TFrom>
		{
		}

		internal static void Set<TFrom, TTo>(ref TFrom from, ref TTo to) where TFrom : struct where TTo : struct, ISettable<TFrom>
		{
		}

		internal static void Set<TFrom, TIntermediate>(ref TFrom[] from, ref IntPtr to, out int arrayLength) where TFrom : struct where TIntermediate : struct, ISettable<TFrom>
		{
			arrayLength = default;
		}

		internal static void Set<TFrom, TIntermediate>(ref TFrom[] from, ref IntPtr to, out uint arrayLength) where TFrom : struct where TIntermediate : struct, ISettable<TFrom>
		{
			arrayLength = default;
		}
	}
}
