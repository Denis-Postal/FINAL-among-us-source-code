using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cpp2ILInjected;
using PowerTools;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class DrillMinigame : Minigame
{
	public SpriteRenderer CaseImage;

	public TextMeshPro statusText;

	public SpriteAnim[] Buttons;

	public AnimationClip BadAnim;

	public AudioClip ButtonSound;

	private int MaxState = 4;

	private int[] states= new int[4];

	private SpriteAnim prevFixedButton;

	private float changeButtonDelay;

	private int[] drillButtonMaps= new int[4] { 20, 22, 21, 24 };

	public void Start()
	{
		PlayerControl.LocalPlayer.SetPlayerMaterialColors(CaseImage);
	}

	// TODO(verify): scale vector, random range and debounce reconstructed from lossy decompile
	public unsafe override void Begin(PlayerTask task)
	{
		base.Begin(task);
		SetupInput(disableCursor: true);
		do
		{
			for (int i = 0; i < states.Length; i++)
			{
				int num = (states[i] = 0);
				Buttons[i].transform.localScale = Vector3.one * Mathf.Lerp(1f, 0.5f, (float)num / (float)MaxState);
				Buttons[i].gameObject.SetActive(num != MaxState);
			}
		}
		while (states.All((int s) => s == MaxState));
	}

	// TODO(verify): scale vector, random range and debounce reconstructed from lossy decompile
	private void Update()
	{
		Player player = ReInput.players.GetPlayer(0);
		if (prevFixedButton != null && changeButtonDelay >= 0f)
		{
			changeButtonDelay -= Time.deltaTime;
			if (changeButtonDelay <= 0f)
			{
				prevFixedButton = null;
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if (player.GetButtonDown(drillButtonMaps[i]) && (prevFixedButton == null || prevFixedButton == Buttons[i]))
			{
				prevFixedButton = Buttons[i];
				changeButtonDelay = 0.25f;
				FixButton(Buttons[i]);
			}
		}
	}

	// TODO(verify): scale vector, random range and debounce reconstructed from lossy decompile
	public unsafe void FixButton(SpriteAnim button)
	{
		int num = Buttons.IndexOf(button);
		if (states[num] != MaxState)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(ButtonSound, loop: false);
			}
			int num2 = ++states[num];
			Buttons[num].transform.localScale = Vector3.one * Mathf.Lerp(1f, 0.5f, (float)num2 / (float)MaxState);
			Buttons[num].gameObject.SetActive(num2 != MaxState);
			if (num2 == MaxState)
			{
				changeButtonDelay = 0f;
			}
			if (states.All((int ss) => ss == MaxState))
			{
				statusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Fine);
				statusText.color = Color.green;
				MyNormTask.NextStep();
				StartCoroutine(CoStartClose());
			}
		}
	}

	public DrillMinigame()
	{ }

	private bool _003CBegin_003Eb__8_0(int s)
	{
		//IL_000f: Expected Obj, but got I4
		object obj = s - MaxState;
		return obj == null;
	}

	private bool _003CFixButton_003Eb__13_0(int ss)
	{
		//IL_000f: Expected Obj, but got I4
		object obj = ss - MaxState;
		return obj == null;
	}
}
