using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Events;

public class FreeplayPopover : MonoBehaviour
{
	[Serializable]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9;

		public static Func<FreeplayPopoverButton, bool> _003C_003E9__6_0;

		public static Func<FreeplayPopoverButton, UiElement> _003C_003E9__6_1;

		static _003C_003Ec()
		{
			_003C_003Ec obj = new _003C_003Ec();
			_003C_003E9 = obj;
		}

		internal bool _003CShow_003Eb__6_0(FreeplayPopoverButton x)
	{
			return x != null && x.gameObject.activeSelf;
	}

		internal UiElement _003CShow_003Eb__6_1(FreeplayPopoverButton x)
		{
			if ((object)x != null)
			{
				return x.button;
			}
			return (UiElement)(object)new NullReferenceException();
		}
	}

	[SerializeField]
	public GameObject content;

	[SerializeField]
	public GameObject background;

	[SerializeField]
	private FreeplayPopoverButton[] buttons;

	[SerializeField]
	private HostLocalGameButton hostGameButton;

	[SerializeField]
	private UiElement backButton;

	private void Awake()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].OnPressEvent += OnMapButtonPressed;
		}
		background.GetComponent<PassiveButton>().OnClick.AddListener(Close);
		background.SetActive(value: false);
		content.SetActive(value: false);
	}

	public void Show()
	{
		content.SetActive(value: true);
		background.SetActive(value: true);
		ControllerManager.Instance.OpenOverlayMenu(name, backButton, buttons[0].Button, (from x in buttons
			where x.gameObject.activeSelf
			select x.Button).ToList());
	}

	public void Close()
	{
		content.SetActive(value: false);
		background.SetActive(value: false);
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void OnMapButtonPressed(FreeplayPopoverButton button)
	{
		background.GetComponent<PassiveButton>().OnClick.RemoveListener(Close);
		FreeplayPopoverButton[] array = buttons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Button.enabled = false;
		}
		PlayMap(button.Map);
	}

	private void PlayMap(MapNames map)
	{
		AmongUsClient.Instance.TutorialMapId = (int)map;
		hostGameButton.OnClick();
	}
}
