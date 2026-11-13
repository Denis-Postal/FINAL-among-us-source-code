using System;
using Newtonsoft.Json;

namespace AmongUs.Data.Player
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PlayerAdsData : AbstractSaveDataGroup
	{
		public const ShowAdsState DEFAULT_ADS_STATE = ShowAdsState.NotAccepted;

		public const bool DEFAULT_HAS_PURCHASED_AD_REMOVAL = false;

		public static readonly DateTime DEFAULT_DATE_LAST_AD = DateTime.MinValue;

		private const int DEFAULT_ADS_WATCHED = 0;

		[JsonProperty("adsState")]
		private ShowAdsState adsState;

		[JsonProperty("hasPurchasedAdRemoval")]
		private bool hasPurchasedAdRemoval;

		[JsonProperty("DateAdWatched")]
		public DateTime lastDateWatchAd = DEFAULT_DATE_LAST_AD;

		[JsonProperty("AdsWatched")]
		private int adsWatched;

		public int AdsWatched => adsWatched;

		public bool CanWatchAdAgain
		{
			get
			{
				DateTime utcNow = AmongUsDateTime.UtcNow;
				DateTime dateTime = new DateTime(lastDateWatchAd.Year, lastDateWatchAd.Month, lastDateWatchAd.Day + 1, lastDateWatchAd.Hour, lastDateWatchAd.Minute + 1, 0, 0, DateTimeKind.Utc);
				if (utcNow > dateTime)
				{
					SetValue(ref adsWatched, 0, OnAdsWatchedChanged);
					adsWatched = 0;
				}
				if (adsWatched >= 2)
				{
					return false;
				}
				return true;
			}
		}

		public ShowAdsState AdsState
		{
			get
			{
				return adsState;
			}
			set
			{
				SetValue(ref adsState, value, OnShowAdsStateChanged);
			}
		}

		public bool HasPurchasedAdRemoval
		{
			get
			{
				return true;
			}
			set
			{
				SetValue(ref hasPurchasedAdRemoval, value, OnHasPurchasedAdRemovalChanged);
			}
		}

		public event Action OnShowAdsStateChanged;

		public event Action OnHasPurchasedAdRemovalChanged;

		public event Action OnAdsWatchedChanged;

		public event Action OnTimeWatchedAdChanged;

		public int HoursLeftNewAd()
		{
			if (adsWatched >= 2)
			{
				DateTime utcNow = AmongUsDateTime.UtcNow;
				DateTime dateTime = new DateTime(lastDateWatchAd.Year, lastDateWatchAd.Month, lastDateWatchAd.Day + 1, lastDateWatchAd.Hour + 1, 0, 0, 0, DateTimeKind.Utc);
				if (utcNow < dateTime)
				{
					return (int)(dateTime - utcNow).TotalHours;
				}
			}
			return 0;
		}

		public void SetDateAdWatched()
		{
			int value = adsWatched + 1;
			SetValue(ref adsWatched, value, OnAdsWatchedChanged);
			SetValue(ref lastDateWatchAd, AmongUsDateTime.UtcNow, OnTimeWatchedAdChanged);
			adsWatched = value;
			lastDateWatchAd = AmongUsDateTime.UtcNow;
		}
	}
}
