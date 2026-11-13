using System.Collections;
using AmongUs.Data;
using TMPro;
using UnityEngine;

public class AdsMenu : MonoBehaviour
{
	[SerializeField]
	private EjectMainMenu ejectMenu;

	[SerializeField]
	private GameObject loadingObject;

	[SerializeField]
	private TransitionOpen adsPopUp;

	[SerializeField]
	private GameObject adButton;

	[SerializeField]
	private GameObject popUpContentPrompt;

	[SerializeField]
	private GameObject popUpContentSuccess;

	[SerializeField]
	private GameObject popUpContentError;

	[SerializeField]
	private TextMeshPro watchAdText;

	[SerializeField]
	private TextMeshPro rewardText;

	private readonly Logger logger = new Logger(Logger.Category.Ads, "AdsMenu");

	private bool waitingAdResponse;

	public void ClickAdButton()
	{
		int adsWatched = DataManager.Player.Ads.AdsWatched;
		watchAdText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WatchAdBody, adsWatched, 2);
		SetPopUpContent(0);
		adsPopUp.gameObject.SetActive(value: true);
	}

	public void ClickAcceptButton()
	{
		if (!waitingAdResponse)
		{
			loadingObject.SetActive(value: true);
			waitingAdResponse = true;
			logger.Debug("Trying to create rewarded ad");
			DestroyableSingleton<AdsManager>.Instance.CreateRewardedAd(AdReadyCallback);
			adButton.SetActive(value: false);
		}
	}

	private void AdReadyCallback(bool isReady)
	{
		logger.Debug("Ad isReady:" + isReady);
		loadingObject.SetActive(value: false);
		waitingAdResponse = false;
		if (isReady)
		{
			logger.Debug("Trying to show rewarded ad");
			DestroyableSingleton<AdsManager>.Instance.ShowRewardedAd(RewardedAdCallback);
			CloseAdPopUp();
		}
		else
		{
			adsPopUp.gameObject.SetActive(value: false);
			SetPopUpContent(-1);
			adsPopUp.gameObject.SetActive(value: true);
		}
	}

	private void RewardedAdCallback(bool gotReward)
	{
		if (gotReward)
		{
			StartCoroutine(RewardedAdCo());
			return;
		}
		adsPopUp.gameObject.SetActive(value: false);
		SetPopUpContent(-1);
		adsPopUp.gameObject.SetActive(value: true);
	}

	private IEnumerator RewardedAdCo()
	{
		yield return new WaitForSeconds(1f);
		adsPopUp.gameObject.SetActive(value: false);
		int num = DataManager.Player.Ads.HoursLeftNewAd();
		logger.Debug("Hours to new ad: " + num);
		if (num > 1)
		{
			rewardText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RewardAdBody2, num);
		}
		else if (num == 1)
		{
			rewardText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RewardAdBody1, num);
		}
		else
		{
			int adsWatched = DataManager.Player.Ads.AdsWatched;
			rewardText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.WatchAdBody, adsWatched, 2);
		}
		SetPopUpContent(1);
		adsPopUp.gameObject.SetActive(value: true);
		if (DataManager.Player.Ads.CanWatchAdAgain)
		{
			adButton.SetActive(value: true);
			yield break;
		}
		logger.Debug("Reached daily cap");
		adButton.SetActive(value: false);
	}

	private void CloseAdPopUp()
	{
		adsPopUp.Close();
	}

	private void SetPopUpContent(int index)
	{
		popUpContentPrompt.SetActive(value: false);
		popUpContentSuccess.SetActive(value: false);
		popUpContentError.SetActive(value: false);
		switch (index)
		{
		case 0:
			popUpContentPrompt.SetActive(value: true);
			break;
		case 1:
			popUpContentSuccess.SetActive(value: true);
			break;
		default:
			popUpContentError.SetActive(value: true);
			break;
		}
	}
}
