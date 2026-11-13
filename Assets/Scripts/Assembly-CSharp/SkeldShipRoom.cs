using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class SkeldShipRoom : PlainShipRoom, IStepWatcher
{
	public AudioClip AmbientSound;

	public float AmbientVolume = 0.7f;

	public float AmbientMaxDist = 8f;

	public Vector2 AmbientOffset;

	public SoundGroup FootStepSounds;

	private RaycastHit2D[] volumeBuffer = new RaycastHit2D[5];

	public int Priority { get; }

	// TODO(verify): audio channel (AmbienceChannel assumed) and the volume maths (2019 baseline) for the ambient sound.
	public void Start()
	{
		if (Constants.ShouldPlaySfx() && (bool)AmbientSound)
		{
			SoundManager.Instance.PlayDynamicSound("Amb " + RoomId, AmbientSound, loop: true, (AudioSource player, float dt) =>
			{
				GetAmbientSoundVolume(player, dt);
			}, SoundManager.Instance.AmbienceChannel);
		}
	}

	public SoundGroup MakeFootstep(PlayerControl player)
	{
		if (!DestroyableSingleton<HudManager>.InstanceExists)
		{
			return null;
		}
		RoomTracker roomTracker = DestroyableSingleton<HudManager>.Instance.roomTracker;
		if ((bool)roomTracker && roomTracker.LastRoom == this)
		{
			return FootStepSounds;
		}
		return null;
	}

	private void GetAmbientSoundVolume(AudioSource player, float dt)
	{
		if (!PlayerControl.LocalPlayer)
		{
			player.volume = 0f;
			return;
		}
		Vector2 vector = (Vector2)transform.position + AmbientOffset;
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		float num = Vector2.Distance(vector, truePosition);
		if (num > AmbientMaxDist)
		{
			player.volume = 0f;
			return;
		}
		Vector2 direction = truePosition - vector;
		int num2 = Physics2D.RaycastNonAlloc(vector, direction, volumeBuffer, num, Constants.ShipOnlyMask);
		float num3 = 1f - num / AmbientMaxDist - (float)num2 * 0.25f;
		player.volume = Mathf.Lerp(player.volume, num3 * AmbientVolume, dt);
	}

	private void _003CStart_003Eb__8_0(AudioSource player, float dt)
	{
		GetAmbientSoundVolume(player, dt);
	}
}
