using System.Collections;
using Cpp2ILInjected;
using Rewired;
using UnityEngine;
using UnityEngine.Audio;

public class TuneRadioMinigame : Minigame
{
	public RadioWaveBehaviour actualSignal;

	public DialBehaviour dial;

	public SpriteRenderer redLight;

	public SpriteRenderer greenLight;

	public float Tolerance = 0.1f;

	public float targetAngle;

	public bool finished;

	private float steadyTimer;

	public AudioClip StaticSound;

	public AudioClip RadioSound;

	public override void Begin(PlayerTask task)
	{
		base.Begin(task);
		targetAngle = dial.DialRange.Next();
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlayDynamicSound("CommsRadio", RadioSound, loop: true, GetRadioVolume, SoundManager.Instance.SfxChannel);
			SoundManager.Instance.PlayDynamicSound("RadioStatic", StaticSound, loop: true, GetStaticVolume, SoundManager.Instance.SfxChannel);
		}
		SetupInput(disableCursor: true);
	}

	private void GetRadioVolume(AudioSource player, float dt)
	{
		player.volume = 1f - actualSignal.NoiseLevel;
	}

	private void GetStaticVolume(AudioSource player, float dt)
	{
		player.volume = actualSignal.NoiseLevel;
	}

	public unsafe void Update()
	{
		if (finished)
		{
			return;
		}
		Vector2 axis2DRaw = ReInput.players.GetPlayer(0).GetAxis2DRaw(16, 17);
		if (axis2DRaw.sqrMagnitude > 0.9f)
		{
			Vector2 normalized = axis2DRaw.normalized;
			float value = Vector2.SignedAngle(Vector2.up, normalized);
			value = dial.DialRange.Clamp(value);
			dial.SetValue(value);
		}
		float f = Mathf.Abs((targetAngle - dial.Value) / dial.DialRange.Width) * 2f;
		actualSignal.NoiseLevel = Mathf.Clamp(Mathf.Sqrt(f), 0f, 1f);
		if (actualSignal.NoiseLevel <= Tolerance)
		{
			if (!dial.Engaged)
			{
				FinishGame();
				return;
			}
			redLight.color = new Color(Mathf.Lerp(1f, 0.35f, steadyTimer), 0f, 0f);
			steadyTimer += Time.deltaTime;
			if (steadyTimer > 1f)
			{
				FinishGame();
			}
		}
		else
		{
			redLight.color = new Color(1f, 0f, 0f);
			steadyTimer = 0f;
		}
	}

	private unsafe void FinishGame()
{
		greenLight.color = Color.green;
		finished = true;
		dial.enabled = false;
		dial.SetValue(targetAngle);
		actualSignal.NoiseLevel = 0f;
		if (MyTask is HudOverrideTask hudOverrideTask)
		{
			if ((bool)PlayerControl.LocalPlayer)
			{
				ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Comms, 0);
			}
			hudOverrideTask.MarkContributed();
		}
		else
		{
			MyNormTask.NextStep();
		}
		StartCoroutine(CoStartClose());
	}

	public override void Close()
	{
		SoundManager.Instance.StopSound(StaticSound);
		SoundManager.Instance.StopSound(RadioSound);
		base.Close();
	}
}
