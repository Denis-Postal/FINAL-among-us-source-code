using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NumberSetter : MonoBehaviour
{
	public enum DateType
	{
		DAYS,
		YEARS,
		MONTHS
	}

	private sealed class _003C_003Ec__DisplayClass18_0
	{
		public NumberButton button;

		public NumberSetter _003C_003E4__this;

		internal void _003CUpdateButtons_003Eb__0()
		{
			_003C_003E4__this.SetData(button);
		}
	}

	public NumberButton ButtonPrefab;

	public Scroller ButtonParent;

	public AgeGateScreen ParentAgeGateScreen;

	public float ButtonStart = 0.5f;

	public float ButtonHeight = 0.5f;

	private NumberButton[] AllButtons;

	public NumberMenu parentMenu;

	public NumberSetter dayNumberSetter;

	public DateType dateType = DateType.MONTHS;

	private List<UiElement> selectableObjects= new List<UiElement>();

	public UiElement backButton;

	private UiElement defaultButton;

	private void OnEnable()
	{
		UpdateDays();
		if (AllButtons != null)
		{
			selectableObjects.Clear();
			NumberButton[] allButtons = AllButtons;
			foreach (NumberButton numberButton in allButtons)
			{
				selectableObjects.Add(numberButton.Button);
			}
			if (isActiveAndEnabled)
			{
				UiElement defaultSelection = ((defaultButton != null) ? defaultButton : AllButtons[0].Button);
				ControllerManager.Instance.OpenOverlayMenu(gameObject.name, backButton, defaultSelection, selectableObjects);
			}
		}
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(gameObject.name);
		defaultButton = null;
	}

	public unsafe void SetData(NumberButton selected)
	{
		parentMenu.SetValue(selected.Text.text);
		for (int i = 0; i < AllButtons.Length; i++)
		{
			AllButtons[i].Text.color = Color.white;
		}
		selected.Text.color = Color.green;
		switch (dateType)
		{
		case DateType.MONTHS:
			ParentAgeGateScreen.Month = selected.monthNum;
			dayNumberSetter.UpdateDays();
			break;
		case DateType.DAYS:
			ParentAgeGateScreen.Day = int.Parse(selected.Text.text);
			break;
		case DateType.YEARS:
			ParentAgeGateScreen.Year = int.Parse(selected.Text.text);
			dayNumberSetter.UpdateDays();
			break;
		}
		parentMenu.Close();
	}

	public unsafe void UpdateDays()
	{
		UpdateButtons();
		if (!ParentAgeGateScreen.IsDefaultDayText(parentMenu.text.text) && ParentAgeGateScreen.Day != 0 && dateType == DateType.DAYS)
		{
			int daysInMonth = GetDaysInMonth();
			ParentAgeGateScreen.Day = Mathf.Clamp(ParentAgeGateScreen.Day, 0, daysInMonth);
			parentMenu.text.text = ParentAgeGateScreen.Day.ToString();
		}
	}

	private int GetDaysInMonth()
	{
		int year = ((ParentAgeGateScreen.Year == 0) ? DateTime.UtcNow.Year : ParentAgeGateScreen.Year);
		int month = ((ParentAgeGateScreen.Month == 0) ? 1 : ParentAgeGateScreen.Month);
		return DateTime.DaysInMonth(year, month);
	}

	private unsafe void UpdateButtons()
	{
		Collider2D component = ButtonParent.GetComponent<Collider2D>();
		_ = AllButtons;
		string[] array = new string[0];
		switch (dateType)
		{
		case DateType.MONTHS:
		{
			array = new string[12];
			for (int i = 0; i < 12; i++)
			{
				array[i] = (i + 1).ToString();
			}
			break;
		}
		case DateType.DAYS:
		{
			int daysInMonth = GetDaysInMonth();
			array = new string[daysInMonth];
			for (int j = 0; j < daysInMonth; j++)
			{
				array[j] = (j + 1).ToString();
			}
			break;
		}
		case DateType.YEARS:
		{
			int year = DateTime.Now.Year;
			array = new string[year - 1900];
			for (int num = year; num > 1900; num--)
			{
				array[year - num] = num.ToString();
			}
			break;
		}
		}
		Vector3 localPosition = new Vector3(0f, ButtonStart, -0.5f);
		TryDestroyExistingButtons();
		AllButtons = new NumberButton[array.Length];
		for (int k = 0; k < array.Length; k++)
		{
			NumberButton button = UnityEngine.Object.Instantiate(ButtonPrefab, ButtonParent.Inner);
			AllButtons[k] = button;
			if (dateType == DateType.MONTHS)
			{
				button.Text.text = DestroyableSingleton<TranslationController>.Instance.GetMonthStringViaNumber(k + 1);
			}
			else
			{
				button.Text.text = array[k];
			}
			button.Button.OnClick.AddListener(() =>
			{
				SetData(button);
			});
			button.Button.ClickMask = component;
			switch (dateType)
			{
			case DateType.MONTHS:
				button.monthNum = k + 1;
				if (button.monthNum == ParentAgeGateScreen.Month)
				{
					button.Text.color = Color.green;
					defaultButton = button.Button;
				}
				break;
			case DateType.DAYS:
				if (array[k] == ParentAgeGateScreen.Day.ToString())
				{
					button.Text.color = Color.green;
					defaultButton = button.Button;
				}
				break;
			case DateType.YEARS:
				if (array[k] == ParentAgeGateScreen.Year.ToString())
				{
					button.Text.color = Color.green;
					defaultButton = button.Button;
				}
				break;
			}
			button.transform.localPosition = localPosition;
			localPosition.y -= ButtonHeight;
		}
		ButtonParent.SetBoundsMax((float)array.Length * ButtonHeight - 2f * ButtonStart - 0.1f, 0f);
	}

	private void TryDestroyExistingButtons()
	{
		if (AllButtons == null)
		{
			return;
		}
		NumberButton[] allButtons = AllButtons;
		foreach (NumberButton numberButton in allButtons)
		{
			if ((bool)numberButton)
			{
				UnityEngine.Object.Destroy(numberButton.gameObject);
			}
		}
	}

	public NumberSetter()
	{ }
}
