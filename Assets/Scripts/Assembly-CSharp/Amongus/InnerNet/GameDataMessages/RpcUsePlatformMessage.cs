using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcUsePlatformMessage : BaseRpcMessage
	{
		public override RpcCalls RpcType => RpcCalls.UsePlatform;

		public RpcUsePlatformMessage(uint rpcObjectNetId) : base(rpcObjectNetId)
		{
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
		}
	}
}
