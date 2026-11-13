using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcCompleteTaskMessage : BaseRpcMessage
	{
		private readonly uint idx;

		public override RpcCalls RpcType => RpcCalls.CompleteTask;

		public RpcCompleteTaskMessage(uint rpcObjectNetId, uint idx) : base(rpcObjectNetId)
		{
			this.idx = idx;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.WritePacked(idx);
		}
	}
}
