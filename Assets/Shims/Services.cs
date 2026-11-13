// Shims for Sentry, LevelPlay (ads), Unity Gaming Services. All no-ops (offline build). TODO(verify)
using System;
using System.Collections.Generic;
namespace Sentry
{
	public enum SentryLevel { Debug, Info, Warning, Error, Fatal }
	public struct SentryId { public static readonly SentryId Empty = default; public override string ToString() => "00000000"; }
	public class Breadcrumb { }
	public class Scope { public void SetTag(string k, string v) { } public void SetExtra(string k, object v) { } public Dictionary<string, string> Tags = new Dictionary<string, string>(); public void AddAttachment(string path) { } public void AddAttachment(byte[] data, string name) { } public User User = new User(); }
	public class User { public string Id; public string Username; public string Email; public string IpAddress; }
}
namespace Sentry.Unity { }
namespace Unity.Services.LevelPlay
{
	public class LevelPlayConfiguration { }
	public class LevelPlayInitError { public string ErrorMessage = ""; public int ErrorCode; }
	public class LevelPlayAdInfo { public string AdUnitId = ""; }
	public class LevelPlayAdError { public string ErrorMessage = ""; public int ErrorCode; }
	public class LevelPlayReward { public string Name = ""; public int Amount; }
	public class LevelPlayRewardedAd
	{
		public event Action<LevelPlayAdInfo> OnAdLoaded; public event Action<LevelPlayAdError> OnAdLoadFailed; public event Action<LevelPlayAdInfo> OnAdDisplayed;
		public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed; public event Action<LevelPlayAdInfo, LevelPlayReward> OnAdRewarded; public event Action<LevelPlayAdInfo> OnAdClosed;
		public event Action<LevelPlayAdInfo> OnAdClicked; public event Action<LevelPlayAdInfo> OnAdInfoChanged;
		public LevelPlayRewardedAd(string adUnitId) { }
		public void LoadAd() { } public void ShowAd(string placement = null) { } public bool IsAdReady() => false; public void DestroyAd() { }
	}
	public static class LevelPlay
	{
		public static event Action<LevelPlayConfiguration> OnInitSuccess; public static event Action<LevelPlayInitError> OnInitFailed;
		public static void SetMetaData(string k, string v) { } public static void SetMetaData(string k, IEnumerable<string> v) { } public static bool SetDynamicUserId(string id) => true;
		public static void Init(string appKey, string userId = null) { }
	}
}
