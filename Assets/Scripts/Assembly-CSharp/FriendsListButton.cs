using System;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Player;
using Assets.InnerNet;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FriendsListButton : MonoBehaviour
{
	public GameObject Button;

	public TextMeshPro NotifCount;

	public GameObject NotifCircle;

	[SerializeField]
	public ActionMapGlyphDisplay gamepadButton;

	private bool showInScene;

	private void Awake()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void OnDestroy()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private unsafe void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	private void Update()
	{
		PlayerData player = DataManager.Player;
		PlayerAccountData account = player.account;
		if (account.loginStatus != EOSManager.AccountLoginStatus.Offline && showInScene && !(ShipStatus.Instance != null))
		{
			Button.SetActive(value: true);
		}
		else
		{
			Button.SetActive(value: false);
		}
	}

	public void CheckNotifCount()
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}

	public unsafe void SetGlyphColor(bool isGrayedOut)
	{
		// TODO(verify): no-op, original logic unrecoverable from decomp
	}
}
