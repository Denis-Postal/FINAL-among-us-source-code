using Object = UnityEngine.Object;
using TMPro;
using UnityEngine;

public class NumberMenu : MonoBehaviour
{
	public TextMeshPro text;

	public void Open()
	{
		gameObject.SetActive(value: true);
	}

	public void SetValue(string val)
	{
		text.text = val;
		Close();
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
	}
}
