using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcClimbLadderMessage : BaseRpcMessage
	{
		private readonly byte ladderId;

		private readonly byte lastClimbLadderSid;

		public override RpcCalls RpcType => RpcCalls.ClimbLadder;

		public RpcClimbLadderMessage(uint rpcObjectNetId, byte ladderId, byte lastClimbLadderSid) : base(rpcObjectNetId)
		{
			this.ladderId = ladderId;
			this.lastClimbLadderSid = lastClimbLadderSid;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(ladderId);
			msg.Write(lastClimbLadderSid);
		}
	}
}
