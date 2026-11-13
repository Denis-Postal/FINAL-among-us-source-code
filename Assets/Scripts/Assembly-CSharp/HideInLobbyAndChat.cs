using UnityEngine;

public class HideInLobbyAndChat : MonoBehaviour
{
	public GameObject ObjectToHide;

	private void Update()
	{
		if ((bool)LobbyBehaviour.Instance)
		{
			ObjectToHide.SetActive(value: false);
		}
		else
		{
			ObjectToHide.SetActive(value: true);
		}
		if (DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening)
		{
			ObjectToHide.SetActive(value: false);
		}
	}
}
