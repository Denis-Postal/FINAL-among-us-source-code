using Object = UnityEngine.Object;
using TMPro;
using UnityEngine;

public class GenericPopup : MonoBehaviour
{
	public TextMeshPro TextAreaTMP;

	public bool destroyOnClose;

	public void Show(string text = "")
	{
		if ((bool)TextAreaTMP)
		{
			TextAreaTMP.text = text;
		}
		gameObject.SetActive(value: true);
	}

	public void Close()
	{
		if (destroyOnClose)
		{
			Object.Destroy(gameObject);
		}
		else
		{
			gameObject.SetActive(value: false);
		}
	}
}
