using UnityEngine;

public class KidAccount : MonoBehaviour
{
	[SerializeField]
	private GameObject signInButton;

	[SerializeField]
	private GameObject createAccountButton;

	[SerializeField]
	private GameObject randomizeNameButton;

	[SerializeField]
	private GameObject editNameButton;

	[SerializeField]
	private GameObject requestPermission;

	[SerializeField]
	private GameObject logOutButton;

	public void CanSetCustomName(bool canSetName)
	{
		bool active = (byte)((canSetName ? 1u : 0u) ^ 1u) != 0;
		randomizeNameButton.SetActive(active);
		editNameButton.SetActive(canSetName);
	}

	public void CanSignIntoAccount(bool canSignIn)
	{
		signInButton.SetActive(canSignIn);
		createAccountButton.SetActive(value: false);
		bool active = (byte)((canSignIn ? 1u : 0u) ^ 1u) != 0;
		requestPermission.SetActive(active);
	}

	public void HasSignedIntoAccount(bool hasSignedIn)
	{
		bool active = (byte)((hasSignedIn ? 1u : 0u) ^ 1u) != 0;
		signInButton.SetActive(active);
		logOutButton.SetActive(hasSignedIn);
		requestPermission.SetActive(value: false);
		createAccountButton.SetActive(value: false);
	}
}
