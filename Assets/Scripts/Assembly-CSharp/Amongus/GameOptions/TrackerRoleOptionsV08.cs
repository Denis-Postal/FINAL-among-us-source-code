using Hazel;

namespace AmongUs.GameOptions
{
	public class TrackerRoleOptionsV08 : IRoleOptions
	{
		public float TrackerCooldown = 15f;

		public float TrackerDuration = 30f;

		public float TrackerDelay = 1f;

		public RoleTypes Type { get; } = RoleTypes.Tracker;

		public void Serialize(MessageWriter writer)
		{
			writer.Write((byte)TrackerCooldown);
			writer.Write((byte)TrackerDuration);
			writer.Write((byte)TrackerDelay);
		}

		public void Deserialize(MessageReader reader)
		{
			TrackerCooldown = (int)reader.ReadByte();
			TrackerDuration = (int)reader.ReadByte();
			TrackerDelay = (int)reader.ReadByte();
		}

		public void SetRecommended()
		{
			TrackerCooldown = 15f;
			TrackerDuration = 30f;
			TrackerDelay = 1f;
		}

		public TrackerRoleOptionsV08()
		{ }
	}
}
