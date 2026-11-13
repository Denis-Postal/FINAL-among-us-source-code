using Object = UnityEngine.Object;
using Cpp2ILInjected;
using UnityEngine;

public class MapFilterButton : MonoBehaviour
{
	public MapNames MapId;

	public PassiveButton Button;

	[SerializeField]
	private SpriteRenderer Icon;

	[SerializeField]
	private SpriteRenderer ButtonCheck;

	[SerializeField]
	private SpriteRenderer ButtonOutline;

	[SerializeField]
	private SpriteRenderer ButtonImage;

	public void Show()
	{
		Icon.gameObject.SetActive(value: true);
		Button.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		Icon.gameObject.SetActive(value: false);
		Button.gameObject.SetActive(value: false);
	}

	public void SetFlipped(bool flipped)
	{
		Icon.flipX = flipped;
		ButtonImage.flipX = flipped;
	}

	public unsafe void SetSelected(bool selected)
	{
		Color color = (selected ? Color.white : Palette.DisabledGrey);
		Icon.color = color;
		ButtonImage.color = color;
		ButtonOutline.color = color;
		ButtonOutline.GetComponent<ButtonRolloverHandler>().OutColor = color;
		ButtonCheck.enabled = selected;
	}
}
