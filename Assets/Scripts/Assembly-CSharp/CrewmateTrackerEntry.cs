using Object = UnityEngine.Object;
using PowerTools;
using UnityEngine;
using UnityEngine.Audio;

public class CrewmateTrackerEntry : MonoBehaviour
{
	[SerializeField]
	private SpriteAnim crewmateAnimator;

	[SerializeField]
	private SpriteAnim slashAnimator;

	[SerializeField]
	private AnimationClip aliveClip;

	public bool IsKilled { get; private set; }

	public void SetKilled(CrewmateTrackerAnim anim)
	{
		if (!IsKilled)
		{
			IsKilled = true;
			slashAnimator.gameObject.SetActive(value: true);
			slashAnimator.Play(anim.SlashAnim);
			crewmateAnimator.Play(anim.CrewmateAnim);
			if (Constants.ShouldPlaySfx())
			{
				SoundManager.Instance.PlaySound(anim.Sfx, loop: false);
			}
		}
	}

	public void Reset()
	{
		if (IsKilled)
		{
			IsKilled = false;
			slashAnimator.gameObject.SetActive(value: false);
			crewmateAnimator.Play(aliveClip);
		}
	}
}
