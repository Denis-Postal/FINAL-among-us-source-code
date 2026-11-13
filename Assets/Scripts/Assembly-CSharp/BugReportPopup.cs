using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BugReportPopup : MonoBehaviour
{
	[SerializeField]
	[Header("Buttons")]
	private PassiveButton[] closeButtons;

	[SerializeField]
	private PassiveButton submitButton;

	[Header("Form")]
	[SerializeField]
	private DropdownButton categoryDropdown;

	[SerializeField]
	private TextBoxTMP descriptionField;

	[SerializeField]
	private PassiveButton descriptionFieldButton;

	[SerializeField]
	private Checkbox attachScreenshotCheckbox;

	[Header("Feedback & Validation")]
	[SerializeField]
	private TextMeshPro descriptionCharacterCount;

	[SerializeField]
	private TextMeshPro submittedText;

	[Header("Console Controller Navigation")]
	[SerializeField]
	private UiElement backButton;

	[SerializeField]
	private UiElement defaultButtonSelected;

	[SerializeField]
	private List<UiElement> controllerSelectable;

	private bool descriptionEntered;

	private StringNames selectedCategory;

	private void Start()
	{
		StringNames[] bugReportCategories = BugReportService.BugReportCategories;
		foreach (StringNames id in bugReportCategories)
		{
			categoryDropdown.AddOption(DestroyableSingleton<TranslationController>.Instance.GetString(id));
		}
		categoryDropdown.SetOnOptionSelect(UpdateSelectedCategory);
		submitButton.OnClick.AddListener(SubmitReport);
		closeButtons.ForEach((PassiveButton b) =>
		{
			b.OnClick.AddListener(() =>
			{
				gameObject.SetActive(value: false);
			});
		});
		descriptionField.OnChange.AddListener(ValidateDescription);
		descriptionFieldButton.OnClick.AddListener(FocusDescription);
	}

	private void FocusDescription()
	{
		if (!descriptionEntered)
		{
			descriptionField.Clear();
		}
		descriptionField.GiveFocus();
	}

	private void UpdateSelectedCategory(int selectedIndex)
	{
		if (selectedIndex < 0 || selectedIndex >= BugReportService.BugReportCategories.Length)
		{
			selectedIndex = 0;
			categoryDropdown.SetSelectedIndex(0);
		}
		selectedCategory = BugReportService.BugReportCategories[selectedIndex];
	}

	private void OnEnable()
	{
		ControllerManager.Instance.OpenOverlayMenu(name, backButton, defaultButtonSelected, controllerSelectable);
		submitButton.SetButtonEnableState(enabled: true);
		submittedText.gameObject.SetActive(value: false);
		descriptionField.Clear();
		categoryDropdown.SetSelectedIndex(0);
		selectedCategory = BugReportService.BugReportCategories[categoryDropdown.SelectedIndex];
		attachScreenshotCheckbox.IsChecked = false;
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void SubmitReport()
	{
		StringNames bugReportCategory = selectedCategory;
		string descriptionFieldText = descriptionField.text;
		if (descriptionFieldText.Length > descriptionField.characterLimit)
		{
			StartCoroutine(Effects.SwayX(descriptionCharacterCount.transform));
		}
		else if (string.IsNullOrEmpty(descriptionFieldText))
		{
			StartCoroutine(Effects.SwayX(descriptionCharacterCount.transform));
		}
		else if (!BugReportService.Instance.CanSubmitFeedback())
		{
			submittedText.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.BugReportPopUpSubmissionFailedText));
		}
		else if (attachScreenshotCheckbox.IsChecked)
		{
			ControllerManager.Instance.CloseAndResetAll();
			DestroyableSingleton<CoroutineHelper>.Instance.DelayBySeconds(0.2f, () =>
			{
				DestroyableSingleton<CoroutineHelper>.Instance.StartCoroutine(BugReportService.Instance.SubmitUserFeedbackWithScreenshot(bugReportCategory, string.Empty, $"{bugReportCategory}: {descriptionFieldText}"));
			});
		}
		else
		{
			BugReportService.Instance.SubmitUserFeedback(bugReportCategory, string.Empty, $"{bugReportCategory}: {descriptionFieldText}");
			submittedText.SetText(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.BugReportPopUpSubmittedText));
			submitButton.SetButtonEnableState(enabled: false);
			submittedText.gameObject.SetActive(value: true);
		}
	}

	private void ValidateDescription()
	{
		descriptionEntered = true;
		int length = descriptionField.text.Length;
		descriptionCharacterCount.text = $"{length}/{descriptionField.characterLimit}";
		if ((double)length < (double)descriptionField.characterLimit * 0.75)
		{
			descriptionCharacterCount.color = Color.white;
		}
		else if (length < descriptionField.characterLimit)
		{
			descriptionCharacterCount.color = new Color(1f, 1f, 0f, 1f);
		}
		else
		{
			descriptionCharacterCount.color = Color.red;
		}
	}
}
