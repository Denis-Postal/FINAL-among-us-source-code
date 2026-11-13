using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Events;

public class ChatLanguageMenu : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public uint lang;

		public ChatLanguageMenu _003C_003E4__this;

		internal void _003COnEnable_003Eb__0()
		{
			ChatLanguageMenu chatLanguageMenu = _003C_003E4__this;
			chatLanguageMenu.Parent.SetLanguageFilter(lang);
			GameObject gameObject = chatLanguageMenu.gameObject;
			gameObject.SetActive(value: false);
		}
	}

	public CreateOptionsPicker Parent;

	public ObjectPoolBehavior ButtonPool;

	public UiElement BackButton;

	private UiElement defaultButtonSelected;

	private List<UiElement> controllerSelectable;

	private void Awake()
	{
		defaultButtonSelected = null;
		controllerSelectable = new List<UiElement>();
	}

	public unsafe void OnEnable()
	{
		uint keywords = (uint)Parent.GetTargetOptions().Keywords;
		int num = ChatLanguageSet.Instance.Languages.Count / 10;
		if (ChatLanguageSet.Instance.Languages.Count != 10)
		{
			num++;
		}
		float num2 = ((float)num / 2f - 0.5f) * -2.5f;
		controllerSelectable.Clear();
		int num3 = 0;
		foreach (KeyValuePair<string, uint> language in ChatLanguageSet.Instance.Languages)
		{
			uint lang = language.Value;
			ChatLanguageButton chatLanguageButton = ButtonPool.Get<ChatLanguageButton>();
			chatLanguageButton.transform.localPosition = new Vector3(num2 + (float)(num3 / 10) * 2.5f, 2f - (float)(num3 % 10) * 0.5f, 0f);
			if (language.Key == "Other")
			{
				chatLanguageButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.OtherLanguage);
			}
			else
			{
				chatLanguageButton.Text.text = language.Key;
			}
			chatLanguageButton.Button.OnClick.RemoveAllListeners();
			chatLanguageButton.Button.OnClick.AddListener(() =>
			{
				ChooseOption(lang);
			});
			chatLanguageButton.SetSelected(language.Value == keywords);
			controllerSelectable.Add(chatLanguageButton.Button);
			if (language.Value == keywords)
			{
				defaultButtonSelected = chatLanguageButton.Button;
			}
			num3++;
		}
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, defaultButtonSelected, controllerSelectable);
	}

	public void OnDisable()
	{
		ButtonPool.ReclaimAll();
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	public void Open()
	{
		gameObject.SetActive(value: true);
	}

	public void ChooseOption(uint language)
	{
		Parent.SetLanguageFilter(language);
		Close();
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
	}
}
