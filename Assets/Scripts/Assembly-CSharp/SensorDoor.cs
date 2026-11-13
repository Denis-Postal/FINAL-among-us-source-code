using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class SensorDoor : MonoBehaviour
{
	public SpriteRenderer LeftSide;

	public SpriteRenderer RightSide;

	public float ActivationDistance = 2f;

	public bool Opening;

	public float OpenDuration;

	private float openTimer;

	public AudioClip OpenSound;

	public AudioClip CloseSound;

	private const float slideVibrationIntensity = 3f;

	public void OnEnable()
	{
		InvokeRepeating("CheckDoor", 0.1f, 0.1f);
		LeftSide.SetCooldownNormalizedUvs();
		RightSide.SetCooldownNormalizedUvs();
	}

	public void SetUvs()
	{
		RightSide.SetCooldownNormalizedUvs();
	}

	private void Update()
	{ /* spliced-from-2019 */
		if (Opening && openTimer < OpenDuration)
		{
			openTimer += Time.deltaTime;
			float value = Mathf.SmoothStep(0f, 1f, openTimer / OpenDuration);
			LeftSide.material.SetFloat("_Percent", value);
			RightSide.material.SetFloat("_Percent", value);
		}
		else if (!Opening && openTimer > 0f)
		{
			openTimer -= Time.deltaTime;
			float value2 = Mathf.SmoothStep(0f, 1f, openTimer / OpenDuration);
			LeftSide.material.SetFloat("_Percent", value2);
			RightSide.material.SetFloat("_Percent", value2);
		}
	}

	private void CheckDoor()
	{
		bool opening = Opening;
		Opening = PhysicsHelpers.CircleContains(transform.position, ActivationDistance, Constants.PlayersOnlyMask);
		if (opening && !Opening)
		{
			SoundManager.Instance.StopSound(OpenSound);
			if (Vector2.Distance(transform.position, PlayerControl.LocalPlayer.GetTruePosition()) < 3f)
			{
				SoundManager.Instance.PlaySound(CloseSound, loop: false);
			}
			VibrationManager.Vibrate(3f, transform.position, 3f, OpenDuration, VibrationManager.VibrationFalloff.None, CloseSound);
		}
		else if (!opening && Opening)
		{
			SoundManager.Instance.StopSound(CloseSound);
			if (Vector2.Distance(transform.position, PlayerControl.LocalPlayer.GetTruePosition()) < 3f)
			{
				SoundManager.Instance.PlaySound(OpenSound, loop: false);
			}
			VibrationManager.Vibrate(3f, transform.position, 3f, OpenDuration, VibrationManager.VibrationFalloff.None, OpenSound);
		}
	}
}
