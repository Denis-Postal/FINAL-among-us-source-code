using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public abstract class BaseRpcMessage : BaseGameDataMessage
	{
		public readonly uint rpcObjectNetId;

		public sealed override GameDataTypes GameDataType { get; } = GameDataTypes.RpcFlag;

		public abstract RpcCalls RpcType { get; }

		public BaseRpcMessage(uint rpcObjectNetId)
		{
			this.rpcObjectNetId = rpcObjectNetId;
		}

		protected sealed override void SerializeValues(MessageWriter msg)
		{
			msg.WritePacked(rpcObjectNetId);
			msg.Write((byte)RpcType);
			SerializeRpcValues(msg);
		}

		protected abstract void SerializeRpcValues(MessageWriter msg);
	}
}
