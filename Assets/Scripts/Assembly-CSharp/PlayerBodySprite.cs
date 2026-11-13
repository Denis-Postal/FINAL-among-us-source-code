using UnityEngine.Serialization;
using System;
using UnityEngine;

[Serializable]
public class PlayerBodySprite
{
	public SpriteRenderer BodySprite;

	[FormerlySerializedAs("extraParts")]
	public SpriteRenderer[] LongModeParts;

	public PlayerPettingHand PettingHand;

	public SpriteRenderer HandHat;

	public Sprite GhostSprite;

	public Sprite[] ExtraGhostSprites;

	public PlayerBodyTypes Type;

	public Vector3 flippedCosmeticOffset;

	public Vector3 normalCosmeticOffset;

	public bool Visible
	{
		get
		{
			return BodySprite.enabled;
		}
		set
		{
			BodySprite.enabled = value;
			SpriteRenderer[] longModeParts = LongModeParts;
			for (int i = 0; i < longModeParts.Length; i++)
			{
				longModeParts[i].enabled = value;
			}
		}
	}

	public void SetFlipX(bool flipX)
	{
		BodySprite.flipX = flipX;
		SpriteRenderer[] longModeParts = LongModeParts;
		for (int i = 0; i < longModeParts.Length; i++)
		{
			longModeParts[i].flipX = flipX;
		}
	}
}
