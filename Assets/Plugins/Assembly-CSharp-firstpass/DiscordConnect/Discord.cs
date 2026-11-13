using System;
using System.Runtime.InteropServices;
using AOT;

namespace DiscordConnect
{
	public class Discord
	{
		private struct DiscordCreateParams
		{
			public string AccessToken;

			public IntPtr EventData;

			public FFIMethods.EventCallback OnEvent;

			public IntPtr ConnectData;

			public FFIMethods.ConnectCallback OnConnect;
		}

		public delegate void EventHandler(Event e);

		public delegate void ConnectHandler(Result result, Discord discord);

		private class BoxedEventCallback
		{
			public EventHandler callback;

			public BoxedEventCallback(EventHandler callback)
			{
			}
		}

		private class BoxedConnectCallback
		{
			public ConnectHandler callback;

			public BoxedConnectCallback(ConnectHandler callback)
			{
			}
		}

		private struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void DestroyMethod(IntPtr instance);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate IntPtr GetUsersManagerMethod(IntPtr instance);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate IntPtr GetRelationshipsManagerMethod(IntPtr instance);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate IntPtr GetActivitiesManagerMethod(IntPtr instance);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate Result RouteMethod(IntPtr instance, string url);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void EventCallback(IntPtr instance, Event e);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void ConnectCallback(IntPtr instance, Result result, IntPtr discord);

			public DestroyMethod Destroy;

			public GetUsersManagerMethod GetUsersManager;

			public GetRelationshipsManagerMethod GetRelationshipsManager;

			public GetActivitiesManagerMethod GetActivitiesManager;

			public RouteMethod Route;
		}

		public delegate void ExchangeHandler(Result result, ExchangeData exchangeData);

		public UsersManager UsersManager;

		public RelationshipsManager RelationshipsManager;

		public ActivitiesManager ActivitiesManager;

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		private FFIMethods Methods => default;

		private Discord(IntPtr ptr)
		{
		}

		~Discord()
		{
		}

		public void Route(string url)
		{
		}

		public static void Connect(string accessToken, EventHandler eventHandler, ConnectHandler connectHandler)
		{
		}

		public static void Route(Uri uri, EventHandler eventHandler)
		{
		}

		[MonoPInvokeCallback(typeof(FFIMethods.EventCallback))]
		private static void EventCallbackFFI(IntPtr ptr, Event e)
		{
		}

		[MonoPInvokeCallback(typeof(FFIMethods.EventCallback))]
		private static void EventCallbackThenFreeFFI(IntPtr ptr, Event e)
		{
		}

		[MonoPInvokeCallback(typeof(FFIMethods.ConnectCallback))]
		private static void ConnectCallbackFFI(IntPtr ptr, Result result, IntPtr discordPtr)
		{
		}

		[PreserveSig]
		private static extern IntPtr discord_connect_connect(ref DiscordCreateParams createParams);

		[PreserveSig]
		private static extern Result discord_route(string AccessToken, IntPtr EventData, FFIMethods.EventCallback OnEvent);
	}
}
