using System;
using System.Collections;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class WeatherSwitchGame : Minigame
{
	public static StringNames[] ControlNames = new StringNames[6]
	{
		StringNames.NodeCA,
		StringNames.NodeTB,
		StringNames.NodeIRO,
		StringNames.NodePD,
		StringNames.NodeGI,
		StringNames.NodeMLG
	};

	public WeatherControl[] Controls;

	private WeatherNodeTask WeatherTask;

	public Transform buttonGlyph;

	public Vector3 buttonGlyphOffset_Off;

	public Vector3 buttonGlyphOffset_On;

	public AudioClip SwitchSound;

	public void Start()
{
		for (int i = 0; i < Controls.Length; i++)
		{
			WeatherControl obj = Controls[i];
			obj.name = DestroyableSingleton<TranslationController>.Instance.GetString(ControlNames[i]);
			obj.Label.text = DestroyableSingleton<TranslationController>.Instance.GetString(ControlNames[i]);
		}
	}

	public unsafe override void Begin(PlayerTask task)
{
		base.Begin(task);
		WeatherTask = MyNormTask as WeatherNodeTask;
		Controls[WeatherTask.NodeId].SetInactive();
		SetupInput(disableCursor: true);
		Vector3 localPosition = Controls[WeatherTask.NodeId].transform.localPosition + buttonGlyphOffset_On;
		localPosition.z = buttonGlyph.transform.localPosition.z;
		buttonGlyph.transform.localPosition = localPosition;
	}

	private void Update()
	{
		if (ReInput.players.GetPlayer(0).GetButtonDown(20))
		{
			FlipSwitch(WeatherTask.NodeId);
		}
	}

	public unsafe void FlipSwitch(int i)
{
		if (amClosing == CloseState.None && i == WeatherTask.NodeId)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(SwitchSound, loop: false);
			}
			WeatherTask.NextStep();
			Controls[WeatherTask.NodeId].SetActive();
			StartCoroutine(CoStartClose());
			Vector3 localPosition = Controls[WeatherTask.NodeId].transform.localPosition + buttonGlyphOffset_Off;
			localPosition.z = buttonGlyph.transform.localPosition.z;
			buttonGlyph.transform.localPosition = localPosition;
		}
	}
}
