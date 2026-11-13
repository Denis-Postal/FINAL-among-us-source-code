using Cpp2ILInjected;

public class ReportButton : ActionButton
{
	public void SetActive(bool isActive)
	{
		if (GameManager.Instance == null)
		{
			SetDisabled();
			ToggleVisible(visible: false);
		}
		else if (GameManager.Instance.CanReportBodies())
		{
			if (isActive)
			{
				SetEnabled();
			}
			else
			{
				SetDisabled();
			}
		}
		else
		{
			ToggleVisible(visible: false);
		}
	}

	public override void DoClick()
	{
		if (isActiveAndEnabled && GameManager.Instance.CanReportBodies())
		{
			PlayerControl.LocalPlayer.ReportClosest();
		}
	}
}
