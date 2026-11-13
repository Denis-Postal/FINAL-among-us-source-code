using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class LobbyTimerExtensionUI : MonoBehaviour
{
	[SerializeField]
	private TimerTextTMP timerText;

	[SerializeField]
	private InfoTextBox popup;

	[SerializeField]
	private TimerTextTMP popupTimerText;

	[SerializeField]
	private AudioClip lobbyTimerPopUpSound;

	private string timerTextTemplate;

	private string popupTitleText;

	private string popupBodyTextTemplate;

	private void Awake()
	{
		string text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyTimerExpiringUnit);
		string text2 = "{0}" + text;
		timerTextTemplate = "<font-weight=800>" + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyTimerExpiringMsg2, text2);
		string text3 = "<color=green>{0}" + text + "</color>";
		popupTitleText = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyTimerExpiringMsg, text3);
		popupBodyTextTemplate = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyTimerExpiringMsg2, text2);
		popup.button1Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyTimerExpiringNo));
		popup.button2Text.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyTimerExpiringOk));
		popup.button1.OnClick.AddListener(() =>
		{
			popup.button1.OnClick.RemoveAllListeners();
			popup.button2.OnClick.RemoveAllListeners();
			popup.Close();
		});
		popup.button2.OnClick.AddListener(() =>
		{
			popup.button1.OnClick.RemoveAllListeners();
			popup.button2.OnClick.RemoveAllListeners();
			LobbyBehaviour.Instance.RpcExtendLobbyTimer();
			popup.Close();
		});
	}

	public void HideAll()
	{
		HideLobbyTimer();
		popup.Close();
	}

	public void ShowLobbyTimer(int timeRemainingSeconds)
	{
		SoundManager.Instance.PlaySound(lobbyTimerPopUpSound, loop: false);
		timerText.SetTimer(timeRemainingSeconds, timerTextTemplate);
		timerText.transform.parent.gameObject.SetActive(value: true);
	}

	public void ShowLobbyTimerPopup(int timeRemainingSeconds, int timeGrantedSeconds)
	{
		popupTimerText.SetTimer(timeRemainingSeconds, popupBodyTextTemplate);
		popup.titleTexxt.SetText(string.Format(popupTitleText, timeGrantedSeconds));
		popup.gameObject.SetActive(value: true);
	}

	private void HideLobbyTimer()
	{
		timerText.transform.parent.gameObject.SetActive(value: false);
	}

	private void _003CAwake_003Eb__7_0()
	{
		InfoTextBox infoTextBox = popup;
		PassiveButton button = infoTextBox.button1;
		button.OnClick.RemoveAllListeners();
		InfoTextBox infoTextBox2 = popup;
		PassiveButton button2 = infoTextBox2.button2;
		button2.OnClick.RemoveAllListeners();
		popup.Close();
	}

	private void _003CAwake_003Eb__7_1()
	{
		InfoTextBox infoTextBox = popup;
		PassiveButton button = infoTextBox.button1;
		button.OnClick.RemoveAllListeners();
		InfoTextBox infoTextBox2 = popup;
		PassiveButton button2 = infoTextBox2.button2;
		button2.OnClick.RemoveAllListeners();
		LobbyBehaviour.Instance.RpcExtendLobbyTimer();
		popup.Close();
	}
}
