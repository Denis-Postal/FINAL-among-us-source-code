using System;
using System.Collections;
using System.Collections.Generic;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ReactorMinigame : Minigame
{
	private Color bad= new Color(1f, 41f / 255f, 0f);

	private Color good= new Color(77f / 255f, 226f / 255f, 213f / 255f);

	private ReactorSystemType reactor;

	public TextMeshPro statusText;

	public SpriteRenderer hand;

	private FloatRange YSweep= new FloatRange(-2.15f, 1.56f);

	public SpriteRenderer sweeper;

	public AudioClip HandSound;

	private bool isButtonDown;

	public unsafe override void Begin(PlayerTask task)
	{
		base.Begin(task);
		reactor = ShipStatus.Instance.Systems[MyTask.StartAt] as ReactorSystemType;
		hand.color = bad;
		SetupInput(disableCursor: false);
	}

	public unsafe void ButtonDown()
	{
		if (!reactor.IsActive || amClosing != CloseState.None)
		{
			return;
		}
		isButtonDown = !isButtonDown;
		if (isButtonDown)
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(HandSound, loop: true);
			}
			ShipStatus.Instance.RpcUpdateSystem(MyTask.StartAt, (byte)(0x40 | ConsoleId));
		}
		else
		{
			SoundManager.Instance.StopSound(HandSound);
			ShipStatus.Instance.RpcUpdateSystem(MyTask.StartAt, (byte)(0x20 | ConsoleId));
		}
		try
		{
			((SabotageTask)MyTask).MarkContributed();
		}
		catch
		{
		}
	}

	// NOTE: the original also pushes a (controller) cursor position via VirtualCursor.instance.SetScreenPosition; value not recoverable, omitted.
	public unsafe void FixedUpdate()
	{
		VirtualCursor.instance.SetScreenPosition(Vector2.zero);
		if (!reactor.IsActive)
		{
			if (amClosing == CloseState.None)
			{
				hand.color = good;
				statusText.text = DestroyableSingleton<TranslationController>.Instance.GetString((MyTask.StartAt == SystemTypes.Reactor) ? StringNames.ReactorNominal : StringNames.SeismicNominal);
				sweeper.enabled = false;
				SoundManager.Instance.StopSound(HandSound);
				StartCoroutine(CoStartClose());
			}
		}
		else if (!isButtonDown)
		{
			statusText.text = DestroyableSingleton<TranslationController>.Instance.GetString((MyTask.StartAt == SystemTypes.Reactor) ? StringNames.ReactorHoldToStop : StringNames.SeismicHoldToStop);
			sweeper.enabled = false;
		}
		else
		{
			statusText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ReactorWaiting);
			Vector3 localPosition = sweeper.transform.localPosition;
			localPosition.y = YSweep.Lerp(Mathf.Sin(Time.time) * 0.5f + 0.5f);
			sweeper.transform.localPosition = localPosition;
			sweeper.enabled = true;
		}
	}

	public override void Close()
	{
		SoundManager.Instance.StopSound(HandSound);
		if ((bool)ShipStatus.Instance)
		{
			ShipStatus.Instance.RpcUpdateSystem(MyTask.StartAt, (byte)(0x20 | ConsoleId));
		}
		base.Close();
	}

	// TODO(verify): bad/good colours from 2019 baseline
	public ReactorMinigame()
	{ }
}
