using UnityEngine;
using UnityEngine.Audio;

public class Viper_KillAnim_Helper : MonoBehaviour
{
	[SerializeField]
	private CosmeticsLayer victCosmetics;

	[SerializeField]
	private AudioClip acidSplashSFX;

	[SerializeField]
	private AudioClip thudSFX;

	[SerializeField]
	private AudioClip acidSpitSFX;

	private void AnimEvent_TurnOffCosmetics()
	{
		victCosmetics.CurrentSkin.Visible = false;
		victCosmetics.SetHatVisorVisible(isVisible: false);
	}

	private void AnimEvent_PlayAcidSplashSFX()
	{
		SoundManager.Instance.PlaySound(acidSplashSFX, loop: false);
		if (Constants.ShouldPlaySfx())
		{
			VibrationManager.Vibrate(3f, 3f, 0f, VibrationManager.VibrationFalloff.None, acidSplashSFX);
		}
	}

	private void AnimEvent_PlayThudSFX()
	{
		SoundManager.Instance.PlaySound(thudSFX, loop: false);
	}

	private void AnimEvent_PlaySpitSFX()
	{
		SoundManager.Instance.PlaySound(acidSpitSFX, loop: false);
	}
}
