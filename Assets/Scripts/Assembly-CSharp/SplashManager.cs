using System;
using System.Collections;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
	[SerializeField]
	private SplashErrorPopup errorPopup;

	[SerializeField]
	private SceneChanger sceneChanger;

	[SerializeField]
	private float minimumSecondsBeforeSceneChange;

	[SerializeField]
	private LogoAnimFinish logoAnimFinish;

	[SerializeField]
	private GameObject loadingObject;

	private bool doneLoadingRefdata;

	private bool startedSceneLoad;

	private float startTime;

	private void Start()
	{
		startTime = Time.time;
		StartCoroutine(InitializeRefdata());
	}

	private void Update()
	{
		if (doneLoadingRefdata && !startedSceneLoad && Time.time - startTime > minimumSecondsBeforeSceneChange)
		{
			sceneChanger.AllowFinishLoadingScene();
			startedSceneLoad = true;
		}
	}

	private IEnumerator InitializeRefdata()
	{
		LogoAnimFinish logoAnimFinish = this.logoAnimFinish;
		logoAnimFinish.FinishLogoAnim = (Action)Delegate.Combine(logoAnimFinish.FinishLogoAnim, new Action(CheckLoadingRefData));
		DestroyableSingleton<ReferenceDataManager>.Instance.showLoadingBar = false;
		yield return DestroyableSingleton<ReferenceDataManager>.Instance.Initialize();
		try
		{
			DestroyableSingleton<TranslationController>.Instance.Initialize();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		LogoAnimFinish logoAnimFinish2 = this.logoAnimFinish;
		logoAnimFinish2.FinishLogoAnim = (Action)Delegate.Remove(logoAnimFinish2.FinishLogoAnim, new Action(CheckLoadingRefData));
		DestroyableSingleton<ReferenceDataManager>.Instance.showLoadingBar = true;
		DestroyableSingleton<LoadingBarManager>.Instance.ToggleLoadingBar(on: false);
		sceneChanger.BeginLoadingScene();
		doneLoadingRefdata = true;
		Logger.GlobalInstance.Info("Done initializing refdata");
	}

	private void CheckLoadingRefData()
	{
		if (!doneLoadingRefdata)
		{
			DestroyableSingleton<ReferenceDataManager>.Instance.showLoadingBar = true;
		}
	}
}
