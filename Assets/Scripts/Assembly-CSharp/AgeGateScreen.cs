using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AmongUs.Data;
using TMPro;
using UnityEngine;

public class AgeGateScreen : MonoBehaviour
{
	[SerializeField]
	private GameObject EnterDateScreen;

	[SerializeField]
	private GameObject MoreInfoScreen;

	[SerializeField]
	private GameObject VerifyScreen;

	[SerializeField]
	private Color DefaultColor;

	[SerializeField]
	private Color FilledColor;

	[SerializeField]
	private TextMeshPro privacyInfoText;

	[SerializeField]
	private TextMeshPro monthText;

	[SerializeField]
	private TextMeshPro dayText;

	[SerializeField]
	private TextMeshPro yearText;

	[SerializeField]
	private TextMeshPro fullDateText;

	private List<SelectableHyperLink> selectableHyperLinks = new List<SelectableHyperLink>();

	private string defaultMonthText;

	private string defaultDayText;

	private string defaultYearText;

	public int Day;

	public int Month;

	public int Year;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	public UiElement DefaultSelection;

	public List<UiElement> selectableObjects;

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void OnEnable()
	{
		SelectableHyperLinkHelper.AddSelectableUiForHyperlinks(selectableHyperLinks, name);
	}

	public IEnumerator Show()
	{
		DateTime dateOfBirth = DataManager.Player.Age.DateOfBirth;
		Year = dateOfBirth.Year;
		Month = dateOfBirth.Month;
		Day = dateOfBirth.Day;
		defaultMonthText = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Month);
		defaultDayText = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Day);
		defaultYearText = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Year);
		monthText.text = defaultMonthText;
		dayText.text = defaultDayText;
		yearText.text = defaultYearText;
		gameObject.SetActive(value: true);
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, DefaultSelection, selectableObjects);
		while (gameObject.activeSelf)
		{
			yield return null;
		}
	}

	public bool IsDefaultDayText(string text)
	{
		return text == defaultDayText;
	}

	private void Update()
	{
		monthText.color = ((monthText.text == defaultMonthText) ? DefaultColor : FilledColor);
		dayText.color = ((dayText.text == defaultDayText) ? DefaultColor : FilledColor);
		yearText.color = ((yearText.text == defaultYearText) ? DefaultColor : FilledColor);
	}

	public void Close()
	{
		if (!ShakeIfInvalid())
		{
			DataManager.Player.Age.LatestSetDate = AmongUsDateTime.UtcNow;
			DataManager.Player.Age.DateOfBirth = new DateTime(Year, Month, Day);
			DataManager.Player.Save();
			GetComponent<TransitionOpen>().Close();
		}
	}

	public void GoToVerify()
	{
		if (!ShakeIfInvalid())
		{
			fullDateText.text = new DateTime(Year, Month, Day).ToString(DestroyableSingleton<TranslationController>.Instance.dateFormats[DataManager.Settings.Language.CurrentLanguage], CultureInfo.InvariantCulture);
			EnterDateScreen.SetActive(value: false);
			VerifyScreen.SetActive(value: true);
		}
	}

	public void GoBackToEdit()
	{
		EnterDateScreen.SetActive(value: true);
		VerifyScreen.SetActive(value: false);
	}

	public void ToggleMoreInfo()
	{
		SelectableHyperLinkHelper.DestroyGOs(selectableHyperLinks, name);
		MoreInfoScreen.SetActive(!MoreInfoScreen.activeSelf);
		EnterDateScreen.SetActive(!EnterDateScreen.activeSelf);
		if (MoreInfoScreen.activeSelf)
		{
			privacyInfoText.text = SelectableHyperLinkHelper.DecomposeAnnouncementText(privacyInfoText, selectableHyperLinks, name, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.AgeVerificationMoreInfo));
			SelectableHyperLinkHelper.UpdateHyperlinkPositions(privacyInfoText, selectableHyperLinks, name);
		}
	}

	public bool ShakeIfInvalid()
	{
		if (yearText.text == AmongUsDateTime.UtcNow.Year.ToString() || monthText.text == DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Month) || dayText.text == DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Day) || yearText.text == DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Year))
		{
			StartCoroutine(Effects.SwayX(transform));
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		SelectableHyperLinkHelper.DestroyGOs(selectableHyperLinks, name);
	}
}
