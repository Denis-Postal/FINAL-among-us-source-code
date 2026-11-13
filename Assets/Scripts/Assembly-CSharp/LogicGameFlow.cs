using System;
using System.Collections.Generic;
using Cpp2ILInjected;

public abstract class LogicGameFlow : GameLogicComponent
{
	public LogicGameFlow(GameManager manager) : base(manager)
	{
	}

	public abstract void CheckEndCriteria();

	public abstract bool IsGameOverDueToDeath();

	protected (int aliveHumans, int aliveImpostor, int impostorCount) GetPlayerCounts()
	
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < GameData.Instance.PlayerCount; i++)
		{
			NetworkedPlayerInfo networkedPlayerInfo = GameData.Instance.AllPlayers[i];
			if (networkedPlayerInfo == null || networkedPlayerInfo.Disconnected || networkedPlayerInfo.Role == null)
			{
				continue;
			}
			if (networkedPlayerInfo.Role.IsImpostor)
			{
				num3++;
			}
			if (!networkedPlayerInfo.IsDead)
			{
				if (networkedPlayerInfo.Role.IsImpostor)
				{
					num2++;
				}
				else
				{
					num++;
				}
			}
			else if (networkedPlayerInfo.Role is ImpostorGhostRole { WasManuallyPicked: not false })
			{
				num2++;
			}
		}
		return (aliveHumans: num, aliveImpostor: num2, impostorCount: num3);
	}
}
