using System;
using System.Collections;
using AmongUs.Data;
using UnityEngine;

public class EndGameNavigation : MonoBehaviour
{
	public ProgressionScreen ProgressionScreen;

	public EndGameManager EndGameManager;

	[Header("Buttons")]
	public GameObject ContinueButton;

	public SpriteRenderer PlayAgainButton;

	public SpriteRenderer ExitButton;

	public void ShowProgression()
	{
		ProgressionScreen.Activate();
		EndGameManager.gameObject.SetActive(value: false);
		ContinueButton.SetActive(value: false);
		ShowDefaultNavigation();
	}

	public void ShowDefaultNavigation()
	{
		PlayAgainButton.gameObject.SetActive(value: true);
		ExitButton.gameObject.SetActive(value: true);
	}

	public void ShowNavigationToProgressionScreen()
	{
		ContinueButton.SetActive(value: true);
	}

	public void HideButtons()
	{
		ContinueButton.SetActive(value: false);
	}

	public void NextGame()
	{
		PlayAgainButton.gameObject.SetActive(value: false);
		ExitButton.gameObject.SetActive(value: false);
		if (EndGameResult.CachedShowAd && !DataManager.Player.Ads.HasPurchasedAdRemoval)
		{
			EndGameResult.CachedShowAd = false;
		}
		GameManager.DestroyInstance();
		StartCoroutine(CoJoinGame());
	}

	public void Exit()
	{
		PlayAgainButton.gameObject.SetActive(value: false);
		ExitButton.gameObject.SetActive(value: false);
		GameManager.DestroyInstance();
		AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
	}

	private static IEnumerator WaitWithTimeout(Func<bool> success)
	{
		for (float timer = 0f; timer < 5f; timer += Time.deltaTime)
		{
			if (success())
			{
				break;
			}
			yield return null;
		}
	}

	private IEnumerator CoJoinGame()
	{
		AmongUsClient.Instance.JoinGame();
		yield return WaitWithTimeout(() => AmongUsClient.Instance.ClientId >= 0);
		if (AmongUsClient.Instance.ClientId < 0)
		{
			AmongUsClient.Instance.ExitGame(AmongUsClient.Instance.LastDisconnectReason);
		}
	}
}
