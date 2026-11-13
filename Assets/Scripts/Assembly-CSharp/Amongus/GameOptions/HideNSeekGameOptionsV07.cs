using System;
using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class HideNSeekGameOptionsV07 : IGameOptions
	{
		public static readonly float[] KillDistances = new float[3] { 0.6f, 0.6f, 0.6f };

		// Comment out the current implementation:
		// private static readonly int[] MaxImpostors = new int[16]
		// {
		// 	0, 0, 0, 0, 1, 1, 1, 1, 1, 1,
		// 	1, 1, 1, 1, 1, 1
		// };
		// private static readonly int[] MinPlayers = new int[4] { 4, 4, 7, 9 };
		private static readonly int[] MaxImpostors = new int[16]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12, 13, 14, 15
		};

		private static readonly int[] MinPlayers = new int[16]
		{
			1, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12, 13, 14, 15
		};

		public int ImpostorPlayerID = -1;

		public float PlayerSpeedMod = 1f;

		public float CrewLightMod = 1f;

		public float ImpostorLightMod = 1.5f;

		public float KillCooldown = 1f;

		public int NumCommonTasks = 1;

		public int NumLongTasks = 1;

		public int NumShortTasks = 2;

		public bool GhostsDoTasks = true;

		public int KillDistance;

		public const float ImpostorSpeedDifference = 0.25f;

		public int CrewmateVentUses = 1;

		public float CrewmateTimeInVent = 3f;

		public float EscapeTime = 200f;

		public float FinalCountdownTime = 50f;

		public float CrewmateFlashlightSize = 0.35f;

		public float ImpostorFlashlightSize = 0.25f;

		public bool useFlashlight = true;

		public bool SeekerFinalMap= true;

		public float SeekerFinalSpeed = 1.2f;

		public bool ShowCrewmateNames;

		public float MaxPingTime = 6f;

		public bool SeekerPings = true;

		public const int CrewmateLeadTime = 10;

		public const float ScaryMusicDistance = 55f;

		public const float VeryScaryMusicDistance = 15f;

		public const float CrewmateVentCooldown = 1f;

		public const float ShowPingTime = 2f;

		private readonly ILogger logger;

		private RoleOptionsCollectionV07 roleOptions= new RoleOptionsCollectionV07();

		public byte Version => 7;

		public GameModes GameMode { get; private set; } = GameModes.HideNSeek;

		public SpecialGameModes SpecialMode => SpecialGameModes.None;

		public GameModes AprilFoolsOnMode => GameModes.SeekFools;

		public GameModes AprilFoolsOffMode => GameModes.HideNSeek;

		public RulesPresets RulesPreset { get; set; }

		public int MaxPlayers { get; private set; } = 15;

		public GameKeywords Keywords { get; private set; } = GameKeywords.English;

		public byte MapId { get; private set; }

		public bool IsDefaults { get; private set; } = true;

		public IRoleOptionsCollection RoleOptions => roleOptions;

		// Comment out the current implementation:
		// public int NumImpostors { get; private set; } = 1;
		public int NumImpostors { get; set; } = 1;

		public int TotalTaskCount => NumCommonTasks + NumLongTasks + NumShortTasks;

		public HideNSeekGameOptionsV07(ILogger logger)
		{
			this.logger = logger;
		}

		public static void Serialize(MessageWriter writer, HideNSeekGameOptionsV07 gameOptions)
		{
			writer.Write((byte)gameOptions.MaxPlayers);
			writer.Write((uint)gameOptions.Keywords);
			writer.Write(gameOptions.MapId);
			writer.Write(gameOptions.PlayerSpeedMod);
			writer.Write(gameOptions.CrewLightMod);
			writer.Write(gameOptions.ImpostorLightMod);
			writer.Write((byte)gameOptions.NumCommonTasks);
			writer.Write((byte)gameOptions.NumLongTasks);
			writer.Write((byte)gameOptions.NumShortTasks);
			writer.Write(gameOptions.IsDefaults);
			writer.Write(gameOptions.CrewmateVentUses);
			writer.Write(gameOptions.EscapeTime);
			writer.Write(gameOptions.CrewmateFlashlightSize);
			writer.Write(gameOptions.ImpostorFlashlightSize);
			writer.Write(gameOptions.useFlashlight);
			writer.Write(gameOptions.SeekerFinalMap);
			writer.Write(gameOptions.FinalCountdownTime);
			writer.Write(gameOptions.SeekerFinalSpeed);
			writer.Write(gameOptions.SeekerPings);
			writer.Write(gameOptions.ShowCrewmateNames);
			writer.Write(gameOptions.ImpostorPlayerID);
			writer.Write(gameOptions.MaxPingTime);
			writer.Write(gameOptions.CrewmateTimeInVent);
		}

		public static HideNSeekGameOptionsV07 Deserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			try
			{
				HideNSeekGameOptionsV07 hideNSeekGameOptionsV = new HideNSeekGameOptionsV07(logger);
				if (isAprilFoolsMode)
				{
					hideNSeekGameOptionsV.GameMode = hideNSeekGameOptionsV.AprilFoolsOnMode;
				}
				hideNSeekGameOptionsV.MaxPlayers = reader.ReadByte();
				hideNSeekGameOptionsV.Keywords = (GameKeywords)reader.ReadUInt32();
				hideNSeekGameOptionsV.MapId = reader.ReadByte();
				hideNSeekGameOptionsV.PlayerSpeedMod = reader.ReadSingle();
				hideNSeekGameOptionsV.CrewLightMod = reader.ReadSingle();
				hideNSeekGameOptionsV.ImpostorLightMod = reader.ReadSingle();
				hideNSeekGameOptionsV.NumCommonTasks = reader.ReadByte();
				hideNSeekGameOptionsV.NumLongTasks = reader.ReadByte();
				hideNSeekGameOptionsV.NumShortTasks = reader.ReadByte();
				hideNSeekGameOptionsV.IsDefaults = reader.ReadBoolean();
				hideNSeekGameOptionsV.CrewmateVentUses = reader.ReadInt32();
				hideNSeekGameOptionsV.EscapeTime = reader.ReadSingle();
				hideNSeekGameOptionsV.CrewmateFlashlightSize = reader.ReadSingle();
				hideNSeekGameOptionsV.ImpostorFlashlightSize = reader.ReadSingle();
				hideNSeekGameOptionsV.useFlashlight = reader.ReadBoolean();
				hideNSeekGameOptionsV.SeekerFinalMap = reader.ReadBoolean();
				hideNSeekGameOptionsV.FinalCountdownTime = reader.ReadSingle();
				hideNSeekGameOptionsV.SeekerFinalSpeed = reader.ReadSingle();
				hideNSeekGameOptionsV.SeekerPings = reader.ReadBoolean();
				hideNSeekGameOptionsV.ShowCrewmateNames = reader.ReadBoolean();
				hideNSeekGameOptionsV.ImpostorPlayerID = reader.ReadInt32();
				hideNSeekGameOptionsV.MaxPingTime = reader.ReadSingle();
				hideNSeekGameOptionsV.CrewmateTimeInVent = reader.ReadSingle();
				return hideNSeekGameOptionsV;
			}
			catch
			{
			}
			return null;
		}

		public bool AreInvalid(int maxExpectedPlayers)
		{
			// Comment out the current implementation:
			// if (MaxPlayers <= maxExpectedPlayers && NumImpostors >= 1)
			// {
			// 	return NumImpostors > 3;
			// }
			// return true;
			if (MaxPlayers <= maxExpectedPlayers && NumImpostors >= 1)
			{
				return NumImpostors > 15;
			}
			return true;
		}

		public void SetRecommendations(int numPlayers, bool isOnline)
		{
			PlayerSpeedMod = 1f;
			CrewLightMod = 0.6f;
			ImpostorLightMod = 0.4f;
			NumCommonTasks = 1;
			NumLongTasks = 1;
			NumShortTasks = 2;
			EscapeTime = 200f;
			FinalCountdownTime = 50f;
			CrewmateFlashlightSize = 0.35f;
			ImpostorFlashlightSize = 0.25f;
			CrewmateVentUses = 1;
			CrewmateTimeInVent = 3f;
			useFlashlight = true;
			ImpostorPlayerID = -1;
			SeekerFinalMap = true;
			SeekerFinalSpeed = 1.2f;
			SeekerPings = true;
			ShowCrewmateNames = false;
			MaxPingTime = 6f;
			IsDefaults = true;
		}

		public void SetRecommendations(int numPlayers, bool isOnline, RulesPresets rulesPresets)
		{
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
			case FloatOptionNames.FinalEscapeTime:
				FinalCountdownTime = value;
				break;
			case FloatOptionNames.EscapeTime:
				EscapeTime = value;
				break;
			case FloatOptionNames.SeekerFinalSpeed:
				SeekerFinalSpeed = value;
				break;
			case FloatOptionNames.MaxPingTime:
				MaxPingTime = value;
				break;
			case FloatOptionNames.CrewmateFlashlightSize:
				CrewmateFlashlightSize = value;
				break;
			case FloatOptionNames.ImpostorFlashlightSize:
				ImpostorFlashlightSize = value;
				break;
			case FloatOptionNames.CrewmateTimeInVent:
				CrewmateTimeInVent = value;
				break;
			default:
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} could not set float named {optionName}");
				break;
			}
		}

		public void SetBool(BoolOptionNames optionName, bool value)
		{
			switch (optionName)
			{
			case BoolOptionNames.IsDefaults:
				IsDefaults = value;
				break;
			case BoolOptionNames.UseFlashlight:
				useFlashlight = value;
				break;
			case BoolOptionNames.SeekerFinalMap:
				SeekerFinalMap = value;
				break;
			case BoolOptionNames.SeekerPings:
				SeekerPings = value;
				break;
			case BoolOptionNames.ShowCrewmateNames:
				ShowCrewmateNames = value;
				break;
			default:
				logger.WriteError($"Options for Mode {GameMode}, Version {Version} could not set bool named {optionName}");
				break;
			}
		}

		public void SetInt(Int32OptionNames optionName, int value)
		{
			switch (optionName)
			{
			case Int32OptionNames.KillDistance:
				KillDistance = value;
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
			case Int32OptionNames.MaxPlayers:
				MaxPlayers = value;
				break;
			case Int32OptionNames.CrewmateVentUses:
				CrewmateVentUses = value;
				break;
			case Int32OptionNames.ImpostorPlayerID:
				ImpostorPlayerID = value;
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
			case FloatOptionNames.CrewmateTimeInVent:
				value = CrewmateTimeInVent;
				return true;
			case FloatOptionNames.FinalEscapeTime:
				value = FinalCountdownTime;
				return true;
			case FloatOptionNames.EscapeTime:
				value = EscapeTime;
				return true;
			case FloatOptionNames.SeekerFinalSpeed:
				value = SeekerFinalSpeed;
				return true;
			case FloatOptionNames.MaxPingTime:
				value = MaxPingTime;
				return true;
			case FloatOptionNames.CrewmateFlashlightSize:
				value = CrewmateFlashlightSize;
				return true;
			case FloatOptionNames.ImpostorFlashlightSize:
				value = ImpostorFlashlightSize;
				return true;
			default:
				return false;
			}
		}

		public bool TryGetBool(BoolOptionNames optionName, out bool value)
		{
			value = false;
			switch (optionName)
			{
			case BoolOptionNames.GhostsDoTasks:
				value = GhostsDoTasks;
				return true;
			case BoolOptionNames.UseFlashlight:
				value = useFlashlight;
				return true;
			case BoolOptionNames.IsDefaults:
				value = IsDefaults;
				return true;
			case BoolOptionNames.SeekerFinalMap:
				value = SeekerFinalMap;
				return true;
			case BoolOptionNames.SeekerPings:
				value = SeekerPings;
				return true;
			case BoolOptionNames.ShowCrewmateNames:
				value = ShowCrewmateNames;
				return true;
			default:
				return false;
			}
		}

		public bool TryGetInt(Int32OptionNames optionName, out int value)
		{
			value = 0;
			switch (optionName)
			{
			case Int32OptionNames.KillDistance:
				value = KillDistance;
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
			case Int32OptionNames.CrewmateVentUses:
				value = CrewmateVentUses;
				return true;
			case Int32OptionNames.ImpostorPlayerID:
				value = ImpostorPlayerID;
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
