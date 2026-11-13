using System;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class CrossplayModeCycle : MonoBehaviour
{
	public TextMeshPro crossplayModeText;

	public SpriteRenderer backgroundSprite;

	public void OnEnable()
	{
		UpdateCrossplayButtonText();
	}

	public void CycleCrossplayMode()
	{
		DataManager.Settings.Multiplayer.CrossPlay = !DataManager.Settings.Multiplayer.CrossPlay;
		DataManager.Settings.Save();
		UpdateCrossplayButtonText();
	}

	public void UpdateCrossplayButtonText()
	{
		StringNames id = (DataManager.Settings.Multiplayer.CrossPlay ? StringNames.CrossPlayAllPlatforms : StringNames.CrossPlaySamePlatform);
		crossplayModeText.text = DestroyableSingleton<TranslationController>.Instance.GetString(id);
	}
}
