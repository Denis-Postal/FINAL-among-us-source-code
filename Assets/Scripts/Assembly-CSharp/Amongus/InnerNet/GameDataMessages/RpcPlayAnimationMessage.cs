using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcPlayAnimationMessage : BaseRpcMessage
	{
		private readonly byte animType;

		public override RpcCalls RpcType => RpcCalls.PlayAnimation;

		public RpcPlayAnimationMessage(uint rpcObjectNetId, byte animType) : base(rpcObjectNetId)
		{
			this.animType = animType;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(animType);
		}
	}
}
