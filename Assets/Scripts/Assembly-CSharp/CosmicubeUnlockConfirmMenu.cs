using System.Collections.Generic;
using UnityEngine;

public class CosmicubeUnlockConfirmMenu : MonoBehaviour
{
	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public PassiveButton PurchaseButton;

	private void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, PurchaseButton, new List<UiElement> { PurchaseButton }, gridNav: true);
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}
}
