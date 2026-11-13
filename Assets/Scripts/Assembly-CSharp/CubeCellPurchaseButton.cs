using TMPro;
using UnityEngine;

public class CubeCellPurchaseButton : MonoBehaviour
{
	public PassiveButton button;

	public TextMeshPro text;

	public SpriteRenderer podIcon;

	public SpriteRenderer buttonBackground;

	public void SetUseable()
	{
		button.enabled = true;
		text.color = Color.white;
		buttonBackground.color = Color.white;
	}

	public void SetInactive()
	{
		button.enabled = false;
		text.color = Color.red;
		buttonBackground.color = Palette.DisabledGrey;
	}
}
