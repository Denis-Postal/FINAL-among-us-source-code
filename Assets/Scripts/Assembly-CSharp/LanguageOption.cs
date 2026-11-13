public class LanguageOption : PoolableBehavior
{
	public PassiveButton Button;

	public void SetTextButton(string text)
	{
		Button.ChangeButtonText(text);
	}

	public void SetButtonSelected(bool selected)
	{
		Button.SelectButton(selected);
	}
}
