using AmongUs.QuickChat;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSendQuickChatMessage : BaseRpcMessage
	{
		private readonly QuickChatPhraseBuilderResult quickChatData;

		public override RpcCalls RpcType => RpcCalls.SendQuickChat;

		public RpcSendQuickChatMessage(uint rpcObjectNetId, QuickChatPhraseBuilderResult quickChatData) : base(rpcObjectNetId)
		{
			this.quickChatData = quickChatData;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			QuickChatNetData.Serialize(quickChatData, msg);
		}
	}
}
