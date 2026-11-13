using System.Collections.Generic;
using UnityEngine;

public class SinglePopHelp : MonoBehaviour
{
	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public bool GridNav;

	public void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultButtonSelected, ControllerSelectable, GridNav);
		ControllerManager.Instance.CurrentUiState.CurrentSelection = DefaultButtonSelected;
	}

	public void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}
}
