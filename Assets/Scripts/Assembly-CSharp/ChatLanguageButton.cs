using Cpp2ILInjected;
using TMPro;
using UnityEngine;

public class ChatLanguageButton : PoolableBehavior
{
	public TextMeshPro Text;

	public PassiveButton Button;

	[SerializeField]
	private SpriteRenderer Background;

	[SerializeField]
	private SpriteRenderer Check;

	public unsafe void SetSelected(bool selected)
	{
		Color color = (selected ? Color.white : Palette.DisabledGrey);
		GetComponent<ButtonRolloverHandler>().OutColor = color;
		Background.color = color;
		Text.color = color;
		if ((bool)Check)
		{
			Check.enabled = selected;
		}
	}

	public override void Reset()
	{
		base.Reset();
	}
}
