using System.Collections;
using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ShowerMinigame : Minigame
{
	public VerticalGauge Gauge;

	public TextMeshPro PercentText;

	private float timer;

	public float MaxTime = 12f;

	public AudioClip washSound;

	public override void Begin(PlayerTask task)
{
		base.Begin(task);
		timer = MaxTime * (float)(int)MyNormTask.Data[0] / 100f;
		PercentText.text = 100 - MyNormTask.Data[0] + "%";
		AirshipStatus airshipStatus = ShipStatus.Instance as AirshipStatus;
		if ((bool)airshipStatus && (bool)airshipStatus.ShowerParticles)
		{
			airshipStatus.ShowerParticles.Play();
		}
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(washSound, loop: true, 1f, SoundManager.Instance.SfxChannel);
		}
		SetupInput(disableCursor: true);
		VibrationManager.Vibrate(2f, 2f, 0f, VibrationManager.VibrationFalloff.None, washSound, loopClip: true);
	}

	public void Update()
{
		if (amClosing == CloseState.None)
		{
			timer += Time.deltaTime;
			MyNormTask.Data[0] = (byte)(timer / MaxTime * 100f);
			Gauge.value = 1f - timer / MaxTime;
			PercentText.text = 100 - MyNormTask.Data[0] + "%";
			if (MyNormTask.Data[0] >= 100)
			{
				MyNormTask.NextStep();
				StartCoroutine(CoStartClose(0.5f));
			}
		}
	}

	public override void Close()
{
		SoundManager.Instance.StopSound(washSound);
		(ShipStatus.Instance as AirshipStatus)?.ShowerParticles.Stop();
		VibrationManager.CancelVibration(washSound);
		base.Close();
	}
}
