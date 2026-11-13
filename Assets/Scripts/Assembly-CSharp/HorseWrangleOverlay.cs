using System.Collections;
using UnityEngine;

public class HorseWrangleOverlay : OverlayKillAnimation
{
	[SerializeField]
	private SpriteRenderer impostorForeground;

	[SerializeField]
	private SpriteRenderer impostorHand;

	[SerializeField]
	private SpriteRenderer[] victimSprites;

	public override IEnumerator CoShow(KillOverlay parent)
	{
		victimParts.SetHatOnFloor();
		PlayerMaterial.SetColors(killerParts.ColorId, impostorForeground);
		PlayerMaterial.SetColors(killerParts.ColorId, impostorHand);
		victimSprites.ForEach((SpriteRenderer b) =>
		{
			PlayerMaterial.SetColors(victimParts.ColorId, b);
		});
		yield return base.CoShow(parent);
	}
}
