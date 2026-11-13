using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetPetStrMessage : BaseRpcMessage
	{
		private readonly string petId;

		private readonly byte rpcSequenceId;

		public override RpcCalls RpcType => RpcCalls.SetPetStr;

		public RpcSetPetStrMessage(uint rpcObjectNetId, string petId, byte rpcSequenceId) : base(rpcObjectNetId)
		{
			this.petId = petId;
			this.rpcSequenceId = rpcSequenceId;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(petId);
			msg.Write(rpcSequenceId);
		}
	}
}
