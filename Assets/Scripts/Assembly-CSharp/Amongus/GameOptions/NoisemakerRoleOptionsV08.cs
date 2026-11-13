using Hazel;

namespace AmongUs.GameOptions
{
	public class NoisemakerRoleOptionsV08 : IRoleOptions
	{
		public bool NoisemakerImpostorAlert = true;

		public float NoisemakerAlertDuration = 10f;

		public RoleTypes Type { get; } = RoleTypes.Noisemaker;

		public void Serialize(MessageWriter writer)
		{
			writer.Write((byte)NoisemakerAlertDuration);
			writer.Write(NoisemakerImpostorAlert);
		}

		public void Deserialize(MessageReader reader)
		{
			NoisemakerAlertDuration = (int)reader.ReadByte();
			NoisemakerImpostorAlert = reader.ReadBoolean();
		}

		public void SetRecommended()
		{
			NoisemakerAlertDuration = 10f;
			NoisemakerImpostorAlert = true;
		}

		public NoisemakerRoleOptionsV08()
		{ }
	}
}
