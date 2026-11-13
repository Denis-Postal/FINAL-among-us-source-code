using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Epic.OnlineServices;
using Epic.OnlineServices.Stats;
using UnityEngine;

namespace AmongUs.Data.Player
{
	public class PlayerStatsData
	{
		public static readonly Logger Logger = new Logger(Logger.Category.Data, "PlayerStatsData");

		public uint level;

		public uint xp;

		public uint xpForNextLevel;

		public Dictionary<StatID, uint> statsCache = new Dictionary<StatID, uint>();

		public Dictionary<StatID, uint> statsToIngest = new Dictionary<StatID, uint>();

		public bool isTrackingStats = true;

		public bool IsTrackingStats => isTrackingStats;

		public uint Level
		{
			get
			{
				return level;
			}
			set
			{
				level = value;
				Logger.Info(string.Format("Setting cached [{0}] value to [{1}]", "Level", value));
			}
		}

		public uint Xp
		{
			get
			{
				return xp;
			}
			set
			{
				xp = value;
				Logger.Info(string.Format("Setting cached [{0}] value to [{1}]", "Xp", value));
			}
		}

		public uint XpForNextLevel
		{
			get
			{
				return xpForNextLevel;
			}
			set
			{
				xpForNextLevel = value;
				Logger.Info(string.Format("Setting cached [{0}] value to [{1}]", "XpForNextLevel", value));
			}
		}

		public void InitializeStats()
		{
			// Offline remake: the original loads stat values from the EOS Stats service (AchievementManager.QueryStats -> CacheStats).
			// There is no service here, so the local statsCache is used as is.
			Logger.Info("InitializeStats() :: offline build, EOS stats query skipped");
		}

		public void SetStatTrackingEnabled(bool enabled)
		{
			isTrackingStats = enabled;
			Logger.Warning("SetStatTrackingEnabled() :: Changing IsTrackingStats enabled to: " + enabled.ToString().ToUpper());
		}

		private bool ValidateStat(StatID stat)
		{
			if (!IsTrackingStats)
			{
				Logger.Info(string.Format("{0} is FALSE. Skipping validation for [{1}.{2}].", "IsTrackingStats", "StatID", stat));
				return false;
			}
			if (stat == StatID.Invalid || stat == StatID.None)
			{
				Logger.Error(string.Format("[{0}.{1}] is invalid. Aborting caching.", "StatID", stat));
				return false;
			}
			if (!statsToIngest.ContainsKey(stat))
			{
				statsToIngest.Add(stat, 0u);
			}
			return true;
		}

		public void IncrementStat(StatID stat)
		{
			if (ValidateStat(stat))
			{
				Logger.Info(string.Format("Increasing [{0}.{1}] by 1", "StatID", stat));
				statsToIngest[stat]++;
			}
		}

		public void ResetStat(StatID stat)
		{
			if (ValidateStat(stat) && statsCache.ContainsKey(stat))
			{
				Logger.Info(string.Format("Resetting [{0}.{1}] to 0", "StatID", stat));
				statsToIngest[stat] = 0u;
			}
		}

		public uint GetStat(StatID stat)
		{
			if (!statsCache.ContainsKey(stat))
			{
				return 0u;
			}
			return statsCache[stat];
		}

		public void SetStatAsMinTime(StatID stat, float timeInSeconds)
		{
			if (ValidateStat(stat))
			{
				uint num = (uint)Mathf.FloorToInt(timeInSeconds * 1000f);
				uint stat2 = GetStat(stat);
				if (stat2 == 0 || stat2 > num)
				{
					Logger.Info(string.Format("Setting [{0}.{1}] record minimum time value from [{2}] to [{3}]", "StatID", stat, stat2, num));
					statsToIngest[stat] = num;
				}
			}
		}

		public float GetStatAsMinTime(StatID stat)
		{
			return (float)GetStat(stat) / 1000f;
		}

		public string GetStatAsMinTimeString(StatID stat)
		{
			float statAsMinTime = GetStatAsMinTime(stat);
			if (float.IsNaN(statAsMinTime) || statAsMinTime <= 0f)
			{
				return "---";
			}
			return $"{statAsMinTime:0.000}";
		}

		public uint GetMapStat(MapNames map, MapStat stat)
		{
			return GetStat(StatIDUtilities.GetMapStatID(map, stat));
		}

		public void IncrementMapStat(MapNames map, MapStat stat)
		{
			IncrementStat(StatIDUtilities.GetMapStatID(map, stat));
		}

		private static RoleTypes ValidateRole(RoleTypes role)
		{
			if (role == RoleTypes.CrewmateGhost)
			{
				role = RoleTypes.Crewmate;
			}
			if (role == RoleTypes.ImpostorGhost)
			{
				role = RoleTypes.Impostor;
			}
			return role;
		}

		public uint GetRoleStat(RoleTypes role, RoleStat stat)
		{
			return GetStat(StatIDUtilities.GetRoleStatID(ValidateRole(role), stat));
		}

		public void IncrementRoleStat(RoleTypes role, RoleStat stat)
		{
			IncrementStat(StatIDUtilities.GetRoleStatID(ValidateRole(role), stat));
		}

		public uint GetGameResultStat(GameOverReason reason, GameResultStat stat)
		{
			return GetStat(StatIDUtilities.GetGameResultStatID(reason, stat));
		}

		public void IncrementGameResultStat(GameOverReason reason, GameResultStat stat)
		{
			IncrementStat(StatIDUtilities.GetGameResultStatID(reason, stat));
		}

		public void IncrementWinStats(GameOverReason reason, MapNames map, RoleTypes role)
		{
			IncrementGameResultStat(reason, GameResultStat.Wins);
			IncrementMapStat(map, MapStat.Wins);
			IncrementRoleStat(role, RoleStat.Wins);
		}

		// Offline remake: the original ingests statsToIngest into Epic Online Services (external service); intentionally a local no-op.
		public void SaveStats()
		{
			Logger.Info("SaveStats() :: offline build, EOS stat ingest skipped");
		}

		private void CacheStats(StatID[] stats)
		{
			// Offline remake: copies stat values from the EOS Stats interface into statsCache; no EOS here.
		}
	}
}
