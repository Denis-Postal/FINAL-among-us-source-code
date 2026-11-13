using AmongUs.GameOptions;
using Hazel;

public abstract class GameOptionsDefinition
{
	public abstract byte Version { get; }

	public abstract IGameOptionsMigrator GetMigrator(ILogger logger);

	public abstract void NormalModeSerialize(MessageWriter writer, IGameOptions optionsData);

	public abstract IGameOptions NormalModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger);

	public abstract void HideAndSeekModeSerialize(MessageWriter writer, IGameOptions optionsData);

	public abstract IGameOptions HideAndSeekModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger);
}
