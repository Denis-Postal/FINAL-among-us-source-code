using Object = UnityEngine.Object;
using UnityEngine;
using UnityEngine.Audio;

public class JudgeGavel : MonoBehaviour
{
	[SerializeField]
	private AudioClip spinAudio;

	[SerializeField]
	private AudioClip slamAudio;

	[SerializeField]
	private TextTranslatorTMP text;

	public void DestroySelf()
	{
		Object.Destroy(gameObject);
	}

	public void PlaySpinSound()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(spinAudio, loop: false);
		}
	}

	public void PlaySlamSound()
	{
		if (Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySoundImmediate(slamAudio, loop: false);
		}
	}

	public void AddAnotherJudgeBeatYouToItText()
	{
		text.TargetText = StringNames.JudgeBeatYouToIt;
	}

	public void ShakeScreen()
	{
		ScreenShake.Instance.ShakeCamera(0.2f, 0.2f);
	}
}
