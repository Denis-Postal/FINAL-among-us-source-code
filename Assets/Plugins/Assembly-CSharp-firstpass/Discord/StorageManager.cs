using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Discord
{
	public class StorageManager
	{
		internal struct FFIEvents
		{
		}

		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result ReadMethod(IntPtr methodsPtr, string name, byte[] data, int dataLen, ref uint read);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ReadAsyncCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ReadAsyncMethod(IntPtr methodsPtr, string name, IntPtr callbackData, ReadAsyncCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ReadAsyncPartialCallback(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void ReadAsyncPartialMethod(IntPtr methodsPtr, string name, ulong offset, ulong length, IntPtr callbackData, ReadAsyncPartialCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result WriteMethod(IntPtr methodsPtr, string name, byte[] data, int dataLen);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void WriteAsyncCallback(IntPtr ptr, Result result);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void WriteAsyncMethod(IntPtr methodsPtr, string name, byte[] data, int dataLen, IntPtr callbackData, WriteAsyncCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result DeleteMethod(IntPtr methodsPtr, string name);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result ExistsMethod(IntPtr methodsPtr, string name, ref bool exists);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate void CountMethod(IntPtr methodsPtr, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result StatMethod(IntPtr methodsPtr, string name, ref FileStat stat);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result StatAtMethod(IntPtr methodsPtr, int index, ref FileStat stat);

			[UnmanagedFunctionPointer(CallingConvention.Winapi)]
			internal delegate Result GetPathMethod(IntPtr methodsPtr, StringBuilder path);

			internal ReadMethod Read;

			internal ReadAsyncMethod ReadAsync;

			internal ReadAsyncPartialMethod ReadAsyncPartial;

			internal WriteMethod Write;

			internal WriteAsyncMethod WriteAsync;

			internal DeleteMethod Delete;

			internal ExistsMethod Exists;

			internal CountMethod Count;

			internal StatMethod Stat;

			internal StatAtMethod StatAt;

			internal GetPathMethod GetPath;
		}

		public delegate void ReadAsyncHandler(Result result, byte[] data);

		public delegate void ReadAsyncPartialHandler(Result result, byte[] data);

		public delegate void WriteAsyncHandler(Result result);

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		private FFIMethods Methods => default;

		internal StorageManager(IntPtr ptr, IntPtr eventsPtr, ref FFIEvents events)
		{
		}

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
		}

		public uint Read(string name, byte[] data)
		{
			return 0u;
		}

		[MonoPInvokeCallback]
		private static void ReadAsyncCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
		}

		public void ReadAsync(string name, ReadAsyncHandler callback)
		{
		}

		[MonoPInvokeCallback]
		private static void ReadAsyncPartialCallbackImpl(IntPtr ptr, Result result, IntPtr dataPtr, int dataLen)
		{
		}

		public void ReadAsyncPartial(string name, ulong offset, ulong length, ReadAsyncPartialHandler callback)
		{
		}

		public void Write(string name, byte[] data)
		{
		}

		[MonoPInvokeCallback]
		private static void WriteAsyncCallbackImpl(IntPtr ptr, Result result)
		{
		}

		public void WriteAsync(string name, byte[] data, WriteAsyncHandler callback)
		{
		}

		public void Delete(string name)
		{
		}

		public bool Exists(string name)
		{
			return false;
		}

		public int Count()
		{
			return 0;
		}

		public FileStat Stat(string name)
		{
			return default;
		}

		public FileStat StatAt(int index)
		{
			return default;
		}

		public string GetPath()
		{
			return null;
		}

		public IEnumerable<FileStat> Files()
		{
			return null;
		}
	}
}
