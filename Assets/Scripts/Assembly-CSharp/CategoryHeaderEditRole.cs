using TMPro;
using UnityEngine;

public class CategoryHeaderEditRole : CategoryHeaderMasked
{
	[SerializeField]
	private SpriteRenderer blankLabel;

	[SerializeField]
	private SpriteRenderer chanceLabel;

	[SerializeField]
	private SpriteRenderer countLabel;

	public unsafe override void SetHeader(StringNames name, int maskLayer)
	{
		base.SetHeader(name, maskLayer);
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
		}
		TextMeshPro[] componentsInChildren2 = GetComponentsInChildren<TextMeshPro>(includeInactive: true);
		foreach (TextMeshPro obj in componentsInChildren2)
		{
			obj.fontMaterial.SetFloat("_StencilComp", 3f);
			obj.fontMaterial.SetFloat("_Stencil", maskLayer);
		}
		switch (name)
		{
		case StringNames.CrewmateRolesHeader:
			Title.color = Palette.CrewmateRoleHeaderTextBlue;
			Background.color = Palette.CrewmateRoleHeaderBlue;
			blankLabel.color = Palette.CrewmateRoleHeaderVeryDarkBlue;
			chanceLabel.color = Palette.CrewmateRoleHeaderDarkBlue;
			countLabel.color = Palette.CrewmateRoleHeaderDarkBlue;
			break;
		case StringNames.ImpostorRolesHeader:
			Title.color = Palette.ImpostorRoleHeaderTextRed;
			Background.color = Palette.ImpostorRoleHeaderRed;
			blankLabel.color = Palette.ImpostorRoleHeaderVeryDarkRed;
			chanceLabel.color = Palette.ImpostorRoleHeaderDarkRed;
			countLabel.color = Palette.ImpostorRoleHeaderDarkRed;
			break;
		}
	}
}
