using Object = UnityEngine.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class SwitchMinigame : Minigame
{
	public Color OnColor= Color.green;

	public Color OffColor= new Color(0.1f, 0.3f, 0.1f);

	private ShipStatus ship;

	public SpriteRenderer[] switches;

	public SpriteRenderer[] lights;

	public RadioWaveBehaviour top;

	public HorizontalGauge middle;

	public FlatWaveBehaviour bottom;

	public AudioClip FlipSound;

	public Transform selectorHighlight;

	private int selectedIndex;

	private float selectorCooldown;

	private bool prevHadVerticalInput;

	public unsafe override void Begin(PlayerTask task)
	{
		base.Begin(task);
		ship = Object.FindObjectOfType<ShipStatus>();
		SwitchSystem switchSystem = ship.Systems[SystemTypes.Electrical] as SwitchSystem;
		for (int i = 0; i < switches.Length; i++)
		{
			byte b = (byte)(1 << i);
			int num = switchSystem.ActualSwitches & b;
			lights[i].color = ((num == (switchSystem.ExpectedSwitches & b)) ? OnColor : OffColor);
			switches[i].flipY = num >> i == 0;
		}
		SetupInput(disableCursor: true);
	}

	public unsafe void FixedUpdate()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		selectorHighlight.gameObject.SetActive(ActiveInputManager.currentControlType == ActiveInputManager.InputType.Joystick);
		Player player = ReInput.players.GetPlayer(0);
		float axisRaw = player.GetAxisRaw(13);
		float axisRaw2 = player.GetAxisRaw(17);
		if (Mathf.Abs(axisRaw) > 0.7f)
		{
			if (selectorCooldown <= 0f)
			{
				selectedIndex += (int)Mathf.Sign(axisRaw);
				selectedIndex = Mathf.Clamp(selectedIndex, 0, switches.Length);
				selectorHighlight.transform.localPosition = switches[selectedIndex].transform.localPosition;
				selectorCooldown = 0.2f;
			}
			else
			{
				selectorCooldown -= Time.deltaTime;
			}
		}
		else
		{
			selectorCooldown = 0f;
		}
		if (Mathf.Abs(axisRaw2) > 0.7f)
		{
			if (!prevHadVerticalInput)
			{
				if (axisRaw2 > 0.7f)
				{
					if (switches[selectedIndex].flipY)
					{
						FlipSwitch(selectedIndex);
					}
				}
				else if (axisRaw2 < -0.7f && !switches[selectedIndex].flipY)
				{
					FlipSwitch(selectedIndex);
				}
			}
			prevHadVerticalInput = true;
		}
		else
		{
			prevHadVerticalInput = false;
		}
		int num = 0;
		SwitchSystem switchSystem = ship.Systems[SystemTypes.Electrical] as SwitchSystem;
		for (int i = 0; i < switches.Length; i++)
		{
			byte b = (byte)(1 << i);
			int num2 = switchSystem.ActualSwitches & b;
			if (num2 == (switchSystem.ExpectedSwitches & b))
			{
				num++;
				lights[i].color = OnColor;
			}
			else
			{
				lights[i].color = OffColor;
			}
			switches[i].flipY = num2 >> i == 0;
		}
		float num3 = (float)num / (float)switches.Length;
		bottom.Center = 0.47f * num3;
		top.NoiseLevel = 1f - num3;
		middle.Value = switchSystem.Level + (Mathf.PerlinNoise(0f, Time.time * 51f) - 0.5f) * 0.04f;
		if (num == switches.Length)
		{
			StartCoroutine(CoStartClose(0.5f));
		}
	}

	public void FlipSwitch(int switchIdx)
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		int num = 0;
		SwitchSystem switchSystem = ship.Systems[SystemTypes.Electrical] as SwitchSystem;
		for (int i = 0; i < switches.Length; i++)
		{
			byte b = (byte)(1 << i);
			if ((switchSystem.ActualSwitches & b) == (switchSystem.ExpectedSwitches & b))
			{
				num++;
			}
		}
		if (num == switches.Length)
		{
			return;
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(FlipSound, loop: false);
		}
		ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Electrical, (byte)switchIdx);
		try
		{
			((SabotageTask)MyTask).MarkContributed();
		}
		catch
		{
		}
	}

	public SwitchMinigame()
	{ }
}
