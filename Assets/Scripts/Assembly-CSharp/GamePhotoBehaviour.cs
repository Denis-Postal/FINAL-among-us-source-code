using System.Collections;
using UnityEngine;

public class GamePhotoBehaviour : MonoBehaviour
{
	public static readonly Color InWaterPink = Color.Lerp(Color.white, new Color32(211, 106, 129, byte.MaxValue), 0.6f);

	public float zOffset;

	public SpriteRenderer Frame;

	public SpriteRenderer Image;

	public Collider2D Hitbox;

	public Color TargetColor = Palette.ClearWhite;

	public void Start()
	{
		transform.SetLocalZ(1f + zOffset);
	}

	internal IEnumerator Pickup()
	{
		yield return Effects.All(Effects.Lerp(0.3f, (float t) =>
		{
			transform.SetLocalZ(1f - t + zOffset);
		}), Effects.ScaleIn(transform, 1f, 1.2f, 0.3f), Effects.Sequence(Effects.Wait(0.15f), Effects.Action(() =>
		{
			TargetColor = Color.white;
		})));
	}

	internal IEnumerator Drop(bool inWater)
	{
		yield return Effects.All(Effects.Lerp(0.3f, (float t) =>
		{
			transform.SetLocalZ(t + zOffset);
		}), Effects.ScaleIn(transform, 1.2f, 1f, 0.3f), Effects.Sequence(Effects.Wait(0.15f), Effects.Action(() =>
		{
			if (inWater)
			{
				TargetColor = InWaterPink;
			}
		})));
	}
}
