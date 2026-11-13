using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetColorMessage : BaseRpcMessage
	{
		private readonly uint netIdToColor;

		private readonly byte bodyColor;

		public override RpcCalls RpcType => RpcCalls.SetColor;

		public RpcSetColorMessage(uint rpcObjectNetId, uint netIdToColor, byte bodyColor) : base(rpcObjectNetId)
		{
			this.netIdToColor = netIdToColor;
			this.bodyColor = bodyColor;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(netIdToColor);
			msg.Write(bodyColor);
		}
	}
}
