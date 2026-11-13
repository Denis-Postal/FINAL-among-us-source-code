using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditName : MonoBehaviour
{
	public NameTextBehaviour nameText;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultSelection;

	public List<UiElement> selectableObjects;

	private void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultSelection, selectableObjects);
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public IEnumerator Show()
	{
		gameObject.SetActive(value: true);
		while (gameObject.activeSelf)
		{
			yield return null;
		}
	}

	public void Close()
	{
		GetComponent<TransitionOpen>().Close();
	}

	public void UpdateName()
	{
		if (!nameText.ShakeIfInvalid())
		{
			nameText.UpdateName();
			DestroyableSingleton<AccountManager>.Instance.UpdateAccountInfoDisplays();
			Close();
		}
	}

	public void RandomizeName()
	{
		nameText.nameSource.SetText(DestroyableSingleton<AccountManager>.Instance.GetRandomName());
	}
}
