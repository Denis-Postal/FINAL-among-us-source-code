using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AmongUs.Data;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using Assets.CoreScripts;
using Cpp2ILInjected;
using Epic.OnlineServices;
using Epic.OnlineServices.Achievements;
using Epic.OnlineServices.Stats;
using Steamworks;
using UnityEngine;

public class AchievementManager : DestroyableSingleton<AchievementManager>
{
	private struct AchievementProgressValue
	{
		public string name;

		public int maxProgress;

		public AchievementProgressValue(string achievementName, int achievementMaxProgress)
		{
			name = achievementName;
			maxProgress = achievementMaxProgress;
		}
	}

	private sealed class _003C_003Ec__DisplayClass71_0
	{
		public AchievementManager _003C_003E4__this;

		public Action callback;

		internal void _003CQueryStats_003Eb__0(ref OnQueryStatsCompleteCallbackInfo data)
		{
			//IL_000e: Expected I4, but got Obj
			AchievementManager achievementManager = _003C_003E4__this;
			object obj = default;
			object arg = (Result)obj;
			string message = $"Stats queried {arg}";
			achievementManager.logger.Info(message);
			if (callback != null)
			{
				Action action = callback;
				Cpp2ILHelpers.NoteDecompilerIssue("Indirect call: v122.invoke_impl (System.IntPtr) (should have been resolved before IL gen)");
			}
		}
	}

	private sealed class _003C_003Ec__DisplayClass86_0
	{
		public string achievementKey;

		public AchievementManager _003C_003E4__this;

		public ProductUserId userId;

		internal unsafe void _003CQueryAndLogAchievementProgress_003Eb__0(ref OnQueryPlayerAchievementsCompleteCallbackInfo data)
		{
			//IL_00c0: Expected I4, but got Obj
			object obj = default;
			object arg = (Result)obj;
			string message = $"Query Achievement Results {arg}";
			Debug.Log(message);
			if (achievementKey != null)
			{
				if ((object)_003C_003E4__this != null)
				{
					_003C_003E4__this.CopyAndLogPlayerAchievementProgress(achievementKey, userId);
					return;
				}
			}
			else if (AchievementGameModeKey != null)
			{
				Cpp2ILHelpers.NoteDecompilerIssue("Method not found @181116D00");
				Dictionary<object, object>.Enumerator enumerator = default;
				string key = default;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						if ((object)_003C_003E4__this == null)
						{
							break;
						}
						_003C_003E4__this.CopyAndLogPlayerAchievementProgress(key, userId);
						continue;
					}
					((Dictionary<string, List<GameModes>>.Enumerator*)(&enumerator))->Dispose();
					return;
				}
				throw new NullReferenceException();
			}
			throw new NullReferenceException();
		}
	}

	private sealed class _003C_003Ec__DisplayClass91_0
	{
		public AchievementManager _003C_003E4__this;

		public DefinitionV2? playerAchievement;

		public string key;

		internal void _003CUnlockAchievement_003Eb__0(ref OnUnlockAchievementsCompleteCallbackInfo data)
		{
			//IL_000e: Expected I4, but got Obj
			//IL_002c: Expected Obj, but got I
			AchievementManager achievementManager = _003C_003E4__this;
			object obj = default;
			object arg = (Result)obj;
			Cpp2ILHelpers.NoteDecompilerIssue("Unmanaged memory load: [this @ rcx (AchievementManager+<>c__DisplayClass91_0)+20]");
			string message = $"Unlocked Achievement {0} Result {arg}";
			achievementManager.logger.Info(message);
			_003C_003E4__this.UnlockAchievementImpl(key);
		}
	}

	private const string TasksCompleteEasyKey = "task_complete_easy";

	private const int TasksCompleteEasy = 10;

	private const string TasksCompleteMediumKey = "task_complete_medium";

	private const int TasksCompleteMedium = 100;

	private const string TasksCompleteHardKey = "task_complete_hard";

	private const int TasksCompleteHard = 500;

	private const string CardSwipeFirstTryKey = "card_first_try";

	private const string KillDuringLightsKey = "kill_during_lights";

	private const string NoVentsImpostorWinKey = "no_vents_impostor_win";

	private const string FirstKillKey = "kills_first";

	private const int KillsFirst = 1;

	private const string KillsEasyKey = "kills_easy";

	private const int KillsEasy = 5;

	private const string KillsMediumKey = "kills_medium";

	private const int KillsMedium = 50;

	private const string KillsHardKey = "kills_hard";

	private const int KillsHard = 150;

	private const int MapWins = 3;

	private const string SkeldWinsKey = "wins_skeld";

	private const string MiraWinsKey = "wins_mira";

	private const string PolusWinsKey = "wins_polus";

	private const string AirshipWinsKey = "wins_airship";

	private const string TwoImpsSurviveKey = "survive_two_impostors";

	private const string TasksWinKey = "win_all_tasks";

	private const string SabotageWinKey = "win_sabotage";

	private const string ImpostorKillWinKey = "win_kills";

	private const string ImpostorVoteWinKey = "win_impostor_vote";

	private const string CrewmateSurviveWinKey = "survive_crewmate";

	private const string DieDuringMedScanKey = "die_during_medscan";

	private const string FixOwnSabotageKey = "fix_own_sabotage";

	private const string ThreeKillsBeforeMeetingKey = "three_kills_before_meeting";

	private const string WinAlwaysCorrectVotesKey = "win_always_correct_votes";

	private const string KillWhileDisguisedKey = "kill_while_shifted";

	private const string BlockKillAsGuardianAngelKey = "block_kill_as_guardian";

	private const string StatNameImpostorKills = "impostorKills";

	private const string StatNameTasksCompleted = "tasksCompleted";

	private const string StatNameMapWinsSkeld = "MapWinsSkeld";

	private const string StatNameMapWinsPolus = "MapWinsPolus";

	private const string StatNameMapWinsMira = "MapWinsMira";

	private const string StatNameMapWinsAirship = "MapWinsAirship";

	private const string HnSCrewmateSurviveWinKey = "hns_survive_crewmate";

	private const string HnSImpostorKillAllKey = "hns_impostor_kill_all";

	private const string HnSCompleteAllTasksKey = "hns_complete_all_tasks";

	private const string HnSCrewmateWinsEasyKey = "hns_crewmate_wins_easy";

	private const int HnSCrewmateWinsEasy = 10;

	private const string HnSCrewmateWinsHardKey = "hns_crewmate_wins_hard";

	private const int HnSCrewmateWinsHard = 50;

	private const string HnSImpostorKillsEasyKey = "hns_impostor_kills_easy";

	private const int HnSImpostorKillsEasy = 100;

	private const string HnSImpostorKillsHardKey = "hns_impostor_kills_hard";

	private const int HnSImpostorKillsHard = 200;

	private const string StatNameHnSCrewmateWin = "hnsCrewmateWin";

	private const string StatNameHnSImpostorKill = "hnsImpostorKills";

	public static readonly Dictionary<string, List<GameModes>> AchievementGameModeKey = new Dictionary<string, List<GameModes>>
	{
		{
			"kills_first",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"kills_easy",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"kills_medium",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"kills_hard",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"task_complete_easy",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"task_complete_medium",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"task_complete_hard",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"wins_skeld",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"wins_mira",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"wins_polus",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"wins_airship",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"survive_two_impostors",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"win_all_tasks",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"win_sabotage",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"win_kills",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"win_impostor_vote",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"survive_crewmate",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"card_first_try",
			new List<GameModes>
			{
				GameModes.Normal,
				GameModes.HideNSeek
			}
		},
		{
			"kill_during_lights",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"die_during_medscan",
			new List<GameModes>
			{
				GameModes.Normal,
				GameModes.HideNSeek
			}
		},
		{
			"fix_own_sabotage",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"three_kills_before_meeting",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"win_always_correct_votes",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"no_vents_impostor_win",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"kill_while_shifted",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"block_kill_as_guardian",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"hns_survive_crewmate",
			new List<GameModes> { GameModes.HideNSeek }
		},
		{
			"hns_impostor_kill_all",
			new List<GameModes> { GameModes.HideNSeek }
		},
		{
			"hns_complete_all_tasks",
			new List<GameModes> { GameModes.HideNSeek }
		},
		{
			"hns_crewmate_wins_easy",
			new List<GameModes> { GameModes.HideNSeek }
		},
		{
			"hns_crewmate_wins_hard",
			new List<GameModes> { GameModes.HideNSeek }
		},
		{
			"hns_impostor_kills_easy",
			new List<GameModes> { GameModes.HideNSeek }
		},
		{
			"hns_impostor_kills_hard",
			new List<GameModes> { GameModes.HideNSeek }
		}
	};

	public static readonly Dictionary<string, List<GameModes>> StatsGameModeKey = new Dictionary<string, List<GameModes>>
	{
		{
			"impostorKills",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"tasksCompleted",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"MapWinsSkeld",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"MapWinsPolus",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"MapWinsMira",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"MapWinsAirship",
			new List<GameModes> { GameModes.Normal }
		},
		{
			"hnsCrewmateWin",
			new List<GameModes> { GameModes.HideNSeek }
		},
		{
			"hnsImpostorKills",
			new List<GameModes> { GameModes.HideNSeek }
		}
	};

	private readonly Logger logger = new Logger(Logger.Category.Gameplay, new string[1] { "AchievementManager" });

	public bool gameStarted;

	private RoleTypes myRole;

	public int cardSwipesThisMatch;

	private int ventsUsedThisMatch;

	private bool meetingCalledThisMatch;

	private int killsThisMatch;

	private bool sabotageCalledLocally;

	private int timesVoted;

	private int timesVotedForImp;

	public int winMap = -1;

	private Dictionary<string, List<AchievementProgressValue>> statName2AchievementProgressValues;

	public override void Awake()
	{
		base.Awake();
	}

	private unsafe void InitializeAchievementProgressDictionary()
	{ // Local/offline: platform achievements and telemetry are disabled.
statName2AchievementProgressValues = new Dictionary<string, List<AchievementProgressValue>>();
}

	public void UpdateAchievementsAndStats()
	{ // Local/offline: platform achievements and telemetry are disabled.
}

	public void QueryStats(Action callback)
	{ // Local/offline: platform achievements and telemetry are disabled.
callback?.Invoke();
}

	public void OnMatchStart(RoleTypes myRole)
	{
		logger.Info("Match started");
		gameStarted = true;
		this.myRole = myRole;
		cardSwipesThisMatch = 0;
		ventsUsedThisMatch = 0;
		meetingCalledThisMatch = false;
		killsThisMatch = 0;
		sabotageCalledLocally = false;
		timesVoted = 0;
		timesVotedForImp = 0;
		winMap = -1;
	}

	public void SetWinMap(int mapIndex)
	{
		winMap = mapIndex;
	}

	public void OnMatchExited()
	{
		logger.Info("Match exited");
		gameStarted = false;
	}

	public void OnMatchEnd(GameOverReason reason, bool didWin)
	{
		if (!gameStarted)
		{
			return;
		}
		logger.Info("Match ended");
		gameStarted = false;
		bool flag = EndGameResult.CachedLocalPlayer?.IsImpostor ?? false;
		if (flag)
		{
			if (ventsUsedThisMatch == 0 && GameManager.Instance.DidImpostorsWin(reason))
			{
				UnlockAchievement("no_vents_impostor_win");
			}
			IEnumerable<NetworkedPlayerInfo> source = GameData.Instance.AllPlayers.Where((NetworkedPlayerInfo p) => p.Role.IsImpostor);
			int num = source.Count();
			if ((num == 2 && num == source.Count((NetworkedPlayerInfo p) => !p.IsDead && !p.Disconnected)) & didWin)
			{
				UnlockAchievement("survive_two_impostors");
			}
			if (reason == GameOverReason.ImpostorsByKill)
			{
				UnlockAchievement("win_kills");
			}
			if (reason == GameOverReason.ImpostorsBySabotage)
			{
				UnlockAchievement("win_sabotage");
			}
			if (reason == GameOverReason.ImpostorsByVote)
			{
				UnlockAchievement("win_impostor_vote");
			}
			if (reason == GameOverReason.HideAndSeek_ImpostorsByKills)
			{
				UnlockAchievement("hns_impostor_kill_all");
			}
		}
		else
		{
			if (GameManager.Instance.DidHumansWin(reason))
			{
				UpdateAchievementProgress("hnsCrewmateWin", 1);
				CachedPlayerData cachedLocalPlayer = EndGameResult.CachedLocalPlayer;
				if (cachedLocalPlayer != null && !cachedLocalPlayer.IsDead)
				{
					UnlockAchievement("survive_crewmate");
					UnlockAchievement("hns_survive_crewmate");
				}
			}
			if (reason == GameOverReason.CrewmatesByTask)
			{
				UnlockAchievement("win_all_tasks");
			}
		}
		switch (winMap)
		{
		case 0:
			UpdateAchievementProgress("MapWinsSkeld", 1);
			break;
		case 1:
			UpdateAchievementProgress("MapWinsMira", 1);
			break;
		case 2:
			UpdateAchievementProgress("MapWinsPolus", 1);
			break;
		case 4:
			UpdateAchievementProgress("MapWinsAirship", 1);
			break;
		}
		if (didWin && !flag && timesVoted > 0 && timesVotedForImp == timesVoted)
		{
			UnlockAchievement("win_always_correct_votes");
		}
	}

	public void OnConsoleUse(IUsable console)
	{
		if (gameStarted && console is Vent)
		{
			ventsUsedThisMatch++;
		}
	}

	public void SabotageCalledLocally()
	{
		sabotageCalledLocally = true;
	}

	public void OnSabotageCleared()
	{
		if (gameStarted)
		{
			sabotageCalledLocally = false;
		}
	}

	public void OnSabotageFixed(bool didContribute)
	{
		if (gameStarted && (sabotageCalledLocally & didContribute))
		{
			UnlockAchievement("fix_own_sabotage");
		}
	}

	public void OnMurder(bool amKiller, bool amVictim, bool isShapeShifted, int shapeshiftTargetId, int victimId)
	{
		if (!gameStarted)
		{
			return;
		}
		if (amVictim)
		{
			if (IsTaskOpen(TaskTypes.SubmitScan))
			{
				UnlockAchievement("die_during_medscan");
			}
		}
		else if (amKiller)
		{
			killsThisMatch++;
			if (TryGetSystemOfType<SwitchSystem>(SystemTypes.Electrical, out var output) && output.IsActive)
			{
				UnlockAchievement("kill_during_lights");
			}
			UpdateAchievementProgress("impostorKills", 1);
			UpdateAchievementProgress("hnsImpostorKills", 1);
			if (isShapeShifted && shapeshiftTargetId == victimId)
			{
				UnlockAchievement("kill_while_shifted");
			}
			if (killsThisMatch >= 3 && !meetingCalledThisMatch)
			{
				UnlockAchievement("three_kills_before_meeting");
			}
		}
	}

	public void OnMeetingCalled()
	{
		if (gameStarted)
		{
			meetingCalledThisMatch = true;
		}
	}

	public void OnProtectACrewmate()
	{
		UnlockAchievement("block_kill_as_guardian");
	}

	public void OnMeetingVote(NetworkedPlayerInfo self, NetworkedPlayerInfo target)
	{
		if (gameStarted && !(target == null))
		{
			timesVoted++;
			if (target.Role.IsImpostor)
			{
				timesVotedForImp++;
			}
		}
	}

	public void OnTaskComplete(TaskTypes taskType)
	{
		if (!gameStarted)
		{
			return;
		}
		UpdateAchievementProgress("tasksCompleted", 1);
		if (taskType == TaskTypes.SwipeCard)
		{
			DestroyableSingleton<UnityTelemetry>.Instance.CardSwipeComplete(cardSwipesThisMatch);
			if (cardSwipesThisMatch == 0)
			{
				UnlockAchievement("card_first_try");
			}
		}
		else
		{
			DestroyableSingleton<UnityTelemetry>.Instance.WriteCompleteTask(taskType);
		}
		if (PlayerControl.LocalPlayer.myTasks.All((PlayerTask t) => t.IsComplete))
		{
			UnlockAchievement("hns_complete_all_tasks");
		}
	}

	public void OnTaskFailure(TaskTypes taskType)
	{
		if (gameStarted && taskType == TaskTypes.SwipeCard)
		{
			cardSwipesThisMatch++;
		}
	}

	public unsafe void QueryAndLogAchievementProgress(string achievementKey = null)
	{ // Local/offline: platform achievements and telemetry are disabled.
}

	private bool IsTaskOpen(TaskTypes taskType)
	{
		if ((bool)Minigame.Instance)
		{
			return Minigame.Instance.TaskType == taskType;
		}
		return false;
	}

	private unsafe bool TryGetSystemOfType<T>(SystemTypes sysType, out T output) where T : class, ISystemType
	{
		output = null;
		if (!ShipStatus.Instance)
		{
			return false;
		}
		if (ShipStatus.Instance.Systems.TryGetValue(sysType, out var value))
		{
			output = value as T;
			return output != null;
		}
		return false;
	}

	private unsafe void CopyAndLogPlayerAchievementProgress(string key, ProductUserId userId)
	{ // Local/offline: platform achievements and telemetry are disabled.
}

	private unsafe void UpdateAchievementProgress(string statName, int progress)
	{ // Local/offline: platform achievements and telemetry are disabled.
}

	private unsafe void UnlockAchievement(string key)
	{ // Local/offline: platform achievements and telemetry are disabled.
}

	public void UpdateAchievementProgressImpl(string key, int progress, int total)
	{ // Local/offline: platform achievements and telemetry are disabled.
}

	public void UnlockAchievementImpl(string key)
	{ // Local/offline: platform achievements and telemetry are disabled.
}

	public AchievementManager()
	{
		//IL_004e: Expected I4, but got I8
	}

	static AchievementManager()
	{ }

	private void _003CUpdateAchievementsAndStats_003Eb__70_0(ref OnQueryDefinitionsCompleteCallbackInfo data)
	{
		//IL_002a: Expected I4, but got Obj
		object obj = default;
		object arg = (Result)obj;
		string message = $"Achievements def queried {arg}";
		logger.Info(message);
		InitializeAchievementProgressDictionary();
	}
}
