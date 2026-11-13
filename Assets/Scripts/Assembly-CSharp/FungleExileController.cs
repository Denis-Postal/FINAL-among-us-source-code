using System.Collections;
using UnityEngine;

public class FungleExileController : ExileController
{
	private const float FADE_BLACK_PLAYER_DURATION = 1.5f;

	private const float WAIT_TIME_FADE_BLACK_RAFT = 1f;

	private const float WAIT_TIME_TEXT_START = 0.2f;

	private const float TEXT_ANIM_DURATION = 2f;

	private const float WAIT_TIME_FINISH_CUTSCENE = 2f;

	private const float WAIT_TIME_START_CUTSCENE = 0.5f;

	private const string BEACH_AMB = "ejection_beach_sfx";

	private const string FIRE_SFX = "ejection_fire_sfx";

	private const float VIBRATION_INTENSITY = 0.2f;

	[SerializeField]
	private SpriteRenderer raftSprite;

	[SerializeField]
	private GameObject raftAnimation;

	[SerializeField]
	private GameObject fire;

	[SerializeField]
	private AudioClip ambience;

	[SerializeField]
	private AudioClip fireSound;

	protected override IEnumerator Animate()
	{
		SoundManager.Instance.PlayNamedSound("ejection_beach_sfx", ambience, loop: true, SoundManager.Instance.SfxChannel);
		if (initData == null || initData.outfit == null)
		{
			Player.gameObject.SetActive(value: false);
			raftAnimation.gameObject.SetActive(value: false);
		}
		if (initData != null && initData.outfit != null && (bool)EjectSound)
		{
			SoundManager.Instance.PlaySound(EjectSound, loop: false, 1f, SoundManager.Instance.SfxChannel);
		}
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.black, Color.clear);
		}
		yield return Effects.Wait(0.5f);
		yield return Effects.All(FadeBlackRaftAndPlayer(), HandleText(0.2f, 2f));
		if (initData.confirmImpostor)
		{
			ImpostorText.gameObject.SetActive(value: true);
		}
		yield return Effects.Bloop(0f, ImpostorText.transform);
		yield return new WaitForSeconds(2f);
		if (DestroyableSingleton<HudManager>.InstanceExists)
		{
			yield return DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.clear, Color.black);
		}
		else
		{
			yield return Effects.Wait(0.2f);
		}
		SoundManager.Instance.StopNamedSound("ejection_beach_sfx");
		SoundManager.Instance.StopNamedSound("ejection_fire_sfx");
		WrapUp();
	}

	private IEnumerator FadeBlackRaftAndPlayer()
	{
		yield return Effects.Wait(1f);
		if (initData != null && initData.outfit != null)
		{
			Player.FadeBlackAll(1.5f);
			yield return Effects.ColorFade(raftSprite, Color.white, Color.black, 1.5f);
			fire.gameObject.SetActive(value: true);
			SoundManager.Instance.PlayNamedSound("ejection_fire_sfx", fireSound, loop: true, SoundManager.Instance.SfxChannel);
			VibrationManager.Vibrate(0.2f, 0.2f, 2.4f, VibrationManager.VibrationFalloff.Linear);
		}
	}
}
