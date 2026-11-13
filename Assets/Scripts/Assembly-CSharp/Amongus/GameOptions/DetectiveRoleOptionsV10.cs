using Hazel;

namespace AmongUs.GameOptions
{
	public class DetectiveRoleOptionsV10 : IRoleOptions
	{
		public float DetectiveSuspectLimit = 3f;

		public RoleTypes Type { get; } = RoleTypes.Detective;

		public void Serialize(MessageWriter writer)
		{
			writer.Write((byte)DetectiveSuspectLimit);
		}

		public void Deserialize(MessageReader reader)
		{
			DetectiveSuspectLimit = (int)reader.ReadByte();
		}

		public void SetRecommended()
		{
			DetectiveSuspectLimit = 3f;
		}

		public DetectiveRoleOptionsV10()
		{ }
	}
}
