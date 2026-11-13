using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcAddVoteBanMessage : BaseRpcMessage
	{
		private readonly int sourceClientId;

		private readonly int clientIdToVoteBan;

		public override RpcCalls RpcType => RpcCalls.AddVote;

		public RpcAddVoteBanMessage(uint rpcObjectNetId, int sourceClientId, int clientIdToVoteBan) : base(rpcObjectNetId)
		{
			this.sourceClientId = sourceClientId;
			this.clientIdToVoteBan = clientIdToVoteBan;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write(sourceClientId);
			msg.Write(clientIdToVoteBan);
		}
	}
}
