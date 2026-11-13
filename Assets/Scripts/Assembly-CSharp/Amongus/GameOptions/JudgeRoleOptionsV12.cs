using Hazel;

namespace AmongUs.GameOptions
{
	public class JudgeRoleOptionsV12 : IRoleOptions
	{
		public float JudgeTaskRequirementPercentage = 50f;

		public RoleTypes Type { get; } = RoleTypes.Judge;

		public void Serialize(MessageWriter writer)
		{
			writer.Write((byte)JudgeTaskRequirementPercentage);
		}

		public void Deserialize(MessageReader reader)
		{
			JudgeTaskRequirementPercentage = (int)reader.ReadByte();
		}

		public void SetRecommended()
		{
			JudgeTaskRequirementPercentage = 50f;
		}

		public JudgeRoleOptionsV12()
		{ }
	}
}
