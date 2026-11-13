using System;
using System.Globalization;
using System.Threading;
using UnityEngine;

public static class Constants
{
	public static class BuildSymbols
	{
		public const string Steam = "STEAMWORKS";

		public const string DisableSteam = "DISABLESTEAMWORKS";

		public const string EGS = "EPICGAMESSTORE";

		public const string DisableEGS = "DISABLEEPIC";

		public const string XboxOneEGS = "EOS_PLATFORM_XBOXONEGDK";

		public const string SeriesXEGS = "EOS_PLATFORM_XSX";

		public const string Itchio = "ITCHIO";

		public const string Win10 = "MICROSOFT_GAME_CORE";

		public const string Console = "CONSOLE";

		public const string Controller = "CONTROLLER";

		public const string ItchioAndroid = "ITCHIO_ANDROID";

		public const string Staging = "STAGING";

		public const string QABuild = "QA_BUILD";

		public const string IsMinorDebug = "IS_MINOR_DEBUG";

		public const string DebugTool = "DEBUG_TOOL";

		public const string Debug = "DEBUG";

		public const string ExternalPlaytest = "EXTERNAL_PLAYTEST";

		public const string DisableObfuscator = "BEEBYTE_OBFUSCATOR_DISABLE";
	}

	public static class CDNPlatforms
	{
		public const string Steam = "Steam";

		public const string EGS = "EGS";

		public const string Itch = "Itch";

		public const string Win10 = "Win10";

		public const string ItchAndroid = "ItchAndroid";

		public const string GPAndroid = "GPAndroid";

		public const string iOS = "iOS";

		public const string PS4 = "PS4";

		public const string PS5 = "PS5";

		public const string Switch = "Switch";

		public const string XB1 = "XB1";

		public const string XSX = "XSX";
	}

	public const float ButtonLongClickTime = 0.5f;

	public const int AnimationFrameRate = 24;

	public const int MinNumPlayers = 4;

	public const int MaxNumPlayers = 15;

	public const string LocalNetAddress = "127.0.0.1";

	public const ushort GamePlayPort = 22023;

	public const ushort AnnouncementPort = 22024;

	public const string InfinitySymbol = "∞";

	public const string DummyNamePrefix = "Dummy";

	public static readonly int ShipOnlyMask = LayerMask.GetMask("Ship");

	public static readonly int ShipAndObjectsMask = LayerMask.GetMask("Ship", "Objects");

	public static readonly int ShipAndAllObjectsMask = LayerMask.GetMask("Ship", "Objects", "ShortObjects");

	public static readonly int NotShipMask = ~LayerMask.GetMask("Ship");

	public static readonly int Usables = ~LayerMask.GetMask("Ship", "UI");

	public static readonly int PlayersOnlyMask = LayerMask.GetMask("Players", "Ghost");

	public static readonly int LivingPlayersOnlyMask = LayerMask.GetMask("Players");

	public static readonly int ShadowMask = LayerMask.GetMask("Shadow", "Objects", "IlluminatedBlocking");

	public static readonly string[] MapNames = new string[6] { "The Skeld", "MIRA HQ", "Polus", "dlekS ehT", "Airship", "The Fungle" };

	public static readonly string Beans = "Beans";

	public static readonly string[] PlayStationControllers = new string[2] { "Sony DualShock 4", "Sony DualSense" };

	public static readonly string PrivacyPolicyURL = "https://www.innersloth.com/privacy-policy/";

	public static readonly string TermsOfUseURL = "https://www.innersloth.com/terms-of-use-for-the-games/";

	public static readonly string ParentPortalURL = "https://parents.innersloth.com/";

	public static readonly string UserIdTokenError = "{\"errors\":[{\"code\":500,\"title\":\"USERIDTOKEN does not exist\",\"keyword\":\"user_auth_error\",\"detail\":1337420,\"detail_text\":\"usertokenid does not exist\",\"error_text\":\"usertokenid does not exist\"}]}\n";

	public static string CCDBucketAccessToken = "f26f8ec35cd092a0c4c4e6bae53c33f9";

	public static CultureInfo CultureInfo = CultureInfo.CreateSpecificCulture("en-US");

	public static readonly int[] CompatVersions = new int[1] { GetBroadcastVersion() };

	public const int Year = 2026;

	public const int Month = 7;

	public const int Day = 20;

	public const int Revision = 0;

	public const int VisualRevision = 0;

	public const string AddressablesVersion = "2026_8_17_1125";

	public const string extraBuildVersionInfo = "(build num: 7489)";

	public const int pipelineBuildNumber = 7489;

	public const BuildEnvironment environment = BuildEnvironment.Production;

	public const byte EmptyPlayerID = byte.MaxValue;

	private static Thread _MainThread;

	public const string DefaultSpriteShader = "Sprites/Default";

	public const string PlayerColorShaderName = "PlayerShader";

	public const string PlayerColorShaderPath = "Unlit/PlayerShader";

	public const int LADDER_COOLDOWN = 5;

	public const int ZIPLINE_COOLDOWN = 7;

	public const int INITIAL_SABOTAGE_COOLDOWN = 10;

	public const int LOW_END_MEMORY_SIZE = 2500;

	private static int MODDER_VERSION = GetVersion(2222, 0, 0, 0);

	private const int MODDED_REVISION_MODIFIER_VALUE = 25;

	public const string SECUREFILE_FILENAME = "secureNew";

	public const string PLAYER_FILENAME = "player.amogus";

	public const string SETTINGS_FILENAME = "settings.amogus";

	public const string MAIN_MENU_SCENE = "MainMenu";

	public const string MATCHMAKING_SCENE = "MatchMaking";

	public const string ONLINE_SCENE = "OnlineGame";

	public const string FIND_GAME_SCENE = "FindAGame";

	public const string TUTORIAL = "Tutorial";

	public const int MAX_ADS_PER_DAY = 2;

	private static string cachedPlatformName = null;

	public static Thread MainThread => _MainThread;

	public static void SetMainThread()
	{
		_MainThread = Thread.CurrentThread;
	}

	internal static int GetBroadcastVersion()
	{
		return 50663600;
	}

	internal static bool IsVersionModded()
	{
		int broadcastVersion = GetBroadcastVersion();
		if (broadcastVersion == MODDER_VERSION)
		{
			return true;
		}
		return GetVersionComponents(broadcastVersion).Revision >= 25;
	}

	internal static int GetVersion(int year, int month, int day, int rev)
	{
		return year * 25000 + month * 1800 + day * 50 + rev;
	}

	internal static (int Year, int Month, int Day, int Revision) GetVersionComponents(int broadcastVersion)
	{
		int num = broadcastVersion / 25000;
		broadcastVersion -= num * 25000;
		int num2 = broadcastVersion / 1800;
		broadcastVersion -= num2 * 1800;
		int num3 = broadcastVersion / 50;
		broadcastVersion -= num3 * 50;
		int item = broadcastVersion;
		return (Year: num, Month: num2, Day: num3, Revision: item);
	}

	internal static byte[] GetBroadcastVersionBytes()
	{
		return BitConverter.GetBytes(GetBroadcastVersion());
	}

	public static bool ShouldPlaySfx()
	{
		if ((bool)AmongUsClient.Instance && AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame)
		{
			return DetectHeadset.Detect();
		}
		return true;
	}

	public static PlatformSpecificData GetPlatformData()
	{
		return new PlatformSpecificData
		{
			Platform = GetPlatformType(),
			PlatformName = GetPlatformName()
		};
	}

	public static void UpdateCachedPlatformName()
	{
		Debug.Log("Updating cached platform name");
	}

	public static string GetPlatformName()
	{
		if (cachedPlatformName == null && Thread.CurrentThread == MainThread)
		{
			UpdateCachedPlatformName();
		}
		if (cachedPlatformName != null)
		{
			return cachedPlatformName;
		}
		return "TESTNAME";
	}

	public static string GetPurchasingPlatformType()
	{
		switch (GetPlatformType())
		{
		case Platforms.Android:
			return "android";
		case Platforms.Switch:
			return "switch";
		case Platforms.StandaloneSteamPC:
			return "steam";
		case Platforms.StandaloneEpicPC:
			return "egs";
		case Platforms.StandaloneWin10:
		case Platforms.Xbox:
			return "xbl";
		case Platforms.Playstation:
			return "psn";
		case Platforms.IPhone:
			return "ios";
		default:
			return "unsupported-" + GetPlatformType();
		}
	}

	public static Platforms GetPlatformType()
	{
		return Platforms.StandaloneSteamPC;
	}

	public static string GetCurrentPlatformName()
	{
		return "Steam";
	}

	public static void OpenURL(string url)
	{
		Application.OpenURL(url);
	}

	public static DateTime ParseDate(string date)
	{
		return DateTime.Parse(date, CultureInfo);
	}
}
