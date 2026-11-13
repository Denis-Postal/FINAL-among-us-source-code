using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetNameMessage : BaseRpcMessage
	{
		private readonly uint netIdOfNamed;

		private readonly string name;

		public override RpcCalls RpcType => RpcCalls.SetName;

		public RpcSetNameMessage(uint rpcObjectNetId, uint netIdOfNamed, string name) : base(rpcObjectNetId)
		{
			this.netIdOfNamed = netIdOfNamed;
			this.name = name;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(netIdOfNamed);
			msg.Write(name);
		}
	}
}
