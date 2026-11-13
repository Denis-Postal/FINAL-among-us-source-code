public class MapOptions
{
	public enum Modes
	{
		None,
		Normal,
		CountOverlay,
		Sabotage,
		Detective
	}

	public Modes Mode = Modes.Normal;

	public bool AllowMovementWhileMapOpen;

	public bool ShowLivePlayerPosition = true;

	public bool IncludeDeadBodies= true;
}
