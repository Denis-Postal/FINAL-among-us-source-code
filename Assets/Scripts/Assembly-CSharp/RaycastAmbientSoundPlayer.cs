using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class RaycastAmbientSoundPlayer : MonoBehaviour
{
	public AudioClip AmbientSound;

	public float AmbientVolume = 1f;

	public float AmbientMaxDist = 8f;

	public float HitModifier = 0.25f;

	public bool ShouldVibrate = true;

	public static List<RaycastAmbientSoundPlayer> players= new List<RaycastAmbientSoundPlayer>();

	public float ambientVolume;

	public float t;

	private RaycastHit2D[] volumeBuffer = new RaycastHit2D[5];

	private void OnEnable()
	{
		players.Add(this);
	}

	private void OnDisable()
	{
		players.Remove(this);
	}

	public void Start()
	{
		if (Constants.ShouldPlaySfx() && (bool)AmbientSound)
		{
			SoundManager.Instance.PlayDynamicSound("Amb " + name, AmbientSound, loop: true, (AudioSource player, float dt) =>
			{
				GetAmbientSoundVolume(player, dt);
			}, SoundManager.Instance.AmbienceChannel);
		}
	}

	private void GetAmbientSoundVolume(AudioSource player, float dt)
	{
		ambientVolume = 0f;
		if (!PlayerControl.LocalPlayer)
		{
			player.volume = 0f;
			return;
		}
		Vector2 vector = transform.position;
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		float num = Vector2.Distance(vector, truePosition);
		if (num > AmbientMaxDist)
		{
			player.volume = 0f;
			return;
		}
		Vector2 direction = truePosition - vector;
		int num2 = Physics2D.RaycastNonAlloc(vector, direction, volumeBuffer, num, Constants.ShipOnlyMask);
		float num3 = 1f - num / AmbientMaxDist - (float)num2 * HitModifier;
		player.volume = Mathf.Lerp(player.volume, num3 * AmbientVolume, dt);
		ambientVolume = player.volume;
	}

	static RaycastAmbientSoundPlayer()
	{ }

	private void _003CStart_003Eb__10_0(AudioSource player, float dt)
	{
		GetAmbientSoundVolume(player, dt);
	}
}
