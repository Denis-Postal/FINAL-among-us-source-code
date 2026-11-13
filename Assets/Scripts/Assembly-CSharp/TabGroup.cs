using Cpp2ILInjected;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
	public SpriteRenderer Button;

	public ButtonRolloverHandler Rollover;

	public GameObject Content;

	internal unsafe void Close()
	{
		Button.color = Palette.DisabledGrey;
		Rollover.OutColor = Palette.DisabledGrey;
		Content.SetActive(value: false);
	}

	internal unsafe void Open()
	{
		if (ActiveInputManager.currentControlType != ActiveInputManager.InputType.Joystick && ControllerManager.Instance.CurrentUiState.CurrentSelection != null && ControllerManager.Instance.CurrentUiState.CurrentSelection.gameObject == Rollover.gameObject)
		{
			Button.color = Color.white;
		}
		Rollover.OutColor = Color.white;
		Content.SetActive(value: true);
	}
}
