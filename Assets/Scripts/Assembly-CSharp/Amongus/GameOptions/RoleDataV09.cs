using Cpp2ILInjected;

namespace AmongUs.GameOptions
{
	public class RoleDataV09
	{
		public RoleTypes Type { get; }

		public IRoleOptions RoleOptions { get; }

		public RoleRate Rate { get; set; }

		public RoleDataV09(IRoleOptions roleOptions, RoleRate rate)
		{
			Type = roleOptions.Type;
			RoleOptions = roleOptions;
			Rate = rate;
		}

		public RoleDataV09(RoleTypes type)
		{
			Type = type;
			switch (Type)
			{
			case RoleTypes.Engineer:
				RoleOptions = new EngineerRoleOptionsV09();
				break;
			case RoleTypes.Scientist:
				RoleOptions = new ScientistRoleOptionsV09();
				break;
			case RoleTypes.Shapeshifter:
				RoleOptions = new ShapeshifterRoleOptionsV09();
				break;
			case RoleTypes.GuardianAngel:
				RoleOptions = new GuardianAngelRoleOptionsV09();
				break;
			case RoleTypes.Noisemaker:
				RoleOptions = new NoisemakerRoleOptionsV09();
				break;
			case RoleTypes.Phantom:
				RoleOptions = new PhantomRoleOptionsV09();
				break;
			case RoleTypes.Tracker:
				RoleOptions = new TrackerRoleOptionsV09();
				break;
			case RoleTypes.CrewmateGhost:
			case RoleTypes.ImpostorGhost:
				break;
			}
		}
	}
}
