using UnityEngine;

public class TriggeredSound : MonoBehaviour
{
	public AudioClip[] SoundToPlay;

	public FloatRange PitchRange = new FloatRange(1f, 1f);

	private AudioSource Player;

	public float MaxVolume = 1f;

	public float MaxDist = 6f;

	public float HitModifier = 0.25f;

	private RaycastHit2D[] volumeBuffer = new RaycastHit2D[5];

	public void Start()
	{
		Player = SoundManager.Instance.GetNamedSfxSource(name + GetHashCode());
		Player.playOnAwake = false;
		Player.loop = false;
	}

	public void PlaySound()
	{
		if ((bool)Player && SoundToPlay != null && PitchRange != null)
		{
			Player.clip = SoundToPlay.Random();
			Player.pitch = PitchRange.Next();
			GetAmbientSoundVolume(Player);
			Player.Play();
		}
	}

	private void GetAmbientSoundVolume(AudioSource player)
	{
		if (!PlayerControl.LocalPlayer)
		{
			player.volume = 0f;
			return;
		}
		Vector2 vector = transform.position;
		Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
		float num = Vector2.Distance(vector, truePosition);
		if (num > MaxDist)
		{
			player.volume = 0f;
			return;
		}
		ContactFilter2D contactFilter = new ContactFilter2D
		{
			useTriggers = false,
			layerMask = Constants.ShipOnlyMask,
			useLayerMask = true
		};
		Vector2 direction = truePosition - vector;
		int num2 = Physics2D.Raycast(vector, direction, contactFilter, volumeBuffer, num);
		float num3 = 1f - num / MaxDist - (float)num2 * HitModifier;
		player.volume = num3 * MaxVolume;
	}
}
