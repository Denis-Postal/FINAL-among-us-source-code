using UnityEngine;

public class AccountsMenu : MonoBehaviour
{
	public void Open()
	{
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: true);
	}

	public void SetValue(string val)
	{
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: false);
	}

	public void Close()
	{
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: false);
	}
}
