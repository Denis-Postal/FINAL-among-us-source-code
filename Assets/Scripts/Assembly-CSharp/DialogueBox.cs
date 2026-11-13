using Object = UnityEngine.Object;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
	public TextMeshPro target;

	public UiElement BackButton;

	private void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton);
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void Show(string dialogue)
	{
		target.text = dialogue;
		if ((bool)Minigame.Instance)
		{
			Minigame.Instance.Close();
		}
		if ((bool)Minigame.Instance)
		{
			Minigame.Instance.Close();
		}
		PlayerControl.LocalPlayer.moveable = false;
		PlayerControl.LocalPlayer.NetTransform.Halt();
		gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		gameObject.SetActive(value: false);
		if (!PlayerControl.LocalPlayer.inVent && !PlayerControl.LocalPlayer.onLadder && !PlayerControl.LocalPlayer.inMovingPlat)
		{
			PlayerControl.LocalPlayer.moveable = true;
		}
		Camera.main.GetComponent<FollowerCamera>().Locked = false;
	}
}
