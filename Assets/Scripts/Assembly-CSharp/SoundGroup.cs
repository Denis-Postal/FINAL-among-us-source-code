using UnityEngine;

public class SoundGroup : ScriptableObject
{
	public AudioClip[] Clips;

	public AudioClip Random()
	{
		return Clips.Random();
	}
}
