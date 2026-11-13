using Hazel;

namespace AmongUs.GameOptions
{
	public class ViperRoleOptionsV10 : IRoleOptions
	{
		public float ViperDissolveTime = 15f;

		public RoleTypes Type { get; } = RoleTypes.Viper;

		public void Serialize(MessageWriter writer)
		{
			writer.Write((byte)ViperDissolveTime);
		}

		public void Deserialize(MessageReader reader)
		{
			ViperDissolveTime = (int)reader.ReadByte();
		}

		public void SetRecommended()
		{
			ViperDissolveTime = 15f;
		}

		public ViperRoleOptionsV10()
		{ }
	}
}
