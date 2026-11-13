using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class EngineBehaviour : MonoBehaviour
{
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public EngineBehaviour _003C_003E4__this;

		public float pitch;

		internal void _003CPlaySteamSound_003Eb__0(AudioSource p, float d)
		{
			_003C_003E4__this.GetSoundDistance(p, d, pitch);
		}
	}

	public AudioClip ElectricSound;

	public AudioClip SteamSound;

	public float SoundDistance = 5f;

	public void PlayElectricSound()
	{
		// roles3: decomp lost the volume callback and channel (passed null: the sound played at full volume everywhere). 2019 baseline passes GetSoundDistance; SFX channel assumed.
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlayDynamicSound("EngineShock" + name, ElectricSound, loop: false, GetSoundDistance, SoundManager.Instance.SfxChannel);
		}
	}

	public void PlaySteamSound()
	{
		if (Constants.ShouldPlaySfx())
		{
			float pitch = FloatRange.Next(0.7f, 1.1f);
			SoundManager.Instance.PlayDynamicSound("EngineSteam" + name, SteamSound, loop: false, (AudioSource p, float d) => GetSoundDistance(p, d, pitch), SoundManager.Instance.SfxChannel);
			VibrationManager.Vibrate(1f, (Vector2)base.transform.position, SoundDistance, 0f, VibrationManager.VibrationFalloff.Linear, SteamSound, loopClip: false); // TODO(verify): falloff type
		}
	}

	private void GetSoundDistance(AudioSource player, float dt)
	{
		GetSoundDistance(player, dt, 1f);
	}

	private void GetSoundDistance(AudioSource player, float dt, float pitch)
	{
		if (!PlayerControl.LocalPlayer)
		{
			player.volume = 0f;
			return;
		}
		float num = Vector2.Distance(transform.position, PlayerControl.LocalPlayer.GetTruePosition());
		float num2 = 1f - num / SoundDistance;
		player.volume = num2 * 0.8f;
		player.pitch = pitch;
	}
}
