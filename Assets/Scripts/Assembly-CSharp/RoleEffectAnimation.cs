using System;
using System.Collections;
using PowerTools;
using UnityEngine;

public class RoleEffectAnimation : MonoBehaviour
{
	public enum SoundType
	{
		None,
		Local,
		Global
	}

	public enum EffectType
	{
		Default,
		ProtectLoop,
		Shapeshift,
		Vanish_Charge,
		Vanish_Poof,
		Appear_Poof
	}

	public EffectType effectType;

	public AnimationClip Clip;

	public SpriteAnim Animator;

	public Action MidAnimCB;

	public SpriteRenderer Renderer;

	public AudioClip UseSound;

	public AudioSource AudioSource;

	private PlayerControl parent;

	public void Play(PlayerControl parent, Action onClipEnded, bool flipX, SoundType soundType, float duration = 0f, bool parentTransform = true, float flipXOffset = 0f)
	{
		StartCoroutine(CoPlay(parent, onClipEnded, flipX, soundType, duration, parentTransform, flipXOffset));
	}

	private IEnumerator CoPlay(PlayerControl parent, Action onClipEnded, bool flipX, SoundType soundType, float duration = 0f, bool parentTransform = true, float flipXOffset = 0f)
	{
		this.parent = parent;
		if (!parentTransform)
		{
			transform.parent = null;
		}
		parent.currentRoleAnimations.Add(this);
		Renderer.flipX = flipX;
		if (flipX)
		{
			transform.position += new Vector3(flipXOffset, 0f, 0f);
		}
		if ((bool)UseSound)
		{
			switch (soundType)
			{
			case SoundType.Global:
				SoundManager.Instance.PlaySound(UseSound, loop: false);
				break;
			case SoundType.Local:
				PlaySound();
				break;
			}
		}
		if (duration > 0f)
		{
			yield return new WaitForSeconds(duration);
		}
		else
		{
			yield return new WaitForAnimationFinish(Animator, Clip);
		}
		if (MeetingHud.Instance == null)
		{
			onClipEnded?.Invoke();
		}
		parent.currentRoleAnimations.Remove(this);
		UnityEngine.Object.Destroy(gameObject);
	}

	public void MidAnimationCallback()
	{
		MidAnimCB();
	}

	public void SetMaskLayerBasedOnWhoShouldSee(bool shouldBeVisible)
	{
		PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(Renderer, shouldBeVisible);
	}

	public void SetMaterialColor(int colorId)
	{
		if (colorId >= 0 && colorId < Palette.PlayerColors.Length)
		{
			Renderer.material.SetColor("_BackColor", Palette.ShadowColors[colorId]);
			Renderer.material.SetColor("_BodyColor", Palette.PlayerColors[colorId]);
			Renderer.material.SetColor("_VisorColor", Palette.VisorColor);
		}
	}

	public void PlaySound()
	{
		if (Constants.ShouldPlaySfx() && !(PlayerControl.LocalPlayer != parent))
		{
			AudioSource.clip = UseSound;
			AudioSource.Play();
		}
	}

	public void ToggleRenderer(bool on)
	{
		Renderer.enabled = on;
	}
}
