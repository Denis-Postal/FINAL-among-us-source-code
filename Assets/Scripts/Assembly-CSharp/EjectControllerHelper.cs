using Cpp2ILInjected;
using UnityEngine;

public class EjectControllerHelper : MonoBehaviour
{
	[SerializeField]
	private EjectMainMenu ejectMenu;

	public void FinshAnim_Event()
	{
		ejectMenu.FinishPressAnim();
	}
}
