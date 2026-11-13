using UnityEngine;

public class FungleReactorShipRoom : PlainShipRoom, IReactorShipRoom
{
	[SerializeField]
	private GameObject reactorCableLeft;

	[SerializeField]
	private GameObject reactorCableRight;

	[SerializeField]
	private GameObject reactorMain;

	[SerializeField]
	private GameObject reactorMainBroken;

	private void Awake()
	{
		StopMeltdown();
	}

	public void StartMeltdown()
	{
		reactorMain.SetActive(value: false);
		reactorMainBroken.SetActive(value: true);
		reactorCableLeft.SetActive(value: true);
		reactorCableRight.SetActive(value: true);
	}

	public void StopMeltdown()
	{
		reactorMain.SetActive(value: true);
		reactorMainBroken.SetActive(value: false);
		reactorCableLeft.SetActive(value: false);
		reactorCableRight.SetActive(value: false);
	}
}
