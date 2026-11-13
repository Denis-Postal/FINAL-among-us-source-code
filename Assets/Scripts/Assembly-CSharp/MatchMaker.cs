using InnerNet;
using UnityEngine;

public class MatchMaker : DestroyableSingleton<MatchMaker>
{
	public TextBoxTMP GameIdText;

	private MonoBehaviour Connecter;

	public void Start()
	{
		if ((bool)GameIdText && (bool)AmongUsClient.Instance)
		{
			GameIdText.SetText(GameCode.IntToGameName(AmongUsClient.Instance.GameId) ?? "");
		}
	}

	public bool Connecting<T>(T button) where T : MonoBehaviour, IConnectButton
	{
		if (!Connecter)
		{
			Connecter = button;
			((IConnectButton)Connecter).StartIcon();
			return true;
		}
		StartCoroutine(Effects.SwayX(Connecter.transform));
		return false;
	}

	public void NotConnecting()
	{
		if ((bool)Connecter)
		{
			((IConnectButton)Connecter).StopIcon();
			Connecter = null;
		}
	}
}
