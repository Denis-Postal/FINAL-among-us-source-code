using PowerTools;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class ButtonAnimRolloverHandler : MonoBehaviour
{
	public Sprite StaticOutImage;

	public AnimationClip RolloverAnim;

	public AudioClip HoverSound;

	private SpriteRenderer target;

	private SpriteAnim animTarget;

	public void Start()
	{
		target = GetComponent<SpriteRenderer>();
		animTarget = GetComponent<SpriteAnim>();
		PassiveButton component = GetComponent<PassiveButton>();
		component.OnMouseOver.AddListener(DoMouseOver);
		component.OnMouseOut.AddListener(DoMouseOut);
	}

	public void DoMouseOver()
	{
		animTarget.Play(RolloverAnim);
		if ((bool)HoverSound)
		{
			SoundManager.Instance.PlaySound(HoverSound, loop: false);
		}
	}

	public void DoMouseOut()
	{
		animTarget.Stop();
		target.sprite = StaticOutImage;
	}
}
