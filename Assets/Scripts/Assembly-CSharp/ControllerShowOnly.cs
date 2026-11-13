using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.UI;

public class ControllerShowOnly : MonoBehaviour
{
	[SerializeField]
	private Image uiImage;

	private void Start()
	{
		UpdateDisplay();
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(UpdateDisplay));
	}

	private void OnDestroy()
	{
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(UpdateDisplay));
	}

	public void UpdateDisplay()
	{
		if ((bool)uiImage)
		{
			uiImage.gameObject.SetActive(value: false);
		}
		if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick && (bool)uiImage)
		{
			uiImage.gameObject.SetActive(value: true);
		}
	}
}
