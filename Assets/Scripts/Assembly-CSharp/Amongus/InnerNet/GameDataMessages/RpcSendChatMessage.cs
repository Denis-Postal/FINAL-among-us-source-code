using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSendChatMessage : BaseRpcMessage
	{
		private readonly string chatText;

		public override RpcCalls RpcType => RpcCalls.SendChat;

		public RpcSendChatMessage(uint rpcObjectNetId, string chatText) : base(rpcObjectNetId)
		{
			this.chatText = chatText;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(chatText);
		}
	}
}
