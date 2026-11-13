using System;
using System.Runtime.InteropServices;
using System.Text;
using AOT;

namespace DiscordConnect
{
	public class LoginSessionRFC6749
	{
		private class BoxedExchangeCallback
		{
			public ExchangeHandler callback;

			public BoxedExchangeCallback(ExchangeHandler callback)
			{
			}
		}

		private struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void DestroyMethod(IntPtr instance);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void GetAuthorizationUrlMethod(IntPtr instance, StringBuilder url);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void ExchangeCallback(IntPtr instance, Result result, ExchangeData exchangeData);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void ExchangeMethod(IntPtr instance, string accessCode, string state, IntPtr callbackData, ExchangeCallback callback);

			public DestroyMethod Destroy;

			public GetAuthorizationUrlMethod GetAuthorizationUrl;

			public ExchangeMethod Exchange;
		}

		public delegate void ExchangeHandler(Result result, ExchangeData exchangeData);

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		private FFIMethods Methods => default;

		public LoginSessionRFC6749(long clientId, string redirectURI, OAuth2Scopes[] scopes)
		{
		}

		~LoginSessionRFC6749()
		{
		}

		private string GetAuthorizationUrl()
		{
			return null;
		}

		public void OpenAuthorizationUrl()
		{
		}

		public void Exchange(string accessCode, string state, ExchangeHandler callback)
		{
		}

		[MonoPInvokeCallback(typeof(FFIMethods.ExchangeCallback))]
		private static void ExchangeCallbackFFI(IntPtr ptr, Result result, ExchangeData exchangeData)
		{
		}

		[PreserveSig]
		private static extern IntPtr login_manager_rfc6749_new(long client_id, string redirect_uri, string scopes);
	}
}
