using Object = UnityEngine.Object;
using UnityEngine;

public class ShiftButtonsCrossplayEnabled : MonoBehaviour
{
	[SerializeField]
	private GameObject crossplayObj;

	[SerializeField]
	private GameObject chatTypeHelpMenu;

	private const float shiftAmt = 1.11f;

	private unsafe void Start()
	{
		if (!crossplayObj.activeSelf)
		{
			gameObject.transform.localPosition = new Vector3(1.11f, 0f, 0f);
			Vector3 localPosition = chatTypeHelpMenu.transform.localPosition;
			localPosition.x -= 1.8499999f;
			chatTypeHelpMenu.transform.localPosition = localPosition;
		}
	}
}
