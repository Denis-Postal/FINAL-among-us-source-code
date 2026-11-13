using Cpp2ILInjected;

namespace AmongUs.GameOptions
{
	public class RoleDataV12
	{
		public RoleTypes Type { get; }

		public IRoleOptions RoleOptions { get; }

		public RoleRate Rate { get; set; }

		public RoleDataV12(IRoleOptions roleOptions, RoleRate rate)
		{
			Type = roleOptions.Type;
			RoleOptions = roleOptions;
			Rate = rate;
		}

		public RoleDataV12(RoleTypes type)
		{
			Type = type;
			switch (Type)
			{
			case RoleTypes.Engineer:
				RoleOptions = new EngineerRoleOptionsV12();
				break;
			case RoleTypes.Scientist:
				RoleOptions = new ScientistRoleOptionsV12();
				break;
			case RoleTypes.Shapeshifter:
				RoleOptions = new ShapeshifterRoleOptionsV12();
				break;
			case RoleTypes.GuardianAngel:
				RoleOptions = new GuardianAngelRoleOptionsV12();
				break;
			case RoleTypes.Noisemaker:
				RoleOptions = new NoisemakerRoleOptionsV12();
				break;
			case RoleTypes.Phantom:
				RoleOptions = new PhantomRoleOptionsV12();
				break;
			case RoleTypes.Tracker:
				RoleOptions = new TrackerRoleOptionsV12();
				break;
			case RoleTypes.Viper:
				RoleOptions = new ViperRoleOptionsV12();
				break;
			case RoleTypes.Detective:
				RoleOptions = new DetectiveRoleOptionsV12();
				break;
			case RoleTypes.Judge:
				RoleOptions = new JudgeRoleOptionsV12();
				break;
			case RoleTypes.SpiritGuide:
				RoleOptions = new SpiritGuideRoleOptionsV12();
				break;
			case RoleTypes.CrewmateGhost:
			case RoleTypes.ImpostorGhost:
			case (RoleTypes)11:
			case (RoleTypes)13:
			case (RoleTypes)14:
			case (RoleTypes)15:
			case (RoleTypes)16:
			case (RoleTypes)17:
			case (RoleTypes)20:
				break;
			}
		}
	}
}
