using Object = UnityEngine.Object;
using System;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using UnityEngine;
using UnityEngine.Audio;

public class NoisemakerRole : CrewmateRole
{
	[SerializeField]
	private AudioClip deathSound;

	[SerializeField]
	private GameObject deathArrowPrefab;

	private const string SFX_ALERT_NAME = "NoisemakerAlert";

	private const float FALLOFF_START_DISTANCE = 7f;

	private const float FALLOFF_MAX_DISTANCE = 50f;

	private const float MIN_VOL_ALERT = 0.5f;

	private const float VIBRATION_INTENSITY = 1f;

	private const float VIBRATION_DURATION = 1.2f;

	private NoisemakerArrow deathArrow;

	private bool impostorAlert => GameManager.Instance.LogicOptions.GetRoleBool(BoolOptionNames.NoisemakerImpostorAlert);

	private float alertDuration => GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.NoisemakerAlertDuration);

	public unsafe void NotifyOfDeath()
	{
		if ((!PlayerControl.LocalPlayer.Data.Role.IsImpostor || impostorAlert) && !PlayerControl.LocalPlayer.AreCommsAffected())
		{
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlayDynamicSound("NoisemakerAlert", deathSound, loop: false, SoundDynamics, SoundManager.Instance.SfxChannel);
				VibrationManager.Vibrate(1f, PlayerControl.LocalPlayer.GetTruePosition(), 7f, 1.2f);
			}
			GameObject gameObject = Object.Instantiate(deathArrowPrefab, transform.position, Quaternion.identity);
			deathArrow = gameObject.GetComponent<NoisemakerArrow>();
			if (KilledByViper())
			{
				float roleFloat = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ViperDissolveTime);
				deathArrow.SetDuration((roleFloat < alertDuration) ? roleFloat : alertDuration);
			}
			else
			{
				deathArrow.SetDuration(alertDuration);
			}
			if (Player == PlayerControl.LocalPlayer)
			{
				deathArrow.alwaysMaxSize = true;
			}
			deathArrow.gameObject.SetActive(value: true);
			deathArrow.target = Player.GetTruePosition();
		}
	}

	private void SoundDynamics(AudioSource source, float dt)
	{
		if (!PlayerControl.LocalPlayer)
		{
			source.volume = 0f;
			return;
		}
		source.volume = 1f;
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		source.volume = SoundManager.GetSoundVolume(Player.GetTruePosition(), truePosition, 7f, 50f, 0.5f);
	}

	public override void OnDeath(DeathReason reason)
	{
		if (reason == DeathReason.Kill)
		{
			NotifyOfDeath();
		}
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
	}

	private bool KilledByViper()
	{
		return Object.FindObjectsOfType<ViperDeadBody>().Any((ViperDeadBody viperBody) => viperBody.ParentId == Player.Data.PlayerId);
	}

	private bool _003CKilledByViper_003Eb__17_0(ViperDeadBody viperBody)
	{
		return viperBody.ParentId == Player.Data.PlayerId;
	}
}
