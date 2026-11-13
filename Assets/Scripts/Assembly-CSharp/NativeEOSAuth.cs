using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Steamworks;
using UnityEngine;

public static class NativeEOSAuth
{
	#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
	private const string EOS_DLL = "EOSSDK-Linux-Shipping";
	#else
	private const string EOS_DLL = "EOSSDK-Win64-Shipping";
	#endif

	public static string AuthenticUserIDToken { get; private set; }
	public static string AuthenticProductUserId { get; private set; } = "000227e2228b43a38d2ef99355ed1d26";
	public static bool IsAuthenticated { get; private set; }

	private static IntPtr platformHandle = IntPtr.Zero;
	private static IntPtr connectHandle = IntPtr.Zero;
	private static bool eosInitialized = false;

	[StructLayout(LayoutKind.Sequential)]
	private struct EOS_InitializeOptions
	{
		public int ApiVersion;
		public IntPtr AllocateMemoryFunction;
		public IntPtr ReallocateMemoryFunction;
		public IntPtr ReleaseMemoryFunction;
		[MarshalAs(UnmanagedType.LPStr)]
		public string ProductName;
		[MarshalAs(UnmanagedType.LPStr)]
		public string ProductVersion;
		public IntPtr Reserved;
		public IntPtr SystemInitializeOptions;
		public IntPtr OverrideThreadAffinity;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct EOS_Platform_ClientCredentials
	{
		[MarshalAs(UnmanagedType.LPStr)]
		public string ClientId;
		[MarshalAs(UnmanagedType.LPStr)]
		public string ClientSecret;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct EOS_Platform_Options
	{
		public int ApiVersion;
		public IntPtr Reserved;
		[MarshalAs(UnmanagedType.LPStr)]
		public string ProductId;
		[MarshalAs(UnmanagedType.LPStr)]
		public string SandboxId;
		public EOS_Platform_ClientCredentials ClientCredentials;
		public int IsServer;
		[MarshalAs(UnmanagedType.LPStr)]
		public string EncryptionKey;
		[MarshalAs(UnmanagedType.LPStr)]
		public string OverrideCountryCode;
		[MarshalAs(UnmanagedType.LPStr)]
		public string OverrideLocaleCode;
		[MarshalAs(UnmanagedType.LPStr)]
		public string DeploymentId;
		public ulong Flags;
		[MarshalAs(UnmanagedType.LPStr)]
		public string CacheDirectory;
		public uint TickBudgetInMilliseconds;
		public IntPtr RTCOptions;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct EOS_Connect_Credentials
	{
		public int ApiVersion;
		[MarshalAs(UnmanagedType.LPStr)]
		public string Token;
		public int Type;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct EOS_Connect_LoginOptions
	{
		public int ApiVersion;
		public IntPtr Credentials;
		public IntPtr UserLoginInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct EOS_Connect_LoginCallbackInfo
	{
		public int ResultCode;
		public IntPtr ClientData;
		public IntPtr LocalUserId;
		public IntPtr ContinuanceToken;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct EOS_Connect_CopyIdTokenOptions
	{
		public int ApiVersion;
		public IntPtr LocalUserId;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct EOS_Connect_IdToken
	{
		public int ApiVersion;
		public IntPtr ProductUserId;
		public IntPtr JsonWebToken;
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void EOS_Connect_OnLoginCallback(ref EOS_Connect_LoginCallbackInfo data);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern int EOS_Initialize(ref EOS_InitializeOptions options);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr EOS_Platform_Create(ref EOS_Platform_Options options);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr EOS_Platform_GetConnectInterface(IntPtr handle);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern void EOS_Connect_Login(IntPtr handle, ref EOS_Connect_LoginOptions options, IntPtr clientData, EOS_Connect_OnLoginCallback completionDelegate);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern void EOS_Platform_Tick(IntPtr handle);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern int EOS_Connect_CopyIdToken(IntPtr handle, ref EOS_Connect_CopyIdTokenOptions options, ref IntPtr outIdToken);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern void EOS_Connect_IdToken_Release(IntPtr idToken);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern int EOS_ProductUserId_ToString(IntPtr localUserId, StringBuilder outBuffer, ref int inOutBufferLength);

	[DllImport(EOS_DLL, CallingConvention = CallingConvention.Cdecl)]
	private static extern int EOS_ByteArray_ToString(byte[] byteArray, uint length, StringBuilder outBuffer, ref uint inOutBufferLength);

	public static string GetAuthenticToken()
	{
		if (!string.IsNullOrEmpty(AuthenticUserIDToken))
		{
			return AuthenticUserIDToken;
		}
		return LoadCachedToken();
	}

	public static string GetProductUserId()
	{
		return !string.IsNullOrEmpty(AuthenticProductUserId) ? AuthenticProductUserId : "000227e2228b43a38d2ef99355ed1d26";
	}

	public static string LoadCachedToken()
	{
		if (!string.IsNullOrEmpty(AuthenticUserIDToken))
		{
			return AuthenticUserIDToken;
		}
		try
		{
			string prefsToken = PlayerPrefs.GetString("AmongUs.EOS.AuthenticUserIDToken", "");
			if (!string.IsNullOrEmpty(prefsToken) && prefsToken.Length > 50)
			{
				AuthenticUserIDToken = prefsToken;
				return AuthenticUserIDToken;
			}
		}
		catch { }

		try
		{
			string scratchPath = @"C:\spacemafia-game\scratch\authentic_jwt.txt";
			if (File.Exists(scratchPath))
			{
				string text = File.ReadAllText(scratchPath).Trim();
				if (!string.IsNullOrEmpty(text) && text.Length > 50)
				{
					AuthenticUserIDToken = text;
					PlayerPrefs.SetString("AmongUs.EOS.AuthenticUserIDToken", AuthenticUserIDToken);
					PlayerPrefs.Save();
					return AuthenticUserIDToken;
				}
			}
		}
		catch { }

		try
		{
			string persistPath = Path.Combine(Application.persistentDataPath, "eos_id_token.jwt");
			if (File.Exists(persistPath))
			{
				string text = File.ReadAllText(persistPath).Trim();
				if (!string.IsNullOrEmpty(text) && text.Length > 50)
				{
					AuthenticUserIDToken = text;
					return AuthenticUserIDToken;
				}
			}
		}
		catch { }

		return string.Empty;
	}

	private static void SaveToken(string token)
	{
		if (string.IsNullOrEmpty(token)) return;
		AuthenticUserIDToken = token;
		try
		{
			PlayerPrefs.SetString("AmongUs.EOS.AuthenticUserIDToken", token);
			PlayerPrefs.Save();
		}
		catch { }
		try
		{
			string scratchPath = @"C:\spacemafia-game\scratch\authentic_jwt.txt";
			File.WriteAllText(scratchPath, token);
		}
		catch { }
		try
		{
			string persistPath = Path.Combine(Application.persistentDataPath, "eos_id_token.jwt");
			File.WriteAllText(persistPath, token);
		}
		catch { }
	}

	public static void Tick()
	{
		if (platformHandle != IntPtr.Zero)
		{
			try
			{
				EOS_Platform_Tick(platformHandle);
			}
			catch { }
		}
	}

	private static bool EnsureEOSPlatform()
	{
		if (connectHandle != IntPtr.Zero) return true;
		try
		{
			if (!eosInitialized)
			{
				EOS_InitializeOptions initOpts = new EOS_InitializeOptions
				{
					ApiVersion = 4,
					ProductName = "AmongUs",
					ProductVersion = "1.0"
				};
				int initRes = EOS_Initialize(ref initOpts);
				Debug.Log($"[NativeEOSAuth] EOS_Initialize result: {initRes}");
				eosInitialized = true;
			}

			string cacheDir = Path.Combine(Application.temporaryCachePath, "EOS");
			EOS_Platform_Options platformOpts = new EOS_Platform_Options
			{
				ApiVersion = 12,
				ProductId = "5fa322f021de4d338d61f6e1dae8b6e9",
				SandboxId = "33956bcb55d4452d8c47e16b94e294bd",
				DeploymentId = "503cd077a7804777aee5a6eeb5cfe62d",
				ClientCredentials = new EOS_Platform_ClientCredentials
				{
					ClientId = "xyza7891qtrmoYLr86we6DlfCA1RRsp8",
					ClientSecret = "nGThQanzvthA2HPaARXe/xutzsKyx5WJveNkBx44ti4"
				},
				EncryptionKey = "AAAABBBBCCCCDDDDEEEFFFF1111222233334444555566667777888899990000",
				Flags = 0x800,
				CacheDirectory = cacheDir
			};
			platformHandle = EOS_Platform_Create(ref platformOpts);
			if (platformHandle == IntPtr.Zero)
			{
				Debug.LogWarning("[NativeEOSAuth] EOS_Platform_Create returned null handle");
				return false;
			}
			connectHandle = EOS_Platform_GetConnectInterface(platformHandle);
			Debug.Log($"[NativeEOSAuth] EOS Platform created! Platform: {platformHandle}, Connect: {connectHandle}");
			return connectHandle != IntPtr.Zero;
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[NativeEOSAuth] EnsureEOSPlatform exception: " + ex);
			return false;
		}
	}

	public static IEnumerator CoAuthenticateWithSteam(Action<bool, string> onComplete)
	{
		LoadCachedToken();

		if (!SteamManager.Initialized)
		{
			Debug.LogWarning("[NativeEOSAuth] SteamManager not initialized, using cached token if available.");
			onComplete?.Invoke(!string.IsNullOrEmpty(AuthenticUserIDToken), AuthenticUserIDToken);
			yield break;
		}

		if (!EnsureEOSPlatform())
		{
			Debug.LogWarning("[NativeEOSAuth] Could not initialize native EOS platform, using cached token.");
			onComplete?.Invoke(!string.IsNullOrEmpty(AuthenticUserIDToken), AuthenticUserIDToken);
			yield break;
		}

		Debug.Log("[NativeEOSAuth] Requesting Steam Encrypted App Ticket...");
		SteamAPICall_t apiCall = SteamUser.RequestEncryptedAppTicket(null, 0);

		byte[] ticketBuffer = new byte[2048];
		uint ticketLength = 0;
		bool ticketReady = false;
		float timeout = 4f;

		while (timeout > 0f && !ticketReady)
		{
			timeout -= Time.unscaledDeltaTime;
			if (SteamUser.GetEncryptedAppTicket(ticketBuffer, 2048, out ticketLength) && ticketLength > 0)
			{
				ticketReady = true;
				break;
			}
			yield return null;
		}

		if (!ticketReady || ticketLength == 0)
		{
			Debug.LogWarning("[NativeEOSAuth] Failed to get Steam Encrypted App Ticket within timeout.");
			onComplete?.Invoke(!string.IsNullOrEmpty(AuthenticUserIDToken), AuthenticUserIDToken);
			yield break;
		}

		byte[] realBytes = new byte[ticketLength];
		Array.Copy(ticketBuffer, 0, realBytes, 0, (int)ticketLength);

		StringBuilder hexBuilder = new StringBuilder((int)(ticketLength * 2 + 2));
		uint inOutLen = (uint)hexBuilder.Capacity;
		int hexRes = EOS_ByteArray_ToString(realBytes, ticketLength, hexBuilder, ref inOutLen);
		string ticketHex;
		if (hexRes == 0 && hexBuilder.Length > 0)
		{
			ticketHex = hexBuilder.ToString();
		}
		else
		{
			ticketHex = BitConverter.ToString(realBytes).Replace("-", "");
		}

		Debug.Log($"[NativeEOSAuth] Steam ticket obtained! Length: {ticketLength}, Hex prefix: {ticketHex.Substring(0, Math.Min(32, ticketHex.Length))}");

		EOS_Connect_Credentials creds = new EOS_Connect_Credentials
		{
			ApiVersion = 1,
			Token = ticketHex,
			Type = 1 // SteamAppTicket = 1
		};

		IntPtr credsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(EOS_Connect_Credentials)));
		Marshal.StructureToPtr(creds, credsPtr, false);

		EOS_Connect_LoginOptions loginOpts = new EOS_Connect_LoginOptions
		{
			ApiVersion = 2,
			Credentials = credsPtr,
			UserLoginInfo = IntPtr.Zero
		};

		bool loginCompleted = false;
		int loginResultCode = -1;
		IntPtr loginLocalUserId = IntPtr.Zero;

		EOS_Connect_OnLoginCallback callback = delegate(ref EOS_Connect_LoginCallbackInfo cbData)
		{
			loginResultCode = cbData.ResultCode;
			loginLocalUserId = cbData.LocalUserId;
			loginCompleted = true;
		};

		Debug.Log("[NativeEOSAuth] Calling EOS_Connect_Login with SteamAppTicket...");
		EOS_Connect_Login(connectHandle, ref loginOpts, IntPtr.Zero, callback);

		float tickTimeout = 6f;
		while (tickTimeout > 0f && !loginCompleted)
		{
			tickTimeout -= Time.unscaledDeltaTime;
			EOS_Platform_Tick(platformHandle);
			yield return null;
		}

		Marshal.FreeHGlobal(credsPtr);

		if (!loginCompleted)
		{
			Debug.LogWarning("[NativeEOSAuth] EOS_Connect_Login timed out, ticking EOS...");
			onComplete?.Invoke(!string.IsNullOrEmpty(AuthenticUserIDToken), AuthenticUserIDToken);
			yield break;
		}

		Debug.Log($"[NativeEOSAuth] EOS_Connect_Login callback received! ResultCode: {loginResultCode}");

		if (loginResultCode == 0 && loginLocalUserId != IntPtr.Zero)
		{
			StringBuilder puidBuilder = new StringBuilder(64);
			int puidLen = 64;
			EOS_ProductUserId_ToString(loginLocalUserId, puidBuilder, ref puidLen);
			AuthenticProductUserId = puidBuilder.ToString();
			Debug.Log($"[NativeEOSAuth] Authenticated EOS Product User ID: {AuthenticProductUserId}");

			EOS_Connect_CopyIdTokenOptions copyOpts = new EOS_Connect_CopyIdTokenOptions
			{
				ApiVersion = 1,
				LocalUserId = loginLocalUserId
			};

			IntPtr idTokenPtr = IntPtr.Zero;
			int copyRes = EOS_Connect_CopyIdToken(connectHandle, ref copyOpts, ref idTokenPtr);
			if (copyRes == 0 && idTokenPtr != IntPtr.Zero)
			{
				EOS_Connect_IdToken idToken = (EOS_Connect_IdToken)Marshal.PtrToStructure(idTokenPtr, typeof(EOS_Connect_IdToken));
				if (idToken.JsonWebToken != IntPtr.Zero)
				{
					string jwt = Marshal.PtrToStringAnsi(idToken.JsonWebToken);
					SaveToken(jwt);
					IsAuthenticated = true;
					Debug.Log($"[NativeEOSAuth] AUTHENTIC USERIDTOKEN OBTAINED SUCCESSFULLY! Prefix: {jwt.Substring(0, Math.Min(40, jwt.Length))}...");
					EOS_Connect_IdToken_Release(idTokenPtr);
					onComplete?.Invoke(true, AuthenticUserIDToken);
					yield break;
				}
				EOS_Connect_IdToken_Release(idTokenPtr);
			}
			else
			{
				Debug.LogWarning($"[NativeEOSAuth] EOS_Connect_CopyIdToken failed with code {copyRes}");
			}
		}
		else
		{
			Debug.LogWarning($"[NativeEOSAuth] EOS Connect login failed with result code {loginResultCode}");
		}

		// Fallback to cached authentic token
		LoadCachedToken();
		bool hasToken = !string.IsNullOrEmpty(AuthenticUserIDToken);
		onComplete?.Invoke(hasToken, AuthenticUserIDToken);
	}
}
