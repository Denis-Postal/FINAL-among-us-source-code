using AmongUs.Data;
using AmongUs.Data.Settings;
using UnityEngine;

public class CheckClassicText : MonoBehaviour
{
	[SerializeField]
	private GameObject reportedText;

	[SerializeField]
	private GameObject reportedImage;

	private void OnEnable()
	{
		if (DataManager.Settings.Language.CurrentLanguage == SupportedLangs.English)
		{
			reportedImage.SetActive(value: true);
			reportedText.SetActive(value: false);
		}
	}
}
