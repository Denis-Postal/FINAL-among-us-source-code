using AmongUs.Data;
using TMPro;
using UnityEngine;

public class CopyPUIDAccount : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro accountText;

	private bool copiedText;

	public void CopyPUID()
	{
		if (DataManager.Player.Account.LoginStatus != EOSManager.AccountLoginStatus.Offline)
		{
			if (copiedText)
			{
				accountText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AccountIDDisplay, DestroyableSingleton<EOSManager>.Instance.ProductUserId);
				copiedText = false;
			}
			else
			{
				GUIUtility.systemCopyBuffer = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AccountIDDisplay, DestroyableSingleton<EOSManager>.Instance.ProductUserId);
				accountText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.CopiedText);
				copiedText = true;
			}
		}
	}

	private void OnDisable()
	{
		copiedText = false;
	}
}
