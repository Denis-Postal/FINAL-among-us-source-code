using System;

public static class AprilFoolsMode
{
	public static bool IsAprilFoolsModeToggledOn;

	internal static bool ShouldFlipSkeld()
	{
		return false;
	}

	public static bool ShouldHorseAround()
	{
		if (ShouldShowAprilFoolsToggle())
		{
			return IsAprilFoolsModeToggledOn;
		}
		return false;
	}

	public static bool ShouldShowAprilFoolsToggle()
	{
		// TODO(verify): reimplemented (decomp stub)
		// TODO(verify): the LimitedTime window comes from ReferenceData.AprilFoolLimitedTime (lost in decompile); offline there is no server timestamp, so false
		EOSManager eos = DestroyableSingleton<EOSManager>.Instance;
		if (eos == null || !eos.HasServerTimestamp) return false;
		ReferenceDataManager rdm = DestroyableSingleton<ReferenceDataManager>.Instance;
		if (rdm == null || rdm.Refdata == null) return false;
		DateTime now = eos.ApproximateServerTime;
		LimitedTimeStartEnd w = rdm.Refdata.AprilFoolLimitedTime;
		return w.timeStart.IsAfterThisTime(now) && w.timeEnd.IsBeforeThisTime(now);
	}

	public static bool ShouldLongAround()
	{
		return false;
	}

	public static bool ShouldClassicMode()
	{
		return false;
	}

	public static bool ShouldClassicMainMenuMode()
	{
		return false;
	}

	public static bool ShouldHorseModeMainMenu()
	{
		return ShouldShowAprilFoolsToggle();
	}

	public static bool IsImpostorMonth()
	{
		// TODO(verify): reimplemented (decomp stub)
		EOSManager eos = DestroyableSingleton<EOSManager>.Instance;
		if (eos == null || !eos.HasServerTimestamp) return false;
		return eos.ApproximateServerTime.Month == 4; // TODO(verify): month constant (decomp "month - 4 == 0")
	}

	public static StringNames GetOptionsToggleStringName()
	{
		if (IsImpostorMonth())
		{
			return StringNames.HorseModeToggle;
		}
		return StringNames.AprilFoolsMode;
	}
}
