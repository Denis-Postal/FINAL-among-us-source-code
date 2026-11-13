using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
	public static class SceneManagerCallbacks
	{
		static SceneManagerCallbacks()
		{
			Debug.Log("Hooked up SceneManager callback");
			SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}

		private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
		{
		}

		private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (mode == LoadSceneMode.Additive)
			{
				SceneManager.SetActiveScene(scene);
			}
		}

		public static void Init()
		{
		}
	}

	public string TargetScene;

	public bool disallowBasedOnSwitchParentalControls;

	public bool disallowBasedOnAssetPackDownloads;

	public GameObject ConnectIcon;

	public Button.ButtonClickedEvent BeforeSceneChange;

	private AsyncOperation loadOp;

	public void Click()
	{
		ChangeScene();
	}

	private void ChangeScene()
	{
		Debug.Log("SceneChanger::ChangeScene to: " + TargetScene);
		BeforeSceneChange.Invoke();
		ChangeScene(TargetScene);
	}

	public static void ChangeScene(string target)
	{
		SceneManager.LoadScene(target);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void BeginLoadingScene()
	{
		BeginLoadingSceneInternal(LoadSceneMode.Single);
	}

	public void BeginLoadingSceneAdditive()
	{
		BeginLoadingSceneInternal(LoadSceneMode.Additive);
	}

	private void BeginLoadingSceneInternal(LoadSceneMode mode)
	{
		SceneManagerCallbacks.Init();
		if (loadOp == null)
		{
			Debug.Log("Begin async loading " + TargetScene);
			loadOp = SceneManager.LoadSceneAsync(TargetScene, mode);
			loadOp.allowSceneActivation = false;
		}
	}

	public void AllowFinishLoadingScene()
	{
		if (loadOp != null)
		{
			loadOp.allowSceneActivation = true;
			loadOp = null;
		}
	}
}
