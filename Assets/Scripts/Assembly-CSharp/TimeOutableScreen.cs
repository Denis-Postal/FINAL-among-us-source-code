using AmongUs.Data;
using AmongUs.Data.Player;
using UnityEngine;

public class TimeOutableScreen : MonoBehaviour
{
	private float timer;

	private static int screenCount;

	private void OnEnable()
	{
		timer = 0f;
		int num = screenCount + 1;
		screenCount = num;
	}

	private void OnDisable()
	{
		timer = 0f;
		int num = screenCount - 1;
		screenCount = num;
		if (screenCount == 0)
		{
			EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
			if ((bool)instance)
			{
				EOSManager instance2 = DestroyableSingleton<EOSManager>.Instance;
				instance2.HideTimeout();
			}
		}
	}

	private void Update()
	{
		PlayerData player = DataManager.Player;
		PlayerAccountData account = player.account;
		if (account.loginStatus != EOSManager.AccountLoginStatus.Offline)
		{
			EOSManager instance = DestroyableSingleton<EOSManager>.Instance;
			if (instance.loginFlowFinished)
			{
				goto IL_01b0;
			}
		}
		if (ControllerManager.Instance.IsUiControllerActive)
		{
			ControllerManager instance2 = ControllerManager.Instance;
			ControllerUiElementsState currentUiState = instance2.CurrentUiState;
			GameObject gameObject = base.gameObject;
			string text = gameObject.name;
			if (currentUiState.MenuName == text)
			{
				EOSManager instance3 = DestroyableSingleton<EOSManager>.Instance;
				if (instance3.FinishedAssets)
				{
					float unscaledDeltaTime = Time.unscaledDeltaTime;
					float num = (timer = unscaledDeltaTime + timer);
					EOSManager instance4 = DestroyableSingleton<EOSManager>.Instance;
					if (num < instance4.TimeOutTime)
					{
						return;
					}
					EOSManager instance5 = DestroyableSingleton<EOSManager>.Instance;
					instance5.ShowTimeout();
				}
			}
		}
		goto IL_01b0;
		IL_01b0:
		timer = 0f;
	}
}
