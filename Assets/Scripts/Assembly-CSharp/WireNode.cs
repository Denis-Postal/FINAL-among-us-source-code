using System;
using Cpp2ILInjected;
using UnityEngine;

public class WireNode : MonoBehaviour
{
	public Collider2D hitbox;

	public SpriteRenderer[] WireColors;

	public SpriteRenderer BaseSymbol;

	public sbyte WireId;

	internal unsafe void SetColor(Color color, Sprite symbol)
	{
		BaseSymbol.sprite = symbol;
		for (int i = 0; i < WireColors.Length; i++)
		{
			WireColors[i].color = color;
		}
	}
}
