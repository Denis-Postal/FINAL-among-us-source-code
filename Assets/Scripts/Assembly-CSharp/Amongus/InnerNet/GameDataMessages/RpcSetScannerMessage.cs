using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetScannerMessage : BaseRpcMessage
	{
		private readonly bool value;

		private readonly byte count;

		public override RpcCalls RpcType => RpcCalls.SetScanner;

		public RpcSetScannerMessage(uint rpcObjectNetId, bool value, byte count) : base(rpcObjectNetId)
		{
			this.value = value;
			this.count = count;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(value);
			msg.Write(count);
		}
	}
}
