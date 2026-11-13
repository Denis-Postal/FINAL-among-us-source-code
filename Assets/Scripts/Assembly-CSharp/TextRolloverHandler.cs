using Cpp2ILInjected;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextRolloverHandler : MonoBehaviour
{
	public TextMeshPro Target;

	public Color OverColor= Color.green;

	public Color OverOutlineColor= Color.white;

	public Color OutColor= Color.white;

	public Color OutOutlineColor= Color.white;

	public void Start()
	{
		PassiveButton component = GetComponent<PassiveButton>();
		component.OnMouseOver.AddListener(DoMouseOver);
		component.OnMouseOut.AddListener(DoMouseOut);
	}

	public unsafe void DoMouseOver()
	{
		Target.color = OverColor;
		Target.outlineColor = OverOutlineColor;
	}

	public unsafe void DoMouseOut()
	{
		Target.color = OutColor;
		Target.outlineColor = OutOutlineColor;
	}

	public TextRolloverHandler()
	{ }
}
