using System;
using Cpp2ILInjected;
using PowerTools;
using UnityEngine;

public class ReactorShipRoom : SkeldShipRoom, IReactorShipRoom
{
	public Sprite normalManifolds;

	public Sprite meltdownManifolds;

	public SpriteRenderer Manifolds;

	public AnimationClip normalReactor;

	public AnimationClip meltdownReactor;

	public SpriteAnim Reactor;

	public AnimationClip normalHighFloor;

	public AnimationClip meltdownHighFloor;

	public SpriteAnim HighFloor;

	public AnimationClip normalMidFloor;

	public AnimationClip meltdownMidFloor;

	public SpriteAnim MidFloor1;

	public SpriteAnim MidFloor2;

	public AnimationClip normalLowFloor;

	public AnimationClip meltdownLowFloor;

	public SpriteAnim LowFloor;

	public AnimationClip[] normalPipes;

	public AnimationClip[] meltdownPipes;

	public SpriteAnim[] Pipes;

	public SupressorBehaviour[] Supressors;

	public MeshRenderer Orb;

	public Rotater OrbGlass;

	public ChainBehaviour[] Arms;

	public unsafe void StartMeltdown()
	{
		if ((bool)Manifolds)
		{
			Manifolds.sprite = meltdownManifolds;
		}
		if ((bool)Reactor)
		{
			Reactor.Play(meltdownReactor);
		}
		if ((bool)HighFloor)
		{
			HighFloor.Play(meltdownHighFloor);
		}
		if ((bool)MidFloor1)
		{
			MidFloor1.Play(meltdownMidFloor);
		}
		if ((bool)MidFloor2)
		{
			MidFloor2.Play(meltdownMidFloor);
		}
		if ((bool)LowFloor)
		{
			LowFloor.Play(meltdownLowFloor);
		}
		for (int i = 0; i < Pipes.Length; i++)
		{
			Pipes[i].Play(meltdownPipes[i]);
		}
		for (int j = 0; j < Supressors.Length; j++)
		{
			Supressors[j].Deactivate();
		}
		if ((bool)Orb)
		{
			Orb.material.SetColor("_Color", Color.red);
			Orb.material.SetFloat("_Speed", 3f);
		}
		if ((bool)OrbGlass)
		{
			OrbGlass.DegreesPerSecond = 1440f;
		}
		for (int k = 0; k < Arms.Length; k++)
		{
			Arms[k].SwingRange.min = -0.75f;
			Arms[k].SwingRange.max = 0.75f;
		}
	}

	public unsafe void StopMeltdown()
	{
		if ((bool)Manifolds)
		{
			Manifolds.sprite = normalManifolds;
		}
		if ((bool)Reactor)
		{
			Reactor.Play(normalReactor);
		}
		if ((bool)HighFloor)
		{
			HighFloor.Play(normalHighFloor);
		}
		if ((bool)MidFloor1)
		{
			MidFloor1.Play(normalMidFloor);
		}
		if ((bool)MidFloor2)
		{
			MidFloor2.Play(normalMidFloor);
		}
		if ((bool)LowFloor)
		{
			LowFloor.Play(normalLowFloor);
		}
		for (int i = 0; i < Pipes.Length; i++)
		{
			Pipes[i].Play(normalPipes[i]);
		}
		for (int j = 0; j < Supressors.Length; j++)
		{
			Supressors[j].Activate();
		}
		if ((bool)Orb)
		{
			Orb.material.SetColor("_Color", Color.white);
			Orb.material.SetFloat("_Speed", 1f);
		}
		if ((bool)OrbGlass)
		{
			OrbGlass.DegreesPerSecond = 720f;
		}
		for (int k = 0; k < Arms.Length; k++)
		{
			Arms[k].SwingRange.min = -0.15f;
			Arms[k].SwingRange.max = 0.15f;
		}
	}
}
