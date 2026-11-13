using Hazel;

namespace AmongUs.GameOptions
{
	public class PhantomRoleOptionsV10 : IRoleOptions
	{
		public float PhantomCooldown = 15f;

		public float PhantomDuration = 30f;

		public RoleTypes Type { get; } = RoleTypes.Phantom;

		public void Serialize(MessageWriter writer)
		{
			writer.Write((byte)PhantomCooldown);
			writer.Write((byte)PhantomDuration);
		}

		public void Deserialize(MessageReader reader)
		{
			PhantomCooldown = (int)reader.ReadByte();
			PhantomDuration = (int)reader.ReadByte();
		}

		public void SetRecommended()
		{
			PhantomCooldown = 15f;
			PhantomDuration = 30f;
		}

		public PhantomRoleOptionsV10()
		{ }
	}
}
