using Object = UnityEngine.Object;
using Cpp2ILInjected;
using UnityEngine;

public class TwitchDeepLink : MonoBehaviour
{
	private void OnEnable()
	{
		if (!CanOpenTwitch.CheckUrl("twitch://broadcast?game_id=510218"))
		{
			gameObject.SetActive(value: false);
		}
	}

	public void OnClick()
	{
		Application.OpenURL("twitch://broadcast?game_id=510218");
	}
}
