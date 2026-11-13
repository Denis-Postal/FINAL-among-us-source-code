using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AmongUs.Data.Legacy;
using Assets.InnerNet;
using Cpp2ILInjected;
using Innersloth.IO;
using UnityEngine;

namespace AmongUs.Data.Player
{
	[JsonObject(MemberSerialization.OptIn)]
	public class PlayerData : AbstractUserSaveData
	{
		private static readonly byte[] DEFAULT_ENCRYPTED;

		[JsonProperty("account")]
		public PlayerAccountData account;

		[JsonProperty("customization")]
		public PlayerCustomizationData customization;

		[JsonProperty("onboarding")]
		public PlayerOnboardingData onboarding;

		[JsonProperty("age")]
		private byte[] encryptedAge;

		public PlayerAgeData age;

		[JsonProperty("store")]
		private byte[] encryptedStore;

		public PlayerStoreData store;

		[JsonProperty("ads")]
		private byte[] encryptedAds;

		public PlayerAdsData ads;

		[JsonProperty("ban")]
		private byte[] encryptedBan;

		public PlayerBanData ban;

		[JsonProperty("announcements")]
		public PlayerAnnouncementData announcements;

		[JsonProperty("friends")]
		public PlayerFriendData friends;

		public PlayerPurchasesData purchases;

		public PlayerStatsData stats;

		protected override int CurrentVersion => 1;

		public PlayerAccountData Account => account;

		public PlayerCustomizationData Customization => customization;

		public PlayerStatsData Stats => stats;

		public PlayerOnboardingData Onboarding => onboarding;

		public PlayerAgeData Age => age;

		public PlayerStoreData Store => store;

		public PlayerAdsData Ads => ads;

		public PlayerBanData Ban => ban;

		public PlayerPurchasesData Purchases => purchases;

		public PlayerAnnouncementData Announcements => announcements;

		public PlayerFriendData Friends => friends;

		protected override string FileName
		{
			get
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363D914]");
				if ((nint)0 == 0)
				{
					_ = 1;
				}
				return "player.amogus";
			}
		}

		protected override AbstractSaveDataGroup[] Groups
		{
			get
			{
				// TODO(verify): order/contents inferred (decomp showed a 9-element array)
				return new AbstractSaveDataGroup[] { account, customization, onboarding, age, store, ads, ban, announcements, friends };
			}
		}

		protected unsafe override void OnSaveStart()
		{
			// TODO(verify): reimplemented (decomp stub)
			TrySerializeAndEncrypt(age, out encryptedAge);
			TrySerializeAndEncrypt(store, out encryptedStore);
			TrySerializeAndEncrypt(ads, out encryptedAds);
			TrySerializeAndEncrypt(ban, out encryptedBan);
		}

		protected unsafe override void OnLoadComplete()
		{
			// TODO(verify): reimplemented (decomp stub)
			if (encryptedAge != null && encryptedAge.Length != 0) TryDecryptAndDeserialize(ref age, encryptedAge);
			if (encryptedStore != null && encryptedStore.Length != 0) TryDecryptAndDeserialize(ref store, encryptedStore);
			if (encryptedAds != null && encryptedAds.Length != 0) TryDecryptAndDeserialize(ref ads, encryptedAds);
			if (encryptedBan != null && encryptedBan.Length != 0) TryDecryptAndDeserialize(ref ban, encryptedBan);
		}

		protected unsafe override void HandleVersionMigration(int version)
		{
			// TODO(verify): reimplemented (decomp stub)
			// TODO(verify): legacy PlayerPrefs migration (418 decomp lines) not ported; fresh installs need none
		}

		public PlayerData()
		{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
			PlayerAccountData playerAccountData = new PlayerAccountData();
			{
			}
			playerAccountData.guardianEmail = "";
			account = playerAccountData;
			PlayerCustomizationData playerCustomizationData = new PlayerCustomizationData();
			{
			}
			playerCustomizationData.name = "";
			playerCustomizationData.pet = "";
			playerCustomizationData.hat = "";
			playerCustomizationData.skin = "";
			playerCustomizationData.visor = "";
			playerCustomizationData.namePlate = "";
			customization = playerCustomizationData;
			onboarding = new PlayerOnboardingData
			{
			alwaysShowMinPlayerWarning = true
			};
			encryptedAge = DEFAULT_ENCRYPTED;
			age = new PlayerAgeData
			{
			dateOfBirth = PlayerAgeData.DEFAULT_DATE_OF_BIRTH,
			latestSetDate = PlayerAgeData.DEFAULT_LATEST_SET_DATE
			};
			encryptedStore = DEFAULT_ENCRYPTED;
			PlayerStoreData playerStoreData = new PlayerStoreData
			{
			activeCosmicube = "",
			bundlesViewDate = PlayerStoreData.DEFAULT_LAST_VIEW_DATE,
			hatsViewDate = PlayerStoreData.DEFAULT_LAST_VIEW_DATE,
			outfitsViewDate = PlayerStoreData.DEFAULT_LAST_VIEW_DATE,
			visorsViewDate = PlayerStoreData.DEFAULT_LAST_VIEW_DATE,
			petsViewDate = PlayerStoreData.DEFAULT_LAST_VIEW_DATE,
			nameplatesViewDate = PlayerStoreData.DEFAULT_LAST_VIEW_DATE,
			cosmicubeViewDate = PlayerStoreData.DEFAULT_LAST_VIEW_DATE
			};
			List<string> legacyPurchases = new List<string>();
			playerStoreData.legacyPurchases = legacyPurchases;
			store = playerStoreData;
			encryptedAds = DEFAULT_ENCRYPTED;
			ads = new PlayerAdsData
			{
			lastDateWatchAd = PlayerAdsData.DEFAULT_DATE_LAST_AD
			};
			encryptedBan = DEFAULT_ENCRYPTED;
			ban = new PlayerBanData();
			PlayerAnnouncementData playerAnnouncementData = new PlayerAnnouncementData();
			List<int> ids = new List<int>();
			playerAnnouncementData.ids = ids;
			playerAnnouncementData.allAnnouncements = new List<Announcement>();
			announcements = playerAnnouncementData;
			PlayerFriendData playerFriendData = new PlayerFriendData();
			List<PlayerFriendData.CachedName> cachedNames = new List<PlayerFriendData.CachedName>();
			playerFriendData.cachedNames = cachedNames;
			friends = playerFriendData;
			purchases = new PlayerPurchasesData();
			PlayerStatsData playerStatsData = new PlayerStatsData();
			Dictionary<StatID, uint> statsCache = new Dictionary<StatID, uint>();
			playerStatsData.statsCache = statsCache;
			playerStatsData.statsToIngest = new Dictionary<StatID, uint>();
			playerStatsData.isTrackingStats = true;
			stats = playerStatsData;
		}

		static PlayerData()
		{
			byte[] dEFAULT_ENCRYPTED = new byte[0];
			DEFAULT_ENCRYPTED = dEFAULT_ENCRYPTED;
		}
	}
}
