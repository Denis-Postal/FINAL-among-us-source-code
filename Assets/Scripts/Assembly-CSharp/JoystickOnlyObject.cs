using Object = UnityEngine.Object;
using System;
using Cpp2ILInjected;
using UnityEngine;

public class JoystickOnlyObject : MonoBehaviour
{
	private void Awake()
	{
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Combine(ActiveInputManager.CurrentInputSourceChanged, new Action(UpdateState));
		UpdateState();
	}

	private void OnDestroy()
	{
		ActiveInputManager.CurrentInputSourceChanged = (Action)Delegate.Remove(ActiveInputManager.CurrentInputSourceChanged, new Action(UpdateState));
	}

	private void UpdateState()
	{
		gameObject.SetActive(ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
	}
}
