using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.Data;
using AmongUs.GameOptions;
using TMPro;
using UnityEngine;

public class StatsPopup : MonoBehaviour
{
	public TextMeshPro StatsText;

	[Header("Console Controller Navigation")]
	public UiElement BackButton;

	[SerializeField]
	private UiElement[] SelectableButtons;

	private static readonly Dictionary<StatID, StringNames> BaseStatsToShow = new Dictionary<StatID, StringNames>
	{
		{
			StatID.BodiesReported,
			StringNames.StatsBodiesReported
		},
		{
			StatID.EmergenciesCalled,
			StringNames.StatsEmergenciesCalled
		},
		{
			StatID.TasksCompleted,
			StringNames.StatsTasksCompleted
		},
		{
			StatID.AllTasksCompleted,
			StringNames.StatsAllTasksCompleted
		},
		{
			StatID.SabotagesFixed,
			StringNames.StatsSabotagesFixed
		},
		{
			StatID.ImpostorKills,
			StringNames.StatsImpostorKills
		},
		{
			StatID.TimesMurdered,
			StringNames.StatsTimesMurdered
		},
		{
			StatID.TimesEjected,
			StringNames.StatsTimesEjected
		},
		{
			StatID.CrewmateStreak,
			StringNames.StatsCrewmateStreak
		},
		{
			StatID.GamesAsImpostor,
			StringNames.StatsGamesImpostor
		},
		{
			StatID.GamesAsCrewmate,
			StringNames.StatsGamesCrewmate
		},
		{
			StatID.GamesStarted,
			StringNames.StatsGamesStarted
		},
		{
			StatID.GamesFinished,
			StringNames.StatsGamesFinished
		},
		{
			StatID.HideAndSeek_GamesCrewmateSurvived,
			StringNames.StatsHidenSeekGamesCrewmateSurvived
		},
		{
			StatID.HideAndSeek_TimesVented,
			StringNames.StatsHidenSeekTimesVented
		},
		{
			StatID.HideAndSeek_ImpostorKills,
			StringNames.StatsImpostorKills_HideAndSeek
		},
		{
			StatID.TimesPettedPet,
			StringNames.StatsTimesPettedPet
		},
		{
			StatID.GameResult_ImpostorsByVote_Wins,
			StringNames.StatsImpostorVoteWins
		},
		{
			StatID.GameResult_ImpostorsByKill_Wins,
			StringNames.StatsImpostorKillsWins
		},
		{
			StatID.GameResult_ImpostorsBySabotage_Wins,
			StringNames.StatsImpostorSabotageWins
		},
		{
			StatID.GameResult_CrewmatesByVote_Wins,
			StringNames.StatsCrewmateVoteWins
		},
		{
			StatID.GameResult_CrewmatesByTask_Wins,
			StringNames.StatsCrewmateTaskWins
		},
		{
			StatID.GameResult_HideAndSeek_CrewmatesByTimer_Wins,
			StringNames.StatsHideAndSeekCrewmateVictory
		},
		{
			StatID.GameResult_HideAndSeek_ImpostorsByKills_Wins,
			StringNames.StatsHideAndSeekImpostorVictory
		}
	};

	private static readonly Dictionary<StatID, StringNames> RoleSpecificStatsToShow = new Dictionary<StatID, StringNames>
	{
		{
			StatID.Role_Engineer_Vents,
			StringNames.StatsEngineerVents
		},
		{
			StatID.Role_GuardianAngel_CrewmatesProtected,
			StringNames.StatsGuardianAngelCrewmatesProtected
		},
		{
			StatID.Role_Scientist_ChargesGained,
			StringNames.StatsScientistChargesGained
		},
		{
			StatID.Role_Shapeshifter_ShiftedKills,
			StringNames.StatsShapeshifterShiftedKills
		},
		{
			StatID.Role_Viper_BodiesDissolved,
			StringNames.StatsViperBodiesDissolved
		},
		{
			StatID.Role_Detective_CrewmatesQuestioned,
			StringNames.StatsDetectiveQuestioned
		},
		{
			StatID.Role_Detective_CasesFilled,
			StringNames.StatsDetectiveCasesFilled
		},
		{
			StatID.Role_Judge_SuccessfulOverrules,
			StringNames.StatsJudgeSuccessfulOverrules
		},
		{
			StatID.Role_Judge_FailedOverrules,
			StringNames.StatsJudgeFailedOverrules
		},
		{
			StatID.Role_SpiritGuide_SentMessage,
			StringNames.StatsSpiritGuideSentMessage
		}
	};

	private void OnDisable()
	{
		ControllerManager.Instance.CloseOverlayMenu(name);
	}

	private void OnEnable()
	{
		DisplayGameStats();
		ControllerManager.Instance.OpenOverlayMenu(name, BackButton, SelectableButtons[0], (from x in SelectableButtons
			where x.gameObject.activeSelf
			select (x)).ToList());
	}

	private static void AppendStat(StringBuilder str, StringNames statName, object stat, params object[] additionalTranslationParams)
	{
		str.Append("<align=left>" + DestroyableSingleton<TranslationController>.Instance.GetString(statName, additionalTranslationParams).Trim() + "<line-height=0>");
		str.AppendLine();
		str.Append("<align=right>" + stat?.ToString() + "<line-height=1em>");
		str.AppendLine();
	}

	public void DisplayGameStats()
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		foreach (StatID key in BaseStatsToShow.Keys)
		{
			AppendStat(stringBuilder, BaseStatsToShow[key], DataManager.Player.Stats.GetStat(key));
		}
		AppendStat(stringBuilder, StringNames.StatsFastestCrewmateWin_HideAndSeek, DataManager.Player.Stats.GetStatAsMinTimeString(StatID.HideAndSeek_FastestCrewmateWin));
		AppendStat(stringBuilder, StringNames.StatsFastestImpostorWin_HideAndSeek, DataManager.Player.Stats.GetStatAsMinTimeString(StatID.HideAndSeek_FastestImpostorWin));
		StatsText.text = stringBuilder.ToString();
	}

	public void DisplayRoleStats()
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		RoleTypes[] array = Enum.GetValues(typeof(RoleTypes)).Cast<RoleTypes>().ToArray();
		foreach (RoleTypes roleTypes in array)
		{
			if (roleTypes != RoleTypes.ImpostorGhost && roleTypes != RoleTypes.CrewmateGhost)
			{
				AppendStat(stringBuilder, StringNames.StatsRoleWins, DataManager.Player.Stats.GetRoleStat(roleTypes, RoleStat.Wins), DestroyableSingleton<RoleManager>.Instance.GetRole(roleTypes)?.NiceName ?? ((object)roleTypes));
			}
		}
		foreach (StatID key in RoleSpecificStatsToShow.Keys)
		{
			AppendStat(stringBuilder, RoleSpecificStatsToShow[key], DataManager.Player.Stats.GetStat(key));
		}
		StatsText.text = stringBuilder.ToString();
	}
}
