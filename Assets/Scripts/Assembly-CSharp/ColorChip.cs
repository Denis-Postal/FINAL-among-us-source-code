using Cpp2ILInjected;
using UnityEngine;

public class ColorChip : MonoBehaviour
{
	public HatParent Inner;

	public GameObject PlayerEquippedForeground;

	public GameObject InUseForeground;

	public PassiveButton Button;

	public SpriteRenderer SelectionHighlight;

	public object Tag;

	public readonly Color unavailableColor= new Color(1f, 1f, 1f, 0.5f);

	public string ProductId { get; set; }

	public void ShowSelected()
	{
		SelectionHighlight.enabled = true;
	}

	public void Deselect()
	{
		SelectionHighlight.enabled = false;
	}

	public unsafe void SetUnavailable()
	{
		Inner.FrontLayer.color = unavailableColor;
		Inner.BackLayer.color = unavailableColor;
	}

	public ColorChip()
	{ // TODO(verify): ctor rebuilt from decomp9 by tools/ctorfix.py
		// TODO(verify): lost struct initializer (decomp: unavailableColor = (Color)0;)
	}
}
