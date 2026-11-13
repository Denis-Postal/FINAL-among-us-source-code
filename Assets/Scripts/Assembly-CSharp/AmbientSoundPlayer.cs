using System;
using Cpp2ILInjected;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientSoundPlayer : MonoBehaviour
{
	public AudioClip AmbientSound;

	public Collider2D[] HitAreas;

	public float MaxVolume = 1f;

	public float DistanceFallOff = -1f;

	public float FallOffRate = 1f;

	[SerializeField]
	private bool useMuffled;

	public void Start()
	{
		// roles3: recovered from decomp9: PlayDynamicSound(name + instanceId, clip, loop, Dynamics, channel); channel was lost in the decompile
		// (null), assumed ambience channel (high-pass when useMuffled). TODO(verify)
		SoundManager.Instance.PlayDynamicSound(name + GetHashCode(), AmbientSound, loop: true, Dynamics, useMuffled ? SoundManager.Instance.AmbienceHighPassChannel : SoundManager.Instance.AmbienceChannel);
	}

	private void Dynamics(AudioSource source, float dt)
	{
		if (!PlayerControl.LocalPlayer)
		{
			source.volume = 0f;
			return;
		}
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		float target = 0f;
		bool inside = false;
		for (int i = 0; i < HitAreas.Length; i++)
		{
			if (HitAreas[i].OverlapPoint(truePosition))
			{
				inside = true;
				break;
			}
		}
		if (inside)
		{
			target = 1f;
		}
		else if (!(DistanceFallOff < 0f))
		{
			target = 1f - Mathf.Clamp01(Vector2.Distance(base.transform.position, truePosition) / DistanceFallOff);
		}
		source.volume = Mathf.Lerp(source.volume, target * MaxVolume, Mathf.Clamp01(dt * FallOffRate));
	}

	public void OnDestroy()
	{
		if (SoundManager.Instance != null)
		{
			SoundManager.Instance.StopNamedSound(name + GetHashCode());
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (DistanceFallOff > 0f)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, DistanceFallOff);
		}
	}
}
