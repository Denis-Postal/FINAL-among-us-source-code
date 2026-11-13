using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcExitVentMessage : BaseRpcMessage
	{
		private readonly int ventId;

		public override RpcCalls RpcType => RpcCalls.ExitVent;

		public RpcExitVentMessage(uint rpcObjectNetId, int ventId) : base(rpcObjectNetId)
		{
			this.ventId = ventId;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.WritePacked(ventId);
		}
	}
}
