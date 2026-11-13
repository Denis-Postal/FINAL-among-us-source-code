using System;
using System.IO;
using AmongUs.Data;
using AmongUs.Data.Settings;
using AmongUs.GameOptions;
using Cpp2ILInjected;
using Hazel;

public class GameOptionsManager
{
	public readonly GameOptionsFactory gameOptionsFactory;

	public readonly ILogger logger;

	private GameModes currentGameMode;

	public IGameOptions currentGameOptions;

	private NormalGameOptionsV12 currentNormalGameOptions;

	private HideNSeekGameOptionsV12 currentHideNSeekGameOptions;

	public IGameOptions currentHostOptions;

	private NormalGameOptionsV12 normalGameHostOptions;

	private HideNSeekGameOptionsV12 hideNSeekGameHostOptions;

	public IGameOptions currentSearchOptions;

	private NormalGameOptionsV12 normalGameSearchOptions;

	private HideNSeekGameOptionsV12 hideNSeekGameSearchOptions;

	public static GameOptionsManager Instance { get; private set; }

	public IGameOptions CurrentGameOptions
	{
		get
		{
			if (currentGameOptions == null)
			{
				CurrentGameOptions = GameHostOptions;
			}
			return currentGameOptions;
		}
		set
		{
			GameModes gameMode = value.GameMode;
			SwitchGameMode(gameMode);
			switch (gameMode)
			{
			case GameModes.Normal:
			case GameModes.NormalFools:
				currentNormalGameOptions = value as NormalGameOptionsV12;
				if (currentNormalGameOptions == null)
				{
					logger.WriteError($"Setting current game mode as {gameMode} but options data type is not of type {typeof(NormalGameOptionsV12)}");
				}
				break;
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				currentHideNSeekGameOptions = value as HideNSeekGameOptionsV12;
				if (currentHideNSeekGameOptions == null)
				{
					logger.WriteError($"Setting current game mode as {gameMode} but options data type is not of type {typeof(HideNSeekGameOptionsV12)}");
				}
				break;
			default:
				logger.WriteError($"Cannot cache settings for game mode {gameMode}");
				break;
			}
			currentGameOptions = value;
		}
	}

	public IGameOptions GameHostOptions
	{
		get
		{
			return currentHostOptions;
		}
		set
		{
			GameModes gameMode = value.GameMode;
			SwitchGameMode(gameMode);
			switch (gameMode)
			{
			case GameModes.Normal:
			case GameModes.NormalFools:
				normalGameHostOptions = value as NormalGameOptionsV12;
				if (normalGameHostOptions == null)
				{
					logger.WriteError($"Setting host options game mode as {gameMode} but options data type is not of type {typeof(NormalGameOptionsV12)}");
				}
				else
				{
					SaveNormalHostOptions();
				}
				break;
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				hideNSeekGameHostOptions = value as HideNSeekGameOptionsV12;
				if (hideNSeekGameHostOptions == null)
				{
					logger.WriteError($"Setting host options game mode as {gameMode} but options data type is not of type {typeof(HideNSeekGameOptionsV12)}");
				}
				else
				{
					SaveHideNSeekHostOptions();
				}
				break;
			default:
				logger.WriteError($"Cannot cache settings for host options game mode {gameMode}");
				break;
			}
			currentHostOptions = value;
		}
	}

	public IGameOptions GameSearchOptions
	{
		get
		{
			return currentSearchOptions;
		}
		set
		{
			GameModes gameMode = value.GameMode;
			SwitchGameMode(gameMode);
			switch (gameMode)
			{
			case GameModes.Normal:
			case GameModes.NormalFools:
				normalGameSearchOptions = value as NormalGameOptionsV12;
				if (normalGameSearchOptions == null)
				{
					logger.WriteError($"Setting search options game mode as {gameMode} but options data type is not of type {typeof(NormalGameOptionsV12)}");
				}
				else
				{
					SaveNormalSearchOptions();
				}
				break;
			case GameModes.HideNSeek:
			case GameModes.SeekFools:
				hideNSeekGameSearchOptions = value as HideNSeekGameOptionsV12;
				if (hideNSeekGameSearchOptions == null)
				{
					logger.WriteError($"Setting search options game mode as {gameMode} but options data type is not of type {typeof(HideNSeekGameOptionsV12)}");
				}
				else
				{
					SaveHideNSeekSearchOptions();
				}
				break;
			default:
				logger.WriteError($"Cannot cache settings for search options game mode {gameMode}");
				break;
			}
			currentSearchOptions = value;
		}
	}

	public bool HasOptions => currentGameOptions != null;

	public Type GetGameOptions()
	{
		return typeof(NormalGameOptionsV12);
	}

	public GameOptionsManager(ILogger logger)
	{
		if (Instance != null)
		{
			throw new Exception("CANNOT HAVE MORE THAN ONE OPTIONS MANAGER INSTANCE!");
		}
		Instance = this;
		gameOptionsFactory = new GameOptionsFactory(logger);
		this.logger = logger;
	}

	public void Initialize()
	{
		LoadOrCreateNormalGameHostOptions();
		LoadOrCreateNormalGameSearchOptions();
		LoadOrCreateHideNSeekGameHostOptions();
		LoadOrCreateHideNSeekGameSearchOptions();
		if (normalGameHostOptions == null)
		{
			logger.WriteError("Normal host options were incompatible with the current game options type. Restoring defaults.");
			normalGameHostOptions = new NormalGameOptionsV12(logger);
			SaveNormalHostOptions();
		}
		if (hideNSeekGameHostOptions == null)
		{
			logger.WriteError("Hide and Seek host options were incompatible with the current game options type. Restoring defaults.");
			hideNSeekGameHostOptions = new HideNSeekGameOptionsV12(logger);
			SaveHideNSeekHostOptions();
		}
		SwitchGameMode(GameModes.Normal);
	}

	public void SwitchGameMode(GameModes gameMode)
	{
		switch (gameMode)
		{
		case GameModes.Normal:
		case GameModes.NormalFools:
			currentHostOptions = normalGameHostOptions;
			currentSearchOptions = normalGameSearchOptions;
			currentGameOptions = currentNormalGameOptions;
			break;
		case GameModes.HideNSeek:
		case GameModes.SeekFools:
			currentHostOptions = hideNSeekGameHostOptions;
			currentSearchOptions = hideNSeekGameSearchOptions;
			currentGameOptions = currentHideNSeekGameOptions;
			break;
		default:
			logger.WriteError($"Cannot switch game mode options to {gameMode} - mode does not exist");
			return;
		}
		currentGameMode = gameMode;
		logger.WriteInfo($"Game options game mode switched to {gameMode}");
	}

	private void LoadOrCreateNormalGameHostOptions()
	{
		byte[] rawNormalHostOptions = DataManager.Settings.Multiplayer.RawNormalHostOptions;
		IGameOptions legacyGameOptions = null;
		if (rawNormalHostOptions == null || rawNormalHostOptions.Length == 0)
		{
			if (!TryLoadLegacyOptionsFile("gameHostOptions", out legacyGameOptions))
			{
				logger.WriteInfo("NormalGameOptions_Current HOST data was not found. Initializing with default values");
				normalGameHostOptions = new NormalGameOptionsV12(logger);
				SaveNormalHostOptions();
				return;
			}
			logger.WriteInfo(string.Format("LEGACY {0} HOST data was found. Migrating from legacy to [V{1}]", "NormalGameOptions_Current", GameOptionsFactory.LatestVersion));
		}
		else
		{
			legacyGameOptions = gameOptionsFactory.FromBytes(rawNormalHostOptions);
		}
		if (legacyGameOptions == null)
		{
			logger.WriteError("NormalGameOptions_Current HOST Data was null after trying to deserialize - Initializing with default values. This should not be happening, it is a fallback to prevent crashes on startup.");
			normalGameHostOptions = new NormalGameOptionsV12(logger);
			SaveNormalHostOptions();
		}
		else if (legacyGameOptions.Version < GameOptionsFactory.LatestVersion)
		{
			normalGameHostOptions = MigrateNormalGameOptions(legacyGameOptions);
			SaveNormalHostOptions();
		}
		else
		{
			normalGameHostOptions = legacyGameOptions as NormalGameOptionsV12;
		}
	}

	private void LoadOrCreateHideNSeekGameHostOptions()
	{
		byte[] rawHideNSeekHostOptions = DataManager.Settings.Multiplayer.RawHideNSeekHostOptions;
		if (rawHideNSeekHostOptions == null || rawHideNSeekHostOptions.Length == 0)
		{
			logger.WriteInfo("HideNSeekGameOptions_Current HOST data was not found. Initializing with default values");
			hideNSeekGameHostOptions = new HideNSeekGameOptionsV12(logger);
			SaveHideNSeekHostOptions();
			return;
		}
		IGameOptions gameOptions = gameOptionsFactory.FromBytes(rawHideNSeekHostOptions);
		if (gameOptions == null)
		{
			logger.WriteError("HideNSeekGameOptions_Current HOST data was null after trying to deserialize - Initializing with default values. This should not be happening, it is a fallback to prevent crashes on startup.");
			hideNSeekGameHostOptions = new HideNSeekGameOptionsV12(logger);
			SaveHideNSeekHostOptions();
		}
		else if (gameOptions.Version < GameOptionsFactory.LatestVersion)
		{
			hideNSeekGameHostOptions = MigrateHideNSeekGameOptions(gameOptions);
			SaveHideNSeekHostOptions();
		}
		else
		{
			hideNSeekGameHostOptions = gameOptions as HideNSeekGameOptionsV12;
		}
	}

	private void LoadOrCreateNormalGameSearchOptions()
	{
		byte[] rawNormalSearchOptions = DataManager.Settings.Multiplayer.RawNormalSearchOptions;
		IGameOptions legacyGameOptions;
		if (rawNormalSearchOptions == null || rawNormalSearchOptions.Length == 0)
		{
			if (!TryLoadLegacyOptionsFile("gameSearchOptions", out legacyGameOptions))
			{
				logger.WriteInfo("NormalGameOptions_Current SEARCH data was not found. Initializing with default values");
				normalGameSearchOptions = new NormalGameOptionsV12(logger);
				SaveNormalSearchOptions();
				return;
			}
			logger.WriteInfo(string.Format("LEGACY {0} SEARCH data was found. Migrating from legacy to [V{1}]", "NormalGameOptions_Current", GameOptionsFactory.LatestVersion));
		}
		else
		{
			legacyGameOptions = gameOptionsFactory.FromBytes(rawNormalSearchOptions);
		}
		if (legacyGameOptions == null)
		{
			logger.WriteError("NormalGameOptions_Current SEARCH Data was null after trying to deserialize - Initializing with default values. This should not be happening, it is a fallback to prevent crashes on startup.");
			normalGameSearchOptions = new NormalGameOptionsV12(logger);
			SaveNormalSearchOptions();
		}
		else if (legacyGameOptions.Version < GameOptionsFactory.LatestVersion)
		{
			normalGameSearchOptions = MigrateNormalGameOptions(legacyGameOptions);
			SaveNormalSearchOptions();
		}
		else
		{
			normalGameSearchOptions = legacyGameOptions as NormalGameOptionsV12;
		}
	}

	private void LoadOrCreateHideNSeekGameSearchOptions()
	{
		byte[] rawHideNSeekSearchOptions = DataManager.Settings.Multiplayer.RawHideNSeekSearchOptions;
		if (rawHideNSeekSearchOptions == null || rawHideNSeekSearchOptions.Length == 0)
		{
			logger.WriteInfo("HideNSeekGameOptions_Current SEARCH data was not found. Initializing with default values");
			hideNSeekGameSearchOptions = new HideNSeekGameOptionsV12(logger);
			SaveHideNSeekSearchOptions();
			return;
		}
		IGameOptions gameOptions = gameOptionsFactory.FromBytes(rawHideNSeekSearchOptions);
		if (gameOptions == null)
		{
			logger.WriteError("HideNSeekGameOptions_Current SEARCH data was null after trying to deserialize - Initializing with default values. This should not be happening, it is a fallback to prevent crashes on startup.");
			hideNSeekGameSearchOptions = new HideNSeekGameOptionsV12(logger);
			SaveHideNSeekSearchOptions();
		}
		else if (gameOptions.Version < GameOptionsFactory.LatestVersion)
		{
			hideNSeekGameSearchOptions = MigrateHideNSeekGameOptions(gameOptions);
			SaveHideNSeekSearchOptions();
		}
		else
		{
			hideNSeekGameSearchOptions = gameOptions as HideNSeekGameOptionsV12;
		}
	}

	private unsafe bool TryLoadLegacyOptionsFile(string filename, out IGameOptions legacyGameOptions)
	{
		legacyGameOptions = null;
		string path = Path.Combine(PlatformPaths.persistentDataPath, filename);
		if (!File.Exists(path))
		{
			logger.WriteInfo("No legacy game options " + filename + " options found. Skipping legacy migration.");
			return false;
		}
		using (FileStream input = File.OpenRead(path))
		{
			using BinaryReader reader = new BinaryReader(input);
			legacyGameOptions = LegacyGameOptions.Deserialize(reader, 6, logger);
		}
		File.Delete(path);
		return true;
	}

	private void SaveNormalHostOptions()
	{
		byte[] rawNormalHostOptions = gameOptionsFactory.ToBytes(normalGameHostOptions, forceAprilFoolsMode: false);
		DataManager.Settings.Multiplayer.RawNormalHostOptions = rawNormalHostOptions;
		DataManager.Settings.Save();
		logger.WriteInfo($"Saved host options for game mode {normalGameHostOptions.GameMode} with version {normalGameHostOptions.Version}");
	}

	private void SaveNormalSearchOptions()
	{
		byte[] rawNormalSearchOptions = gameOptionsFactory.ToBytes(normalGameSearchOptions, forceAprilFoolsMode: false);
		DataManager.Settings.Multiplayer.RawNormalSearchOptions = rawNormalSearchOptions;
		DataManager.Settings.Save();
		logger.WriteInfo($"Saved search options for game mode {normalGameSearchOptions.GameMode} with version {normalGameSearchOptions.Version}");
	}

	private void SaveHideNSeekHostOptions()
	{
		byte[] rawHideNSeekHostOptions = gameOptionsFactory.ToBytes(hideNSeekGameHostOptions, forceAprilFoolsMode: false);
		DataManager.Settings.Multiplayer.RawHideNSeekHostOptions = rawHideNSeekHostOptions;
		DataManager.Settings.Save();
		logger.WriteInfo($"Saved host options for game mode {hideNSeekGameHostOptions.GameMode} with version {hideNSeekGameHostOptions.Version}");
	}

	private void SaveHideNSeekSearchOptions()
	{
		byte[] rawHideNSeekSearchOptions = gameOptionsFactory.ToBytes(hideNSeekGameSearchOptions, forceAprilFoolsMode: false);
		DataManager.Settings.Multiplayer.RawHideNSeekSearchOptions = rawHideNSeekSearchOptions;
		DataManager.Settings.Save();
		logger.WriteInfo($"Saved search options for game mode {hideNSeekGameSearchOptions.GameMode} with version {hideNSeekGameSearchOptions.Version}");
	}

	private unsafe NormalGameOptionsV12 MigrateNormalGameOptions(IGameOptions gameOptions)
	{
		logger.WriteInfo($"Migrating game options for mode {gameOptions.GameMode}");
		while (gameOptions.Version != GameOptionsFactory.LatestVersion)
		{
			logger.WriteInfo($"Attempting to migrating game options for mode {gameOptions.GameMode} from version {gameOptions.Version} to {gameOptions.Version + 1}");
			if (!gameOptionsFactory.TryGetMigrator(gameOptions.Version, out var migrator))
			{
				logger.WriteError($"Failed to migrating game options for mode {gameOptions.GameMode} from version {gameOptions.Version} to {gameOptions.Version + 1} - no migrator available");
				return new NormalGameOptionsV12(logger);
			}
			if (!migrator.TryMigrate(gameOptions, out var updatedVersion))
			{
				logger.WriteError($"Failed to migrating game options for mode {gameOptions.GameMode} from version {gameOptions.Version} to {gameOptions.Version + 1} - conversion failed");
				return new NormalGameOptionsV12(logger);
			}
			gameOptions = updatedVersion;
		}
		return gameOptions as NormalGameOptionsV12;
	}

	private unsafe HideNSeekGameOptionsV12 MigrateHideNSeekGameOptions(IGameOptions gameOptions)
	{
		logger.WriteInfo($"Migrating game options for mode {gameOptions.GameMode}");
		while (gameOptions.Version != GameOptionsFactory.LatestVersion)
		{
			logger.WriteInfo($"Attempting to migrating game options for mode {gameOptions.GameMode} from version {gameOptions.Version} to {gameOptions.Version + 1}");
			if (!gameOptionsFactory.TryGetMigrator(gameOptions.Version, out var migrator))
			{
				logger.WriteError($"Failed to migrating game options for mode {gameOptions.GameMode} from version {gameOptions.Version} to {gameOptions.Version + 1} - no migrator available");
				return new HideNSeekGameOptionsV12(logger);
			}
			if (!migrator.TryMigrate(gameOptions, out var updatedVersion))
			{
				logger.WriteError($"Failed to migrating game options for mode {gameOptions.GameMode} from version {gameOptions.Version} to {gameOptions.Version + 1} - conversion failed");
				return new HideNSeekGameOptionsV12(logger);
			}
			gameOptions = updatedVersion;
		}
		return gameOptions as HideNSeekGameOptionsV12;
	}
}
