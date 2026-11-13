using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetStartCounterMessage : BaseRpcMessage
	{
		private readonly int counterId;

		private readonly sbyte secondsLeft;

		public override RpcCalls RpcType => RpcCalls.SetStartCounter;

		public RpcSetStartCounterMessage(uint rpcObjectNetId, int counterId, sbyte secondsLeft) : base(rpcObjectNetId)
		{
			this.counterId = counterId;
			this.secondsLeft = secondsLeft;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.WritePacked(counterId);
			msg.Write(secondsLeft);
		}
	}
}
