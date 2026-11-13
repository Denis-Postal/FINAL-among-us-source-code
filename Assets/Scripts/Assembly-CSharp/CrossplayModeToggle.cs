using System;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class CrossplayModeToggle : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro CrossplayModeText;

	[SerializeField]
	private CreateOptionsPicker OptionsPicker;

	public void OnEnable()
	{
		UpdateDisplay();
	}

	public void CycleMode()
	{
		DataManager.Settings.Multiplayer.CrossPlay = !DataManager.Settings.Multiplayer.CrossPlay;
		DataManager.Settings.Save();
		UpdateDisplay();
		if (OptionsPicker != null)
		{
			OptionsPicker.Refresh();
		}
	}

	public void UpdateDisplay()
	{
		CrossplayModeText.text = (DataManager.Settings.Multiplayer.CrossPlay ? DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.CrossPlayAllPlatforms) : DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.CrossPlaySamePlatform));
	}
}
