using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DiscordConnect
{
	public class LoginSession
	{
		private struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void DestroyMethod(IntPtr instance);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void GetAuthorizationUrlMethod(IntPtr instance, StringBuilder url);

			public DestroyMethod Destroy;

			public GetAuthorizationUrlMethod GetAuthorizationUrl;
		}

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		private FFIMethods Methods => default;

		public LoginSession(long clientId, string redirectURI, OAuth2Scopes[] scopes)
		{
		}

		~LoginSession()
		{
		}

		private string GetAuthorizationUrl()
		{
			return null;
		}

		public void OpenAuthorizationUrl()
		{
		}

		[PreserveSig]
		private static extern IntPtr login_manager_new(long client_id, string redirect_uri, string scopes);
	}
}
