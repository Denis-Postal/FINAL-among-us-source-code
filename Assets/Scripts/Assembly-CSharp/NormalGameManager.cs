using System;
using Cpp2ILInjected;

public class NormalGameManager : GameManager
{
	public override void InitComponents()
	{
		LogicFlow = AddGameLogic(new LogicGameFlowNormal(this));
		LogicMinigame = AddGameLogic(new LogicMinigame(this));
		LogicRoleSelection = AddGameLogic(new LogicRoleSelectionNormal(this));
		LogicUsables = AddGameLogic(new LogicUsablesBasic(this));
		LogicOptions = AddGameLogic(new LogicOptionsNormal(this));
	}

	public override MapOptions GetMapOptions()
	{
		return new MapOptions
		{
			Mode = ((!PlayerControl.LocalPlayer.Data.Role.IsImpostor || (bool)MeetingHud.Instance) ? MapOptions.Modes.Normal : MapOptions.Modes.Sabotage)
		};
	}

	public override bool IsNormal()
	{
		return true;
	}

	public override PlayerBodyTypes GetBodyType(PlayerControl player)
	
	{
		if (AprilFoolsMode.ShouldHorseAround())
		{
			return PlayerBodyTypes.Horse;
		}
		if (AprilFoolsMode.ShouldLongAround())
		{
			return PlayerBodyTypes.Long;
		}
		if (AprilFoolsMode.ShouldClassicMode())
		{
			return PlayerBodyTypes.Classic;
		}
		return PlayerBodyTypes.Normal;
	}

	public override DeadBody GetDeadBody(RoleBehaviour impostorRole)
	
	{
		if (impostorRole is ViperRole)
		{
			return deadBodyPrefab[1];
		}
		if (AprilFoolsMode.ShouldClassicMode())
		{
			return deadBodyPrefab[2];
		}
		return deadBodyPrefab[0];
	}
}
