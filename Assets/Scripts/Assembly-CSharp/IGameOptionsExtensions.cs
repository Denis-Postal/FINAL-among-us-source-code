using System.Text;
using AmongUs.GameOptions;
using UnityEngine;

public static class IGameOptionsExtensions
{
	private const byte SkeldBit = 1;

	private const byte MiraBit = 2;

	private const byte PolusBit = 4;

	private const byte AirshipBit = 16;

	private const byte FungleBit = 32;

	private const byte MapMask = 55;

	private static StringBuilder SettingsStringBuilder = new StringBuilder(2048);

	public static void SetRecommendations(this IGameOptions gameOptions, int numPlayers, NetworkModes modes)
	{
		numPlayers = Mathf.Clamp(numPlayers, 4, 15);
		bool isOnlineGame = modes == NetworkModes.OnlineGame;
		gameOptions.SetRecommendations(numPlayers, isOnlineGame);
	}

	public static void SetRecommendations(this IGameOptions gameOptions, int numPlayers, NetworkModes modes, RulesPresets rulesPresets)
	{
		numPlayers = Mathf.Clamp(numPlayers, 4, 15);
		bool isOnlineGame = modes == NetworkModes.OnlineGame;
		gameOptions.SetRecommendations(numPlayers, isOnlineGame, rulesPresets);
	}

	public static int GetAdjustedNumImpostors(this IGameOptions gameOptions, int playerCount)
	{
		int numImpostors = GameOptionsManager.Instance.CurrentGameOptions.NumImpostors;
		// Comment out the current implementation:
		// int max = 3;
		// int[] intArray = gameOptions.GetIntArray(Int32ArrayOptionNames.MaxImpostors);
		// if (intArray != null && GameData.Instance.PlayerCount < intArray.Length)
		// {
		// 	max = intArray[GameData.Instance.PlayerCount];
		// }
		// return Mathf.Clamp(numImpostors, 1, max);

		int max = (gameOptions != null && gameOptions.GameMode == GameModes.HideNSeek) ? 15 : 5;
		int[] intArray = gameOptions?.GetIntArray(Int32ArrayOptionNames.MaxImpostors);
		int currentCount = (GameData.Instance != null && GameData.Instance.PlayerCount > 0) ? GameData.Instance.PlayerCount : playerCount;
		if (intArray != null && currentCount < intArray.Length && currentCount >= 0)
		{
			max = intArray[currentCount];
		}
		if (currentCount > 0)
		{
			max = Mathf.Min(max, currentCount);
		}
		return Mathf.Clamp(numImpostors, 1, max);
	}

	public static void ToggleMapFilter(this IGameOptions gameOptions, byte newId)
	{
		byte b = (byte)((gameOptions.MapId ^ (1 << (int)newId)) & 0x37);
		if (b != 0)
		{
			gameOptions.SetByte(ByteOptionNames.MapId, b);
		}
	}

	public static bool FilterContainsMap(this IGameOptions gameOptions, byte newId)
	{
		int num = 1 << (int)newId;
		return (gameOptions.MapId & num) == num;
	}

	public static float GetValue(this IGameOptions gameOptions, BaseGameSetting data)
	{
		float value = -1f;
		int value2 = -1;
		bool value3 = false;
		bool flag = false;
		switch (data.Type)
		{
		case OptionTypes.Checkbox:
			gameOptions.TryGetBool((data as CheckboxGameSetting).OptionName, out value3);
			flag = true;
			break;
		case OptionTypes.Float:
			gameOptions.TryGetFloat((data as FloatGameSetting).OptionName, out value);
			break;
		case OptionTypes.Int:
			gameOptions.TryGetInt((data as IntGameSetting).OptionName, out value2);
			break;
		case OptionTypes.Player:
			gameOptions.TryGetInt((data as PlayerSelectionGameSetting).OptionName, out value2);
			break;
		case OptionTypes.String:
			gameOptions.TryGetInt((data as StringGameSetting).OptionName, out value2);
			break;
		case OptionTypes.Map:
			(data as MapSelectionGameSetting)?.TryGetInt(gameOptions, out value2);
			break;
		default:
			Debug.LogError("Could not parse type of " + data.Title);
			break;
		}
		if (value == -1f && value2 != -1)
		{
			value = value2;
		}
		else if ((value == -1f) & flag)
		{
			value = (value3 ? 1 : 0);
		}
		return value;
	}

	public static string ToHudString(this IGameOptions gameOptions, int numPlayers)
	{
		if (!gameOptions.TryGetIntArray(Int32ArrayOptionNames.MaxImpostors, out var value))
		{
			Debug.LogError("Failed to generate HUD string for game options, no MaxImpostors array found");
			return "hud string generation failed";
		}
		numPlayers = Mathf.Clamp(numPlayers, 0, value.Length);
		SettingsStringBuilder.Length = 0;
		try
		{
			SettingsStringBuilder.AppendLine(DestroyableSingleton<TranslationController>.Instance.GetString(gameOptions.GetBool(BoolOptionNames.IsDefaults) ? StringNames.GameRecommendedSettings : StringNames.GameCustomSettings));
			int num = value[numPlayers];
			int num2 = ((gameOptions.MapId == 0 && AprilFoolsMode.ShouldFlipSkeld()) ? 3 : gameOptions.MapId);
			string value2 = Constants.MapNames[num2];
			AppendItem(SettingsStringBuilder, StringNames.GameMapName, value2);
			SettingsStringBuilder.Append($"{DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameNumImpostors)}: {gameOptions.NumImpostors}");
			if (gameOptions.NumImpostors > num)
			{
				SettingsStringBuilder.Append($" ({DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Limit)}: {num})");
			}
			SettingsStringBuilder.AppendLine();
			GameModes gameMode = gameOptions.GameMode;
			if (gameMode == GameModes.Normal)
			{
				AppendItem(SettingsStringBuilder, StringNames.GameConfirmImpostor, gameOptions.GetBool(BoolOptionNames.ConfirmImpostor));
				AppendItem(SettingsStringBuilder, StringNames.GameNumMeetings, gameOptions.GetInt(Int32OptionNames.NumEmergencyMeetings));
				AppendItem(SettingsStringBuilder, StringNames.GameAnonymousVotes, gameOptions.GetBool(BoolOptionNames.AnonymousVotes));
				AppendItem(SettingsStringBuilder, StringNames.GameEmergencyCooldown, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameSecondsAbbrev, gameOptions.GetInt(Int32OptionNames.EmergencyCooldown)));
				AppendItem(SettingsStringBuilder, StringNames.GameDiscussTime, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameSecondsAbbrev, gameOptions.GetInt(Int32OptionNames.DiscussionTime)));
				int num3 = gameOptions.GetInt(Int32OptionNames.VotingTime);
				if (num3 > 0)
				{
					AppendItem(SettingsStringBuilder, StringNames.GameVotingTime, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameSecondsAbbrev, num3));
				}
				else
				{
					AppendItem(SettingsStringBuilder, StringNames.GameVotingTime, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameSecondsAbbrev, "∞"));
				}
				AppendItem(SettingsStringBuilder, StringNames.GamePlayerSpeed, gameOptions.GetFloat(FloatOptionNames.PlayerSpeedMod), "x");
				AppendItem(SettingsStringBuilder, StringNames.GameTaskBarMode, DestroyableSingleton<TranslationController>.Instance.GetString((StringNames)(277 + gameOptions.GetInt(Int32OptionNames.TaskBarMode))));
				AppendItem(SettingsStringBuilder, StringNames.GameVisualTasks, gameOptions.GetBool(BoolOptionNames.VisualTasks));
			}
			AppendItem(SettingsStringBuilder, StringNames.GameCrewLight, gameOptions.GetFloat(FloatOptionNames.CrewLightMod), "x");
			AppendItem(SettingsStringBuilder, StringNames.GameImpostorLight, gameOptions.GetFloat(FloatOptionNames.ImpostorLightMod), "x");
			AppendItem(SettingsStringBuilder, StringNames.GameKillCooldown, DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.GameSecondsAbbrev, gameOptions.GetFloat(FloatOptionNames.KillCooldown)));
			AppendItem(SettingsStringBuilder, StringNames.GameKillDistance, DestroyableSingleton<TranslationController>.Instance.GetString((StringNames)(204 + gameOptions.GetInt(Int32OptionNames.KillDistance))));
			AppendItem(SettingsStringBuilder, StringNames.GameCommonTasks, gameOptions.GetInt(Int32OptionNames.NumCommonTasks));
			AppendItem(SettingsStringBuilder, StringNames.GameLongTasks, gameOptions.GetInt(Int32OptionNames.NumLongTasks));
			AppendItem(SettingsStringBuilder, StringNames.GameShortTasks, gameOptions.GetInt(Int32OptionNames.NumShortTasks));
			if (gameMode == GameModes.Normal)
			{
				foreach (RoleBehaviour allRole in DestroyableSingleton<RoleManager>.Instance.AllRoles)
				{
					if (allRole.Role != RoleTypes.Crewmate && allRole.Role != RoleTypes.Impostor && allRole.Role != RoleTypes.CrewmateGhost && allRole.Role != RoleTypes.ImpostorGhost)
					{
						AppendItem(SettingsStringBuilder, DestroyableSingleton<TranslationController>.Instance.GetString(allRole.StringName) + ": " + DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoleChanceAndQuantity, gameOptions.RoleOptions.GetNumPerGame(allRole.Role), gameOptions.RoleOptions.GetChancePerGame(allRole.Role)));
					}
				}
			}
			if (gameMode == GameModes.HideNSeek)
			{
				AppendImpostorPlayerSetting(SettingsStringBuilder, gameOptions);
				AppendItem(SettingsStringBuilder, StringNames.GamePlayerSpeed, gameOptions.GetFloat(FloatOptionNames.PlayerSpeedMod), "x");
				AppendItem(SettingsStringBuilder, StringNames.EscapeTime, gameOptions.GetFloat(FloatOptionNames.EscapeTime), "s");
				AppendItem(SettingsStringBuilder, StringNames.FinalEscapeTime, gameOptions.GetFloat(FloatOptionNames.FinalEscapeTime), "s");
				AppendItem(SettingsStringBuilder, StringNames.MaxVentUses, gameOptions.GetInt(Int32OptionNames.CrewmateVentUses));
				AppendItem(SettingsStringBuilder, StringNames.MaxTimeInVent, gameOptions.GetFloat(FloatOptionNames.CrewmateTimeInVent), "s");
				AppendItem(SettingsStringBuilder, StringNames.UseFlashlight, gameOptions.GetBool(BoolOptionNames.UseFlashlight));
				AppendItem(SettingsStringBuilder, StringNames.CrewmateFlashlightSize, gameOptions.GetFloat(FloatOptionNames.CrewmateFlashlightSize), "x");
				AppendItem(SettingsStringBuilder, StringNames.ImpostorFlashlightSize, gameOptions.GetFloat(FloatOptionNames.ImpostorFlashlightSize), "x");
				AppendItem(SettingsStringBuilder, StringNames.SeekerFinalSpeed, gameOptions.GetFloat(FloatOptionNames.SeekerFinalSpeed), "");
				AppendItem(SettingsStringBuilder, StringNames.SeekerFinalMap, gameOptions.GetBool(BoolOptionNames.SeekerFinalMap));
				AppendItem(SettingsStringBuilder, StringNames.SeekerPings, gameOptions.GetBool(BoolOptionNames.SeekerPings));
				AppendItem(SettingsStringBuilder, StringNames.MaxPingTime, gameOptions.GetFloat(FloatOptionNames.MaxPingTime), "s");
				AppendItem(SettingsStringBuilder, StringNames.ShowCrewmateNames, gameOptions.GetBool(BoolOptionNames.ShowCrewmateNames));
			}
		}
		catch
		{
		}
		return SettingsStringBuilder.ToString();
	}

	private static void AppendItem(StringBuilder settings, StringNames stringName, bool value)
	{
		settings.Append(DestroyableSingleton<TranslationController>.Instance.GetString(stringName));
		settings.Append(": ");
		settings.AppendLine(DestroyableSingleton<TranslationController>.Instance.GetString(value ? StringNames.SettingsOn : StringNames.SettingsOff));
	}

	private static void AppendItem(StringBuilder settings, StringNames stringName, float value, string secs)
	{
		settings.Append(DestroyableSingleton<TranslationController>.Instance.GetString(stringName));
		settings.Append(": ");
		settings.Append(value);
		settings.AppendLine(secs);
	}

	private static void AppendItem(StringBuilder settings, StringNames stringName, int value, string secs)
	{
		settings.Append(DestroyableSingleton<TranslationController>.Instance.GetString(stringName));
		settings.Append(": ");
		settings.Append(value);
		settings.AppendLine(secs);
	}

	private static void AppendItem(StringBuilder settings, StringNames stringName, string value)
	{
		settings.Append(DestroyableSingleton<TranslationController>.Instance.GetString(stringName));
		settings.Append(": ");
		settings.AppendLine(value);
	}

	private static void AppendItem(StringBuilder settings, StringNames stringName, int value)
	{
		settings.Append(DestroyableSingleton<TranslationController>.Instance.GetString(stringName));
		settings.Append(": ");
		settings.Append(value);
		settings.AppendLine();
	}

	private static void AppendItem(StringBuilder settings, string str)
	{
		settings.Append(str);
		settings.AppendLine();
	}

	private static void AppendImpostorPlayerSetting(StringBuilder settings, IGameOptions options)
	{
		bool isGamePublic = AmongUsClient.Instance.IsGamePublic;
		settings.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Impostor));
		settings.Append(": ");
		int id = options.GetInt(Int32OptionNames.ImpostorPlayerID);
		if ((id < 0) | isGamePublic)
		{
			settings.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoundRobin));
		}
		else
		{
			NetworkedPlayerInfo networkedPlayerInfo = GameData.Instance.AllPlayers.Find((NetworkedPlayerInfo p) => p.PlayerId == id);
			if (networkedPlayerInfo != null)
			{
				settings.Append(networkedPlayerInfo.PlayerName);
			}
			else
			{
				settings.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoundRobin));
			}
		}
		settings.AppendLine();
	}

	public static StringNames GetRulesPresetTitle(this IGameOptions gameOptions)
	{
		return gameOptions.RulesPreset switch
		{
			RulesPresets.Standard => GameManager.Instance.GameSettingsList.StandardRulesName, 
			RulesPresets.Custom => StringNames.GameModeCustom, 
			_ => GameManager.Instance.GameSettingsList.AlternateRulesName, 
		};
	}
}
