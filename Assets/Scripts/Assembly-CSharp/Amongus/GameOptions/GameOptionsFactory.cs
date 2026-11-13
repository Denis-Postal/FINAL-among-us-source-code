using System;
using System.Collections.Generic;
using System.IO;
using Hazel;

namespace AmongUs.GameOptions
{
	public class GameOptionsFactory
	{
		public const byte FIRST_MODULAR_OPTIONS_DATA_VERSION = 7;

		public static readonly int LatestVersion;

		private static readonly Dictionary<int, GameOptionsDefinition> cachedVersionDefinitions;

		private readonly ILogger logger;

		private readonly Dictionary<byte, IGameOptionsMigrator> migrators;

		static GameOptionsFactory()
		{
			GameOptionsDefinition[] array = new GameOptionsDefinition[6]
			{
				new GameOptionsDefinitionV07(),
				new GameOptionsDefinitionV08(),
				new GameOptionsDefinitionV09(),
				new GameOptionsDefinitionV10(),
				new GameOptionsDefinitionV11(),
				new GameOptionsDefinitionV12()
			};
			LatestVersion = -1;
			cachedVersionDefinitions = new Dictionary<int, GameOptionsDefinition>();
			GameOptionsDefinition[] array2 = array;
			foreach (GameOptionsDefinition gameOptionsDefinition in array2)
			{
				cachedVersionDefinitions.Add(gameOptionsDefinition.Version, gameOptionsDefinition);
				if (gameOptionsDefinition.Version > LatestVersion)
				{
					LatestVersion = gameOptionsDefinition.Version;
				}
			}
		}

		public GameOptionsFactory(ILogger logger)
		{
			this.logger = logger;
			migrators = new Dictionary<byte, IGameOptionsMigrator>();
			foreach (GameOptionsDefinition value in cachedVersionDefinitions.Values)
			{
				logger.WriteInfo($"GameOptionsDefinition found for: V{value.Version}");
				if (value.GetMigrator(logger) == null)
				{
					if (value.Version >= 7)
					{
						logger.WriteError(string.Format("[{0}] Migration setup failed. Version '{1}' returned a null migrator.", "GameOptionsFactory", value.Version));
					}
				}
				else
				{
					migrators.Add((byte)(value.Version - 1), value.GetMigrator(logger));
				}
			}
			if (LatestVersion < 7)
			{
				logger.WriteError(string.Format("[{0}] Initialization failed. Latest version could not be determined (Got '{1}') (Expected >= '{2}')", "GameOptionsFactory", LatestVersion, (byte)7));
			}
		}

		public byte[] ToBytes(IGameOptions data, bool forceAprilFoolsMode)
		{
			byte version = data.Version;
			if (version < 7)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
					if (!(data is LegacyGameOptions gameOptionsData))
					{
						logger.WriteError($"Could not serialize LEGACY game options data for version {version}");
						return null;
					}
					LegacyGameOptions.Serialize(binaryWriter, gameOptionsData);
					binaryWriter.Flush();
					memoryStream.Position = 0L;
					return memoryStream.ToArray();
				}
			}
			GameModes gameModes = data.GameMode;
			if (forceAprilFoolsMode)
			{
				gameModes = data.AprilFoolsOnMode;
			}
			MessageWriter messageWriter = MessageWriter.Get();
			messageWriter.Write(data.Version);
			messageWriter.StartMessage(0);
			messageWriter.Write((byte)gameModes);
			if (!cachedVersionDefinitions.TryGetValue(data.Version, out var value))
			{
				logger.WriteError($"Could not serialize '{gameModes}' options data for version {data.Version}: GameOptionsDefinition for Version {data.Version} (and associated Serializer) could not be retrieved.");
			}
			switch (gameModes)
			{
			case GameModes.Normal:
			case GameModes.NormalFools:
				value?.NormalModeSerialize(messageWriter, data);
				break;
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				value?.HideAndSeekModeSerialize(messageWriter, data);
				break;
			}
			messageWriter.EndMessage();
			byte[] result = messageWriter.ToByteArray(includeHeader: false);
			messageWriter.Recycle();
			return result;
		}

		public void ToNetworkMessageWithSize(MessageWriter writer, IGameOptions data)
		{
			byte[] bytes = ToBytes(data, forceAprilFoolsMode: false);
			writer.WriteBytesAndSize(bytes);
		}

		public IGameOptions FromBytes(byte[] rawBytes)
		{
			int num = rawBytes.Length;
			int num2 = 0;
			MessageReader sized = MessageReader.GetSized(num);
			sized.Length = num;
			rawBytes.CopyTo(sized.Buffer, 0);
			sized.Offset = num2;
			sized.Length = num - num2;
			sized.Position = 0;
			byte version = sized.ReadByte();
			IGameOptions result = DeserializeByVersion(sized, version);
			sized.Recycle();
			return result;
		}

		public IGameOptions FromNetworkMessageWithSize(MessageReader reader)
		{
			int num = (int)reader.ReadPackedUInt32();
			byte b = reader.ReadByte();
			IGameOptions result = DeserializeByVersion(reader, b);
			if (b < 7)
			{
				reader.Position = reader.Position + num - 1;
			}
			return result;
		}

		public bool TryGetMigrator(byte version, out IGameOptionsMigrator migrator)
		{
			return migrators.TryGetValue(version, out migrator);
		}

		public IGameOptions DeserializeByVersion(MessageReader reader, byte version)
		{
			if (version < 7)
			{
				using (MemoryStream input = new MemoryStream(reader.Buffer, reader.Position + reader.Offset - 1, reader.BytesRemaining + 1))
				{
					using BinaryReader reader2 = new BinaryReader(input);
					return LegacyGameOptions.Deserialize(reader2, version, logger);
				}
			}
			return DeserializeModularByVersion(reader, version);
		}

		private IGameOptions DeserializeModularByVersion(MessageReader reader, byte version)
		{
			MessageReader messageReader = reader.ReadMessage();
			GameModes gameModes = (GameModes)messageReader.ReadByte();
			logger.WriteVerbose(string.Format("[{0}] Reading {1} FromInternalMessage for mode [{2}] version [{3}]", "GameOptionsFactory", "IGameOptions", gameModes, version));
			bool isAprilFoolsMode = gameModes == GameModes.NormalFools || gameModes == GameModes.SeekFools;
			if (!cachedVersionDefinitions.TryGetValue(version, out var value) || value == null)
			{
				logger.WriteError(string.Format("[{0}] Could not parse '{1}' options data for version '{2}': GameOptionsDefinition for Version {3} (and associated Deserializer) could not be retrieved.", "GameOptionsFactory", gameModes, version, version));
			}
			switch (gameModes)
			{
			default:
				logger.WriteError(string.Format("[{0}] Could not parse options data for game mode '{1}': Type '{2}' was not configured.", "GameOptionsFactory", gameModes, gameModes));
				return null;
			case GameModes.Normal:
			case GameModes.NormalFools:
				return value?.NormalModeDeserialize(messageReader, isAprilFoolsMode, logger);
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				return value?.HideAndSeekModeDeserialize(messageReader, isAprilFoolsMode, logger);
			}
		}
	}
}
