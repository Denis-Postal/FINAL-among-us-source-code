using System;
using System.Collections;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using UnityEngine;

public class FindGameButton : MonoBehaviour, IConnectButton
{
	public GameObject loadAnim;

	private void OnEnable()
	{
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
	}

	public unsafe void OnClick()
	{
		AmongUsClient.Instance.CheckOnlinePermissions(ContinueClick, NotOnlinePermissions, StartIcon, checkOnline: false);
	}

	private void NotOnlinePermissions()
	{
	}

	private void ContinueClick()
	{
		DataManager.Settings.Save();
		if (DestroyableSingleton<MatchMaker>.Instance.Connecting(this))
		{
			AmongUsClient.Instance.NetworkMode = NetworkModes.OnlineGame;
			AmongUsClient.Instance.MainMenuScene = "MainMenu";
			StartCoroutine(AmongUsClient.Instance.CoFindGame());
		}
	}

	private void OnDisable()
	{
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
	}

	public void StartIcon()
	{
		if ((bool)loadAnim)
		{
			loadAnim.SetActive(value: true);
		}
	}

	public void StopIcon()
	{
		if ((bool)loadAnim)
		{
			loadAnim.SetActive(value: false);
		}
	}
}
