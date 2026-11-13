using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcCloseMeetingMessage : BaseRpcMessage
	{
		public override RpcCalls RpcType => RpcCalls.CloseMeeting;

		public RpcCloseMeetingMessage(uint rpcObjectNetId) : base(rpcObjectNetId)
		{
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
		}
	}
}
