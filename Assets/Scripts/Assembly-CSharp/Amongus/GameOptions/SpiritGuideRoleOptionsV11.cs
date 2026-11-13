using Hazel;

namespace AmongUs.GameOptions
{
	public class SpiritGuideRoleOptionsV11 : IRoleOptions
	{
		public float SpiritGuideCooldownSeconds = 20f;

		public RoleTypes Type { get; } = RoleTypes.SpiritGuide;

		public void Serialize(MessageWriter writer)
		{
			writer.Write((byte)SpiritGuideCooldownSeconds);
		}

		public void Deserialize(MessageReader reader)
		{
			SpiritGuideCooldownSeconds = (int)reader.ReadByte();
		}

		public void SetRecommended()
		{
			SpiritGuideCooldownSeconds = 20f;
		}

		public SpiritGuideRoleOptionsV11()
		{ }
	}
}
