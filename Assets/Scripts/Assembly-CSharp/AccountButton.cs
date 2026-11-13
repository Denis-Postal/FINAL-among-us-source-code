using TMPro;
using UnityEngine;

public class AccountButton : PoolableBehavior
{
	public TextMeshPro Text;

	public PassiveButton Button;

	public SpriteRenderer Background;

	public void SetSelected(bool selected)
	{
		ButtonRolloverHandler component = GetComponent<ButtonRolloverHandler>();
		Color outColor = (Background.color = (selected ? Color.white : Color.black));
		component.OutColor = outColor;
	}

	public override void Reset()
	{
		base.Reset();
	}
}
