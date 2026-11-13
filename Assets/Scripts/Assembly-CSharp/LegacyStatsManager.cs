using System;
using System.Collections.Generic;
using System.IO;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using Innersloth.IO;
using UnityEngine;

public class LegacyStatsManager
{
	public class LegacyStats
	{
		public byte version;

		public float FastestCrewmateWin_HideAndSeek = float.NaN;

		public float FastestImpostorWin_HideAndSeek = float.NaN;

		public readonly Dictionary<StringNames, uint> gameplayStats = new Dictionary<StringNames, uint>();

		public readonly Dictionary<GameOverReason, uint> winReasons = new Dictionary<GameOverReason, uint>();

		public readonly Dictionary<GameOverReason, uint> loseReasons = new Dictionary<GameOverReason, uint>();

		public readonly Dictionary<GameOverReason, uint> drawReasons = new Dictionary<GameOverReason, uint>();

		public readonly Dictionary<MapNames, uint> mapWins = new Dictionary<MapNames, uint>();

		public readonly Dictionary<RoleTypes, uint> roleWins = new Dictionary<RoleTypes, uint>();

		private static readonly List<StringNames> SimpleStats = new List<StringNames>
		{
			StringNames.StatsBodiesReported,
			StringNames.StatsEmergenciesCalled,
			StringNames.StatsTasksCompleted,
			StringNames.StatsAllTasksCompleted,
			StringNames.StatsSabotagesFixed,
			StringNames.StatsImpostorKills,
			StringNames.StatsTimesMurdered,
			StringNames.StatsTimesEjected,
			StringNames.StatsCrewmateStreak,
			StringNames.StatsGamesImpostor,
			StringNames.StatsGamesCrewmate,
			StringNames.StatsGamesStarted,
			StringNames.StatsGamesFinished,
			StringNames.StatsEngineerVents,
			StringNames.StatsGuardianAngelCrewmatesProtected,
			StringNames.StatsScientistChargesGained,
			StringNames.StatsShapeshifterShiftedKills,
			StringNames.StatsHidenSeekGamesCrewmateSurvived,
			StringNames.StatsHidenSeekTimesVented,
			StringNames.StatsTimesPettedPet,
			StringNames.StatsImpostorKills_HideAndSeek
		};

		public void MigrateFromPlayerStats2ToPlayerStats3(BinaryReader reader)
		{
			byte b = reader.ReadByte();
			gameplayStats[StringNames.StatsBodiesReported] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsEmergenciesCalled] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsTasksCompleted] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsAllTasksCompleted] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsSabotagesFixed] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsImpostorKills] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsTimesMurdered] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsTimesEjected] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsCrewmateStreak] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsGamesImpostor] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsGamesCrewmate] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsGamesStarted] = reader.ReadUInt32();
			gameplayStats[StringNames.StatsGamesFinished] = reader.ReadUInt32();
			for (int i = 0; i < 7; i++)
			{
				winReasons[(GameOverReason)i] = reader.ReadUInt32();
			}
			for (int j = 0; j < 7; j++)
			{
				loseReasons[(GameOverReason)j] = reader.ReadUInt32();
			}
			if (b > 1)
			{
				for (int k = 0; k < 7; k++)
				{
					drawReasons[(GameOverReason)k] = reader.ReadUInt32();
				}
			}
			if (b > 2)
			{
				reader.ReadSingle();
				reader.ReadInt64();
			}
			if (b > 3)
			{
				int num = reader.ReadInt32();
				for (int l = 0; l < num; l++)
				{
					mapWins[(MapNames)l] = reader.ReadUInt32();
				}
			}
		}

		public void Reset()
		{
			gameplayStats.Clear();
			winReasons.Clear();
			loseReasons.Clear();
			drawReasons.Clear();
			mapWins.Clear();
			FastestImpostorWin_HideAndSeek = float.NaN;
			FastestCrewmateWin_HideAndSeek = float.NaN;
		}

		public void Deserialize(BinaryReader reader)
		{
			version = reader.ReadByte();
			int num = (int)reader.ReadUInt32();
			for (int i = 0; i < num; i++)
			{
				StringNames key = SimpleStats[i];
				gameplayStats[key] = reader.ReadUInt32();
			}
			for (int j = num; j < SimpleStats.Count; j++)
			{
				StringNames key2 = SimpleStats[j];
				gameplayStats[key2] = 0u;
			}
			ReadEnumDict(winReasons, reader);
			ReadEnumDict(loseReasons, reader);
			ReadEnumDict(drawReasons, reader);
			ReadEnumDict(mapWins, reader);
			ReadEnumDict(roleWins, reader);
			TryReadFloat(ref FastestCrewmateWin_HideAndSeek, reader);
			TryReadFloat(ref FastestImpostorWin_HideAndSeek, reader);
		}

		private void ReadEnumDict<T>(Dictionary<T, uint> enumDict, BinaryReader reader) where T : Enum
		{
			uint num = (uint)Enum.GetNames(typeof(T)).Length;
			int num2 = (int)reader.ReadUInt32();
			for (int i = 0; i < num2; i++)
			{
				T key = (T)Enum.Parse(typeof(T), i.ToString());
				enumDict[key] = reader.ReadUInt32();
			}
			for (int j = num2; j < num; j++)
			{
				T key2 = (T)Enum.Parse(typeof(T), j.ToString());
				enumDict[key2] = 0u;
			}
		}

		private void TryReadFloat(ref float value, BinaryReader reader)
		{
			if (reader.BaseStream.Position >= reader.BaseStream.Length)
			{
				return;
			}
			try
			{
				value = reader.ReadSingle();
			}
			catch
			{
			}
		}
	}

	public static LegacyStatsManager Instance = new LegacyStatsManager();

	public const string LEGACY_STATS_FILENAME = "playerStats3";

	public const string LEGACY_PREVIOUS_STATS_FILENAME = "playerStats2";

	private const byte FINAL_LEGACY_STATS_MANAGER_VERISON = 6;

	private const byte StatsVersion = 6;

	private static string playerStats2Path = FileIO.GetDataPathTo("playerStats2");

	private static string playerStats3Path = FileIO.GetDataPathTo("playerStats3");

	protected LegacyStats stats;

	private bool loadedStats;

	private LegacyStats TheStats
	{
		get
		{
			LoadStats();
			return stats;
		}
	}

	public void ForceLoadStats()
	{
		loadedStats = false;
	}

	public static bool Exists()
	{
		return FileIO.Exists(playerStats3Path);
	}

	protected virtual void LoadStats()
	{
		if (loadedStats)
		{
			return;
		}
		loadedStats = true;
		PlayerStatsData.Logger.Info("[LegacyStatsManager] LoadStats() :: Attempting to load legacy stats file. This should only happen once.");
		stats = new LegacyStats();
		if (FileIO.Exists(playerStats2Path))
		{
			try
			{
				using BinaryReader reader = new BinaryReader(File.OpenRead(playerStats2Path));
				stats = new LegacyStats();
				stats.MigrateFromPlayerStats2ToPlayerStats3(reader);
				PlayerStatsData.Logger.Info("[LegacyStatsManager] LoadStats() :: Stats migration successful, deleting old file");
				FileIO.Delete(playerStats2Path);
				return;
			}
			catch
			{
				PlayerStatsData.Logger.Error("[LegacyStatsManager] LoadStats() :: Deleting corrupted stats file");
				FileIO.Delete(playerStats2Path);
				return;
			}
		}
		if (FileIO.Exists(playerStats3Path))
		{
			try
			{
				using BinaryReader reader2 = new BinaryReader(File.OpenRead(playerStats3Path));
				stats.Deserialize(reader2);
				return;
			}
			catch
			{
				PlayerStatsData.Logger.Error("[LegacyStatsManager] LoadStats() :: Deleting corrupted stats file");
				FileIO.Delete(playerStats3Path);
				return;
			}
		}
		stats = new LegacyStats();
		ResetStatDisplay();
	}

	private void ResetStatDisplay()
	{
		TheStats.Reset();
	}

	public void MigrateToPlayerStatsData(Dictionary<StatID, uint> statsToIngest)
	{
		LoadStats();
		MigrateStatToIngest(StringNames.StatsBodiesReported, StatID.BodiesReported);
		MigrateStatToIngest(StringNames.StatsEmergenciesCalled, StatID.EmergenciesCalled);
		MigrateStatToIngest(StringNames.StatsTasksCompleted, StatID.TasksCompleted);
		MigrateStatToIngest(StringNames.StatsAllTasksCompleted, StatID.AllTasksCompleted);
		MigrateStatToIngest(StringNames.StatsSabotagesFixed, StatID.SabotagesFixed);
		MigrateStatToIngest(StringNames.StatsImpostorKills, StatID.ImpostorKills);
		MigrateStatToIngest(StringNames.StatsTimesMurdered, StatID.TimesMurdered);
		MigrateStatToIngest(StringNames.StatsTimesEjected, StatID.TimesEjected);
		MigrateStatToIngest(StringNames.StatsCrewmateStreak, StatID.CrewmateStreak);
		MigrateStatToIngest(StringNames.StatsGamesImpostor, StatID.GamesAsImpostor);
		MigrateStatToIngest(StringNames.StatsGamesCrewmate, StatID.GamesAsCrewmate);
		MigrateStatToIngest(StringNames.StatsGamesStarted, StatID.GamesStarted);
		MigrateStatToIngest(StringNames.StatsGamesFinished, StatID.GamesFinished);
		MigrateStatToIngest(StringNames.StatsEngineerVents, StatID.Role_Engineer_Vents);
		MigrateStatToIngest(StringNames.StatsGuardianAngelCrewmatesProtected, StatID.Role_GuardianAngel_CrewmatesProtected);
		MigrateStatToIngest(StringNames.StatsScientistChargesGained, StatID.Role_Scientist_ChargesGained);
		MigrateStatToIngest(StringNames.StatsShapeshifterShiftedKills, StatID.Role_Shapeshifter_ShiftedKills);
		MigrateStatToIngest(StringNames.StatsHidenSeekGamesCrewmateSurvived, StatID.HideAndSeek_GamesCrewmateSurvived);
		MigrateStatToIngest(StringNames.StatsHidenSeekTimesVented, StatID.HideAndSeek_TimesVented);
		MigrateStatToIngest(StringNames.StatsTimesPettedPet, StatID.TimesPettedPet);
		MigrateStatToIngest(StringNames.StatsImpostorKills_HideAndSeek, StatID.HideAndSeek_ImpostorKills);
		foreach (GameOverReason key in TheStats.winReasons.Keys)
		{
			SetIngestValue(StatIDUtilities.GetGameResultStatID(key, GameResultStat.Wins), TheStats.winReasons[key]);
		}
		foreach (GameOverReason key2 in TheStats.loseReasons.Keys)
		{
			SetIngestValue(StatIDUtilities.GetGameResultStatID(key2, GameResultStat.Losses), TheStats.loseReasons[key2]);
		}
		foreach (GameOverReason key3 in TheStats.drawReasons.Keys)
		{
			SetIngestValue(StatIDUtilities.GetGameResultStatID(key3, GameResultStat.Draws), TheStats.drawReasons[key3]);
		}
		foreach (MapNames key4 in TheStats.mapWins.Keys)
		{
			SetIngestValue(StatIDUtilities.GetMapStatID(key4, MapStat.Wins), TheStats.mapWins[key4]);
		}
		foreach (RoleTypes key5 in TheStats.roleWins.Keys)
		{
			SetIngestValue(StatIDUtilities.GetRoleStatID(key5, RoleStat.Wins), TheStats.roleWins[key5]);
		}
		if (!float.IsNaN(TheStats.FastestCrewmateWin_HideAndSeek) && TheStats.FastestCrewmateWin_HideAndSeek > 0f)
		{
			SetIngestValue(StatID.HideAndSeek_FastestCrewmateWin, (uint)Mathf.FloorToInt(TheStats.FastestCrewmateWin_HideAndSeek));
		}
		if (!float.IsNaN(TheStats.FastestImpostorWin_HideAndSeek) && TheStats.FastestImpostorWin_HideAndSeek > 0f)
		{
			SetIngestValue(StatID.HideAndSeek_FastestImpostorWin, (uint)Mathf.FloorToInt(TheStats.FastestImpostorWin_HideAndSeek));
		}
		if (FileIO.Exists(playerStats3Path))
		{
			PlayerStatsData.Logger.Info("MigrateToPlayerStatsData() :: Deleting [" + "playerStats3".Color(Palette.LogErrorColor) + "]");
			FileIO.Delete(playerStats3Path);
		}
		if (FileIO.Exists(playerStats2Path))
		{
			PlayerStatsData.Logger.Info("MigrateToPlayerStatsData() :: Deleting [" + "playerStats2".Color(Palette.LogErrorColor) + "]");
			FileIO.Delete(playerStats2Path);
		}
		void MigrateStatToIngest(StringNames stringName, StatID stat)
		{
			PlayerStatsData.Logger.Info(string.Format("{0}() :: Migrating [{1}.{2}] to [{3}.{4}]", "MigrateToPlayerStatsData", "StringNames".Color(Palette.LogErrorColor), stringName, "StatID".Color(Palette.LogSuccessColor), stat));
			if (!TheStats.gameplayStats.ContainsKey(stringName))
			{
				PlayerStatsData.Logger.Info(string.Format("{0}() :: [{1}.{2}] not found. Skipped.", "MigrateToPlayerStatsData", "StringNames".Color(Palette.LogErrorColor), stringName));
			}
			else
			{
				SetIngestValue(stat, TheStats.gameplayStats[stringName]);
			}
		}
		void SetIngestValue(StatID stat, uint value)
		{
			if (stat != StatID.Invalid && stat != StatID.None)
			{
				PlayerStatsData.Logger.Info(string.Format("{0}() :: Setting [{1}.{2}] value to [{3}]", "MigrateToPlayerStatsData", "StatID".Color(Palette.LogSuccessColor), stat, value));
				if (!statsToIngest.ContainsKey(stat))
				{
					statsToIngest.Add(stat, 0u);
				}
				statsToIngest[stat] = value;
			}
		}
	}
}
