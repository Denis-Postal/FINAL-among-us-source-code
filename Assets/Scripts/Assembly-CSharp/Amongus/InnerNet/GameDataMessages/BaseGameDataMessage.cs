using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public abstract class BaseGameDataMessage : IGameDataMessage
	{
		public abstract GameDataTypes GameDataType { get; }

		public void Serialize(MessageWriter msg)
		{
			msg.StartMessage((byte)GameDataType);
			SerializeValues(msg);
			msg.EndMessage();
		}

		protected abstract void SerializeValues(MessageWriter msg);
	}
}
