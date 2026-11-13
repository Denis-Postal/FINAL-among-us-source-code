using UnityEngine;

public class LongModeKillAnimation : OverlayKillAnimation
{
	[SerializeField]
	private SpriteRenderer killerNeck;

	[SerializeField]
	private SpriteRenderer killerHead;

	[SerializeField]
	private HatParent sideHat;

	[SerializeField]
	private VisorLayer sideVisor;

	public override void Initialize(KillOverlayInitData initData)
	{
		base.Initialize(initData);
		PlayerMaterial.SetColors(killerParts.ColorId, killerNeck);
		PlayerMaterial.SetColors(killerParts.ColorId, killerHead);
		sideHat.SetHat(initData.killerOutfit.HatId, killerParts.ColorId);
		sideVisor.SetVisor(initData.killerOutfit.VisorId, killerParts.ColorId);
	}

	public LongModeKillAnimation()
	{ }
}
