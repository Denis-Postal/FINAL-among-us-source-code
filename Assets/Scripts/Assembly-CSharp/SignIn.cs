using UnityEngine;

public class SignIn : MonoBehaviour
{
	[SerializeField]
	private GameObject mainScreen;

	[SerializeField]
	private GameObject signInScreen;

	[SerializeField]
	private GameObject createAccountScreen;

	public void Close()
	{
		TransitionOpen component = GetComponent<TransitionOpen>();
		component.Close();
	}

	public void Open()
	{
		TransitionOpen component = GetComponent<TransitionOpen>();
		component.OnEnable();
	}
}
