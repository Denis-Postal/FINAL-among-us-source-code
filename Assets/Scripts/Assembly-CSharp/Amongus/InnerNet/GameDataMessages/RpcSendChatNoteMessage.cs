using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSendChatNoteMessage : BaseRpcMessage
	{
		private readonly byte srcPlayerId;

		private readonly ChatNoteTypes noteType;

		public override RpcCalls RpcType => RpcCalls.SendChatNote;

		public RpcSendChatNoteMessage(uint rpcObjectNetId, byte srcPlayerId, ChatNoteTypes noteType) : base(rpcObjectNetId)
		{
			this.srcPlayerId = srcPlayerId;
			this.noteType = noteType;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(srcPlayerId);
			msg.Write((byte)noteType);
		}
	}
}
