using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public interface IGameDataMessage
	{
		GameDataTypes GameDataType { get; }

		void Serialize(MessageWriter writer);
	}
}
