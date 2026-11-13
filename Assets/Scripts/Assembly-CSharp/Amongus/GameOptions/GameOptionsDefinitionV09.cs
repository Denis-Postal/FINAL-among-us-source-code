using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class GameOptionsDefinitionV09 : GameOptionsDefinition
	{
		public override byte Version => 9;

		public override IGameOptionsMigrator GetMigrator(ILogger logger)
		{
			return new GameOptionsMigratorV08toV09(logger);
		}

		public override void NormalModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			NormalGameOptionsV09.Serialize(writer, optionsData as NormalGameOptionsV09);
		}

		public override IGameOptions NormalModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return NormalGameOptionsV09.Deserialize(reader, isAprilFoolsMode, logger);
		}

		public override void HideAndSeekModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			HideNSeekGameOptionsV09.Serialize(writer, optionsData as HideNSeekGameOptionsV09);
		}

		public override IGameOptions HideAndSeekModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return HideNSeekGameOptionsV09.Deserialize(reader, isAprilFoolsMode, logger);
		}
	}
}
