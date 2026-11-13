using System;
using System.Collections;
using System.Collections.Generic;
using PowerTools;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	[SerializeField]
	private SpriteAnim glowAnimator;

	[SerializeField]
	private SpriteRenderer glowSpriteRenderer;

	[SerializeField]
	private List<PlayerAnimationGroup> animationGroups;

	[Header("Med Scan")]
	[SerializeField]
	private SpriteAnim[] scannerAnims;

	[SerializeField]
	private SpriteRenderer[] scannersImages;

	private PlayerAnimationGroup group;

	private PlayerBodyTypes type;

	private bool loggedRunDiagnostics;

	public float Time => Animator.Time;

	public float ShapeshiftScale => group.ShapeshiftScale;

	public Vector3 DefaultPlayerScale => group.defaultPlayerScale;

	private SpriteAnim Animator
	{
		get
		{
			if (group == null)
			{
				throw new NullReferenceException("PlayerAnimations have not been initialized properly before attempting usage.");
			}
			return group.SpriteAnimator;
		}
	}

	public void SetBodyType(PlayerBodyTypes bodyType, Vector3 flippedCosmeticOffset, Vector3 normalCosmeticOffset)
	{
		type = bodyType;
		loggedRunDiagnostics = false;
		foreach (PlayerAnimationGroup animationGroup in animationGroups)
		{
			if (animationGroup.BodyType == type)
			{
				group = animationGroup;
				group.SpriteAnimator.Initialize();
				animationGroup.NodeSyncs.ForEach((SpriteAnimNodeSync n) =>
				{
					n.enabled = true;
					n.flipOffset = flippedCosmeticOffset;
					n.normalOffset = normalCosmeticOffset;
				});
			}
			else
			{
				animationGroup.NodeSyncs.ForEach((SpriteAnimNodeSync n) =>
				{
					n.enabled = false;
				});
			}
		}
		if (group == null)
		{
			throw new NullReferenceException("Somehow, the animation group was still null after setting body type.");
		}
	}

	public void UpdateCosmeticOffset(Vector3 flippedCosmeticOffset, Vector3 normalCosmeticOffset)
	{
		group?.NodeSyncs.ForEach((SpriteAnimNodeSync n) =>
		{
			n.enabled = true;
			n.flipOffset = flippedCosmeticOffset;
			n.normalOffset = normalCosmeticOffset;
		});
	}

	public OverlayKillAnimation[] GetKillAnimations()
	{
		return group.KillAnims;
	}

	public bool IsPlayingRunAnimation()
	{
		return Animator.GetCurrentAnimation() == group.RunAnim;
	}

	public bool IsPlayingSpawnAnimation()
	{
		return Animator.GetCurrentAnimation() == group.SpawnAnim;
	}

	public bool IsPlayingClimbAnimation()
	{
		if (!(Animator.GetCurrentAnimation() == group.ClimbUpAnim))
		{
			return Animator.GetCurrentAnimation() == group.ClimbDownAnim;
		}
		return true;
	}

	public bool IsPlayingGuardianAngelIdleAnimation()
	{
		return Animator.GetCurrentAnimation() == group.GhostGuardianAngelAnim;
	}

	public bool IsPlayingGhostIdleAnimation()
	{
		return Animator.GetCurrentAnimation() == group.GhostIdleAnim;
	}

	public bool IsPlayingEnterVentAnimation()
	{
		return Animator.GetCurrentAnimation() == group.EnterVentAnim;
	}

	public bool IsPlayingAnyLadderAnimation()
	{
		if (!(Animator.GetCurrentAnimation() == group.ClimbDownAnim))
		{
			return Animator.GetCurrentAnimation() == group.ClimbUpAnim;
		}
		return true;
	}

	public bool IsPlayingSomeAnimation()
	{
		return Animator.GetCurrentAnimation() != null;
	}

	public void PlayRunAnimation()
	{
		Animator.Play(group.RunAnim);
		if (group.RunAnimTime > 0f)
		{
			Animator.Time = group.RunAnimTime;
		}
		if (!loggedRunDiagnostics)
		{
			UnityEngine.Animator unityAnimator = Animator.m_animator;
			string controllerName = unityAnimator != null && unityAnimator.runtimeAnimatorController != null
				? unityAnimator.runtimeAnimatorController.name
				: "<null>";
			string currentAnimation = Animator.GetCurrentAnimation() != null
				? Animator.GetCurrentAnimation().name
				: "<null>";
			string spriteName = unityAnimator != null && unityAnimator.GetComponent<SpriteRenderer>() != null && unityAnimator.GetComponent<SpriteRenderer>().sprite != null
				? unityAnimator.GetComponent<SpriteRenderer>().sprite.name
				: "<null>";
			Logger.GlobalInstance.Error($"[PlayerRenderDiag] Run requested bodyType={type}, clip={(group.RunAnim != null ? group.RunAnim.name : "<null>")}, current={currentAnimation}, controller={controllerName}, animatorEnabled={(unityAnimator != null && unityAnimator.enabled)}, speed={Animator.Speed}, time={Animator.Time:F3}, sprite={spriteName}", this);
			loggedRunDiagnostics = true;
		}
	}

	public void PlayIdleAnimation()
	{
		Animator.Play(group.IdleAnim);
	}

	public void PlayClimbAnimation(bool down)
	{
		Animator.Play(down ? group.ClimbDownAnim : group.ClimbUpAnim);
		Animator.Time = 0f;
	}

	public void PlayGuardianAngelIdleAnimation()
	{
		Animator.Play(group.GhostGuardianAngelAnim);
	}

	public void PlayGhostIdleAnimation()
	{
		Animator.Play(group.GhostIdleAnim);
	}

	public IEnumerator CoPlaySpawnAnimation(bool flipX)
	{
		glowSpriteRenderer.flipX = flipX;
		glowAnimator.Play(group.SpawnGlowAnim);
		yield return new WaitForAnimationFinish(Animator, group.SpawnAnim);
	}

	public IEnumerator CoPlayEnterVentAnimation(int numFramesUntilDisappearance)
	{
		Animator.Play(group.EnterVentAnim);
		if (numFramesUntilDisappearance > 0)
		{
			while (Animator.FrameTime < numFramesUntilDisappearance)
			{
				yield return null;
			}
			Animator.Stop();
		}
		else
		{
			while (Animator.IsPlaying())
			{
				yield return null;
			}
		}
	}

	public IEnumerator CoPlayExitVentAnimation()
	{
		Animator.Play(group.ExitVentAnim);
		while (Animator.IsPlaying())
		{
			yield return null;
		}
	}

	public IEnumerator CoPlayJumpAnimation()
	{
		yield return new WaitForAnimationFinish(Animator, group.EnterVentAnim, restartIfAlreadyPlaying: true, 5);
	}

	public IEnumerator CoPlayCustomAnimation(AnimationClip customAnim)
	{
		yield return new WaitForAnimationFinish(Animator, customAnim);
	}

	public void PlayScanner(bool on, bool isDead, bool flipX)
	{
		for (int i = 0; i < scannerAnims.Length; i++)
		{
			SpriteAnim spriteAnim = scannerAnims[i];
			if (on && !isDead)
			{
				spriteAnim.gameObject.SetActive(value: true);
				spriteAnim.Play();
				scannersImages[i].flipX = !flipX;
				continue;
			}
			if (spriteAnim.isActiveAndEnabled)
			{
				spriteAnim.Stop();
			}
			spriteAnim.gameObject.SetActive(value: false);
		}
	}
}
