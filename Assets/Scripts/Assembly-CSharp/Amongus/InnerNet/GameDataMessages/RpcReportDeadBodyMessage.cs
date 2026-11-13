using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcReportDeadBodyMessage : BaseRpcMessage
	{
		private readonly byte targetPlayerId;

		public override RpcCalls RpcType => RpcCalls.ReportDeadBody;

		public RpcReportDeadBodyMessage(uint rpcObjectNetId, byte targetPlayerId) : base(rpcObjectNetId)
		{
			this.targetPlayerId = targetPlayerId;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(targetPlayerId);
		}
	}
}
