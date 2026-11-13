using System;
using System.Text;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using UnityEngine;

public class ViperRole : ImpostorRole
{
	[SerializeField]
	private Sprite killSprite;

	private float dissolveMaxTime => GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ViperDissolveTime);

	public override void Initialize(PlayerControl player)
	{
		base.Initialize(player);
		if (player.AmOwner)
		{
			DestroyableSingleton<HudManager>.Instance.KillButton.ChangeButtonText(StringNames.ViperAbility);
			DestroyableSingleton<HudManager>.Instance.KillButton.ChangeGraphic(killSprite);
		}
	}

	public override void KillAnimSpecialSetup(DeadBody deadBody, PlayerControl killer, PlayerControl victim)
	{
		if (killer == Player)
		{
			ViperDeadBody viperDeadBody = deadBody as ViperDeadBody;
			if (viperDeadBody != null)
			{
				viperDeadBody.SetupViperInfo(dissolveMaxTime, Player, victim);
			}
		}
	}

	public override void AppendTaskHint(StringBuilder taskStringBuilder)
	{
		RoleBehaviour.AppendTaskHint(this, taskStringBuilder);
	}
}
