using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetNamePlateStrMessage : BaseRpcMessage
	{
		private readonly string namePlateId;

		private readonly byte rpcSequenceId;

		public override RpcCalls RpcType => RpcCalls.SetNamePlateStr;

		public RpcSetNamePlateStrMessage(uint rpcObjectNetId, string namePlateId, byte rpcSequenceId) : base(rpcObjectNetId)
		{
			this.namePlateId = namePlateId;
			this.rpcSequenceId = rpcSequenceId;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(namePlateId);
			msg.Write(rpcSequenceId);
		}
	}
}
