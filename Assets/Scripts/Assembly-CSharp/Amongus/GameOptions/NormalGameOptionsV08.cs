using System;
using System.Runtime.CompilerServices;
using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class NormalGameOptionsV08 : IGameOptions
	{
		private static readonly int[] RecommendedKillCooldown = new int[16]
		{
			0, 0, 0, 0, 45, 30, 15, 35, 30, 25,
			20, 20, 20, 20, 20, 20
		};

		private static readonly int[] RecommendedImpostors = new int[16]
		{
			0, 0, 0, 0, 1, 1, 1, 2, 2, 2,
			2, 2, 3, 3, 3, 3
		};

		// Comment out the current implementation:
		// private static readonly int[] MaxImpostors = new int[16]
		// {
		// 	0, 0, 0, 0, 1, 1, 1, 2, 2, 3,
		// 	3, 3, 3, 3, 3, 3
		// };
		// private static readonly int[] MinPlayers = new int[4] { 4, 4, 7, 9 };
		private static readonly int[] MaxImpostors = new int[16]
		{
			0, 0, 0, 0, 1, 4, 5, 5, 5, 5,
			5, 5, 5, 5, 5, 5
		};

		private static readonly float[] KillDistances = new float[3] { 1f, 1.8f, 2.5f };

		private static readonly string[] KillDistanceStrings = new string[3] { "Short", "Normal", "Long" };

		private static readonly int[] MinPlayers = new int[6] { 4, 4, 5, 5, 5, 6 };

		private static readonly RoleTypes[] NonStandardRoles = new RoleTypes[8]
		{
			RoleTypes.Engineer,
			RoleTypes.Scientist,
			RoleTypes.Noisemaker,
			RoleTypes.Phantom,
			RoleTypes.Noisemaker,
			RoleTypes.Shapeshifter,
			RoleTypes.Tracker,
			RoleTypes.GuardianAngel
		};

		public float PlayerSpeedMod = 1f;

		public float CrewLightMod = 1f;

		public float ImpostorLightMod = 1.5f;

		public float KillCooldown = 15f;

		public int NumCommonTasks = 1;

		public int NumLongTasks = 1;

		public int NumShortTasks = 2;

		public int NumEmergencyMeetings = 1;

		public int EmergencyCooldown;

		public bool GhostsDoTasks = true;

		public int KillDistance = 1;

		public int DiscussionTime = 15;

		public int VotingTime = 120;

		public bool ConfirmImpostor = true;

		public bool VisualTasks= true;

		public bool AnonymousVotes;

		public TaskBarMode TaskBarMode;

		private readonly ILogger logger;

		public RoleOptionsCollectionV08 roleOptions= new RoleOptionsCollectionV08();

		public byte Version => 8;

		public GameModes GameMode { get; private set; } = GameModes.Normal;

		public SpecialGameModes SpecialMode { get; set; }

		public GameModes AprilFoolsOnMode => GameModes.NormalFools;

		public GameModes AprilFoolsOffMode => GameModes.Normal;

		public RulesPresets RulesPreset { get; set; }

		public int MaxPlayers { get; set; } = 10;

		public GameKeywords Keywords { get; set; } = GameKeywords.English;

		public byte MapId { get; set; }

		public bool IsDefaults { get; set; } = true;

		public IRoleOptionsCollection RoleOptions => roleOptions;

		public int NumImpostors { get; set; } = 1;

		public int TotalTaskCount => NumCommonTasks + NumLongTasks + NumShortTasks;

		public NormalGameOptionsV08(ILogger logger)
		{
			roleOptions.AddOrUpdateRole(new RoleDataV08(RoleTypes.Shapeshifter));
			roleOptions.AddOrUpdateRole(new RoleDataV08(RoleTypes.Scientist));
			roleOptions.AddOrUpdateRole(new RoleDataV08(RoleTypes.GuardianAngel));
			roleOptions.AddOrUpdateRole(new RoleDataV08(RoleTypes.Engineer));
			roleOptions.AddOrUpdateRole(new RoleDataV08(RoleTypes.Noisemaker));
			roleOptions.AddOrUpdateRole(new RoleDataV08(RoleTypes.Phantom));
			roleOptions.AddOrUpdateRole(new RoleDataV08(RoleTypes.Tracker));
			this.logger = logger;
		}

		public static void Serialize(MessageWriter writer, NormalGameOptionsV08 gameOptions)
		{
			writer.Write((byte)gameOptions.SpecialMode);
			writer.Write((byte)gameOptions.RulesPreset);
			writer.Write((byte)gameOptions.MaxPlayers);
			writer.Write((uint)gameOptions.Keywords);
			writer.Write(gameOptions.MapId);
			writer.Write(gameOptions.PlayerSpeedMod);
			writer.Write(gameOptions.CrewLightMod);
			writer.Write(gameOptions.ImpostorLightMod);
			writer.Write(gameOptions.KillCooldown);
			writer.Write((byte)gameOptions.NumCommonTasks);
			writer.Write((byte)gameOptions.NumLongTasks);
			writer.Write((byte)gameOptions.NumShortTasks);
			writer.Write(gameOptions.NumEmergencyMeetings);
			writer.Write((byte)gameOptions.NumImpostors);
			writer.Write((byte)gameOptions.KillDistance);
			writer.Write(gameOptions.DiscussionTime);
			writer.Write(gameOptions.VotingTime);
			writer.Write(gameOptions.IsDefaults);
			writer.Write((byte)gameOptions.EmergencyCooldown);
			writer.Write(gameOptions.ConfirmImpostor);
			writer.Write(gameOptions.VisualTasks);
			writer.Write(gameOptions.AnonymousVotes);
			writer.Write((byte)gameOptions.TaskBarMode);
			RoleOptionsCollectionV08.Serialize(writer, gameOptions.roleOptions);
		}

		public static NormalGameOptionsV08 Deserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			try
			{
				NormalGameOptionsV08 normalGameOptionsV = new NormalGameOptionsV08(logger);
				if (isAprilFoolsMode)
				{
					normalGameOptionsV.GameMode = normalGameOptionsV.AprilFoolsOnMode;
				}
				normalGameOptionsV.SpecialMode = (SpecialGameModes)reader.ReadByte();
				normalGameOptionsV.RulesPreset = (RulesPresets)reader.ReadByte();
				normalGameOptionsV.MaxPlayers = reader.ReadByte();
				normalGameOptionsV.Keywords = (GameKeywords)reader.ReadUInt32();
				normalGameOptionsV.MapId = reader.ReadByte();
				normalGameOptionsV.PlayerSpeedMod = reader.ReadSingle();
				normalGameOptionsV.CrewLightMod = reader.ReadSingle();
				normalGameOptionsV.ImpostorLightMod = reader.ReadSingle();
				normalGameOptionsV.KillCooldown = reader.ReadSingle();
				normalGameOptionsV.NumCommonTasks = reader.ReadByte();
				normalGameOptionsV.NumLongTasks = reader.ReadByte();
				normalGameOptionsV.NumShortTasks = reader.ReadByte();
				normalGameOptionsV.NumEmergencyMeetings = reader.ReadInt32();
				normalGameOptionsV.NumImpostors = reader.ReadByte();
				normalGameOptionsV.KillDistance = reader.ReadByte();
				normalGameOptionsV.DiscussionTime = reader.ReadInt32();
				normalGameOptionsV.VotingTime = reader.ReadInt32();
				normalGameOptionsV.IsDefaults = reader.ReadBoolean();
				normalGameOptionsV.EmergencyCooldown = reader.ReadByte();
				normalGameOptionsV.ConfirmImpostor = reader.ReadBoolean();
				normalGameOptionsV.VisualTasks = reader.ReadBoolean();
				normalGameOptionsV.AnonymousVotes = reader.ReadBoolean();
				normalGameOptionsV.TaskBarMode = (TaskBarMode)reader.ReadByte();
				normalGameOptionsV.roleOptions.UpdateFrom(RoleOptionsCollectionV08.Deserialize(reader));
				return normalGameOptionsV;
			}
			catch (Exception ex)
			{
				logger.WriteError("[NormalGameOptions] Deserialize() failed. More info in stack:\r\n" + ex.Message + "\r\n" + ex.StackTrace);
			}
			return null;
		}

		public bool AreInvalid(int maxExpectedPlayers)
		{
			// Comment out the current implementation:
			// if (MaxPlayers <= maxExpectedPlayers && NumImpostors >= 1 && NumImpostors <= 3 && KillDistance >= 0 && KillDistance <= 2 && !(PlayerSpeedMod <= 0f))
			// {
			// 	return PlayerSpeedMod > 3f;
			// }
			// return true;
			if (MaxPlayers <= maxExpectedPlayers && NumImpostors >= 1 && NumImpostors <= 5 && KillDistance >= 0 && KillDistance <= 2 && !(PlayerSpeedMod <= 0f))
			{
				return PlayerSpeedMod > 3f;
			}
			return true;
		}

		public void SetRecommendations(int numPlayers, bool isOnline)
		{
			SetRecommendations(numPlayers, isOnline, RulesPresets.Standard);
		}

		public void SetRecommendations(int numPlayers, bool isOnline, RulesPresets rulesPresets)
		{
			if (!isOnline)
			{
				NumImpostors = RecommendedImpostors[numPlayers];
			}
			RulesPreset = rulesPresets;
			ConfirmImpostor = true;
			NumEmergencyMeetings = 1;
			EmergencyCooldown = (isOnline ? 15 : 0);
			DiscussionTime = 15;
			VotingTime = 120;
			AnonymousVotes = false;
			PlayerSpeedMod = 1f;
			CrewLightMod = 1f;
			ImpostorLightMod = 1.5f;
			KillCooldown = RecommendedKillCooldown[numPlayers];
			KillDistance = 1;
			VisualTasks = true;
			TaskBarMode = TaskBarMode.Normal;
			NumCommonTasks = 1;
			NumLongTasks = 1;
			NumShortTasks = 2;
			if (rulesPresets == RulesPresets.StandardRoles)
			{
				roleOptions.SetRoleRate(RoleTypes.Shapeshifter, 1, 100);
				roleOptions.SetRoleRate(RoleTypes.Scientist, 1, 100);
				roleOptions.SetRoleRate(RoleTypes.GuardianAngel, 1, 100);
				roleOptions.SetRoleRate(RoleTypes.Engineer, 1, 100);
				roleOptions.SetRoleRate(RoleTypes.Phantom, 1, 100);
				roleOptions.SetRoleRate(RoleTypes.Tracker, 1, 100);
				roleOptions.SetRoleRate(RoleTypes.Noisemaker, 1, 100);
				roleOptions.SetRoleRecommended(RoleTypes.Shapeshifter);
				roleOptions.SetRoleRecommended(RoleTypes.Scientist);
				roleOptions.SetRoleRecommended(RoleTypes.GuardianAngel);
				roleOptions.SetRoleRecommended(RoleTypes.Engineer);
				roleOptions.SetRoleRecommended(RoleTypes.Phantom);
				roleOptions.SetRoleRecommended(RoleTypes.Tracker);
				roleOptions.SetRoleRecommended(RoleTypes.Noisemaker);
			}
			else
			{
				roleOptions.SetRoleRate(RoleTypes.Shapeshifter, 0, 0);
				roleOptions.SetRoleRate(RoleTypes.Scientist, 0, 0);
				roleOptions.SetRoleRate(RoleTypes.GuardianAngel, 0, 0);
				roleOptions.SetRoleRate(RoleTypes.Engineer, 0, 0);
				roleOptions.SetRoleRate(RoleTypes.Tracker, 0, 0);
				roleOptions.SetRoleRate(RoleTypes.Phantom, 0, 0);
				roleOptions.SetRoleRate(RoleTypes.Noisemaker, 0, 0);
			}
		}

		public void SetByte(ByteOptionNames optionName, byte value)
		{
			if (optionName == ByteOptionNames.MapId)
			{
				MapId = value;
			}
			else
			{
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} could not set byte named {optionName}");
			}
		}

		public void SetFloat(FloatOptionNames optionName, float value)
		{
			switch (optionName)
			{
			case FloatOptionNames.PlayerSpeedMod:
				PlayerSpeedMod = value;
				break;
			case FloatOptionNames.CrewLightMod:
				CrewLightMod = value;
				break;
			case FloatOptionNames.ImpostorLightMod:
				ImpostorLightMod = value;
				break;
			case FloatOptionNames.KillCooldown:
				KillCooldown = value;
				break;
			case FloatOptionNames.ShapeshifterCooldown:
			{
				if (roleOptions.TryGetRoleOptions<ShapeshifterRoleOptionsV08>(RoleTypes.Shapeshifter, out var roleData7))
				{
					roleData7.ShapeshifterCooldown = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Shapeshifter role does not exist");
				}
				break;
			}
			case FloatOptionNames.ShapeshifterDuration:
			{
				if (roleOptions.TryGetRoleOptions<ShapeshifterRoleOptionsV08>(RoleTypes.Shapeshifter, out var roleData14))
				{
					roleData14.ShapeshifterDuration = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Shapeshifter role does not exist");
				}
				break;
			}
			case FloatOptionNames.ScientistBatteryCharge:
			{
				if (roleOptions.TryGetRoleOptions<ScientistRoleOptionsV08>(RoleTypes.Scientist, out var roleData8))
				{
					roleData8.ScientistBatteryCharge = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Scientist role does not exist");
				}
				break;
			}
			case FloatOptionNames.ScientistCooldown:
			{
				if (roleOptions.TryGetRoleOptions<ScientistRoleOptionsV08>(RoleTypes.Scientist, out var roleData3))
				{
					roleData3.ScientistCooldown = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Scientist role does not exist");
				}
				break;
			}
			case FloatOptionNames.GuardianAngelCooldown:
			{
				if (roleOptions.TryGetRoleOptions<GuardianAngelRoleOptionsV08>(RoleTypes.GuardianAngel, out var roleData12))
				{
					roleData12.GuardianAngelCooldown = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Guardian Angel role does not exist");
				}
				break;
			}
			case FloatOptionNames.ProtectionDurationSeconds:
			{
				if (roleOptions.TryGetRoleOptions<GuardianAngelRoleOptionsV08>(RoleTypes.GuardianAngel, out var roleData10))
				{
					roleData10.ProtectionDurationSeconds = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Guardian Angel role does not exist");
				}
				break;
			}
			case FloatOptionNames.EngineerCooldown:
			{
				if (roleOptions.TryGetRoleOptions<EngineerRoleOptionsV08>(RoleTypes.Engineer, out var roleData5))
				{
					roleData5.EngineerCooldown = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Engineer role does not exist");
				}
				break;
			}
			case FloatOptionNames.EngineerInVentMaxTime:
			{
				if (roleOptions.TryGetRoleOptions<EngineerRoleOptionsV08>(RoleTypes.Engineer, out var roleData2))
				{
					roleData2.EngineerInVentMaxTime = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Engineer role does not exist");
				}
				break;
			}
			case FloatOptionNames.PhantomCooldown:
			{
				if (roleOptions.TryGetRoleOptions<PhantomRoleOptionsV08>(RoleTypes.Phantom, out var roleData13))
				{
					roleData13.PhantomCooldown = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Phantom role does not exist");
				}
				break;
			}
			case FloatOptionNames.PhantomDuration:
			{
				if (roleOptions.TryGetRoleOptions<PhantomRoleOptionsV08>(RoleTypes.Phantom, out var roleData11))
				{
					roleData11.PhantomDuration = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Phantom role does not exist");
				}
				break;
			}
			case FloatOptionNames.TrackerCooldown:
			{
				if (roleOptions.TryGetRoleOptions<TrackerRoleOptionsV08>(RoleTypes.Tracker, out var roleData9))
				{
					roleData9.TrackerCooldown = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set options {optionName} because Tracker role does not exist");
				}
				break;
			}
			case FloatOptionNames.TrackerDuration:
			{
				if (roleOptions.TryGetRoleOptions<TrackerRoleOptionsV08>(RoleTypes.Tracker, out var roleData6))
				{
					roleData6.TrackerDuration = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set options {optionName} because Tracker role does not exist");
				}
				break;
			}
			case FloatOptionNames.TrackerDelay:
			{
				if (roleOptions.TryGetRoleOptions<TrackerRoleOptionsV08>(RoleTypes.Tracker, out var roleData4))
				{
					roleData4.TrackerDelay = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set options {optionName} because Tracker role does not exist");
				}
				break;
			}
			case FloatOptionNames.NoisemakerAlertDuration:
			{
				if (roleOptions.TryGetRoleOptions<NoisemakerRoleOptionsV08>(RoleTypes.Noisemaker, out var roleData))
				{
					roleData.NoisemakerAlertDuration = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set options {optionName} because Tracker role does not exist");
				}
				break;
			}
			default:
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} could not set float named {optionName}");
				break;
			}
		}

		public void SetBool(BoolOptionNames optionName, bool value)
		{
			switch (optionName)
			{
			case BoolOptionNames.VisualTasks:
				VisualTasks = value;
				break;
			case BoolOptionNames.ConfirmImpostor:
				ConfirmImpostor = value;
				break;
			case BoolOptionNames.AnonymousVotes:
				AnonymousVotes = value;
				break;
			case BoolOptionNames.IsDefaults:
				IsDefaults = value;
				break;
			case BoolOptionNames.ShapeshifterLeaveSkin:
			{
				if (roleOptions.TryGetRoleOptions<ShapeshifterRoleOptionsV08>(RoleTypes.Shapeshifter, out var roleData3))
				{
					roleData3.ShapeshifterLeaveSkin = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Shapeshifter role does not exist");
				}
				break;
			}
			case BoolOptionNames.ImpostorsCanSeeProtect:
			{
				if (roleOptions.TryGetRoleOptions<GuardianAngelRoleOptionsV08>(RoleTypes.GuardianAngel, out var roleData2))
				{
					roleData2.ImpostorsCanSeeProtect = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Guardian Angel role does not exist");
				}
				break;
			}
			case BoolOptionNames.NoisemakerImpostorAlert:
			{
				if (roleOptions.TryGetRoleOptions<NoisemakerRoleOptionsV08>(RoleTypes.Noisemaker, out var roleData))
				{
					roleData.NoisemakerImpostorAlert = value;
				}
				else
				{
					logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Noisemaker role does not exist");
				}
				break;
			}
			default:
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} could not set bool named {optionName}");
				break;
			}
		}

		public void SetInt(Int32OptionNames optionName, int value)
		{
			switch (optionName)
			{
			case Int32OptionNames.RulePreset:
				RulesPreset = (RulesPresets)value;
				break;
			case Int32OptionNames.KillDistance:
				KillDistance = value;
				break;
			case Int32OptionNames.EmergencyCooldown:
				EmergencyCooldown = value;
				break;
			case Int32OptionNames.DiscussionTime:
				DiscussionTime = value;
				break;
			case Int32OptionNames.VotingTime:
				VotingTime = value;
				break;
			case Int32OptionNames.NumImpostors:
				NumImpostors = value;
				break;
			case Int32OptionNames.NumCommonTasks:
				NumCommonTasks = value;
				break;
			case Int32OptionNames.NumShortTasks:
				NumShortTasks = value;
				break;
			case Int32OptionNames.NumLongTasks:
				NumLongTasks = value;
				break;
			case Int32OptionNames.NumEmergencyMeetings:
				NumEmergencyMeetings = value;
				break;
			case Int32OptionNames.TaskBarMode:
				TaskBarMode = (TaskBarMode)value;
				break;
			case Int32OptionNames.MaxPlayers:
				MaxPlayers = value;
				break;
			default:
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} could not set int named {optionName}");
				break;
			}
		}

		public void SetUInt(UInt32OptionNames optionName, uint value)
		{
			if (optionName == UInt32OptionNames.Keywords)
			{
				Keywords = (GameKeywords)value;
			}
			else
			{
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} could not set uint named {optionName}");
			}
		}

		public byte GetByte(ByteOptionNames optionName)
		{
			if (TryGetByte(optionName, out var value))
			{
				return value;
			}
			return 0;
		}

		public float GetFloat(FloatOptionNames optionName)
		{
			if (TryGetFloat(optionName, out var value))
			{
				return value;
			}
			return 0f;
		}

		public bool GetBool(BoolOptionNames optionName)
		{
			if (TryGetBool(optionName, out var value))
			{
				return value;
			}
			return false;
		}

		public int GetInt(Int32OptionNames optionName)
		{
			if (TryGetInt(optionName, out var value))
			{
				return value;
			}
			return 0;
		}

		public int[] GetIntArray(Int32ArrayOptionNames optionName)
		{
			if (TryGetIntArray(optionName, out var value))
			{
				return value;
			}
			return null;
		}

		public float[] GetFloatArray(FloatArrayOptionNames optionName)
		{
			if (TryGetFloatArray(optionName, out var value))
			{
				return value;
			}
			return null;
		}

		public bool TryGetByte(ByteOptionNames optionName, out byte value)
		{
			value = 0;
			if (optionName == ByteOptionNames.MapId)
			{
				value = MapId;
				return true;
			}
			return false;
		}

		public bool TryGetFloat(FloatOptionNames optionName, out float value)
		{
			value = 0f;
			switch (optionName)
			{
			case FloatOptionNames.KillCooldown:
				value = KillCooldown;
				return true;
			case FloatOptionNames.PlayerSpeedMod:
				value = PlayerSpeedMod;
				return true;
			case FloatOptionNames.ImpostorLightMod:
				value = ImpostorLightMod;
				return true;
			case FloatOptionNames.CrewLightMod:
				value = CrewLightMod;
				return true;
			case FloatOptionNames.ShapeshifterCooldown:
			{
				if (roleOptions.TryGetRoleOptions<ShapeshifterRoleOptionsV08>(RoleTypes.Shapeshifter, out var roleData7))
				{
					value = roleData7.ShapeshifterCooldown;
					return true;
				}
				return false;
			}
			case FloatOptionNames.ShapeshifterDuration:
			{
				if (roleOptions.TryGetRoleOptions<ShapeshifterRoleOptionsV08>(RoleTypes.Shapeshifter, out var roleData14))
				{
					value = roleData14.ShapeshifterDuration;
					return true;
				}
				return false;
			}
			case FloatOptionNames.ScientistCooldown:
			{
				if (roleOptions.TryGetRoleOptions<ScientistRoleOptionsV08>(RoleTypes.Scientist, out var roleData8))
				{
					value = roleData8.ScientistCooldown;
					return true;
				}
				return false;
			}
			case FloatOptionNames.ScientistBatteryCharge:
			{
				if (roleOptions.TryGetRoleOptions<ScientistRoleOptionsV08>(RoleTypes.Scientist, out var roleData3))
				{
					value = roleData3.ScientistBatteryCharge;
					return true;
				}
				return false;
			}
			case FloatOptionNames.ProtectionDurationSeconds:
			{
				if (roleOptions.TryGetRoleOptions<GuardianAngelRoleOptionsV08>(RoleTypes.GuardianAngel, out var roleData12))
				{
					value = roleData12.ProtectionDurationSeconds;
					return true;
				}
				return false;
			}
			case FloatOptionNames.GuardianAngelCooldown:
			{
				if (roleOptions.TryGetRoleOptions<GuardianAngelRoleOptionsV08>(RoleTypes.GuardianAngel, out var roleData10))
				{
					value = roleData10.GuardianAngelCooldown;
					return true;
				}
				return false;
			}
			case FloatOptionNames.EngineerCooldown:
			{
				if (roleOptions.TryGetRoleOptions<EngineerRoleOptionsV08>(RoleTypes.Engineer, out var roleData5))
				{
					value = roleData5.EngineerCooldown;
					return true;
				}
				return false;
			}
			case FloatOptionNames.EngineerInVentMaxTime:
			{
				if (roleOptions.TryGetRoleOptions<EngineerRoleOptionsV08>(RoleTypes.Engineer, out var roleData2))
				{
					value = roleData2.EngineerInVentMaxTime;
					return true;
				}
				return false;
			}
			case FloatOptionNames.PhantomCooldown:
			{
				if (roleOptions.TryGetRoleOptions<PhantomRoleOptionsV08>(RoleTypes.Phantom, out var roleData13))
				{
					value = roleData13.PhantomCooldown;
					return true;
				}
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Phantom role does not exist");
				return false;
			}
			case FloatOptionNames.PhantomDuration:
			{
				if (roleOptions.TryGetRoleOptions<PhantomRoleOptionsV08>(RoleTypes.Phantom, out var roleData11))
				{
					value = roleData11.PhantomDuration;
					return true;
				}
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} can not set option {optionName} because Phantom role does not exist");
				return false;
			}
			case FloatOptionNames.TrackerCooldown:
			{
				if (roleOptions.TryGetRoleOptions<TrackerRoleOptionsV08>(RoleTypes.Tracker, out var roleData9))
				{
					value = roleData9.TrackerCooldown;
					return true;
				}
				return false;
			}
			case FloatOptionNames.TrackerDuration:
			{
				if (roleOptions.TryGetRoleOptions<TrackerRoleOptionsV08>(RoleTypes.Tracker, out var roleData6))
				{
					value = roleData6.TrackerDuration;
					return true;
				}
				return false;
			}
			case FloatOptionNames.TrackerDelay:
			{
				if (roleOptions.TryGetRoleOptions<TrackerRoleOptionsV08>(RoleTypes.Tracker, out var roleData4))
				{
					value = roleData4.TrackerDelay;
					return true;
				}
				return false;
			}
			case FloatOptionNames.NoisemakerAlertDuration:
			{
				if (roleOptions.TryGetRoleOptions<NoisemakerRoleOptionsV08>(RoleTypes.Noisemaker, out var roleData))
				{
					value = roleData.NoisemakerAlertDuration;
					return true;
				}
				return false;
			}
			default:
				return false;
			}
		}

		public bool TryGetBool(BoolOptionNames optionName, out bool value)
		{
			value = false;
			switch (optionName)
			{
			case BoolOptionNames.VisualTasks:
				value = VisualTasks;
				return true;
			case BoolOptionNames.ConfirmImpostor:
				value = ConfirmImpostor;
				return true;
			case BoolOptionNames.AnonymousVotes:
				value = AnonymousVotes;
				return true;
			case BoolOptionNames.GhostsDoTasks:
				value = GhostsDoTasks;
				return true;
			case BoolOptionNames.IsDefaults:
				value = IsDefaults;
				return true;
			case BoolOptionNames.ShapeshifterLeaveSkin:
			{
				if (roleOptions.TryGetRoleOptions<ShapeshifterRoleOptionsV08>(RoleTypes.Shapeshifter, out var roleData))
				{
					value = roleData.ShapeshifterLeaveSkin;
					return true;
				}
				return false;
			}
			case BoolOptionNames.ImpostorsCanSeeProtect:
			{
				if (roleOptions.TryGetRoleOptions<GuardianAngelRoleOptionsV08>(RoleTypes.GuardianAngel, out var roleData3))
				{
					value = roleData3.ImpostorsCanSeeProtect;
					return true;
				}
				return false;
			}
			case BoolOptionNames.NoisemakerImpostorAlert:
			{
				if (roleOptions.TryGetRoleOptions<NoisemakerRoleOptionsV08>(RoleTypes.Noisemaker, out var roleData2))
				{
					value = roleData2.NoisemakerImpostorAlert;
					return true;
				}
				return false;
			}
			case BoolOptionNames.Roles:
			{
				RoleTypes[] nonStandardRoles = NonStandardRoles;
				foreach (RoleTypes role in nonStandardRoles)
				{
					if (roleOptions.GetNumPerGame(role) > 0)
					{
						value = true;
						return true;
					}
				}
				value = false;
				return true;
			}
			default:
				return false;
			}
		}

		public bool TryGetInt(Int32OptionNames optionName, out int value)
		{
			value = 0;
			switch (optionName)
			{
			case Int32OptionNames.RulePreset:
				value = (int)RulesPreset;
				return true;
			case Int32OptionNames.KillDistance:
				value = KillDistance;
				return true;
			case Int32OptionNames.NumEmergencyMeetings:
				value = NumEmergencyMeetings;
				return true;
			case Int32OptionNames.EmergencyCooldown:
				value = EmergencyCooldown;
				return true;
			case Int32OptionNames.DiscussionTime:
				value = DiscussionTime;
				return true;
			case Int32OptionNames.VotingTime:
				value = VotingTime;
				return true;
			case Int32OptionNames.NumCommonTasks:
				value = NumCommonTasks;
				return true;
			case Int32OptionNames.NumShortTasks:
				value = NumShortTasks;
				return true;
			case Int32OptionNames.NumLongTasks:
				value = NumLongTasks;
				return true;
			case Int32OptionNames.NumImpostors:
				value = NumImpostors;
				return true;
			case Int32OptionNames.TaskBarMode:
				value = (int)TaskBarMode;
				return true;
			case Int32OptionNames.MaxPlayers:
				value = MaxPlayers;
				return true;
			default:
				return false;
			}
		}

		public bool TryGetIntArray(Int32ArrayOptionNames optionName, out int[] value)
		{
			value = null;
			switch (optionName)
			{
			case Int32ArrayOptionNames.MaxImpostors:
				value = MaxImpostors;
				return true;
			case Int32ArrayOptionNames.MinPlayers:
				value = MinPlayers;
				return true;
			default:
				return false;
			}
		}

		public bool TryGetFloatArray(FloatArrayOptionNames optionName, out float[] value)
		{
			value = null;
			if (optionName == FloatArrayOptionNames.KillDistances)
			{
				value = KillDistances;
				return true;
			}
			return false;
		}

		public bool TryClearAprilFoolsMode()
		{
			bool result = GameMode == AprilFoolsOnMode;
			GameMode = AprilFoolsOffMode;
			return result;
		}
	}
}
