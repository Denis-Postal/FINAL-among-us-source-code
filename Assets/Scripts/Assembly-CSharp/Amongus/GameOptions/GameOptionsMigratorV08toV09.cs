using System;
using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class GameOptionsMigratorV08toV09 : IGameOptionsMigrator
	{
		public readonly ILogger logger;

		public GameOptionsMigratorV08toV09(ILogger logger)
		{
			this.logger = logger;
		}

		public bool TryMigrate(IGameOptions previousVersion, out IGameOptions updatedVersion)
		{
			switch (previousVersion.GameMode)
			{
			default:
				logger.WriteError($"{GetType().Name} expects previous version to have a valid GameMode, but it got {previousVersion.GameMode}");
				updatedVersion = null;
				return false;
			case GameModes.Normal:
			case GameModes.NormalFools:
				return TryMigrateNormal(previousVersion, out updatedVersion);
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				return TryMigrateHideAndSeek(previousVersion, out updatedVersion);
			}
		}

		private bool TryMigrateNormal(IGameOptions previousVersion, out IGameOptions updatedVersion)
		{
			updatedVersion = null;
			NormalGameOptionsV08 normalGameOptionsV = previousVersion as NormalGameOptionsV08;
			NormalGameOptionsV09 normalGameOptionsV2 = new NormalGameOptionsV09(logger);
			if (normalGameOptionsV == null)
			{
				logger.WriteError($"{GetType().Name} expects previous version = {normalGameOptionsV2.Version - 1}, but got {previousVersion.Version}");
				return false;
			}
			normalGameOptionsV2.MaxPlayers = normalGameOptionsV.MaxPlayers;
			normalGameOptionsV2.Keywords = normalGameOptionsV.Keywords;
			normalGameOptionsV2.MapId = normalGameOptionsV.MapId;
			normalGameOptionsV2.PlayerSpeedMod = normalGameOptionsV.PlayerSpeedMod;
			normalGameOptionsV2.CrewLightMod = normalGameOptionsV.CrewLightMod;
			normalGameOptionsV2.ImpostorLightMod = normalGameOptionsV.ImpostorLightMod;
			normalGameOptionsV2.KillCooldown = normalGameOptionsV.KillCooldown;
			normalGameOptionsV2.NumCommonTasks = normalGameOptionsV.NumCommonTasks;
			normalGameOptionsV2.NumLongTasks = normalGameOptionsV.NumLongTasks;
			normalGameOptionsV2.NumShortTasks = normalGameOptionsV.NumShortTasks;
			normalGameOptionsV2.NumEmergencyMeetings = normalGameOptionsV.NumEmergencyMeetings;
			normalGameOptionsV2.EmergencyCooldown = normalGameOptionsV.EmergencyCooldown;
			normalGameOptionsV2.NumImpostors = normalGameOptionsV.NumImpostors;
			normalGameOptionsV2.GhostsDoTasks = normalGameOptionsV.GhostsDoTasks;
			normalGameOptionsV2.KillDistance = normalGameOptionsV.KillDistance;
			normalGameOptionsV2.DiscussionTime = normalGameOptionsV.DiscussionTime;
			normalGameOptionsV2.VotingTime = normalGameOptionsV.VotingTime;
			normalGameOptionsV2.ConfirmImpostor = normalGameOptionsV.ConfirmImpostor;
			normalGameOptionsV2.VisualTasks = normalGameOptionsV.VisualTasks;
			normalGameOptionsV2.AnonymousVotes = normalGameOptionsV.AnonymousVotes;
			normalGameOptionsV2.TaskBarMode = normalGameOptionsV.TaskBarMode;
			normalGameOptionsV2.SpecialMode = normalGameOptionsV.SpecialMode;
			normalGameOptionsV2.RulesPreset = normalGameOptionsV.RulesPreset;
			normalGameOptionsV2.Tag = normalGameOptionsV2.Tag;
			normalGameOptionsV2.IsDefaults = normalGameOptionsV.IsDefaults;
			normalGameOptionsV2.RoleOptions.SetRoleRate(RoleTypes.Shapeshifter, normalGameOptionsV.RoleOptions.GetNumPerGame(RoleTypes.Shapeshifter), normalGameOptionsV.RoleOptions.GetChancePerGame(RoleTypes.Shapeshifter));
			normalGameOptionsV2.RoleOptions.SetRoleRate(RoleTypes.Scientist, normalGameOptionsV.RoleOptions.GetNumPerGame(RoleTypes.Scientist), normalGameOptionsV.RoleOptions.GetChancePerGame(RoleTypes.Scientist));
			normalGameOptionsV2.RoleOptions.SetRoleRate(RoleTypes.GuardianAngel, normalGameOptionsV.RoleOptions.GetNumPerGame(RoleTypes.GuardianAngel), normalGameOptionsV.RoleOptions.GetChancePerGame(RoleTypes.GuardianAngel));
			normalGameOptionsV2.RoleOptions.SetRoleRate(RoleTypes.Engineer, normalGameOptionsV.RoleOptions.GetNumPerGame(RoleTypes.Engineer), normalGameOptionsV.RoleOptions.GetChancePerGame(RoleTypes.Engineer));
			normalGameOptionsV2.SetFloat(FloatOptionNames.ShapeshifterCooldown, normalGameOptionsV.GetFloat(FloatOptionNames.ShapeshifterCooldown));
			normalGameOptionsV2.SetFloat(FloatOptionNames.ShapeshifterDuration, normalGameOptionsV.GetFloat(FloatOptionNames.ShapeshifterDuration));
			normalGameOptionsV2.SetBool(BoolOptionNames.ShapeshifterLeaveSkin, normalGameOptionsV.GetBool(BoolOptionNames.ShapeshifterLeaveSkin));
			normalGameOptionsV2.SetFloat(FloatOptionNames.ScientistBatteryCharge, normalGameOptionsV.GetFloat(FloatOptionNames.ScientistBatteryCharge));
			normalGameOptionsV2.SetFloat(FloatOptionNames.ScientistCooldown, normalGameOptionsV.GetFloat(FloatOptionNames.ScientistCooldown));
			normalGameOptionsV2.SetFloat(FloatOptionNames.GuardianAngelCooldown, normalGameOptionsV.GetFloat(FloatOptionNames.GuardianAngelCooldown));
			normalGameOptionsV2.SetFloat(FloatOptionNames.ProtectionDurationSeconds, normalGameOptionsV.GetFloat(FloatOptionNames.ProtectionDurationSeconds));
			normalGameOptionsV2.SetBool(BoolOptionNames.ImpostorsCanSeeProtect, normalGameOptionsV.GetBool(BoolOptionNames.ImpostorsCanSeeProtect));
			normalGameOptionsV2.SetFloat(FloatOptionNames.EngineerCooldown, normalGameOptionsV.GetFloat(FloatOptionNames.EngineerCooldown));
			normalGameOptionsV2.SetFloat(FloatOptionNames.EngineerInVentMaxTime, normalGameOptionsV.GetFloat(FloatOptionNames.EngineerInVentMaxTime));
			normalGameOptionsV2.RoleOptions.SetRoleRate(RoleTypes.Noisemaker, normalGameOptionsV.RoleOptions.GetNumPerGame(RoleTypes.Noisemaker), normalGameOptionsV.RoleOptions.GetChancePerGame(RoleTypes.Noisemaker));
			normalGameOptionsV2.RoleOptions.SetRoleRate(RoleTypes.Phantom, normalGameOptionsV.RoleOptions.GetNumPerGame(RoleTypes.Phantom), normalGameOptionsV.RoleOptions.GetChancePerGame(RoleTypes.Phantom));
			normalGameOptionsV2.RoleOptions.SetRoleRate(RoleTypes.Tracker, normalGameOptionsV.RoleOptions.GetNumPerGame(RoleTypes.Tracker), normalGameOptionsV.RoleOptions.GetChancePerGame(RoleTypes.Tracker));
			CopyFloat(normalGameOptionsV2, normalGameOptionsV, FloatOptionNames.PhantomCooldown);
			CopyFloat(normalGameOptionsV2, normalGameOptionsV, FloatOptionNames.PhantomDuration);
			CopyFloat(normalGameOptionsV2, normalGameOptionsV, FloatOptionNames.TrackerCooldown);
			CopyFloat(normalGameOptionsV2, normalGameOptionsV, FloatOptionNames.TrackerDuration);
			CopyFloat(normalGameOptionsV2, normalGameOptionsV, FloatOptionNames.TrackerDelay);
			CopyFloat(normalGameOptionsV2, normalGameOptionsV, FloatOptionNames.NoisemakerAlertDuration);
			CopyBool(normalGameOptionsV2, normalGameOptionsV, BoolOptionNames.NoisemakerImpostorAlert);
			CopyInt32(normalGameOptionsV2, normalGameOptionsV, Int32OptionNames.RulePreset);
			updatedVersion = normalGameOptionsV2;
			return true;
		}

		private bool TryMigrateHideAndSeek(IGameOptions previousVersion, out IGameOptions updatedVersion)
		{
			updatedVersion = null;
			HideNSeekGameOptionsV08 hideNSeekGameOptionsV = previousVersion as HideNSeekGameOptionsV08;
			HideNSeekGameOptionsV09 hideNSeekGameOptionsV2 = new HideNSeekGameOptionsV09(logger);
			if (hideNSeekGameOptionsV == null)
			{
				logger.WriteError($"{GetType().Name} expects previous version = {hideNSeekGameOptionsV2.Version - 1}, but got {previousVersion.Version}");
				return false;
			}
			hideNSeekGameOptionsV2.MaxPlayers = hideNSeekGameOptionsV.MaxPlayers;
			hideNSeekGameOptionsV2.Keywords = hideNSeekGameOptionsV.Keywords;
			hideNSeekGameOptionsV2.MapId = hideNSeekGameOptionsV.MapId;
			hideNSeekGameOptionsV2.PlayerSpeedMod = hideNSeekGameOptionsV.PlayerSpeedMod;
			hideNSeekGameOptionsV2.CrewLightMod = hideNSeekGameOptionsV.CrewLightMod;
			hideNSeekGameOptionsV2.ImpostorLightMod = hideNSeekGameOptionsV.ImpostorLightMod;
			hideNSeekGameOptionsV2.KillCooldown = hideNSeekGameOptionsV.KillCooldown;
			hideNSeekGameOptionsV2.NumCommonTasks = hideNSeekGameOptionsV.NumCommonTasks;
			hideNSeekGameOptionsV2.NumLongTasks = hideNSeekGameOptionsV.NumLongTasks;
			hideNSeekGameOptionsV2.NumShortTasks = hideNSeekGameOptionsV.NumShortTasks;
			hideNSeekGameOptionsV2.GhostsDoTasks = hideNSeekGameOptionsV.GhostsDoTasks;
			hideNSeekGameOptionsV2.KillDistance = hideNSeekGameOptionsV.KillDistance;
			hideNSeekGameOptionsV2.CrewmateVentUses = hideNSeekGameOptionsV.CrewmateVentUses;
			hideNSeekGameOptionsV2.CrewmateTimeInVent = hideNSeekGameOptionsV.CrewmateTimeInVent;
			hideNSeekGameOptionsV2.EscapeTime = hideNSeekGameOptionsV.EscapeTime;
			hideNSeekGameOptionsV2.FinalCountdownTime = hideNSeekGameOptionsV.FinalCountdownTime;
			hideNSeekGameOptionsV2.CrewmateFlashlightSize = hideNSeekGameOptionsV.CrewmateFlashlightSize;
			hideNSeekGameOptionsV2.ImpostorFlashlightSize = hideNSeekGameOptionsV.ImpostorFlashlightSize;
			hideNSeekGameOptionsV2.useFlashlight = hideNSeekGameOptionsV.useFlashlight;
			hideNSeekGameOptionsV2.SeekerFinalMap = hideNSeekGameOptionsV.SeekerFinalMap;
			hideNSeekGameOptionsV2.SeekerFinalSpeed = hideNSeekGameOptionsV.SeekerFinalSpeed;
			hideNSeekGameOptionsV2.ShowCrewmateNames = hideNSeekGameOptionsV.ShowCrewmateNames;
			hideNSeekGameOptionsV2.MaxPingTime = hideNSeekGameOptionsV.MaxPingTime;
			hideNSeekGameOptionsV2.SeekerPings = hideNSeekGameOptionsV.SeekerPings;
			hideNSeekGameOptionsV2.SpecialMode = hideNSeekGameOptionsV.SpecialMode;
			hideNSeekGameOptionsV2.RulesPreset = hideNSeekGameOptionsV.RulesPreset;
			hideNSeekGameOptionsV2.Tag = hideNSeekGameOptionsV2.Tag;
			hideNSeekGameOptionsV2.NumImpostors = hideNSeekGameOptionsV.NumImpostors;
			hideNSeekGameOptionsV2.IsDefaults = hideNSeekGameOptionsV.IsDefaults;
			updatedVersion = hideNSeekGameOptionsV2;
			return true;
		}

		private void CopyFloat(IGameOptions newVersion, IGameOptions oldVersion, FloatOptionNames option)
		{
			newVersion.SetFloat(option, oldVersion.GetFloat(option));
		}

		private void CopyBool(IGameOptions newVersion, IGameOptions oldVersion, BoolOptionNames option)
		{
			newVersion.SetBool(option, oldVersion.GetBool(option));
		}

		private void CopyInt32(IGameOptions newVersion, IGameOptions oldVersion, Int32OptionNames option)
		{
			newVersion.SetInt(option, oldVersion.GetInt(option));
		}
	}
}
