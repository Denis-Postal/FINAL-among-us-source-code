using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace DiscordConnect
{
	public class RelationshipsManager
	{
		private class BoxedGetCallback
		{
			public GetHandler callback;

			public RelationshipsManager self;

			public BoxedGetCallback(GetHandler callback, RelationshipsManager self)
			{
			}
		}

		public delegate void GetHandler(Result result, IEnumerable<Relationship> relationships);

		private struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void CountMethod(IntPtr instance, ref int count);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void GetMethod(IntPtr instance, IntPtr data, GetRelationshipsCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void GetLatestMethod(IntPtr instance, IntPtr data, GetRelationshipsCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void GetAtMethod(IntPtr instance, uint index, ref Relationship relationship);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void GetRelationshipsCallback(IntPtr data, Result result);

			public CountMethod Count;

			public GetMethod Get;

			public GetLatestMethod GetLatest;

			public GetAtMethod GetAt;
		}

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		private FFIMethods Methods => default;

		internal RelationshipsManager(IntPtr ptr)
		{
		}

		private uint Count()
		{
			return 0u;
		}

		public void Get(GetHandler handler)
		{
		}

		public void GetLatest(GetHandler handler)
		{
		}

		private Relationship GetAt(uint index)
		{
			return default;
		}

		[MonoPInvokeCallback(typeof(FFIMethods.GetRelationshipsCallback))]
		private static void GetCallbackFFI(IntPtr ptr, Result result)
		{
		}
	}
}
