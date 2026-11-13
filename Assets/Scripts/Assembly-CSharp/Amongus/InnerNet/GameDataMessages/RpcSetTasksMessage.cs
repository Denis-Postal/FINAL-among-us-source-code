using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetTasksMessage : BaseRpcMessage
	{
		private readonly byte[] taskTypeIds;

		public override RpcCalls RpcType => RpcCalls.SetTasks;

		public RpcSetTasksMessage(uint rpcObjectNetId, byte[] taskTypeIds) : base(rpcObjectNetId)
		{
			this.taskTypeIds = taskTypeIds;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.WriteBytesAndSize(taskTypeIds);
		}
	}
}
