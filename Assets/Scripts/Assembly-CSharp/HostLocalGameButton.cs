using System;
using System.Collections;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using InnerNet;
using PowerTools;
using UnityEngine;

public class HostLocalGameButton : MonoBehaviour, IConnectButton
{
	private sealed class _003CCoStartGame_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		public int _003C_003E1__state;

		private object _003C_003E2__current;

		public HostLocalGameButton _003C_003E4__this;

		private float _003Ctime_003E5__2;

		object IEnumerator<object>.Current => _003C_003E2__current;

		object IEnumerator.Current => _003C_003E2__current;

		public _003CCoStartGame_003Ed__10(int _003C_003E1__state)
		{
			((IDisposable)this).Dispose();
			this._003C_003E1__state = _003C_003E1__state;
		}

		void IDisposable.Dispose()
		{
		}

		private IEnumerator inner;

		private bool MoveNext()
		{ // TODO(verify): decomp9 state machine was unreadable; flow rebuilt in HostLocalGameButton.CoStartGameFlow
			if (inner == null)
			{
				inner = _003C_003E4__this.CoStartGameFlow();
			}
			if (inner.MoveNext())
			{
				_003C_003E2__current = inner.Current;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		void IEnumerator.Reset()
		{
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			NotSupportedException ex = new NotSupportedException();
			Cpp2ILHelpers.NoteDecompilerIssue("Method not found @1803A88A0");
			throw ex;
		}
	}

	public AudioClip IntroMusic;

	public SpriteRenderer FillScreen;

	public SpriteAnim connectIcon;

	public AnimationClip connectClip;

	public NetworkModes NetworkMode;

	public void Start()
	{
		if (DestroyableSingleton<MatchMaker>.InstanceExists)
		{
			MatchMaker instance = DestroyableSingleton<MatchMaker>.Instance;
			instance.NotConnecting();
		}
	}

	public void OnClick()
	{
		GameOptionsManager optionsManager = GameOptionsManager.Instance;
		if (optionsManager == null)
		{
			optionsManager = new GameOptionsManager(new UnityLogger());
		}
		if (optionsManager.currentGameOptions == null || optionsManager.currentHostOptions == null)
		{
			optionsManager.Initialize();
		}
		optionsManager.SwitchGameMode(GameModes.Normal);
		if (NetworkMode != NetworkModes.FreePlay)
		{
			PlayerData player = DataManager.Player;
			if (player.ban.IsBanned)
			{
				AmongUsClient instance = AmongUsClient.Instance;
				instance.LastDisconnectReason = DisconnectReasons.IntentionalLeaving;
				DisconnectPopup instance2 = DestroyableSingleton<DisconnectPopup>.Instance;
				instance2.Show();
				return;
			}
			MatchMaker instance3 = DestroyableSingleton<MatchMaker>.Instance;
			if (!instance3.Connecting(this))
			{
				return;
			}
		}
		else
		{
			PlayerData player2 = DataManager.Player;
			string text = player2.customization.Name;
			if (!NameTextBehaviour.IsValidName(text))
			{
				PlayerData player3 = DataManager.Player;
				player3.customization.Name = "";
				PlayerData player4 = DataManager.Player;
				player4.Save();
			}
		}
		Logger.GlobalInstance.Info("Hosting a local game");
		IEnumerator obj = CoStartGameFlow(); // TODO(verify): generated MoveNext is a stub, parent flow reconstructed
		Coroutine coroutine = StartCoroutine(obj);
	}

	public void StartIcon()
	{
		if ((bool)connectIcon)
		{
			connectIcon.Play(connectClip);
		}
	}

	public void StopIcon()
	{
		if ((bool)connectIcon)
		{
			connectIcon.Stop();
			SpriteRenderer component = connectIcon.GetComponent<SpriteRenderer>();
			component.sprite = null;
		}
	}

	public void ClickHideNSeek()
	{
		GameOptionsManager.Instance.SwitchGameMode(GameModes.HideNSeek);
		IEnumerator obj = CoStartGameFlow(); // TODO(verify): generated MoveNext is a stub, parent flow reconstructed
		Coroutine coroutine = StartCoroutine(obj);
	}

	private IEnumerator CoStartGameFlow()
	{ // TODO(verify): inferred from decomp9 fragments (fade-in, StopAllSound, NetworkMode switch, Connect(HostAndClient))
		DestroyableSingleton<MatchMaker>.Instance.NotConnecting();
		if (AmongUsClient.Instance.mode == MatchMakerModes.None && (bool)FillScreen)
		{
			SoundManager.Instance.CrossFadeSound("MainBG", IntroMusic, 0.5f);
			FillScreen.gameObject.SetActive(value: true);
			for (float t = 0f; t < 0.25f; t += Time.deltaTime)
			{
				FillScreen.color = new Color(0f, 0f, 0f, Mathf.Clamp01(t * 4f));
				yield return null;
			}
			FillScreen.color = Color.black;
			SoundManager.Instance.CrossFadeSound("MainBG", null, 0.5f);
			for (float t = 0f; t < 0.25f; t += Time.deltaTime)
			{
				yield return null;
			}
		}
		SoundManager.Instance.StopAllSound();
		AmongUsClient.Instance.NetworkMode = NetworkMode;
		if (NetworkMode == NetworkModes.LocalGame)
		{
			DestroyableSingleton<InnerNetServer>.Instance.StartAsServer();
			AmongUsClient.Instance.SetEndpoint("127.0.0.1", 22023, dtls: false);
			AmongUsClient.Instance.MainMenuScene = "MatchMaking";
			AmongUsClient.Instance.OnlineScene = "OnlineGame";
		}
		else if (NetworkMode == NetworkModes.FreePlay)
		{
			DestroyableSingleton<InnerNetServer>.Instance.StartAsLocalServer();
			AmongUsClient.Instance.SetEndpoint("127.0.0.1", 22023, dtls: false);
			AmongUsClient.Instance.MainMenuScene = "MainMenu";
			AmongUsClient.Instance.OnlineScene = "Tutorial";
		}
		yield return new WaitForSeconds(0.1f);
		AmongUsClient.Instance.Connect(MatchMakerModes.HostAndClient, null);
		yield return AmongUsClient.Instance.WaitForConnectionOrFail();
	}

	private IEnumerator CoStartGame()
	{
		IEnumerator obj = CoStartGameFlow(); // TODO(verify): generated MoveNext is a stub, parent flow reconstructed
		return obj;
	}
}
