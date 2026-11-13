using System;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using UnityEngine;

public class HideObjectIfMinor : MonoBehaviour
{
	private void Awake()
	
	{
		// TODO(verify): from decomp9
		AccountManager instance = DestroyableSingleton<AccountManager>.Instance;
		if (instance != null)
		{
			instance.OnLoggedInStatusChange += UpdateVisual;
		}
	}

	private void OnDestroy()
	
	{
		AccountManager instance = DestroyableSingleton<AccountManager>.Instance;
		if (instance != null)
		{
			instance.OnLoggedInStatusChange -= UpdateVisual;
		}
	}

	private void Start()
	{
		UpdateVisual();
	}

	private void OnEnable()
	{
		UpdateVisual();
	}

	public void UpdateVisual()
	{
		EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
		if (!instance.IsMinorOrWaiting())
		{
			PlayerData player = DataManager.Player;
			PlayerAccountData account = player.account;
			if (account.loginStatus != EOSManager.AccountLoginStatus.Offline)
			{
				GameObject gameObject = base.gameObject;
				gameObject.SetActive(value: true);
				return;
			}
		}
		GameObject gameObject2 = base.gameObject;
		gameObject2.SetActive(value: false);
	}
}
