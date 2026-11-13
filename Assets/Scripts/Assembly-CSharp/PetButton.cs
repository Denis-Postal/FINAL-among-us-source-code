public class PetButton : ActionButton
{
	public void SetTarget()
	{
		SetEnabled();
	}

	public override void DoClick()
	{
		if (isActiveAndEnabled && canInteract && (bool)PlayerControl.LocalPlayer)
		{
			PlayerControl.LocalPlayer.TryPet();
		}
	}
}
