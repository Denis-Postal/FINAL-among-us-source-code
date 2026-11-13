using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetLevelMessage : BaseRpcMessage
	{
		private readonly uint level;

		public override RpcCalls RpcType => RpcCalls.SetLevel;

		public RpcSetLevelMessage(uint rpcObjectNetId, uint level) : base(rpcObjectNetId)
		{
			this.level = level;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.WritePacked(level);
		}
	}
}
