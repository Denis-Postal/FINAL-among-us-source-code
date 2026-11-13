using Hazel;
using UnityEngine;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcPetMessage : BaseRpcMessage
	{
		private readonly Vector2 pos;

		private readonly Vector2 petPos;

		public override RpcCalls RpcType => RpcCalls.Pet;

		public RpcPetMessage(uint rpcObjectNetId, Vector2 pos, Vector2 petPos) : base(rpcObjectNetId)
		{
			this.pos = pos;
			this.petPos = petPos;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			NetHelpers.WriteVector2(pos, msg);
			NetHelpers.WriteVector2(petPos, msg);
		}
	}
}
