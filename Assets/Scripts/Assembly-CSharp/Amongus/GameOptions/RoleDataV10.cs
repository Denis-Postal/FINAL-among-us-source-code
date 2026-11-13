using Cpp2ILInjected;

namespace AmongUs.GameOptions
{
	public class RoleDataV10
	{
		public RoleTypes Type { get; }

		public IRoleOptions RoleOptions { get; }

		public RoleRate Rate { get; set; }

		public RoleDataV10(IRoleOptions roleOptions, RoleRate rate)
		{
			Type = roleOptions.Type;
			RoleOptions = roleOptions;
			Rate = rate;
		}

		public RoleDataV10(RoleTypes type)
		{
			Type = type;
			switch (Type)
			{
			case RoleTypes.Engineer:
				RoleOptions = new EngineerRoleOptionsV10();
				break;
			case RoleTypes.Scientist:
				RoleOptions = new ScientistRoleOptionsV10();
				break;
			case RoleTypes.Shapeshifter:
				RoleOptions = new ShapeshifterRoleOptionsV10();
				break;
			case RoleTypes.GuardianAngel:
				RoleOptions = new GuardianAngelRoleOptionsV10();
				break;
			case RoleTypes.Noisemaker:
				RoleOptions = new NoisemakerRoleOptionsV10();
				break;
			case RoleTypes.Phantom:
				RoleOptions = new PhantomRoleOptionsV10();
				break;
			case RoleTypes.Tracker:
				RoleOptions = new TrackerRoleOptionsV10();
				break;
			case RoleTypes.Viper:
				RoleOptions = new ViperRoleOptionsV10();
				break;
			case RoleTypes.Detective:
				RoleOptions = new DetectiveRoleOptionsV10();
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
