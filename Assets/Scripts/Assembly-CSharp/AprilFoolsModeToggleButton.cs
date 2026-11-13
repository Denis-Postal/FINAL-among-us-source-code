using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class AprilFoolsModeToggleButton : MonoBehaviour
{
	public ButtonRolloverHandler onButton;

	public ButtonRolloverHandler offButton;

	public SpriteRenderer onButtonSprite;

	public SpriteRenderer offButtonSprite;

	public TextMeshPro onText;

	public TextMeshPro offText;

	public void ToggleAprilFoolsMode(bool modeOn)
	{
		AprilFoolsMode.IsAprilFoolsModeToggledOn = modeOn;
	}

	private unsafe void Update()
	{
		if (AprilFoolsMode.IsAprilFoolsModeToggledOn)
		{
			offButton.OutColor = Color.grey;
			offText.color = Color.grey;
			offButtonSprite.color = Color.grey;
			onButton.OutColor = Color.white;
			onText.color = Color.white;
			onButtonSprite.color = Color.green;
		}
		else
		{
			onButton.OutColor = Color.grey;
			onText.color = Color.grey;
			onButtonSprite.color = Color.grey;
			offButton.OutColor = Color.white;
			offText.color = Color.white;
			offButtonSprite.color = Color.green;
		}
	}
}
