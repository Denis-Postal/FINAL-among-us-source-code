using System;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using Cpp2ILInjected;

public static class StatIDUtilities
{
	private const string EOS_STAT_PREFIX = "PlayerStat_";

	public unsafe static bool TryGetStatID(out StatID stat, params object[] search)
	{
		string text = string.Join("_", search);
		if (Enum.TryParse<StatID>(text, out stat))
		{
			return true;
		}
		PlayerStatsData.Logger.Error("[StatIDUtilities] TryGetStatID() :: Could not find StatID matching string '" + text + "'");
		stat = StatID.Invalid;
		return false;
	}

	public static StatID GetStatID(params object[] search)
	{
		TryGetStatID(out var stat, search);
		return stat;
	}

	public static StatID GetMapStatID(MapNames map, MapStat stat)
	{
		return GetStatID("Map", map, stat);
	}

	public unsafe static StatID GetRoleStatID(RoleTypes role, RoleStat stat)
	{
		return GetRoleStatID(role, stat.ToString());
	}

	public static StatID GetRoleStatID(RoleTypes role, string stat)
	{
		return GetStatID("Role", role, stat);
	}

	public static StatID GetGameResultStatID(GameOverReason reason, GameResultStat stat)
	{
		return GetStatID("GameResult", reason, stat);
	}

	public static string GetStatNameForEOS(StatID stat)
	{
		return string.Format("{0}{1}", "PlayerStat_", stat);
	}

	public static StatID GetStatIDFromEOSStatName(string name)
	{
		return GetStatID(name.Replace("PlayerStat_", ""));
	}
}
