using UnityEngine;

public class WerewolfKill_VicHelper : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer bloodSplatter;

	[SerializeField]
	private CosmeticsLayer victCosmetics;

	private unsafe void Start()
	{
		bloodSplatter.material.SetColor("_BackColor", Palette.ShadowColors[victCosmetics.ColorId]);
		bloodSplatter.material.SetColor("_BodyColor", Palette.PlayerColors[victCosmetics.ColorId]);
		bloodSplatter.material.SetColor("_VisorColor", Palette.VisorColor);
	}

	private void AnimEvent_TurnOffCosmetics()
	{
		victCosmetics.CurrentSkin.Visible = false;
		victCosmetics.SetHatVisorVisible(isVisible: false);
	}
}
