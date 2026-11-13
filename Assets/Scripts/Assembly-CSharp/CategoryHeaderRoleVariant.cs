using TMPro;
using UnityEngine;

public class CategoryHeaderRoleVariant : CategoryHeaderMasked
{
	[SerializeField]
	private SpriteRenderer icon;

	public unsafe override void SetHeader(StringNames name, int maskLayer)
	{
		base.SetHeader(name, maskLayer);
		switch (name)
		{
		case StringNames.CrewmateRolesHeader:
			Background.color = Palette.CrewmateRoleHeaderBlue;
			Title.color = Palette.CrewmateRoleHeaderTextBlue;
			break;
		case StringNames.ImpostorRolesHeader:
			Background.color = Palette.ImpostorRoleHeaderRed;
			Title.color = Palette.ImpostorRoleHeaderTextRed;
			break;
		}
	}

	public unsafe void SetHeader(StringNames roleName, int maskLayer, bool crewmate, Sprite roleIcon = null)
	{
		base.SetHeader(roleName, maskLayer);
		if (crewmate)
		{
			Background.color = Palette.CrewmateRoleHeaderBlue;
			Divider.color = Palette.CrewmateRoleHeaderBlue;
			Title.color = Palette.CrewmateRoleHeaderTextBlue;
			if (roleIcon != null && icon != null)
			{
				icon.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
				icon.sprite = roleIcon;
			}
		}
		else
		{
			Background.color = Palette.ImpostorRoleHeaderRed;
			Divider.color = Palette.ImpostorRoleHeaderRed;
			Title.color = Palette.ImpostorRoleHeaderTextRed;
			if (roleIcon != null && icon != null)
			{
				icon.material.SetInt(PlayerMaterial.MaskLayer, maskLayer);
				icon.sprite = roleIcon;
			}
		}
	}
}
