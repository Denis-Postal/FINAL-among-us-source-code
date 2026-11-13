using UnityEngine;

public class FullAccount : MonoBehaviour
{
	[SerializeField]
	private GameObject randomizeNameButton;

	[SerializeField]
	private GameObject editNameButton;

	[SerializeField]
	private GameObject manageAccount;

	public void CanSetCustomName(bool canSetName)
	{
		if (!canSetName)
		{
			DestroyableSingleton<AccountManager>.Instance.CheckAndRegenerateName();
		}
		randomizeNameButton.SetActive(!canSetName);
		editNameButton.SetActive(canSetName);
	}

	public void UpdateLoggedInAccountVisuals()
	{
		// Offline remake: there is no EOS login, linked external accounts or minor/parental state. The local profile can always change its
		// display name; the "0 external accounts" warning (account logger) does not apply.
		CanSetCustomName(canSetName: true);
		// "Manage Account" opens the live account website; not available offline.
		if (manageAccount != null)
		{
			manageAccount.SetActive(value: false);
		}
	}
}
