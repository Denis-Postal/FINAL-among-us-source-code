using UnityEngine;

public class WaitForHostPopup : DestroyableSingleton<WaitForHostPopup>
{
	public GameObject Content;

	public UiElement DefaultButtonSelected;

	private void Start()
	{
		Hide();
	}

	public void Show()
	{
		if ((bool)AmongUsClient.Instance && AmongUsClient.Instance.ClientId > 0)
		{
			Content.SetActive(value: true);
			ControllerManager.Instance.OpenOverlayMenu(name, null, DefaultButtonSelected);
		}
	}

	public void ExitGame()
	{
		AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
		Content.SetActive(value: false);
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void Hide()
	{
		Content.SetActive(value: false);
		ControllerManager.Instance.CloseOverlayMenu(name);
	}
}
