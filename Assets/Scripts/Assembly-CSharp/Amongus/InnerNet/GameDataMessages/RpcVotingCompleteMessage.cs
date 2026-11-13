using Cpp2ILInjected;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcVotingCompleteMessage : BaseRpcMessage
	{
		private readonly MeetingHud.VoterState[] voterStates;

		private readonly byte exiledPlayerId;

		private readonly bool tie;

		private readonly bool wasOverruled;

		private readonly ushort overrideId;

		public override RpcCalls RpcType => RpcCalls.VotingComplete;

		public RpcVotingCompleteMessage(uint rpcObjectNetId, MeetingHud.VoterState[] voterStates, byte exiledPlayerId, bool tie, bool wasOverruled, ushort overrideId) : base(rpcObjectNetId)
		{
			this.voterStates = voterStates;
			this.exiledPlayerId = exiledPlayerId;
			this.tie = tie;
			this.wasOverruled = wasOverruled;
			this.overrideId = overrideId;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.WritePacked(voterStates.Length);
			MeetingHud.VoterState[] array = voterStates;
			foreach (MeetingHud.VoterState voterState in array)
			{
				voterState.Serialize(msg);
			}
			msg.Write(exiledPlayerId);
			msg.Write(tie);
			msg.Write(wasOverruled);
			msg.Write(overrideId);
		}
	}
}
