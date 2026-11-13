using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetVisorStrMessage : BaseRpcMessage
	{
		private readonly string visorId;

		private readonly byte rpcSequenceId;

		public override RpcCalls RpcType => RpcCalls.SetVisorStr;

		public RpcSetVisorStrMessage(uint rpcObjectNetId, string visorId, byte rpcSequenceId) : base(rpcObjectNetId)
		{
			this.visorId = visorId;
			this.rpcSequenceId = rpcSequenceId;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(visorId);
			msg.Write(rpcSequenceId);
		}
	}
}
