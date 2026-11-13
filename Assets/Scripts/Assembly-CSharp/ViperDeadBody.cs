using Object = UnityEngine.Object;
using AmongUs.Data;
using AmongUs.Data.Player;
using PowerTools;
using UnityEngine;
using UnityEngine.Audio;

public class ViperDeadBody : DeadBody
{
	[SerializeField]
	private AnimationClip[] dissolveAnims;

	[SerializeField]
	private SpriteRenderer acidRenderer;

	[SerializeField]
	private SpriteRenderer splashRenderer;

	[SerializeField]
	private SpriteAnim spriteAnim;

	[SerializeField]
	private AudioClip acidSplashSFX;

	private float dissolveCurrentTime;

	private float maxDissolveTime;

	private bool victimDissolving;

	private int dissolveStage;

	private PlayerControl myKiller;

	private PlayerControl myController;

	public void SetupViperInfo(float maxTime, PlayerControl killer, PlayerControl owner)
	{
		maxDissolveTime = maxTime;
		victimDissolving = true;
		dissolveCurrentTime = maxTime;
		dissolveStage = 0;
		myKiller = killer;
		myController = owner;
		if (PlayerControl.LocalPlayer == killer && Constants.ShouldPlaySfx())
		{
			SoundManager.Instance.PlaySound(acidSplashSFX, loop: false);
		}
		if (splashRenderer.sharedMaterial.shader.name == "Sprites/Default")
		{
			splashRenderer.material = DestroyableSingleton<HatManager>.Instance.DefaultShader;
		}
		if (acidRenderer.sharedMaterial.shader.name == "Sprites/Default")
		{
			acidRenderer.material = DestroyableSingleton<HatManager>.Instance.DefaultShader;
		}
	}

	private void FixedUpdate()
	{
		if (!victimDissolving || !(dissolveCurrentTime > 0f))
		{
			return;
		}
		dissolveCurrentTime -= Time.fixedDeltaTime;
		if (dissolveCurrentTime <= 0f)
		{
			myController.DisableCurrentTrackers();
			victimDissolving = false;
			myCollider.enabled = false;
			spriteAnim.gameObject.SetActive(value: false);
			myController.cosmetics.HidePetViper();
			if (PlayerControl.LocalPlayer == myKiller)
			{
				DataManager.Player.Stats.IncrementStat(StatID.Role_Viper_BodiesDissolved);
			}
			return;
		}
		float num = dissolveCurrentTime / maxDissolveTime;
		if (num <= 0.8f && num > 0.3f)
		{
			if (dissolveStage != 1)
			{
				dissolveStage = 1;
				spriteAnim.Play(dissolveAnims[0]);
			}
		}
		else if (num <= 0.3f && dissolveStage != 2)
		{
			dissolveStage = 2;
			spriteAnim.Play(dissolveAnims[1]);
		}
	}
}
