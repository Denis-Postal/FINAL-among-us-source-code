using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class GameOptionsDefinitionV08 : GameOptionsDefinition
	{
		public override byte Version => 8;

		public override IGameOptionsMigrator GetMigrator(ILogger logger)
		{
			return new GameOptionsMigratorV07toV08(logger);
		}

		public override void NormalModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			NormalGameOptionsV08.Serialize(writer, optionsData as NormalGameOptionsV08);
		}

		public override IGameOptions NormalModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return NormalGameOptionsV08.Deserialize(reader, isAprilFoolsMode, logger);
		}

		public override void HideAndSeekModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			HideNSeekGameOptionsV08.Serialize(writer, optionsData as HideNSeekGameOptionsV08);
		}

		public override IGameOptions HideAndSeekModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return HideNSeekGameOptionsV08.Deserialize(reader, isAprilFoolsMode, logger);
		}
	}
}
