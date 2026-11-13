using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class DespawnGameDataMessage : BaseGameDataMessage
	{
		private readonly uint objToDespawnNetId;

		public override GameDataTypes GameDataType { get; } = GameDataTypes.DespawnFlag;

		public DespawnGameDataMessage(uint objToDespawnNetId)
		{
			this.objToDespawnNetId = objToDespawnNetId;
		}

		protected override void SerializeValues(MessageWriter msg)
		{
			msg.WritePacked(objToDespawnNetId);
		}
	}
}
