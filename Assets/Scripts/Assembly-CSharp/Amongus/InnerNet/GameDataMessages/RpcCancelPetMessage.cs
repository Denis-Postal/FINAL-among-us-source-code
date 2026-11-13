using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcCancelPetMessage : BaseRpcMessage
	{
		public override RpcCalls RpcType => RpcCalls.CancelPet;

		public RpcCancelPetMessage(uint rpcObjectNetId) : base(rpcObjectNetId)
		{
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
		}
	}
}
