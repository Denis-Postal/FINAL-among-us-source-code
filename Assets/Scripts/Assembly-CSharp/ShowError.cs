using UnityEngine;

[RequireComponent(typeof(PassiveButton))]
public class ShowError : MonoBehaviour
{
	[TextArea(2, 5)]
	public string ErrorMessage = "This button doesn't work because the game has been decompiled.";

	private PassiveButton button;

	private void Awake()
	{
		button = GetComponent<PassiveButton>();
	}

	private void OnEnable()
	{
		if (button != null)
		{
			button.OnClick.AddListener(Show);
		}
	}

	private void OnDisable()
	{
		if (button != null)
		{
			button.OnClick.RemoveListener(Show);
		}
	}

	private void Show()
	{
		if (!string.IsNullOrWhiteSpace(ErrorMessage))
		{
			DestroyableSingleton<DisconnectPopup>.Instance.ShowCustom(ErrorMessage);
		}
	}
}
