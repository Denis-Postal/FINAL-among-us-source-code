using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetHatStrMessage : BaseRpcMessage
	{
		private readonly string hatId;

		private readonly byte rpcSequenceId;

		public override RpcCalls RpcType => RpcCalls.SetHatStr;

		public RpcSetHatStrMessage(uint rpcObjectNetId, string hatId, byte rpcSequenceId) : base(rpcObjectNetId)
		{
			this.hatId = hatId;
			this.rpcSequenceId = rpcSequenceId;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(hatId);
			msg.Write(rpcSequenceId);
		}
	}
}
