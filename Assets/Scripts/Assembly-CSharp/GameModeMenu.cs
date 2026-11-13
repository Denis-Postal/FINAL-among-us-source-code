using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Events;

public class GameModeMenu : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public GameModes entry;

		public GameModeMenu _003C_003E4__this;

		internal void _003COnEnable_003Eb__0()
		{
			GameModeMenu gameModeMenu = _003C_003E4__this;
			gameModeMenu.Parent.SetGameMode(entry);
			GameObject gameObject = gameModeMenu.gameObject;
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
		uint gameMode = (uint)Parent.GetTargetOptions().GameMode;
		float num = ((float)Mathf.CeilToInt((float)Enum.GetNames(typeof(GameModes)).Length / 10f) / 2f - 0.5f) * -2.5f;
		controllerSelectable.Clear();
		int num2 = 0;
		foreach (GameModes entry in Enum.GetValues(typeof(GameModes)))
		{
			if (entry != GameModes.None && entry != GameModes.NormalFools && entry != GameModes.SeekFools)
			{
				ChatLanguageButton chatLanguageButton = ButtonPool.Get<ChatLanguageButton>();
				chatLanguageButton.transform.localPosition = new Vector3(num + (float)(num2 / 10) * 2.5f, 2f - (float)(num2 % 10) * 0.5f, 0f);
				chatLanguageButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(GameModesHelpers.ModeToName[entry]);
				chatLanguageButton.Button.OnClick.RemoveAllListeners();
				chatLanguageButton.Button.OnClick.AddListener(() =>
				{
					ChooseOption(entry);
				});
				chatLanguageButton.SetSelected((int)entry == gameMode);
				controllerSelectable.Add(chatLanguageButton.Button);
				if ((int)entry == gameMode)
				{
					defaultButtonSelected = chatLanguageButton.Button;
				}
				num2++;
			}
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

	public void ChooseOption(GameModes mode)
	{
		Parent.SetGameMode(mode);
		Close();
	}

	public void Close()
	{
		gameObject.SetActive(value: false);
	}
}
