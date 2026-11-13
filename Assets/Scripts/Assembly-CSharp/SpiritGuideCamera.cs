using System.Collections.Generic;
using PowerTools;
using UnityEngine;

public class SpiritGuideCamera : MonoBehaviour, IPlayerVisibleItem
{
	[SerializeField]
	private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

	[SerializeField]
	private SpriteAnimNodeSync nodesSync;

	[SerializeField]
	private AudioClip CameraFlashSound;

	public SpriteAnim animator;

	public bool Visible
	{
		get
		{
			return sprites.TrueForAll((SpriteRenderer s) => s.enabled);
		}
		set
		{
			foreach (SpriteRenderer sprite in sprites)
			{
				sprite.enabled = value;
			}
		}
	}

	public void SetNodesParent(SpriteAnimNodes t)
	{
		nodesSync.Parent = t;
	}

	public void PlayCameraFlashSound()
	{
		SoundManager.Instance.PlaySoundImmediate(CameraFlashSound, loop: false);
	}
}
