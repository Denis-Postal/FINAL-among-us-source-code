using System.Collections;
using UnityEngine;

public class TagAmbientSoundPlayer : MonoBehaviour
{
	public AudioClip AmbientSound;

	public float MaxVolume = 1f;

	public string TargetTag = "NoSnow";

	private float targetVolume;

	public void Start()
	{
		SoundManager.Instance.PlayDynamicSound(name + GetHashCode(), AmbientSound, loop: true, Dynamics, SoundManager.Instance.AmbienceChannel);
		StartCoroutine(Run());
	}

	private void Dynamics(AudioSource source, float dt)
	{
		source.volume = Mathf.Lerp(source.volume, targetVolume * MaxVolume, dt);
	}

	public void OnDestroy()
	{
		SoundManager.Instance.StopNamedSound(name + GetHashCode());
	}

	private IEnumerator Run()
	{
		ContactFilter2D filter = new ContactFilter2D
		{
			layerMask = Constants.ShipOnlyMask,
			useLayerMask = true,
			useTriggers = true
		};
		Collider2D[] buffer = new Collider2D[10];
		WaitForSeconds wait = new WaitForSeconds(0.1f);
		while (true)
		{
			yield return wait;
			if (!PlayerControl.LocalPlayer)
			{
				continue;
			}
			bool flag = false;
			int num = PlayerControl.LocalPlayer.Collider.Overlap(filter, buffer);
			for (int i = 0; i < num; i++)
			{
				if (buffer[i].tag == TargetTag)
				{
					flag = true;
					break;
				}
			}
			targetVolume = ((!flag) ? 1 : 0);
		}
	}
}
