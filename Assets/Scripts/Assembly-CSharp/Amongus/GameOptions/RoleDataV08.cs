using Cpp2ILInjected;

namespace AmongUs.GameOptions
{
	public class RoleDataV08
	{
		public RoleTypes Type { get; }

		public IRoleOptions RoleOptions { get; }

		public RoleRate Rate { get; set; }

		public RoleDataV08(IRoleOptions roleOptions, RoleRate rate)
		{
			Type = roleOptions.Type;
			RoleOptions = roleOptions;
			Rate = rate;
		}

		public RoleDataV08(RoleTypes type)
		{
			Type = type;
			switch (Type)
			{
			case RoleTypes.Engineer:
				RoleOptions = new EngineerRoleOptionsV08();
				break;
			case RoleTypes.Scientist:
				RoleOptions = new ScientistRoleOptionsV08();
				break;
			case RoleTypes.Shapeshifter:
				RoleOptions = new ShapeshifterRoleOptionsV08();
				break;
			case RoleTypes.GuardianAngel:
				RoleOptions = new GuardianAngelRoleOptionsV08();
				break;
			case RoleTypes.Noisemaker:
				RoleOptions = new NoisemakerRoleOptionsV08();
				break;
			case RoleTypes.Phantom:
				RoleOptions = new PhantomRoleOptionsV08();
				break;
			case RoleTypes.Tracker:
				RoleOptions = new TrackerRoleOptionsV08();
				break;
			case RoleTypes.CrewmateGhost:
			case RoleTypes.ImpostorGhost:
				break;
			}
		}
	}
}
