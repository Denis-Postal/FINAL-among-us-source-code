using Cpp2ILInjected;
using Hazel;

namespace AmongUs.GameOptions
{
	public class GameOptionsDefinitionV12 : GameOptionsDefinition
	{
		public override byte Version => 12;

		public override IGameOptionsMigrator GetMigrator(ILogger logger)
		{
			return new GameOptionsMigratorV11toV12(logger);
		}

		public override void NormalModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			NormalGameOptionsV12.Serialize(writer, optionsData as NormalGameOptionsV12);
		}

		public override IGameOptions NormalModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return NormalGameOptionsV12.Deserialize(reader, isAprilFoolsMode, logger);
		}

		public override void HideAndSeekModeSerialize(MessageWriter writer, IGameOptions optionsData)
		{
			HideNSeekGameOptionsV12.Serialize(writer, optionsData as HideNSeekGameOptionsV12);
		}

		public override IGameOptions HideAndSeekModeDeserialize(MessageReader reader, bool isAprilFoolsMode, ILogger logger)
		{
			return HideNSeekGameOptionsV12.Deserialize(reader, isAprilFoolsMode, logger);
		}
	}
}
