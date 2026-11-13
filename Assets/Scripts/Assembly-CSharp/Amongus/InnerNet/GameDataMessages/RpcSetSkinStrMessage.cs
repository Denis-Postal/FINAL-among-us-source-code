using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetSkinStrMessage : BaseRpcMessage
	{
		private readonly string skinId;

		private readonly byte rpcSequenceId;

		public override RpcCalls RpcType => RpcCalls.SetSkinStr;

		public RpcSetSkinStrMessage(uint rpcObjectNetId, string skinId, byte rpcSequenceId) : base(rpcObjectNetId)
		{
			this.skinId = skinId;
			this.rpcSequenceId = rpcSequenceId;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(skinId);
			msg.Write(rpcSequenceId);
		}
	}
}
