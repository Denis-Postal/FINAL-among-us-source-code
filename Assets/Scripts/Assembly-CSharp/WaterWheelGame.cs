using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class WaterWheelGame : Minigame
{
	public SpriteRenderer Wheel;

	public VerticalSpriteGauge WaterLevel;

	public SpriteRenderer Watertop;

	public int WheelScale = 4;

	public AudioClip FillStart;

	public AudioClip FillLoop;

	public AudioClip WheelTurn;

	private TouchpadBehavior touchpad;

	private float Rate = 0.01f;

	private AudioSource fillSound;

	private Vector2 prevStickInput= Vector2.zero;

	private bool hadPrev;

	private bool grabbed;

	private float Water
	{
		get
		{
			return WaterLevel.Value;
		}
		set
		{
			float num = Mathf.Clamp(value, 0f, 1f);
			WaterLevel.Value = num;
			MyNormTask.Data[ConsoleId] = (byte)(num * 255f);
		}
	}

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		WaterLevel.Value = (float)(int)MyNormTask.Data[ConsoleId] / 255f;
		if (Constants.ShouldPlaySfx())
		{
			fillSound = SoundManager.Instance.PlaySound(FillStart, loop: false);
		}
		touchpad = GetComponent<TouchpadBehavior>();
		SetupInput(disableCursor: true);
	}

	// Applies a signed rotation (degrees, wrapped to [-180,180]) to the wheel; returns the absolute z change.
	private float TurnWheel(float angle)
	{
		if (angle > 180f)
		{
			angle -= 360f;
		}
		if (-180f > angle)
		{
			angle += 360f;
		}
		float num = angle / (float)WheelScale;
		Vector3 localEulerAngles = Wheel.transform.localEulerAngles;
		float num2 = Mathf.Clamp(localEulerAngles.z + num, 0.0001f, 358.99f);
		float num3 = Mathf.Abs(localEulerAngles.z - num2);
		if (num3 > 1f && Constants.ShouldPlaySfx())
		{
			AudioMixerGroup audioMixer = default;
			AudioSource audioSource = SoundManager.Instance.PlaySound(WheelTurn, loop: false, 1f, audioMixer);
			if (audioSource.timeSamples == 0)
			{
				audioSource.pitch = FloatRange.Next(0.9f, 1.1f);
			}
		}
		Wheel.transform.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, num2);
		Rate += num / (float)(WheelScale * 360);
		return num3;
	}

	// TODO(verify): controller/touchpad input and Watertop position reconstructed from a damaged decompile.
	public unsafe void Update()
	{
		if (amClosing != CloseState.None)
		{
			return;
		}
		Water += Time.deltaTime * Rate;
		if ((bool)fillSound && !fillSound.isPlaying)
		{
			fillSound = SoundManager.Instance.PlaySound(FillLoop, loop: true);
		}
		if ((bool)fillSound)
		{
			fillSound.volume = Mathf.Lerp(0f, 1f, Rate * 5f);
			fillSound.pitch = Mathf.Lerp(0.75f, 1.25f, Water);
		}
		Vector2 vector = ReInput.players.GetPlayer(0).GetAxis2DRaw(13, 14);
		if (touchpad.IsTouching())
		{
			vector = touchpad.GetCenterToTouch().normalized;
		}
		if (vector.magnitude > 0.9f)
		{
			vector = vector.normalized;
			if (hadPrev)
			{
				float num = prevStickInput.AngleSigned(vector);
				if (num > 180f)
				{
					num -= 360f;
				}
				if (num < -180f)
				{
					num += 360f;
				}
				num /= (float)WheelScale;
				Vector3 localEulerAngles = Wheel.transform.localEulerAngles;
				float z = localEulerAngles.z;
				float num2 = Mathf.Clamp(localEulerAngles.z + num, 0.0001f, 358.99f);
				if (Mathf.Abs(localEulerAngles.z - num2) > 1f && Constants.ShouldPlaySfx())
				{
					AudioSource audioSource = SoundManager.Instance.PlaySound(WheelTurn, loop: false);
					if (audioSource.timeSamples == 0)
					{
						audioSource.pitch = FloatRange.Next(0.9f, 1.1f);
					}
				}
				localEulerAngles.z = num2;
				Wheel.transform.localEulerAngles = localEulerAngles;
				Rate += num / (float)(360 * WheelScale);
				float num3 = Mathf.Abs(z - num2) * 0.01f;
				VibrationManager.Vibrate(num3, num3, 0.02f);
			}
			prevStickInput = vector;
			hadPrev = true;
		}
		else
		{
			hadPrev = false;
		}
		if (grabbed)
		{
			Controller controller = DestroyableSingleton<PassiveButtonManager>.Instance.controller;
			Vector2 vector2 = Wheel.transform.position;
			float num4 = (controller.DragStartPosition - vector2).AngleSigned(controller.DragPosition - vector2);
			if (num4 > 180f)
			{
				num4 -= 360f;
			}
			if (num4 < -180f)
			{
				num4 += 360f;
			}
			num4 /= (float)WheelScale;
			Vector3 localEulerAngles2 = Wheel.transform.localEulerAngles;
			float num5 = Mathf.Clamp(localEulerAngles2.z + num4, 0.0001f, 358.99f);
			if (Mathf.Abs(localEulerAngles2.z - num5) > 1f && Constants.ShouldPlaySfx())
			{
				AudioSource audioSource2 = SoundManager.Instance.PlaySound(WheelTurn, loop: false);
				if (audioSource2.timeSamples == 0)
				{
					audioSource2.pitch = FloatRange.Next(0.9f, 1.1f);
				}
			}
			localEulerAngles2.z = num5;
			Wheel.transform.localEulerAngles = localEulerAngles2;
			Rate += num4 / (float)(360 * WheelScale);
			controller.ResetDragPosition();
		}
		else if (WaterLevel.Value >= 1f)
		{
			MyNormTask.NextStep();
			StartCoroutine(CoStartClose());
		}
		Vector3 localPosition = Watertop.transform.localPosition;
		localPosition.y = WaterLevel.TopY;
		Watertop.transform.localPosition = localPosition;
	}

	public void Grab()
	{
		grabbed = !grabbed;
	}

	public override void Close()
	{
		if ((bool)fillSound)
		{
			fillSound.Stop();
		}
		base.Close();
	}

	public WaterWheelGame()
	{ }
}
