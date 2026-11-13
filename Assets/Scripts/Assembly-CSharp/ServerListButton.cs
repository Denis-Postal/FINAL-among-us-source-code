using TMPro;
using UnityEngine;

public class ServerListButton : PoolableBehavior
{
	public TextMeshPro Text;

	public PassiveButton Button;

	public SpriteRenderer Background;

	public TextTranslatorTMP textTranslator;

	public unsafe void SetSelected(bool selected)
	{
		if ((bool)Background)
		{
			ButtonRolloverHandler component = GetComponent<ButtonRolloverHandler>();
			Color outColor = (Background.color = (selected ? Color.white : Color.black));
			component.OutColor = outColor;
		}
	}

	public void SetTextTranslationId(StringNames id, string defaultStr)
	{
		textTranslator.TargetText = id;
		textTranslator.defaultStr = defaultStr;
	}
}
