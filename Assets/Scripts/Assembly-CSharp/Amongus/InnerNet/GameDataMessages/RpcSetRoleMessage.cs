using AmongUs.GameOptions;
using Hazel;

namespace AmongUs.InnerNet.GameDataMessages
{
	public class RpcSetRoleMessage : BaseRpcMessage
	{
		private readonly RoleTypes roleType;

		private readonly bool canOverrideRole;

		public override RpcCalls RpcType => RpcCalls.SetRole;

		public RpcSetRoleMessage(uint rpcObjectNetId, RoleTypes roleType, bool canOverrideRole) : base(rpcObjectNetId)
		{
			this.roleType = roleType;
			this.canOverrideRole = canOverrideRole;
		}

		protected override void SerializeRpcValues(MessageWriter msg)
		{
			msg.Write((ushort)roleType);
			msg.Write(canOverrideRole);
		}
	}
}
