using System;
using TMPro;
using UnityEngine;

public class ViewSettingsInfoPanelRoleVariant : ViewSettingsInfoPanel
{
	[SerializeField]
	protected SpriteRenderer chanceBackground;

	[SerializeField]
	protected SpriteRenderer iconSprite;

	[SerializeField]
	protected TextMeshPro chanceText;

	[SerializeField]
	protected TextMeshPro chanceTitle;

	[SerializeField]
	protected Sprite disabledCube;

	[SerializeField]
	protected Sprite crewmateCube;

	[SerializeField]
	protected Sprite impostorCube;

	public unsafe void SetInfo(string name, int count, int chance, int maskLayer, Color32 color, Sprite roleIcon, bool crewmateTeam, bool showDisabledBackground = false)
	{
		titleText.text = name;
		settingText.text = count.ToString();
		chanceText.text = chance.ToString();
		iconSprite.sprite = roleIcon;
		if (showDisabledBackground)
		{
			titleText.color = Palette.White_75Alpha;
			chanceTitle.color = Palette.White_75Alpha;
			chanceBackground.sprite = disabledCube;
			background.sprite = disabledCube;
			labelBackground.color = Palette.DisabledGrey;
		}
		else
		{
			labelBackground.color = color;
			if (crewmateTeam)
			{
				chanceBackground.sprite = crewmateCube;
				background.sprite = crewmateCube;
			}
			else
			{
				chanceBackground.sprite = impostorCube;
				background.sprite = impostorCube;
			}
		}
		SetMaskLayer(maskLayer);
	}

	protected override void SetMaskLayer(int maskLayer)
	{
		base.SetMaskLayer(maskLayer);
		chanceBackground.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		iconSprite.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		chanceText.fontMaterial.SetFloat("_StencilComp", 3f);
		chanceText.fontMaterial.SetFloat("_Stencil", maskLayer);
		chanceTitle.fontMaterial.SetFloat("_StencilComp", 3f);
		chanceTitle.fontMaterial.SetFloat("_Stencil", maskLayer);
	}
}
