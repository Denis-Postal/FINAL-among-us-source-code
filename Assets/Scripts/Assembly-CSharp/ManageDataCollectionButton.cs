using Object = UnityEngine.Object;
using UnityEngine;

public class ManageDataCollectionButton : MonoBehaviour
{
	public GenericPopup PopupPrefab;

	public void ManageData()
	{
	}

	private void ShowPopup(string error)
	{
		if ((bool)PopupPrefab)
		{
			GenericPopup genericPopup = Object.Instantiate(PopupPrefab);
			genericPopup.TextAreaTMP.text = error;
			genericPopup.transform.SetWorldZ(transform.position.z - 1f);
		}
	}
}
