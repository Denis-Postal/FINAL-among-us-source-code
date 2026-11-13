using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern bool SetEnvironmentVariable(string lpName, string lpValue);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool SetCurrentDirectory(string lpPath);

	protected static bool s_EverInitialized;

	protected static SteamManager s_instance;

	protected bool m_bInitialized;

	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	protected static SteamManager Instance
	{
		get
		{
			if (s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return s_instance;
		}
	}

	public static bool Initialized
	{
		get
		{
			if (s_instance == null)
			{
				_ = Instance;
			}
			return s_instance != null && s_instance.m_bInitialized;
		}
	}

	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		s_EverInitialized = false;
		s_instance = null;
	}

	protected virtual void Awake()
	{
		if (s_instance != null)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		s_instance = this;
		if (s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		if (Application.isEditor)
		{
			try
			{
				if (SteamAPI.GetHSteamPipe() != (HSteamPipe)0)
				{
					if (CSteamAPIContext.GetSteamUser() == IntPtr.Zero)
					{
						CSteamAPIContext.Init();
					}
					m_bInitialized = true;
					s_EverInitialized = true;
					Debug.Log("[Steamworks.NET] SteamAPI already active in Editor! SteamID: " + SteamUser.GetSteamID());
					return;
				}
			}
			catch { }
		}
		if (!Application.isEditor)
		{
			try
			{
				if (SteamAPI.RestartAppIfNecessary((AppId_t)945360u))
				{
					Application.Quit();
					return;
				}
			}
			catch (DllNotFoundException ex)
			{
				Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib.\n" + ex, this);
				return;
			}
		}
		try
		{
			string projectDir = Path.GetFullPath(Application.dataPath + "/..");
			SetCurrentDirectory(projectDir);
			Directory.SetCurrentDirectory(projectDir);
			string targetTxt = Path.Combine(projectDir, "steam_appid.txt");
			if (!File.Exists(targetTxt))
			{
				File.WriteAllText(targetTxt, "945360");
			}
		}
		catch { }
		try
		{
			SetEnvironmentVariable("SteamAppId", "945360");
			SetEnvironmentVariable("SteamGameId", "945360");
			System.Environment.SetEnvironmentVariable("SteamAppId", "945360");
			System.Environment.SetEnvironmentVariable("SteamGameId", "945360");
		}
		catch { }
		Debug.Log($"[Steamworks.NET] Initializing Steamworks... IsSteamRunning: {SteamAPI.IsSteamRunning()}, CWD: {Directory.GetCurrentDirectory()}");
		m_bInitialized = SteamAPI.Init();
		if (!m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Ensure Steam is running and your account owns the game.", this);
		}
		else
		{
			s_EverInitialized = true;
			Debug.Log("[Steamworks.NET] SteamAPI_Init() succeeded! SteamID: " + SteamUser.GetSteamID());
		}
	}

	protected virtual void OnEnable()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		if (m_bInitialized && m_SteamAPIWarningMessageHook == null)
		{
			m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
			SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
		}
	}

	protected virtual void OnDestroy()
	{
		if (!(s_instance != this))
		{
			s_instance = null;
			if (m_bInitialized && !Application.isEditor)
			{
				SteamAPI.Shutdown();
			}
		}
	}

	protected virtual void Update()
	{
		if (m_bInitialized)
		{
			SteamAPI.RunCallbacks();
		}
	}
}
