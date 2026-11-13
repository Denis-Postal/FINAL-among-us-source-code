using System;
using System.Collections.Generic;
using PowerTools;
using UnityEngine;

[Serializable]
public class PlayerAnimationGroup
{
	public PlayerBodyTypes BodyType;

	public SpriteAnim SpriteAnimator;

	public List<SpriteAnimNodeSync> NodeSyncs;

	public OverlayKillAnimation[] KillAnims;

	public AnimationClip RunAnim;

	public AnimationClip IdleAnim;

	public AnimationClip GhostIdleAnim;

	public AnimationClip EnterVentAnim;

	public AnimationClip ExitVentAnim;

	public AnimationClip SpawnAnim;

	public AnimationClip SpawnGlowAnim;

	public AnimationClip ClimbUpAnim;

	public AnimationClip ClimbDownAnim;

	public AnimationClip GhostGuardianAngelAnim;

	public float RunAnimTime;

	public float ShapeshiftScale;

	public Vector3 defaultPlayerScale= new Vector3(0.5f, 0.5f, 1f);

	public PlayerAnimationGroup()
	{ }
}
