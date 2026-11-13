using System;
using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class GameOptionsMigratorV06toV07 : IGameOptionsMigrator
	{
		public readonly ILogger logger;

		public GameOptionsMigratorV06toV07(ILogger logger)
		{
			this.logger = logger;
		}

		public bool TryMigrate(IGameOptions previousVersion, out IGameOptions updatedVersion)
		{
			updatedVersion = null;
			NormalGameOptionsV07 normalGameOptionsV = new NormalGameOptionsV07(logger);
			if (!(previousVersion is LegacyGameOptions legacyGameOptions))
			{
				logger.WriteError($"GameOptionsMigratorV06toV07 expects previous version = 6, but got {previousVersion.Version}");
				return false;
			}
			normalGameOptionsV.MaxPlayers = legacyGameOptions.MaxPlayers;
			normalGameOptionsV.Keywords = legacyGameOptions.Keywords;
			normalGameOptionsV.MapId = legacyGameOptions.MapId;
			normalGameOptionsV.PlayerSpeedMod = legacyGameOptions.PlayerSpeedMod;
			normalGameOptionsV.CrewLightMod = legacyGameOptions.CrewLightMod;
			normalGameOptionsV.ImpostorLightMod = legacyGameOptions.ImpostorLightMod;
			normalGameOptionsV.KillCooldown = legacyGameOptions.KillCooldown;
			normalGameOptionsV.NumCommonTasks = legacyGameOptions.NumCommonTasks;
			normalGameOptionsV.NumLongTasks = legacyGameOptions.NumLongTasks;
			normalGameOptionsV.NumShortTasks = legacyGameOptions.NumShortTasks;
			normalGameOptionsV.NumEmergencyMeetings = legacyGameOptions.NumEmergencyMeetings;
			normalGameOptionsV.EmergencyCooldown = legacyGameOptions.EmergencyCooldown;
			normalGameOptionsV.NumImpostors = legacyGameOptions.NumImpostors;
			normalGameOptionsV.GhostsDoTasks = legacyGameOptions.GhostsDoTasks;
			normalGameOptionsV.KillDistance = legacyGameOptions.KillDistance;
			normalGameOptionsV.DiscussionTime = legacyGameOptions.DiscussionTime;
			normalGameOptionsV.VotingTime = legacyGameOptions.VotingTime;
			normalGameOptionsV.ConfirmImpostor = legacyGameOptions.ConfirmImpostor;
			normalGameOptionsV.VisualTasks = legacyGameOptions.VisualTasks;
			normalGameOptionsV.AnonymousVotes = legacyGameOptions.AnonymousVotes;
			normalGameOptionsV.TaskBarMode = legacyGameOptions.TaskBarMode;
			normalGameOptionsV.IsDefaults = legacyGameOptions.IsDefaults;
			normalGameOptionsV.RoleOptions.SetRoleRate(RoleTypes.Shapeshifter, legacyGameOptions.RoleOptions.GetNumPerGame(RoleTypes.Shapeshifter), legacyGameOptions.RoleOptions.GetChancePerGame(RoleTypes.Shapeshifter));
			normalGameOptionsV.RoleOptions.SetRoleRate(RoleTypes.Scientist, legacyGameOptions.RoleOptions.GetNumPerGame(RoleTypes.Scientist), legacyGameOptions.RoleOptions.GetChancePerGame(RoleTypes.Scientist));
			normalGameOptionsV.RoleOptions.SetRoleRate(RoleTypes.GuardianAngel, legacyGameOptions.RoleOptions.GetNumPerGame(RoleTypes.GuardianAngel), legacyGameOptions.RoleOptions.GetChancePerGame(RoleTypes.GuardianAngel));
			normalGameOptionsV.RoleOptions.SetRoleRate(RoleTypes.Engineer, legacyGameOptions.RoleOptions.GetNumPerGame(RoleTypes.Engineer), legacyGameOptions.RoleOptions.GetChancePerGame(RoleTypes.Engineer));
			normalGameOptionsV.SetFloat(FloatOptionNames.ShapeshifterCooldown, legacyGameOptions.GetFloat(FloatOptionNames.ShapeshifterCooldown));
			normalGameOptionsV.SetFloat(FloatOptionNames.ShapeshifterDuration, legacyGameOptions.GetFloat(FloatOptionNames.ShapeshifterDuration));
			normalGameOptionsV.SetBool(BoolOptionNames.ShapeshifterLeaveSkin, legacyGameOptions.GetBool(BoolOptionNames.ShapeshifterLeaveSkin));
			normalGameOptionsV.SetFloat(FloatOptionNames.ScientistBatteryCharge, legacyGameOptions.GetFloat(FloatOptionNames.ScientistBatteryCharge));
			normalGameOptionsV.SetFloat(FloatOptionNames.ScientistCooldown, legacyGameOptions.GetFloat(FloatOptionNames.ScientistCooldown));
			normalGameOptionsV.SetFloat(FloatOptionNames.GuardianAngelCooldown, legacyGameOptions.GetFloat(FloatOptionNames.GuardianAngelCooldown));
			normalGameOptionsV.SetFloat(FloatOptionNames.ProtectionDurationSeconds, legacyGameOptions.GetFloat(FloatOptionNames.ProtectionDurationSeconds));
			normalGameOptionsV.SetBool(BoolOptionNames.ImpostorsCanSeeProtect, legacyGameOptions.GetBool(BoolOptionNames.ImpostorsCanSeeProtect));
			normalGameOptionsV.SetFloat(FloatOptionNames.EngineerCooldown, legacyGameOptions.GetFloat(FloatOptionNames.EngineerCooldown));
			normalGameOptionsV.SetFloat(FloatOptionNames.EngineerInVentMaxTime, legacyGameOptions.GetFloat(FloatOptionNames.EngineerInVentMaxTime));
			updatedVersion = normalGameOptionsV;
			return true;
		}
	}
}
