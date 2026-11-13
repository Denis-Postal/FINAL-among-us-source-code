using Object = UnityEngine.Object;
using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
	public void Start()
	{
		if (!DestroyableSingleton<HudManager>.InstanceExists)
		{
			gameObject.SetActive(value: false);
		}
	}

	public void OnClick()
	{
		if ((bool)AmongUsClient.Instance)
		{
			AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
		}
		else
		{
			SceneChanger.ChangeScene("MainMenu");
		}
	}
}
