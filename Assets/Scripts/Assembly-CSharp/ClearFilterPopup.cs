using UnityEngine;

public class ClearFilterPopup : MonoBehaviour
{
	[SerializeField]
	private TransitionOpen transitionOpen;

	public void ClearFilters()
	{
		DestroyableSingleton<FindAGameManager>.Instance.ClearAllFilters();
		Close();
	}

	public void Close()
	{
		transitionOpen.Close();
	}
}
