using System;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.AntiCheatClient;
using Epic.OnlineServices.AntiCheatServer;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.CustomInvites;
using Epic.OnlineServices.Ecom;
using Epic.OnlineServices.Friends;
using Epic.OnlineServices.IntegratedPlatform;
using Epic.OnlineServices.Inventory;
using Epic.OnlineServices.KWS;
using Epic.OnlineServices.Leaderboards;
using Epic.OnlineServices.Lobby;
using Epic.OnlineServices.Metrics;
using Epic.OnlineServices.Mods;
using Epic.OnlineServices.P2P;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Presence;
using Epic.OnlineServices.ProgressionSnapshot;
using Epic.OnlineServices.RTC;
using Epic.OnlineServices.RTCAdmin;
using Epic.OnlineServices.ReceiptValidator;
using Epic.OnlineServices.Reports;
using Epic.OnlineServices.Sanctions;
using Epic.OnlineServices.Sessions;
using Epic.OnlineServices.Stats;
using Epic.OnlineServices.TitleStorage;
using Epic.OnlineServices.UI;
using Epic.OnlineServices.UserInfo;

namespace Epic.OnlineServices.Platform
{
	public sealed class PlatformInterface : Handle
	{
		public const int AndroidInitializeoptionssysteminitializeoptionsApiLatest = 2;

		public static readonly Utf8String CheckforlauncherandrestartEnvVar;

		public const int ClientcredentialsClientidMaxLength = 64;

		public const int ClientcredentialsClientsecretMaxLength = 64;

		public const int CountrycodeMaxBufferLen = 5;

		public const int CountrycodeMaxLength = 4;

		public const int GetdesktopcrossplaystatusApiLatest = 1;

		public const int InitializeApiLatest = 4;

		public const int InitializeThreadaffinityApiLatest = 3;

		public const int InitializeoptionsProductnameMaxLength = 64;

		public const int InitializeoptionsProductversionMaxLength = 64;

		public const int LocalecodeMaxBufferLen = 10;

		public const int LocalecodeMaxLength = 9;

		public const int OptionsApiLatest = 14;

		public const int OptionsDeploymentidMaxLength = 64;

		public const int OptionsEncryptionkeyLength = 64;

		public const int OptionsProductidMaxLength = 64;

		public const int OptionsSandboxidMaxLength = 64;

		public const int RtcoptionsApiLatest = 2;

		public const int Ps4InitializeoptionssysteminitializeoptionsApiLatest = 8;

		public const int Ps5InitializeoptionssysteminitializeoptionsApiLatest = 3;

		public const int WindowsRtcoptionsplatformspecificoptionsApiLatest = 1;

		public const int XboxonegdkInitializeoptionssysteminitializeoptionsApiLatest = 3;

		public const int XboxonegdkOptionssystemspecificoptionsApiLatest = 1;

		public const int XsxInitializeoptionssysteminitializeoptionsApiLatest = 3;

		public const int XsxOptionssystemspecificoptionsApiLatest = 1;

		public static Result Initialize(ref AndroidInitializeOptions options)
		{
			return Result.Success;
		}

		public PlatformInterface()
		{
		}

		public PlatformInterface(IntPtr innerHandle)
		{
		}

		public Result CheckForLauncherAndRestart()
		{
			return Result.Success;
		}

		public static PlatformInterface Create(ref Options options)
		{
			return new PlatformInterface((IntPtr)1);
		}

		public AchievementsInterface GetAchievementsInterface()
		{
			return new AchievementsInterface((IntPtr)1);
		}

		public Result GetActiveCountryCode(EpicAccountId localUserId, out Utf8String outBuffer)
		{
			outBuffer = "US";
			return Result.Success;
		}

		public Result GetActiveLocaleCode(EpicAccountId localUserId, out Utf8String outBuffer)
		{
			outBuffer = "en";
			return Result.Success;
		}

		public AntiCheatClientInterface GetAntiCheatClientInterface()
		{
			return null;
		}

		public AntiCheatServerInterface GetAntiCheatServerInterface()
		{
			return null;
		}

		public ApplicationStatus GetApplicationStatus()
		{
			return ApplicationStatus.Foreground;
		}

		public AuthInterface GetAuthInterface()
		{
			return new AuthInterface((IntPtr)1);
		}

		public ConnectInterface GetConnectInterface()
		{
			return new ConnectInterface((IntPtr)1);
		}

		public CustomInvitesInterface GetCustomInvitesInterface()
		{
			return new CustomInvitesInterface((IntPtr)1);
		}

		public Result GetDesktopCrossplayStatus(ref GetDesktopCrossplayStatusOptions options, out DesktopCrossplayStatusInfo outDesktopCrossplayStatusInfo)
		{
			outDesktopCrossplayStatusInfo = default;
			return Result.Success;
		}

		public EcomInterface GetEcomInterface()
		{
			return new EcomInterface((IntPtr)1);
		}

		public FriendsInterface GetFriendsInterface()
		{
			return new FriendsInterface((IntPtr)1);
		}

		public IntegratedPlatformInterface GetIntegratedPlatformInterface()
		{
			return new IntegratedPlatformInterface((IntPtr)1);
		}

		public InventoryInterface GetInventoryInterface()
		{
			return new InventoryInterface((IntPtr)1);
		}

		public KWSInterface GetKWSInterface()
		{
			return new KWSInterface((IntPtr)1);
		}

		public LeaderboardsInterface GetLeaderboardsInterface()
		{
			return new LeaderboardsInterface((IntPtr)1);
		}

		public LobbyInterface GetLobbyInterface()
		{
			return new LobbyInterface((IntPtr)1);
		}

		public MetricsInterface GetMetricsInterface()
		{
			return new MetricsInterface((IntPtr)1);
		}

		public ModsInterface GetModsInterface()
		{
			return null;
		}

		public NetworkStatus GetNetworkStatus()
		{
			return NetworkStatus.Online;
		}

		public Result GetOverrideCountryCode(out Utf8String outBuffer)
		{
			outBuffer = "US";
			return Result.Success;
		}

		public Result GetOverrideLocaleCode(out Utf8String outBuffer)
		{
			outBuffer = "en";
			return Result.Success;
		}

		public P2PInterface GetP2PInterface()
		{
			return new P2PInterface((IntPtr)1);
		}

		public PlayerDataStorageInterface GetPlayerDataStorageInterface()
		{
			return new PlayerDataStorageInterface((IntPtr)1);
		}

		public PresenceInterface GetPresenceInterface()
		{
			return new PresenceInterface((IntPtr)1);
		}

		public ProgressionSnapshotInterface GetProgressionSnapshotInterface()
		{
			return new ProgressionSnapshotInterface((IntPtr)1);
		}

		public RTCAdminInterface GetRTCAdminInterface()
		{
			return new RTCAdminInterface((IntPtr)1);
		}

		public RTCInterface GetRTCInterface()
		{
			return new RTCInterface((IntPtr)1);
		}

		public ReceiptValidatorInterface GetReceiptValidatorInterface()
		{
			return new ReceiptValidatorInterface((IntPtr)1);
		}

		public ReportsInterface GetReportsInterface()
		{
			return new ReportsInterface((IntPtr)1);
		}

		public SanctionsInterface GetSanctionsInterface()
		{
			return new SanctionsInterface((IntPtr)1);
		}

		public SessionsInterface GetSessionsInterface()
		{
			return new SessionsInterface((IntPtr)1);
		}

		public StatsInterface GetStatsInterface()
		{
			return new StatsInterface((IntPtr)1);
		}

		public TitleStorageInterface GetTitleStorageInterface()
		{
			return new TitleStorageInterface((IntPtr)1);
		}

		public UIInterface GetUIInterface()
		{
			return new UIInterface((IntPtr)1);
		}

		public UserInfoInterface GetUserInfoInterface()
		{
			return new UserInfoInterface((IntPtr)1);
		}

		public static Result Initialize(ref InitializeOptions options)
		{
			return Result.Success;
		}

		public void Release()
		{
		}

		public Result SetApplicationStatus(ApplicationStatus newStatus)
		{
			return Result.Success;
		}

		public Result SetNetworkStatus(NetworkStatus newStatus)
		{
			return Result.Success;
		}

		public Result SetOverrideCountryCode(Utf8String newCountryCode)
		{
			return Result.Success;
		}

		public Result SetOverrideLocaleCode(Utf8String newLocaleCode)
		{
			return Result.Success;
		}

		public static Result Shutdown()
		{
			return Result.Success;
		}

		public void Tick()
		{
		}

		public static Utf8String ToString(ApplicationStatus applicationStatus)
		{
			return null;
		}

		public static Utf8String ToString(NetworkStatus networkStatus)
		{
			return null;
		}

		public static Result Initialize(ref PS4InitializeOptions options)
		{
			return Result.Success;
		}

		public Result ManagedByApplicationNotificationMessages(IntPtr inEvent)
		{
			return Result.Success;
		}

		public static Result Initialize(ref PS5InitializeOptions options)
		{
			return Result.Success;
		}

		public static PlatformInterface Create(ref WindowsOptions options)
		{
			return null;
		}

		public static PlatformInterface Create(ref XboxOneGDKOptions options)
		{
			return null;
		}

		public static Result Initialize(ref XboxOneGDKInitializeOptions options)
		{
			return Result.Success;
		}

		public static PlatformInterface Create(ref XSXOptions options)
		{
			return null;
		}

		public static Result Initialize(ref XSXInitializeOptions options)
		{
			return Result.Success;
		}
	}
}
