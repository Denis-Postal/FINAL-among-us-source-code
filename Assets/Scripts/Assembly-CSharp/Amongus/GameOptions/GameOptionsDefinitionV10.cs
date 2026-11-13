using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class GameOptionsDefinitionV10 : GameOptionsDefinition
	{
		public override byte Version => 10;

		public override IGameOptionsMigrator GetMigrator(ILogger logger)
		{
			return new GameOptionsMigratorV09toV10(logger);
		}

		public override void NormalModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			NormalGameOptionsV10.Serialize(writer, optionsData as NormalGameOptionsV10);
		}

		public override IGameOptions NormalModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return NormalGameOptionsV10.Deserialize(reader, isAprilFoolsMode, logger);
		}

		public override void HideAndSeekModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			HideNSeekGameOptionsV10.Serialize(writer, optionsData as HideNSeekGameOptionsV10);
		}

		public override IGameOptions HideAndSeekModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return HideNSeekGameOptionsV10.Deserialize(reader, isAprilFoolsMode, logger);
		}
	}
}
