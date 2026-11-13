using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Settings;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LanguageSetter : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public LanguageButton button;

		public LanguageSetter _003C_003E4__this;

		internal void _003CStart_003Eb__0()
		{
			_003C_003E4__this.SetLanguage(button);
		}
	}

	public LanguageButton ButtonPrefab;

	public Scroller ButtonParent;

	public float ButtonStart = 0.5f;

	public float ButtonHeight = 0.5f;

	private LanguageButton[] AllButtons;

	public TextMeshPro parentLangButton;

	private List<UiElement> selectableButtons= new List<UiElement>();

	public UiElement backButton;

	private void OnEnable()
	{
		if (AllButtons == null)
		{
			return;
		}
		LanguageButton languageButton = null;
		LanguageButton[] allButtons = AllButtons;
		foreach (LanguageButton languageButton2 in allButtons)
		{
			if (languageButton2.Title.color == Color.green)
			{
				languageButton = languageButton2;
			}
		}
		ControllerManager.Instance.OpenOverlayMenu(gameObject.name, backButton, languageButton.Button, selectableButtons);
	}

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(gameObject.name);
	}

	public unsafe void Start()
	{
		TranslatedImageSet[] array = DestroyableSingleton<TranslationController>.Instance.Languages.Values.ToArray();
		Collider2D component = ButtonParent.GetComponent<Collider2D>();
		bool flag = false;
		LanguageButton languageButton = null;
		if (AllButtons == null)
		{
			flag = true;
		}
		Vector3 localPosition = new Vector3(0f, ButtonStart, -0.5f);
		AllButtons = new LanguageButton[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			LanguageButton button = Object.Instantiate(ButtonPrefab, ButtonParent.Inner);
			AllButtons[i] = button;
			button.Language = array[i];
			button.Title.text = array[i].Name;
			if (array[i].languageID == DataManager.Settings.Language.CurrentLanguage)
			{
				languageButton = button;
				button.Title.color = Color.green;
				parentLangButton.text = array[i].Name;
			}
			button.Button.OnClick.AddListener(() =>
			{
				SetLanguage(button);
			});
			button.Button.ClickMask = component;
			button.transform.localPosition = localPosition;
			localPosition.y -= ButtonHeight;
		}
		if (flag)
		{
			LanguageButton[] allButtons = AllButtons;
			foreach (LanguageButton languageButton2 in allButtons)
			{
				selectableButtons.Add(languageButton2.Button);
			}
			ControllerManager.Instance.OpenOverlayMenu(gameObject.name, backButton, (languageButton != null) ? languageButton.Button : AllButtons[0].Button, selectableButtons);
		}
		ButtonParent.SetBoundsMax((float)array.Length * ButtonHeight - 2f * ButtonStart - 0.1f, 0f);
	}

	public unsafe void SetLanguage(LanguageButton selected)
	{
		for (int i = 0; i < AllButtons.Length; i++)
		{
			AllButtons[i].Title.color = Color.white;
		}
		selected.Title.color = Color.green;
		parentLangButton.text = selected.Language.Name;
		DestroyableSingleton<TranslationController>.Instance.SetLanguage(selected.Language.languageID);
		Close();
	}

	public void Open()
	{
		gameObject.SetActive(value: true);
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
	}

	public LanguageSetter()
	{ }
}
