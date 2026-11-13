using System.Collections.Generic;
using Cpp2ILInjected;
using UnityEngine;

public class KillButton : ActionButton
{
	private PlayerControl currentTarget;

	[SerializeField]
	private TextTranslatorTMP textTranslator;

	[SerializeField]
	private Sprite defaultKillSprite;

	public PlayerControl Target => currentTarget;

	public override void DoClick()
	{
		if (isActiveAndEnabled && (bool)currentTarget && !isCoolingDown && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.CanMove)
		{
			PlayerControl.LocalPlayer.CmdCheckMurder(currentTarget);
			SetTarget(null);
		}
	}

	public void CheckClick(PlayerControl target)
	{
		if ((bool)currentTarget && currentTarget == target)
		{
			DoClick();
		}
		else if ((bool)currentTarget && currentTarget != target && PlayerControl.LocalPlayer.Data.Role.GetPlayersInAbilityRangeSorted(new List<PlayerControl>()).Contains(target))
		{
			SetTarget(target);
			DoClick();
		}
	}

	public void SetTarget(PlayerControl target)
	{
		if ((bool)PlayerControl.LocalPlayer && !(PlayerControl.LocalPlayer.Data == null) && (bool)PlayerControl.LocalPlayer.Data.Role)
		{
			RoleTeamTypes teamType = PlayerControl.LocalPlayer.Data.Role.TeamType;
			if ((bool)currentTarget && currentTarget != target)
			{
				currentTarget.ToggleHighlight(active: false, teamType);
			}
			currentTarget = target;
			if ((bool)currentTarget)
			{
				currentTarget.ToggleHighlight(active: true, teamType);
				SetEnabled();
			}
			else
			{
				SetDisabled();
			}
		}
	}

	private void Start()
	{
		isCoolingDown = true;
		graphic.SetCooldownNormalizedUvs();
		SetTarget(null);
	}

	public void ChangeButtonText(StringNames stringName)
	{
		textTranslator.TargetText = stringName;
		textTranslator.ResetText();
	}

	public void ChangeGraphic(Sprite newSprite)
	{
		graphic.sprite = newSprite;
		graphic.SetCooldownNormalizedUvs();
	}

	public void ResetKillButton()
	{
		ChangeButtonText(StringNames.KillLabel);
		graphic.sprite = defaultKillSprite;
		graphic.SetCooldownNormalizedUvs();
	}
}
