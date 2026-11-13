using Cpp2ILInjected;

namespace AmongUs.GameOptions
{
	public class RoleDataV11
	{
		public RoleTypes Type { get; }

		public IRoleOptions RoleOptions { get; }

		public RoleRate Rate { get; set; }

		public RoleDataV11(IRoleOptions roleOptions, RoleRate rate)
		{
			Type = roleOptions.Type;
			RoleOptions = roleOptions;
			Rate = rate;
		}

		public RoleDataV11(RoleTypes type)
		{
			Type = type;
			switch (Type)
			{
			case RoleTypes.Engineer:
				RoleOptions = new EngineerRoleOptionsV11();
				break;
			case RoleTypes.Scientist:
				RoleOptions = new ScientistRoleOptionsV11();
				break;
			case RoleTypes.Shapeshifter:
				RoleOptions = new ShapeshifterRoleOptionsV11();
				break;
			case RoleTypes.GuardianAngel:
				RoleOptions = new GuardianAngelRoleOptionsV11();
				break;
			case RoleTypes.Noisemaker:
				RoleOptions = new NoisemakerRoleOptionsV11();
				break;
			case RoleTypes.Phantom:
				RoleOptions = new PhantomRoleOptionsV11();
				break;
			case RoleTypes.Tracker:
				RoleOptions = new TrackerRoleOptionsV11();
				break;
			case RoleTypes.Viper:
				RoleOptions = new ViperRoleOptionsV11();
				break;
			case RoleTypes.Detective:
				RoleOptions = new DetectiveRoleOptionsV11();
				break;
			case RoleTypes.Judge:
				RoleOptions = new JudgeRoleOptionsV11();
				break;
			case RoleTypes.CrewmateGhost:
			case RoleTypes.ImpostorGhost:
			case (RoleTypes)11:
			case (RoleTypes)13:
			case (RoleTypes)14:
			case (RoleTypes)15:
			case (RoleTypes)16:
			case (RoleTypes)17:
				break;
			}
		}
	}
}
