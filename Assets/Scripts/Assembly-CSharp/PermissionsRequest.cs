using AmongUs.Data;
using AmongUs.Data.Player;
using UnityEngine;

public class PermissionsRequest : MonoBehaviour
{
	public EmailTextBehaviour emailText;

	public EmailTextBehaviour emailConfirmText;

	public void Show()
	{
		GameObject gameObject = base.gameObject;
		gameObject.SetActive(value: true);
	}

	public void Close()
	{
		TransitionOpen component = GetComponent<TransitionOpen>();
		component.Close();
	}

	public void ContinueWithout()
	{
		EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
		instance.PlayOffline();
		TransitionOpen component = GetComponent<TransitionOpen>();
		component.Close();
	}

	public void SendEmail()
	{
		if (!emailText.ShakeIfInvalid() && !emailConfirmText.ShakeIfInvalid())
		{
			EmailTextBehaviour emailTextBehaviour = emailText;
			TextBoxTMP nameSource = emailTextBehaviour.nameSource;
			EmailTextBehaviour emailTextBehaviour2 = emailConfirmText;
			TextBoxTMP nameSource2 = emailTextBehaviour2.nameSource;
			if (!emailTextBehaviour2.ShakeIfDoesntMatch(nameSource.text, nameSource2.text))
			{
				string emailValidEmail = emailText.GetEmailValidEmail();
				PlayerData player = DataManager.Player;
				player.account.GuardianEmail = emailValidEmail;
				PlayerData player2 = DataManager.Player;
				player2.Save();
				AccountManager instance = DestroyableSingleton<AccountManager>.Instance;
				instance.UpdateGuardianEmailDisplay();
				EOSManager instance2 = DestroyableSingleton<EOSManager>.Instance;
				instance2.CreateKWSUer();
				Close();
			}
		}
	}
}
