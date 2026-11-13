public class VentButton : ActionButton
{
	private Vent currentTarget;

	public void SetTarget(Vent target)
	{
		currentTarget = target;
		if (target != null)
		{
			SetEnabled();
		}
		else
		{
			SetDisabled();
		}
	}

	public override void DoClick()
	{
		currentTarget?.Use();
	}
}
