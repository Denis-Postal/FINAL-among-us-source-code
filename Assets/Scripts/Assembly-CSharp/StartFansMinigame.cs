using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class StartFansMinigame : Minigame
{
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public SpriteRenderer target;

		public StartFansMinigame _003C_003E4__this;

		internal void _003CBegin_003Eb__0()
		{
			_003C_003E4__this.RotateImage(target);
		}
	}

	public TextMeshPro ActionText;

	public SpriteRenderer[] CodeIcons;

	public Sprite[] IconSprites;

	public AudioClip revealSound;

	public AudioClip cycleSound;

	public AudioClip completeSound;

	public PassiveButton mainCodeButton;

	public PassiveButton closeButton;

	public List<UiElement> codeButtons;

	public ControllerButtonBehavior enterCodeHotkey;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		if (ConsoleId == 0)
		{
			ActionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RevealCode);
		}
		else
		{
			ActionText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.EnterCode);
		}
		byte[] data = MyNormTask.Data;
		for (int i = 0; i < CodeIcons.Length; i++)
		{
			SpriteRenderer target = CodeIcons[i];
			CodeIcons[i].transform.parent.gameObject.SetActive(value: false);
			if (ConsoleId == 0)
			{
				target.sprite = IconSprites[data[i]];
				continue;
			}
			CodeIcons[i].GetComponent<PassiveButton>().OnClick.AddListener(() =>
			{
				RotateImage(target);
			});
		}
		List<UiElement> list = new List<UiElement>(1);
		list.Add(mainCodeButton);
		ControllerManager.Instance.OpenOverlayMenu(name, closeButton, mainCodeButton, list);
	}

	public override void Close()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
		base.Close();
	}

	public void RevealCode()
	{
		for (int i = 0; i < CodeIcons.Length; i++)
		{
			CodeIcons[i].transform.parent.gameObject.SetActive(value: true);
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(revealSound, loop: false);
		}
		if (MyNormTask.taskStep == 0)
		{
			MyNormTask.NextStep();
		}
		if (ConsoleId != 0)
		{
			ControllerManager.Instance.CloseOverlayMenu(name);
			ControllerManager.Instance.OpenOverlayMenu(name, closeButton, codeButtons[0], codeButtons);
		}
	}

	public void RotateImage(SpriteRenderer target)
	{
		if (ConsoleId == 0 || amClosing != CloseState.None)
		{
			return;
		}
		int num = IconSprites.IndexOf(target.sprite);
		num = (num + 1) % IconSprites.Length;
		target.sprite = IconSprites[num];
		for (int i = 0; i < CodeIcons.Length; i++)
		{
			SpriteRenderer spriteRenderer = CodeIcons[i];
			int num2 = IconSprites.IndexOf(spriteRenderer.sprite);
			if (MyNormTask.Data[i] != num2)
			{
				if (Constants.ShouldPlaySfx())
				{
					SoundManager.Instance.PlaySound(cycleSound, loop: false);
				}
				return;
			}
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(completeSound, loop: false);
		}
		MyNormTask.NextStep();
		StartCoroutine(CoStartClose());
	}
}
