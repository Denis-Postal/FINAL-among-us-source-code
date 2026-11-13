using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class GameOptionsDefinitionV11 : GameOptionsDefinition
	{
		public override byte Version => 11;

		public override IGameOptionsMigrator GetMigrator(ILogger logger)
		{
			return new GameOptionsMigratorV10toV11(logger);
		}

		public override void NormalModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			NormalGameOptionsV11.Serialize(writer, optionsData as NormalGameOptionsV11);
		}

		public override IGameOptions NormalModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return NormalGameOptionsV11.Deserialize(reader, isAprilFoolsMode, logger);
		}

		public override void HideAndSeekModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			HideNSeekGameOptionsV11.Serialize(writer, optionsData as HideNSeekGameOptionsV11);
		}

		public override IGameOptions HideAndSeekModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return HideNSeekGameOptionsV11.Deserialize(reader, isAprilFoolsMode, logger);
		}
	}
}
