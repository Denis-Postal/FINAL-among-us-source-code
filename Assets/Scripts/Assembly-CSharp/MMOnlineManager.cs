using Object = UnityEngine.Object;
using System.Collections.Generic;
using AmongUs.Data;
using AmongUs.Data.Player;
using Cpp2ILInjected;
using UnityEngine;

public class MMOnlineManager : DestroyableSingleton<MMOnlineManager>
{
	public GameObject HelpMenu;

	public UiElement BackButton;

	public UiElement DefaultButtonSelected;

	public List<UiElement> ControllerSelectable;

	public bool IsControllerManagerSceneInit;

	public void Start()
	{
		ControllerManager.Instance.NewScene(name, BackButton, DefaultButtonSelected, ControllerSelectable);
		IsControllerManagerSceneInit = true;
		if ((bool)VirtualCursor.instance)
		{
			VirtualCursor.instance.gameObject.SetActive(value: false);
		}
		if (HelpMenu != null)
		{
			if (!DataManager.Player.Onboarding.ViewedOnlineHelpMessage)
			{
				DataManager.Player.Onboarding.ViewedOnlineHelpMessage = true;
				DataManager.Player.Save();
				HelpMenu.gameObject.SetActive(value: true);
			}
			else
			{
				HelpMenu.gameObject.SetActive(value: false);
			}
		}
	}

	private void Update()
	{ /* spliced-from-2019 */
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			SceneChanger.ChangeScene("MainMenu");
		}
	}
}
