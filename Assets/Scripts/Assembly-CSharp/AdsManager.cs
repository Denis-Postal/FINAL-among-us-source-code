using System;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdsManager : DestroyableSingleton<AdsManager>
{
	private string apiKey = "23c72ed15";

	private string rewardedAdUnitId = "eb39qixpai5zjqk1";

	private LevelPlayRewardedAd rewardedAd;

	private bool initialized;

	private readonly Logger logger = new Logger(Logger.Category.Ads, new string[1] { "AdsManager" });

	private Action<bool> adMenuCallbak;

	private Action<bool> rewardedAdCallback;

	public bool IsInitialized()
	{
		return initialized;
	}

	public override void Awake()
	{
		base.Awake();
		GameObject obj = gameObject;
		UnityEngine.Object.Destroy(obj);
	}

	public void InitLevelPlay(bool isChild)
	{
		string value;
		if (!isChild)
		{
			LevelPlay.SetMetaData("UnityAds_coppa", "false");
			value = "false";
		}
		else
		{
			LevelPlay.SetMetaData("UnityAds_coppa", "true");
			value = "true";
		}
		LevelPlay.SetMetaData("is_child_directed", value);
		Action<LevelPlayConfiguration> value2 = SdkInitializationCompletedEvent;
		LevelPlay.OnInitSuccess += value2;
		Action<LevelPlayInitError> value3 = SdkInitializationFailedEvent;
		LevelPlay.OnInitFailed += value3;
		EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
		string productUserId = instance.ProductUserId;
		bool flag = LevelPlay.SetDynamicUserId(productUserId);
		EOSManager instance2 = DestroyableSingleton<EOSManager>.Instance;
		string productUserId2 = instance2.ProductUserId;
		LevelPlay.Init(apiKey, productUserId2);
	}

	public void RequestATTPermission()
	{
	}

	private void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
	{
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363C097]");
		if ((nint)0 == 0)
		{
			_ = 1;
		}
		initialized = true;
		logger.Debug("Completed level play init");
	}

	private void SdkInitializationFailedEvent(LevelPlayInitError initError)
	{
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363C098]");
		if ((nint)0 == 0)
		{
			_ = 1;
		}
		string message = "Failed level play init " + initError.ErrorMessage;
		logger.Error(message);
		initialized = false;
	}

	public void CreateRewardedAd(Action<bool> adCallback)
	{
		//IL_003d: Expected Obj, but got I
		//IL_004d: Expected Obj, but got I
		//IL_005d: Expected Obj, but got I
		if (rewardedAd != null && rewardedAd.IsAdReady())
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [adCallback @ rdx (System.Action`1<System.Boolean>)+18]");
			object obj = 0;
			Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [adCallback @ rdx (System.Action`1<System.Boolean>)+28]");
			object obj2 = 0;
			Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [adCallback @ rdx (System.Action`1<System.Boolean>)+40]");
			object obj3 = 0;
			Cpp2ILHelpers.NoteDecompilerIssue("Indirect jump: v104 @ rax_v48 (should have been resolved before IL gen)");
		}
		string[] array = new string[1];
		EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
		string userIDToken = instance.UserIDToken;
		string text = "JWT=" + userIDToken;
		array[0] = text;
		LevelPlay.SetMetaData("LevelPlay_Rewarded_Server_Params", array);
		EOSManager instance2 = DestroyableSingleton<EOSManager>.Instance;
		string productUserId = instance2.ProductUserId;
		LevelPlay.Init(apiKey, productUserId);
		adMenuCallbak = adCallback;
		LevelPlayRewardedAd levelPlayRewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);
		rewardedAd = levelPlayRewardedAd;
		Action<LevelPlayAdInfo> value = RewardedOnAdLoadedEvent;
		rewardedAd.OnAdLoaded += value;
		Action<LevelPlayAdError> value2 = RewardedOnAdLoadFailedEvent;
		rewardedAd.OnAdLoadFailed += value2;
		Action<LevelPlayAdInfo> value3 = RewardedOnAdDisplayedEvent;
		rewardedAd.OnAdDisplayed += value3;
		Action<LevelPlayAdInfo, LevelPlayAdError> value4 = RewardedOnAdDisplayFailedEvent;
		rewardedAd.OnAdDisplayFailed += value4;
		Action<LevelPlayAdInfo, LevelPlayReward> value5 = RewardedOnAdRewardedEvent;
		rewardedAd.OnAdRewarded += value5;
		Action<LevelPlayAdInfo> value6 = RewardedOnAdClosedEvent;
		rewardedAd.OnAdClosed += value6;
		Action<LevelPlayAdInfo> value7 = RewardedOnAdClickedEvent;
		rewardedAd.OnAdClicked += value7;
		Action<LevelPlayAdInfo> value8 = RewardedOnAdInfoChangedEvent;
		rewardedAd.OnAdInfoChanged += value8;
		rewardedAd.LoadAd();
	}

	public void ShowRewardedAd(Action<bool> rewardCallback)
	{
		rewardedAdCallback = rewardCallback;
		if (!rewardedAd.IsAdReady())
		{
			rewardedAd = null;
			logger.Debug("Ad not ready");
			Action<bool> action = rewardedAdCallback;
			if (rewardedAdCallback != null)
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Indirect call: [v231 @ rcx_v17 (System.Action`1<System.Boolean>)+18] (should have been resolved before IL gen)");
			}
		}
		else
		{
			logger.Debug("Showing Ad");
			string[] array = new string[1];
			EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
			string userIDToken = instance.UserIDToken;
			string text = "JWT=" + userIDToken;
			array[0] = text;
			LevelPlay.SetMetaData("LevelPlay_Rewarded_Server_Params", array);
			rewardedAd.ShowAd();
		}
	}

	private void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo)
	{
		//IL_00b5: Expected Obj, but got I
		//IL_00c5: Expected Obj, but got I
		//IL_00d5: Expected Obj, but got I
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363C09B]");
		if ((nint)0 == 0)
		{
			_ = 1;
		}
		Logger logger;
		object message;
		if (!rewardedAd.IsAdReady())
		{
			rewardedAd = null;
			logger = this.logger;
			message = "Ad not ready";
		}
		else
		{
			logger = this.logger;
			message = "Ad ready";
		}
		while (true)
		{
			logger.Debug(message);
			Action<bool> action = adMenuCallbak;
			bool flag = rewardedAd.IsAdReady();
			Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [v49 @ rdi_v2 (System.Action`1<System.Boolean>)+18]");
			object obj = 0;
			Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [v49 @ rdi_v2 (System.Action`1<System.Boolean>)+28]");
			object obj2 = 0;
			Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [v49 @ rdi_v2 (System.Action`1<System.Boolean>)+40]");
			object obj3 = 0;
			Cpp2ILHelpers.NoteDecompilerIssue("Indirect jump: v129 @ r9_v3 (should have been resolved before IL gen)");
		}
	}

	private void RewardedOnAdLoadFailedEvent(LevelPlayAdError adError)
	{
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363C09C]");
		if ((nint)0 == 0)
		{
			_ = 1;
		}
		rewardedAd = null;
		Action<bool> action = adMenuCallbak;
		Cpp2ILHelpers.NoteDecompilerIssue("Indirect call: [v32 @ rcx_v3 (System.Action`1<System.Boolean>)+18] (should have been resolved before IL gen)");
		string message = "Error loading ad: " + adError.ErrorMessage;
		logger.Error(message);
	}

	private void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
	{
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363C09D]");
		if ((nint)0 == 0)
		{
			_ = 1;
		}
		rewardedAd = null;
		logger.Debug("Ad displayed");
	}

	private void RewardedOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
	{
		Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [18363C09E]");
		if ((nint)0 == 0)
		{
			_ = 1;
		}
		rewardedAd = null;
		Action<bool> action = rewardedAdCallback;
		if (rewardedAdCallback != null)
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Indirect call: [v32 @ rcx_v3 (System.Action`1<System.Boolean>)+18] (should have been resolved before IL gen)");
		}
		string message = "Error displaying ad: " + error.ErrorMessage;
		logger.Error(message);
	}

	private void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward)
	{
		rewardedAd = null;
		InventoryManager instance = DestroyableSingleton<InventoryManager>.Instance;
		instance.UpdateStarCount(1);
		PlayerData player = DataManager.Player;
		player.ads.SetDateAdWatched();
		PlayerData player2 = DataManager.Player;
		player2.Save();
		Action<bool> action = rewardedAdCallback;
		if (rewardedAdCallback != null)
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Indirect call: [v153 @ rcx_v11 (System.Action`1<System.Boolean>)+18] (should have been resolved before IL gen)");
		}
		logger.Debug("We got a reward");
	}

	private void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo)
	{
	}

	private void RewardedOnAdClickedEvent(LevelPlayAdInfo adInfo)
	{
	}

	private void RewardedOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
	{
	}
}
