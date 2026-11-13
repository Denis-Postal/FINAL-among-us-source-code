using Object = UnityEngine.Object;
using System;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

public class GameManagerCreator : MonoBehaviour
{
	public NormalGameManager NormalGameManagerPrefab;

	public HideAndSeekManager HideAndSeekManagerPrefab;

	public static GameManagerCreator Instance { get; private set; }

	// TODO(verify): rewritten from decomp9 (mode switch was a damaged jump chain): Normal/NormalFools -> NormalGameManagerPrefab, HideNSeek/SeekFools -> HideAndSeekManagerPrefab, else null
	public static GameManager CreateGameManager(GameModes mode)
	{
		if (GameManager.Instance != null)
		{
			Object.Destroy(GameManager.Instance);
		}
		Debug.Log("Creating GameManager: " + mode);
		GameManager result = null;
		switch (mode)
		{
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
			result = Object.Instantiate(Instance.HideAndSeekManagerPrefab);
			break;
		case GameModes.Normal:
		case GameModes.NormalFools:
			result = Object.Instantiate(Instance.NormalGameManagerPrefab);
			break;
		}
		return result;
	}

	private void Awake()
	{
		if (Instance != null)
		{
			Logger.GlobalInstance.Error("There can be only one game manager creator! Destroying this one...");
			Object.Destroy(gameObject);
		}
		else
		{
			Instance = this;
			Object.DontDestroyOnLoad(gameObject);
		}
	}
}
